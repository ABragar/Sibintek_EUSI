using Base;
using Base.UI;
using Base.UI.Editors;
using CorpProp.Entities.ManyToMany;
using CorpProp.Extentions;
using CorpProp.Helpers;
using CorpProp.RosReestr.Entities;
using CorpProp.RosReestr.Helpers;
using CorpProp.RosReestr.Migration;
using CorpProp.RosReestr.Services;
using System.Linq;

namespace CorpProp.RosReestr.Model
{
    public static class ExtractNZSModel
    {
        /// <summary>
        /// Создает конфигурацию модели выписки на НЗС по умолчанию.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static void CreateModelConfig(this IInitializerContext context)
        {

            context.CreateVmConfig<ExtractNZS>()           
            .Title("Выписка из ЕГРН - Объект незавершенного строительства")
            .DetailView_Default()
            .ListView_Default()
            .LookupProperty(x => x.Text(t => t.ExtractNumber))
            .IsReadOnly(true);

        }

        /// <summary>
        /// Конфигурация карточки выписки на НЗС по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ExtractNZS> DetailView_Default(this ViewModelConfigBuilder<ExtractNZS> conf)
        {
            //TODO: натсроить дитейл
            return conf;
        }


        /// <summary>
        /// Конфигурация реестра выписок на НЗС по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ExtractNZS> ListView_Default(this ViewModelConfigBuilder<ExtractNZS> conf)
        {

            //TODO: настроить ListView
            return conf;               

        }
    }
}
