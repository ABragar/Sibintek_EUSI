using Base.DAL;
using Base.Extensions;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Import;
using CorpProp.Entities.ManyToMany;
using CorpProp.Entities.NSI;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.Services.Settings;
using EUSI.Entities.Estate;
using EUSI.Entities.ManyToMany;
using EUSI.Import;
using EUSI.Model;
using EUSI.Services.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CorpProp.Entities.Estate;
using EUSI.Entities.NSI;
using EUSI.Helpers;

namespace EUSI.Services.Estate
{
    public class EstateRegistrationImport
    {
        private readonly IAccountingObjectExtService _accountingObjectService;
        private ISibEmailService _emailService;
        private System.Collections.Concurrent.ConcurrentBag<CorpProp.Entities.Estate.Estate> estates;
        private System.Collections.Concurrent.ConcurrentBag<AccountingObject> oss;
        private System.Collections.Concurrent.ConcurrentBag<FileCardAndAccountingObject> fileOS;
        private System.Collections.Concurrent.ConcurrentBag<FileCardAndEstate> fileOI;
        private EstateRegistration _er;
        private IUnitOfWork _uow;
        
        public EstateRegistrationImport(IAccountingObjectExtService accountingObjectService
            , ISibEmailService emailService)
        {
            _accountingObjectService = accountingObjectService;
            _emailService = emailService;
            estates = new System.Collections.Concurrent.ConcurrentBag<CorpProp.Entities.Estate.Estate>();
            oss = new System.Collections.Concurrent.ConcurrentBag<AccountingObject>();
            fileOI = new System.Collections.Concurrent.ConcurrentBag<FileCardAndEstate>();
            fileOS = new System.Collections.Concurrent.ConcurrentBag<FileCardAndAccountingObject>();
        }

        public void CreateObjectsFromEstateRegistration(IUnitOfWork uow, EstateRegistration er)
        {
            if (er == null) return;
            _er = er;
            _uow = uow;
            var rows = uow.GetRepository<EstateRegistrationRow>()
                .FilterAsNoTracking(f => !f.Hidden && !f.IsHistory && f.EstateRegistrationID == er.ID)
                .Include(pr => pr.EstateDefinitionType)
                .Include(pr => pr.EstateType)
                .Include(pr => pr.IntangibleAssetType)
                .ToList();

            if (er.ERType == null)
                throw new Exception("Не указан объект заявки.");

            if (er.ERType.Code == "OSVGP")
            {
                var isArenda = (er.ERReceiptReason != null && er.ERReceiptReason.Code.Contains("Rent"));
                ERImportHolder holder = new ERImportHolder(null, uow, null, null, null, null, null, null, null, null);
                holder.EstateRegistration = er;
                holder.Rows = rows;
                holder.Docs = uow.GetRepository<FileCardAndEstateRegistrationObject>()
                .Filter(f => !f.Hidden && f.ObjRigthId == er.ID)
                .Include(pr => pr.ObjLeft)
                .Select(s => s.ObjLeft)
                .ToList();
                holder.SetVals();
                ///TODO: описать обработку в "CloneOS"  атрибутов "ContragentID" или "LessorSubjectID" опираясь на признак "isArenda"
                holder.ExecuteVGP(isArenda);
                return;
            }

            if (er.ERType.Code == "Union")
            {
                if (er.ERReceiptReason.Code == "Union")
                    ExecuteUnion();
                if (er.ERReceiptReason.Code == "Division")
                    ExecuteDivision();
                return;
            }

            foreach (var row in rows)
            {
                try
                {
                    var est = CreateEstate(row);
                    var os = CreateOS(row);
                    os.Estate = est;
                    estates.Add(est);
                    oss.Add(os);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            if (er.ERType.Code != "Docs")
                CreateFiles();
            else
                CreateDocs();

            if (estates.Count > 0)
                _uow.GetRepository<CorpProp.Entities.Estate.Estate>().CreateCollection(estates);
            if (oss.Count > 0)
                _uow.GetRepository<CorpProp.Entities.Accounting.AccountingObject>().CreateCollection(oss);

            if (fileOI.Count > 0)
                _uow.GetRepository<FileCardAndEstate>().CreateCollection(fileOI);
            if (fileOS.Count > 0)
                _uow.GetRepository<FileCardAndAccountingObject>().CreateCollection(fileOS);
            CreateLinks();
        }

        /// <summary>
        /// 15762:Реализация проверки кадастрового объекта на предмет наличия в справочнике "Объект_облаг_кадастр_стоимости" и автоматического заполнения атрибутов в момент создания ОИ
        /// </summary>
        /// <param name="est"></param>
        private void CheckEstate_PropertyListTaxBaseCadastral(CorpProp.Entities.Estate.Estate est)
        {
            var obj = est as Cadastral;
            if (!string.IsNullOrEmpty(obj?.CadastralNumber))
            {
                var currentYear = DateTime.Now.Year;
                var plc = _uow.GetRepository<PropertyListTaxBaseCadastral>()
                    .FilterAsNoTracking(x => !x.Hidden
                                             && !x.IsHistory
                                             && ((x.CadastralNumber == obj.CadastralNumber)|| (x.RoomCadastralNumber == obj.CadastralNumber))
                                             && x.DateFrom.HasValue && x.DateFrom.Value.Year == currentYear
                                             && x.DateTo.HasValue && x.DateTo.Value.Year == currentYear)
                    .FirstOrDefault();

                if (plc != null)
                {
                    var taxBaseCad = _uow.GetRepository<TaxBase>().All().SingleOrDefault(x => x.Code == "102");
                    var estateTaxesCad = new EstateTaxes()
                    {
                        TaxesOf = obj,
                        TaxCadastralIncludeDate = plc.ApprovingDocDate,
                        TaxCadastralIncludeDoc = plc.ApprovingDocNumber,
                        TaxBase = taxBaseCad
                    };
                    _uow.GetRepository<EstateTaxes>().Create(estateTaxesCad);
                }
                else
                {
                    var taxBaseNoCad = _uow.GetRepository<TaxBase>().All().SingleOrDefault(x => x.Code == "101");
                    var estateTaxesNoCad = new EstateTaxes()
                    {
                        TaxesOf = obj,
                        TaxCadastralIncludeDate = null,
                        TaxCadastralIncludeDoc = "",
                        TaxBase = taxBaseNoCad
                    };
                    _uow.GetRepository<EstateTaxes>().Create(estateTaxesNoCad);
                }
            }
        }

        private void CreateLinks()
        {
            var osLink = new List<AccountingObjectAndEstateRegistrationObject>();
            var estLink = new List<EstateAndEstateRegistrationObject>();
            foreach (var est in estates)
            {
                estLink.Add(new EstateAndEstateRegistrationObject()
                {
                    ObjLeft = est,
                    ObjRigth = _er,
                    IsPrototype = true
                });
            }
            foreach (var os in oss)
            {
                osLink.Add(new AccountingObjectAndEstateRegistrationObject()
                {
                    ObjLeft = os,
                    ObjRigth = _er,
                    IsPrototype = true
                });
            }

            _uow.GetRepository<EstateAndEstateRegistrationObject>()
                .CreateCollection(estLink);
            _uow.GetRepository<AccountingObjectAndEstateRegistrationObject>()
                .CreateCollection(osLink);
        }

        public void CreateDocs()
        {
            var osLink = new List<AccountingObjectAndEstateRegistrationObject>();
            var estLink = new List<EstateAndEstateRegistrationObject>();
            var docs = _uow.GetRepository<FileCardAndEstateRegistrationObject>()
                .Filter(f => !f.Hidden && f.ObjRigthId == _er.ID && f.ObjLeft != null && !f.ObjLeft.Hidden)
                .Select(s => s.ObjLeft)
                .ToList();
            ;
            foreach (var doc in docs)
            {
                var est = _uow.GetRepository<CorpProp.Entities.Estate.Estate>()
                    .Filter(f =>
                    !f.Hidden
                    && !f.IsHistory
                    && f.Number != null
                    && f.Number == doc.EUSINumber)
                    .FirstOrDefault();
                if (est != null)
                {
                    fileOI.Add(new FileCardAndEstate()
                    {
                        ObjLeft = doc,
                        ObjRigth = est
                    });

                    if (!estLink.Any(f => f.ObjLeft == est))
                        estLink.Add(new EstateAndEstateRegistrationObject()
                        {
                            ObjLeft = est,
                            ObjRigth = _er,
                        });

                    var os = _uow.GetRepository<CorpProp.Entities.Accounting.AccountingObject>()
                      .Filter(f =>
                      !f.Hidden
                      && !f.IsHistory
                      && f.Estate != null
                      && f.Estate.Number == doc.EUSINumber
                      && f.Consolidation != null
                      && f.Consolidation.Code == _er.Consolidation.Code)
                      ;

                    if (os != null)
                    {
                        foreach (var item in os)
                        {
                            fileOS.Add(new FileCardAndAccountingObject()
                            {
                                ObjLeft = doc,
                                ObjRigth = item
                            });

                            if (!osLink.Any(f => f.ObjLeft == item))
                                osLink.Add(new AccountingObjectAndEstateRegistrationObject()
                                {
                                    ObjLeft = item,
                                    ObjRigth = _er,
                                });
                        }
                    }
                }
            }
            if (estLink.Any())
                _uow.GetRepository<EstateAndEstateRegistrationObject>()
                    .CreateCollection(estLink);
            if (osLink.Any())
                _uow.GetRepository<AccountingObjectAndEstateRegistrationObject>()
                    .CreateCollection(osLink);
        }

        public void CreateFiles()
        {
            var docs = _uow.GetRepository<FileCardAndEstateRegistrationObject>()
                .FilterAsNoTracking(f => !f.Hidden && f.ObjRigthId == _er.ID && f.ObjLeft != null)
                .Select(s => s.ObjLeft)?
                .ToList()
                ;
            foreach (var doc in docs)
            {
                try
                {
                    foreach (var est in estates)
                    {
                        fileOI.Add(new FileCardAndEstate()
                        {
                            ObjLeft = doc,
                            ObjRigth = est
                        });
                    }

                    foreach (var os in oss)
                    {
                        fileOS.Add(new FileCardAndAccountingObject()
                        {
                            ObjLeft = doc,
                            ObjRigth = os
                        });
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        /// <summary>
        /// Создание нового ОИ из строки объекта заявки.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public CorpProp.Entities.Estate.Estate CreateEstate(EstateRegistrationRow row)
        {
            var type = TypesHelper.GetTypeByName(row.EstateDefinitionType.Code);
            CorpProp.Entities.Estate.Estate est = Activator.CreateInstance(type) as CorpProp.Entities.Estate.Estate;
            est.EstateDefinitionTypeID = row.EstateDefinitionTypeID;
            UpdateEstate(row, ref est);
            est.EstateStatus = EstateStatus.Create;
            // Задача 10185: Реализовать автоматическое установление значания признака "За балансом" (реестр замечаний п.п. 39)
            est.OutOfBalance = EUSIImportHelper.IsOutOfBalance(_er.ERReceiptReason?.Code);
            est.ERContractNumber = _er.ERContractNumber;
            est.ERContractDate = _er.ERContractDate;
            est.PrimaryDocNumber = _er.PrimaryDocNumber;
            est.PrimaryDocDate = _er.PrimaryDocDate;

            //15762
            CheckEstate_PropertyListTaxBaseCadastral(est);

            return est;
        }

        /// <summary>
        /// Обновляет ОИ значениями из строки объекта заявки.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="est"></param>
        private void UpdateEstate(EstateRegistrationRow row, ref CorpProp.Entities.Estate.Estate est)
        {
            est.FillFromER("IntangibleAssetTypeID", row.IntangibleAssetTypeID);
            est.FillFromER("Name", row.NameEstateByDoc);
            est.FillFromER("NameByDoc", row.NameEstateByDoc);
            est.FillFromER("NameEUSI", row.NameEUSI);
            est.FillFromER("StartDateUse", row.StartDateUse);
            est.FillFromER("CadastralNumber", row.CadastralNumber);
            est.FillFromER("EstateTypeID", row.EstateTypeID);
            est.FillFromER("SibCountryID", row.SibCountryID);
            est.FillFromER("SibFederalDistrictID", row.SibFederalDistrictID);
            est.FillFromER("SibRegionID", row.SibRegionID);
            est.FillFromER("SibCityNSIID", row.SibCityNSIID);
            est.FillFromER("Address", row.Address);
            est.FillFromER("YearOfIssue", row.VehicleYearOfIssue);
            est.FillFromER("VehicleCategoryID", row.VehicleCategoryID);
            if (est.GetType().IsTS())
                est.FillFromER("RegDate", row.VehicleRegDate);
            est.FillFromER("DieselEngine", row.VehicleDieselEngine);
            est.FillFromER("SibMeasureID", row.VehiclePowerMeasureID);
            est.FillFromER("Power", row.VehiclePower);
            est.FillFromER("SerialNumber", row.VehicleSerialNumber);
            est.FillFromER("EngineSize", row.VehicleEngineSize);
            est.FillFromER("VehicleModelID", row.VehicleModelID);
            est.FillFromER("RegNumber", row.VehicleRegNumber);
            est.FillFromER("SibRegionID", row.SibRegionID);
            if (String.IsNullOrEmpty(est.NameEUSI))
                est.NameEUSI = est.NameByDoc;
        }

        /// <summary>
        /// Создание нового ОС/НМА из строки объекта заявки.
        /// </summary>
        /// <param name="row">Объект заявки.</param>
        /// <returns></returns>
        private AccountingObject CreateOS(EstateRegistrationRow row)
        {
            AccountingObject os = new AccountingObject();
            os.ActualDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            os.ConsolidationID = _er.ConsolidationID;
            os.ReceiptReasonID = _uow.GetRepository<ReceiptReason>()
               .FilterAsNoTracking(f => !f.Hidden && !f.IsHistory && f.Code == _er.ERReceiptReason.Code)
               .FirstOrDefault()?.ID;
            os.ContragentID = _er.ContragentID;
            os.OwnerID = _er.SocietyID;
            os.EstateDefinitionTypeID = row.EstateDefinitionTypeID;
            UpdateOS(row, ref os);
            os.StateObjectRSBU = _uow.GetRepository<StateObjectRSBU>()
                .Filter(f =>
                !f.Hidden
                && !f.IsHistory
                && f.Code != null
                && f.Code.ToLower() == "draft")
                .FirstOrDefault();

            os.PrimaryDocNumber = _er.PrimaryDocNumber;
            os.PrimaryDocDate = _er.PrimaryDocDate;
            os.ContractNumber = _er.ERContractNumber;
            os.ContractDate = _er.ERContractDate;
            os.CreatingFromER = _er?.Number;
            os.CreatingFromERPosition = row.Position;
            return os;
        }

        /// <summary>
        /// Обновляет ОС значениями из объекта заявки.
        /// </summary>
        /// <param name="row">Объект заявки.</param>
        /// <param name="os">ОС/НМА.</param>
        /// <returns></returns>
        private void UpdateOS(EstateRegistrationRow row, ref AccountingObject os)
        {
            os.StartDateUse = row.StartDateUse ?? os.StartDateUse;
            os.CadastralNumber = row.CadastralNumber ?? os.CadastralNumber;
            os.SibCountryID = row.SibCountryID ?? os.SibCountryID;
            os.SibFederalDistrictID = row.SibFederalDistrictID ?? os.SibFederalDistrictID;
            os.RegionID = row.SibRegionID ?? os.RegionID;
            os.SibCityNSIID = row.SibCityNSIID ?? os.SibCityNSIID;
            os.Address = row.Address ?? os.Address;
            os.YearOfIssue = row.VehicleYearOfIssue ?? os.YearOfIssue;
            os.VehicleCategoryID = row.VehicleCategoryID ?? os.VehicleCategoryID;
            os.DieselEngine = (row.VehicleDieselEngine || os.DieselEngine);
            os.SibMeasureID = row.VehiclePowerMeasureID ?? os.SibMeasureID;
            os.Power = row.VehiclePower ?? os.Power;
            os.SerialNumber = row.VehicleSerialNumber ?? os.SerialNumber;
            os.EngineSize = row.VehicleEngineSize ?? os.EngineSize;
            os.VehicleModelID = row.VehicleModelID ?? os.VehicleModelID;
            os.VehicleRegNumber = row.VehicleRegNumber ?? os.VehicleRegNumber;
        }
        
        /// <summary>
        /// Выполняет катомный набор проверок для заявки типа "Объединение".
        /// </summary>
        /// <param name="UnitOfWork">Сессия.</param>
        /// <param name="Rows">Объекты заявки.</param>
        /// <param name="EstateRegistration">Заявка.</param>
        /// <returns>Коллекция ошибок в результате проверки.</returns>
        public static IList<ImportErrorLog> CheckUnion
        (
            IUnitOfWork UnitOfWork
            , IList<EstateRegistrationRow> Rows
            , EstateRegistration EstateRegistration
        )
        {
            var listErrors = new List<ImportErrorLog>();

            //ОИ, указанные на листе заявки ЕУСИ "Объединение", должны присутствовать в системе ЕУСИ.
            var estates = UnitOfWork.GetRepository<CorpProp.Entities.Estate.Estate>()
                .Filter(f => !f.Hidden && !f.IsHistory)
                .Include(inc => inc.EstateDefinitionType);

            var q =
                from oi in Rows
                join gl in estates on oi.EUSINumber equals gl.EUSINumber?.ToString() into outer
                from gr in outer.DefaultIfEmpty()
                select new
                {
                    EUSINumber = oi.EUSINumber,
                    Estate = gr,
                    EstateID = gr?.ID,
                    Number = gr?.Number,
                    EstateDefinitionType = gr?.EstateDefinitionType,
                    Oid = gr?.Oid,
                    InventoryNumber = gr?.InventoryNumber
                };
            if (q.Where(w => w.Estate == null).Any())
                foreach (var item in q.Where(w => w.Estate == null))
                {
                    listErrors
                        .AddError($"В Системе не найден объект имущества с номером ЕУСИ <{item.EUSINumber}>");

                }

            //В заявке "Объединение" для всех номеров ЕУСИ д.б. указаны одинаковые
            // -Инвентарный номер;
            // -Тип Объекта имущества
            if (Rows.GroupBy(g => g.EstateDefinitionType?.Code).Count() > 1 || Rows.GroupBy(g => g.InventoryNumber).Count() > 1)
                listErrors.AddError("В заявке для всех номеров ЕУСИ должны быть указаны одинаковые: Инвентарный номер, Тип Объекта имущества.");

            //Тип объекта имущества в ОЗ не соответствует типу ОИ в ОИ
            if (q.Join(q, l => l.Oid, r => r.Oid, (l, r) => new { l, r })
                .Where(f => (f.l.EstateDefinitionType != null
                && f.r.EstateDefinitionType != null
                && f.l.EstateDefinitionType.Code != f.r.EstateDefinitionType.Code)
                || f.l.InventoryNumber != f.r.InventoryNumber
                )
                .Any()
                )
                listErrors.AddError($"Тип Объекта имущества, указанный в заявке не соответствует типам объектов имущества в Системе.");
            
            var eusis = Rows.Select(s => s.EUSINumber).Distinct();

            var ois = UnitOfWork.GetRepository<CorpProp.Entities.Estate.Estate>()
                .FilterAsNoTracking(f => !f.Hidden && !f.IsHistory && f.Number != null && eusis.Contains(f.Number.ToString()))
                .ToList();

            foreach (var oi in ois.Where(f => f.EstateStatus == EstateStatus.Archive))
            {
                listErrors.AddError($"Объект имущества с номером ЕУСИ {oi.EUSINumber} находится в статусе \"Архив\"");
            }

            if (listErrors.Any())
                return listErrors;

            var be = EstateRegistration.Consolidation?.Code;
            foreach (var oi in ois)
            {
                var oss = UnitOfWork.GetRepository<CorpProp.Entities.Accounting.AccountingObject>()
                   .FilterAsNoTracking(f => !f.Hidden && !f.IsHistory && f.EstateID == oi.ID)
                   .Include(inc => inc.Consolidation)
                   .Include(inc => inc.StateObjectRSBU)
                   .ToList();
                // у ОИ нет ОСов с БЕ и в статусе не архив
                if (!oss.Any(f => f.Consolidation != null && f.Consolidation.Code == be))
                {
                    listErrors.AddError($"По объекту имущества с номером ЕУСИ {oi.EUSINumber} не найдено ОС/НМА, удовлетворяющих условию: БЕ в карточке ОС/НМА = БЕ в заявке И Состояние ОС/НМА ≠ В архиве.");
                }
                //у ОИ есть ОСы с другим БЕ и не в статусе Архив
                if (oss.Any(f =>
                (f.Consolidation == null || (f.Consolidation != null && f.Consolidation.Code != be))
                && (f.StateObjectRSBU == null || (f.StateObjectRSBU != null && f.StateObjectRSBU.Code != "Archive"))))
                {
                    listErrors.AddError($"По объекту имущества с номером ЕУСИ {oi.EUSINumber} найдены ОС/НМА, удовлетворяющие условию: БЕ в карточке ОС/НМА ≠ БЕ в заявке И Состояние ОС/НМА ≠ В архиве.");
                }
            }
            return listErrors;
        }

        public static IList<ImportErrorLog> CheckDivision
       (
           IUnitOfWork UnitOfWork
           , IList<EstateRegistrationRow> Rows
           , EstateRegistration EstateRegistration
       )
        {
            var listErrors = new List<ImportErrorLog>();

            //ОИ, указанные на листе заявки ЕУСИ "Объединение", должны присутствовать в системе ЕУСИ.
            var estates = UnitOfWork.GetRepository<CorpProp.Entities.Estate.Estate>()
                .Filter(f => !f.Hidden && !f.IsHistory)
                .Include(inc => inc.EstateDefinitionType);

            var q =
                from oi in Rows
                join gl in estates on oi.EUSINumber equals gl.EUSINumber?.ToString() into outer
                from gr in outer.DefaultIfEmpty()
                select new
                {
                    EUSINumber = oi.EUSINumber,
                    Estate = gr,
                    EstateID = gr?.ID,
                    Number = gr?.Number,
                    EstateDefinitionType = gr?.EstateDefinitionType,
                    Oid = gr?.Oid,
                    InventoryNumber = gr?.InventoryNumber
                };
            if (q.Where(w => w.Estate == null).Any())
                foreach (var item in q.Where(w => w.Estate == null))
                {
                    listErrors
                        .AddError($"В Системе не найден объект имущества с номером ЕУСИ <{item.EUSINumber}>");

                }

            //В заявке для всех номеров ЕУСИ указаны одинаковые
            // -Номер ЕУСИ;
            // -Тип Объекта имущества
            if (Rows.GroupBy(g => g.EstateDefinitionType?.Code).Count() > 1 || Rows.GroupBy(g => g.EUSINumber).Count() > 1)
                listErrors.AddError("В заявке для всех номеров ЕУСИ должны быть указаны одинаковые: Номер ЕУСИ, Тип Объекта имущества.");

            //В заявке есть повторяющиеся инвентарные номеров
            if (Rows.Where(w => !String.IsNullOrWhiteSpace(w.InventoryNumber))
                .GroupBy(g => g.InventoryNumber)
                .Select(group => new {
                    InventoryNumber = group.Key,
                    Count = group.ToList().Count()
                }).Any(w => w.Count > 1))
                listErrors.AddError("В заявке есть повторяющиеся инвентарные номера.");

            //несоответсвие типа ОИ
            if (q.Join(q, l => l.Oid, r => r.Oid, (l, r) => new { l, r })
                .Where(f => (f.l.EstateDefinitionType != null
                && f.r.EstateDefinitionType != null
                && f.l.EstateDefinitionType.Code != f.r.EstateDefinitionType.Code)
                || f.l.EUSINumber != f.r.EUSINumber
                )
                .Any()
                )
                listErrors.AddError($"Тип Объекта имущества, указанный в заявке не соответствует типам объектов имущества в Системе.");

            var eusis = Rows.Select(s => s.EUSINumber).Distinct();

            var ois = UnitOfWork.GetRepository<CorpProp.Entities.Estate.Estate>()
                .FilterAsNoTracking(f => !f.Hidden && !f.IsHistory && f.Number != null && eusis.Contains(f.Number.ToString()))
                .ToList();

            foreach (var oi in ois.Where(f => f.EstateStatus == EstateStatus.Archive))
            {
                listErrors.AddError($"Объект имущества с номером ЕУСИ {oi.EUSINumber} находится в статусе \"Архив\"");
            }

            if (listErrors.Any())
                return listErrors;

            var be = EstateRegistration.Consolidation?.Code;
            foreach (var oi in ois)
            {
                var oss = UnitOfWork.GetRepository<CorpProp.Entities.Accounting.AccountingObject>()
                   .FilterAsNoTracking(f => !f.Hidden && !f.IsHistory && f.EstateID == oi.ID)
                   .Include(inc => inc.Consolidation)
                   .Include(inc => inc.StateObjectRSBU)
                   .ToList();
                // у ОИ нет ОСов с БЕ и в статусе не архив
                if (!oss.Any(f => f.Consolidation != null && f.Consolidation.Code == be))
                {
                    listErrors.AddError($"По объекту имущества с номером ЕУСИ {oi.EUSINumber} не найдено ОС/НМА, удовлетворяющих условию: БЕ в карточке ОС/НМА = БЕ в заявке И Состояние ОС/НМА ≠ В архиве.");
                }
                //у ОИ есть ОСы с другим БЕ и не в статусе Архив
                if (oss.Any(f =>
                (f.Consolidation == null || (f.Consolidation != null && f.Consolidation.Code != be))
                && (f.StateObjectRSBU == null || (f.StateObjectRSBU != null && f.StateObjectRSBU.Code != "Archive"))))
                {
                    listErrors.AddError($"По объекту имущества с номером ЕУСИ {oi.EUSINumber} найдены ОС/НМА, удовлетворяющие условию: БЕ в карточке ОС/НМА ≠ БЕ в заявке И Состояние ОС/НМА ≠ В архиве.");
                }
            }
            return listErrors;
        }

        /// <summary>
        /// Выполнение заявки типа Объединение.
        /// </summary>
        public void ExecuteUnion()
        {
            var rows = _uow.GetRepository<EstateRegistrationRow>()
               .FilterAsNoTracking(f => !f.Hidden && !f.IsHistory && f.EstateRegistrationID == _er.ID)
               .Include(pr => pr.EstateDefinitionType)
               .Include(pr => pr.EstateType)
               .Include(pr => pr.IntangibleAssetType)
               .ToList();
            //выполнение предварительной проверки
            var logs = CheckUnion(_uow, rows, _er);
            if (logs.Any())
            {
                var err = new System.Text.StringBuilder("Заявка не может быть выполнена из-за наличия ошибок: ");
                foreach (var log in logs)
                {
                    err.Append(log.ErrorText);
                }
                throw new Exception(err.ToString());
            }

            var eusis = rows.Select(s => s.EUSINumber).Distinct();
            var invNumber = rows.FirstOrDefault(f => !String.IsNullOrEmpty(f.InventoryNumber))?.InventoryNumber;
            var edt = rows.FirstOrDefault(f => f.EstateDefinitionType != null)?.EstateDefinitionType;

            var be = _er.Consolidation?.Code;
            var estatess = _uow.GetRepository<CorpProp.Entities.Estate.Estate>()
                .Filter(f => !f.Hidden && !f.IsHistory && f.Number != null && eusis.Contains(f.Number.ToString()));

            var ooss = _uow.GetRepository<CorpProp.Entities.Accounting.AccountingObject>()
               .Filter(f => !f.Hidden && !f.IsHistory && f.Estate != null && f.Estate.Number != null && eusis.Contains(f.Estate.Number.ToString()))
               .ToList();

            var invIsNull = String.IsNullOrWhiteSpace(invNumber);

            var osFinded = _uow.GetRepository<CorpProp.Entities.Accounting.AccountingObject>()
                .Filter(f =>
                    !f.Hidden && !f.IsHistory
                    && f.Estate != null && f.Estate.Number != null
                    && eusis.Contains(f.Estate.Number.ToString())
                    && f.Consolidation != null && f.Consolidation.Code == be
                    && !invIsNull
                    && f.InventoryNumber == invNumber)
                    .Include(inc => inc.Estate)
                    .FirstOrDefault();

            CorpProp.Entities.Estate.Estate oi = null;
            if (osFinded == null)
            {
                oi = CreateUnionEstate(edt?.Code, _er.Consolidation?.Code);
                estates.Add(oi);
            }
            else
            {
                oi = osFinded.Estate;
            }

            if (_er.ClaimObject != null)
            {
                UpdateEstate(_er.ClaimObject, ref oi);
                if (osFinded != null)
                {
                    UpdateOS(_er.ClaimObject, ref osFinded);
                }
            }

            //перенос в архив
            var stateArhive = _uow.GetRepository<CorpProp.Entities.NSI.StateObjectRSBU>()
                        .Filter(f => !f.Hidden && f.Code == "Archive")
                        .FirstOrDefault();
            var rrID = _uow.GetRepository<ReceiptReason>()
                .FilterAsNoTracking(f => !f.Hidden && f.Code == "Union")
                .FirstOrDefault()?.ID;

            foreach (var item in estatess)
            {
                if (item.ID != oi.ID)
                {
                    item.EstateStatus = EstateStatus.Archive;
                }

                var estID = item.ID;
                var osses = ooss.Where(f => f.EstateID == estID).ToList();

                if (oi.ID == 0)
                {
                    _uow.GetRepository<CorpProp.Entities.ManyToMany.EstateAndEstate>()
                   .Create(new EstateAndEstate()
                   {
                       ObjLeft = oi,
                       ObjRigth = item
                   });
                }

                _uow.GetRepository<EstateAndEstateRegistrationObject>()
                .Create(new EstateAndEstateRegistrationObject()
                {
                    ObjLeft = item,
                    ObjRigth = _er,
                    IsPrototype = (item.ID == 0),
                });
                estates.Add(item);

                foreach (var obu in osses)
                {
                    if (!(osFinded != null && obu.ID == osFinded.ID))
                    {
                        obu.StateObjectRSBU = stateArhive;
                        obu.StateObjectRSBUID = stateArhive.ID;
                    }
                    if (osFinded != null && obu.ID == osFinded.ID)
                    {
                        obu.ReceiptReason = null;
                        obu.ReceiptReasonID = rrID;
                    }
                    _uow.GetRepository<AccountingObjectAndEstateRegistrationObject>()
                    .Create(new AccountingObjectAndEstateRegistrationObject()
                    {
                        ObjLeft = obu,
                        ObjRigth = _er
                    });
                    oss.Add(obu);
                }
            }

            if (oi.EstateDefinitionType?.Code != edt?.Code)
            {
                oi.EstateDefinitionTypeID = edt?.ID;
                oi.EstateDefinitionType = null;
            }

            CreateFiles();
        }

        /// <summary>
        /// Создает новый объект имущества для заявки типа "Объединение".
        /// </summary>
        /// <param name="id">ИД старого объекта имущества.</param>
        /// <param name="eusiNumber">Номер ЕУСИ.</param>
        /// <param name="be">Код балансовой единицы.</param>
        /// <returns></returns>
        private CorpProp.Entities.Estate.Estate CreateUnionEstate(
            string estateTypeName, string be)
        {
            var newEstate = CreateInstance(estateTypeName) as CorpProp.Entities.Estate.Estate;
            newEstate.ERContractNumber = _er.ERContractNumber;
            newEstate.ERContractDate = _er.ERContractDate;
            newEstate.PrimaryDocNumber = _er.PrimaryDocNumber;
            newEstate.PrimaryDocDate = _er.PrimaryDocDate;

            var newOS = new CorpProp.Entities.Accounting.AccountingObject();

            newOS.Consolidation = _uow.GetRepository<Consolidation>()
                .Filter(f => !f.Hidden && !f.IsHistory && f.Code == be)?.FirstOrDefault();
            newOS.Owner = _uow.GetRepository<CorpProp.Entities.Subject.Society>()
                .Filter(f => !f.Hidden && !f.IsHistory && f.ConsolidationUnit != null && f.ConsolidationUnit.Code == be)
                ?.FirstOrDefault();
            newOS.Estate = newEstate;
            newOS.PrimaryDocNumber = _er.PrimaryDocNumber;
            newOS.PrimaryDocDate = _er.PrimaryDocDate;
            newOS.ContractNumber = _er.ERContractNumber;
            newOS.ContractDate = _er.ERContractDate;
            newOS.ReceiptReason = _uow.GetRepository<CorpProp.Entities.NSI.ReceiptReason>()
                .Filter(f => !f.Hidden && !f.IsHistory && f.Code == "Union")
                ?.FirstOrDefault();
            newOS.CreatingFromER = _er?.Number;

            //Task TFS: 15109
            if (_er.ClaimObject != null)
            {
                newOS.NameEUSI = _er.ClaimObject.NameEUSI;
                if (String.IsNullOrEmpty(newOS.NameEUSI))
                    newOS.NameEUSI = newOS.NameByDoc;
                newOS.SibCountryID = _er.ClaimObject.SibCountryID;
                newOS.SibCityNSIID = _er.ClaimObject.SibCityNSIID;
                newOS.SibFederalDistrictID = _er.ClaimObject.SibFederalDistrictID;
                newOS.RegionID = _er.ClaimObject.SibRegionID;
                newOS.Address = _er.ClaimObject.Address;
                newOS.CadastralNumber = _er.ClaimObject.CadastralNumber;
            }
            
            newOS = _uow.GetRepository<CorpProp.Entities.Accounting.AccountingObject>().Create(newOS);
            oss.Add(newOS);
            _uow.GetRepository<AccountingObjectAndEstateRegistrationObject>()
                .Create(new AccountingObjectAndEstateRegistrationObject()
                {
                    ObjLeft = newOS,
                    ObjRigth = _er,
                    IsPrototype = true
                });
            return newEstate;
        }

        /// <summary>
        /// Выполнение заявки типа Разукрупнение.
        /// </summary>
        public void ExecuteDivision()
        {
            var rows = _uow.GetRepository<EstateRegistrationRow>()
               .FilterAsNoTracking(f => !f.Hidden && !f.IsHistory && f.EstateRegistrationID == _er.ID)
               .Include(pr => pr.EstateDefinitionType)
               .Include(pr => pr.EstateType)
               .Include(pr => pr.IntangibleAssetType)
               .ToList();

            //выполнение предварительной проверки
            var logs = CheckDivision(_uow, rows, _er);
            if (logs.Any())
            {
                var err = new System.Text.StringBuilder("Заявка не может быть выполнена из-за наличия ошибок: ");
                foreach (var log in logs)
                {
                    err.Append(log.ErrorText);
                }
                throw new Exception(err.ToString());
            }

            var groups = rows.GroupBy(g => g.EUSINumber);

            foreach (var gr in groups)
            {
                var inventorys = gr.ToList().Where(w => !String.IsNullOrWhiteSpace(w.InventoryNumber))
                    .Select(s => s.InventoryNumber);

                var oi = _uow.GetRepository<CorpProp.Entities.Estate.Estate>()
                    .Filter(f => !f.Hidden && !f.IsHistory && f.Number != null && f.Number.ToString() == gr.Key)
                    .FirstOrDefault();

                _uow.GetRepository<EstateAndEstateRegistrationObject>()
                    .Create(new EstateAndEstateRegistrationObject()
                    {
                        ObjLeft = oi,
                        ObjRigth = _er,
                    });
                estates.Add(oi);

                var os = _uow.GetRepository<CorpProp.Entities.Accounting.AccountingObject>()
                    .Filter(f => !f.Hidden && !f.IsHistory && f.Estate != null && f.Estate.Number != null
                    && f.Estate.Number.ToString() == gr.Key
                    && !String.IsNullOrEmpty(f.InventoryNumber)
                    && inventorys.Contains(f.InventoryNumber))
                    .FirstOrDefault();

                if (os == null)
                    oi.SendToArchive(_uow);

                foreach (var row in gr.ToList())
                {
                    var currentOS = (String.IsNullOrWhiteSpace(row.InventoryNumber)) ? null :
                        _uow.GetRepository<AccountingObject>()
                        .Filter(f => !f.Hidden && !f.IsHistory && f.Estate != null && f.Estate.Number != null
                        && f.Estate.Number.ToString() == gr.Key
                        && f.InventoryNumber == row.InventoryNumber)
                        .FirstOrDefault();
                    if (currentOS != null)
                    {
                        _uow.GetRepository<AccountingObjectAndEstateRegistrationObject>()
                         .Create(new AccountingObjectAndEstateRegistrationObject()
                         {
                             ObjLeft = currentOS,
                             ObjRigth = _er,
                         });

                        UpdateOS(row, ref currentOS);
                        //TODO: получить уточнения от тестировщиков и методологов
                        //из какой строки объекта заявки обновлять ОИ для случая,
                        //когда для одного ОИ найдено оболее 1-го ОС-а
                        UpdateEstate(row, ref oi);

                        oss.Add(currentOS);
                        continue;
                    }

                    var newEstate = CreateEstate(row);
                    newEstate = _uow.GetRepository<CorpProp.Entities.Estate.Estate>().Create(newEstate);

                    _uow.GetRepository<EstateAndEstateRegistrationObject>()
                       .Create(new EstateAndEstateRegistrationObject()
                       {
                           ObjLeft = newEstate,
                           ObjRigth = _er,
                           IsPrototype = (oi.ID == 0)
                       });
                    estates.Add(newEstate);

                    var newOS = CreateOS(row);
                    newOS.Estate = newEstate;
                    newOS.EstateID = null;
                    newOS.InventoryNumber = row.InventoryNumber;
                    newOS.ReceiptReasonID = _uow.GetRepository<ReceiptReason>()
                                            .FilterAsNoTracking(f => !f.Hidden && !f.IsHistory && f.Code != null && f.Code.ToLower() == "division")
                                            .FirstOrDefault()?.ID;

                    newOS = _uow.GetRepository<CorpProp.Entities.Accounting.AccountingObject>().Create(newOS);
                    _uow.GetRepository<AccountingObjectAndEstateRegistrationObject>()
                      .Create(new AccountingObjectAndEstateRegistrationObject()
                      {
                          ObjLeft = newOS,
                          ObjRigth = _er,
                          IsPrototype = true,
                      });
                    oss.Add(newOS);
                }
            }
            CreateFiles();
        }

        /// <summary>
        /// Проверяет заполненность обязательных полей объектов заявки.
        /// </summary>
        /// <returns></returns>
        public string CheckRequiredRows(IUnitOfWork uow, EstateRegistration er)
        {
            var rows = uow.GetRepository<EstateRegistrationRow>()
               .FilterAsNoTracking(f => !f.Hidden && !f.IsHistory && f.EstateRegistrationID == er.ID)
               .Include(pr => pr.EstateDefinitionType)
               .Include(pr => pr.EstateType)
               .Include(pr => pr.IntangibleAssetType)
               .Include(pr => pr.SibCityNSI)
               .Include(pr => pr.SibCountry)
               .Include(pr => pr.SibFederalDistrict)
               .Include(pr => pr.SibRegion)
               .ToList();

            string strErr = "";

            if (rows.Any(r =>
                    //ВГП
                    CheckRequiredsFields((er.ERType != null && er.ERType.Code == "OSVGP" && String.IsNullOrEmpty(r.EUSINumber)), true, "Номер ЕУСИ", strErr, out strErr)
                || //объединение
                    CheckRequiredsFields((er.ERType != null && er.ERType.Code == "Union" && er.ERReceiptReason != null && er.ERReceiptReason.Code == "Union" && (String.IsNullOrEmpty(r.EUSINumber))), true, "Номер ЕУСИ", strErr, out strErr)
                || //разукрупнение
                    CheckRequiredsFields((er.ERType != null && er.ERType.Code == "Union" && er.ERReceiptReason != null && er.ERReceiptReason.Code == "Division" && String.IsNullOrEmpty(r.EUSINumber)), true, "Номер ЕУСИ", strErr, out strErr)
                || //все остальное
                (
                    (er.ERType != null && er.ERType.Code != "OSVGP" && er.ERType.Code != "Docs" && er.ERType.Code != "Union") &&
                    //проверяем обычные обязательные поля
                    (
                           CheckRequiredsFields(String.IsNullOrEmpty(r.NameEstateByDoc), true, "Наименование объекта (в соответствии с документами)", strErr, out strErr)
                        || CheckRequiredsFields(String.IsNullOrEmpty(r.NameEUSI), true, "Наименование ЕУСИ", strErr, out strErr)
                        || CheckRequiredsFields(er.ERType.Code != "OSVGP" && r.EstateDefinitionType == null, true, "Тип ОИ", strErr, out strErr)
                        || CheckRequiredsFields(r.SibCountry == null, true, "Страна", strErr, out strErr)
                        ||
                        (
                            r.EstateDefinitionType != null

                            &&
                            (
                                    //если это НМА
                                    CheckRequiredsFields((TypesHelper.GetTypeByName(r.EstateDefinitionType.Code).IsNMA() && r.IntangibleAssetType == null), true, "Вид НМА", strErr, out strErr)
                                || //это не НМА
                                    CheckRequiredsFields((!TypesHelper.GetTypeByName(r.EstateDefinitionType.Code).IsNMA() && r.EstateType == null), true, "Класс КС", strErr, out strErr)
                                &&
                                (
                                    (
                                        (
                                            !TypesHelper.GetTypeByName(r.EstateDefinitionType.Code).IsOthers()
                                            && !TypesHelper.GetTypeByName(r.EstateDefinitionType.Code).IsNMA()
                                        )
                                        && (
                                            (
                                                (er.ERType != null && er.ERType.Code?.ToLower() == "osarenda")
                                                && CheckRequiredsFields((r.SibCountry != null && r.SibFederalDistrict != null && r.SibRegion != null && r.SibCityNSI != null && !String.IsNullOrEmpty(r.Address)), false, "Страна; Федеральный Округ; Регион; Населенный пункт ; Адрес", strErr, out strErr)
                                            )
                                            ||
                                            (
                                                CheckRequiredsFields(r.SibFederalDistrict == null, true, "Федеральный Округ", strErr, out strErr)
                                                && CheckRequiredsFields(r.SibRegion == null, true, "Регион", strErr, out strErr)
                                            )
                                           )
                                    )
                                    ||
                                    (
                                        (
                                                TypesHelper.GetTypeByName(r.EstateDefinitionType.Code).IsReals()
                                             || TypesHelper.GetTypeByName(r.EstateDefinitionType.Code).IsLand()
                                             || TypesHelper.GetTypeByName(r.EstateDefinitionType.Code).IsNKS()
                                             || TypesHelper.GetTypeByName(r.EstateDefinitionType.Code).IsOthers()
                                           )
                                        && (
                                                CheckRequiredsFields(!TypesHelper.GetTypeByName(r.EstateDefinitionType.Code).IsOthers() && r.SibCityNSI == null, true, "Населенный пункт", strErr, out strErr)
                                             || CheckRequiredsFields(String.IsNullOrEmpty(r.Address), true, "Адрес", strErr, out strErr)
                                           )
                                    )

                                )
                            )
                        )
                    )
                )
            ))
                return string.Format("В объектах заявки заполнены не все обязательные поля{0}{1}", string.IsNullOrEmpty(strErr) ? "." : ":", string.IsNullOrEmpty(strErr) ? "" : Environment.NewLine + strErr);
            //TODO: сформировать полноценное сообщение со списком незаполненных полей.
            return null;
        }

        public bool CheckRequiredsFields(bool checkV, bool compareV, string Text, string inText, out string outText)
        {
            bool value = bool.Equals(checkV, compareV);
            outText = (value) ? (inText + ((string.IsNullOrEmpty(inText)) ? "" : ";") + Environment.NewLine + Text) : inText;
            //ttext.Add("");
            return value;
        }

        private object CreateInstance(string typeName)
        {
            var type = TypesHelper.GetTypeByName(typeName);
            return Activator.CreateInstance(type);
        }
    }

    public static class EstateExtentions
    {
        public static void FillFromER(
            this CorpProp.Entities.Estate.Estate est
            , string property
            , object value)
        {
            PropertyInfo prop = est.GetType().GetProperty(property);
            if (prop != null && prop.SetMethod != null && value != null)
                prop.SetValue(est, value);
        }

        /// <summary>
        /// Отправляет Ои и связанные ОС в архив.
        /// </summary>
        /// <param name="est">Объект имущества.</param>
        /// <param name="uow">Сессия.</param>
        public static void SendToArchive(
           this CorpProp.Entities.Estate.Estate est
           , IUnitOfWork uow
           )
        {
            est.EstateStatus = EstateStatus.Archive;

            var stateArhive = uow.GetRepository<CorpProp.Entities.NSI.StateObjectRSBU>()
                            .Filter(f => !f.Hidden && f.Code == "Archive")
                            .FirstOrDefault();

            var obu = uow.GetRepository<CorpProp.Entities.Accounting.AccountingObject>()
               .Filter(f => !f.Hidden && !f.IsHistory && f.EstateID == est.ID)
               .Include(inc => inc.StateObjectRSBU)
               .ToList();

            foreach (var item in obu)
            {
                item.StateObjectRSBU = stateArhive;
                item.StateObjectRSBUID = stateArhive?.ID;
            }
        }
    }
}
