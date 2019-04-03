using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace EUSI.Services.Estate
{
    public class UpdateTaxBaseCadastralObjectsServise
    {
        private const string ConnectionStringName = "DataContext";
        private string connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;

        public void Update(int year)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                string sql = GetCommandText(year);
                SqlCommand command = new SqlCommand(sql, conn);
                command.CommandTimeout = 60000;
                command.CommandType = CommandType.Text;

                conn.Open();
                var reader = command.ExecuteReader();
                do
                {
                    reader.Read();
                    var resultName = reader.GetName(0);
                    switch (resultName)
                    {
                        case "errText":
                            {
                                if (reader.HasRows)
                                {
                                    var errText = Convert.ToString(reader[0]);
                                    var errResultCode = Convert.ToString(reader[1]);
                                    var errCode = Convert.ToString(reader[2]);
                                }
                                break;
                            }
                    }
                } while (reader.NextResult());
                reader.Close();
                conn.Close();
            }
        }

        public string GetCommandText(int year)
        {
            var script = new StringBuilder();

            //Начало транзакции
            script.AppendLine($"BEGIN TRANSACTION BEGIN TRY");
            script.AppendLine($"DECLARE @dateFilter INT = {year}");
            script.AppendLine(@"DECLARE @TaxBaseIDCad int, @TaxBaseIDNoCad int

                                SELECT @TaxBaseIDCad = d.id FROM [CorpProp.Base].dictobject d INNER JOIN [CorpProp.NSI].taxbase tb ON d.id = tb.id WHERE code = 102
                                SELECT @TaxBaseIDNoCad = d.id FROM [CorpProp.Base].dictobject d INNER JOIN [CorpProp.NSI].taxbase tb ON d.id = tb.id WHERE code = 101

                                DECLARE @CheckUpdateTableTaxes table (ID int)
                                DECLARE @TaxBaseCadastral TABLE (id INT, cadastralnumber  NVARCHAR(max), roomcadastralnumber  NVARCHAR(max), ApprovingDocNumber NVARCHAR(max), ApprovingDocDate DATETIME)
                                --отбираем записи справочника согласно условиям
                                INSERT INTO @TaxBaseCadastral (id, cadastralnumber, roomcadastralnumber, ApprovingDocNumber, ApprovingDocDate)
                                SELECT plc.id, plc.cadastralnumber, plc.roomcadastralnumber, plc.ApprovingDocNumber, plc.ApprovingDocDate
                                FROM [corpprop.base].dictobject d
                                INNER JOIN [EUSI.NSI].propertylisttaxbasecadastral plc ON d.id = plc.id
                                INNER JOIN [CorpProp.Base].dictobject status ON d.dictobjectstatusid = status.id
                                INNER JOIN [CorpProp.Base].dictobject state ON d.dictobjectstateid = state.id
                                WHERE Datepart(year, d.datefrom) = @dateFilter
                                AND Datepart(year, d.dateto) = @dateFilter
                                AND status.code = N'AddConfirm'
                                AND state.code = N'NotOld'
                                AND d.ishistory = 0
                                AND d.hidden = 0
                                AND (Isnull(Ltrim(Rtrim(cadastralnumber)), '') != '' OR Isnull(Ltrim(Rtrim(roomcadastralnumber)), '') != '')

                                --Заполняем таблицу соответствия
                                DECLARE @mapping TABLE ( cadastralid INT, estatetaxsesID int, taxbasecadastralid INT)

                                INSERT INTO @mapping (cadastralid, estatetaxsesID, taxbasecadastralid)

                                select cad.ID, tax.ID, tplc.ID
                                from [CorpProp.Estate].Cadastral as cad
                                left join @TaxBaseCadastral as tplc on  cad.CadastralNumber = tplc.CadastralNumber or  cad.CadastralNumber = tplc.RoomCadastralNumber
                                left join [CorpProp.Estate].Estate as est on cad.ID=est.ID
                                left join [CorpProp.Estate].EstateTaxes as tax on est.ID=tax.TaxesOfID
                                where 
                                (Isnull(Ltrim(Rtrim(cad.CadastralNumber)), '') != '')
                                and est.Hidden=0 and Datepart(year, est.ActualDate) = @dateFilter

                                --Соответствия найдены
                                --обновляем EstateTaxes
                                UPDATE tax SET
                                tax.TaxCadastralIncludeDate = tbc.ApprovingDocDate,
                                tax.TaxCadastralIncludeDoc = tbc.ApprovingDocNumber,
                                tax.TaxBaseID = @TaxBaseIDCad  --Кадастровая стоимость
                                output inserted.ID into @CheckUpdateTableTaxes
                                FROM [CorpProp.Estate].estatetaxes as tax
                                INNER JOIN @mapping m ON tax.ID = m.estatetaxsesID
                                INNER JOIN @TaxBaseCadastral tbc ON m.taxbasecadastralid = tbc.id


                                --создаем EstateTaxes, если нет
                                INSERT INTO[CorpProp.Estate].estatetaxes (taxesofid, taxcadastralincludedate, taxcadastralincludedoc, taxbaseid, hidden, sortorder)
                                output inserted.id into @CheckUpdateTableTaxes
                                SELECT cad.id, tbc.ApprovingDocDate, tbc.ApprovingDocNumber, 
                                TaxBaseID = @TaxBaseIDCad,
                                Hidden = 0,
                                SortOrder = -1

                                FROM [CorpProp.Estate].cadastral cad
                                INNER JOIN @mapping m ON cad.id = m.cadastralid
                                INNER JOIN @TaxBaseCadastral tbc ON m.taxbasecadastralid = tbc.id
                                LEFT JOIN [CorpProp.Estate].estatetaxes et ON cad.id=et.taxesofid
                                WHERE et.id IS NULL

                                --Соответствия не найдены
                                UPDATE et
                                SET    TaxCadastralIncludeDate = NULL,
                                TaxCadastralIncludeDoc = NULL,
                                TaxBaseID = @TaxBaseIDNoCad
                                --Среднегодовая стоимость
                                FROM [CorpProp.Estate].cadastral cad
                                INNER JOIN @mapping m ON cad.id = m.cadastralid
                                INNER JOIN [CorpProp.Estate].estatetaxes et ON et.taxesofid = cad.id
                                WHERE m.taxbasecadastralid IS NULL

                                INSERT INTO[CorpProp.Estate].estatetaxes (taxesofid, taxcadastralincludedate, taxcadastralincludedoc, taxbaseid, hidden, sortorder)
                                SELECT cad.id, NULL, NULL, 
                                TaxBaseID = @TaxBaseIDNoCad,
                                Hidden = 0,
                                SortOrder = -1
                                FROM [CorpProp.Estate].cadastral cad
                                INNER JOIN @mapping m ON cad.id = m.cadastralid
                                LEFT JOIN [CorpProp.Estate].estatetaxes et ON et.taxesofid = cad.id
                                WHERE m.taxbasecadastralid IS NULL
                                AND et.id IS NULL

                                UPDATE plc
                                SET IsCadastralEstateUpdated = 0,
                                CadastralEstateUpdatedDate = NULL
                                from [EUSI.NSI].PropertyListTaxBaseCadastral as plc
                                inner join @TaxBaseCadastral as tbc on plc.ID=tbc.id
                                left join @mapping as m on plc.ID=m.taxbasecadastralid
                                left join [CorpProp.Estate].Cadastral as cad on m.cadastralid=cad.ID
                                left join [CorpProp.Estate].EstateTaxes as tax on cad.ID=tax.TaxesOfID
                                left join @CheckUpdateTableTaxes as t on tax.ID=t.ID
                                where t.ID is null

                                update plc
                                SET IsCadastralEstateUpdated = 1,
                                CadastralEstateUpdatedDate = GETDATE()
                                from [EUSI.NSI].PropertyListTaxBaseCadastral as plc
                                inner join @TaxBaseCadastral as tbc on plc.ID=tbc.id
                                left join @mapping as m on plc.ID=m.taxbasecadastralid
                                left join [CorpProp.Estate].Cadastral as cad on m.cadastralid=cad.ID
                                left join [CorpProp.Estate].EstateTaxes as tax on cad.ID=tax.TaxesOfID
                                left join @CheckUpdateTableTaxes as t on tax.ID=t.ID
                                where t.ID is not null");
            
            //TODO: Создание историчности с делением
            script.AppendLine($"/*TODO: Создание историчности с делением*/");

            //Окончание транзакции
            script.AppendLine(@"COMMIT 
                    Select 'COMMIT END' as 'Result' 
                    END TRY
                    BEGIN CATCH
                    declare @strErr nvarchar(max)='' 
                    set @strErr = cast(ERROR_MESSAGE() as nvarchar(max)) 
                    SELECT
                     cast(ERROR_MESSAGE() as nvarchar(max)) as 'errText'
                    , N'ERR_NsiQueryBulk' as 'errResultCode'
                    , N'SQL_' + cast(ERROR_NUMBER() as nvarchar(max)) AS 'errCode' ;
                    ROLLBACK
                    RAISERROR(@strErr, 10, 1)
                    END CATCH");
         

            return script.ToString();
        }
    }
}