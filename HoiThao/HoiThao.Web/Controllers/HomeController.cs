using AutoMapper;
using HoiThao.Web.Common;
using HoiThao.Web.Data.Models;
using HoiThao.Web.Infrastructure.Core;
using HoiThao.Web.Infrastructure.Extensions;
using HoiThao.Web.Models;
using HoiThao.Web.Service;
using LinqToExcel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HoiThao.Web.Controllers
{
    public class HomeController : BaseController
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
        OleDbConnection Econ;

        private IaseanService _aseanService;

        public HomeController(IaseanService aseanService)
        {
            _aseanService = aseanService;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult LoadData(string name, string status, int page, int pageSize)
        {
            int totalRow = 0;

            var listAccount = _aseanService.Search(name, page, pageSize, status, out totalRow);
            //var query = listuser.OrderBy(x => x.tenhd);
            var responseData = Mapper.Map<IEnumerable<asean>, IEnumerable<aseanViewModel>>(listAccount);

            return Json(new
            {
                data = responseData,
                total = totalRow,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLastId()
        {
            bool status = false;
            string message = "";
            var aseanId = _aseanService.GetLastId(ref status, ref message);
            return Json(new
            {
                status = status,
                data = aseanId,
                message = message
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNextId(string id)
        {
            bool status = false;
            string message = "";
            var userId = _aseanService.GetNextId(id, ref status, ref message);
            return Json(new
            {
                status = status,
                data = userId,
                message = message
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveData(string strAsean, int Hidid)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var aseanVM = serializer.Deserialize<aseanViewModel>(strAsean);

            var aseanM = new asean();
            aseanM.Updateasean(aseanVM);

            bool status = true;
            string message = string.Empty;

            //var aseanExist = _aseanService.GetByUsername(user.username);

            if (Hidid == 0)//dang them
            {


                try
                {
                    _aseanService.add(aseanM);
                    _aseanService.save();

                    status = true;
                    message = "Add new success.";

                }
                catch (Exception ex)
                {
                    throw ex;

                    status = false;
                    message = ex.Message;
                }



            }
            else if (Hidid != 0)
            {
                aseanM.k = Hidid;
                try
                {
                    _aseanService.UpdateAsean(aseanM);
                    _aseanService.save();
                    message = "Update success.";

                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    status = false;
                    throw ex;
                }



            }
            return Json(new
            {
                status = status,
                message = message
            });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            bool status = true;
            string message = string.Empty;
            try
            {
                _aseanService.Delete(id);
                _aseanService.save();
            }
            catch (Exception ex)
            {
                status = false;
                message = ex.Message;
            }

            return Json(new
            {
                status = status,
                message = message
            });
        }

        [HttpPost]
        public JsonResult UpdateCheckin(string strAsean)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var aseanVM = serializer.Deserialize<aseanViewModel>(strAsean);

            var aseanM = new asean();
            aseanM.Updateasean(aseanVM);

            string message = string.Empty;


            try
            {
                _aseanService.UpdateCheckin(aseanM);
                return Json(new
                {
                    status = true,
                    message = "Checkin" +
                    " success."
                });

            }
            catch (Exception ex)
            {
                throw ex;
                return Json(new
                {
                    status = false,
                    message = "Checkin failure."
                });
            }
        }

        [HttpGet]
        public JsonResult GetDetail(int id)
        {
            bool statusAsean = false;
            var aseanM = _aseanService.GetById(id);
            var aseanViewModel = Mapper.Map<asean, aseanViewModel>(aseanM);
            if (aseanViewModel != null)
            {
                statusAsean = true;
            }
            return Json(new
            {
                data = aseanViewModel,
                status = statusAsean
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllHotel()
        {
            bool statusAsean = false;
            var aseanM = _aseanService.GetAllHotel();

            var settings = new JsonSerializerSettings() { ContractResolver = new NullToEmptyStringResolver() };
            var str = JsonConvert.SerializeObject(aseanM, settings);

            if (aseanM != null)
            {
                statusAsean = true;
            }
            return Json(new
            {
                data = str,
                status = statusAsean
            }, JsonRequestBehavior.AllowGet);
        }

        public FileResult DownloadExcel()
        {
            string path = "/Doc/Book3.xlsx";
            return File(path, "application/vnd.ms-excel", "Book3.xlsx");
        }
        [HttpPost]
        public JsonResult UploadExcel()
        {
            var pic = System.Web.HttpContext.Current.Request.Files["HelpSectionImages"];
            HttpPostedFileBase file = new HttpPostedFileWrapper(pic);

            string filename = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string filepath = "/excelfolder/" + filename;
            file.SaveAs(Path.Combine(Server.MapPath("/excelfolder"), filename));
            try
            {
                InsertExcelData(filepath, filename);
                return Json(new
                {
                    status = true,
                    message = "Import success"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw ex;
                return Json(new
                {
                    status = true,
                    message = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
            //return View();
        }
        private void ExcelConn(string filepath)
        {
            string constr = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0; Data Source={0}; Extended Properties=""Excel 12.0 xml; HDR=yes;""", filepath);
            Econ = new OleDbConnection(constr);
        }
        private void InsertExcelData(string filepath, string filename)
        {
            string fullpath = Server.MapPath("/excelfolder/") + filename;
            ExcelConn(fullpath);
            string query = string.Format("SELECT * FROM [{0}]", "Sheet1$");
            OleDbCommand Ecom = new OleDbCommand(query, Econ);
            Econ.Open();
            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(query, Econ);
            oda.Fill(ds);
            DataTable dt = ds.Tables[0];
            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            objbulk.DestinationTableName = "asean";
            //objbulk.ColumnMappings.Add("UserId", "UserId");
            objbulk.ColumnMappings.Add("title", "title");
            objbulk.ColumnMappings.Add("id", "id");
            objbulk.ColumnMappings.Add("firstname", "firstname");
            objbulk.ColumnMappings.Add("country", "country");
            objbulk.ColumnMappings.Add("company", "company");

            objbulk.ColumnMappings.Add("Note1", "Hotel");
            objbulk.ColumnMappings.Add("cibongsen", "HotelCheckin");
            objbulk.ColumnMappings.Add("cobongsen", "HotelCheckout");
            objbulk.ColumnMappings.Add("KS", "HotelPrice");
            //objbulk.ColumnMappings.Add("HotelBookingInf", "HotelBookingInf");

            objbulk.ColumnMappings.Add("Amount", "amount");
            objbulk.ColumnMappings.Add("Bank fee", "bankfee");

            objbulk.ColumnMappings.Add("Total", "grandtotal");
            objbulk.ColumnMappings.Add("Payment Method", "mop");
            objbulk.ColumnMappings.Add("Note 2", "dfno");
            objbulk.ColumnMappings.Add("Note", "note");

            objbulk.ColumnMappings.Add("company", "email");
            objbulk.ColumnMappings.Add("Payment Status", "payment");
            objbulk.ColumnMappings.Add("At", "at");
            objbulk.ColumnMappings.Add("Dt1", "dt");

            con.Open();
            objbulk.WriteToServer(dt);
            con.Close();
            Econ.Close();
            GC.Collect();
            if (System.IO.File.Exists(fullpath))
                System.IO.File.Delete(fullpath);
        }


    }
}