using Base;
using Base.UI;
using CorpProp.Entities.ManyToMany;
using CorpProp.Helpers;
using CorpProp.RosReestr.Entities;
using CorpProp.RosReestr.Helpers;
using CorpProp.RosReestr.Services;
using System.Linq;
using CorpProp.Extentions;
using Base.UI.Editors;
using CorpProp.RosReestr.Migration;

namespace CorpProp.RosReestr.Model
{
    public static class ExtractSubjModel
    {
        /// <summary>
        /// Создает конфигурацию модели выписки о правах ЮЛ по умолчанию.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static void CreateModelConfig(this IInitializerContext context)
        {

            context.CreateVmConfig<ExtractSubj>()
            .Service<IExtractSubjService>()
            .Title("Выписка из ЕГРН о правах юридического лица на ОНИ")
            .DetailView_Default()
            .ListView_Default()
            .LookupProperty(x => x.Text(t => t.Name))
            .IsReadOnly(true);

        }

        /// <summary>
        /// Конфигурация карточки выписки ЮЛ по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ExtractSubj> DetailView_Default(this ViewModelConfigBuilder<ExtractSubj> conf)
        {

            
                conf.DetailView(x => x
               .Title("Выписка из ЕГРН о правах юридического лица на ОНИ")
               .Editors(editors => editors
                    //реквизиты выписки
                    .Add(c => c.isAccept, ac => ac.TabName(ExtractTabs.GeneralData1).Visible(true).Order(-1))
                    .Add(ed => ed.SibUser, ac => ac.Title("Пользователь").TabName(ExtractTabs.GeneralData1).Order(1))
                    .Add(ed => ed.Society, ac => ac.Title("ОГ").TabName(ExtractTabs.GeneralData1).IsRequired(true).Order(1))
                    .Add(ed => ed.SubjectRecord, ac => ac.Title("Субъект").TabName(ExtractTabs.GeneralData1).IsRequired(true).Order(2))
                    .Add(ed => ed.ExtractNumber, ac => ac.Title("Номер выписки").TabName(ExtractTabs.GeneralData1).IsRequired(true).Order(3))
                    .Add(ed => ed.ExtractDate, ac => ac.Title("Дата выписки").TabName(ExtractTabs.GeneralData1).Order(4))
                    .Add(ed => ed.StartDate, ac => ac.Title("Дата начала").TabName(ExtractTabs.GeneralData1).Order(5))
                    .Add(ed => ed.EndDate, ac => ac.Title("Дата окончания").TabName(ExtractTabs.GeneralData1).Order(6))                  
                    .Add(ed => ed.OfficeNumber, ac => ac.Title("Исходящий номер учреждения").TabName(ExtractTabs.GeneralData1).Order(7))
                    .Add(ed => ed.OfficeDate, ac => ac.Title("Исходящая дата учреждения").TabName(ExtractTabs.GeneralData1).Order(8))
                    .Add(ed => ed.Registrator, ac => ac.Title("Регистратор").TabName(ExtractTabs.GeneralData1).Order(9))
                    .Add(ed => ed.UpdateCPStatus, ac => ac.Title("Статус обновления в АИС КС").TabName(ExtractTabs.GeneralData1).Order(10))
                    .Add(ed => ed.UpdateCPDateTime, ac => ac.Title("Дата обновления в АИС КС").TabName(ExtractTabs.GeneralData1).Order(11))


                    .Add(ed => ed.NumberRequest, ac => ac.Title("Номер запроса").TabName(ExtractTabs.GeneralData1).Order(10))
                    .Add(ed => ed.DateRequest, ac => ac.Title("Дата запроса").TabName(ExtractTabs.GeneralData1).Order(11))
                    .Add(ed => ed.Title, ac => ac.Title("Служба").TabName(ExtractTabs.GeneralData1).Order(12))
                    .Add(ed => ed.DeptName, ac => ac.Title("Управление").TabName(ExtractTabs.GeneralData1).Order(13))                  
                    .Add(ed => ed.ReceivName, ac => ac.Title("Адресат").TabName(ExtractTabs.GeneralData1).Order(14))
                    .Add(ed => ed.Representativ, ac => ac.Title("Ответственный от имени Адресата").TabName(ExtractTabs.GeneralData1).Order(15))
                    .Add(ed => ed.ReceivAdress, ac => ac.Title("Адрес").TabName(ExtractTabs.GeneralData1).Order(16))

                    //права на ОНИ (плоская таблица)
                    .AddOneToManyAssociation<SubjRight>("ExtractSubj_SubjRights", y => y
                     .TabName("[001]Строки выписки") 
                     .Order(1)
                     .Filter((uofw, q, id, oid) => q.Where(w => w.ExtractID == id)))

                    //права
                    .AddOneToManyAssociation<RightRecord>("ExtractSubj_RightRecords", y => y
                     .TabName(ExtractTabs.Rights2)
                     .Title("Права")
                     .Mnemonic("CheckboxExtractRights")
                     .Filter((uofw, q, id, oid) => q.Where(w => w.ExtractID == id)))


                    //они
                    .AddOneToManyAssociation<ObjectRecord>("ExtractSubj_ObjectRecords", y => y
                     .TabName(ExtractTabs.Objetcs3)
                     .Title("ОНИ")
                     .Filter((uofw, q, id, oid) => q.Where(w => w.ExtractID == id)))

                    //док-ты основания прав
                    .AddOneToManyAssociation<DocumentRecord>("ExtractSubj_DocRights", y => y
                     .TabName(ExtractTabs.DocRights4)
                     .Title("Документы основания регистрации")
                     .Filter((uofw, q, id, oid) => q.Where(w => w.ExtractID == id && w.RightRecord != null)))

                    //ограничения/обременения
                    .AddOneToManyAssociation<RestrictRecord>("ExtractSubj_RestrictRecords", y => y
                     .TabName(ExtractTabs.Encumbrances5)
                     .Title("Ограничения/Обременения")
                     .Filter((uofw, q, id, oid) => q.Where(w => w.ExtractID == id )))


                   

                     //сервисная информация
                     .Add(ed => ed.CodeType, ac => ac.Title("Тип передаваемой информации")
                        .TabName(ExtractTabs.SrvInfo5).Order(1))
                     .Add(ed => ed.Version, ac => ac.Title("Версия схемы")
                        .TabName(ExtractTabs.SrvInfo5).Order(2))
                     .Add(ed => ed.Scope, ac => ac.Title("Тип учетной системы")
                        .TabName(ExtractTabs.SrvInfo5).Order(3))


                     .Add(ed => ed.RecipientKod, ac => ac.Title("Код организации")
                        .TabName(ExtractTabs.SrvInfo5).Order(4))
                     .Add(ed => ed.RecipientName, ac => ac.Title("Наименование организации")
                        .TabName(ExtractTabs.SrvInfo5).Order(5))


                     .Add(ed => ed.SenderKod, ac => ac.Title("Код")
                        .TabName(ExtractTabs.SrvInfo5).Order(6))
                     .Add(ed => ed.SenderName, ac => ac.Title("Наименование")
                        .TabName(ExtractTabs.SrvInfo5).Order(7))
                     .Add(ed => ed.RegionName, ac => ac.Title("Регион")
                        .TabName(ExtractTabs.SrvInfo5).Order(8))
                     .Add(ed => ed.DateUpload, ac => ac.Title("Дата выгрузки")
                        .TabName(ExtractTabs.SrvInfo5).Order(9))

                     .Add(ed => ed.FIO, ac => ac.Title("ФИО")
                        .TabName(ExtractTabs.SrvInfo5).Order(10))
                     .Add(ed => ed.Appointment, ac => ac.Title("Должность")
                        .TabName(ExtractTabs.SrvInfo5).Order(11))
                     .Add(ed => ed.EMail, ac => ac.Title("Email")
                        .TabName(ExtractTabs.SrvInfo5).Order(12))
                     .Add(ed => ed.Telephone, ac => ac.Title("Телефон")
                        .TabName(ExtractTabs.SrvInfo5).Order(13))

                    .AddOneToManyAssociation<MigrateLog>("ExtractSubj_MigrateLogs",
                        y => y.TabName(ExtractTabs.Logs9)
                            .IsReadOnly(true)
                            .TabName("Журнал миграции")
                            .Filter((uofw, q, id, oid) => q.Where(w => w.MigrateHistory != null && w.MigrateHistory.ExtractID == id))
                        )
               )

              )
              .Config.DetailView.Editors
                  .AddManyToMany("ExtractSubj_FileCards"
                      , typeof(FileCardAndExtract)
                      , typeof(IManyToManyLeftAssociation<>)
                      , ManyToManyAssociationType.Rigth
                      , y => y.TabName(ExtractTabs.FileCards8).Visible(true).Order(1))
              ;

            return conf;
        }


        /// <summary>
        /// Конфигурация реестра выписок ЮЛ по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ExtractSubj> ListView_Default(this ViewModelConfigBuilder<ExtractSubj> conf)
        {
            return
                conf.ListView(x => x
                .Title("Выписки")
                 .Columns(col => col
                    .Add(c => c.SubjectRecord, ac => ac.Title("Субъект").Visible(true).Order(1))
                    .Add(c => c.ExtractDate, ac => ac.Title("Дата выписки").Visible(true).Order(1))
                    .Add(c => c.ExtractNumber, ac => ac.Title("Номер выписки").Visible(true).Order(2))
                    .Add(c => c.StartDate, ac => ac.Title("Дата начала").Visible(true).Order(3))
                    .Add(c => c.EndDate, ac => ac.Title("Дата окончания").Visible(true).Order(4))
                    .Add(c => c.NumberRequest, ac => ac.Title("Номер запроса").Visible(true).Order(5))
                    .Add(c => c.DateRequest, ac => ac.Title("Дата запроса").Visible(true).Order(5))
                    .Add(c => c.CountRights, ac => ac.Title("Кол-во прав").Visible(true).Order(5))
                    .Add(c => c.CountObjects, ac => ac.Title("Кол-во ОНИ").Visible(true).Order(5))                 
                    .Add(c => c.ID, ac => ac.Title("Идентификатор").Visible(false).Order(6))
                    .Add(c => c.SibUser, ac => ac.Title("Пользователь").Visible(true).Order(7))
                    .Add(c => c.isAccept, ac => ac.Visible(true).Order(8))
                    .Add(c => c.Society, ac => ac.Title("Общество группы").Visible(false).Order(1))
                   
                 )
               );

        }
    }
}
