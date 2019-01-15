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
    }
    public class aseanRepository : RepositoryBase<asean>, IaseanRepository
    {
        public aseanRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}