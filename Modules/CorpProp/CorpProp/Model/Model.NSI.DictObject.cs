using Base;
using Base.UI;
using CorpProp.Entities.Base;
using CorpProp.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Model.NSI
{
    public static class DicObjectModel
    {
        /// <summary>
        /// Создает конфигурацию модели права по умолчанию.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static void CreateModelConfig(this IInitializerContext context)
        {

            context.CreateVmConfig<DictObject>()
                .Service<IDictObjectService<DictObject>>()
                .Title("Справочник")               
                .DetailView_Default()
                .ListView_Default()
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfig<DictObject>("DictMenu")
                .Service<IDictHistoryService<DictObject>>()
                .Title("Справочник")
                .DetailView_DictMenu()
                .ListView_DictMenu()
                .LookupProperty(x => x.Text(t => t.Name));


        }

        /// <summary>
        /// Конфигурация карточки НСИ по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<DictObject> DetailView_Default(this ViewModelConfigBuilder<DictObject> conf)
        {
            return
                conf.DetailView(x => x
               .Title(conf.Config.Title)
               .Editors(edt => edt
                    .Add(ed => ed.Name, ac=>ac.Visible(true).Order(1).IsRequired(true))
                    .Add(ed => ed.PublishCode, ac => ac.Visible(true).Order(2))
                    .Add(ed => ed.IsDefault, ac => ac.Visible(false).Order(3))
                    .Add(ed => ed.DateFrom, ac => ac.Visible(true).Order(4).IsRequired(true))
                    .Add(ed => ed.DateTo, ac => ac.Visible(true).Order(5))
                    .Add(ed => ed.DictObjectState, ac => ac.Visible(true).Order(6).IsReadOnly(true))
                    .Add(ed => ed.DictObjectStatus, ac => ac.Visible(true).Order(7))                   

              )
             );
        }

        /// <summary>
        /// Конфигурация реестра НСИ по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<DictObject> ListView_Default(this ViewModelConfigBuilder<DictObject> conf)
        {
            return
                conf.ListView(x => x               
                .Columns( cols => cols
                        .Add(col => col.PublishCode, ac=>ac.Visible(false).Order(1))
                        .Add(col => col.Name, ac => ac.Visible(true).Order(2))
                        .Add(col => col.DictObjectState, ac => ac.Visible(true).Order(3))
                        .Add(col => col.DictObjectStatus, ac => ac.Visible(true).Order(4))
                        .Add(col => col.DateFrom, ac => ac.Visible(false).Order(5))
                        .Add(col => col.DateTo, ac => ac.Visible(false).Order(6))
                        .Add(col => col.Code, ac => ac.Visible(false).Order(7))
                    )
               );

        }


        public static ViewModelConfigBuilder<DictObject> DetailView_DictMenu(this ViewModelConfigBuilder<DictObject> conf)
        {
            return
                conf.DetailView(x => x
               .Title(conf.Config.Title)
               .Editors(edt => edt
                    .Add(ed => ed.Name, ac => ac.Visible(true).Order(1).IsRequired(true))
                    .Add(ed => ed.PublishCode, ac => ac.Visible(true).Order(2))
                    .Add(ed => ed.IsDefault, ac => ac.Visible(false).Order(3))
                    .Add(ed => ed.DateFrom, ac => ac.Visible(true).Order(4).IsRequired(true))
                    .Add(ed => ed.DateTo, ac => ac.Visible(true).Order(5))
                    .Add(ed => ed.DictObjectState, ac => ac.Visible(true).Order(6).IsReadOnly(true))
                    .Add(ed => ed.DictObjectStatus, ac => ac.Visible(true).Order(7))

              )
             );
        }

        public static ViewModelConfigBuilder<DictObject> ListView_DictMenu(this ViewModelConfigBuilder<DictObject> conf)
        {
            return
                conf.ListView(x => x
                .IsMultiEdit(true)
                .Columns(cols => cols
                       .Add(col => col.PublishCode, ac => ac.Visible(true).Order(1))
                       .Add(col => col.Name, ac => ac.Visible(true).Order(2))
                       .Add(col => col.DictObjectState, ac => ac.Visible(true).Order(3))
                       .Add(col => col.DictObjectStatus, ac => ac.Visible(true).Order(4))
                       .Add(col => col.DateFrom, ac => ac.Visible(true).Order(5))
                       .Add(col => col.DateTo, ac => ac.Visible(true).Order(6))
                       .Add(col => col.Code, ac => ac.Visible(false).Order(7))

                    )
               );

        }

    }
}
