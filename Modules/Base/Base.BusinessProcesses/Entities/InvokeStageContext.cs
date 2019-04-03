using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.BusinessProcesses.Entities.Steps;
using Base.Security;
using Base.Task.Entities;

namespace Base.BusinessProcesses.Entities
{
    public class InvokeStageContext
    {
        //public List<BPTask> TasksToCreate { get; set; } = new List<BPTask>();
        //public List<BPTask> TasksToUpdate { get; set; } = new List<BPTask>();
        public Dictionary<Stage,ICollection<User>> PermitedUsers { get; set; } = new Dictionary<Stage, ICollection<User>>();
        public StageAction Action { get; set; }
        public ActionComment ActionComment { get; set; }
        public IBPObject BPObject { get; set; }

        /// <summary>
        /// Назначен из меню выбора пользователей
        /// </summary>
        public int? PerformUserID { get; set; }
    }
}
