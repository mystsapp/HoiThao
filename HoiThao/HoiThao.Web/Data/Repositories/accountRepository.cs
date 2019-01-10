using HoiThao.Web.Data.Infrastructure;
using HoiThao.Web.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoiThao.Web.Data.Repositories
{
    public interface IaccountRepository : IRepository<account>
    {
    }
    public class accountRepository : RepositoryBase<account>, IaccountRepository
    {
        public accountRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

    }
}