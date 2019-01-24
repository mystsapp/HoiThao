﻿using HoiThao.Web.Common;
using HoiThao.Web.Data.Infrastructure;
using HoiThao.Web.Data.Models;
using HoiThao.Web.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoiThao.Web.Service
{
    public interface IaseanService
    {
        void add(asean aseanItem);
        void Update(asean aseanItem);

        asean GetById(int id);
        IEnumerable<asean> Search(string keyword, int page, int pageSize, string status, out int totalRow);
        string GetLastId(ref bool status, ref string message);
        string GetNextId(string id, ref bool status, ref string message);

        decimal UpdateCheckin(asean entity);
        void UpdateAsean(asean entity);

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
    }
}