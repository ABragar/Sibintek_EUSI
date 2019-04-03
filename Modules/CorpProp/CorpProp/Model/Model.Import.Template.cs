using Base;
using Base.UI;
using CorpProp.Entities.Document;
using CorpProp.Entities.Import;
using CorpProp.Helpers;
using CorpProp.Services.Import;
using System.Linq;

namespace CorpProp.Model.Import
{
    public static class ImportTemplateModel
    {
        /// <summary>
        /// Создает конфигурацию модели шаблона импорта по умолчанию.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static void CreateModelConfig(this IInitializerContext context)
        {

            context.CreateVmConfig<ImportTemplate>()                   
                   .Title("Шаблон импорта")
                   .DetailView_Default()
                   .ListView_Default()
                   .LookupProperty(x => x.Text(c => c.ID))
                   .IsReadOnly();

        }

        /// <summary>
        /// Конфигурация карточки шаблона импорта по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ImportTemplate> DetailView_Default(this ViewModelConfigBuilder<ImportTemplate> conf)
        {
            return
                conf.DetailView(x => x
               .Title(conf.Config.Title)
               .Editors(editors => editors
                    .Add(ed => ed.Name, ac => ac.Title("Наименование").Order(1).IsRequired(true))
                    .Add(ed => ed.Mnemonic, ac => ac.Title("Объект загрузки").Order(2).IsRequired(true))
                    .Add(ed => ed.Version, ac => ac.Title("Версия").Order(3))
                    .Add(ed => ed.Active, ac => ac.Title("Активный").Order(4))
                    .Add(ed => ed.FileNameFormat, ac => ac.Title("Формат имени файла").Order(5))
                    .Add(ed => ed.Description, ac => ac.Title("Примечание").Order(6))
                    .Add(ed => ed.RowData, ac => ac.Title("Номер строки начала данных").Order(7))
                    .Add(ed => ed.ColumnData, ac => ac.Title("Номер колонки начала данных").Order(8))
                    .Add(ed => ed.RowHistory, ac => ac.Title("Номер строки истории").Order(9))
                    .Add(ed => ed.RowFiledSystem, ac => ac.Title("Номер строки системных свойств").Order(10))
                    .Add(ed => ed.RowFiledName, ac => ac.Title("Номер строки наименований свойств").Order(11))
                    .Add(ed => ed.RowRequired, ac => ac.Title("Номер строки обязательности").Order(12))                   


                    .AddOneToManyAssociation<ImportTemplateItem>("ImportTemplate_Items",
                        y => y.TabName("Элементы")
                            .IsLabelVisible(true)
                         .Create((uofw, entity, id) =>
                         {
                             entity.ImportTemplate = uofw.GetRepository<ImportTemplate>().Find(id);
                         })
                         .Delete((uofw, entity, id) =>
                         {
                             entity.ImportTemplate = null;
                         })

                            .Filter((uofw, q, id, oid) => q.Where(w => w.ImportTemplateID == id))
                        )
              ));
        }

        /// <summary>
        /// Конфигурация реестра шаблона импорта по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ImportTemplate> ListView_Default(this ViewModelConfigBuilder<ImportTemplate> conf)
        {
            return
                conf.ListView(x => x
                .Title("Шаблон импорта")
                 .Columns(col => col
                    .Add(c => c.Name, ac => ac.Title("Наименование").Visible(true).Order(1))
                    .Add(c => c.Mnemonic, ac => ac.Title("Объект загрузки").Visible(true).Order(2))
                    .Add(c => c.Version, ac => ac.Title("Версия").Visible(true).Order(3))
                    .Add(c => c.Active, ac => ac.Title("Активный").Visible(true).Order(4))
                    .Add(c => c.Description, ac => ac.Title("Примечание").Visible(true).Order(5))
                    .Add(c => c.ID, ac => ac.Title("Идентификатор").Visible(false).Order(6))
                 )
               );

        }

    }
}
