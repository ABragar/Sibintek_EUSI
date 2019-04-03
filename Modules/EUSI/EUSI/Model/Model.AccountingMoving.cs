using Base;
using Base.UI;
using CorpProp.Entities.Accounting;
using EUSI.Entities.Accounting;
using EUSI.Services.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EUSI.Helpers;
using CorpProp.Extentions;
using CorpProp.Entities.ManyToMany;
using Base.UI.Editors;
using EUSI.Entities.ManyToMany;
using CorpProp.Services.Base;
using Base.UI.ViewModal;


namespace EUSI.Model
{
    public static class AccountingMovingModel
    {
        public static void Init(IInitializerContext context)
        {          

            //движения дефолт
            context.CreateVmConfig<AccountingMoving>()
               .Service<IAccountingMovingService>()
               .Title("Регистр движений")
               .DetailView_Default()
               .ListView_Default()
               .LookupProperty(lp => lp.Text(p => p.ID));

            //движения РСБУ
            context.CreateVmConfig<AccountingMoving>("AccMovingRSBU")
               .Service<IAccountingMovingService>()
               .Title("Регистр движений РСБУ")
               .DetailView_RSBU()
               .ListView_RSBU()
               .LookupProperty(lp => lp.Text(p => p.ID));
                       

            //движения МСФО
            context.CreateVmConfig<AccountingMoving>("AccMovingMSFO")
               .Service<IAccountingMovingService>()
               .Title("Регистр движений МСФО")
               .DetailView_MSFO()
               .ListView_MSFO()
               .LookupProperty(lp => lp.Text(p => p.ID));

            //движения НУ
            context.CreateVmConfig<AccountingMoving>("AccMovingNU")
               .Service<IAccountingMovingService>()
               .Title("Регистр движений НУ")
               .DetailView_NU()
               .ListView_NU()
               .LookupProperty(lp => lp.Text(p => p.ID));


            //движения упрощенное внедрение
            context.CreateVmConfig<AccountingMovingMSFO>()
               .Service<IAccountingMovingMSFOService>()
               .Title("Регистр движений (упрощенное внедрение)")
               .DetailView_Default()
               .ListView_Default()
               .LookupProperty(lp => lp.Text(p => p.ID));

            
            context.CreateVmConfig<AccountingMovingMSFO>("Credit08")
               .Service<IAccountingMovingMSFOService>()
               .Title("Кредитование 08")
               .DetailView_Credit08()
               .ListView_Credit08()
               .LookupProperty(lp => lp.Text(p => p.ID));


            context.CreateVmConfig<AccountingMovingMSFO>("Credit07")
               .Service<IAccountingMovingMSFOService>()
               .Title("Кредитование 07")
               .DetailView_Credit07()
               .ListView_Credit07()
               .LookupProperty(lp => lp.Text(p => p.ID));



            context.CreateVmConfig<AccountingMovingMSFO>("Credit01")
               .Service<IAccountingMovingMSFOService>()
               .Title("Кредитование 01")
               .DetailView_Credit01()
               .ListView_Credit01()
               .LookupProperty(lp => lp.Text(p => p.ID));
            

            context.CreateVmConfig<AccountingMovingMSFO>("Debit08")
               .Service<IAccountingMovingMSFOService>()
               .Title("Дебетование 08")
               .DetailView_Debit08()
               .ListView_Debit08()
               .LookupProperty(lp => lp.Text(p => p.ID));


            context.CreateVmConfig<AccountingMovingMSFO>("Debit07")
               .Service<IAccountingMovingMSFOService>()
               .Title("Дебетование 07")
               .DetailView_Debit07()
               .ListView_Debit07()
               .LookupProperty(lp => lp.Text(p => p.ID));

            context.CreateVmConfig<AccountingMovingMSFO>("Debit01")
             .Service<IAccountingMovingMSFOService>()
             .Title("Дебетование 01")
             .DetailView_Debit01()
             .ListView_Debit01()
             .LookupProperty(lp => lp.Text(p => p.ID));


            context.CreateVmConfig<AccountingMovingMSFO>("Depreciation01")
             .Service<IAccountingMovingMSFOService>()
             .Title("Амортизация 01")
             .DetailView_Depreciation01()
             .ListView_Depreciation01()
             .LookupProperty(lp => lp.Text(p => p.ID));
            
            CreateMovingVersConfig<AccountingMoving>(context);
            CreateMovingVersConfig<AccountingMovingMSFO>(context);

        }

        public static ViewModelConfigBuilder<AccountingMoving> DetailView_Default(this ViewModelConfigBuilder<AccountingMoving> conf)
        {
            return
               conf.DetailView(x => x
               .Title("Регистр движений")
               .Editors(editors => editors.Clear()
                .Add(ed => ed.ID, ac => ac.Visible(true).IsReadOnly())
                .Add(ed => ed.Consolidation, ac => ac.Visible(true).IsReadOnly())
                .Add(ed => ed.EUSINumber, ac => ac.Visible(true).IsReadOnly())
                .Add(ed => ed.InventoryNumber, ac => ac.Visible(true).IsReadOnly())
                .Add(ed => ed.Angle, ac => ac.Visible(true).IsReadOnly())
                .Add(ed => ed.Date, ac => ac.Visible(true).IsReadOnly())
                .Add(ed => ed.MovingType, ac => ac.Visible(true).IsReadOnly())
                .Add(ed => ed.Amount, ac => ac.Visible(true).IsReadOnly())
                .Add(ed => ed.Period, ac => ac.Visible(true).IsReadOnly().AddParam("format", "MMMM yyyy"))
                .Add(ed => ed.CreateDate, ac => ac.Visible(true).IsReadOnly().Title("Дата загрузки"))
                .Add(ed => ed.LoadType, ac => ac.Visible(true).IsReadOnly())
                .Add(ed => ed.ExternalID, ac => ac.Visible(true).IsReadOnly())
                .AddManyToManyRigthAssociation<FileCardAndAccountingMoving>("AccountingMoving_FileCards",
                    y => y.TabName("[2]Перечень первичных документов"))               
              ));
        }

        public static ViewModelConfigBuilder<AccountingMoving> ListView_Default(this ViewModelConfigBuilder<AccountingMoving> conf)
        {
            return
                conf.ListView(lv => lv
                .Title("Регистр движений")
                .HiddenActions(new[] { LvAction.Create, LvAction.Delete })
                .DataSource(ds => ds.Sort(sort => sort.Add(s => s.Date, System.ComponentModel.ListSortDirection.Ascending)))
                .Columns(cols => cols.Clear()
                .Add(ed => ed.ExternalID, ac => ac.Visible(true))
                .Add(ed => ed.Consolidation, ac => ac.Visible(true))
                .Add(ed => ed.EUSINumber, ac => ac.Visible(true))                
                .Add(ed => ed.InventoryNumber, ac => ac.Visible(true))
                .Add(ed => ed.Name, ac => ac.Visible(true))
                .Add(ed => ed.Angle, ac => ac.Visible(true))
                .Add(ed => ed.Date, ac => ac.Visible(true))
                .Add(ed => ed.MovingType, ac => ac.Visible(true))
                .Add(ed => ed.Amount, ac => ac.Visible(true))
                .Add(ed => ed.Period, ac => ac.Visible(true))
                .Add(ed => ed.CreateDate, ac => ac.Visible(true).Title("Дата загрузки"))
                .Add(ed => ed.LoadType, ac => ac.Visible(true))
                .Add(ed => ed.PositionConsolidation, ac => ac.Visible(true))
                ));
        }
        
        public static ViewModelConfigBuilder<AccountingMoving> DetailView_RSBU(this ViewModelConfigBuilder<AccountingMoving> conf)
        {
            return
               conf.DetailView_Default()
               .DetailView(x => x
               .Title("Движение РСБУ")
               .Editors(eds => eds.Add(ed => ed.AccountLedgerLUS, ac => ac.Visible(true)))
               );
        }

        public static ViewModelConfigBuilder<AccountingMoving> ListView_RSBU(this ViewModelConfigBuilder<AccountingMoving> conf)
        {
            return
                conf.ListView_Default()
                .ListView(lv => lv
                .Title("Регистр движений РСБУ") 
                .Columns(cols => cols.Add(col => col.AccountLedgerLUS, ac => ac.Visible(true)))
                .DataSource(ds => ds.Filter(f => f.Angle != null && f.Angle.Code == "RSBU"))
                );
        }
        
        public static ViewModelConfigBuilder<AccountingMoving> DetailView_MSFO(this ViewModelConfigBuilder<AccountingMoving> conf)
        {
            return
               conf.DetailView_Default()
               .DetailView(x => x
               .Title("Движение МСФО")
               .Editors(eds=> eds.Add(e => e.InRSBU , ac=>ac.Visible(true)))
               );
        }

        public static ViewModelConfigBuilder<AccountingMoving> ListView_MSFO(this ViewModelConfigBuilder<AccountingMoving> conf)
        {
            return
                conf.ListView_Default()
                .ListView(lv => lv
                .Title("Регистр движений МСФО") 
                .Columns(cols => cols.Add(ed => ed.InRSBU, ac => ac.Visible(true)))
                .DataSource(ds => ds.Filter(f => f.Angle != null && f.Angle.Code == "MSFO"))
                );
        }

        public static ViewModelConfigBuilder<AccountingMoving> DetailView_NU(this ViewModelConfigBuilder<AccountingMoving> conf)
        {
            return
               conf.DetailView_Default()
               .DetailView(x => x
               .Title("Движение НУ"));
        }

        public static ViewModelConfigBuilder<AccountingMoving> ListView_NU(this ViewModelConfigBuilder<AccountingMoving> conf)
        {
            return
                conf.ListView_Default()
                .ListView(lv => lv
                .Title("Регистр движений НУ")
                .DataSource(ds => ds.Filter(f => f.Angle != null && f.Angle.Code == "NU"))
                );
        }


        public static ViewModelConfigBuilder<AccountingMovingMSFO> DetailView_Default(this ViewModelConfigBuilder<AccountingMovingMSFO> conf)
        {
            return
               conf.DetailView(x => x
               .Title("Регистр движений (упрощенное внедрение)")
               .Editors(editors => editors.Clear()
                .Add(ed => ed.TypeMovingMSFO, ac => ac.Visible(true).IsReadOnly(true))
                .Add(ed => ed.ExternalID, ac => ac.Visible(true).Title("ID записи шаблона"))
                .Add(ed => ed.Date, ac => ac.Visible(true))
                .Add(ed => ed.EUSINumber, ac => ac.Visible(true))
                .Add(ed => ed.Consolidation, ac => ac.Visible(true))
                .Add(ed => ed.SubPosition, ac => ac.Visible(true))
                .Add(ed => ed.InventoryNumber, ac => ac.Visible(true))
                .Add(ed => ed.Cost, ac => ac.Visible(true))
                .Add(ed => ed.MovingType, ac => ac.Visible(true))
                .Add(ed => ed.Operation, ac => ac.Visible(true))


                .Add(ed => ed.InventoryDebit, ac => ac.Visible(true))
                .Add(ed => ed.InventoryCredit, ac => ac.Visible(true))
                .Add(ed => ed.DepGroupDebit, ac => ac.Visible(true))
                .Add(ed => ed.DepGroupCredit, ac => ac.Visible(true))
                .Add(ed => ed.BusinessUnitDebit, ac => ac.Visible(true))
                .Add(ed => ed.BusinessUnitCredit, ac => ac.Visible(true))
                .Add(ed => ed.BusinessAreaDebit, ac => ac.Visible(true))
                .Add(ed => ed.BusinessAreaCredit, ac => ac.Visible(true))

                .Add(ed => ed.GroupObjDebit, ac => ac.Visible(true))
                .Add(ed => ed.GroupObjCredit, ac => ac.Visible(true))
                .Add(ed => ed.GroupObjTypeDebit, ac => ac.Visible(true))
                .Add(ed => ed.GroupObjTypeCredit, ac => ac.Visible(true))
                .Add(ed => ed.DocDate, ac => ac.Visible(true))
                .Add(ed => ed.DateOfReceipt, ac => ac.Visible(true))
                .Add(ed => ed.AnalyticOneDebit, ac => ac.Visible(true))
                .Add(ed => ed.AnalyticOneCredit, ac => ac.Visible(true))


                .Add(ed => ed.AnalyticTwoDebit, ac => ac.Visible(true))
                .Add(ed => ed.AnalyticTwoCredit, ac => ac.Visible(true))

                .Add(ed => ed.OKOFDebit, ac => ac.Visible(true))
                .Add(ed => ed.OKOFCredit, ac => ac.Visible(true))
                .Add(ed => ed.IXODepreciationDebit, ac => ac.Visible(true))
                .Add(ed => ed.IXODepreciationCredit, ac => ac.Visible(true))
                .Add(ed => ed.IXOInitialDebit, ac => ac.Visible(true))
                .Add(ed => ed.IXOInitialCredit, ac => ac.Visible(true))
                .Add(ed => ed.Contragent, ac => ac.Visible(true))
                .Add(ed => ed.NameDebit, ac => ac.Visible(true))
                .Add(ed => ed.DepositDebit, ac => ac.Visible(true))
                .Add(ed => ed.DepositCredit, ac => ac.Visible(true))
                .Add(ed => ed.BatchNumberDebit, ac => ac.Visible(true))
                .Add(ed => ed.BatchNumberCredit, ac => ac.Visible(true))
                .Add(ed => ed.SPPDebit, ac => ac.Visible(true))
                .Add(ed => ed.UsefulEndDebit, ac => ac.Visible(true))
                .Add(ed => ed.PartnerWhoCredit, ac => ac.Visible(true))
                .Add(ed => ed.PartnerCredit, ac => ac.Visible(true))

                .Add(ed => ed.PositionCredit, ac => ac.Visible(true))
                .Add(ed => ed.PositionStorno, ac => ac.Visible(true))
                .Add(ed => ed.PositionStornoSign, ac => ac.Visible(true))
                .Add(ed => ed.Explanation, ac => ac.Visible(true))
                .Add(ed => ed.IsLeavingDebit, ac => ac.Visible(true))
                .Add(ed => ed.SPPItemDebit, ac => ac.Visible(true))
                .Add(ed => ed.SPPItemCredit, ac => ac.Visible(true))
                .Add(ed => ed.UsefulDebit, ac => ac.Visible(true))
                .Add(ed => ed.CostDepreciation, ac => ac.Visible(true))
                .Add(ed => ed.AccountGKDebit, ac => ac.Visible(true))
                .Add(ed => ed.AccountGKCredit, ac => ac.Visible(true))
                .Add(ed => ed.ExternalIDDebit, ac => ac.Visible(true))
                .Add(ed => ed.ExternalIDCredit, ac => ac.Visible(true))
              )
              );
        }

        public static ViewModelConfigBuilder<AccountingMovingMSFO> ListView_Default(this ViewModelConfigBuilder<AccountingMovingMSFO> conf)
        {
            return
                conf.ListView(lv => lv
                .Title("Регистр движений (упрощенное внедрение)")                
                .HiddenActions(new[] { LvAction.Create, LvAction.Delete })                
                .DataSource(ds => 
                    ds.Groups(gr => gr.Groupable(true).Add(g => g.TypeMovingMSFO))
                    .Sort(sort => sort.Add(s => s.Date, System.ComponentModel.ListSortDirection.Ascending)))
                .Columns(cols => cols.Clear()
                .Add(ed => ed.ExternalID, ac => ac.Visible(true).Title("ID записи шаблона"))
                .Add(ed => ed.TypeMovingMSFO, ac => ac.Visible(true))                
                .Add(ed => ed.Date, ac => ac.Visible(true))
                .Add(ed => ed.EUSINumber, ac => ac.Visible(true))
                .Add(ed => ed.Name, ac => ac.Visible(true))
                .Add(ed => ed.Consolidation, ac => ac.Visible(true))
                .Add(ed => ed.SubPosition, ac => ac.Visible(true))              
                .Add(ed => ed.InventoryNumber, ac => ac.Visible(true))               
                .Add(ed => ed.Cost, ac => ac.Visible(true))
                .Add(ed => ed.MovingType, ac => ac.Visible(true))
                .Add(ed => ed.Operation, ac => ac.Visible(true))


                .Add(ed => ed.InventoryDebit, ac => ac.Visible(true))
                .Add(ed => ed.InventoryCredit, ac => ac.Visible(true))
                .Add(ed => ed.DepGroupDebit, ac => ac.Visible(true))
                .Add(ed => ed.DepGroupCredit, ac => ac.Visible(true))
                .Add(ed => ed.BusinessUnitDebit, ac => ac.Visible(true))
                .Add(ed => ed.BusinessUnitCredit, ac => ac.Visible(true))
                .Add(ed => ed.BusinessAreaDebit, ac => ac.Visible(true))
                .Add(ed => ed.BusinessAreaCredit, ac => ac.Visible(true))
               
                .Add(ed => ed.GroupObjDebit, ac => ac.Visible(true))
                .Add(ed => ed.GroupObjCredit, ac => ac.Visible(true))
                .Add(ed => ed.GroupObjTypeDebit, ac => ac.Visible(true))
                .Add(ed => ed.GroupObjTypeCredit, ac => ac.Visible(true))
                .Add(ed => ed.DocDate, ac => ac.Visible(true))
                .Add(ed => ed.DateOfReceipt, ac => ac.Visible(true))
                .Add(ed => ed.AnalyticOneDebit, ac => ac.Visible(true))
                .Add(ed => ed.AnalyticOneCredit, ac => ac.Visible(true))


                .Add(ed => ed.AnalyticTwoDebit, ac => ac.Visible(true))
                .Add(ed => ed.AnalyticTwoCredit, ac => ac.Visible(true))
               
                .Add(ed => ed.OKOFDebit, ac => ac.Visible(true))
                .Add(ed => ed.OKOFCredit, ac => ac.Visible(true))
                .Add(ed => ed.IXODepreciationDebit, ac => ac.Visible(true))
                .Add(ed => ed.IXODepreciationCredit, ac => ac.Visible(true))
                .Add(ed => ed.IXOInitialDebit, ac => ac.Visible(true))
                .Add(ed => ed.IXOInitialCredit, ac => ac.Visible(true))
                .Add(ed => ed.Contragent, ac => ac.Visible(true))
                .Add(ed => ed.NameDebit, ac => ac.Visible(true))
                .Add(ed => ed.DepositDebit, ac => ac.Visible(true))
                .Add(ed => ed.DepositCredit, ac => ac.Visible(true))
                .Add(ed => ed.BatchNumberDebit, ac => ac.Visible(true))
                .Add(ed => ed.BatchNumberCredit, ac => ac.Visible(true))
                .Add(ed => ed.SPPDebit, ac => ac.Visible(true))
                .Add(ed => ed.UsefulEndDebit, ac => ac.Visible(true))
                .Add(ed => ed.PartnerWhoCredit, ac => ac.Visible(true))
                .Add(ed => ed.PartnerCredit, ac => ac.Visible(true))
                
                .Add(ed => ed.PositionCredit, ac => ac.Visible(true))
                .Add(ed => ed.PositionStorno, ac => ac.Visible(true))
                .Add(ed => ed.PositionStornoSign, ac => ac.Visible(true))
                .Add(ed => ed.Explanation, ac => ac.Visible(true))
                .Add(ed => ed.IsLeavingDebit, ac => ac.Visible(true))
                .Add(ed => ed.SPPItemDebit, ac => ac.Visible(true))
                .Add(ed => ed.SPPItemCredit, ac => ac.Visible(true))
                .Add(ed => ed.UsefulDebit, ac => ac.Visible(true))
                .Add(ed => ed.CostDepreciation, ac => ac.Visible(true))
                .Add(ed => ed.AccountGKDebit, ac => ac.Visible(true))
                .Add(ed => ed.AccountGKCredit, ac => ac.Visible(true))
                .Add(ed => ed.ExternalIDDebit, ac => ac.Visible(true))
                .Add(ed => ed.ExternalIDCredit, ac => ac.Visible(true))
                .Add(ed => ed.LoadType, ac => ac.Visible(false))

                )
                .OnClientEditRow(
                @"var dataItem = grid.dataItem(grid.select());
                 if (dataItem) {
                    if (dataItem.TypeMovingMSFO) {  
                        switch (dataItem.TypeMovingMSFO) {
                          case 1:
                            mnemonic = 'Credit01';
                            break;
                          case 2:
                            mnemonic = 'Credit07';
                            break;
                          case 3:
                            mnemonic = 'Credit08';
                            break;
                            case 4:
                            mnemonic = 'Debit01';
                            break;
                            case 5:
                            mnemonic = 'Debit07';
                            break;
                            case 6:
                            mnemonic = 'Debit08';
                            break;
                            case 7:
                            mnemonic = 'Depreciation01';
                            break;
                           default:
                           break;
                        }                       
                    }
                }")
                );
        }
        
        public static ViewModelConfigBuilder<AccountingMovingMSFO> DetailView_Credit08(this ViewModelConfigBuilder<AccountingMovingMSFO> conf)
        {            
               conf.DetailView(x => x
               .Title("Кредитование 08")
               .Editors(editors => editors.Clear()
                .Add(ed => ed.ExternalID, ac => ac.Visible(true).IsReadOnly().Title("ID записи шаблона"))
                .Add(ed => ed.EUSINumber, ac => ac.Visible(true).IsReadOnly())               
                .Add(ed => ed.Consolidation, ac => ac.Visible(true).IsReadOnly())                
                .Add(ed => ed.SubPosition, ac => ac.Visible(true).IsReadOnly())
                .Add(ed => ed.Operation, ac => ac.Visible(true).IsReadOnly())
                .Add(ed => ed.DocDate, ac => ac.Visible(true).IsReadOnly())
                .Add(ed => ed.Date, ac => ac.Visible(true).IsReadOnly())
                .Add(ed => ed.Cost, ac => ac.Visible(true).IsReadOnly())
                .Add(ed => ed.PositionStorno, ac => ac.Visible(true).IsReadOnly())
                .Add(ed => ed.Explanation, ac => ac.Visible(true).IsReadOnly())
                .Add(ed => ed.MovingType, ac => ac.Visible(true).IsReadOnly())
                .Add(ed => ed.AccountGKCredit, ac => ac.Visible(true).IsReadOnly())
                .Add(ed => ed.Contragent, ac => ac.Visible(true).IsReadOnly())
                .Add(ed => ed.PartnerWhoCredit, ac => ac.Visible(true).IsReadOnly())
                .Add(ed => ed.ExternalIDCredit, ac => ac.Visible(true).IsReadOnly())
                .Add(ed => ed.BatchNumberCredit, ac => ac.Visible(true).IsReadOnly())
                .Add(ed => ed.SPPItemCredit, ac => ac.Visible(true).IsReadOnly())
                .Add(ed => ed.BusinessAreaCredit, ac => ac.Visible(true).IsReadOnly())
                .Add(ed => ed.ExternalIDDebit, ac => ac.Visible(true).IsReadOnly())
                .Add(ed => ed.BatchNumberDebit, ac => ac.Visible(true).IsReadOnly())
                .Add(ed => ed.AccountGKDebit, ac => ac.Visible(true).IsReadOnly())
                .Add(ed => ed.IXOInitialDebit, ac => ac.Visible(true).IsReadOnly())
                .Add(ed => ed.GroupObjTypeDebit, ac => ac.Visible(true).IsReadOnly())
                .Add(ed => ed.BusinessAreaDebit, ac => ac.Visible(true).IsReadOnly())
                .Add(ed => ed.DepositDebit, ac => ac.Visible(true).IsReadOnly())
                .Add(ed => ed.PositionDebit, ac => ac.Visible(true).IsReadOnly())
                .Add(ed => ed.AnalyticOneDebit, ac => ac.Visible(true).IsReadOnly())
                .Add(ed => ed.AnalyticTwoDebit, ac => ac.Visible(true).IsReadOnly())
                .Add(ed => ed.SPPItemDebit, ac => ac.Visible(true).IsReadOnly())  
              ))
              .Config.DetailView.Editors
              .AddManyToMany("AcccMoving_FileCards"
                  , typeof(FileCardAndAccountingMoving)
                  , typeof(IManyToManyLeftAssociation<>)
                  , ManyToManyAssociationType.Rigth
                  , y => y.TabName("[2]Перечень первичных документов").Visible(true).Order(1))
               ;
            return conf;
        }

        public static ViewModelConfigBuilder<AccountingMovingMSFO> ListView_Credit08(this ViewModelConfigBuilder<AccountingMovingMSFO> conf)
        {
            return
                conf.ListView(lv => lv
                .Title("Кредитование 08")
                .HiddenActions(new[] { LvAction.Create, LvAction.Delete })
                .DataSource(ds => ds.Filter(f => f.TypeMovingMSFO == TypeMovingMSFO.Credit08)
                .Sort(sort => sort.Add(s => s.Date, System.ComponentModel.ListSortDirection.Ascending)))
                .Columns(cols => cols.Clear()
                .Add(ed => ed.ExternalID, ac => ac.Visible(true).Title("ID записи шаблона"))
                .Add(ed => ed.EUSINumber, ac => ac.Visible(true))
                .Add(ed => ed.Consolidation, ac => ac.Visible(true))
                .Add(ed => ed.SubPosition, ac => ac.Visible(true))
                .Add(ed => ed.Operation, ac => ac.Visible(true))
                .Add(ed => ed.DocDate, ac => ac.Visible(true))
                .Add(ed => ed.Date, ac => ac.Visible(true))
                .Add(ed => ed.Cost, ac => ac.Visible(true))
                .Add(ed => ed.PositionStorno, ac => ac.Visible(true))
                .Add(ed => ed.Explanation, ac => ac.Visible(true))
                .Add(ed => ed.MovingType, ac => ac.Visible(true))
                .Add(ed => ed.AccountGKCredit, ac => ac.Visible(true))
                .Add(ed => ed.Contragent, ac => ac.Visible(true))
                .Add(ed => ed.PartnerWhoCredit, ac => ac.Visible(true))
                .Add(ed => ed.ExternalIDCredit, ac => ac.Visible(true))
                .Add(ed => ed.BatchNumberCredit, ac => ac.Visible(true))
                .Add(ed => ed.SPPItemCredit, ac => ac.Visible(true))
                .Add(ed => ed.BusinessAreaCredit, ac => ac.Visible(true))
                .Add(ed => ed.ExternalIDDebit, ac => ac.Visible(true))
                .Add(ed => ed.BatchNumberDebit, ac => ac.Visible(true))
                .Add(ed => ed.AccountGKDebit, ac => ac.Visible(true))
                .Add(ed => ed.IXOInitialDebit, ac => ac.Visible(true))
                .Add(ed => ed.GroupObjTypeDebit, ac => ac.Visible(true))
                .Add(ed => ed.BusinessAreaDebit, ac => ac.Visible(true))
                .Add(ed => ed.DepositDebit, ac => ac.Visible(true))
                .Add(ed => ed.PositionDebit, ac => ac.Visible(true))
                .Add(ed => ed.AnalyticOneDebit, ac => ac.Visible(true))
                .Add(ed => ed.AnalyticTwoDebit, ac => ac.Visible(true))
                .Add(ed => ed.SPPItemDebit, ac => ac.Visible(true))
                ));
        }
        
        public static ViewModelConfigBuilder<AccountingMovingMSFO> DetailView_Credit07(this ViewModelConfigBuilder<AccountingMovingMSFO> conf)
        {
            conf.DetailView(x => x
            .Title("Кредитование 07")
            .Editors(editors => editors.Clear()
             .Add(ed => ed.ExternalID, ac => ac.Visible(true).IsReadOnly().Title("ID записи шаблона"))
             .Add(ed => ed.EUSINumber, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.Consolidation, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.InventoryNumber, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.SubPosition, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.Operation, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.DocDate, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.Cost, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.PositionStorno, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.Explanation, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.MovingType, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.AccountGKCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.Contragent, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.PartnerWhoCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.ExternalIDCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.BatchNumberCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.SPPItemCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.BusinessAreaCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.ExternalIDDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.BatchNumberDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.AccountGKDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.IXOInitialDebit, ac => ac.Visible(true).IsReadOnly())            
             .Add(ed => ed.SPPItemDebit, ac => ac.Visible(true).IsReadOnly())
           ))
           .Config.DetailView.Editors
           .AddManyToMany("AcccMoving_FileCards"
               , typeof(FileCardAndAccountingMoving)
               , typeof(IManyToManyLeftAssociation<>)
               , ManyToManyAssociationType.Rigth
               , y => y.TabName("[2]Перечень первичных документов").Visible(true).Order(1))
            ;
            return conf;
        }

        public static ViewModelConfigBuilder<AccountingMovingMSFO> ListView_Credit07(this ViewModelConfigBuilder<AccountingMovingMSFO> conf)
        {
            return
                conf.ListView(lv => lv
                .Title("Кредитование 07")
                .HiddenActions(new[] { LvAction.Create, LvAction.Delete })
                 .DataSource(ds => ds.Filter(f => f.TypeMovingMSFO == TypeMovingMSFO.Credit07)
                 .Sort(sort => sort.Add(s => s.Date, System.ComponentModel.ListSortDirection.Ascending)))
                .Columns(cols => cols.Clear()
                 .Add(ed => ed.ExternalID, ac => ac.Visible(true).Title("ID записи шаблона"))
                 .Add(ed => ed.EUSINumber, ac => ac.Visible(true))
                 .Add(ed => ed.Consolidation, ac => ac.Visible(true))
                 .Add(ed => ed.InventoryNumber, ac => ac.Visible(true))
                 .Add(ed => ed.SubPosition, ac => ac.Visible(true))
                 .Add(ed => ed.Operation, ac => ac.Visible(true))
                 .Add(ed => ed.DocDate, ac => ac.Visible(true))
                 .Add(ed => ed.Date, ac => ac.Visible(true))
                 .Add(ed => ed.Cost, ac => ac.Visible(true))
                 .Add(ed => ed.PositionStorno, ac => ac.Visible(true))
                 .Add(ed => ed.Explanation, ac => ac.Visible(true))
                 .Add(ed => ed.MovingType, ac => ac.Visible(true))
                 .Add(ed => ed.AccountGKCredit, ac => ac.Visible(true))
                 .Add(ed => ed.Contragent, ac => ac.Visible(true))
                 .Add(ed => ed.PartnerWhoCredit, ac => ac.Visible(true))
                 .Add(ed => ed.ExternalIDCredit, ac => ac.Visible(true))
                 .Add(ed => ed.BatchNumberCredit, ac => ac.Visible(true))
                 .Add(ed => ed.SPPItemCredit, ac => ac.Visible(true))
                 .Add(ed => ed.BusinessAreaCredit, ac => ac.Visible(true))
                 .Add(ed => ed.ExternalIDDebit, ac => ac.Visible(true))
                 .Add(ed => ed.BatchNumberDebit, ac => ac.Visible(true))
                 .Add(ed => ed.AccountGKDebit, ac => ac.Visible(true))
                 .Add(ed => ed.IXOInitialDebit, ac => ac.Visible(true))
                 .Add(ed => ed.SPPItemDebit, ac => ac.Visible(true))
             ));
        }
        
        public static ViewModelConfigBuilder<AccountingMovingMSFO> DetailView_Credit01(this ViewModelConfigBuilder<AccountingMovingMSFO> conf)
        {
            conf.DetailView(x => x
            .Title("Кредитование 01")
            .Editors(editors => editors.Clear()
             .Add(ed => ed.ExternalID, ac => ac.Visible(true).IsReadOnly().Title("ID записи шаблона"))
             .Add(ed => ed.EUSINumber, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.Consolidation, ac => ac.Visible(true).IsReadOnly())          
             .Add(ed => ed.SubPosition, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.Operation, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.DocDate, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.Date, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.Cost, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.CostDepreciation, ac => ac.Visible(true).IsReadOnly())          
             .Add(ed => ed.PositionStorno, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.Explanation, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.MovingType, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.AccountGKCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.Contragent, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.PartnerWhoCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.ExternalIDCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.InventoryCredit, ac => ac.Visible(true).IsReadOnly())            
             .Add(ed => ed.BusinessAreaCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.ExternalIDDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.InventoryDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.IsLeavingDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.AccountGKDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.IXOInitialDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.IXODepreciationDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.GroupObjDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.BusinessAreaDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.OKOFDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.DepGroupDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.DepositDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.PositionDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.AnalyticOneDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.AnalyticTwoDebit, ac => ac.Visible(true).IsReadOnly())             
           ))
           .Config.DetailView.Editors
           .AddManyToMany("AcccMoving_FileCards"
               , typeof(FileCardAndAccountingMoving)
               , typeof(IManyToManyLeftAssociation<>)
               , ManyToManyAssociationType.Rigth
               , y => y.TabName("[2]Перечень первичных документов").Visible(true).Order(1))
            ;
            return conf;
        }

        public static ViewModelConfigBuilder<AccountingMovingMSFO> ListView_Credit01(this ViewModelConfigBuilder<AccountingMovingMSFO> conf)
        {
            return
                conf.ListView(lv => lv
                .Title("Кредитование 01")
                .HiddenActions(new[] { LvAction.Create, LvAction.Delete })
                 .DataSource(ds => ds.Filter(f => f.TypeMovingMSFO == TypeMovingMSFO.Credit01)
                .Sort(sort => sort.Add(s => s.Date, System.ComponentModel.ListSortDirection.Ascending)))
                .Columns(cols => cols.Clear()
                 .Add(ed => ed.ExternalID, ac => ac.Visible(true).Title("ID записи шаблона"))
                 .Add(ed => ed.EUSINumber, ac => ac.Visible(true))
                 .Add(ed => ed.Consolidation, ac => ac.Visible(true))
                 .Add(ed => ed.SubPosition, ac => ac.Visible(true))
                 .Add(ed => ed.Operation, ac => ac.Visible(true))
                 .Add(ed => ed.DocDate, ac => ac.Visible(true))
                 .Add(ed => ed.Date, ac => ac.Visible(true))
                 .Add(ed => ed.Cost, ac => ac.Visible(true))
                 .Add(ed => ed.CostDepreciation, ac => ac.Visible(true))
                 .Add(ed => ed.PositionStorno, ac => ac.Visible(true))
                 .Add(ed => ed.Explanation, ac => ac.Visible(true))
                 .Add(ed => ed.MovingType, ac => ac.Visible(true))
                 .Add(ed => ed.AccountGKCredit, ac => ac.Visible(true))
                 .Add(ed => ed.Contragent, ac => ac.Visible(true))
                 .Add(ed => ed.PartnerWhoCredit, ac => ac.Visible(true))
                 .Add(ed => ed.ExternalIDCredit, ac => ac.Visible(true))
                 .Add(ed => ed.InventoryCredit, ac => ac.Visible(true))
                 .Add(ed => ed.BusinessAreaCredit, ac => ac.Visible(true))
                 .Add(ed => ed.ExternalIDDebit, ac => ac.Visible(true))
                 .Add(ed => ed.InventoryDebit, ac => ac.Visible(true))
                 .Add(ed => ed.IsLeavingDebit, ac => ac.Visible(true))
                 .Add(ed => ed.AccountGKDebit, ac => ac.Visible(true))
                 .Add(ed => ed.IXOInitialDebit, ac => ac.Visible(true))
                 .Add(ed => ed.IXODepreciationDebit, ac => ac.Visible(true))
                 .Add(ed => ed.GroupObjDebit, ac => ac.Visible(true))
                 .Add(ed => ed.BusinessAreaDebit, ac => ac.Visible(true))
                 .Add(ed => ed.OKOFDebit, ac => ac.Visible(true))
                 .Add(ed => ed.DepGroupDebit, ac => ac.Visible(true))
                 .Add(ed => ed.DepositDebit, ac => ac.Visible(true))
                 .Add(ed => ed.PositionDebit, ac => ac.Visible(true))
                 .Add(ed => ed.AnalyticOneDebit, ac => ac.Visible(true))
                 .Add(ed => ed.AnalyticTwoDebit, ac => ac.Visible(true))
                ));
        }
        
        public static ViewModelConfigBuilder<AccountingMovingMSFO> DetailView_Debit08(this ViewModelConfigBuilder<AccountingMovingMSFO> conf)
        {
            conf.DetailView(x => x
            .Title("Дебетование 08")
            .Editors(editors => editors.Clear()
             .Add(ed => ed.ExternalID, ac => ac.Visible(true).IsReadOnly().Title("ID записи шаблона"))
             .Add(ed => ed.EUSINumber, ac => ac.Visible(true).IsReadOnly())            
             .Add(ed => ed.Consolidation, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.InventoryNumber, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.SubPosition, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.Operation, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.DocDate, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.Date, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.DateOfReceipt, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.Cost, ac => ac.Visible(true).IsReadOnly())            
             .Add(ed => ed.PositionStorno, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.Explanation, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.MovingType, ac => ac.Visible(true).IsReadOnly())

             .Add(ed => ed.NameDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.ExternalIDDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.BatchNumberDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.SPPItemDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.BusinessAreaDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.DepositDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.PositionDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.AnalyticOneDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.AnalyticTwoDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.AccountGKDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.IXOInitialDebit, ac => ac.Visible(true).IsReadOnly())            
             .Add(ed => ed.GroupObjTypeDebit, ac => ac.Visible(true).IsReadOnly())

             .Add(ed => ed.BusinessUnitDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.AccountGKCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.Contragent, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.PartnerCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.ExternalIDCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.BatchNumberCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.BusinessAreaCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.DepositCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.PositionCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.AnalyticOneCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.AnalyticTwoCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.SPPItemCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.IXOInitialCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.GroupObjTypeCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.BusinessUnitCredit, ac => ac.Visible(true).IsReadOnly())
           ))
           .Config.DetailView.Editors
           .AddManyToMany("AcccMoving_FileCards"
               , typeof(FileCardAndAccountingMoving)
               , typeof(IManyToManyLeftAssociation<>)
               , ManyToManyAssociationType.Rigth
               , y => y.TabName("[2]Перечень первичных документов").Visible(true).Order(1))
            ;
            return conf;
        }

        public static ViewModelConfigBuilder<AccountingMovingMSFO> ListView_Debit08(this ViewModelConfigBuilder<AccountingMovingMSFO> conf)
        {
            return
                conf.ListView(lv => lv
                .Title("Дебетование 08")
                .HiddenActions(new[] { LvAction.Create, LvAction.Delete })
                .DataSource(ds => ds.Filter(f => f.TypeMovingMSFO == TypeMovingMSFO.Debit08)
                .Sort(sort => sort.Add(s => s.Date, System.ComponentModel.ListSortDirection.Ascending)))
                .Columns(cols => cols.Clear()
                 .Add(ed => ed.ExternalID, ac => ac.Visible(true).Title("ID записи шаблона"))
                 .Add(ed => ed.EUSINumber, ac => ac.Visible(true))
                 .Add(ed => ed.Consolidation, ac => ac.Visible(true))
                 .Add(ed => ed.InventoryNumber, ac => ac.Visible(true))
                 .Add(ed => ed.SubPosition, ac => ac.Visible(true))
                 .Add(ed => ed.Operation, ac => ac.Visible(true))
                 .Add(ed => ed.DocDate, ac => ac.Visible(true))
                 .Add(ed => ed.Date, ac => ac.Visible(true))
                 .Add(ed => ed.DateOfReceipt, ac => ac.Visible(true))
                 .Add(ed => ed.Cost, ac => ac.Visible(true))
                 .Add(ed => ed.PositionStorno, ac => ac.Visible(true))
                 .Add(ed => ed.Explanation, ac => ac.Visible(true))
                 .Add(ed => ed.MovingType, ac => ac.Visible(true))

                 .Add(ed => ed.NameDebit, ac => ac.Visible(true))
                 .Add(ed => ed.ExternalIDDebit, ac => ac.Visible(true))
                 .Add(ed => ed.BatchNumberDebit, ac => ac.Visible(true))
                 .Add(ed => ed.SPPItemDebit, ac => ac.Visible(true))
                 .Add(ed => ed.BusinessAreaDebit, ac => ac.Visible(true))
                 .Add(ed => ed.DepositDebit, ac => ac.Visible(true))
                 .Add(ed => ed.PositionDebit, ac => ac.Visible(true))
                 .Add(ed => ed.AnalyticOneDebit, ac => ac.Visible(true))
                 .Add(ed => ed.AnalyticTwoDebit, ac => ac.Visible(true))
                 .Add(ed => ed.AccountGKDebit, ac => ac.Visible(true))
                 .Add(ed => ed.IXOInitialDebit, ac => ac.Visible(true))
                 .Add(ed => ed.GroupObjTypeDebit, ac => ac.Visible(true))

                 .Add(ed => ed.BusinessUnitDebit, ac => ac.Visible(true))
                 .Add(ed => ed.AccountGKCredit, ac => ac.Visible(true))
                 .Add(ed => ed.Contragent, ac => ac.Visible(true))
                 .Add(ed => ed.PartnerCredit, ac => ac.Visible(true))
                 .Add(ed => ed.ExternalIDCredit, ac => ac.Visible(true))
                 .Add(ed => ed.BatchNumberCredit, ac => ac.Visible(true))
                 .Add(ed => ed.BusinessAreaCredit, ac => ac.Visible(true))
                 .Add(ed => ed.DepositCredit, ac => ac.Visible(true))
                 .Add(ed => ed.PositionCredit, ac => ac.Visible(true))
                 .Add(ed => ed.AnalyticOneCredit, ac => ac.Visible(true))
                 .Add(ed => ed.AnalyticTwoCredit, ac => ac.Visible(true))
                 .Add(ed => ed.SPPItemCredit, ac => ac.Visible(true))
                 .Add(ed => ed.IXOInitialCredit, ac => ac.Visible(true))
                 .Add(ed => ed.GroupObjTypeCredit, ac => ac.Visible(true))
                 .Add(ed => ed.BusinessUnitCredit, ac => ac.Visible(true))
                ));
        }
        
        public static ViewModelConfigBuilder<AccountingMovingMSFO> DetailView_Debit07(this ViewModelConfigBuilder<AccountingMovingMSFO> conf)
        {
            conf.DetailView(x => x
            .Title("Дебетование 07")
            .Editors(editors => editors.Clear()
             .Add(ed => ed.ExternalID, ac => ac.Visible(true).IsReadOnly().Title("ID записи шаблона"))
             .Add(ed => ed.EUSINumber, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.Consolidation, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.InventoryNumber, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.SubPosition, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.Operation, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.DocDate, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.Date, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.DateOfReceipt, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.Cost, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.PositionStorno, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.Explanation, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.MovingType, ac => ac.Visible(true).IsReadOnly())

             .Add(ed => ed.NameDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.ExternalIDDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.BatchNumberDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.SPPItemDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.BusinessAreaDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.AccountGKDebit, ac => ac.Visible(true).IsReadOnly())             
             .Add(ed => ed.IXOInitialDebit, ac => ac.Visible(true).IsReadOnly())
            
             .Add(ed => ed.AccountGKCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.Contragent, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.PartnerCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.ExternalIDCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.BatchNumberCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.SPPItemCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.BusinessAreaCredit, ac => ac.Visible(true).IsReadOnly())            
             .Add(ed => ed.IXOInitialCredit, ac => ac.Visible(true).IsReadOnly())
             
           ))
           .Config.DetailView.Editors
           .AddManyToMany("AcccMoving_FileCards"
               , typeof(FileCardAndAccountingMoving)
               , typeof(IManyToManyLeftAssociation<>)
               , ManyToManyAssociationType.Rigth
               , y => y.TabName("[2]Перечень первичных документов").Visible(true).Order(1))
            ;
            return conf;
        }

        public static ViewModelConfigBuilder<AccountingMovingMSFO> ListView_Debit07(this ViewModelConfigBuilder<AccountingMovingMSFO> conf)
        {
            return
                conf.ListView(lv => lv
                .Title("Дебетование 07")
                .HiddenActions(new[] { LvAction.Create, LvAction.Delete })
                .DataSource(ds => ds.Filter(f => f.TypeMovingMSFO == TypeMovingMSFO.Debit07)
                .Sort(sort => sort.Add(s => s.Date, System.ComponentModel.ListSortDirection.Ascending)))
                .Columns(cols => cols.Clear()
                .Add(ed => ed.ExternalID, ac => ac.Visible(true).Title("ID записи шаблона"))
                 .Add(ed => ed.EUSINumber, ac => ac.Visible(true))
                 .Add(ed => ed.Consolidation, ac => ac.Visible(true))
                 .Add(ed => ed.InventoryNumber, ac => ac.Visible(true))
                 .Add(ed => ed.SubPosition, ac => ac.Visible(true))
                 .Add(ed => ed.Operation, ac => ac.Visible(true))
                 .Add(ed => ed.DocDate, ac => ac.Visible(true))
                 .Add(ed => ed.Date, ac => ac.Visible(true))
                 .Add(ed => ed.DateOfReceipt, ac => ac.Visible(true))
                 .Add(ed => ed.Cost, ac => ac.Visible(true))
                 .Add(ed => ed.PositionStorno, ac => ac.Visible(true))
                 .Add(ed => ed.Explanation, ac => ac.Visible(true))
                 .Add(ed => ed.MovingType, ac => ac.Visible(true))

                 .Add(ed => ed.NameDebit, ac => ac.Visible(true))
                 .Add(ed => ed.ExternalIDDebit, ac => ac.Visible(true))
                 .Add(ed => ed.BatchNumberDebit, ac => ac.Visible(true))
                 .Add(ed => ed.SPPItemDebit, ac => ac.Visible(true))
                 .Add(ed => ed.BusinessAreaDebit, ac => ac.Visible(true))
                 .Add(ed => ed.AccountGKDebit, ac => ac.Visible(true))
                 .Add(ed => ed.IXOInitialDebit, ac => ac.Visible(true))

                 .Add(ed => ed.AccountGKCredit, ac => ac.Visible(true))
                 .Add(ed => ed.Contragent, ac => ac.Visible(true))
                 .Add(ed => ed.PartnerCredit, ac => ac.Visible(true))
                 .Add(ed => ed.ExternalIDCredit, ac => ac.Visible(true))
                 .Add(ed => ed.BatchNumberCredit, ac => ac.Visible(true))
                 .Add(ed => ed.SPPItemCredit, ac => ac.Visible(true))
                 .Add(ed => ed.BusinessAreaCredit, ac => ac.Visible(true))
                 .Add(ed => ed.IXOInitialCredit, ac => ac.Visible(true))
                ));
        }

        public static ViewModelConfigBuilder<AccountingMovingMSFO> DetailView_Debit01(this ViewModelConfigBuilder<AccountingMovingMSFO> conf)
        {
            conf.DetailView(x => x
            .Title("Дебетование 01")
            .Editors(editors => editors.Clear()
             .Add(ed => ed.ExternalID, ac => ac.Visible(true).IsReadOnly().Title("ID записи шаблона"))
             .Add(ed => ed.EUSINumber, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.Consolidation, ac => ac.Visible(true).IsReadOnly())           
             .Add(ed => ed.SubPosition, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.Operation, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.DocDate, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.Date, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.DateOfReceipt, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.Cost, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.CostDepreciation, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.PositionStorno, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.Explanation, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.MovingType, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.NameDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.ExternalIDDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.InventoryDebit, ac => ac.Visible(true).IsReadOnly())            
             .Add(ed => ed.BusinessAreaDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.OKOFDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.DepGroupDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.DepositDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.PositionDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.AnalyticOneDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.AnalyticTwoDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.AccountGKDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.UsefulDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.UsefulEndDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.IXOInitialDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.IXODepreciationDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.GroupObjDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.BusinessAreaDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.AccountGKCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.Contragent, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.PartnerCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.ExternalIDCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.BatchNumberCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.InventoryCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.BusinessAreaCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.OKOFCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.DepGroupCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.DepositCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.PositionCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.AnalyticOneCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.AnalyticTwoCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.SPPItemCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.IXOInitialCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.GroupObjCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.BusinessUnitCredit, ac => ac.Visible(true).IsReadOnly())
           ))
           .Config.DetailView.Editors
           .AddManyToMany("AcccMoving_FileCards"
               , typeof(FileCardAndAccountingMoving)
               , typeof(IManyToManyLeftAssociation<>)
               , ManyToManyAssociationType.Rigth
               , y => y.TabName("[2]Перечень первичных документов").Visible(true).Order(1))
            ;
            return conf;
        }

        public static ViewModelConfigBuilder<AccountingMovingMSFO> ListView_Debit01(this ViewModelConfigBuilder<AccountingMovingMSFO> conf)
        {
            return
                conf.ListView(lv => lv
                .Title("Дебетование 01")
                .HiddenActions(new[] { LvAction.Create, LvAction.Delete })
                .DataSource(ds => ds.Filter(f => f.TypeMovingMSFO == TypeMovingMSFO.Debit01)
                .Sort(sort => sort.Add(s => s.Date, System.ComponentModel.ListSortDirection.Ascending)))
                .Columns(cols => cols.Clear()
                 .Add(ed => ed.ExternalID, ac => ac.Visible(true).Title("ID записи шаблона"))
                 .Add(ed => ed.EUSINumber, ac => ac.Visible(true))
                 .Add(ed => ed.Consolidation, ac => ac.Visible(true))
                 .Add(ed => ed.SubPosition, ac => ac.Visible(true))
                 .Add(ed => ed.Operation, ac => ac.Visible(true))
                 .Add(ed => ed.DocDate, ac => ac.Visible(true))
                 .Add(ed => ed.Date, ac => ac.Visible(true))
                 .Add(ed => ed.DateOfReceipt, ac => ac.Visible(true))
                 .Add(ed => ed.Cost, ac => ac.Visible(true))
                 .Add(ed => ed.CostDepreciation, ac => ac.Visible(true))
                 .Add(ed => ed.PositionStorno, ac => ac.Visible(true))
                 .Add(ed => ed.Explanation, ac => ac.Visible(true))
                 .Add(ed => ed.MovingType, ac => ac.Visible(true))
                 .Add(ed => ed.NameDebit, ac => ac.Visible(true))
                 .Add(ed => ed.ExternalIDDebit, ac => ac.Visible(true))
                 .Add(ed => ed.InventoryDebit, ac => ac.Visible(true))
                 .Add(ed => ed.BusinessAreaDebit, ac => ac.Visible(true))
                 .Add(ed => ed.OKOFDebit, ac => ac.Visible(true))
                 .Add(ed => ed.DepGroupDebit, ac => ac.Visible(true))
                 .Add(ed => ed.DepositDebit, ac => ac.Visible(true))
                 .Add(ed => ed.PositionDebit, ac => ac.Visible(true))
                 .Add(ed => ed.AnalyticOneDebit, ac => ac.Visible(true))
                 .Add(ed => ed.AnalyticTwoDebit, ac => ac.Visible(true))
                 .Add(ed => ed.AccountGKDebit, ac => ac.Visible(true))
                 .Add(ed => ed.UsefulDebit, ac => ac.Visible(true))
                 .Add(ed => ed.UsefulEndDebit, ac => ac.Visible(true))
                 .Add(ed => ed.IXOInitialDebit, ac => ac.Visible(true))
                 .Add(ed => ed.IXODepreciationDebit, ac => ac.Visible(true))
                 .Add(ed => ed.GroupObjDebit, ac => ac.Visible(true))
                 .Add(ed => ed.BusinessAreaDebit, ac => ac.Visible(true))
                 .Add(ed => ed.AccountGKCredit, ac => ac.Visible(true))
                 .Add(ed => ed.Contragent, ac => ac.Visible(true))
                 .Add(ed => ed.PartnerCredit, ac => ac.Visible(true))
                 .Add(ed => ed.ExternalIDCredit, ac => ac.Visible(true))
                 .Add(ed => ed.BatchNumberCredit, ac => ac.Visible(true))
                 .Add(ed => ed.InventoryCredit, ac => ac.Visible(true))
                 .Add(ed => ed.BusinessAreaCredit, ac => ac.Visible(true))
                 .Add(ed => ed.OKOFCredit, ac => ac.Visible(true))
                 .Add(ed => ed.DepGroupCredit, ac => ac.Visible(true))
                 .Add(ed => ed.DepositCredit, ac => ac.Visible(true))
                 .Add(ed => ed.PositionCredit, ac => ac.Visible(true))
                 .Add(ed => ed.AnalyticOneCredit, ac => ac.Visible(true))
                 .Add(ed => ed.AnalyticTwoCredit, ac => ac.Visible(true))
                 .Add(ed => ed.SPPItemCredit, ac => ac.Visible(true))
                 .Add(ed => ed.IXOInitialCredit, ac => ac.Visible(true))
                 .Add(ed => ed.GroupObjCredit, ac => ac.Visible(true))
                 .Add(ed => ed.BusinessUnitCredit, ac => ac.Visible(true))
                ));
        }
        
        public static ViewModelConfigBuilder<AccountingMovingMSFO> DetailView_Depreciation01(this ViewModelConfigBuilder<AccountingMovingMSFO> conf)
        {
            conf.DetailView(x => x
            .Title("Амортизация 01")
            .Editors(editors => editors.Clear()
             .Add(ed => ed.ExternalID, ac => ac.Visible(true).IsReadOnly().Title("ID записи шаблона"))
             .Add(ed => ed.EUSINumber, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.Consolidation, ac => ac.Visible(true).IsReadOnly())           
             .Add(ed => ed.SubPosition, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.Operation, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.DocDate, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.Date, ac => ac.Visible(true).IsReadOnly())            
             .Add(ed => ed.CostDepreciation, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.PositionStorno, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.Explanation, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.MovingType, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.AccountGKDebit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.IXODepreciationCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.PositionCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.ExternalIDCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.InventoryCredit, ac => ac.Visible(true).IsReadOnly())
             .Add(ed => ed.AccountGKCredit, ac => ac.Visible(true).IsReadOnly())            
           ))
           .Config.DetailView.Editors
           .AddManyToMany("AcccMoving_FileCards"
               , typeof(FileCardAndAccountingMoving)
               , typeof(IManyToManyLeftAssociation<>)
               , ManyToManyAssociationType.Rigth
               , y => y.TabName("[2]Перечень первичных документов").Visible(true).Order(1))
            ;
            return conf;
        }

        public static ViewModelConfigBuilder<AccountingMovingMSFO> ListView_Depreciation01(this ViewModelConfigBuilder<AccountingMovingMSFO> conf)
        {
            return
                conf.ListView(lv => lv
                .Title("Амортизация 01")
                .HiddenActions(new[] { LvAction.Create, LvAction.Delete })
                .DataSource(ds => ds.Filter(f => f.TypeMovingMSFO == TypeMovingMSFO.Depreciation01)
                .Sort(sort => sort.Add(s => s.Date, System.ComponentModel.ListSortDirection.Ascending)))
                .Columns(cols => cols.Clear()
                .Add(ed => ed.ExternalID, ac => ac.Visible(true).Title("ID записи шаблона"))
                 .Add(ed => ed.EUSINumber, ac => ac.Visible(true))
                 .Add(ed => ed.Consolidation, ac => ac.Visible(true))
                 .Add(ed => ed.SubPosition, ac => ac.Visible(true))
                 .Add(ed => ed.Operation, ac => ac.Visible(true))
                 .Add(ed => ed.DocDate, ac => ac.Visible(true))
                 .Add(ed => ed.Date, ac => ac.Visible(true))
                 .Add(ed => ed.CostDepreciation, ac => ac.Visible(true))
                 .Add(ed => ed.PositionStorno, ac => ac.Visible(true))
                 .Add(ed => ed.Explanation, ac => ac.Visible(true))
                 .Add(ed => ed.MovingType, ac => ac.Visible(true))
                 .Add(ed => ed.AccountGKDebit, ac => ac.Visible(true))
                 .Add(ed => ed.IXODepreciationCredit, ac => ac.Visible(true))
                 .Add(ed => ed.PositionCredit, ac => ac.Visible(true))
                 .Add(ed => ed.ExternalIDCredit, ac => ac.Visible(true))
                 .Add(ed => ed.InventoryCredit, ac => ac.Visible(true))
                 .Add(ed => ed.AccountGKCredit, ac => ac.Visible(true))
                ));
        }

      
        /// <summary>
        /// Для каждого существующего конфига движения создает конфиг для просмотра его историчных версий.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        public static void CreateMovingVersConfig<T>(this IInitializerContext context) where T : AccountingMoving
        {
            var configs = context.GetVmConfigs()
              .Where(w => w.Value != null 
                && Type.Equals(w.Value.TypeEntity, typeof(T)))
              .DefaultIfEmpty()
              .Select(s => s.Value)
              .ToList();

            foreach (var conf in configs)
            {
                context.CreateVmConfigOnBase<T>(conf.Mnemonic, conf.Mnemonic + "Vers")
                 .Service<IAccountingMovingHistoryService<T>>()
                 .ListView(lv => lv
                    .Columns(col => col.Add(c => c.ImportDate, ac => ac.Visible(true).Order(-100).Title("Дата импорта")))
                    .DataSource(ds => ds.Groups(gr => gr.Groupable(false)))
                 )
                 .IsReadOnly(true)
                ;

                context.ModifyVmConfig<T>(conf.Mnemonic)
                 .DetailView(dv => dv.Editors(eds => eds
                 .AddOneToManyAssociation<T>(Guid.NewGuid().ToString(),
                        editor => editor
                        .TabName("[3]Предыдущие версии")
                        .Mnemonic(conf.Mnemonic + "Vers")
                        .IsReadOnly(true)
                        .IsLabelVisible(false)
                  .Filter((uofw, q, id, oid) =>
                      q.Where(w => w.Oid == oid && w.IsHistory)
                  )))                 
                  );
            }
        }

    }
}
