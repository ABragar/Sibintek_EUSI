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
    /// Предоставляет данные и методы сервиса объекта - Кадастровый номер.
    /// </summary>
    public interface ICadNumberService : IBaseObjectService<CadNumber>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - Кадастровый номер.
    /// </summary>
    public class CadNumberService : BaseObjectService<CadNumber>, ICadNumberService
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса CadNumberService.
        /// </summary>
        /// <param name="facade"></param>
        public CadNumberService(IBaseObjectServiceFacade facade) : base(facade)
        {


        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>Кадастровый номер.</returns>
        public override CadNumber Create(IUnitOfWork unitOfWork, CadNumber obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<CadNumber> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<CadNumber> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.ObjectRecordLand)
                    .SaveOneObject(x => x.ObjectRecordRoom)
                    .SaveOneObject(x => x.ObjectRecordCarParking)
                    .SaveOneObject(x => x.ExtractLand)
                    .SaveOneObject(x => x.ExtractRoom)
                    .SaveOneObject(x => x.ExtractCarParking)
                    
                    ;
        }
    }
}
