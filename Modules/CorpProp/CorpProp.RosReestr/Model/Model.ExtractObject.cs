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
    public static class ExtractObjectModel
    {
        /// <summary>
        /// Создает конфигурацию модели выписки ОНИ по умолчанию.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static void CreateModelConfig(this IInitializerContext context)
        {

            context.CreateVmConfig<ExtractObject>()
            .Service<IExtractObjectService>()
            .Title("Выписка из ЕГРН о характеристиках объекта недвижимости")
            .DetailView_Default()
            .ListView_Default()
            .LookupProperty(x => x.Text(t => t.ExtractNumber))
            .IsReadOnly(true);

        }

        /// <summary>
        /// Конфигурация карточки выписки ОНИ по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ExtractObject> DetailView_Default(this ViewModelConfigBuilder<ExtractObject> conf)
        {

            
                conf.DetailView(x => x
               .Title(conf.Config.Title)
               .Editors(editors => editors
                    //реквизиты выписки
                    .Add(ed => ed.Name, ac => ac.Title("Наименование").TabName(ExtractTabs.Info1).IsRequired(true))
                    .Add(ed => ed.ExtractType, ac => ac.Title("Тип выписки").TabName(ExtractTabs.Info1).IsRequired(true))
                    .Add(ed => ed.ExtractFormat, ac => ac.Title("Формат выписки").TabName(ExtractTabs.Info1).IsRequired(true))
                    .Add(ed => ed.ExtractNumber, ac => ac.Title("Регистрационный номер").TabName(ExtractTabs.Info1))
                    .Add(ed => ed.DateUpload, ac => ac.Title("Дата выгрузки").TabName(ExtractTabs.Info1))
                    .Add(ed => ed.DateRequest, ac => ac.Title("Дата запроса").TabName(ExtractTabs.Info1))
                    .Add(ed => ed.NumberRequest, ac => ac.Title("Номер запроса").TabName(ExtractTabs.Info1))
                    .Add(ed => ed.FileCard, ac => ac.Title("Документ").TabName(ExtractTabs.Info1))
                    .Add(ed => ed.SenderName, ac => ac.Title("Орган регистрации прав").TabName(ExtractTabs.Info1))
                    .Add(ed => ed.Appointment, ac => ac.Title("Наименование должности").TabName(ExtractTabs.Info1))
                    .Add(ed => ed.FIO, ac => ac.Title("Инициалы, Фамилия").TabName(ExtractTabs.Info1))
                    .Add(ed => ed.DateReceipt, ac => ac.Title("Дата получения запроса органом регистрации прав").TabName(ExtractTabs.Info1))
                    .Add(ed => ed.ReceivName, ac => ac.Title("Адресат").TabName(ExtractTabs.Info1))
                    .Add(ed => ed.Representativ, ac => ac.Title("Получатель").TabName(ExtractTabs.Info1))
                    .Add(ed => ed.Guid, ac => ac.Title("Глобальный уникальный идентификатор документа").TabName(ExtractTabs.Info1))
                    .Add(ed => ed.Status, ac => ac.Title("Статус записи об объекте недвижимости").TabName(ExtractTabs.Info1))
                    .Add(ed => ed.UpdateCPStatus, ac => ac.Title("Статус обновления в АИС КС").TabName(ExtractTabs.Info1))
                    .Add(ed => ed.UpdateCPDateTime, ac => ac.Title("Дата обновления в АИС КС").TabName(ExtractTabs.Info1))

                    //характеристики недвижимости
                    .Add(ed => ed.RegistrationDate, ac => ac.Title("Дата постановки на учет").TabName(ExtractTabs.Estate2))
                    .Add(ed => ed.CancelDate, ac => ac.Title("Дата снятия с учета").TabName(ExtractTabs.Estate2))
                    .Add(ed => ed.CadNumber, ac => ac.Title("Кадастровый номер").TabName(ExtractTabs.Estate2))
                    .Add(ed => ed.Quarter_cad_number, ac => ac.Title("Номер кадастрового квартала").TabName(ExtractTabs.Estate2))
                    .Add(ed => ed.TypeStr, ac => ac.Title("Вид недвижимости").TabName(ExtractTabs.Estate2))
                    .Add(ed => ed.Cost, ac => ac.Title("Кадастровая стоимость").TabName(ExtractTabs.Estate2))
                    .Add(ed => ed.ObjectName, ac => ac.Title("Наименование").TabName(ExtractTabs.Estate2))
                    .Add(ed => ed.PurposeStr, ac => ac.Title("Назначение").TabName(ExtractTabs.Estate2))
                    .Add(ed => ed.Permitted_usesStr, ac => ac.Title("Вид(ы) разрешенного использования").TabName(ExtractTabs.Estate2))
                    .Add(ed => ed.Address, ac => ac.Title("Адрес (местоположение)").TabName(ExtractTabs.Estate2))
                    .Add(ed => ed.Area, ac => ac.Title("Площадь, в кв. метрах").TabName(ExtractTabs.Estate2))
                    .Add(ed => ed.Floors, ac => ac.Title("Количество этажей (в том числе подземных)").TabName(ExtractTabs.Estate2))
                    .Add(ed => ed.Underground_floors, ac => ac.Title("Количество подземных этажей").TabName(ExtractTabs.Estate2))
                    .Add(ed => ed.Land_cad_numbersStr, ac => ac.Title("Кадастровые номера ЗУ").TabName(ExtractTabs.Estate2)
                    .Description("Кадастровые номера иных объектов недвижимости (земельных участков), в пределах которых расположен объект недвижимости"))
                    .Add(ed => ed.Room_cad_numbersStr, ac => ac.Title("Кадастровые номера помещений").TabName(ExtractTabs.Estate2)
                    .Description("Кадастровые номера помещений, расположенных в объекте недвижимости"))
                    .Add(ed => ed.Car_parking_space_cad_numbersStr, ac => ac.Title("Кадастровые номера машино-мест").TabName(ExtractTabs.Estate2)
                    .Description("Кадастровые номера машино-мест, расположенных в объекте недвижимости"))
                    .Add(ed => ed.Old_numbersStr, ac => ac.Title("Ранее присвоенные номера").TabName(ExtractTabs.Estate2))
                    
                    .Add(ed => ed.Special_notes, ac => ac.Title("Особые отметки").TabName(ExtractTabs.Estate2))


                    //сведения о бесхозяйном имуществе
                    .Add(ed => ed.OwnerlessRightRecordRegDate, ac => ac.Title("Дата регистрации").TabName(ExtractTabs.Ownerless5))
                    .Add(ed => ed.Ownerless_right_number, ac => ac.Title("Номер регистрации").TabName(ExtractTabs.Ownerless5))
                    .Add(ed => ed.Authority_name, ac => ac.Title("Наименование органа").TabName(ExtractTabs.Ownerless5)
                    .Description("Наименование органа местного самоуправления (органа государственной власти - для городов федерального значения Москвы, Санкт-Петербурга, Севастополя), представившего заявление о постановке на учет данного объекта недвижимости в качестве бесхозяйного"))

                    
                    ////////////////////////////////////////////////////////////////////
                    .AddOneToManyAssociation<RightRecord>("ExtractBuild_RightRecords",
                       editor => editor
                       .TabName(ExtractTabs.Rights4)
                       .Title("Права ЕГРН")
                       .IsReadOnly(true)
                       .IsLabelVisible(false)
                   .Filter((uofw, q, id, oid) =>
                     q.Where(w => w.ExtractID == id)
                     ))
                   .AddOneToManyAssociation<RestrictRecord>("ExtractBuild_RestrictRecords",
                       editor => editor
                       .TabName(ExtractTabs.Encumbrances6)
                       .Title("Обременения/ограничения")
                       .IsReadOnly(true)
                       .IsLabelVisible(false)
                   .Filter((uofw, q, id, oid) =>
                     q.Where(w => w.ExtractID == id)
                     ))
                   .AddOneToManyAssociation<DealRecord>("ExtractBuild_Deal_records",
                       editor => editor
                       .TabName(ExtractTabs.Deals7)
                       .Title("Сделки, совершенные без необходимого в силу закона согласия третьего лица, органа")
                       .IsReadOnly(true)
                       .IsLabelVisible(false)
                   .Filter((uofw, q, id, oid) =>
                     q.Where(w => w.ExtractID == id)
                     ))
                   .AddOneToManyAssociation<MigrateLog>("ExtractObject_MigrateLogs",
                        y => y.TabName(ExtractTabs.Logs9)
                            .IsReadOnly(true)
                            .TabName("Журнал миграции")
                            .Filter((uofw, q, id, oid) => q.Where(w => w.MigrateHistory != null && w.MigrateHistory.ExtractID == id))
                        )

              ))
               .Config.DetailView.Editors
                  .AddManyToMany("ExtractObject_FileCards"
                      , typeof(FileCardAndExtract)
                      , typeof(IManyToManyLeftAssociation<>)
                      , ManyToManyAssociationType.Rigth
                      , y => y.TabName(ExtractTabs.FileCards8).Visible(true).Order(1))
              ;
            return conf;
        }


        /// <summary>
        /// Конфигурация реестра выписок ОНИ по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ExtractObject> ListView_Default(this ViewModelConfigBuilder<ExtractObject> conf)
        {
            return
                conf.ListView(x => x
                .Title("Выписки ЕГРН на ОНИ")
                 .Columns(col => col
                     .Add(c => c.ID, ac => ac.Title("Идентификатор").Visible(false).Order(1))
                     .Add(c => c.ExtractType, ac => ac.Title("Тип Выписки").Visible(true).Order(2))
                     .Add(c => c.ExtractNumber, ac => ac.Title("Регистрацонный номер").Visible(true).Order(3))
                     .Add(c => c.DateUpload, ac => ac.Title("Дата выгрузки").Visible(true).Order(4))
                     .Add(c => c.DateRequest, ac => ac.Title("Дата запроса").Visible(true).Order(5))
                     .Add(c => c.NumberRequest, ac => ac.Title("Номер запроса").Visible(true).Order(6))
                     .Add(c => c.ReceivName, ac => ac.Title("Адресат").Visible(true).Order(7))
                     .Add(c => c.Representativ, ac => ac.Title("Получатель").Visible(true).Order(8))
                     .Add(c => c.TypeStr, ac => ac.Title("Вид недвижимости").Visible(true).Order(9))
                     .Add(c => c.CadNumber, ac => ac.Title("Кадастровый номер").Visible(true).Order(10))
                     .Add(c => c.ObjectName, ac => ac.Title("Наименование").Visible(true).Order(11))
                     .Add(c => c.PurposeStr, ac => ac.Title("Назначение").Visible(true).Order(12))
                     .Add(c => c.Area, ac => ac.Title("Площадь").Visible(true).Order(13))
                     .Add(c => c.Cost, ac => ac.Title("Кадастровая стоимость").Visible(true).Order(14))
                     .Add(c => c.SibUser, ac => ac.Title("Пользователь").Visible(true).Order(15))
                     .Add(c => c.isAccept, ac => ac.Visible(true).Order(16))
                 )
               );

        }
    }
}
