using Base.DAL;
using Base.Service;
using CorpProp.Entities.ProjectActivity;
using System.Linq;

namespace CorpProp.Services.ProjectActivity
{
    class SibProjectTemplateTemplateService
    {
    }
    /// <summary>
    /// Предоставляет данные и методы сервиса объекта - шаблон проекта.
    /// </summary>
    public interface ISibProjectTemplateService : IBaseObjectService<SibProject>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - шаблон проекта.
    /// </summary>
    public class SibProjectTemplateService : BaseObjectService<SibProject>, ISibProjectTemplateService
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса SibProjectTemplateService.
        /// </summary>
        /// <param name="facade"></param>
        public SibProjectTemplateService(IBaseObjectServiceFacade facade) : base(facade)
        {


        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<SibProject> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<SibProject> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.Initiator)
                    .SaveOneObject(x => x.Template)
                    .SaveOneObject(x => x.Status)
                    .SaveOneObject(x => x.SibProjectType)
                    ;
        }

        public override SibProject CreateDefault(IUnitOfWork unitOfWork)
        {
            var obj = base.CreateDefault(unitOfWork);

            if (!obj.IsTemplate)
                obj.IsTemplate = true;

            if (obj.Status == null && obj.ID == 0)
            {
                SibProjectStatus defaultStatus = unitOfWork.GetRepository<SibProjectStatus>()
                    .Filter(f => !f.Hidden && f.Code.ToLower() == "draft")
                    .FirstOrDefault();
                if (defaultStatus != null)
                {
                    obj.Status = defaultStatus;
                    obj.StatusID = defaultStatus.ID;
                }
            }

            return obj;
        }
    }
}
