using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.BusinessProcesses.Entities;
using Base.DAL;
using CorpProp.Entities.ProjectActivity;

namespace CorpProp.Helpers
{
    public static class SibTaskHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uofw"></param>
        /// <param name="id"></param>
        public static void VeryfyUpdateParentTaskPeriod(ITransactionUnitOfWork uofw, int id)
        {
            var task = uofw.GetRepository<SibTask>().All().Single(x => x.ID == id);
            var parentTask = task.Parent_;
            if (parentTask == null)
            {
                return;
            }

            if (task.Period.Start <= parentTask.Period.Start.AddDays(-1))
            {
                parentTask.Period.Start = task.Period.Start.AddDays(-1);
            }


            if (task.Period.End.HasValue)
            {
                if (parentTask.Period.End.HasValue)
                {
                    if (task.Period.End.Value >= parentTask.Period.End.Value.AddDays(1))
                    {
                        parentTask.Period.End = task.Period.End.Value.AddDays(1);
                    }
                }
                else
                {
                    parentTask.Period.End = task.Period.End.Value.AddDays(1);
                }
            }

            uofw.SaveChanges();
            if (parentTask.Parent_ != null)
            {
                VeryfyUpdateParentTaskPeriod(uofw, parentTask.ID);
            }
        }
    }
}
