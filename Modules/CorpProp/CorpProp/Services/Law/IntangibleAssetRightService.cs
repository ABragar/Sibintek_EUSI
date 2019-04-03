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
    /// Предоставляет данные и методы сервиса объекта - право на НМА.
    /// </summary>
    public interface IIntangibleAssetRightService : ITypeObjectService<IntangibleAssetRight>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - право на НМА.
    /// </summary>
    public class IntangibleAssetRightService : TypeObjectService<IntangibleAssetRight>, IIntangibleAssetRightService
    {

        private readonly ILogService _logger;
        /// <summary>
        /// Инициализирует новый экземпляр класса IntangibleAssetRightService.
        /// </summary>
        /// <param name="facade"></param>
        public IntangibleAssetRightService(IBaseObjectServiceFacade facade, ILogService logger) : base(facade, logger)
        {
            _logger = logger;

        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>Право на НМА.</returns>
        public override IntangibleAssetRight Create(IUnitOfWork unitOfWork, IntangibleAssetRight obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<IntangibleAssetRight> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<IntangibleAssetRight> objectSaver)
        {
            

            if (objectSaver != null && objectSaver.Dest != null)
            {
                objectSaver.Src.Title = objectSaver.Src.IntangibleAssetRightType?.Name + " "
                    + objectSaver.Dest.RegNumber + " "
                    + ((objectSaver.Dest.DateFrom != null) ? objectSaver.Dest.DateFrom.Value.ToString("dd.MM.yyyy") : "");
                objectSaver.Dest.Title = objectSaver.Src.Title;
            }

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.IntangibleAsset)                   
                    .SaveOneObject(x => x.IntangibleAssetRightType)
                    .SaveOneObject(x => x.Image)                   
                    ;
        }
    }
}
