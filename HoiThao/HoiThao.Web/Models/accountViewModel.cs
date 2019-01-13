using System;

namespace HoiThao.Web.Models
{
    public class accountViewModel
    {
        public int id { get; set; }
        public string nhom { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string hoten { get; set; }
        public string daily { get; set; }
        public string chinhanh { get; set; }
        public string role { get; set; }
        public bool doimatkhau { get; set; }
        public Nullable<System.DateTime> ngaydoimk { get; set; }
        public bool trangthai { get; set; }
        public string khoi { get; set; }
        public string nguoitao { get; set; }
        public Nullable<System.DateTime> ngaytao { get; set; }
        public string nguoicapnhat { get; set; }
        public Nullable<System.DateTime> ngaycapnhat { get; set; }
    }
}