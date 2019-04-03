using Base;
using Base.UI;
using CorpProp.RosReestr.Entities;
using CorpProp.RosReestr.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Model
{
    public static class RightRecordModel
    {
        /// <summary>
        /// Создает конфигурацию модели выписки о правах ЮЛ по умолчанию.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static void CreateModelConfig(this IInitializerContext context)
        {
            
            context.CreateVmConfig<RightRecord>()
               .Service<IRightRecordService>()
               .Title("Право ЕГРН")
               .ListView_Default()
               .DetailView_Default()               
               .LookupProperty(x => x.Text(t => t.RegNumber))
               .IsReadOnly()
               ;

            context.CreateVmConfig<RightRecord>("CheckboxExtractRights")
               .Service<IRightRecordService>()
               .Title("Право ЕГРН")
               .DetailView_Default()
               .ListView_CheckboxExtractRights()
               .LookupProperty(x => x.Text(t => t.RegNumber))
               .IsReadOnly()
               ;

            //представление объединений права, они, обременений, документов ждя выписок ЮЛ
            context.CreateVmConfig<SubjRight>()               
               .Title("Строки выписки")
               .DetailView(dv=>dv.Title("Строка выписки"))
               .ListView(lv=>lv.Title("Строки выписки")
                .OnClientEditRow(
                   @"var dataItem = grid.dataItem(grid.select());
                 if (dataItem) {
                    if (dataItem.RightRecordID) {
                        id = dataItem.RightRecordID;
                        mnemonic = 'RightRecord';
                    }
                }"))
               .LookupProperty(x => x.Text(t => t.ID))
               .IsReadOnly()
               ;

        }

     
        public static ViewModelConfigBuilder<RightRecord> DetailView_Default(this ViewModelConfigBuilder<RightRecord> conf)
        {

            return
                conf.DetailView(x => x
               .Title(conf.Config.Title)
               .Editors(ed => ed
                   .Add(e => e.isAccept, ac => ac.Visible(true).Order(-1))
                   .Add(e=>e.RegNumber, ac=>ac.Title("Регистрационный номер").Visible(true).Order(1))
                   .Add(e => e.RightHoldersStr, ac => ac.Title("Правообладатели").Visible(true).Order(2))
                   .Add(e => e.RightTypeName, ac => ac.Title("Вид права").Visible(true).Order(3))
                   .Add(e => e.ShareText, ac => ac.Title("Доля").Visible(true).Order(4))
                   .Add(e => e.RegDate, ac => ac.Title("Дата регистрации").Visible(true).Order(5))
                   .Add(e => e.EndDate, ac => ac.Title("Дата прекращения").Visible(true).Order(6))
                   .Add(e => e.CadastralNumber, ac => ac.Title("Кадастровый номер").Visible(true).Order(7))
                   .Add(e => e.ObjectTypeText, ac => ac.Title("Тип ОНИ").Visible(true).Order(8))                   
                   .Add(e => e.Address, ac => ac.Title("Адрес").Visible(true).Order(10))
                   .Add(e => e.ObjectRecord, ac => ac.Title("ОНИ").Visible(true).Order(11))
                   .Add(e => e.Estate, ac => ac.Title("Объект имущества (КС)").Visible(true).Order(12))
                   .Add(e => e.Extract, ac => ac.Title("Выписка").Visible(true).Order(13))

                    .AddOneToManyAssociation<RightHolder>("RightRecord_RightHolders",
                        editor => editor
                        .TabName("[001]Правообладатели")
                        .Title("Правообладатели")
                        .IsReadOnly(true)
                        .IsLabelVisible(true)
                    .Filter((uofw, q, id, oid) =>
                      q.Where(w => w.RightRecordID == id)
                      ))
                     .AddOneToManyAssociation<DocumentRecord>("RightRecord_Underlying_documents2",
                        editor => editor
                        .TabName("[005]Документы-основание")
                        .Title("Документы-основание")
                        .IsReadOnly(true)
                        .IsLabelVisible(false)
                    .Filter((uofw, q, id, oid) =>
                      q.Where(w => w.RightRecordID == id)
                      ))
                   .AddOneToManyAssociation<DealRecord>("RightRecord_DealRecords",
                       editor => editor
                       .TabName("[006]Сделки без согласия")
                       .Title("Сделки без согласия")
                       .IsReadOnly(true)
                       .IsLabelVisible(false)
                   .Filter((uofw, q, id, oid) =>
                     q.Where(w => w.RightRecordID == id)
                     ))
               )
              );
        }


       
        public static ViewModelConfigBuilder<RightRecord> ListView_Default(this ViewModelConfigBuilder<RightRecord> conf)
        {
            return
                conf.ListView(x => x
                .Title("Права ЕГРН")
                .Columns(cols=>cols
                .Add(col => col.ID, ac => ac.Title("Идентификатор").Visible(false).Order(1))
                .Add(col=>col.RightHoldersStr, ac=>ac.Title("Правообладатели").Visible(true).Order(2))
                .Add(col => col.RegNumber, ac => ac.Title("Регистрационный номер").Visible(true).Order(3))
                .Add(col => col.RegDate, ac => ac.Title("Дата регистрации").Visible(true).Order(4))
                .Add(col => col.RightTypeName, ac => ac.Title("Вид, доля").Visible(true).Order(5))
                .Add(col => col.ShareText, ac => ac.Title("Доля").Visible(false).Order(6))
                .Add(col => col.EndDate, ac => ac.Title("Дата прекращения").Visible(true).Order(7))
                .Add(col => col.CadastralNumber, ac => ac.Title("Кадастровый номер").Visible(true).Order(8))
                .Add(col => col.ObjectTypeText, ac => ac.Title("Тип ОНИ").Visible(true).Order(9))
                .Add(col => col.Address, ac => ac.Title("Адрес").Visible(true).Order(10))
                .Add(col => col.ObjectRecord, ac => ac.Title("ОНИ").Visible(true).Order(11))
                .Add(col => col.Extract, ac => ac.Title("Выписка").Visible(false).Order(12))
                .Add(col => col.isAccept, ac => ac.Visible(true).Order(13))
                )
               );

        }


        public static ViewModelConfigBuilder<RightRecord> ListView_CheckboxExtractRights(this ViewModelConfigBuilder<RightRecord> conf)
        {
            return
                conf.ListView_Default()
                .ListView(x=>x.IsMultiSelect(true)
                .Columns(cols => cols                
                .Add(c=>c.UpdateCPStatus, ac=>ac.Visible(true))
                .Add(c => c.UpdateCPDateTime, ac => ac.Visible(true))
                )
                
               );

        }
    }
}
