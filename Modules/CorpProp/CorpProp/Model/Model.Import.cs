using Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Model
{
    public static class ImportModel
    {
        /// <summary>
        /// Инициализация моделей импорта.
        /// </summary>
        /// <param name="context"></param>
		public static void Init(IInitializerContext context)
        {

            CorpProp.Model.Import.ImportHistoryModel.CreateModelConfig(context);
            CorpProp.Model.Import.ImportErrorLogModel.CreateModelConfig(context);            


        }
    }
}
