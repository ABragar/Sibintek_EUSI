using Base.DAL;
using Base.Service;
using CorpProp.Common;
using CorpProp.Entities.Import;
using CorpProp.Entities.Mapping;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Services.Mapping
{
    
    
    public interface IAccountingEstatesService : IBaseObjectService<AccountingEstates>, IExcelImportEntity, ISystemImportEntity
    {

    }
  
    public class AccountingEstatesService : BaseObjectService<AccountingEstates>, IAccountingEstatesService
    {

        
        public AccountingEstatesService(IBaseObjectServiceFacade facade) : base(facade)
        {


        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>Судно.</returns>
        public override AccountingEstates Create(IUnitOfWork unitOfWork, AccountingEstates obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<AccountingEstates> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<AccountingEstates> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.ClassFixedAsset)
                    ;
        }

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

                for (int r = 9; r < table.Rows.Count; r++)
                {
                    var row = table.Rows[r];
                    string err = "";
                    var obj = new AccountingEstates();
                    obj.FillObject(uofw, typeof(AccountingEstates),
                               row, row.Table.Columns, ref err, ref history, colsNameMapping, IdentyDictByCode());
                    if (obj.ID != 0)
                        uofw.GetRepository<AccountingEstates>().Update(obj);
                    else
                        uofw.GetRepository<AccountingEstates>().Create(obj);
                    uofw.SaveChanges();
                    count++;
                }
            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
            }
        }

        /// <summary>
        /// Идентификация справочников по коду.
        /// </summary>
        /// <returns></returns>
        public bool IdentyDictByCode()
        {
            return true;
        }

        public void CancelImport(
             IUnitOfWork uofw
            , ref ImportHistory history
            )
        {
            throw new NotImplementedException();
        }
    }
}
