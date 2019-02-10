using HoiThao.Web.Common;
using HoiThao.Web.Data.Infrastructure;
using HoiThao.Web.Data.Models;
using HoiThao.Web.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace HoiThao.Web.Service
{
    public interface IaseanService
    {
        void add(asean aseanItem);
        void Update(asean aseanItem);

        asean GetById(int id);
        IEnumerable<string> GetAllHotel();

        IEnumerable<asean> Search(string keyword, int page, int pageSize, string status, out int totalRow);
        string GetLastId(ref bool status, ref string message);
        string GetNextId(string id, ref bool status, ref string message);

        decimal UpdateCheckin(asean entity);
        void UpdateAsean(asean entity);

        DataTable ConferenceReport();
        DataTable CheckinReport();
        DataTable ConferenceGroupByCountry();
        DataTable PaymentReport();
        DataTable PickupReport();
        DataTable AirticketReport();
        DataTable TourReport();
        DataTable HotelReport(string hotel);

        IEnumerable<string> GetAllCountry();
        DataTable GetAllByCountry(string countryName);

        void Delete(int id);
        void save();
    }

    public class aseanService : IaseanService
    {
        private IaseanRepository _aseanRepository;
        private IUnitOfWork _unitOfWork;

        public aseanService(IaseanRepository aseanRepository, IUnitOfWork unitOfWork)
        {
            _aseanRepository = aseanRepository;
            _unitOfWork = unitOfWork;
        }
        public void add(asean aseanItem)
        {
            _aseanRepository.Add(aseanItem);
        }

        public string GetLastId(ref bool status, ref string message)
        {
            var id = _aseanRepository.GetLastId(ref status, ref message);
            return id;
        }

        public string GetNextId(string id, ref bool status, ref string message)
        {
            try
            {
                var NextId = Utilities.NextID(id, "ID");
                status = true;
                message = "Next Id ok.";
                return NextId;
            }
            catch (Exception)
            {

                status = false;
                message = "Next Id fail.";
                return null;
            }
        }

        public asean GetById(int id)
        {
            return _aseanRepository.GetSingleById(id);
        }

        public void save()
        {
            _unitOfWork.Commit();
        }

        public IEnumerable<asean> Search(string keyword, int page, int pageSize, string status, out int totalRow)
        {
            var query = _aseanRepository.GetAll();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = _aseanRepository.GetMulti(x => x.firstname.Contains(keyword) || x.id.Contains(keyword));
            }

            if (!string.IsNullOrEmpty(status))
            {
                var statusBool = bool.Parse(status);
                query = query.Where(x => x.invited == statusBool);
            }

            totalRow = query.Count();
            query = query.OrderByDescending(x => x.id).Skip((page - 1) * pageSize).Take(pageSize);

            return query;
        }

        public void Update(asean aseanItem)
        {
            _aseanRepository.Update(aseanItem);
        }

        public decimal UpdateCheckin(asean entity)
        {
            return _aseanRepository.UpdateCheckin(entity);
        }

        public void UpdateAsean(asean entity)
        {
            _aseanRepository.UpdateAsean(entity);
        }

        public void Delete(int id)
        {
            _aseanRepository.Delete(id);
        }

        public DataTable ConferenceReport()
        {
            try
            {
                DataTable dt = new DataTable();
                var result = _aseanRepository.GetAll().Select(p => new
                {
                    p.k,
                    p.dangky,
                    p.firstname,
                    p.address,
                    p.country,
                    p.tel,
                    p.email,
                    p.id,
                    p.totala,
                    p.totalb,
                    p.grandtotal,
                    p.payment,
                    p.currency,
                    p.lastname,
                    p.title,
                    p.cardnumber
                });
                var count = result.Count();

                dt = EntityToTable.ToDataTable(result);
                if (dt.Rows.Count > 0)
                    return dt;
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }

        public DataTable CheckinReport()
        {
            try
            {
                DataTable dt = new DataTable();
                var result = _aseanRepository.GetMulti(x=>x.checkin.HasValue).Select(p => new
                {
                    p.k,
                    p.checkin,
                    p.firstname,
                    p.address,
                    p.country,
                    p.tel,
                    p.email,
                    p.id,
                    p.totala,
                    p.totalb,
                    p.grandtotal,
                    p.payment,
                    p.currency,
                    p.lastname,
                    p.title,
                    p.cardnumber
                });
                var count = result.Count();

                dt = EntityToTable.ToDataTable(result);
                if (dt.Rows.Count > 0)
                    return dt;
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }

        public DataTable ConferenceGroupByCountry()
        {
            try
            {
                DataTable dt = new DataTable();
                var result = _aseanRepository.GetAll().Select(p => new
                {
                    p.k,
                    p.dangky,
                    p.firstname,
                    p.address,
                    p.country,
                    p.tel,
                    p.email,
                    p.id,
                    p.totala,
                    p.totalb,
                    p.grandtotal,
                    p.payment,
                    p.currency,
                    p.lastname,
                    p.title,
                    p.cardnumber
                }).OrderBy(x => x.country);





                var count = result.Count();

                dt = EntityToTable.ToDataTable(result);
                if (dt.Rows.Count > 0)
                    return dt;
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<string> GetAllCountry()
        {
            return _aseanRepository.GetAll().Select(x => x.country).Distinct();
        }

        public DataTable GetAllByCountry(string countryName)
        {
            var result = _aseanRepository.GetAll().Where(x => x.country == countryName).Select(p => new
            {
                p.k,
                p.dangky,
                p.firstname,
                p.address,
                p.country,
                p.tel,
                p.email,
                p.id,
                p.totala,
                p.totalb,
                p.grandtotal,
                p.payment,
                p.currency,
                p.lastname,
                p.title,
                p.cardnumber
            });
            DataTable dt = new DataTable();
            dt = EntityToTable.ToDataTable(result);
            if (dt.Rows.Count > 0)
                return dt;
            else
                return null;
        }

        public DataTable PaymentReport()
        {
            try
            {
                DataTable dt = new DataTable();
                var result = _aseanRepository.GetAll().Select(p => new
                {
                    p.k,
                    p.dangky,
                    p.firstname,
                    p.address,
                    p.department,
                    p.tel,
                    p.email,
                    p.id,
                    p.totala,
                    p.totalb,
                    p.grandtotal,
                    p.payment,
                    p.currency,
                    p.lastname,
                    p.title,
                    p.cardnumber,
                    p.invited,
                    p.amount,
                    p.country,
                    p.cabk,
                    p.caravelle,
                    p.car_ah,
                    p.Hotel,
                    p.note
                });

                var count = result.Count();

                dt = EntityToTable.ToDataTable(result);
                if (dt.Rows.Count > 0)
                    return dt;
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }

        public DataTable PickupReport()
        {
            try
            {
                DataTable dt = new DataTable();
                var result = _aseanRepository.GetAll().Select(p => new
                {
                    p.k,
                    p.dangky,
                    p.firstname,
                    p.address,
                    p.department,
                    p.tel,
                    p.email,
                    p.id,
                    p.totala,
                    p.totalb,
                    p.grandtotal,
                    p.payment,
                    p.currency,
                    p.lastname,
                    p.title,
                    p.cardnumber,
                    p.invited,
                    p.amount
                });

                var count = result.Count();

                dt = EntityToTable.ToDataTable(result);
                if (dt.Rows.Count > 0)
                    return dt;
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }

        public DataTable AirticketReport()
        {
            try
            {
                DataTable dt = new DataTable();
                var result = _aseanRepository.GetAll().Select(p => new
                {
                    p.k,
                    p.dangky,
                    p.firstname,
                    p.address,
                    p.department,
                    p.tel,
                    p.email,
                    p.id,
                    p.ydepdate,
                    p.cdepdate
                });

                var count = result.Count();

                dt = EntityToTable.ToDataTable(result);
                if (dt.Rows.Count > 0)
                    return dt;
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }

        public DataTable TourReport()
        {
            try
            {
                DataTable dt = new DataTable();
                var result = _aseanRepository.GetAll().Select(p => new
                {
                    p.k,
                    p.dangky,
                    p.firstname,
                    p.address,
                    p.department,
                    p.tel,
                    p.email,
                    p.id,
                    p.fax,
                    p.Hotel
                });

                var count = result.Count();

                dt = EntityToTable.ToDataTable(result);
                if (dt.Rows.Count > 0)
                    return dt;
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<string> GetAllHotel()
        {
            //List<string> hotelList = _aseanRepository.GetAll().Select(x => x.Hotel).Distinct().ToList();
            //return _chinhanhRepository.GetAll().Select(x => x.chinhanh1).Distinct();
            return _aseanRepository.GetAll().Select(x => x.Hotel).Distinct();
            //foreach(var item in hotelList)
            //{
            //    if (item == null)
            //        hotelList.RemoveAt(item.);
            //}
            //return hotelList;
        }

        public DataTable HotelReport(string hotel)
        {
            DataTable dt = new DataTable();
            if (hotel == "Other")
            {
                var result = _aseanRepository.GetMulti(x => x.Hotel == null).Select(p => new
                {
                    p.k,
                    p.dangky,
                    p.firstname,
                    p.address,
                    p.department,
                    p.tel,
                    p.email,
                    p.id,
                    p.HotelCheckin,
                    p.HotelCheckout,
                    p.HotelPrice,
                    p.HotelBookingInf,
                    p.group
                });
                var count = result.Count();
                dt = EntityToTable.ToDataTable(result);

            }
            else
            {
                var result = _aseanRepository.GetMulti(x => x.Hotel == hotel).Select(p => new
                {
                    p.k,
                    p.dangky,
                    p.firstname,
                    p.address,
                    p.department,
                    p.tel,
                    p.email,
                    p.id,
                    p.HotelCheckin,
                    p.HotelCheckout,
                    p.HotelPrice,
                    p.HotelBookingInf,
                    p.group
                });
                var count = result.Count();
                dt = EntityToTable.ToDataTable(result);

            }



            if (dt.Rows.Count > 0)
                return dt;
            else
                return null;

        }
    }
}