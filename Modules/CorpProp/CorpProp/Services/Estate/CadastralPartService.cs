using Base;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Security.Service;
using Base.Service;
using CorpProp.Entities.Estate;
using CorpProp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Services.Estate
{
    /// <summary>
    /// Предоставляет данные и методы сервиса для части кадатсрового объекта.
    /// </summary>
    public interface ICadastralPartService : IBaseObjectService<CadastralPart>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом.
    /// </summary>
    public class CadastralPartService : BaseObjectService<CadastralPart>, ICadastralPartService
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса CadastralPartService.
        /// </summary>
        /// <param name="facade"></param>
        public CadastralPartService(IBaseObjectServiceFacade facade) : base(facade)
        {


        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns></returns>
        public override CadastralPart Create(IUnitOfWork unitOfWork, CadastralPart obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<CadastralPart> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<CadastralPart> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.Cadastral)                   
                    ;
        }
    }
}
