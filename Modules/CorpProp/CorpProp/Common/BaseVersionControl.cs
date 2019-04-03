using Base;
using Base.DAL;
using Base.Extensions;
using CorpProp.Entities.Base;
using CorpProp.Entities.Import;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Common
{
    /// <summary>
    /// Представляет базовую реализацию контроля версий историчных записей.
    /// </summary>
    public abstract class BaseVersionControl<T> where T : TypeObject
    {
        private IUnitOfWork uow;
        private DataTable table;
        private Dictionary<string, string> colsNameMapping;        

        /// <summary>
        /// Получает сессию.
        /// </summary>
        public IUnitOfWork Uow { get { return uow; } }

        /// <summary>
        /// Получает таблицу импортируемых данных.
        /// </summary>
        public DataTable Table { get { return table; } }

        /// <summary>
        /// Получает мэппинг колонок и свойств.
        /// </summary>
        public Dictionary<string, string> ColsNameMapping { get { return colsNameMapping; } }

        /// <summary>
        /// Получает или задает дату начала периода импорта данных.
        /// </summary>
        public DateTime StartPeriod { get; set; }

        /// <summary>
        /// Получает или задает дату окончания периода имопрта данных.
        /// </summary>
        public DateTime EndPeriod { get; set; }


        /// <summary>
        /// Инициализирует новый экземпляр класса BaseVersionControl.
        /// </summary>
        public BaseVersionControl(
            IUnitOfWork _uow
            , DataTable _table
            , Dictionary<string, string> _colsNameMapping
            , DateTime _period
            , ref ImportHistory history
            )
        {
            uow = _uow;
            table = _table;
            colsNameMapping = _colsNameMapping;
            StartPeriod = new DateTime(_period.Year, _period.Month, 1);
            EndPeriod = StartPeriod.AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// Выполняет контроль.
        /// </summary>
        /// <param name="row">Строка данных.</param>
        /// <param name="obj">Объект обновления.</param>
        /// <param name="history">История импорта.</param>
        public virtual void Execute(DataRow row, ref T obj, ref ImportHistory history)
        {
            if (obj.ID == 0)
            {
                Fill(row, ref obj, ref history);
                obj.ActualDate = StartPeriod;
                obj.NonActualDate = EndPeriod;
            }
            else if (obj.ActualDate == StartPeriod)
                UpdateVersion(row, ref obj, ref history);
            else if (obj.ActualDate < StartPeriod)
                CreateNewVersion(row, ref obj, ref history);
            else if (obj.ActualDate > StartPeriod)
                CreateOldVersion(row, ref obj, ref history);
            return;
        }

        /// <summary>
        /// Обновляет текущий экземпляр объекта.
        /// </summary>
        /// <param name="row">Строка таблицы.</param>
        /// <param name="obj">Объект обновления.</param>
        /// <param name="history">История импорта.</param>
        protected virtual void UpdateVersion(
             DataRow row
           , ref T obj
           , ref ImportHistory history
           )
        {
            try
            {
                Fill(row, ref obj, ref history);
                obj.ActualDate = StartPeriod;
                obj.NonActualDate = EndPeriod;
                uow.GetRepository<T>().Update(obj);
            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
            }
        }

        /// <summary>
        /// Создает новую версию историчности.
        /// </summary>
        /// <param name="row">Сессия.</param>
        /// <param name="obj">Актуальная запись объекта.</param>
        /// <param name="history">История импорта.</param>
        protected virtual void CreateNewVersion(
              DataRow row
            , ref T obj
            , ref ImportHistory history)
        {
            try
            {
                //откладываем текущую запись в историю
                var clone = uow.GetRepository<T>().GetOriginal(obj.ID);
                clone.ID = 0;
                if (clone.NonActualDate == null)
                    clone.NonActualDate = StartPeriod.AddDays(-1);
                clone.IsHistory = true;
                uow.GetRepository<T>().Create(clone);

                //обновляем текущую запись
                Fill(row, ref obj, ref history);
                obj.ActualDate = StartPeriod;
                obj.NonActualDate = EndPeriod;
                uow.GetRepository<T>().Update(obj);
            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
            }
        }

        /// <summary>
        /// Обновляет объект в соответствии с данными строки таблицы.
        /// </summary>
        /// <param name="row">Строка таблицы.</param>
        /// <param name="obj">Объект обновления.</param>
        /// <param name="history">История импорта.</param>
        /// <returns></returns>
        protected virtual void Fill(
              DataRow row
            , ref T obj
            , ref ImportHistory history)
        {
            try
            {
                var err = "";
                obj.FillObject(uow, typeof(T), row, row.Table.Columns, ref err, ref history, colsNameMapping);
                obj.ImportDate = DateTime.Now;
                obj.ImportUpdateDate = DateTime.Now;

            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
            }
            return;
        }


        /// <summary>
        /// Создает историчную запись об объекте "задним числом".
        /// </summary>
        /// <param name="row">Строка таблицы.</param>
        /// <param name="obj">Актуальная запись ОБУ.</param>
        /// <param name="history">История импорта.</param>
        protected virtual void CreateOldVersion(
              DataRow row
            , ref T obj
            , ref ImportHistory history
            )
        {
            try
            {
                //найти историчную запись за период
                var oid = obj.Oid;
                var vers = uow.GetRepository<T>()
                    .Filter(f => !f.Hidden && f.Oid == oid && f.ActualDate == StartPeriod)                   
                    .FirstOrDefault();

                if (vers != null)
                {
                    UpdateVersion(row, ref vers, ref history);
                    obj = vers;
                    return;
                }

                //создать историчную запись задним числом.
                var newObj = Activator.CreateInstance<T>();
                newObj.Oid = oid;
                newObj.IsHistory = true;
                Fill(row, ref newObj, ref history);
                newObj.ActualDate = StartPeriod;
                newObj.NonActualDate = EndPeriod;
                obj = uow.GetRepository<T>().Create(newObj);
            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
            }
        }
    }
}
