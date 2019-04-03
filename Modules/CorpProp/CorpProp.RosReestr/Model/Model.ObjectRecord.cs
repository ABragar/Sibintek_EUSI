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
    public static class ObjectRecordModel
    {
        /// <summary>
        /// Создает конфигурацию модели выписки о правах ЮЛ по умолчанию.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static void CreateModelConfig(this IInitializerContext context)
        {

            context.CreateVmConfig<ObjectRecord>()
            .Service<IObjectRecordService>()
            .Title("ОНИ")
            .DetailView_Default()
            .ListView_Default()
            .LookupProperty(x => x.Text(t => t.CadastralNumber))
            .IsReadOnly(true);

        }

        /// <summary>
        /// Конфигурация карточки выписки ЮЛ по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ObjectRecord> DetailView_Default(this ViewModelConfigBuilder<ObjectRecord> conf)
        {

            return
                conf.DetailView(x => x
               .Title(conf.Config.Title)
               .Editors(eds=>eds
               .Add(ed=>ed.CadastralNumber , ac=>ac.Title("Кадастровый номер").Visible(true).Order(1))
               .Add(ed => ed.TypeValue, ac => ac.Title("Тип ОНИ").Visible(true).Order(1))
               .Add(ed => ed.Name, ac => ac.Title("Наименование").Visible(true).Order(1))
               .Add(ed => ed.PurposeName, ac => ac.Title("Назначение").Visible(true).Order(1))
               .Add(ed => ed.GroundCategoryText, ac => ac.Title("Категория земель").Visible(true).Order(1))
               .Add(ed => ed.AreaText, ac => ac.Title("Площадь (текст)").Visible(true).Order(1))
               .Add(ed => ed.Address, ac => ac.Title("Адрес").Visible(true).Order(1))
               .Add(ed => ed.RegionCode, ac => ac.Title("Регион (код)").Visible(true).Order(1))
               .Add(ed => ed.RegionName, ac => ac.Title("Регион (наименование)").Visible(true).Order(1))

               .Add(ed => ed.Area, ac => ac.Visible(true))
               .Add(ed => ed.CadastralNumber, ac => ac.Visible(true))

               .AddOneToManyAssociation<ObjectPartNumberRestrictions>("ObjectRecord_Object_parts",
                        editor => editor
                        .TabName("[002]Характеристики недвижимости")
                        .Title("Сведения о частях здания")
                        .IsReadOnly(true)
                        .IsLabelVisible(true)
                    .Filter((uofw, q, id, oid) =>
                      q.Where(w => w.ObjectRecordID == id)
                      ))

               .AddOneToManyAssociation<RightRecord>("ObjectRecord_RightRecords",
                        editor => editor
                        .TabName("[004]Права")
                        .Title("Права")
                        .IsReadOnly(true)
                        .IsLabelVisible(false)
                    .Filter((uofw, q, id, oid) =>
                      q.Where(w => w.ObjectRecordID == id)
                      ))
                    .AddOneToManyAssociation<RestrictRecord>("ObjectdRecord_RestrictRecords",
                        editor => editor
                        .TabName("[006]Обременения/ограничения")
                        .Title("Обременения/ограничения")
                        .IsReadOnly(true)
                        .IsLabelVisible(false)
                    .Filter((uofw, q, id, oid) =>
                      q.Where(w => w.ObjectRecordID == id)
                      ))

               )
              );
        }


        /// <summary>
        /// Конфигурация реестра выписок ЮЛ по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<ObjectRecord> ListView_Default(this ViewModelConfigBuilder<ObjectRecord> conf)
        {
            return
                conf.ListView(x => x
                .Title("ОНИ")
                .Columns(cols=>cols               
                .Add(col => col.CadastralNumber, ac => ac.Title("Кадастровый номер").Visible(true).Order(1))
                .Add(col => col.TypeValue, ac => ac.Title("Тип ОНИ").Visible(true).Order(2))
                .Add(col => col.Name, ac => ac.Title("Наименование").Visible(true).Order(3))
                .Add(col => col.PurposeName, ac => ac.Title("Назначение").Visible(true).Order(4))
                .Add(col => col.GroundCategoryText, ac => ac.Title("Категория земель").Visible(true).Order(5))
                .Add(col => col.AreaText, ac => ac.Title("Площадь (текст)").Visible(true).Order(6))
                .Add(col => col.Address, ac => ac.Title("Адрес").Visible(true).Order(7))
                .Add(col => col.RegionCode, ac => ac.Title("Регион (код)").Visible(true).Order(8))
                .Add(col => col.RegionName, ac => ac.Title("Регион (наименование)").Visible(true).Order(9))              


                )
               );

        }
    }
}
