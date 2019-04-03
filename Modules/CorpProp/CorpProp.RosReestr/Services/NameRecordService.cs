using Base.DAL;
using Base.Service;
using CorpProp.RosReestr.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Services
{

    /// <summary>
    /// Предоставляет данные и методы сервиса объекта - наименования.
    /// </summary>
    public interface INameRecordService : IBaseObjectService<NameRecord>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - наименования.
    /// </summary>
    public class NameRecordService : BaseObjectService<NameRecord>, INameRecordService
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса NameRecordService.
        /// </summary>
        /// <param name="facade"></param>
        public NameRecordService(IBaseObjectServiceFacade facade) : base(facade)
        {


        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>физ.лицо.</returns>
        public override NameRecord Create(IUnitOfWork unitOfWork, NameRecord obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<NameRecord> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<NameRecord> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.DealRecord)
                   
                    ;
        }
    }
}
