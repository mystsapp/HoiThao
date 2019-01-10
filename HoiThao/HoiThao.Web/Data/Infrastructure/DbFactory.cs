using HoiThao.Web.Data.Models;

namespace HoiThao.Web.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        private HoiThaoDbContext dbContext;

        public HoiThaoDbContext Init()
        {
            return dbContext ?? (dbContext = new HoiThaoDbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}