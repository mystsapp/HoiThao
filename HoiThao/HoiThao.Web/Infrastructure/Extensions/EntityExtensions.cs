using HoiThao.Web.Data.Models;
using HoiThao.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoiThao.Web.Infrastructure.Extensions
{
    public static class EntityExtensions
    {
        public static void Updateaccount(this account acc, accountViewModel accViewModel)
        {
            acc.username = accViewModel.username;
            acc.password = accViewModel.password;
            acc.hoten = accViewModel.hoten;
            acc.daily = accViewModel.daily;
            acc.chinhanh = accViewModel.chinhanh;
            acc.role = accViewModel.role;
            acc.doimatkhau = accViewModel.doimatkhau;
            acc.ngaydoimk = accViewModel.ngaydoimk;
            acc.trangthai = accViewModel.trangthai;
            acc.khoi = accViewModel.khoi;
            acc.nguoitao = accViewModel.nguoitao;
            acc.ngaytao = accViewModel.ngaytao;

            acc.nguoicapnhat = accViewModel.nguoicapnhat;
            acc.ngaycapnhat = accViewModel.ngaycapnhat;

            acc.nhom = accViewModel.nhom;
        }

        public static void Updateasean(this asean acc, aseanViewModel accViewModel)
        {
            acc.k = accViewModel.k;
            acc.invited = accViewModel.invited;
            acc.speaker = accViewModel.speaker;
            acc.id = accViewModel.id;
            acc.title = accViewModel.title;
            acc.firstname = accViewModel.firstname;
            acc.lastname = accViewModel.lastname;
            acc.company = accViewModel.company;
            acc.email = accViewModel.email;
            acc.tel = accViewModel.tel;
            acc.country = accViewModel.country;
            acc.payment = accViewModel.payment;
            acc.amount = accViewModel.amount;
            acc.bankfee = accViewModel.bankfee;
            acc.mop = accViewModel.mop;
            acc.cardnumber = accViewModel.cardnumber;

            acc.currency = accViewModel.currency;
            acc.rate = accViewModel.rate;
            acc.grandtotal = accViewModel.grandtotal;
            acc.checkin = accViewModel.checkin;
            acc.descript = accViewModel.descript;
            acc.vatbill = accViewModel.vatbill;
            acc.taxcode = accViewModel.taxcode;
            acc.fax = accViewModel.fax;
            acc.dangky = accViewModel.dangky;
            acc.totala = accViewModel.totala;
            acc.totalb = accViewModel.totalb;

            acc.address = accViewModel.address;
            acc.department = accViewModel.department;
            acc.institutio = accViewModel.institutio;
            acc.makh = accViewModel.makh;

            acc.code = accViewModel.code;
            acc.group = accViewModel.group;
            acc.note = accViewModel.note;
            acc.Hotel = accViewModel.Hotel;

            acc.HotelCheckin = accViewModel.HotelCheckin;
            acc.HotelCheckout = accViewModel.HotelCheckout;
            acc.HotelPrice = accViewModel.HotelPrice;
            acc.HotelDon = accViewModel.HotelDon;

            acc.HotelTien = accViewModel.HotelTien;
            acc.HotelBookingInf = accViewModel.HotelBookingInf;
        }
    }
}