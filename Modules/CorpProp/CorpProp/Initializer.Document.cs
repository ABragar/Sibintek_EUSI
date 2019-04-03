using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Base.DAL;
using Base.Security.Service.Abstract;
using Base.UI;
using CorpProp.Entities.Document;

namespace CorpProp
{
    public static class DocumentInitializer
    {
        public static void Seed(IUnitOfWork uow)
        {
            CreateSp_UpdateNsiRecordStatus();

            //разрешения на файлы
            uow.GetRepository<FileCardPermission>().CreateCollection(
                                                                     new List<FileCardPermission>()
                                                                     {
                                                                         new FileCardPermission()
                                                                         {
                                                                             Name =
                                                                                 "Только для Автора",
                                                                             AccessModifier =
                                                                                 AccessModifier.
                                                                                     AuthorOnly
                                                                         },
                                                                         new FileCardPermission()
                                                                         {
                                                                             Name =
                                                                                 "Для всех пользователей ОГ, к которому привязан Автор",
                                                                             AccessModifier =
                                                                                 AccessModifier.
                                                                                     AuthorSociety
                                                                         },
                                                                         new FileCardPermission()
                                                                         {
                                                                             Name =
                                                                                 "Для всех пользователей ОГ с равным набором ролей, к которому привязан Автор",
                                                                             AccessModifier =
                                                                                 AccessModifier.
                                                                                     AuthorSocietyWithEqualRoles
                                                                         },
                                                                         new FileCardPermission()
                                                                         {
                                                                             Name =
                                                                                 "Для всех пользователей АИС КС",
                                                                             AccessModifier =
                                                                                 AccessModifier.
                                                                                     Everyone
                                                                         }
                                                                     });
            }

            private static void CreateSp_UpdateNsiRecordStatus()
            {

                string sql = @"
                IF OBJECT_ID(N'sp_UpdateNsiRecordStatus','P') IS NOT NULL
                    BEGIN
                        DROP PROCEDURE sp_UpdateNsiRecordStatus
                    END
                ";

                ExecuteSqlQuery(sql);

                sql = @"
                CREATE PROCEDURE [dbo].[sp_UpdateNsiRecordStatus]
                                @result int OUT,
                                @State NVARCHAR(MAX),
                                @Status NVARCHAR(MAX),
                                @TableName NVARCHAR(MAX)
                AS

                BEGIN TRY

                  --For Examle
                  --DECLARE @State NVARCHAR(MAX) = N'Temporary';
                  --DECLARE @Status NVARCHAR(MAX) = N'DelRequest';
                  --DECLARE @TableName NVARCHAR(MAX) = N'OKTMO'
  
                  DECLARE @DictObjectStateId INT = NULL;
                  DECLARE @DictObjectStatusId INT = NULL;
  
                  DECLARE @GetDictObjectByCode NVARCHAR(MAX)= 'SELECT TOP (1) @Id = [ID] 
                                                                FROM  [CorpProp.Base].[DictObject] do
                                                                WHERE  do.[Code] = @Code AND do.[Hidden] = 0';
  
                  DECLARE @ParmDefinition NVARCHAR(MAX) = N'@Id INT OUTPUT, @Code NVARCHAR(MAX)';
  
                  EXECUTE sys.sp_executesql 
                      @GetDictObjectByCode, 
                      @ParmDefinition, 
                      @Code = @State, 
                      @Id = @DictObjectStateId  OUTPUT;
  
                  EXECUTE sys.sp_executesql 
                      @GetDictObjectByCode,
                      @ParmDefinition, 
                      @Code = @Status, 
                      @Id = @DictObjectStatusId OUTPUT;
      
                  EXECUTE ('  
                          UPDATE do
                          SET do.DictObjectStateID = ' + @DictObjectStateId +',
                              do.DictObjectStatusID = ' +  @DictObjectStatusId + '
              
                          FROM [CorpProp.Base].DictObject do
        
                          INNER JOIN ' + @TableName + ' o ON do.ID = o.ID
                          WHERE (do.ImportUpdateDate < CONVERT (date, GETDATE()) 
                                            OR do.ImportUpdateDate IS NULL) 
                                         AND do.Hidden = 0 ');

                  SET @result= 0;
                END TRY

                BEGIN CATCH
                    SET @result = 1;
                END CATCH";

                ExecuteSqlQuery(sql);
            }

            private static void ExecuteSqlQuery(string sql)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DataContext"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
        }
    }
