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
    /// Предоставляет данные и методы сервиса объекта - сделка без согласия.
    /// </summary>
    public interface IDealRecordService : IBaseObjectService<DealRecord>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - сделка без согласия.
    /// </summary>
    public class DealRecordService : BaseObjectService<DealRecord>, IDealRecordService
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса DealRecordService.
        /// </summary>
        /// <param name="facade"></param>
        public DealRecordService(IBaseObjectServiceFacade facade) : base(facade)
        {


        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>сделка без согласия.</returns>
        public override DealRecord Create(IUnitOfWork unitOfWork, DealRecord obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<DealRecord> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<DealRecord> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.ObjectRecord)
                    .SaveOneObject(x => x.Extract)

                   
                   
                    ;
        }
    }
}
