using Base.DAL;
using Base.Service;
using CorpProp.Entities.ProjectActivity;
using CorpProp.Entities.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using AppContext = Base.Ambient.AppContext;
using CorpProp.Helpers;
using Base.Security;

namespace CorpProp.Services.ProjectActivity
{   

    /// <summary>
    /// Предоставляет данные и методы сервиса объекта - проект.
    /// </summary>
    public interface ISibProjectService : IBaseObjectService<SibProject>
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - проект.
    /// </summary>
    public class SibProjectService : BaseObjectService<SibProject>, ISibProjectService
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса SibProjectService.
        /// </summary>
        /// <param name="facade"></param>
        public SibProjectService(IBaseObjectServiceFacade facade) : base(facade)
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
            if (objectSaver.Src.ProjectNumber == null)
            {
                objectSaver.Dest.ProjectNumber = ProjectActivityHelper.SetProjectNumber(unitOfWork, objectSaver.Src);
            }

            if ((objectSaver.Src.DateFrom != null && objectSaver.Src.DateTo != null) &&
                objectSaver.Src.DateFrom.Value.Date > objectSaver.Src.DateTo.Value.Date)
                throw new Exception("Дата начала проекта больше даты окончания.");

            if (objectSaver.Src.Status != null && objectSaver.Dest.StatusID != objectSaver.Src.Status.ID)
            {
                var taskRepo = unitOfWork.GetRepository<SibTask>();
                SibProjectStatus projectStatus = unitOfWork.GetRepository<SibProjectStatus>().Find(f => f.ID == objectSaver.Src.Status.ID);
                List<SibTask> tasks = taskRepo.Filter(f => f.Project != null && f.Project.ID == objectSaver.Src.ID && !f.Hidden).ToList();

                if (projectStatus != null && projectStatus.Code == "Current" && tasks.Count > 0)
                {
                    SibTaskStatus taskStatus = unitOfWork.GetRepository<SibTaskStatus>()
                        .Filter(f => !f.Hidden && f.Code == "Appoint")
                        .FirstOrDefault();

                    if (taskStatus != null)
                    {
                        foreach (SibTask task in tasks)
                        {
                            task.SibStatus = taskStatus;
                            taskRepo.Update(task);
                            unitOfWork.SaveChanges();
                        }
                    }
                }
            }

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
           
            if (obj.Initiator == null)
            {

                var uid = AppContext.SecurityUser.ID;
                if (uid != 0)
                {
                    int? profileId = unitOfWork.GetRepository<User>().Find(f => f.ID == uid && !f.Hidden).BaseProfileID;

                    var currentSibUser = profileId != null ? unitOfWork.GetRepository<SibUser>().Find(f => f.ID == profileId && !f.Hidden) : null;

                    if (currentSibUser != null)
                    {
                        obj.Initiator = currentSibUser;
                        obj.InitiatorID = currentSibUser.ID;
                    }
                }
            }

            if (obj.Status == null && obj.ID == 0)
            {
                SibProjectStatus defaultStatus = unitOfWork.GetRepository<SibProjectStatus>()
                    .Filter(f => !f.Hidden && f.Code.ToLower() == "draft")
                    .FirstOrDefault();
                obj.Status = defaultStatus;
                obj.StatusID = defaultStatus?.ID;
            }

            return obj;
        }
    }
}
