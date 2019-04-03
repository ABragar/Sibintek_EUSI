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
    /// Предоставляет данные и методы сервиса для кадастровой стоимости.
    /// </summary>
    public interface ICadastralValueService : IBaseObjectService<CadastralValue>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с кадастровой стоимостью.
    /// </summary>
    public class CadastralValueService : BaseObjectService<CadastralValue>, ICadastralValueService
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса CadastralValueService.
        /// </summary>
        /// <param name="facade"></param>
        public CadastralValueService(IBaseObjectServiceFacade facade) : base(facade)
        {


        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>Кадастровая стоимость.</returns>
        public override CadastralValue Create(IUnitOfWork unitOfWork, CadastralValue obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<CadastralValue> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<CadastralValue> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                   
                    .SaveOneObject(x => x.Cadastral)                  
                    .SaveOneObject(x => x.InformationSource)
                    ;
        }
    }
}
