using System;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Builders;
using System.Linq;

namespace DAL.Migrations
{
    public abstract class DbMigrationWithHistory:DbMigration
    {
        /// <summary>
        /// Adds an operation to create a new stored procedure.
        /// Wrapper on <see cref="CreateStoredProcedure{TParameters}"/> to add history logs.
        /// </summary>
        /// <param name="name">
        /// The name of the stored procedure. Schema name is optional, if no schema is specified then dbo is
        /// assumed.
        /// </param>
        /// <param name="parametersAction">The action that specifies the parameters of the stored procedure.</param>
        /// <param name="body">The body of the stored procedure.</param>
        /// <param name="moveOldToHistory">Specifies whether to defer to the history of stored procedure if one already exists.
        /// Rename old procedure to  pattern {spName}_{DateTime.Now:yyyyMMdd}_{GetType().Name}Migration.
        /// Example name of history procedure: ReportService.pGetSocietyInfo_20190117_ZipGraphImagesMigration
        /// </param>
        /// <param name="anonymousArguments">
        /// The additional arguments that may be processed by providers. Use anonymous type syntax
        /// to specify arguments. For example, 'new { SampleArgument = "MyValue" }'.
        /// </param>
        /// <typeparam name="TParameters">
        /// The parameters in this create stored procedure operation. You do not need to specify this
        /// type, it will be inferred from the <paramref name="parametersAction" /> parameter you supply.
        /// </typeparam>
        protected void CreateStoredProcedure<TParameters>(
            string name,
            Func<ParameterBuilder, TParameters> parametersAction,
            string body,
            bool moveOldToHistory = false,
            object anonymousArguments = null)
        {
            if (moveOldToHistory)
            {
                string[] spFullName = name.Split('.');
                string spScheme=null;
                string spName=null;
                if (spFullName.Length > 1)
                {
                    spName = spFullName.Last();
                    spFullName[spFullName.Length - 1] = "";
                    spScheme = String.Join(".",spFullName).TrimEnd('.');
                }
                string historySpName = $"{spName}_{DateTime.Now:yyyyMMdd}_{GetType().Name}Migration";
                Sql(
                    $"IF(EXISTS (" +
                    $"SELECT * FROM sys.objects as o " +
                    $"left join sys.schemas as s on o.schema_id = s.schema_id " +
                    $"WHERE o.[name] = N'{spName}' AND type in (N'P', N'PC') {(spScheme!=null?"and s.name=N'"+spScheme+"' ":" ")} "+
                    $") AND NOT EXISTS (" +
                    $"SELECT * FROM sys.objects as o " +
                    $"left join sys.schemas as s on o.schema_id = s.schema_id " +
                    $"WHERE o.[name] = N'{spName}_{DateTime.Now:yyyyMMdd}_{GetType().Name}' AND type in (N'P', N'PC') {(spScheme!=null?"and s.name=N'"+spScheme+"' ":" ")} " +
                    $"))" +
                    $"BEGIN EXEC sp_rename '{name}', '{historySpName}' END"
                );
            }
            CreateStoredProcedure(name,parametersAction,body,anonymousArguments);
        }
    }
}