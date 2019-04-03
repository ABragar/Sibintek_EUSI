using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base;

namespace CorpProp.Model
{
    public static class SettingsModel
    {
        public static void Init(IInitializerContext context)
        {
            CorpProp.Model.Settings.ExportImportSettingsModel.CreateModelConfig(context);
            CorpProp.Model.Settings.SibNotificationModel.CreateModelConfig(context);
        }
    }
}
