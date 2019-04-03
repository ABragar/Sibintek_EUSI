using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Base.DAL;
using Base.Service;
using Base.Service.Log;
using CorpProp.Analyze.Entities.Accounting;
using CorpProp.Common;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Estate;
using CorpProp.Entities.Import;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.Services.Base;

namespace CorpProp.Analyze.Services.Accounting
{
    public interface IBankAccountService : ITypeObjectService<BankAccount>, IExcelImportEntity
    {
    }

    public class BankAccountService : TypeObjectService<BankAccount>, IBankAccountService
    {
        private readonly ILogService _logger;
        public BankAccountService(IBaseObjectServiceFacade facade, ILogService logger) : base(facade, logger)
        {
            _logger = logger;
        }

        protected override IObjectSaver<BankAccount> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<BankAccount> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneObject(account => account.Society)
                .SaveOneObject(account => account.Currency);
        }

        public void Import(IUnitOfWork uofw, IUnitOfWork histUofw, DataTable table,
            Dictionary<string, string> colsNameMapping, ref int count,
            ref ImportHistory history)
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


        public virtual List<BankAccount> FindObjects(IUnitOfWork uofw, string idEup)
        {
            string str = ImportHelper.GetIDEUP(idEup);
            List<BankAccount> list = new List<BankAccount>();
            list = uofw.GetRepository<BankAccount>().Filter(x =>
                !x.Hidden &&
                x.Society != null && x.Society.IDEUP == str).ToList<BankAccount>();
            return list;
        }

        /// <summary>
        /// Имопртирует ОБУ из строки файла.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="row">Строка файла.</param>      
        /// <param name="error">Текст ошибки.</param>
        /// <param name="count">Количество импортированных объектов.</param>
        public virtual void ImportObject(
            IUnitOfWork uofw
            , DataRow row
            , Dictionary<string, string> colsNameMapping
            , ref string error
            , ref int count
            , ref ImportHistory history)
        {
            try
            {
                bool isNew = true;
                BankAccount obj = null;
                
                obj = Activator.CreateInstance<BankAccount>();
                obj.FillObject(uofw, typeof(BankAccount), row, row.Table.Columns, ref error,
                    ref history, colsNameMapping);
                if (obj != null) obj.ImportDate = DateTime.Now;
                if (!String.IsNullOrEmpty(error))
                    obj = null;
                else
                {
                    if (obj != null)
                    {
                        this.CreateFromImport(uofw, obj, history);
                    }
                }
            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
            }
        }

        public BankAccount CreateFromImport(
            IUnitOfWork uow
            , BankAccount obj
            , ImportHistory history)
        {
            var bo = this.Create(uow, obj);
            uow.GetRepository<ImportObject>()
                .Create(new ImportObject(bo, history.Oid, TypeImportObject.CreateObject));

            uow.SaveChanges();
            return bo;
        }

        public void CancelImport(
            IUnitOfWork uow
            , ref ImportHistory history
        )
        {
            var hisID = history.Oid;
            //TODO: придется материализовать
            var imported = uow.GetRepository<ImportObject>()
                .Filter(f => !f.Hidden
                             && f.ImportHistoryOid == hisID
                             && f.Type == TypeImportObject.CreateObject)
                .ToList<ImportObject>();

            string err = "";
            int count = 0;
            foreach (var item in imported)
            {
                var obj = ImportHelper.GetBaseObject(uow, item.Entity.GetTypeBo(), item.Entity.ID, ref err);
                if (obj != null)
                {
                    ImportHelper.UpdateRepositoryObject(uow, item.Entity.GetTypeBo(), obj);
                    var pr = obj.GetType().GetProperty("Hidden");
                    if (pr != null)
                        pr.SetValue(obj, true);
                    item.Hidden = true;
                }

                count++;
            }

            history.ResultText = $"Импорт отменен. Обработано {count} объектов.";
            history.IsCanceled = true;
            uow.SaveChanges();
        }
    }
}