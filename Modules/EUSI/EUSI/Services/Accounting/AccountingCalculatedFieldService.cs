using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;
using Base.Service;
using CorpProp.Common;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Import;
using CorpProp.Entities.NSI;
using CorpProp.Extentions;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using EUSI.Entities.Accounting;

namespace EUSI.Services.Accounting
{
    public interface IAccountingCalculatedFieldService : IBaseObjectService<AccountingCalculatedField>, IExcelImportEntity
    {

    }

    public class AccountingCalculatedFieldService : BaseObjectService<AccountingCalculatedField>,
        IAccountingCalculatedFieldService
    {
        public AccountingCalculatedFieldService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        protected override IObjectSaver<AccountingCalculatedField> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<AccountingCalculatedField> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver);
        }

        /// <inheritdoc />
        public void Import(
            IUnitOfWork uofw
            , IUnitOfWork histUofw
            , DataTable table
            , Dictionary<string, string> colsNameMapping
            , ref int count
            , ref ImportHistory history)
        {
            try
            {
                string err = "";
                //пропускаем первые 9 строк файла не считая строки названия колонок.
                int start = ImportHelper.GetRowStartIndex(table);
                      
                for (int i = start; i < table.Rows.Count; i++)
                {
                    var row = table.Rows[i];
                    ImportObject(uofw, row, colsNameMapping, ref err, ref count, ref history);
                    count++;
                }
            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
            }
        }     

        /// <summary>
        /// Имопртирует из строки файла.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="row">Строка файла.</param>
        /// <param name="colsNameMapping">Мэппинг колонок.</param>
        /// <param name="error">Текст ошибки.</param>
        /// <param name="count">Количество импортированных объектов.</param>
        /// <param name="history">Лог импорта.</param>
        public void ImportObject(
            IUnitOfWork uofw
            , DataRow row
            , Dictionary<string, string> colsNameMapping          
            , ref string error
            , ref int count
            , ref ImportHistory history)
        {
            try
            {
                const string calcDatasource = "НА";
                bool isNew = true;
                AccountingCalculatedField accountingCalculatedField = null;
                var inventoryNumber = ImportHelper.GetValueByName(uofw, typeof(string), row, "InventoryNumber", colsNameMapping);
                var consolidation = ImportHelper.GetValueByName(uofw, typeof(string), row, "Consolidation", colsNameMapping);
                var oktmo = ImportHelper.GetValueByName(uofw, typeof(string), row, "OKTMO", colsNameMapping);
                var year = ImportHelper.GetValueByName(uofw, typeof(int), row, "year", colsNameMapping);

                if (inventoryNumber != null && consolidation != null)
                {
                    List<AccountingCalculatedField> accountingCalculatedFields = FindObjects(uofw, calcDatasource, inventoryNumber.ToString(), int.Parse(year.ToString()));
                    

                    if (accountingCalculatedFields.Count > 1)
                    {
                        error += $"Невозможно обновить объект. В Системе найдено более одной записи.{System.Environment.NewLine}";
                        history.ImportErrorLogs.AddError(row.Table.Rows.IndexOf(row), 0, "", error, ErrorType.System);
                        return;
                    }

                    if (accountingCalculatedFields.Count == 0)
                    {
                        accountingCalculatedField = new AccountingCalculatedField();
                    }

                    if (accountingCalculatedFields.Count == 1)
                    {
                        isNew = false;
                        accountingCalculatedField = accountingCalculatedFields[0];
                    }

                    accountingCalculatedField.FillObject(uofw, typeof(AccountingCalculatedField),
                        row, row.Table.Columns, ref error, ref history, colsNameMapping);
                    //accountingCalculatedField.ImportDate = DateTime.Now; 

                        //var start = ImportHelper.GetValueByName(uofw, typeof(DateTime), row, "Period.Start", colsNameMapping);
                        //var end = ImportHelper.GetValueByName(uofw, typeof(DateTime), row, "Period.End", colsNameMapping);

                        //if (!String.IsNullOrEmpty(start?.ToString()))
                        //    obj.Period.Start = (start.ToString().GetDate() == null) ? DateTime.Parse(start.ToString()) : start.ToString().GetDate().Value;
                        //else
                        //    obj.Period.Start = DateTime.Now;

                        //if (end != null && !String.IsNullOrEmpty(end.ToString()))
                        //    obj.Period.End = (end.ToString().GetDate() == null) ? DateTime.Parse(end.ToString()) : end.ToString().GetDate().Value;
                        //else
                        //    obj.Period.End = DateTime.Now;

                        //вид загрузки
                        //obj.LoadTypeID = GetLoadTypeID(uofw, history);
                        //номер еуси
                        //var eusinumb = ImportHelper.GetValueByName(uofw, typeof(string), row, "EUSINumber", colsNameMapping);
                        //obj.EUSI = (eusinumb != null) ? eusinumb.ToString() : "";

                        AccountingCalculatedField newAccountingCalculatedField = (isNew)? this.Create(uofw, accountingCalculatedField) : this.Update(uofw, accountingCalculatedField);
                }
                else
                {
                    error += $"Невозможно идентифицировать запись. {System.Environment.NewLine}";
                    history.ImportErrorLogs.AddError(row.Table.Rows.IndexOf(row), 0, "", error, ErrorType.System);
                    accountingCalculatedField = null;
                }
            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
            }

        }

        public List<AccountingCalculatedField> FindObjects(
            IUnitOfWork uofw
            , string calculationDatasource
            , string oktmo
            , int year)
        {
            List<AccountingCalculatedField> list = uofw.GetRepository<AccountingCalculatedField>()
            .Filter(x => !x.Hidden &&             
             x.Year == year &&
             String.Equals(x.CalculationDatasource, calculationDatasource, StringComparison.CurrentCultureIgnoreCase) &&
             String.Equals(x.OKTMO, oktmo, StringComparison.CurrentCultureIgnoreCase)
             )
             .ToList();
            
            return list;
        }

        public void CancelImport(IUnitOfWork uofw, ref ImportHistory history)
        {
            throw new NotImplementedException();
        }
    }
}
