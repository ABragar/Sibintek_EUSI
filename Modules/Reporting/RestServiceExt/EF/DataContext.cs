using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using RestService.Entities;
using RestService.Identity;

namespace RestService.EF
{
    public class DataContext : DbContext
        {
            public DataContext(): base("DataContext")
            {
            }

            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
            //throw new UnintentionalCodeFirstException();
            }


            public virtual DbRawSqlQuery<GraphBySociety> pGraphBySociety(int? societyID)
            {
                SqlParameter param1 = new SqlParameter("@SocietyID", societyID);
                return this.Database.SqlQuery<GraphBySociety>("pGraphBySociety @SocietyID", param1);
            }

            public virtual DbRawSqlQuery<UserInfo> pGetUserInfo(string login)
            {
                SqlParameter param1 = new SqlParameter("@Login", login);
                return this.Database.SqlQuery<UserInfo>("ReportService.pGetUserInfo @Login", param1);
            }
    }
}