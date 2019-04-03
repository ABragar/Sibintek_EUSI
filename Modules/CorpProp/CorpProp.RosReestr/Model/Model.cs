using Base;
using Base.UI;
using CorpProp.RosReestr.Entities;
using CorpProp.RosReestr.Migration;
using CorpProp.RosReestr.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Model
{
    public static class RosReestrModel
    {
        /// <summary>
        /// Инициализация моделей модуля Росреестра.
        /// </summary>
        /// <param name="context"></param>
		public static void Init(IInitializerContext context)
        {
            CorpProp.RosReestr.Model.ExtractModel.ModifyModelConfig(context);
            CorpProp.RosReestr.Model.ExtractBuildModel.CreateModelConfig(context);
            CorpProp.RosReestr.Model.ExtractLandModel.CreateModelConfig(context);
            CorpProp.RosReestr.Model.ExtractNZSModel.CreateModelConfig(context);
            CorpProp.RosReestr.Model.ExtractObjectModel.CreateModelConfig(context);
            CorpProp.RosReestr.Model.ExtractSubjModel.CreateModelConfig(context);

            CorpProp.RosReestr.Model.ObjectRecordModel.CreateModelConfig(context);
            CorpProp.RosReestr.Model.RightRecordModel.CreateModelConfig(context);

            #region RosReestr

            context.CreateVmConfig<Migration.MigrateHistory>()
                .ListView(x => x.Title("История миграции"))
                .DetailView(x => x.Title("История миграции")
                .Editors(eds=>eds
                    .AddOneToManyAssociation<MigrateLog>("ImportHelper_ImportErrorLog",
                        y => y.TabName("Журнал миграции")
                            .IsReadOnly(true)
                            .TabName("Журнал миграции")
                            .Create((uofw, entity, id) =>
                            {
                                entity.MigrateHistoryID = id;
                                entity.MigrateHistory = uofw.GetRepository<MigrateHistory>().Find(id);
                            })
                            .Delete((uofw, entity, id) =>
                            {
                                entity.MigrateHistory = null;
                                entity.MigrateHistoryID = null;
                            })
                            .Filter((uofw, q, id, oid) => q.Where(w => w.MigrateHistoryID == id))
                        )

                ))
                .LookupProperty(x=>x.Text(t=>t.ID));

           

            context.CreateVmConfig<Migration.MigrateLog>()
                .ListView(x => x.Title("Журнал миграции")
                .Columns(cols=>cols.Add(c=>c.Mnemonic, ac=>ac.DataType(Base.Attributes.PropertyDataType.ExtraId))))
                .DetailView(x => x.Title("Журнал миграции"))
                .LookupProperty(x => x.Text(t => t.ID))
                .IsReadOnly();
                       

            context.CreateVmConfig<Migration.MigrateState>()
                .ListView(x => x.Title("Статусы миграции"))
                .DetailView(x => x.Title("Статус миграции"))
                .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfig<AnotherSubject>()
               .Service<IAnotherSubjectService>()
               .Title("Иной Субъект")
               .ListView(x => x.Title("Иные Субъекты"))
               .DetailView(x => x.Title("Иной Субъект")
                   .Editors(ed =>
                            ed
                            .Add(e => e.Name)
                            .Add(e => e.Short_name)
                            .Add(e => e.Comment)
                            .Add(e => e.Print_text)
                            .Add(e => e.Registration_organ)
                            .Add(e => e.Email)
                            .Add(e => e.Mailing_addess)
                            .Add(e => e.Aparthouse_owners_name)
                            .Add(e => e.Bonds_number)
                            .Add(e => e.Issue_date)
                            .Add(e => e.Certificate_name)
                            .Add(e => e.Investment_unit_name)
                            .Add(e => e.Equity_participants)
                            .Add(e => e.Not_equity_participants)
                            .Add(e => e.PublicServitude)
                            .Add(e => e.undefined)
                            .AddOneToManyAssociation<SubjectRecord>("AnotherSubject_Partners",
                                editor => editor
                                .Title("Инвестиционное товарищество")
                                .IsLabelVisible(true)
                            .Filter((uofw, q, id, oid) =>
                              q.Where(w => w.PartnerID == id)
                           ))

                   ))
               .LookupProperty(x => x.Text(t => t.Name))
               .IsReadOnly();



            context.CreateVmConfig<BaseParameter>()             
              .Title("Характеристика")
              .ListView(x => x.Title("Характеристики"))
              .DetailView(x => x.Title("Характеристика"))
              .LookupProperty(x => x.Text(t => t.ID))
              .IsReadOnly()
              ;

            context.CreateVmConfig<BuildRecord>()
              .Service<IBuildRecordService>()
              .Title("Здание")
              .ListView(x => x.Title("Здания"))
              .DetailView(x => x.Title("Здание")
              .Editors(ed =>
                     ed.AddOneToManyAssociation<RightRecord>("BuildRecord_RightRecords",
                        editor => editor
                        .TabName("[004]Права")
                        .Title("Права")
                        .IsReadOnly(true)
                        .IsLabelVisible(false)
                    .Filter((uofw, q, id, oid) =>
                      q.Where(w => w.ObjectRecordID == id)
                      ))
                    .AddOneToManyAssociation<RestrictRecord>("BuildRecord_RestrictRecords",
                        editor => editor
                        .TabName("[006]Обременения/ограничения")
                        .Title("Обременения/ограничения")
                        .IsReadOnly(true)
                        .IsLabelVisible(false)
                    .Filter((uofw, q, id, oid) =>
                      q.Where(w => w.ObjectRecordID == id)
                      ))
                    .AddOneToManyAssociation<DealRecord>("BuildRecord_Deal_records",
                        editor => editor
                        .TabName("[007]Сделки, без согласия третьего лица, органа")
                        .Title("Сделки, совершенные без необходимого в силу закона согласия третьего лица, органа")
                        .IsReadOnly(true)
                        .IsLabelVisible(false)
                    .Filter((uofw, q, id, oid) =>
                      q.Where(w => w.ObjectRecordID == id)
                      ))
                    .AddOneToManyAssociation<RoomLocationInBuildPlans>("BuildRecord_Room_records",
                        editor => editor
                        .TabName("[002]Характеристики недвижимости")
                        .Title("Местоположение помещений (план(ы))")
                        .IsReadOnly(true)
                        .IsLabelVisible(true)
                    .Filter((uofw, q, id, oid) =>
                      q.Where(w => w.ObjectRecordID == id)
                      ))
                    .AddOneToManyAssociation<CarParkingSpaceLocationInBuildPlans>("BuildRecord_Car_parking_space_records",
                        editor => editor
                        .TabName("[002]Характеристики недвижимости")
                        .Title("Местоположение машино-мест (план(ы))")
                        .IsReadOnly(true)
                        .IsLabelVisible(true)
                    .Filter((uofw, q, id, oid) =>
                      q.Where(w => w.ObjectRecordID == id)
                      ))
                   .AddOneToManyAssociation<CadNumber>("BuildRecord_Land_cad_numbers",
                        editor => editor
                        .TabName("[002]Характеристики недвижимости")
                        .Title("Кадастровые номера (ЗУ)")
                        .IsReadOnly(true)
                        .IsLabelVisible(true)
                    .Filter((uofw, q, id, oid) =>
                      q.Where(w => w.ObjectRecordLandID == id)
                      ))
                    .AddOneToManyAssociation<CadNumber>("BuildRecord_Room_cad_numbers2",
                        editor => editor
                        .TabName("[002]Характеристики недвижимости")
                        .Title("Кадастровые номера помещений")
                        .IsReadOnly(true)
                        .IsLabelVisible(true)
                    .Filter((uofw, q, id, oid) =>
                      q.Where(w => w.ObjectRecordRoomID == id)
                      ))
                      .AddOneToManyAssociation<CadNumber>("BuildRecord_Car_parking_space_cad_numbers",
                        editor => editor
                        .TabName("[002]Характеристики недвижимости")
                        .Title("Кадастровые номера машино-мест")
                        .IsReadOnly(true)
                        .IsLabelVisible(true)
                    .Filter((uofw, q, id, oid) =>
                      q.Where(w => w.ObjectRecordCarParkingID == id)
                      ))
                    .AddOneToManyAssociation<OldNumber>("BuildRecord_Old_numbers",
                        editor => editor
                        .TabName("[002]Характеристики недвижимости")
                        .Title("Ранее присвоенные номера")
                        .IsReadOnly(true)
                        .IsLabelVisible(true)
                    .Filter((uofw, q, id, oid) =>
                      q.Where(w => w.ObjectRecordID == id)
                      ))
                    .AddOneToManyAssociation<PermittedUse>("BuildRecord_Permitted_uses",
                        editor => editor
                        .TabName("[002]Характеристики недвижимости")
                        .Title("Вид(ы) разрешенного использования")
                        .IsReadOnly(true)
                        .IsLabelVisible(true)
                    .Filter((uofw, q, id, oid) =>
                      q.Where(w => w.ObjectRecordID == id)
                      ))
                    .AddOneToManyAssociation<ObjectPartNumberRestrictions>("BuildRecord_Object_parts",
                        editor => editor
                        .TabName("[002]Характеристики недвижимости")
                        .Title("Сведения о частях здания")
                        .IsReadOnly(true)
                        .IsLabelVisible(true)
                    .Filter((uofw, q, id, oid) =>
                      q.Where(w => w.ObjectRecordID == id)
                      ))
                    .AddOneToManyAssociation<ContourOKSOut>("BuildRecord_Contours",
                        editor => editor
                        .TabName("[002]Характеристики недвижимости")
                        .Title("Описание местоположения контура здания")
                        .IsReadOnly(true)
                        .IsLabelVisible(true)
                    .Filter((uofw, q, id, oid) =>
                      q.Where(w => w.ObjectRecordID == id)
                      ))
               ))
              .LookupProperty(x => x.Text(t => t.Name))
              .IsReadOnly()
               ;

            context.CreateVmConfig<CadNumber>()
             .Service<ICadNumberService>()
             .Title("Кадастровый номер")
             .ListView(x => x.Title("Кадастровые номера"))
             .DetailView(x => x.Title("Кадастровый номер"))
             .LookupProperty(x => x.Text(t => t.Cad_number))
             .IsReadOnly()
             ;

            context.CreateVmConfig<CarParkingSpaceLocationInBuildPlans>()
               .Service<ICarParkingSpaceLocationInBuildPlansService>()
               .Title("Местоположение машино-мест в ОНИ")
               .ListView(x => x.Title("Местоположение машино-мест в ОНИ"))
               .DetailView(x => x.Title("Местоположение машино-мест в ОНИ"))
               .LookupProperty(x => x.Text(t => t.ID))
               .IsReadOnly()
               ;

            context.CreateVmConfig<ContourOKSOut>()
              .Service<IContourOKSOutService>()
              .Title("Контур ОКС")
              .ListView(x => x.Title("Контур ОКС"))
              .DetailView(x => x.Title("Контур ОКС"))
              .LookupProperty(x => x.Text(t => t.ID))
              .IsReadOnly()
              ;

            context.CreateVmConfig<DealRecord>()
             .Service<IDealRecordService>()
             .Title("Сделка без согласия")
             .ListView(x => x.Title("Сделки без согласия"))
             .DetailView(x => x.Title("Сделка без согласия"))
             .LookupProperty(x => x.Text(t => t.ID))
             .IsReadOnly()
              ;


            context.CreateVmConfig<DocumentRecord>()
               .Service<IDocumentRecordService>()
               .Title("Документ-основание ЕГРН")
               .ListView(x => x.Title("Документы-основание ЕГРН"))
               .DetailView(x => x.Title("Документ-основание ЕГРН"))
               .LookupProperty(x => x.Text(t => t.Content))
               .IsReadOnly()
               ;


            context.CreateVmConfig<IndividualSubject>()
               .Service<IIndividualSubjectService>()
               .Title("Физическое лицо")
               .ListView(x => x.Title("Физические лица"))
               .DetailView(x => x.Title("Физическое лицо")
                .Editors(ed =>
                            ed
                            .Add(e => e.Individual_typeCode)
                            .Add(e => e.Individual_typeName)
                            //.Add(e => e.Surname)
                            //.Add(e => e.FirstName)
                            //.Add(e => e.Patronymic)
                            //.Add(e => e.Birth_date)
                            //.Add(e => e.Birth_place)
                            //.Add(e => e.No_citizenship)
                            //.Add(e => e.Citizenship_countryCode)
                            //.Add(e => e.Citizenship_countryName)
                            //.Add(e => e.Snils)
                            //.Add(e => e.Email)
                            //.Add(e => e.Mailing_addess)
                   )
               )
               .LookupProperty(x => x.Text(t => t.Name))
               .IsReadOnly()
               ;



            context.CreateVmConfig<LandRecord>()
               .Service<ILandRecordService>()
               .Title("Земельный участок ЕГРН")
               .ListView(x => x.Title("Земельные участки ЕГРН"))
               .DetailView(x => x.Title("Земельный участок ЕГРН")
               .Editors(ed =>
                    ed
                    .Add(editor => editor.Floors, action => action.Visible(false))
                    .Add(editor => editor.Floors, action => action.Visible(false))
                    .Add(editor => editor.Underground_floors, action => action.Visible(false))
                    .Add(editor => editor.PurposeStr, action => action.Visible(false))
                    .Add(editor => editor.Year_built, action => action.Visible(false))
                    .Add(editor => editor.Year_commisioning, action => action.Visible(false))
                    .Add(editor => editor.Permitted_usesStr, action => action.Visible(false))

                    .Add(editor => editor.Subtype, action => action.Visible(true))
                    .Add(editor => editor.Date_removed_cad_account, action => action.Visible(true))
                    .Add(editor => editor.Reg_date_by_doc, action => action.Visible(true))
                    .Add(editor => editor.Category, action => action.Visible(true))
                    .Add(editor => editor.PermittedBy_document, action => action.Visible(true))
                    .Add(editor => editor.PermittedLand_use, action => action.Visible(true))
                    .Add(editor => editor.PermittedLand_use_mer, action => action.Visible(true))
                    .Add(editor => editor.Permittes_Grad_Reg_numb_border, action => action.Visible(true))
                    .Add(editor => editor.Permittes_Grad_Land_use, action => action.Visible(true))
                    .Add(editor => editor.Permittes_Grad_use_text, action => action.Visible(true))
                    .Add(editor => editor.Inaccuracy, action => action.Visible(true))
                    .Add(editor => editor.In_boundaries_mark, action => action.Visible(true))
                    .Add(editor => editor.Ref_point_name, action => action.Visible(true))
                    .Add(editor => editor.Location_description, action => action.Visible(true))
                    .AddOneToManyAssociation<RightRecord>("LandRecord_RightRecords",
                       editor => editor
                       .TabName("[004]Права")
                       .Title("Права")
                       .IsReadOnly(true)
                       .IsLabelVisible(false)
                   .Filter((uofw, q, id, oid) =>
                     q.Where(w => w.ObjectRecordID == id)
                     ))
                   .AddOneToManyAssociation<RestrictRecord>("LandRecord_RestrictRecords",
                       editor => editor
                       .TabName("[006]Обременения/ограничения")
                       .Title("Обременения/ограничения")
                       .IsReadOnly(true)
                       .IsLabelVisible(false)
                   .Filter((uofw, q, id, oid) =>
                     q.Where(w => w.ObjectRecordID == id)
                     ))
                   .AddOneToManyAssociation<DealRecord>("LandRecord_Deal_records",
                       editor => editor
                       .TabName("[007]Сделки, без согласия третьего лица, органа")
                       .Title("Сделки, совершенные без необходимого в силу закона согласия третьего лица, органа")
                       .IsReadOnly(true)
                       .IsLabelVisible(false)
                   .Filter((uofw, q, id, oid) =>
                     q.Where(w => w.ObjectRecordID == id)
                     ))
                   .AddOneToManyAssociation<CadNumber>("LandRecord_Room_cad_numbers",
                       editor => editor
                       .TabName("[002]Характеристики недвижимости")
                       .Title("Кадастровые номера расположенных в пределах ЗУ ОНИ")
                       .IsReadOnly(true)
                       .IsLabelVisible(true)
                   .Filter((uofw, q, id, oid) =>
                     q.Where(w => w.ObjectRecordRoomID == id)
                     ))
                   .AddOneToManyAssociation<OldNumber>("LandRecord_Old_numbers",
                       editor => editor
                       .TabName("[002]Характеристики недвижимости")
                       .Title("Ранее присвоенные номера")
                       .IsReadOnly(true)
                       .IsLabelVisible(true)
                   .Filter((uofw, q, id, oid) =>
                     q.Where(w => w.ObjectRecordID == id)
                     ))
                   .AddOneToManyAssociation<ObjectPartNumberRestrictions>("LandRecord_Object_parts",
                       editor => editor
                       .TabName("[002]Характеристики недвижимости")
                       .Title("Сведения о частях ЗУ")
                       .IsReadOnly(true)
                       .IsLabelVisible(true)
                   .Filter((uofw, q, id, oid) =>
                     q.Where(w => w.ObjectRecordID == id)
                     ))
                   .AddOneToManyAssociation<ContourOKSOut>("LandRecord_Contours",
                       editor => editor
                       .TabName("[002]Земельный участок")
                       .Title("Описание местоположения границ")
                       .IsReadOnly(true)
                       .IsLabelVisible(true)
                   .Filter((uofw, q, id, oid) =>
                     q.Where(w => w.ObjectRecordID == id)
                     ))
               ))
               .LookupProperty(x => x.Text(t => t.ID))
               .IsReadOnly()
               ;



            context.CreateVmConfig<LegalSubject>()
               .Service<ILegalSubjectService>()
               .Title("Юридическое лицо")
               .ListView(x => x.Title("Юридические лица"))
               .DetailView(x => x.Title("Юридическое лицо")
                .Editors(ed =>
                            ed
                            .Add(e => e.TypeCode)
                            .Add(e => e.TypeName)
                            .Add(e => e.Full_name)
                            .Add(e => e.Inn)
                            .Add(e => e.Ogrn)
                            .Add(e => e.Incorporation_formCode)
                            .Add(e => e.Incorporation_formName)
                            .Add(e => e.Name)
                            .Add(e => e.Incorporate_countryCode)
                            .Add(e => e.Incorporate_countryName)
                            .Add(e => e.Registration_number)
                            .Add(e => e.Date_state_reg)
                            .Add(e => e.Registration_organ)
                            .Add(e => e.Reg_address_subject)
                            .Add(e => e.Email)
                            .Add(e => e.Mailing_addess)
                   )
               )
               .LookupProperty(x => x.Text(t => t.Name))
               .IsReadOnly()
               ;



            context.CreateVmConfig<NameRecord>()
               .Service<INameRecordService>()
               .Title("Наименование")
               .ListView(x => x.Title("Наименования"))
               .DetailView(x => x.Title("Наименование"))
               .LookupProperty(x => x.Text(t => t.ID))
               .IsReadOnly()
               ;


            context.CreateVmConfig<ObjectPartNumberRestrictions>()
               .Service<IObjectPartNumberRestrictionsService>()
               .Title("Часть ОНИ")
               .ListView(x => x.Title("Часть ОНИ"))
               .DetailView(x => x.Title("Часть ОНИ"))
               .LookupProperty(x => x.Text(t => t.ID))
               .IsReadOnly()
               ;



            context.CreateVmConfig<OldNumber>()
               .Service<IOldNumberService>()
               .Title("Ранее присвоенный кадастровый номер")
               .ListView(x => x.Title("Ранее присвоенные кадастровые номера"))
               .DetailView(x => x.Title("Ранее присвоенный кадастровый номер"))
               .LookupProperty(x => x.Text(t => t.ID))
               .IsReadOnly()
               ;


            context.CreateVmConfig<PermittedUse>()
               .Service<IPermittedUseService>()
               .Title("Вид разрешенного использования ЕГРН")
               .ListView(x => x.Title("Виды разрешенного использования ЕГРН"))
               .DetailView(x => x.Title("Вид разрешенного использования ЕГРН"))
               .LookupProperty(x => x.Text(t => t.ID))
               .IsReadOnly()
               ;



            context.CreateVmConfig<PublicSubject>()
               .Service<IPublicSubjectService>()
               .Title("Публично-правовое образование")
               .ListView(x => x.Title("Публично-правовые образования"))
               .DetailView(x => x.Title("Публично-правовое образование")
                .Editors(ed =>
                            ed
                            .Add(e => e.Name)
                            .Add(e => e.Inn)
                            .Add(e => e.ForeignPublicCode)
                            .Add(e => e.ForeignPublicName)
                            .Add(e => e.MunicipalityName)
                            .Add(e => e.RussiaCode)
                            .Add(e => e.RussiaName)
                            .Add(e => e.SubjectOfRFCode)
                            .Add(e => e.SubjectOfRFName)
                            .Add(e => e.UnionStateName)
                            .Add(e => e.Email)
                            .Add(e => e.Mailing_addess)
                   )
               )
               .LookupProperty(x => x.Text(t => t.Name))
               .IsReadOnly()
               ;


            context.CreateVmConfig<RestrictedRightsPartyOut>()
               .Service<IRestrictedRightsPartyOutService>()
               .Title("Выгодополучаетль")
               .ListView(x => x.Title("Выгодополучаетли"))
               .DetailView(x => x.Title("Выгодополучаетль"))
               .LookupProperty(x => x.Text(t => t.ID))
               .IsReadOnly()
               ;


            context.CreateVmConfig<RestrictRecord>()
               .Service<IRestrictRecordService>()
               .Title("Ограничение/Обременение ЕГРН")
               .ListView(x => x.Title("Ограничения/Обременения ЕГРН"))
               .DetailView(x => x.Title("Ограничение/Обременение ЕГРН")
                .Editors(ed =>
                     ed.AddOneToManyAssociation<RightRecordNumber>("RestrictRecord_Restricting_rights",
                        editor => editor
                        .TabName("[001]Ограничиваемые права")
                        .Title("Ограничиваемые права")
                        .IsReadOnly(true)
                        .IsLabelVisible(false)
                    .Filter((uofw, q, id, oid) =>
                      q.Where(w => w.RestrictRecordID == id)
                      ))
                     .AddOneToManyAssociation<RestrictedRightsPartyOut>("RestrictRecord_RestrictedRightsPartyOuts",
                        editor => editor
                        .TabName("[002]Выгодополучатели")
                        .Title("Сведения о лицах, в пользу которых установлены ограничения права и обременения объекта недвижимости")
                        .IsReadOnly(true)
                        .IsLabelVisible(false)
                    .Filter((uofw, q, id, oid) =>
                      q.Where(w => w.RestrictRecordID == id)
                      ))
                     .AddOneToManyAssociation<DocumentRecord>("RestrictRecord_Underlying_documents",
                        editor => editor
                        .TabName("[004]Документы-основание")
                        .Title("Документы-основание")
                        .IsReadOnly(true)
                        .IsLabelVisible(false)
                    .Filter((uofw, q, id, oid) =>
                      q.Where(w => w.RestrictRecordID == id)
                      ))
                     .AddOneToManyAssociation<DealRecord>("RestrictRecord_Third_party_consents",
                        editor => editor
                        .TabName("[005]Сделки без согласия")
                        .Title("Сделки без согласия")
                        .IsReadOnly(true)
                        .IsLabelVisible(false)
                    .Filter((uofw, q, id, oid) =>
                      q.Where(w => w.RestrictRecordID == id)
                      ))

               ))
               .LookupProperty(x => x.Text(t => t.ID))
               .IsReadOnly()
               ;

            context.CreateVmConfig<RightHolder>()
              .Service<IRightHolderService>()
              .Title("Правобладатель")
              .ListView(x => x.Title("Правобладатели"))
              .DetailView(x => x.Title("Правобладатель"))
              .LookupProperty(x => x.Text(t => t.ID))
              .IsReadOnly()
              ;

            

            context.CreateVmConfig<RightRecordNumber>()
               .Service<IRightRecordNumberService>()
               .Title("Номер реестровой записи о вещном праве")
               .ListView(x => x.Title("Номера реестровых записей о вещном праве"))
               .DetailView(x => x.Title("Номер реестровой записи о вещном праве"))
               .LookupProperty(x => x.Text(t => t.ID))
               .IsReadOnly()
               ;

            context.CreateVmConfig<RoomLocationInBuildPlans>()
               .Service<IRoomLocationInBuildPlansService>()
               .Title("Местоположение помещения")
               .ListView(x => x.Title("Местоположения помещений"))
               .DetailView(x => x.Title("Местоположение помещения"))
               .LookupProperty(x => x.Text(t => t.ID))
               .IsReadOnly()
               ;

            context.CreateVmConfig<SubjectRecord>()
               .Service<ISubjectRecordService>()
               .Title("Субъект")
               .ListView(x => x.Title("Субъекты"))
               .DetailView(x => x.Title("Субъект"))
               .LookupProperty(x => x.Text(t => t.Name))
               .IsReadOnly()
               ;


            #endregion

        }
    }
}
