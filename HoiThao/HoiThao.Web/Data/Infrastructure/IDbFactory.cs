using HoiThao.Web.Data.Models;
using System;

namespace HoiThao.Web.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        HoiThaoDbContext Init();
    }
}