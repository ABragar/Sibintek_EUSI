using Base;
using Base.UI;
using Base.UI.Editors;
using CorpProp.Entities.Law;
using CorpProp.Entities.ManyToMany;
using CorpProp.Helpers;
using CorpProp.Services.Law;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CorpProp.RosReestr.Model
{
    public static class ExtractModel
    {
        /// <summary>
        /// Создает конфигурацию модели выписки по умолчанию.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static void ModifyModelConfig(this IInitializerContext context)
        {

            context.ModifyVmConfig<Extract>()
            .Service<IExtractService>()
            .Title("Выписка из ЕГРН")
            .DetailView_Default()
            .ListView_Default()
            .LookupProperty(x => x.Text(t => t.ExtractNumber))
            .IsReadOnly(true);

        }

        /// <summary>
        /// Конфигурация карточки выписки по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<Extract> DetailView_Default(this ViewModelConfigBuilder<Extract> conf)
        {

            return
                conf.DetailView(x => x
               .Title(conf.Config.Title)
               .Editors(editors => editors
                    .AddManyToManyRigthAssociation<FileCardAndExtract>("Extract_FileCards", y => y.TabName("[006]Документы"))
              )

              );
        }


        /// <summary>
        /// Конфигурация реестра выписок по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<Extract> ListView_Default(this ViewModelConfigBuilder<Extract> conf)
        {
            return
                conf.ListView(x => x
                .Title("Выписки")
                 .Columns(col => col
                    .Add(c => c.Name, ac => ac.Title("Наименование").Visible(true).Order(1))
                    //.Add(c => c.Mnemonic, ac => ac.Title("Объект загрузки").Visible(true).Order(2))
                    //.Add(c => c.Version, ac => ac.Title("Версия").Visible(true).Order(3))
                    //.Add(c => c.Active, ac => ac.Title("Активный").Visible(true).Order(4))
                    //.Add(c => c.Description, ac => ac.Title("Примечание").Visible(true).Order(5))
                    .Add(c => c.ID, ac => ac.Title("Идентификатор").Visible(false).Order(6))
                 )
               );

        }

      
    }
}
