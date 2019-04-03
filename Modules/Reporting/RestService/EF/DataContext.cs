using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using RestService.Entities;

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
        }
}