using Base.DAL;
using Base.Entities.Complex;
using Base.Service;
using CorpProp.Entities.ProjectActivity;
using System;
using System.Linq;

namespace CorpProp.Services.ProjectActivity
{
   
    ///<summary>
    /// Предоставляет данные и методы сервиса объекта - шаблон задачи.
    /// </summary>
    public interface ISibTaskTemplateService : IBaseObjectService<SibTask>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - шаблон задачи.
    /// </summary>
    public class SibTaskTemplateService : BaseObjectService<SibTask>, ISibTaskTemplateService
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса SibTaskTemplateService.
        /// </summary>
        /// <param name="facade"></param>
        public SibTaskTemplateService(IBaseObjectServiceFacade facade) : base(facade)
        {


        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<SibTask> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<SibTask> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                   .SaveOneObject(t => t.Project)
                                   .SaveOneObject(t => t.Responsible)
                                   .SaveOneObject(t => t.SibStatus)
                                   .SaveOneObject(t => t.Parent_)
                    ;
        }


        public override SibTask CreateDefault(IUnitOfWork unitOfWork)
        {
            var obj = base.CreateDefault(unitOfWork);

            if (!obj.IsTemplate)
                obj.IsTemplate = true;

            if (obj.Period == null || obj.Period.Start == DateTime.MinValue)
            {
                obj.Period = new Period()
                {
                    Start = DateTime.Now,
                    End = DateTime.Now
                };
            }

            if (obj.SibStatus == null && obj.ID == 0)
            {
                SibTaskStatus defaultStatus = unitOfWork.GetRepository<SibTaskStatus>()
                    .Filter(f => !f.Hidden && f.Code.ToLower() == "draft")
                    .FirstOrDefault();

                if (defaultStatus != null)
                {
                    obj.SibStatus = defaultStatus;
                    obj.SibStatusID = defaultStatus.ID;
                }
            }

            return obj;
        }
    }
}
