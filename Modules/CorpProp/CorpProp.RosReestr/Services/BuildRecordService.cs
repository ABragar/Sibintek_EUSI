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
    /// Предоставляет данные и методы сервиса объекта - здание.
    /// </summary>
    public interface IBuildRecordService : IBaseObjectService<BuildRecord>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - здание.
    /// </summary>
    public class BuildRecordService : BaseObjectService<BuildRecord>, IBuildRecordService
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса BuildRecordService.
        /// </summary>
        /// <param name="facade"></param>
        public BuildRecordService(IBaseObjectServiceFacade facade) : base(facade)
        {


        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>здание.</returns>
        public override BuildRecord Create(IUnitOfWork unitOfWork, BuildRecord obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<BuildRecord> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<BuildRecord> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.Extract)
                    .SaveOneObject(x => x.Cadastral)


                  

                    ;
        }
    }
}
