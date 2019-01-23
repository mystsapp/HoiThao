using HoiThao.Web.Data.Infrastructure;
using HoiThao.Web.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoiThao.Web.Data.Repositories
{
    public interface IaseanRepository : IRepository<asean>
    {
        string GetLastId(ref bool status, ref string message);

        void UpdateAsean(asean entity);

        decimal UpdateCheckin(asean entity);
    }
    public class aseanRepository : RepositoryBase<asean>, IaseanRepository
    {
        public aseanRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public string GetLastId(ref bool status, ref string message)
        {

            try
            {
                var id = DbContext.aseans.Where(x => x.id.Contains("ID")).OrderByDescending(x => x.id).Select(x => x.id).FirstOrDefault();
                status = true;
                message = "Last Id ok.";
                return id.ToString();
            }
            catch (Exception)
            {

                status = false;
                message = "Last Id fail.";
                return null;
            }

        }

        public void UpdateAsean(asean entity)
        {
            var aseanModel = DbContext.aseans.Find(entity.k);
            try
            {
                aseanModel.invited = entity.invited;
                aseanModel.speaker = entity.speaker;
                aseanModel.id = entity.id;
                aseanModel.title = entity.title;
                aseanModel.firstname = entity.firstname;
                aseanModel.lastname = entity.lastname;
                aseanModel.company = entity.company;
                aseanModel.email = entity.email;
                aseanModel.tel = entity.tel;
                aseanModel.country = entity.country;
                aseanModel.payment = entity.payment;
                aseanModel.amount = entity.amount;
                aseanModel.bankfee = entity.bankfee;
                aseanModel.mop = entity.mop;
                aseanModel.cardnumber = entity.cardnumber;
                aseanModel.currency = entity.currency;
                aseanModel.rate = entity.rate;
                aseanModel.grandtotal = entity.grandtotal;

                aseanModel.descript = entity.descript;
                aseanModel.vatbill = entity.vatbill;
                aseanModel.taxcode = entity.taxcode;
                aseanModel.fax = entity.fax;
                aseanModel.dangky = entity.dangky;
                aseanModel.totala = entity.totala;
                aseanModel.totalb = entity.totalb;
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public decimal UpdateCheckin(asean entity)
        {
            var aseanModel = DbContext.aseans.Find(entity.k);
            try
            {
                aseanModel.checkin = entity.checkin;
                DbContext.SaveChanges();
                return entity.k;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public string UpdateCheckin(asean entity)
        //{
        //    throw new NotImplementedException();
        //}
    }
}