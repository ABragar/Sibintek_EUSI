using Base.DAL;
using CorpProp.Entities.ProjectActivity;
using System.Linq;

namespace CorpProp.Helpers
{
    public static class ProjectActivityHelper
    {
        /// <summary>
        /// Формирует номер задачи.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="task">Задача.</param>
        /// <returns>Номер задачи.</returns>
        public static string SetTaskNumber(IUnitOfWork unitOfWork, SibTask task)
        {
            string number = "";
            GenerateTaskNumber(unitOfWork, task, ref number);
            if (task.Project == null)
                return number;

            var projectNumber = unitOfWork.GetRepository<SibProject>().Find(task.Project.ID).ProjectNumber ?? "";
            number = $"{projectNumber}.{number}";

            return number;
        }

        /// <summary>
        /// Формирует номер проекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="project">Проект.</param>
        /// <returns>Номер проекта.</returns>
        public static string SetProjectNumber(IUnitOfWork unitOfWork, SibProject project)
        {
            var projectRepo = unitOfWork.GetRepository<SibProject>();

            var pp = projectRepo.All().OrderByDescending(m => m.ID).FirstOrDefault();
            int maxNumber = pp?.ID ?? 0;

            var society = project.Initiator?.SocietyID != null ?
                unitOfWork.GetRepository<Entities.Subject.Society>().Find(f => f.ID == project.Initiator.SocietyID) :
                null;

            var societyCode = society?.ConsolidationUnit != null ?
                unitOfWork.GetRepository<Entities.NSI.Consolidation>().Find(f => f.ID == society.ConsolidationUnit.ID).Code :
                null;

            var number = (string.IsNullOrEmpty(societyCode) ? "" : societyCode + "-") + ++maxNumber;

            return number;
        }

        /// <summary>
        /// Составляет иерархичный номер задачи.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="task">Задача.</param>
        /// <param name="number">Номер.</param>
        /// <param name="isParent">Признак родителя.</param>
        private static void GenerateTaskNumber(IUnitOfWork unitOfWork, SibTask task, ref string number, bool isParent = false)
        {
            string separator = string.IsNullOrEmpty(number) ? "" : ".";
            var taskRepo = unitOfWork.GetRepository<SibTask>();
            int? projectId = task.Project?.ID;
            int? parentId = task.Parent?.ID;
            var tt = taskRepo.All().Where(w => w.ProjectID == projectId && w.ParentID == parentId).OrderByDescending(o => o.InternalNumber).FirstOrDefault();
            int internalNumber = tt?.InternalNumber ?? 0;
            internalNumber = !isParent || internalNumber == 0? ++internalNumber : internalNumber;
            number = $"{internalNumber}{separator}{number}";

            if (task.Parent == null)
                return;

            var pt = taskRepo.Find(task.Parent.ID);
            GenerateTaskNumber(unitOfWork, pt, ref number, true);
        }
    }
}
