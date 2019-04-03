using Base;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Security.Service;
using Base.Service;
using Base.Service.Log;
using CorpProp.Entities.Estate;
using CorpProp.Entities.Law;
using CorpProp.Helpers;
using CorpProp.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Services.Law
{
    /// <summary>
    /// Предоставляет данные и методы сервиса объекта - обременение.
    /// </summary>
    public interface IEncumbranceService : ITypeObjectService<Encumbrance>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - обременение.
    /// </summary>
    public class EncumbranceService : TypeObjectService<Encumbrance>, IEncumbranceService
    {

        private readonly ILogService _logger;
        /// <summary>
        /// Инициализирует новый экземпляр класса EncumbranceService.
        /// </summary>
        /// <param name="facade"></param>
        public EncumbranceService(IBaseObjectServiceFacade facade, ILogService logger) : base(facade, logger)
        {
            _logger = logger;

        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>Обременение.</returns>
        public override Encumbrance Create(IUnitOfWork unitOfWork, Encumbrance obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<Encumbrance> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<Encumbrance> objectSaver)
        {
            if (objectSaver != null && objectSaver.Dest != null)
                objectSaver.Dest.Title = objectSaver.Dest.EncumbranceType?.Name + " "
                    + objectSaver.Dest.RegNumber + " "
                    + ((objectSaver.Dest.RegDate != null) ? objectSaver.Dest.RegDate.Value.ToString("dd.MM.yyyy") : "");
            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.EncumbranceType)                    
                    .SaveOneObject(x => x.Right)                    
                    ;
        }
    }
}
