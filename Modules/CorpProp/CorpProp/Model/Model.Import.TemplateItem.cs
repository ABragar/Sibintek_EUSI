using Base;
using Base.UI;
using CorpProp.Entities.Document;
using CorpProp.Entities.Import;
using CorpProp.Helpers;
using CorpProp.Services.Import;
using System.Linq;

namespace CorpProp.Model.Import
{
    public static class ImportTemplateItemModel
    {
        /// <summary>
        /// Создает конфигурацию модели элемента шаблона импорта по умолчанию.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static void CreateModelConfig(this IInitializerContext context)
        {

            context.CreateVmConfig<ImportTemplateItem>()
                   .Title("Элемент шаблона импорта")
                   .DetailView_Default()
                   .ListView_Default()
                   .LookupProperty(x => x.Text(c => c.ID))
                   .IsReadOnly();

        }

        /// <summary>
        /// Конфигурация карточки элемента шаблона импорта по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ImportTemplateItem> DetailView_Default(this ViewModelConfigBuilder<ImportTemplateItem> conf)
        {
            return
                conf.DetailView(x => x
               .Title(conf.Config.Title)
               .Editors(editors => editors
                    .Add(ed => ed.Row, ac => ac.Title("Номер строки").Order(1).IsRequired(true))
                    .Add(ed => ed.Column, ac => ac.Title("Номер колонки").Order(2).IsRequired(true))
                    .Add(ed => ed.Value, ac => ac.Title("Значение").Order(3))
              ));
        }

        /// <summary>
        /// Конфигурация реестра элемента шаблона импорта по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ImportTemplateItem> ListView_Default(this ViewModelConfigBuilder<ImportTemplateItem> conf)
        {
            return
                conf.ListView(x => x
                .Title("Элементы шаблона импорта")
                 .Columns(col => col
                    .Add(c => c.Row, ac => ac.Title("Строка").Visible(true).Order(1))
                    .Add(c => c.Column, ac => ac.Title("Колонка").Visible(true).Order(2))
                    .Add(c => c.Value, ac => ac.Title("Значение").Visible(true).Order(3))                 
                    .Add(c => c.ImportTemplate, ac => ac.Title("Шаблон").Visible(true).Order(5))
                    .Add(c => c.ID, ac => ac.Title("Идентификатор").Visible(false).Order(6))
                 )
               );

        }

    }
}
