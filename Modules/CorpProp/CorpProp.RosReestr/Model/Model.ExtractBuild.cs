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
using Base.Attributes;
using SibRosReestr;
using CorpProp.RosReestr.Tabs;

namespace CorpProp.RosReestr.Model
{
    public static class ExtractBuildModel
    {
        /// <summary>
        /// Создает конфигурацию модели выписки на здание по умолчанию.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static void CreateModelConfig(this IInitializerContext context)
        {

            context.CreateVmConfig<ExtractBuild>()
            .Service<IExtractBuildService>()
            .Title("Выписка из ЕГРН - здание")
            .DetailView_Default()
            .ListView_Default()
            .LookupProperty(x => x.Text(t => t.ExtractNumber))
            .IsReadOnly(true)
            .AddQuickView()
            .AddLinkedObjectsTab(); ;     

        }

        /// <summary>
        /// Конфигурация карточки выписки на здание по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ExtractBuild> DetailView_Default(this ViewModelConfigBuilder<ExtractBuild> conf)
        {           

            
                conf.DetailView(x => x
               .Title(conf.Config.Title)
               .Editors(editors => editors
                    //реквизиты выписки
                    .Add(c => c.isAccept, ac => ac.TabName(ExtractTabs.Info1).Visible(true).Order(-1))
                    .Add(ed => ed.SibUser, ac => ac.Title("Пользователь").TabName(ExtractTabs.Info1).Order(1))
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
                    .Add(ed => ed.ObjectName, ac => ac.Title("Наименование здания").TabName(ExtractTabs.Estate2))
                    .Add(ed => ed.PurposeStr, ac => ac.Title("Назначение здания").TabName(ExtractTabs.Estate2))
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
                   
                    .Add(ed => ed.Year_built, ac => ac.Title("Год завершения строительства").TabName(ExtractTabs.Estate2))
                    .Add(ed => ed.Year_commisioning, ac => ac.Title("Год ввода в эксплуатацию по завершении строительства").TabName(ExtractTabs.Estate2))                                      
                    .Add(ed => ed.Special_notes, ac => ac.Title("Особые отметки").TabName(ExtractTabs.Estate2))


                    //сведения о бесхозяйном имуществе
                    .Add(ed => ed.OwnerlessRightRecordRegDate, ac => ac.Title("Дата регистрации").TabName(ExtractTabs.Ownerless5))
                    .Add(ed => ed.Ownerless_right_number, ac => ac.Title("Номер регистрации").TabName(ExtractTabs.Ownerless5))
                    .Add(ed => ed.Authority_name, ac => ac.Title("Наименование органа").TabName(ExtractTabs.Ownerless5)
                    .Description("Наименование органа местного самоуправления (органа государственной власти - для городов федерального значения Москвы, Санкт-Петербурга, Севастополя), представившего заявление о постановке на учет данного объекта недвижимости в качестве бесхозяйного"))



                    #region Collections
                    // .AddOneToManyAssociation<RoomLocationInBuildPlans>("ExtractBuild_Room_records",
                    //    editor => editor
                    //    .TabName(ExtractTabs.Estate2)
                    //    .Title("Местоположение помещений (план(ы))")
                    //    .IsReadOnly(true)
                    //    .IsLabelVisible(true)
                    //.Filter((uofw, q, id, oid) =>
                    //  q.Where(w => w.ExtractID == id)
                    //  ))

                    //.AddOneToManyAssociation<CarParkingSpaceLocationInBuildPlans>("ExtractBuild_Car_parking_space_records",
                    //    editor => editor
                    //    .TabName(ExtractTabs.Estate2)
                    //    .Title("Местоположение машино-мест (план(ы))")
                    //    .IsReadOnly(true)
                    //    .IsLabelVisible(true)
                    //.Filter((uofw, q, id, oid) =>
                    //  q.Where(w => w.ExtractID == id)
                    //  ))

                    // .AddOneToManyAssociation<CadNumber>("ExtractBuild_Land_cad_numbers",
                    //    editor => editor
                    //    .TabName(ExtractTabs.Estate2)
                    //    .Title("Кадастровые номера (ЗУ)")
                    //    .IsReadOnly(true)
                    //    .IsLabelVisible(true)
                    //.Filter((uofw, q, id, oid) =>
                    //  q.Where(w => w.ExtractLandID == id)
                    //  ))

                    //  .AddOneToManyAssociation<CadNumber>("ExtractBuild_Room_cad_numbers",
                    //    editor => editor
                    //    .TabName(ExtractTabs.Estate2)
                    //    .Title("Кадастровые номера помещений")
                    //    .IsReadOnly(true)
                    //    .IsLabelVisible(true)
                    //.Filter((uofw, q, id, oid) =>
                    //  q.Where(w => w.ExtractRoomID == id)
                    //  ))

                    // .AddOneToManyAssociation<CadNumber>("ExtractBuild_Car_parking_space_cad_numbers",
                    //    editor => editor
                    //    .TabName(ExtractTabs.Estate2)
                    //    .Title("Кадастровые номера машино-мест")
                    //    .IsReadOnly(true)
                    //    .IsLabelVisible(true)
                    // .Filter((uofw, q, id, oid) =>
                    //  q.Where(w => w.ExtractCarParkingID == id)
                    //  ))
                    // .AddOneToManyAssociation<OldNumber>("ExtractBuild_Old_numbers",
                    //    editor => editor
                    //    .TabName(ExtractTabs.Estate2)
                    //    .Title("Ранее присвоенные номера")
                    //    .IsReadOnly(true)
                    //    .IsLabelVisible(true)
                    //.Filter((uofw, q, id, oid) =>
                    //  q.Where(w => w.ExtractID == id)
                    //  ))

                    //.AddOneToManyAssociation<PermittedUse>("ExtractBuild_Permitted_uses",
                    //    editor => editor
                    //    .TabName(ExtractTabs.Estate2)
                    //    .Title("Вид(ы) разрешенного использования")
                    //    .IsReadOnly(true)
                    //    .IsLabelVisible(true)
                    //.Filter((uofw, q, id, oid) =>
                    //  q.Where(w => w.ExtractID == id)
                    //  ))

                    //.AddOneToManyAssociation<ObjectPartNumberRestrictions>("ExtractBuild_Object_parts",
                    //    editor => editor
                    //    .TabName(ExtractTabs.Estate2)
                    //    .Title("Сведения о частях здания")
                    //    .IsReadOnly(true)
                    //    .IsLabelVisible(true)
                    //.Filter((uofw, q, id, oid) =>
                    //  q.Where(w => w.ExtractID == id)
                    //  ))
                    //.AddOneToManyAssociation<ContourOKSOut>("ExtractBuild_Contours",
                    //    editor => editor
                    //    .TabName(ExtractTabs.Estate2)
                    //    .Title("Описание местоположения контура здания")
                    //    .IsReadOnly(true)
                    //    .IsLabelVisible(true)
                    //.Filter((uofw, q, id, oid) =>
                    //  q.Where(w => w.ExtractID == id)
                    //  ))
               #endregion

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
                       .Title("Обременения/ограничения ЕГРН")
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

                     .AddOneToManyAssociation<MigrateLog>("ExtractBuild_MigrateLogs",
                        y => y.TabName(ExtractTabs.Logs9)
                            .IsReadOnly(true)
                            .TabName("Журнал миграции")                            
                            .Filter((uofw, q, id, oid) => q.Where(w => w.MigrateHistory != null && w.MigrateHistory.ExtractID == id))
                        )

              ))
               .Config.DetailView.Editors
                  .AddManyToMany("ExtractBuild_FileCards"
                      , typeof(FileCardAndExtract)
                      , typeof(IManyToManyLeftAssociation<>)
                      , ManyToManyAssociationType.Rigth
                      , y => y.TabName(ExtractTabs.FileCards8).Visible(true).Order(1))
              ;
            return conf;
        }


        /// <summary>
        /// Конфигурация реестра выписок на здание по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ExtractBuild> ListView_Default(this ViewModelConfigBuilder<ExtractBuild> conf)
        {
            return
                conf.ListView(x => x
                .Title("Выписки ЕГРН - здание")
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
                 )
               );

        }

        /// <summary>
        /// Закладка Быстрый просмотр
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        private static ViewModelConfigBuilder<ExtractBuild> AddQuickView(this ViewModelConfigBuilder<ExtractBuild> conf)
        {
            conf
                .AddParameterGroup(10)
                .AddSpecificationGroup(20)
                .AddUseGroup(35)
                .AddRightsInfoGroup(40)
                .AddClaimsGroup(50)
                .AddAdditionalGroup(70);
            return conf;
        }

        private static ViewModelConfigBuilder<ExtractBuild> AddLinkedObjectsTab(this ViewModelConfigBuilder<ExtractBuild> conf)
        {
            conf
                .AddLinkedObjectsGroup(10);
            return conf;
        }

        private static ViewModelConfigBuilder<ExtractBuild> AddParameterGroup(this ViewModelConfigBuilder<ExtractBuild> conf, int startIndex)
        {
            var tabName = ExtractTabs.QuickView;
            var groupName = ExtractGroups.Parameters;
            conf.DetailView(x => x.Editors(
                editors => editors
                    //Параметры запроса/выписки
                    .Add(ed => ed.ExtractNumber, ac => ac.Title("Номер выписки").TabName(tabName).Group(groupName).Order(startIndex++))
                    .Add(ed => ed.DateRequest, ac => ac.Title("Дата запроса").TabName(tabName).Group(groupName).Order(startIndex++))
                    .Add(ed => ed.ExtractDate, ac => ac.Title("Дата рассмотрения").TabName(tabName).Group(groupName).Order(startIndex++))
                    .Add(ed => ed.DateUpload, ac => ac.Title("Дата выгрузки").TabName(tabName).Group(groupName).Order(startIndex++))
            ));
            return conf;
        }
        private static ViewModelConfigBuilder<ExtractBuild> AddSpecificationGroup(this ViewModelConfigBuilder<ExtractBuild> conf, int startIndex)
        {
            var tabName = ExtractTabs.QuickView;
            var groupName = ExtractGroups.Specifications;
            conf.DetailView(x => x.Editors(
                editors => editors
                    //Основные характеристики                    
                    .AddEmpty(ac => ac.Title("Вид объекта недвижимости").TabName(tabName).Group(groupName).Order(startIndex++))
                    .Add(ed => ed.Name, ac => ac.Title("Наименование").TabName(tabName).Group(groupName).Order(startIndex++))
                    .Add(ed => ed.CadNumber, ac => ac.Title("Кадастровый номер").TabName(tabName).Group(groupName).Order(startIndex++))
                    .Add(ed => ed.RegistrationDate, ac => ac.Title("Дата присвоения").TabName(tabName).Group(groupName).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("Ранее присвоенный гос. учетный номер").TabName(tabName).Group(groupName).Order(startIndex++))
                    .Add(ed => ed.Cost, ac => ac.Title("Кадастровая стоимость, руб.").TabName(tabName).Group(groupName).Order(startIndex++))
                    .Add(ed => ed.Address, ac => ac.Title("Адрес:").TabName(tabName).Group(groupName).Order(startIndex++))
                    .AddEmpty(ac=>ac.TabName(tabName).Group(groupName).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("Основная характеристика").TabName(tabName).Group(groupName).Order(startIndex++).DataType(PropertyDataType.Label))
                    .AddEmpty(ac => ac.Title("Тип").TabName(tabName).Group(groupName).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("Значение").TabName(tabName).Group(groupName).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("Ед. измерения").TabName(tabName).Group(groupName).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("Год ввода в эксплуатацию по завершении строительства").TabName(tabName).Group(groupName).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("Год завершения строительства").TabName(tabName).Group(groupName).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("Количество этажей").TabName(tabName).Group(groupName).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("Количество этажей").TabName(tabName).Group(groupName).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("Материал наружных стен").TabName(tabName).Group(groupName).Order(startIndex++))
            ));
            return conf;
        }
        private static ViewModelConfigBuilder<ExtractBuild> AddUseGroup(this ViewModelConfigBuilder<ExtractBuild> conf, int startIndex)
        {
            var tabName = ExtractTabs.QuickView;
            var groupName = ExtractGroups.Use;
            conf.DetailView(x => x.Editors(
                editors => editors
                    //Назначение и использование
                    .Add(ed => ed.PurposeName, ac => ac.Title("Назначение").TabName(tabName).Group(groupName).Order(startIndex++))
                    .Add(ed => ed.Permitted_usesStr, ac => ac.Title("Вид разрешенного использования").TabName(tabName).Group(groupName).Order(startIndex++))
            ));
            return conf;
        }

        private static ViewModelConfigBuilder<ExtractBuild> AddRightsInfoGroup(this ViewModelConfigBuilder<ExtractBuild> conf, int startIndex)
        {
            var tabName = ExtractTabs.QuickView;
            var groupName = ExtractGroups.RightsInfo;
            conf.DetailView(x => x.Editors(
                editors => editors
                    //Сведения о правах
                    .AddEmpty(ac => ac.Title("Правообладатель (правообладатели)").TabName(tabName).Group(groupName).DataType(PropertyDataType.MultilineText).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("Вид").TabName(tabName).Group(groupName).Order(startIndex++))
                    .AddEmpty(ac => ac.TabName(tabName).Group(groupName).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("Номер").TabName(tabName).Group(groupName).Order(startIndex++))
                    .AddEmpty(ac => ac.TabName(tabName).Group(groupName).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("Дата регистрации права").TabName(tabName).Group(groupName).Order(startIndex++))
            ));
            return conf;
        }

        private static ViewModelConfigBuilder<ExtractBuild> AddClaimsGroup(this ViewModelConfigBuilder<ExtractBuild> conf, int startIndex)
        {
            var tabName = ExtractTabs.QuickView;
            var groupName = ExtractGroups.Claims;
            conf.DetailView(x => x.Editors(
                editors => editors
                    //Правопритязания
                    //todo add label only Ограничения/обременения 
                    .AddEmpty(ac => ac.Title("Вид").TabName(tabName).Group(groupName).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("Дата").TabName(tabName).Group(groupName).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("Номер").TabName(tabName).Group(groupName).Order(startIndex++))

                    .AddEmpty(ac => ac.Title("Срок").TabName(tabName).Group(groupName).DataType(PropertyDataType.MultilineText).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("Лицо").TabName(tabName).Group(groupName).DataType(PropertyDataType.MultilineText).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("Основание").TabName(tabName).Group(groupName).DataType(PropertyDataType.MultilineText).Order(startIndex++))
                    
                    .AddEmpty(ac => ac.Title("Заявленные в судебном порядке права требования").TabName(tabName).Group(groupName).DataType(PropertyDataType.MultilineText).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("Сведения о возражении в отношении зарегистрированного права").TabName(tabName).Group(groupName).DataType(PropertyDataType.MultilineText).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("Сведения о наличии решений об изъятии для гос.нужд").TabName(tabName).Group(groupName).DataType(PropertyDataType.MultilineText).Order(startIndex++))
                    
                    .AddEmpty(ac => ac.Title("Сведения о невозможности гос. регистрации без личного участия").TabName(tabName).Group(groupName).DataType(PropertyDataType.MultilineText).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("Правопритязания и сведения о наличии не рассмотренных заявлений о гос. регистрации").TabName(tabName).Group(groupName).DataType(PropertyDataType.MultilineText).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("Сведения об осуществлении гос. регистрации сделки без необходимого согласия третьего лица").TabName(tabName).Group(groupName).DataType(PropertyDataType.MultilineText).Order(startIndex++))
            ));
            return conf;
        }

        private static ViewModelConfigBuilder<ExtractBuild> AddAdditionalGroup(this ViewModelConfigBuilder<ExtractBuild> conf, int startIndex)
        {
            var tabName = ExtractTabs.QuickView;
            var groupName = ExtractGroups.Additional;
            conf.DetailView(x => x.Editors(
                editors => editors
                    //Дополнительные сведения
                    .AddEmpty(ac => ac.Title("-Кадастровые номера расположенных в пределах участка объектов недвижимости").TabName(tabName).Group(groupName).DataType(PropertyDataType.MultilineText).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("-Сведения о включении объекта недвижимости в состав предприятия как ИК").TabName(tabName).Group(groupName).DataType(PropertyDataType.MultilineText).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("-Кадастровые номера помещений, машино-мест, расположенных в здании или сооружении").TabName(tabName).Group(groupName).DataType(PropertyDataType.MultilineText).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("-Сведения о включении объекта недвижимости в состав ЕНК").TabName(tabName).Group(groupName).DataType(PropertyDataType.MultilineText).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("-Кадастровые объектов недвижимости, из которых образован объект недвижимости").TabName(tabName).Group(groupName).DataType(PropertyDataType.MultilineText).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("-Кадастровый номер земельного участка, если входящие в состав ЕНК объекты расположены на одном земельном участке").TabName(tabName).Group(groupName).DataType(PropertyDataType.MultilineText).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("-Кадастровые номера образованных объектов").TabName(tabName).Group(groupName).DataType(PropertyDataType.MultilineText).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("-Особые отметки:").TabName(tabName).Group(groupName).DataType(PropertyDataType.MultilineText).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("-Сведения о включении объекта недвижимости в реестр объектов культурного наследия").TabName(tabName).Group(groupName).DataType(PropertyDataType.MultilineText).Order(startIndex++))
            ));
            return conf;
        }

        private static ViewModelConfigBuilder<ExtractBuild> AddLinkedObjectsGroup(this ViewModelConfigBuilder<ExtractBuild> conf, int startIndex)
        {
            var tabName = ExtractTabs.LinkedObjects;
            var groupName = ExtractGroups.LinkedObjects;
            conf.DetailView(x => x.Editors(
                editors => editors
                    //Параметры запроса/выписки
                    .AddEmpty(ac => ac.Title("Кадастровые номера иных  объектов недвижимости в пределах которых расположено Здание").TabName(tabName).Group(groupName).Order(startIndex++))
            ));
            return conf;
        }
    }
}
