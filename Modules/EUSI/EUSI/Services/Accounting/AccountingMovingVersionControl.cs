using Base.DAL;
using CorpProp.Common;
using CorpProp.Entities.Import;
using CorpProp.Helpers.Import.Extentions;
using EUSI.Entities.Accounting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUSI.Services.Accounting
{
    /// <summary>
    /// Представляет управление историей и версионностью регистров движения.
    /// </summary>
    public class AccountingMovingVersionControl<T> : BaseVersionControl<T> where T: AccountingMoving
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса AccountingMovingVersionControl.
        /// </summary>
        public AccountingMovingVersionControl(
            IUnitOfWork _uow
            , DataTable _table
            , Dictionary<string, string> _colsNameMapping
            , DateTime _period
            , ref ImportHistory history
            ) : base(_uow, _table, _colsNameMapping, _period, ref history)
        {

        }

        /// <summary>
        /// Переопредлеяет алгоритм выполнения контроля версий.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="obj"></param>
        /// <param name="history"></param>
        public override void Execute(DataRow row, ref T obj, ref ImportHistory history)
        {                       
            if (obj.ID == 0)
            {
                Fill(row, ref obj, ref history);
                obj.ActualDate = StartPeriod;
                obj.NonActualDate = EndPeriod;
            }
            else            
                CreateNewVersion(row, ref obj, ref history);  
        }
                

        /// <summary>
        /// Переопределяет логику создания новой версии.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="obj"></param>
        /// <param name="history"></param>
        protected override void CreateNewVersion(DataRow row, ref T obj, ref ImportHistory history)
        {
            try
            {
                //откладываем текущую запись в историю
                var clone = Uow.GetRepository<T>().GetOriginal(obj.ID);
                clone.ID = 0;
                if (clone.NonActualDate == null)                
                    clone.NonActualDate = EndPeriod;  
                clone.IsHistory = true;
                Uow.GetRepository<T>().Create(clone);
                              

                //обновляем текущую запись
                Fill(row, ref obj, ref history);
                obj.ActualDate = StartPeriod;
                obj.NonActualDate = EndPeriod;
                Uow.GetRepository<T>().Update(obj);
            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
            }
        }


    }
}
