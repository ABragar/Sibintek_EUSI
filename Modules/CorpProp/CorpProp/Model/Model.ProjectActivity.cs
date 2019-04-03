using Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Model
{
    public static class ProjectActivityModel
    {
        public static void Init(IInitializerContext context)
        {
            CorpProp.Model.ProjectActivity.ProjectModel.CreateModelConfig(context);
            CorpProp.Model.ProjectActivity.TaskModel.CreateModelConfig(context);
        }
    }
}
