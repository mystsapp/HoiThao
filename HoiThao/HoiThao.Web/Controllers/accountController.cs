using AutoMapper;
using HoiThao.Web.Data.Models;
using HoiThao.Web.Infrastructure.Core;
using HoiThao.Web.Infrastructure.Extensions;
using HoiThao.Web.Models;
using HoiThao.Web.Service;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HoiThao.Web.Controllers
{
    public class accountController : BaseController
    {
        private IaccountService _accountService;
        private ICommonService _commonService;

        public accountController(IaccountService accountService, ICommonService commonService)
        {
            _accountService = accountService;
            _commonService = commonService;
        }

        // GET: account
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult LoadData(string name, string status, int page, int pageSize)
        {
            int totalRow = 0;

            var listAccount = _accountService.Search(name, page, pageSize, status, out totalRow);
            //var query = listuser.OrderBy(x => x.tenhd);
            var responseData = Mapper.Map<IEnumerable<account>, IEnumerable<accountViewModel>>(listAccount);

            return Json(new
            {
                data = responseData,
                total = totalRow,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDetail(int id)
        {
            bool statusUser = false;
            var user = _accountService.GetById(id);
            var userViewModel = Mapper.Map<account, accountViewModel>(user);
            if (user != null)
            {
                statusUser = true;
            }
            return Json(new
            {
                data = userViewModel,
                status = statusUser
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveData(string strUser, int Hidid, string hidPass)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var user1 = serializer.Deserialize<accountViewModel>(strUser);

            var user = new account();
            user.Updateaccount(user1);

            bool status = true;
            string message = string.Empty;

            var useraccount = _accountService.GetByUsername(user.username);

            if (Hidid == 0)//dang them
            {
                if (useraccount == null)
                {
                    user.password = _accountService.EncodeSHA1(user.password);
                    user.ngaytao = DateTime.Now;
                    user.ngaycapnhat = DateTime.Now;
                    user.ngaydoimk = DateTime.Now;

                    try
                    {
                        _accountService.Add(user);
                        _accountService.Save();

                    }
                    catch (Exception ex)
                    {
                        status = false;
                        //message = ex.Message;
                        throw ex;
                    }
                    message = "Đã lưu thành công.";
                }
                else
                {
                    message = "Username này đã tồn tại.";
                }


            }
            else if (Hidid != 0)
            {
                //int userCount = _accountService.CountByUsername(useraccount.username);

                user.id = Hidid;
                user.ngaycapnhat = DateTime.Now;
                //var oldUser = _accountService.GetById(user.username);
                if (user.password != "") //password field is required
                {
                    user.password = _accountService.EncodeSHA1(user.password);
                    user.ngaydoimk = DateTime.Now;
                }
                else
                    user.password = hidPass;

                try
                {
                    _accountService.Update(user);
                    _accountService.Save();
                    message = "Đã cập nhật thành công.";

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
                _accountService.Delete(id);
                _accountService.Save();
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
    }
}