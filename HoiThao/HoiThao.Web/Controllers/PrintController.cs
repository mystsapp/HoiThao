using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using HoiThao.Web.Data.Models;
using HoiThao.Web.Infrastructure.Core;
using HoiThao.Web.Models;
using HoiThao.Web.Service;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Linq;
using CrystalDecisions.Shared;
using HoiThao.Web.Common;

namespace HoiThao.Web.Controllers
{
    public class PrintController : BaseController
    {
        private IaseanService _iaseanService;

        public PrintController(IaseanService iaseanService)
        {
            _iaseanService = iaseanService;
        }

        // GET: Print
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AseanReportDemo()
        {
            var aseanList = _iaseanService.GetAll();
            var responseData = Mapper.Map<IEnumerable<asean>, IEnumerable<aseanViewModel>>(aseanList);
            return View(responseData);
        }

        public CrystalReportPdfResult ExportReport()
        {
            var aseanList = _iaseanService.GetAll().Select(x=>new {
                x.k,
                x.id,
                x.firstname,
                x.email
            });
            //var responseData = Mapper.Map<IEnumerable<asean>, IEnumerable<aseanViewModel>>(aseanList);

            ReportDocument rd = new ReportDocument();
            string reportPath = Path.Combine(Server.MapPath("~/Reports"), "rpt_asean.rpt");
            return new CrystalReportPdfResult(reportPath, aseanList);
        }
    }
}