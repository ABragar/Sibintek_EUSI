using Base.DAL;
using Base.Service;
using CorpProp.Entities.ProjectActivity;

namespace CorpProp.Services.ProjectActivity
{
   
    /// <summary>
    /// Предоставляет данные и методы сервиса объекта - отчет.
    /// </summary>
    public interface ISibTaskReportService : IBaseObjectService<SibTaskReport>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - отчет.
    /// </summary>
    public class SibTaskReportService : BaseObjectService<SibTaskReport>, ISibTaskReportService
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса SibTaskReportService.
        /// </summary>
        /// <param name="facade"></param>
        public SibTaskReportService(IBaseObjectServiceFacade facade) : base(facade)
        {


        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<SibTaskReport> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<SibTaskReport> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                                   .SaveOneObject(t => t.Status)
                                   .SaveOneObject(t => t.Task)                                   
                    ;
        }
    }
}
