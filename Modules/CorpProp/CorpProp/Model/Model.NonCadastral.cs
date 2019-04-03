using Base;
using Base.UI;
using CorpProp.Entities.Estate;
using CorpProp.Services.Estate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Model
{
    /// <summary>
    /// Предоставляет методы конфигурации модели не кадастрового объекта.
    /// </summary>
    public static class NonCadastralModel
    {
        /// <summary>
        /// Создает конфигурацию модели земельного участка по умолчанию.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static void CreateModelConfig(this IInitializerContext context)
        {

            context.CreateVmConfig<NonCadastral>()
                   .Service<INonCadastralService > ()
                   .Title("Не кадастровый объект")
                   .DetailView(x => x.Title("Не кадастровый объект"))
                   .ListView(x =>x.Title("Не кадастровые объект"))
                   .LookupProperty(x => x.Text(c => c.Name));
           
        }
    }
}
