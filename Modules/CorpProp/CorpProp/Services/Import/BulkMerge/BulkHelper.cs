using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Services.Import.BulkMerge
{
    public class BulkHelper
    {
        /// <summary>
        ///  Возвращает текст sql-скрипта для создания новых объектов имущества.
        /// </summary>
        /// <param name="tblTypeEstateMap">Временная таблица, содержащая ОИ, которые необходимо создать (таблица мэппинга источника импортируемых данных, ОИ и создаваемого типа).</param>
        /// <param name="varActualDate">Дата актуальности создаваемых ОИ. Наименование переменной или дата.</param>
        /// <returns></returns>
        /// <remarks>Таблица tblTypeEstateMap должна содержать обязательные колонки: oidEstate UNIQUEIDENTIFIER, typeEstate NVARCHAR(255).</remarks>
        public string CreateEstateScript(string tblTypeEstateMap, string varActualDate)
        {
            var script = new StringBuilder();
            //Временные таблицы
            script.AppendLine($"CREATE TABLE #tblTypeEstate (TypeName nvarchar(255))");
            script.AppendLine($"create table #tblReferencedEstate (Position int, RefEstateTableName nvarchar(500), TableName nvarchar(500), COLUMN_Position nvarchar(500), COLUMN_NAME nvarchar(500), DATA_TYPE nvarchar(500), DefaulValue nvarchar(500))");
            script.AppendLine($"CREATE TABLE #tblCreateEstate (SortOrder int, RefEstateTableName nvarchar(500),Position int,  TableName nvarchar(500), StrScript nvarchar(MAX))");
            script.AppendLine($"CREATE TABLE #tblShemEstate (TableName nvarchar(500), ReferencedTableName nvarchar(500))");            

            //собираем информацию о типах ОИ
            script.AppendLine($"insert into #tblTypeEstate (typeName) select typeEstate FROM {tblTypeEstateMap} group by typeEstate");
            //Сбор данных о наследниках Estate
            script.AppendLine($"INSERT INTO #tblShemEstate (TableName, ReferencedTableName)");
            script.AppendLine($"SELECT");
            script.AppendLine($"OBJECT_NAME(f.parent_object_id) AS TableName,");
            script.AppendLine($"OBJECT_NAME(f.referenced_object_id) AS ReferenceTableName");
            script.AppendLine($"FROM    sys.foreign_keys AS f");
            script.AppendLine($"INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id");
            script.AppendLine($"INNER JOIN sys.objects AS o ON o.OBJECT_ID = fc.referenced_object_id");
            script.AppendLine($"WHERE SCHEMA_NAME(f.SCHEMA_ID) = 'CorpProp.Estate' and COL_NAME(fc.parent_object_id, fc.parent_column_id) = 'ID' and SCHEMA_NAME(o.SCHEMA_ID) = 'CorpProp.Estate' ORDER BY ReferenceTableName");
            //
            script.AppendLine($"declare @RefTableName nvarchar(500) ,@RefEstateTableName nvarchar(500) , @endTableName  nvarchar(500)='Estate'");
            script.AppendLine($"declare @tPosition int = 1");
            //Курсор формирования скриптов создания ОИ от Estate
            script.AppendLine($"DECLARE @TypeEstate nvarchar(255)");
            script.AppendLine($"DECLARE typeEstate_cur CURSOR LOCAL FOR");
            script.AppendLine($"SELECT typeName FROM  #tblTypeEstate");
            script.AppendLine($"OPEN typeEstate_cur");
            script.AppendLine($"FETCH NEXT FROM typeEstate_cur INTO @TypeEstate");
            script.AppendLine($"WHILE @@FETCH_STATUS = 0");
            script.AppendLine($"BEGIN");
            script.AppendLine($"set @RefTableName = @TypeEstate");
            script.AppendLine($"set @RefEstateTableName = @RefTableName");
            script.AppendLine($"while (isnull(@RefTableName, '') <> '') ");
            script.AppendLine($"Begin");
            script.AppendLine($"INSERT INTO #tblReferencedEstate(Position, RefEstateTableName, TableName,COLUMN_Position,COLUMN_NAME, DATA_TYPE, DefaulValue)");
            script.AppendLine($"SELECT @tPosition, @RefEstateTableName, @RefTableName");
            script.AppendLine($",case ");
            script.AppendLine($"when COLUMN_NAME = 'ID' then '01'");
            script.AppendLine($"when COLUMN_NAME = 'OID' then '02'");
            script.AppendLine($"else '1' + cast(ROW_NUMBER() OVER(ORDER BY COLUMN_NAME ASC) AS nvarchar(255))");
            script.AppendLine($"end");
            script.AppendLine($"AS COLUMN_Position");
            script.AppendLine($", COLUMN_NAME, DATA_TYPE");
            script.AppendLine($",case ");
            script.AppendLine($"when COLUMN_NAME = 'ID' then 'est.id'");
            script.AppendLine($"when DATA_TYPE = 'int' then '0'");
            script.AppendLine($"when DATA_TYPE = 'float' then '0'");
            script.AppendLine($"when DATA_TYPE = 'bit' then '0'");
            script.AppendLine($"when DATA_TYPE = 'uniqueidentifier' then 'NEWID()'");
            script.AppendLine($"end AS defaulValue");
            script.AppendLine($"FROM INFORMATION_SCHEMA.columns");
            script.AppendLine($"WHERE TABLE_SCHEMA = 'CorpProp.Estate' and TABLE_NAME = @RefTableName and IS_NULLABLE = 'NO' and COLUMN_NAME not in ('RowVersion')");
            script.AppendLine($"and TABLE_NAME<>'Estate'");
            script.AppendLine($"order by  COLUMN_NAME");
            script.AppendLine($"if (isnull(@RefTableName, '') <> 'Estate') ");
            script.AppendLine($"SELECT @RefTableName = ReferencedTableName, @tPosition = @tPosition + 1 FROM #tblShemEstate WHERE TableName=@RefTableName");
            script.AppendLine($"else");
            script.AppendLine($"set @RefTableName = NULL");
            script.AppendLine($"End");
            script.AppendLine($"INSERT INTO #tblCreateEstate (SortOrder,RefEstateTableName,Position, TableName, StrScript)");
            script.AppendLine($"SELECT(ROW_NUMBER() OVER(ORDER BY tmp.RefEstateTableName, tmp.Position desc)), tmp.RefEstateTableName ,  tmp.Position, tmp.TableName, tmp3.str3Script");
            script.AppendLine($"FROM #tblReferencedEstate AS tmp with (nolock)");
            script.AppendLine($"left join (");
            script.AppendLine($"SELECT tmp2.Position, tmp2.RefEstateTableName, 'INSERT INTO [CorpProp.Estate].' + tmp2.TableName");
            script.AppendLine($"+ ' (' + ");
            script.AppendLine($"STUFF((SELECT case when ccm.TableName = 'Estate' and ccm.COLUMN_NAME = 'ID' then '' else ', ' + ccm.COLUMN_NAME end + ''");
            script.AppendLine($"FROM #tblReferencedEstate AS ccm with (nolock)");
            script.AppendLine($"WHERE ccm.Position = tmp2.Position and ccm.RefEstateTableName = tmp2.RefEstateTableName");
            script.AppendLine($"order by ccm.COLUMN_Position FOR XML PATH('')),1,1,'')");
            script.AppendLine($"+ ') SELECT ' + ");
            script.AppendLine($"STUFF((SELECT case when ccm.TableName = 'Estate' and ccm.COLUMN_NAME = 'ID' then '' else ',' + ccm.DefaulValue end + ' '");
            script.AppendLine($"FROM #tblReferencedEstate AS ccm with (nolock)");
            script.AppendLine($"WHERE ccm.Position = tmp2.Position  and ccm.RefEstateTableName = tmp2.RefEstateTableName");
            script.AppendLine($"order by ccm.COLUMN_Position FOR XML PATH('')),1,1,'')");
            script.AppendLine($"+ ' FROM {tblTypeEstateMap} AS ao left outer join [CorpProp.Estate].[Estate] AS est on ao.oidEstate=est.oid left join [CorpProp.Estate].'");
            script.AppendLine($"+ tmp2.TableName + ' AS obj on est.ID=obj.ID  WHERE ao.typeEstate = ''' + tmp2.RefEstateTableName + ''' and est.oid is not null and obj.id is null'");
            script.AppendLine($"AS str3Script");
            script.AppendLine($"FROM #tblReferencedEstate AS tmp2 with (nolock)) AS tmp3 on tmp.Position=tmp3.Position and tmp.RefEstateTableName=tmp3.RefEstateTableName");
            script.AppendLine($"left join {tblTypeEstateMap} AS aoBulk on tmp.RefEstateTableName=aoBulk.typeEstate");
            script.AppendLine($"group by tmp.Position,tmp.RefEstateTableName, tmp.TableName, tmp3.str3Script");
            script.AppendLine($"order by tmp.RefEstateTableName,tmp.Position desc");
            script.AppendLine($"delete FROM #tblReferencedEstate");
            script.AppendLine($"FETCH NEXT FROM typeEstate_cur INTO @TypeEstate");
            script.AppendLine($"END");
            script.AppendLine($"CLOSE typeEstate_cur");
            script.AppendLine($"DEALLOCATE typeEstate_cur");

            script.AppendLine($"DROP TABLE #tblTypeEstate");
            script.AppendLine($"DROP TABLE #tblReferencedEstate");
            script.AppendLine($"DROP TABLE #tblShemEstate");


            //Создание Estate для ОС
            script.AppendLine($"declare @ExecuteDate datetime = getdate()");
            script.AppendLine($"INSERT INTO [CorpProp.Estate].[Estate] (Oid, EstateStatus, Hidden, IsHistory, IsNonCoreAsset, OutOfBalance, SortOrder, [CreateDate],[ActualDate],[NonActualDate],[ImportUPDATEDate],[ImportDate])");
            script.AppendLine($"SELECT ao.oidEstate, 0, 0, 0, 0, 0, 0, @ExecuteDate, {varActualDate}, NULL, @ExecuteDate, @ExecuteDate");
            script.AppendLine($"FROM {tblTypeEstateMap} AS ao  with (nolock) ");
            script.AppendLine($"left outer join [CorpProp.Estate].[Estate] AS est with(nolock) on ao.oidEstate = est.oid");
            script.AppendLine($"WHERE ao.typeEstate is not null and est.oid is null");

            //Курсор для выполнения сформированных скриптов создания ОИ от Estate
            script.AppendLine($"DECLARE @StrScript nvarchar(max)");
            script.AppendLine($"DECLARE CreateEstate_cur CURSOR LOCAL FOR");
            script.AppendLine($"SELECT StrScript FROM  #tblCreateEstate");
            script.AppendLine($"order by RefEstateTableName, SortOrder");
            script.AppendLine($"OPEN CreateEstate_cur");
            script.AppendLine($"FETCH NEXT FROM CreateEstate_cur INTO @StrScript");
            script.AppendLine($"WHILE @@FETCH_STATUS = 0");
            script.AppendLine($"BEGIN");
            script.AppendLine($"execute(@StrScript)");
            script.AppendLine($"FETCH NEXT FROM CreateEstate_cur INTO @StrScript");
            script.AppendLine($"END");
            script.AppendLine($"CLOSE CreateEstate_cur");
            script.AppendLine($"DEALLOCATE CreateEstate_cur");


            script.AppendLine($"DROP TABLE #tblCreateEstate");
            script.AppendLine();

            return script.ToString();
        }


        
    }
}
