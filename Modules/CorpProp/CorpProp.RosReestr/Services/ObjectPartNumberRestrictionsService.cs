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
    /// Предоставляет данные и методы сервиса объекта - часть ОНИ.
    /// </summary>
    public interface IObjectPartNumberRestrictionsService : IBaseObjectService<ObjectPartNumberRestrictions>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - часть ОНИ.
    /// </summary>
    public class ObjectPartNumberRestrictionsService : BaseObjectService<ObjectPartNumberRestrictions>, IObjectPartNumberRestrictionsService
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса ObjectPartNumberRestrictionsService.
        /// </summary>
        /// <param name="facade"></param>
        public ObjectPartNumberRestrictionsService(IBaseObjectServiceFacade facade) : base(facade)
        {


        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>часть ОНИ.</returns>
        public override ObjectPartNumberRestrictions Create(IUnitOfWork unitOfWork, ObjectPartNumberRestrictions obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<ObjectPartNumberRestrictions> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<ObjectPartNumberRestrictions> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.ObjectRecord)
                    .SaveOneObject(x => x.Extract)
                    ;
        }
    }
}
