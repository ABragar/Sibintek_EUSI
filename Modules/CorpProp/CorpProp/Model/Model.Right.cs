using Base;
using Base.UI;
using CorpProp.Entities.Law;
using CorpProp.Entities.ManyToMany;
using CorpProp.Services.Law;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Model.Law
{
    public static class RightModel
    {
        /// <summary>
        /// Создает конфигурацию модели права по умолчанию.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static void CreateModelConfig(this IInitializerContext context)
        {

            //TODO: отображаем права только по кадастровым объектам
            context.CreateVmConfig<Right>()
                .Service<IRightService>()
                .Title("Право АИС КС")
                .DetailView_Default()
                .LookupProperty(x => x.Text(t => t.RegNumber))
                .ListView(x =>x.Title("Права АИС КС"));

            context.CreateVmConfig<Right>("Cadastral_Rights")
              .Service<IRightService>()
              .Title("Право АИС КС")
              .DetailView_Default()
              .LookupProperty(x => x.Text(t => t.RegNumber))
              .ListView(x => x
              .Title("Права АИС КС")
              .Columns(col => col
              .Add(c => c.Title, ac => ac.Visible(false))
              .Add(c => c.ID, ac => ac.Visible(false))

              .Add(c => c.RightKind, ac => ac.Visible(false))
              .Add(c => c.KindAndShare, ac => ac.Visible(true))
              .Add(c => c.ShareText, ac => ac.Visible(false))
              .Add(c => c.ShareRightNumerator, ac => ac.Visible(false))
              .Add(c => c.ShareRightDenominator, ac => ac.Visible(false))
              .Add(c => c.RegDate, ac => ac.Visible(true))
              .Add(c => c.RightType, ac => ac.Visible(true))
              .Add(c => c.RegDateEnd, ac => ac.Visible(true))
              .Add(c => c.Confiscation, ac => ac.Visible(true))
              ));


        }

        /// <summary>
        /// Конфигурация карточки права по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<Right> DetailView_Default(this ViewModelConfigBuilder<Right> conf)
        {
            return
                conf.DetailView(x => x
               .Title(conf.Config.Title)
               .Editors(edt => edt
               .AddOneToManyAssociation<Encumbrance>("Right_Encumbrances",
                         y => y.TabName("[2]Ограничения/обременения")
                         .Title("Ограничения/обременения")
                         .IsLabelVisible(false)
                         .Create((uofw, entity, id) =>
                         {
                             entity.Right = uofw.GetRepository<Right>().Find(id);
                         })
                         .Delete((uofw, entity, id) =>
                         {
                             entity.Right = null;
                         })

                   .Filter((uofw, q, id, oid) => q.Where(w => w.RightID == id)))
                  
                   .AddManyToManyRigthAssociation<FileCardAndLegalRight>("Right_LegalFileCards", y => y.TabName("[004]Правоустанавливающие документы"))
                   .AddManyToManyRigthAssociation<FileCardAndCertificateRight>("Right_CertificateFileCards", y => y.TabName("[005]Правоудостоверяющие документы"))
                   
                   )
                   .DefaultSettings((uow, r, commonEditorViewModel) =>
                   {
                       r.ShareRightDenominator = 1;
                       r.ShareRightNumerator = 1;
                       r.Share = "1";
                   })
             );
        }

        /// <summary>
        /// Конфигурация реестра прав по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<Right> ListView_Default(this ViewModelConfigBuilder<Right> conf)
        {
            return
                conf.ListView(x => x
                .Title("Права")
                 
               );

        }

    }
}
