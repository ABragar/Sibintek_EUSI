using System;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using DAL.Entities;
using DAL.Migrations;
using NLog;
using UserInfo = DAL.Entities.UserInfo;

namespace DAL.EF
{
    public partial class ReportDbContext:DbContext
    {
        private static ILogger _log = LogManager.GetCurrentClassLogger();
        private const string Schema = "ReportService"; 
        public ReportDbContext() : base("name=DataContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ReportDbContext, Configuration>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);
//            modelBuilder.Entity<HistoryRow>().ToTable(tableName: "__MigrationHistory", schemaName: "Report");

            modelBuilder.Entity<ReportHistory>().HasKey(f => f.ID)
            .Property(f => f.CreatedDate)
            .HasColumnType("datetime2");
            modelBuilder.Entity<Report>().HasKey(f => f.ID);

        }
        


        public virtual void ExecuteSQL(string sql)
        {
            using (Database.Connection)
            {
                Database.Connection.Open();
                DbTransaction transaction = Database.Connection.BeginTransaction();
                DbCommand command = Database.Connection.CreateCommand();
                command.Transaction = transaction;
                try
                {
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                    _log.Debug($"Transaction success. SQL: {sql.Substring(0, 20)}...");
                }
                catch (Exception ex)
                {
                    _log.Error($"Transaction failed. SQL: {sql.Substring(0, 20)}...");
                    _log.Error(ex.Message);
                    try
                    {
                        transaction.Rollback();
                        _log.Info($"Transaction rollbak success. SQL: {sql.Substring(0, 20)}...");
                    }
                    catch (Exception exRollback)
                    {
                        _log.Error($"Transaction rollbak failde. SQL: {sql.Substring(0, 20)}...");
                        _log.Error(exRollback.Message);
                    }

                    throw ex;
                }
            }
        }

        public virtual DbRawSqlQuery<UserInfo> pGetUserInfo(string login)
        {
            SqlParameter param1 = new SqlParameter("@Login", login);
            return Database.SqlQuery<UserInfo>($"{Schema}.pGetUserInfo @Login", param1);
        }
        
        public virtual DbRawSqlQuery<SocietyInfo> pGetSocietyInfo(int societyId)
        {
            SqlParameter param1 = new SqlParameter("@vIntId", societyId);
            SqlParameter param2 = new SqlParameter("@vStrIDEUP", SqlDbType.NVarChar){Value = DBNull.Value};
            return Database.SqlQuery<SocietyInfo>($"{Schema}.pGetSocietyInfo @vIntId , @vStrIDEUP", new []{
                param1,param2});
        }
        
        public virtual DbRawSqlQuery<SocietyInfo> pGetSocietyInfoByIdEUP(string ideup)
        {
            SqlParameter param1 = new SqlParameter("@vIntId", SqlDbType.Int){Value = DBNull.Value};
            SqlParameter param2 = new SqlParameter("@vStrIDEUP", ideup);
            return Database.SqlQuery<SocietyInfo>($"{Schema}.pGetSocietyInfo @vIntId , @vStrIDEUP", new []{param1,param2});
        }
        
        public virtual DbRawSqlQuery<GraphBySociety> pGraphBySociety(int? societyID)
        {
            SqlParameter param1 = new SqlParameter("@SocietyID", societyID);
            return this.Database.SqlQuery<GraphBySociety>($"{Schema}.pGraphBySociety @SocietyID", param1);
        }
    }
}