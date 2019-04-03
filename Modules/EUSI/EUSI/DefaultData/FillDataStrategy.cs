using Base.DAL;
using CorpProp.DefaultData;
using CorpProp.Entities.Document;
using CorpProp.Entities.Estate;
using CorpProp.Entities.Export;
using CorpProp.Entities.History;
using CorpProp.Entities.NSI;
using EUSI.Entities.ManyToMany;
using EUSI.Entities.NSI;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;

namespace EUSI.DefaultData
{
    public class FillDataStrategy: IFillDataStrategy<DefaultDataHolder>
    {
        /// <summary>
        /// Создает в репозиториях дефолтные знаечния, прочитанные из десериализованного объекта DefaultDataHolder.
        /// </summary>
        /// <param name="uow">Сессия.</param>
        /// <param name="data">Десериализованный объект, содержащий дефолтные данные.</param>
        public void FillData(IDefaultDataHelper _dataHelper, IUnitOfWork uow, DefaultDataHolder data)
        {
            if (data == null) return;
            try
            {
                if (data.EstateDefinitionTypes != null) _dataHelper.CreateDictItem<EstateDefinitionType>(uow, data.EstateDefinitionTypes);
                if (data.EstateRegistrationStateNSI != null) _dataHelper.CreateDictItem<EstateRegistrationStateNSI>(uow, data.EstateRegistrationStateNSI);
                if (data.EstateRegistrationTypeNSI != null) _dataHelper.CreateDictItem<EstateRegistrationTypeNSI>(uow, data.EstateRegistrationTypeNSI);
                
                if (data.ReceiptReasons != null)
                {
                    var res = uow.GetRepository<ReceiptReason>().Filter(f => !f.Hidden).ToList();

                    foreach (var item in res)
                    {
                        item.Hidden = true;
                        uow.GetRepository<ReceiptReason>().Update(item);
                    }
                    _dataHelper.CreateDictItem<ReceiptReason>(uow, data.ReceiptReasons);
                }

                if (data.HistoricalSettingss != null)                
                    _dataHelper.CreateDefaultItem<HistoricalSettings>(uow, data.HistoricalSettingss);

                if (data.Angles != null)
                    _dataHelper.CreateDictItem<Angle>(uow, data.Angles);
                if (data.LoadTypes != null)
                    _dataHelper.CreateDictItem<LoadType>(uow, data.LoadTypes);
                if (data.MovingTypes != null)
                    _dataHelper.CreateDictItem<MovingType>(uow, data.MovingTypes);
                if (data.EstateRegistrationOriginators != null)
                    _dataHelper.CreateDictItem<EstateRegistrationOriginator>(uow, data.EstateRegistrationOriginators);
                if (data.ERReceiptReasons != null)
                    _dataHelper.CreateDictItem<ERReceiptReason>(uow, data.ERReceiptReasons);
                if (data.FileCardTypes != null)
                    _dataHelper.CreateDictItem<FileCardType>(uow, data.FileCardTypes);

                if (data.NSIs != null) _dataHelper.CreateNSI(uow, data.NSIs);

                if (data.ExportTemplates != null) CreateExportTemplate(uow, data.ExportTemplates);


                if (data.TaxRateLowerLands != null)
                    _dataHelper.CreateDictItem<TaxRateLowerLand>(uow, data.TaxRateLowerLands);
                if (data.TaxFreeLands != null)
                    _dataHelper.CreateDictItem<TaxFreeLand>(uow, data.TaxFreeLands);
                if (data.AddonOKOFs != null)
                    _dataHelper.CreateDictItem<AddonOKOF>(uow, data.AddonOKOFs);
                if (data.TaxFreeTSs != null)
                    _dataHelper.CreateDictItem<TaxFreeTS>(uow, data.TaxFreeTSs);
                if (data.TaxRateLowerTSs != null)
                    _dataHelper.CreateDictItem<TaxRateLowerTS>(uow, data.TaxRateLowerTSs);
                if (data.TaxLowerLands != null)
                    _dataHelper.CreateDictItem<TaxLowerLand>(uow, data.TaxLowerLands);
                if (data.VehicleCategorys != null)
                    _dataHelper.CreateDictItem<VehicleCategory>(uow, data.VehicleCategorys);
                if (data.TaxRateLowers != null)
                    _dataHelper.CreateDictItem<TaxRateLower>(uow, data.TaxRateLowers);
                if (data.TaxLowers != null)
                    _dataHelper.CreateDictItem<TaxLower>(uow, data.TaxLowers);
                if (data.TaxLowerTSs != null)
                    _dataHelper.CreateDictItem<TaxLowerTS>(uow, data.TaxLowerTSs);
               
                if (data.EstateTypes != null)
                    _dataHelper.CreateDictItem<EstateType>(uow, data.EstateTypes);
                   
                if (data.TransactionKinds != null)
                    _dataHelper.CreateDictItem<TransactionKind>(uow, data.TransactionKinds);

                if (data.ERTypeERReceiptReasons != null)
                    _dataHelper.CreateERTypeReRiceiptReason(uow, data.ERTypeERReceiptReasons);
                if (data.EstateTypesMappings != null)
                    _dataHelper.CreateEstateTypeMapping(uow, data.EstateTypesMappings);

                if (data.StateObjectRents != null)
                    _dataHelper.CreateDictItem<StateObjectRent>(uow, data.StateObjectRents);

                if (data.EngineTypes != null)
                    _dataHelper.CreateDictItem<EngineType>(uow, data.EngineTypes);

                if (data.ReportMonitoringEventTypes != null)
                    _dataHelper.CreateDictItem<ReportMonitoringEventType>(uow, data.ReportMonitoringEventTypes);
                if (data.ReportMonitoringResults != null)
                    _dataHelper.CreateDictItem<ReportMonitoringResult>(uow, data.ReportMonitoringResults);
                CreateMonitorEventTypeAndResults(uow);
                CreateMonitorEventPrecedings(uow, data.MonitorEventPrecedings);

                uow.SaveChanges();              
            }
            catch (Exception ex) { System.Diagnostics.Trace.TraceError(ex.ToString()); }
           
        }

        /// <summary>
        /// Проходится по данным десериализованного объекта и инициирует добавление их в БД.
        /// </summary>
        /// <param name="context">Контекст.</param>
        /// <param name="data">Десериализованный объект, содержащий дефолтные данные.</param>
        public void FillContext(IDefaultDataHelper _dataHelper, DbContext context, DefaultDataHolder data)
        {
            if (data == null) return;
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex) { System.Diagnostics.Trace.TraceError(ex.ToString()); }

        }


        private void CreateExportTemplate(IUnitOfWork uow, IList<ExportTemplate> list)
        {
            if (list != null)
            {
                var rep = uow.GetRepository<ExportTemplate>();
               
                foreach (var item in list)
                {                    
                    item.File = CreateFileDB(item);
                    rep.Create(item);
                }

                uow.SaveChanges();
            }
        }

        private FileDB CreateFileDB(ExportTemplate tmpl)
        {
            FileDB file = null;
            System.Reflection.Assembly assembly = typeof(EUSI.Entities.Accounting.AccountingMoving).Assembly;
            var resName = assembly.GetManifestResourceNames().Where(f => f.Contains(tmpl.Code)).FirstOrDefault();
            
            using (Stream rstream = assembly.GetManifestResourceStream(resName))
            {
                if (rstream != null)
                {
                    file = new FileDB();
                    file.Content = ReadFully(rstream);
                    file.Name = resName;
                    file.Ext = resName.Contains(".") ? resName.Substring(resName.LastIndexOf(".", StringComparison.Ordinal) + 1).ToUpper() : "";                    
                }
            }
            return file;
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Дефолтные настройки контрольных процедур и результатов.
        /// </summary>
        /// <param name="uow"></param>      
        private void CreateMonitorEventTypeAndResults(IUnitOfWork uow)
        {
            var events = uow.GetRepository<ReportMonitoringEventType>().All();
            var results = uow.GetRepository<ReportMonitoringResult>().All();
            foreach (var eventType in events)
            {
                if (eventType.Code?.StartsWith("IMP_") == true || eventType.Code == "Report_Screen_DraftOS")
                {
                    uow.GetRepository<MonitorEventTypeAndResult>()
                        .Create(new MonitorEventTypeAndResult()
                        {
                            ObjLeft = eventType,
                            ObjRigth = results.FirstOrDefault(f => f.Code == "Loaded"),
                            IsManualPick = false
                        });
                    uow.GetRepository<MonitorEventTypeAndResult>()
                       .Create(new MonitorEventTypeAndResult()
                       {
                           ObjLeft = eventType,
                           ObjRigth = results.FirstOrDefault(f => f.Code == "NotLoaded"),
                           IsManualPick = false
                       });
                }
                else if (eventType.Code?.StartsWith("Report_") == true && eventType.Code != "Report_Screen_DraftOS")
                {
                    uow.GetRepository<MonitorEventTypeAndResult>()
                        .Create(new MonitorEventTypeAndResult()
                        {
                            ObjLeft = eventType,
                            ObjRigth = results.FirstOrDefault(f => f.Code == "Diff"),
                            IsManualPick = false
                        });
                    uow.GetRepository<MonitorEventTypeAndResult>()
                       .Create(new MonitorEventTypeAndResult()
                       {
                           ObjLeft = eventType,
                           ObjRigth = results.FirstOrDefault(f => f.Code == "NoDiff"),
                           IsManualPick = false
                       });
                    uow.GetRepository<MonitorEventTypeAndResult>()
                       .Create(new MonitorEventTypeAndResult()
                       {
                           ObjLeft = eventType,
                           ObjRigth = results.FirstOrDefault(f => f.Code == "Error"),
                           IsManualPick = false
                       });

                    uow.GetRepository<MonitorEventTypeAndResult>()
                       .Create(new MonitorEventTypeAndResult()
                       {
                           ObjLeft = eventType,
                           ObjRigth = results.FirstOrDefault(f => f.Code == "Diff"),
                           IsManualPick = true
                       });

                    uow.GetRepository<MonitorEventTypeAndResult>()
                       .Create(new MonitorEventTypeAndResult()
                       {
                           ObjLeft = eventType,
                           ObjRigth = results.FirstOrDefault(f => f.Code == "AcceptDiff"),
                           IsManualPick = true
                       });
                }

            }
            uow.SaveChanges();
        }

        /// <summary>
        /// Дефолтные настройки предшественников для контрольных процедур.
        /// </summary>
        /// <param name="uow"></param>
        private void CreateMonitorEventPrecedings(
            IUnitOfWork uow
            , List<MonitorEventPreceding> precedings)
        {
            if (precedings == null) return;
            foreach (var preceding in precedings)
            {
                uow.GetRepository<MonitorEventPreceding>()
                    .Create(new MonitorEventPreceding()
                    {
                        ObjLeft = uow.GetRepository<ReportMonitoringEventType>().Filter(f => !f.Hidden && f.Code == preceding.ObjLeft.Code)?.FirstOrDefault(),
                        ObjRigth = uow.GetRepository<ReportMonitoringEventType>().Filter(f => !f.Hidden && f.Code == preceding.ObjRigth.Code)?.FirstOrDefault()
                    });
            }
            uow.SaveChanges();
        }
    }
}
