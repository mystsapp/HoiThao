using HoiThao.Web.Data.Models;
using HoiThao.Web.Infrastructure.Core;
using HoiThao.Web.Service;
using LinqToExcel;
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

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
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
                throw ex;
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
            objbulk.ColumnMappings.Add("Amount", "amount");
            objbulk.ColumnMappings.Add("Bank fee", "bankfee");

            objbulk.ColumnMappings.Add("Total", "grandtotal");
            objbulk.ColumnMappings.Add("Payment Method", "mop");
            objbulk.ColumnMappings.Add("Note 2", "dfno");
            objbulk.ColumnMappings.Add("company", "email");
            objbulk.ColumnMappings.Add("Payment Status", "payment");
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