using Base;
using Base.Attributes;
using DAL = Base.DAL;
using CorpProp.Entities.History;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;
using System.Data.Entity.Infrastructure;
using System.Reflection;
using CorpProp.Helpers;

namespace CorpProp.Entities.Base
{
    /// <summary>
    /// Предоставляет методы и сведения о свойствах указанного объекта Системы.
    /// </summary>
    public interface ITypeObject : IBaseObject, IHistoryObject
    {
        System.Guid Oid { get; set; }

        DateTime? CreateDate { get; set; }
    }

    /// <summary>
    /// Представляет базовый объект Системы.  
    /// </summary>    
    public abstract class TypeObject : BaseObject, ITypeObject
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса TypeObject.
        /// </summary>
        protected TypeObject() : base()
        {
            Init();
        }


        protected virtual Guid GetGuid()
        {
            return System.Guid.NewGuid();
        }

        /// <summary>
        /// Инициализация.
        /// </summary>
        void Init()
        {
            CreateDate = DateTime.Now;
            ActualDate = DateTime.Now.Date;
            Oid = GetGuid();
            IsHistory = false;
        }

        /// <summary>
        /// Получает или задает уникальный ИД объекта.
        /// </summary>
        [SystemProperty]
        public System.Guid Oid { get; set; }

        [SystemProperty]
        [DefaultValue(false)]
        public bool IsHistory { get; set; }

        ///// <summary>
        ///// Получает или задает дату создания объекта.
        ///// </summary>
        [SystemProperty]
        public DateTime? CreateDate { get; set; }

        ///// <summary>
        ///// Получает или задает дату начала действия историчности записи.
        ///// </summary>
        [SystemProperty]
        [PropertyDataType(PropertyDataType.Date)]
        public DateTime? ActualDate { get; set; }

        /// <summary>
        /// Получает или задает дату окончания действия историчности записи.
        /// </summary>
        [SystemProperty]
        [PropertyDataType(PropertyDataType.Date)]
        public DateTime? NonActualDate { get; set; }

        ///// <summary>
        ///// Получает или задает дату обновления объекта при импорте.
        ///// </summary>
        public DateTime? ImportUpdateDate { get; set; }

        ///// <summary>
        ///// Получает или задает дату импорта.
        ///// </summary>
        public DateTime? ImportDate { get; set; }


        /// <summary>
        /// Метод, исполняемый перед сохранением сесии экземпляра объекта.
        /// </summary>
        /// <param name="uow">Сессия, в которую добавлен экземпляр.</param>
        /// <param name="entry">Запись объекта типа DbEntityEntry.</param>
        /// <remarks>
        /// А-ля OnSaving без события.
        /// Данный метод вызывается у экземпляра объекта при сохранении сессии (см.UnitOfWork.SaveChanges).        
        /// НЕ СОХРАНЯЙТЕ СЕССИЮ - IUnitOfWork uow !!!
        /// </remarks>
        /// <see cref="DAL.Internal.UnitOfWork"/>
        /// <see cref="DbEntityEntry"/>
        public virtual void OnSaving(IUnitOfWork uow, object entry)
        {
            if (ID == 0 || IsHistory || (ActualDate != null && ActualDate.Value.Date == DateTime.Now.Date)) return;
            InitHistory(uow);
            //НЕ СОХРАНЯЙТЕ СЕССИЮ - IUnitOfWork uow !!!
        }

        /// <summary>
        /// Инициирует историчность объекта.
        /// </summary>
        /// <param name="uow">Сессия.</param>
        public virtual void InitHistory(IUnitOfWork uow)
        {
            List<string> tNames = TypesHelper.GetAllTypeNames(this.GetType().GetBaseObjectType())
                .Split(';')
                .ToList<string>();

            bool enableHistory = uow.GetRepository<HistoricalSettings>()
                .Filter(x => !x.Hidden && tNames.Contains(x.TypeName.ToLower()))
                .Any();
            if (enableHistory)
            {
                if (HistoryHelper.GetCurrHistory(uow, this.GetType().GetBaseObjectType(), this) != null) return;

                List<string> historySettings = uow.GetRepository<HistoricalSettings>()
                .Filter(x => !x.Hidden && tNames.Contains(x.TypeName.ToLower()))
                .Select(s => s.Propertys).ToList();

                SetCurrentHistory(uow, historySettings);
            }
        }

        /// <summary>
        /// Задает историчную запись объекта.
        /// </summary>
        /// <param name="uow">Сессия.</param>
        protected virtual void SetCurrentHistory(IUnitOfWork uow, List<string> historyPropertys)
        {
            if (historyPropertys == null || uow == null) return;

            try
            {
                Type tt = this.GetType().GetBaseObjectType();
                var original = HistoryHelper.GetOriginalObject(uow, tt, ID) as TypeObject;
                if (original == null) return;
                foreach (var props in historyPropertys)
                {
                    foreach (var prop in props.Split(';'))
                    {
                        PropertyInfo pr = this.GetType().GetProperty(prop);
                        if (pr == null) continue;
                        if (!Object.Equals(pr.GetValue(this), pr.GetValue(original)))
                        {
                            var history = original;
                            history.ID = 0;
                            history.NonActualDate = DateTime.Now.Date;
                            history.IsHistory = true;
                            this.ActualDate = DateTime.Now.Date;
                            ImportHelper.CreateRepositoryObject(uow, tt, history);
                            return;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
            }

        }


    }
}
