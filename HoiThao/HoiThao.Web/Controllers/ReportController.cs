using HoiThao.Web.Service;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HoiThao.Web.Controllers
{
    public class ReportController : Controller
    {
        private IaseanService _aseanService;

        public ReportController(IaseanService iaseanService)
        {
            _aseanService = iaseanService;
        }
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ConferenceReport()
        {

            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");
            // Định dạng chiều dài cho cột
            xlSheet.Column(1).Width = 10;//STT
            xlSheet.Column(2).Width = 25;// Registration date
            xlSheet.Column(3).Width = 40;// Name
            xlSheet.Column(4).Width = 20;// Address
            xlSheet.Column(5).Width = 20;// Country
            xlSheet.Column(6).Width = 10;// Telephone
            xlSheet.Column(7).Width = 25;//Email

            xlSheet.Column(8).Width = 10;//ID
            xlSheet.Column(9).Width = 25;// Total A
            xlSheet.Column(10).Width = 40;// Total B
            xlSheet.Column(11).Width = 20;// Grand Total
            xlSheet.Column(12).Width = 20;// Paid
            xlSheet.Column(13).Width = 10;// Currency
            xlSheet.Column(14).Width = 25;//Full delegate

            xlSheet.Column(15).Width = 10;// Resident
            xlSheet.Column(16).Width = 25;//Accompany Persons

            xlSheet.Cells[2, 1].Value = "CONFERENCE REPORT  ";
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 16, FontStyle.Bold));
            xlSheet.Cells[2, 1, 2, 7].Merge = true;
            setCenterAligment(2, 1, 2, 7, xlSheet);
            // dinh dang tu ngay den ngay
            //if (tungay == denngay)
            //{
            //    fromTo = "Ngày: " + tungay;
            //}
            //else
            //{
            //    fromTo = "Từ ngày: " + tungay + " đến ngày: " + denngay;
            //}

            //xlSheet.Cells[3, 1].Value = fromTo;
            //xlSheet.Cells[3, 1, 3, 7].Merge = true;
            //xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 14, FontStyle.Bold));
            //setCenterAligment(3, 1, 3, 7, xlSheet);

            // Tạo header
            xlSheet.Cells[5, 1].Value = "STT";
            xlSheet.Cells[5, 2].Value = "Registration date ";
            xlSheet.Cells[5, 3].Value = "Name ";
            xlSheet.Cells[5, 4].Value = "Address";
            xlSheet.Cells[5, 5].Value = "Country";
            xlSheet.Cells[5, 6].Value = "Telephone";
            xlSheet.Cells[5, 7].Value = "Email";

            xlSheet.Cells[5, 8].Value = "ID";
            xlSheet.Cells[5, 9].Value = "Total A ";
            xlSheet.Cells[5, 10].Value = "Total B ";
            xlSheet.Cells[5, 11].Value = "Grand Total";
            xlSheet.Cells[5, 12].Value = "Paid";
            xlSheet.Cells[5, 13].Value = "Currency";
            xlSheet.Cells[5, 14].Value = "Full delegate";

            xlSheet.Cells[5, 15].Value = "Resident";
            xlSheet.Cells[5, 16].Value = "Accompany Persons";
            xlSheet.Cells[5, 1, 5, 16].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));

            // do du lieu tu table
            int dong = 5;


            DataTable dt = _aseanService.ConferenceReport();


            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dong++;
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (String.IsNullOrEmpty(dt.Rows[i][j].ToString()))
                        {
                            xlSheet.Cells[dong, j + 1].Value = "";
                        }
                        else
                        {
                            xlSheet.Cells[dong, j + 1].Value = dt.Rows[i][j];
                        }
                    }
                }
            }
            else
            {
                // SetAlert("No sale.", "warning");
                return Json(new
                {
                    status = false,
                    message = "failure"
                });
            }
            dong++;
            //// Merger cot 4,5 ghi tổng tiền
            //setRightAligment(dong, 3, dong, 3, xlSheet);
            //xlSheet.Cells[dong, 1, dong, 2].Merge = true;
            //xlSheet.Cells[dong, 1].Value = "Tổng tiền: ";

            // Sum tổng tiền

            //xlSheet.Cells[dong, 5].Value = "TC";
            //xlSheet.Cells[dong, 6].Formula = "SUM(F6:F" + (6 + dt.Rows.Count - 1) + ")";
            //xlSheet.Cells[dong, 7].Formula = "SUM(G6:G" + (6 + dt.Rows.Count - 1) + ")";

            // định dạng số

            //NumberFormat(dong, 6, dong, 7, xlSheet);

            setBorder(5, 1, 5 + dt.Rows.Count, 16, xlSheet);
            setFontBold(5, 1, 5, 16, 12, xlSheet);
            setFontSize(6, 1, 6 + dt.Rows.Count, 16, 12, xlSheet);
            // dinh dang giua cho cot stt
            setCenterAligment(6, 1, 6 + dt.Rows.Count, 1, xlSheet);

            setBorder(dong, 5, dong, 16, xlSheet);
            setFontBold(dong, 5, dong, 16, 12, xlSheet);

            // dinh dạng ngay thang cho cot ngay di , ngay ve
            DateTimeFormat(6, 2, 6 + dt.Rows.Count, 2, xlSheet);
            // canh giưa cot  ngay di, ngay ve, so khach 
            setCenterAligment(6, 4, 6 + dt.Rows.Count, 6, xlSheet);
            // dinh dạng number cot doanh so
            NumberFormat(6, 11, 6 + dt.Rows.Count, 11, xlSheet);
            NumberFormat(6, 12, 6 + dt.Rows.Count, 12, xlSheet);

            //xlSheet.View.FreezePanes(6, 20);

            try
            {
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + "CONFERENCE_REPORT" + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ".xlsx");
                Response.BinaryWrite(ExcelApp.GetAsByteArray());
                Response.End();

                
            }
            catch (Exception)
            {

                throw;
            }

            return View();
        }

        public ActionResult WorkshopReport()
        {

            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");
            // Định dạng chiều dài cho cột
            xlSheet.Column(1).Width = 10;//STT
            xlSheet.Column(2).Width = 25;// Registration date
            xlSheet.Column(3).Width = 40;// Name
            xlSheet.Column(4).Width = 20;// Address
            xlSheet.Column(5).Width = 20;// Country
            xlSheet.Column(6).Width = 10;// Telephone
            xlSheet.Column(7).Width = 25;//Email

            xlSheet.Column(8).Width = 10;//ID
            xlSheet.Column(9).Width = 25;// Total A
            xlSheet.Column(10).Width = 40;// Total B
            xlSheet.Column(11).Width = 20;// Grand Total
            xlSheet.Column(12).Width = 20;// Paid
            xlSheet.Column(13).Width = 10;// Currency
            xlSheet.Column(14).Width = 25;//Full delegate

            xlSheet.Column(15).Width = 10;// Resident
            xlSheet.Column(16).Width = 25;//Accompany Persons

            xlSheet.Cells[2, 1].Value = "CONFERENCE REPORT  ";
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 16, FontStyle.Bold));
            xlSheet.Cells[2, 1, 2, 7].Merge = true;
            setCenterAligment(2, 1, 2, 7, xlSheet);
            // dinh dang tu ngay den ngay
            //if (tungay == denngay)
            //{
            //    fromTo = "Ngày: " + tungay;
            //}
            //else
            //{
            //    fromTo = "Từ ngày: " + tungay + " đến ngày: " + denngay;
            //}

            //xlSheet.Cells[3, 1].Value = fromTo;
            //xlSheet.Cells[3, 1, 3, 7].Merge = true;
            //xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 14, FontStyle.Bold));
            //setCenterAligment(3, 1, 3, 7, xlSheet);

            // Tạo header
            xlSheet.Cells[5, 1].Value = "STT";
            xlSheet.Cells[5, 2].Value = "Registration date ";
            xlSheet.Cells[5, 3].Value = "Name ";
            xlSheet.Cells[5, 4].Value = "Address";
            xlSheet.Cells[5, 5].Value = "Country";
            xlSheet.Cells[5, 6].Value = "Telephone";
            xlSheet.Cells[5, 7].Value = "Email";

            xlSheet.Cells[5, 8].Value = "ID";
            xlSheet.Cells[5, 9].Value = "Total A ";
            xlSheet.Cells[5, 10].Value = "Total B ";
            xlSheet.Cells[5, 11].Value = "Grand Total";
            xlSheet.Cells[5, 12].Value = "Paid";
            xlSheet.Cells[5, 13].Value = "Currency";
            xlSheet.Cells[5, 14].Value = "Full delegate";

            xlSheet.Cells[5, 15].Value = "Resident";
            xlSheet.Cells[5, 16].Value = "Accompany Persons";
            xlSheet.Cells[5, 1, 5, 16].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));

            // do du lieu tu table
            int dong = 5;


            DataTable dt = _aseanService.ConferenceReport();


            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dong++;
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (String.IsNullOrEmpty(dt.Rows[i][j].ToString()))
                        {
                            xlSheet.Cells[dong, j + 1].Value = "";
                        }
                        else
                        {
                            xlSheet.Cells[dong, j + 1].Value = dt.Rows[i][j];
                        }
                    }
                }
            }
            else
            {
                // SetAlert("No sale.", "warning");
                return Json(new
                {
                    status = false,
                    message = "failure"
                });
            }
            dong++;
            //// Merger cot 4,5 ghi tổng tiền
            //setRightAligment(dong, 3, dong, 3, xlSheet);
            //xlSheet.Cells[dong, 1, dong, 2].Merge = true;
            //xlSheet.Cells[dong, 1].Value = "Tổng tiền: ";

            // Sum tổng tiền

            //xlSheet.Cells[dong, 5].Value = "TC";
            //xlSheet.Cells[dong, 6].Formula = "SUM(F6:F" + (6 + dt.Rows.Count - 1) + ")";
            //xlSheet.Cells[dong, 7].Formula = "SUM(G6:G" + (6 + dt.Rows.Count - 1) + ")";

            // định dạng số

            //NumberFormat(dong, 6, dong, 7, xlSheet);

            setBorder(5, 1, 5 + dt.Rows.Count, 16, xlSheet);
            setFontBold(5, 1, 5, 16, 12, xlSheet);
            setFontSize(6, 1, 6 + dt.Rows.Count, 16, 12, xlSheet);
            // dinh dang giua cho cot stt
            setCenterAligment(6, 1, 6 + dt.Rows.Count, 1, xlSheet);

            setBorder(dong, 5, dong, 16, xlSheet);
            setFontBold(dong, 5, dong, 16, 12, xlSheet);

            // dinh dạng ngay thang cho cot ngay di , ngay ve
            DateTimeFormat(6, 2, 6 + dt.Rows.Count, 2, xlSheet);
            // canh giưa cot  ngay di, ngay ve, so khach 
            setCenterAligment(6, 4, 6 + dt.Rows.Count, 6, xlSheet);
            // dinh dạng number cot doanh so
            NumberFormat(6, 11, 6 + dt.Rows.Count, 11, xlSheet);
            NumberFormat(6, 12, 6 + dt.Rows.Count, 12, xlSheet);

            //xlSheet.View.FreezePanes(6, 20);

            try
            {
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + "CONFERENCE_REPORT" + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ".xlsx");
                Response.BinaryWrite(ExcelApp.GetAsByteArray());
                Response.End();


            }
            catch (Exception)
            {

                throw;
            }

            return View();
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////


        private static void NumberFormat(int fromRow, int fromColumn, int toRow, int toColumn, ExcelWorksheet sheet)
        {
            using (var range = sheet.Cells[fromRow, fromColumn, toRow, toColumn])
            {
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                range.Style.Numberformat.Format = "#,#0";
            }
        }
        private static void DateFormat(int fromRow, int fromColumn, int toRow, int toColumn, ExcelWorksheet sheet)
        {
            using (var range = sheet.Cells[fromRow, fromColumn, toRow, toColumn])
            {
                range.Style.Numberformat.Format = "dd/MM/yyyy";
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
        }
        private static void DateTimeFormat(int fromRow, int fromColumn, int toRow, int toColumn, ExcelWorksheet sheet)
        {
            using (var range = sheet.Cells[fromRow, fromColumn, toRow, toColumn])
            {
                range.Style.Numberformat.Format = "dd/MM/yyyy HH:mm";
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
        }
        private static void setRightAligment(int fromRow, int fromColumn, int toRow, int toColumn, ExcelWorksheet sheet)
        {
            using (var range = sheet.Cells[fromRow, fromColumn, toRow, toColumn])
            {
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            }
        }
        private static void setCenterAligment(int fromRow, int fromColumn, int toRow, int toColumn, ExcelWorksheet sheet)
        {
            using (var range = sheet.Cells[fromRow, fromColumn, toRow, toColumn])
            {
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
        }
        private static void setFontSize(int fromRow, int fromColumn, int toRow, int toColumn, int fontSize, ExcelWorksheet sheet)
        {
            using (var range = sheet.Cells[fromRow, fromColumn, toRow, toColumn])
            {
                range.Style.Font.SetFromFont(new Font("Times New Roman", fontSize, FontStyle.Regular));
            }
        }
        private static void setFontBold(int fromRow, int fromColumn, int toRow, int toColumn, int fontSize, ExcelWorksheet sheet)
        {
            using (var range = sheet.Cells[fromRow, fromColumn, toRow, toColumn])
            {
                range.Style.Font.SetFromFont(new Font("Times New Roman", fontSize, FontStyle.Bold));
            }
        }
        private static void setBorder(int fromRow, int fromColumn, int toRow, int toColumn, ExcelWorksheet sheet)
        {
            using (var range = sheet.Cells[fromRow, fromColumn, toRow, toColumn])
            {
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            }
        }
        private static void PhantramFormat(int fromRow, int fromColumn, int toRow, int toColumn, ExcelWorksheet sheet)
        {
            using (var range = sheet.Cells[fromRow, fromColumn, toRow, toColumn])
            {
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.Numberformat.Format = "0 %";
            }
        }
    }
}