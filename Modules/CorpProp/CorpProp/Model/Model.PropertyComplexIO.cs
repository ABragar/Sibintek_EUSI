using Base;
using Base.UI;
using CorpProp.Entities.Estate;
using CorpProp.Entities.ManyToMany;
using CorpProp.Helpers;
using CorpProp.Services.Estate;
using System.Linq;

namespace CorpProp.Model
{
    public static class PropertyComplexIOModel
    {
        public static void CreateModelConfig(this IInitializerContext context)
        {
            context.CreateVmConfig<PropertyComplexIO>()
                .Service<IPropertyComplexIOService>()
                .Title("Имущественный комплекс")
                .ListView(lv => lv
                    .Columns(c => c
                    .Clear()
                        .Add(a => a.ID)
                        .Add(a => a.PCNumber, col => col.Visible(true).Order(1))
                        .Add(a => a.NameEUSI, col => col.Visible(true).Order(2))
                        .Add(a => a.PropertyComplexIOType, col => col.Visible(true).Order(3))
                        .Add(a => a.InventoryObjectsCount, col => col.Visible(true).Order(4))
                        .Add(a => a.InitialCostOBU, col => col.Visible(true).Order(5))
                        .Add(a => a.ResidualCostOBU, col => col.Visible(true).Order(6))
                        .Add(a => a.InitialCostNU, col => col.Visible(true).Order(7))
                        .Add(a => a.ResidualCostNU, col => col.Visible(true).Order(8))
                        .Add(a => a.SibCityNSI, col => col.Visible(true).Order(9))
                        .Add(a => a.SibRegion, col => col.Visible(true).Order(10))
                        .Add(a => a.Country, col => col.Visible(true).Order(11))
                    )
                    .Title("Имущественный комплекс")
                    .DataSource(ds => ds
                        .Filter(f => f.IsPropertyComplex)
                    )
                )
                .DetailView_Default()
                .LookupProperty(lp => lp.Text(t => t.ID))
                ;
        }

        public static ViewModelConfigBuilder<PropertyComplexIO> DetailView_Default(this ViewModelConfigBuilder<PropertyComplexIO> conf)
        {
            return
                conf.DetailView(dv => dv
                    .Title(conf.Config.Title)
                    .Editors(edt => edt
                        .Clear()
                        .Add(a => a.IsPropertyComplex, h => h.TabName(EstateTabs.GeneralInfo).Visible(true).IsReadOnly())
                        .Add(a => a.PCNumber, h => h.Title("Номер ИК").TabName(EstateTabs.GeneralInfo).Visible(true).IsReadOnly())
                        .Add(a => a.Name, h => h.Title("Наименование").TabName(EstateTabs.GeneralInfo).Visible(true).IsRequired(true))
                        .Add(a => a.Parent, h => h.Title("Принадлежность ИК").Mnemonic("IK").TabName(EstateTabs.GeneralInfo).Visible(true))
                        .AddEmpty(eb => {
                            eb.EditorTemplate("ParentsBranch").Visible(true)
                                                               .TabName(EstateTabs.GeneralInfo)
                                                               .Title("Структура принадлежности ИК");
                        })
                        .Add(a => a.SibUser, h => h.Title("Пользователь").TabName(EstateTabs.GeneralInfo).Visible(true))
                        //.Add(a => a.PropertyComplexIOType, h => h.Title("Тип ИК").TabName(EstateTabs.GeneralInfo).Visible(true))
                        .Add(a => a.InventoryObjectsCount, h => h.Title("Количество объектов, входящих в ИК").TabName(EstateTabs.GeneralInfo).Visible(true))
                        .Add(a => a.InitialCostOBU, h => h.Title("Первоначальная стоимость ИК по данным БУ").TabName(EstateTabs.GeneralInfo).Visible(true))
                        .Add(a => a.ResidualCostOBU, h => h.Title("Остаточная стоимость ИК по данным БУ").TabName(EstateTabs.GeneralInfo).Visible(true))
                        .Add(a => a.InitialCostNU, h => h.Title("Первоначальная стоимость ИК по данным НУ").TabName(EstateTabs.GeneralInfo).Visible(true))
                        .Add(a => a.ResidualCostNU, h => h.Title("Остаточная стоимость ИК по данным НУ").TabName(EstateTabs.GeneralInfo).Visible(true))
                        .Add(a => a.Country, h => h.Title("Страна").TabName(EstateTabs.Address6).Visible(true))
                        .Add(a => a.SibRegion,h => h.Title("Регион").TabName(EstateTabs.Address6).Visible(true))
                        .Add(a => a.Address, h => h.Title("Адрес").TabName(EstateTabs.Address6).Visible(true))
                        .AddOneToManyAssociation<InventoryObject>("PropertyComplexIO_InventoryObject", a => a
                            .TabName("Объекты имущества")
                            .Mnemonic("IK_Estates")
                            .Create((uofw, entity, id) =>
                            {
                                entity.Parent = uofw.GetRepository<InventoryObject>().Find(id);
                                entity.ParentID = id;
                            })
                            .Delete((uofw, entity, id) =>
                            {
                                entity.Parent = null;
                                entity.ParentID = null;
                            })
                            .Filter((uofw, q, id, oid) => q.Where(w => w.ParentID == id && !w.Hidden && !w.IsPropertyComplex))
                            .IsReadOnly(false)
                        )
                        .AddManyToManyLeftAssociation<IKAndLand>("PropertyComplexIO_Land", y => y.TabName("Земельные участки").Mnemonic("IK_Lands"))
                    )
                )
                ;

        }

        /// <summary>
        /// Конфигурация реестра ИК по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<PropertyComplexIO> ListView_Default(this ViewModelConfigBuilder<PropertyComplexIO> conf)
        {
            return
                conf.ListView(x => x
                .Title("Имущественные комплексы")
                .Columns(c => c

                     .Add(col => col.Number, col => col.Visible(true))
                     .Add(col => col.Name, col => col.Visible(true))
                     .Add(col => col.ID, col => col.Visible(false))
                     .Add(col => col.Country, col => col.Visible(true))
                     .Add(col => col.SibRegion, col => col.Visible(true))
                     .Add(col => col.ParentID, col => col.Visible(true))
                     .Add(col => col.Address, col => col.Visible(true))
                ));

        }
    }
}
