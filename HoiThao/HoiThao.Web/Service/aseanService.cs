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

        public void save()
        {
            _unitOfWork.Commit();
        }
    }
}