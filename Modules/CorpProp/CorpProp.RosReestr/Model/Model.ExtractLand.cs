using Base;
using Base.Attributes;
using Base.UI;
using Base.UI.Editors;
using CorpProp.Entities.ManyToMany;
using CorpProp.Extentions;
using CorpProp.Helpers;
using CorpProp.RosReestr.Entities;
using CorpProp.RosReestr.Helpers;
using CorpProp.RosReestr.Migration;
using CorpProp.RosReestr.Services;
using CorpProp.RosReestr.Tabs;
using System.Linq;

namespace CorpProp.RosReestr.Model
{
    public static class ExtractLandModel
    {
        /// <summary>
        /// Создает конфигурацию модели выписки на ЗУ по умолчанию.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static void CreateModelConfig(this IInitializerContext context)
        {

            context.CreateVmConfig<ExtractLand>()
            .Service<IExtractLandService>()
            .Title("Выписка из ЕГРН - земля")
            .DetailView_Default()
            .ListView_Default()
            .LookupProperty(x => x.Text(t => t.ExtractNumber))
            .IsReadOnly(true)
            .AddQuickView();

        }

        /// <summary>
        /// Конфигурация карточки выписки на ЗУ по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ExtractLand> DetailView_Default(this ViewModelConfigBuilder<ExtractLand> conf)
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

                    //земельный участок
                    .Add(ed => ed.Subtype, ac => ac.Title("Вид ЗУ").TabName(ExtractTabs.Land2))
                    .Add(ed => ed.Category, ac => ac.Title("Категория ЗУ").TabName(ExtractTabs.Land2))
                    .Add(ed => ed.Date_removed_cad_account, ac => ac.Title("Сведения об изъятии").TabName(ExtractTabs.Land2))
                    .Add(ed => ed.Reg_date_by_doc, ac => ac.Title("Дата постановки по документу").TabName(ExtractTabs.Land2))
                    .Add(ed => ed.PermittedBy_document, ac => ac.Title("Вид разрешенного использования по документу").TabName(ExtractTabs.Land2))
                    .Add(ed => ed.PermittedLand_use, ac => ac.Title("Вид разрешенного использования (старый)").TabName(ExtractTabs.Land2))
                    .Add(ed => ed.PermittedLand_use_mer, ac => ac.Title("Вид разрешенного использования (приказ 540 от 01.09.2014)").TabName(ExtractTabs.Land2))
                    .Add(ed => ed.Permittes_Grad_Reg_numb_border, ac => ac.Title("Реестровый номер границы").TabName(ExtractTabs.Land2))
                    .Add(ed => ed.Permittes_Grad_Land_use, ac => ac.Title("Вид использования по градостроительному регламенту").TabName(ExtractTabs.Land2))
                    .Add(ed => ed.Permittes_Grad_use_text, ac => ac.Title("Разрешенное использование").TabName(ExtractTabs.Land2))
                    
                    .AddOneToManyAssociation<ContourOKSOut>("ExtractLand_Contours",
                       editor => editor
                       .TabName(ExtractTabs.Land2)
                       .Title("Описание местоположения границ")
                       .IsReadOnly(true)
                       .IsLabelVisible(true)
                       .Filter((uofw, q, id, oid) =>
                         q.Where(w => w.ExtractID == id)
                       ))

                       
                    //характеристики недвижимости
                    .Add(ed => ed.RegistrationDate, ac => ac.Title("Дата постановки на учет").TabName(ExtractTabs.Estate2))
                    .Add(ed => ed.CancelDate, ac => ac.Title("Дата снятия с учета").TabName(ExtractTabs.Estate2))
                    .Add(ed => ed.CadNumber, ac => ac.Title("Кадастровый номер").TabName(ExtractTabs.Estate2))
                    .Add(ed => ed.Quarter_cad_number, ac => ac.Title("Номер кадастрового квартала").TabName(ExtractTabs.Estate2))
                    .Add(ed => ed.TypeStr, ac => ac.Title("Вид недвижимости").TabName(ExtractTabs.Estate2))
                    .Add(ed => ed.Cost, ac => ac.Title("Кадастровая стоимость").TabName(ExtractTabs.Estate2))
                    .Add(ed => ed.ObjectName, ac => ac.Title("Наименование ОНИ").TabName(ExtractTabs.Estate2))
                    .Add(ed => ed.PurposeStr, ac => ac.Title("Назначение ЗУ").TabName(ExtractTabs.Estate2))
                    .Add(ed => ed.Permitted_usesStr, ac => ac.Title("Вид(ы) разрешенного использования").TabName(ExtractTabs.Estate2))
                    .Add(ed => ed.Address, ac => ac.Title("Адрес (местоположение)").TabName(ExtractTabs.Estate2))
                    .Add(ed => ed.Area, ac => ac.Title("Площадь, в кв. метрах").TabName(ExtractTabs.Estate2))
                    .Add(ed => ed.Inaccuracy, ac => ac.Title("Погрешность площади").TabName(ExtractTabs.Estate2))
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

                    .AddOneToManyAssociation<CadNumber>("ExtractLand_Room_cad_numbers",
                       editor => editor
                       .TabName(ExtractTabs.Estate2)
                       .Title("Кадастровые номера расположенных в пределах ЗУ ОНИ")
                       .IsReadOnly(true)
                       .IsLabelVisible(true)
                       .Filter((uofw, q, id, oid) =>
                         q.Where(w => w.ExtractRoomID == id)
                     ))


                   .AddOneToManyAssociation<ObjectPartNumberRestrictions>("ExtractLand_Object_parts",
                       editor => editor
                       .TabName(ExtractTabs.Estate2)
                       .Title("Сведения о частях ЗУ")
                       .IsReadOnly(true)
                       .IsLabelVisible(true)
                       .Filter((uofw, q, id, oid) =>
                         q.Where(w => w.ExtractID == id)
                     ))


                    //сведения о бесхозяйном имуществе
                    .Add(ed => ed.OwnerlessRightRecordRegDate, ac => ac.Title("Дата регистрации").TabName(ExtractTabs.Ownerless5))
                    .Add(ed => ed.Ownerless_right_number, ac => ac.Title("Номер регистрации").TabName(ExtractTabs.Ownerless5))
                    .Add(ed => ed.Authority_name, ac => ac.Title("Наименование органа").TabName(ExtractTabs.Ownerless5)
                    .Description("Наименование органа местного самоуправления (органа государственной власти - для городов федерального значения Москвы, Санкт-Петербурга, Севастополя), представившего заявление о постановке на учет данного объекта недвижимости в качестве бесхозяйного"))


                    .AddOneToManyAssociation<RightRecord>("ExtractLand_RightRecords",
                       editor => editor
                       .TabName(ExtractTabs.Rights4)
                       .Title("Права ЕГРН")
                       .IsReadOnly(true)
                       .IsLabelVisible(false)
                   .Filter((uofw, q, id, oid) =>
                     q.Where(w => w.ExtractID == id)
                     ))
                   .AddOneToManyAssociation<RestrictRecord>("ExtractLand_RestrictRecords",
                       editor => editor
                       .TabName(ExtractTabs.Encumbrances6)
                       .Title("Обременения/ограничения ЕГРН")
                       .IsReadOnly(true)
                       .IsLabelVisible(false)
                   .Filter((uofw, q, id, oid) =>
                     q.Where(w => w.ExtractID == id)
                     ))
                   .AddOneToManyAssociation<DealRecord>("ExtractLand_Deal_records",
                       editor => editor
                       .TabName(ExtractTabs.Deals7)
                       .Title("Сделки, совершенные без необходимого в силу закона согласия третьего лица, органа")
                       .IsReadOnly(true)
                       .IsLabelVisible(false)
                   .Filter((uofw, q, id, oid) =>
                     q.Where(w => w.ExtractID == id)
                     ))
                     .AddOneToManyAssociation<MigrateLog>("ExtractLand_MigrateLogs",
                        y => y.TabName(ExtractTabs.Logs9)
                            .IsReadOnly(true)
                            .TabName("Журнал миграции")
                            .Filter((uofw, q, id, oid) => q.Where(w => w.MigrateHistory != null && w.MigrateHistory.ExtractID == id))
                        )

              ))
               .Config.DetailView.Editors
                  .AddManyToMany("ExtractLand_FileCards"
                      , typeof(FileCardAndExtract)
                      , typeof(IManyToManyLeftAssociation<>)
                      , ManyToManyAssociationType.Rigth
                      , y => y.TabName(ExtractTabs.FileCards8).Visible(true).Order(1))
              ;
            return conf;
        }


        /// <summary>
        /// Конфигурация реестра выписок на ЗУ по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ExtractLand> ListView_Default(this ViewModelConfigBuilder<ExtractLand> conf)
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

        private static ViewModelConfigBuilder<ExtractLand> AddQuickView(this ViewModelConfigBuilder<ExtractLand> conf)
        {
            conf
                .AddParameterGroup(10)
                .AddSpecificationGroup(20)
                .AddUseGroup(31)
                .AddRightsInfoGroup(35)
                .AddClaimsGroup(45)
                .AddAdditionalGroup(55);
            return conf;
        }

        private static ViewModelConfigBuilder<ExtractLand> AddParameterGroup(this ViewModelConfigBuilder<ExtractLand> conf, int startIndex)
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

        private static ViewModelConfigBuilder<ExtractLand> AddSpecificationGroup(this ViewModelConfigBuilder<ExtractLand> conf, int startIndex)
        {
            var tabName = ExtractTabs.QuickView;
            var groupName = ExtractGroups.Specifications;
            conf.DetailView(x => x.Editors(
                editors => editors
                    //Основные характеристики                    
                    .Add(ed => ed.SubtypeName, ac => ac.Title("Вид объекта недвижимости").TabName(tabName).Group(groupName).Order(startIndex++))
                    .Add(ed => ed.Name, ac => ac.Title("Наименование").TabName(tabName).Group(groupName).Order(startIndex++))
                    .Add(ed => ed.CadNumber, ac => ac.Title("Кадастровый номер").TabName(tabName).Group(groupName).Order(startIndex++))
                    .Add(ed => ed.RegistrationDate, ac => ac.Title("Дата присвоения").TabName(tabName).Group(groupName).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("Ранее присвоенный гос. учетный номер").TabName(tabName).Group(groupName).Order(startIndex++))
                    .Add(ed => ed.Cost, ac => ac.Title("Кадастровая стоимость, руб.").TabName(tabName).Group(groupName).Order(startIndex++))
                    .Add(ed => ed.Address, ac => ac.Title("Адрес:").TabName(tabName).Group(groupName).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("Значение основной характеристики").TabName(tabName).Group(groupName).Order(startIndex++).DataType(PropertyDataType.Label))
                    .AddEmpty(ac => ac.Title("Тип").TabName(tabName).Group(groupName).Order(startIndex++))
                    .Add(ed => ed.Area, ac => ac.Title("Значение").TabName(tabName).Group(groupName).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("Ед.  измерения").TabName(tabName).Group(groupName).Order(startIndex++))
            ));
            return conf;
        }

        private static ViewModelConfigBuilder<ExtractLand> AddUseGroup(this ViewModelConfigBuilder<ExtractLand> conf, int startIndex)
        {
            var tabName = ExtractTabs.QuickView;
            var groupName = ExtractGroups.Use;
            conf.DetailView(x => x.Editors(
                editors => editors
                    //Назначение и использование
                    .Add(ed => ed.Permitted_usesStr, ac => ac.Title("Вид разрешенного использования").TabName(tabName).Group(groupName).Order(startIndex++))
                    .Add(ed => ed.CategoryName, ac => ac.Title("Категория земель").TabName(tabName).Group(groupName).Order(startIndex++))
            ));
            return conf;
        }

        private static ViewModelConfigBuilder<ExtractLand> AddRightsInfoGroup(this ViewModelConfigBuilder<ExtractLand> conf, int startIndex)
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

        private static ViewModelConfigBuilder<ExtractLand> AddClaimsGroup(this ViewModelConfigBuilder<ExtractLand> conf, int startIndex)
        {
            var tabName = ExtractTabs.QuickView;
            var groupName = ExtractGroups.Claims;
            conf.DetailView(x => x.Editors(
                editors => editors
                    //Правопритязания
                    .AddEmpty(ac => ac.Title("Ограничения/обременения").TabName(tabName).Group(groupName).DataType(PropertyDataType.MultilineText).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("Договоры участия в долевом строительстве").TabName(tabName).Group(groupName).DataType(PropertyDataType.MultilineText).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("Заявленные в судебном порядке права требования").TabName(tabName).Group(groupName).DataType(PropertyDataType.MultilineText).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("Сведения о возражении в отношении зарегистрированного права").TabName(tabName).Group(groupName).DataType(PropertyDataType.MultilineText).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("Сведения о наличии решений об изъятии для гос.нужд").TabName(tabName).Group(groupName).DataType(PropertyDataType.MultilineText).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("Сведения о невозможности гос. регистрации без личного участия").TabName(tabName).Group(groupName).DataType(PropertyDataType.MultilineText).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("Правопритязания и сведения о наличии не рассмотренных заявлений о гос. регистрации").TabName(tabName).Group(groupName).DataType(PropertyDataType.MultilineText).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("Сведения об осуществлении гос. регистрации сделки без").TabName(tabName).Group(groupName).DataType(PropertyDataType.MultilineText).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("Сведения о невозможности гос. регистрации перехода, или ограничения на участок из земель с/х назначения").TabName(tabName).Group(groupName).DataType(PropertyDataType.MultilineText).Order(startIndex++))
            ));
            return conf;
        }

        private static ViewModelConfigBuilder<ExtractLand> AddAdditionalGroup(this ViewModelConfigBuilder<ExtractLand> conf, int startIndex)
        {
            var tabName = ExtractTabs.QuickView;
            var groupName = ExtractGroups.Additional;
            conf.DetailView(x => x.Editors(
                editors => editors
                    //Дополнительные сведения
                    .AddEmpty(ac => ac.Title("-Кадастровые номера расположенных в пределах участка объектов недвижимости").TabName(tabName).Group(groupName).DataType(PropertyDataType.MultilineText).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("-Сведения о включении объекта недвижимости в состав предприятия как ИК").TabName(tabName).Group(groupName).DataType(PropertyDataType.MultilineText).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("-Кадастровые объектов недвижимости, из которых образован объект недвижимости").TabName(tabName).Group(groupName).DataType(PropertyDataType.MultilineText).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("-Кадастровые номера образованных объектов").TabName(tabName).Group(groupName).DataType(PropertyDataType.MultilineText).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("-зоны с особыми условиями использования (объект культурного наследия)").TabName(tabName).Group(groupName).DataType(PropertyDataType.MultilineText).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("-о природных объектах расположенных в пределах участка").TabName(tabName).Group(groupName).DataType(PropertyDataType.MultilineText).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("-особой экономической зоны, территории опережающего развития, игорной зоны").TabName(tabName).Group(groupName).DataType(PropertyDataType.MultilineText).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("Сведения о результатах проведения гос. зим. контроля").TabName(tabName).Group(groupName).DataType(PropertyDataType.MultilineText).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("-особо охраняемой природной территории").TabName(tabName).Group(groupName).DataType(PropertyDataType.MultilineText).Order(startIndex++))
                    .AddEmpty(ac => ac.Title("Сведения о расположении земельного участка в границах территории, в отношении которой утвержден ПМТ").TabName(tabName).Group(groupName).DataType(PropertyDataType.MultilineText).Order(startIndex++))
            ));
            return conf;
        }

    }
}
