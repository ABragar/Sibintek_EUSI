using Base.DAL;
using CorpProp.Common;
using CorpProp.Entities.Document;
using CorpProp.Entities.Import;
using CorpProp.Entities.NSI;
using CorpProp.Extentions;
using CorpProp.Helpers.Import.Extentions;
using EUSI.Entities.Accounting;
using EUSI.Entities.NU;
using EUSI.Extentions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EUSI.Import
{
    public class TaxImportHolder : IImportHolder
    {
        IUnitOfWork _uow;
        FileCardOne _file;
        IUnitOfWork _historyUow;

        ImportHistory _importHistory;
        StreamReader _reader;

        public TaxImportHolder(IUnitOfWork uow, IUnitOfWork historyUow, StreamReader reader, FileCardOne file)
        {
            _uow = uow;
            _historyUow = historyUow;           
            _reader = reader;
            _file = file;
            CreateImportHistory();
        }

        public IUnitOfWork UnitOfWork { get { return _uow; } }

        public IUnitOfWork UofWHistory { get { return _historyUow; } }

        public ImportHistory ImportHistory { get { return _importHistory; } }

        public Consolidation Consolidation { get; set; }

        public void Import()
        {
            Sib.Taxes.Helper.DeclarationImport help = new Sib.Taxes.Helper.DeclarationImport();
            var shema = help.FindValidShema(_reader.BaseStream);
            
            if (!String.IsNullOrEmpty(shema))
            {
                var tt = help.GetTypeByShema(shema);

                if (tt == null)
                {
                    ImportHistory.ImportErrorLogs.AddError("Не удалось определить тип. Для файла не найдена реализация схемы.");
                    return;
                }

                MethodInfo method = help.GetType().GetMethod("DeserializeFromStream");
                MethodInfo generic = method.MakeGenericMethod(tt);                
                var obj = generic.Invoke(help, new object[] { _reader.BaseStream });

                if (obj == null)
                {
                    ImportHistory.ImportErrorLogs.AddError("Не удалось десериализовать объект.");
                    return;
                }               

                this.GetType()
                    .GetMethod("ImportDeclare", new Type[] { obj.GetType(), typeof(int?) })
                    .Invoke(this, new object[] { obj, _file?.ID });
                               
                SetImportResult();


            }
            else
                ImportHistory.ImportErrorLogs.AddError("Файл не соответствует схеме.");
        }


        private void SetImportResult()
        {
            if (ImportHistory.ImportErrorLogs.Count == 0)
            {
                ImportHistory.ResultText = $"Завершено успешно.";                
                _historyUow.SaveChanges();
                _uow.SaveChanges();
            }
            else
            {
                ImportHistory.ResultText = "Завершено с ошибками.";
                if (ImportHistory.ImportErrorLogs.Where(f => f.ErrorType.Contains("схеме")).Any())
                {
                    ImportHistory.ResultText += System.Environment.NewLine;
                    ImportHistory.ResultText += "Файл не соответсвует схеме.";
                }
                else if (ImportHistory.ImportErrorLogs.Count == 1)
                {
                    ImportHistory.ResultText += System.Environment.NewLine;
                    ImportHistory.ResultText += ImportHistory.ImportErrorLogs.First().ErrorText;
                }
                _historyUow.SaveChanges();                
            }
        }

        public void CreateImportHistory()
        {
            _importHistory = _historyUow.GetRepository<ImportHistory>().Create(new ImportHistory());
            _importHistory.FileCardID = _file.ID;
            _importHistory.Mnemonic = nameof(Declaration);
            _importHistory.SibUserID = Base.Ambient.AppContext.SecurityUser?.GetSibUserID(_historyUow);
        }



        private void SetConsolidation(Declaration declare)
        {

            if (!String.IsNullOrEmpty(declare.INN))
            {
                Consolidation = _uow.GetRepository<Consolidation>()
                .Filter(f => !f.Hidden && !f.IsHistory && f.INN == declare.INN).FirstOrDefault();
            }                
            else if (!String.IsNullOrEmpty(declare.ReorgINN))
            {
                Consolidation = _uow.GetRepository<Consolidation>()
                .Filter(f => !f.Hidden && !f.IsHistory && f.INN == declare.ReorgINN).FirstOrDefault();
            }

            if (Consolidation == null)
                ImportHistory.ImportErrorLogs.AddError("Не найдена Балансовая единица.");
        }



        #region Импорт типизированных деклараций

        /// <summary>
        /// Импорт декларации по налогу на ОИ.
        /// </summary>
        /// <param name="obj"></param>
        public void ImportDeclare(Sib.Taxes.Declaration.IMUD.V1_084_00_05_04_02.Файл obj, int? fileID)
        {
            try
            {                
                //TODO: причесать
                if (obj == null) return;

                this.ImportHistory.Mnemonic = nameof(DeclarationEstate);

                int fCorrNumb = 0;

                Int32.TryParse(obj.Документ.НомКорр, out fCorrNumb);

                if (_uow.GetRepository<Declaration>()
                    .FilterAsNoTracking(f => !f.Hidden && !f.IsHistory && f.FileName == obj.ИдФайл && f.CorrectionNumb.HasValue && f.CorrectionNumb.Value == fCorrNumb)
                    .Any())
                {
                    ImportHistory.ImportErrorLogs.AddError($"Декларация <{obj.ИдФайл}> с номером корректировки <{obj.Документ.НомКорр}> уже существует в Системе.");
                    return;
                }

                var declare = _uow.GetRepository<DeclarationEstate>().Create(new DeclarationEstate());
                declare.FileCardID = fileID;
                declare.FileName = obj.ИдФайл;
                declare.SysVersion = obj.ВерсПрог;
                declare.FormatVersion = obj.ВерсФорм.GetEnumAttrValue();
                if (obj.Документ == null) return;
                declare.KND = obj.Документ.КНД.GetEnumAttrValue();
                declare.FileDate = obj.Документ.ДатаДок.GetDate();
                declare.PeriodCode = obj.Документ.Период.GetEnumAttrValue();
                declare.Year = obj.Документ.ОтчетГод;
                declare.AuthorityCode = obj.Документ.КодНО;
                declare.CorrectionNumb = fCorrNumb;
                declare.LocationCode = obj.Документ.ПоМесту.GetEnumAttrValue();

                if (obj.Документ.СвНП != null)
                {
                    declare.Phone = obj.Документ.СвНП.Тлф;
                    if (obj.Документ.СвНП.НПЮЛ != null)
                    {
                        declare.SubjectName = obj.Документ.СвНП.НПЮЛ.НаимОрг;
                        declare.INN = obj.Документ.СвНП.НПЮЛ.ИННЮЛ;
                        declare.KPP = obj.Документ.СвНП.НПЮЛ.КПП;
                        declare.ReorgFormCode = obj.Документ.СвНП.НПЮЛ.СвРеоргЮЛ?.ФормРеорг.GetEnumAttrValue();
                        declare.ReorgINN = obj.Документ.СвНП.НПЮЛ.СвРеоргЮЛ?.ИННЮЛ;
                        declare.ReorgKPP = obj.Документ.СвНП.НПЮЛ.СвРеоргЮЛ?.КПП;
                        SetConsolidation(declare);
                        if (Consolidation == null)
                            return;
                    }

                }
                if (obj.Документ.Подписант != null)
                {
                    declare.RepresentType = obj.Документ.Подписант.ПрПодп.GetEnumAttrValue();
                    declare.LastName = obj.Документ.Подписант.ФИО?.Фамилия;
                    declare.FirstName = obj.Документ.Подписант.ФИО?.Имя;
                    declare.MiddleName = obj.Документ.Подписант.ФИО?.Отчество;
                    declare.RepresentDoc = obj.Документ.Подписант.СвПред?.НаимДок;
                    declare.RepresentOrg = obj.Документ.Подписант.СвПред?.НаимОрг;
                }

                var declRowRepo = new System.Collections.Concurrent.ConcurrentBag<DeclarationRow>();
                var calcRepo = new System.Collections.Concurrent.ConcurrentBag<AccountingCalculatedField>();

                var tbAvg = _uow.GetRepository<TaxBase>().Filter(f => !f.Hidden && f.Code == "101").FirstOrDefault()?.ID;
                var tbCad = _uow.GetRepository<TaxBase>().Filter(f => !f.Hidden && f.Code == "102").FirstOrDefault()?.ID;

                foreach (var item in obj.Документ.ИмущНД)
                {
                    var gr = new DeclarationRow();
                    gr.TypeRow = "1";
                    gr.OKTMO = item.ОКТМО;
                    gr.KBK = item.КБК;
                    gr.Sum = item.НалПУ?.GetValue();
                    gr.Declaration = declare;
                    declRowRepo.Add(gr);

                    Parallel.ForEach(item.РасОбДеятРФ, (ss) =>
                    {
                        try
                        {
                            var calcSS = new AccountingCalculatedField();
                            calcSS.AvgPriceYear = (ss.РасчНал != null && ss.РасчНал.СтИмущ != null) ? ss.РасчНал.СтИмущ.GetValue() : 0m;
                            calcSS.CalculateDate = DateTime.Parse($"31.12.{declare.Year}");
                            calcSS.CalculationDatasource = "НА";
                            calcSS.Consolidation = Consolidation;
                            calcSS.Declaration = declare;
                            calcSS.EstateKindCode = ss.ВидИмущ.GetEnumAttrValue();
                            calcSS.IFNS = declare.AuthorityCode;
                            calcSS.OKTMO = gr.OKTMO;
                            calcSS.OKOF = gr.OKOF;

                            calcSS.PrepaymentSumYear = (ss.РасчНал != null && !String.IsNullOrEmpty(ss.РасчНал.СумАвИсчисл)) ? ss.РасчНал.СумАвИсчисл.GetValue() : 0m;
                            if (ss.ДанРасСтПер != null)
                            {
                                calcSS.ResidualCost_01 = ss.ДанРасСтПер?.ОстСтом0101?.СтОстОН?.GetValue();
                                calcSS.ResidualCost_02 = ss.ДанРасСтПер?.ОстСтом0102?.СтОстОН?.GetValue();
                                calcSS.ResidualCost_03 = ss.ДанРасСтПер?.ОстСтом0103?.СтОстОН?.GetValue();
                                calcSS.ResidualCost_04 = ss.ДанРасСтПер?.ОстСтом0104?.СтОстОН?.GetValue();
                                calcSS.ResidualCost_05 = ss.ДанРасСтПер?.ОстСтом0105?.СтОстОН?.GetValue();
                                calcSS.ResidualCost_06 = ss.ДанРасСтПер?.ОстСтом0106?.СтОстОН?.GetValue();
                                calcSS.ResidualCost_07 = ss.ДанРасСтПер?.ОстСтом0107?.СтОстОН?.GetValue();
                                calcSS.ResidualCost_08 = ss.ДанРасСтПер?.ОстСтом0108?.СтОстОН?.GetValue();
                                calcSS.ResidualCost_09 = ss.ДанРасСтПер?.ОстСтом0109?.СтОстОН?.GetValue();
                                calcSS.ResidualCost_10 = ss.ДанРасСтПер?.ОстСтом0110?.СтОстОН?.GetValue();
                                calcSS.ResidualCost_11 = ss.ДанРасСтПер?.ОстСтом0111?.СтОстОН?.GetValue();
                                calcSS.ResidualCost_12 = ss.ДанРасСтПер?.ОстСтом0112?.СтОстОН?.GetValue();
                                calcSS.ResidualCost_13 = ss.ДанРасСтПер?.ОстСтом3112?.СтОстОН?.GetValue();
                                calcSS.ResidualCost_14 = ss.ДанРасСтПер?.ВтчНедИм?.СтОстОН?.GetValue();

                            }
                            calcSS.TaxBaseID = tbAvg;
                            calcSS.TaxBaseValue = ss.РасчНал?.НалБаза?.GetValue();
                            calcSS.TaxExemption = ss.РасчНал?.КодНалЛьг;
                            calcSS.TaxExemptionLow = ss.РасчНал?.КодЛгПНС;
                            calcSS.TaxRate = ss.РасчНал?.НалСтав;
                            calcSS.TaxSumYear = (ss.РасчНал != null && ss.РасчНал.СумНалИсчисл != null) ? ss.РасчНал.СумНалИсчисл.GetValue() : 0m;
                            calcSS.UntaxedAnnualCostAvg = (ss.РасчНал != null && !string.IsNullOrEmpty(ss.РасчНал.СтИмущНеобл)) ? ss.РасчНал.СтИмущНеобл.GetValue() : 0m; 
                            calcSS.Year = Int32.Parse(declare.Year);

                            //Сумма налога, подлежащая уплате в бюджет
                            var a = calcSS.TaxSumYear - ((ss.РасчНал != null && ss.РасчНал.СумЛгУмен != null) ? ss.РасчНал.СумЛгУмен.GetValue() : 0m);
                            var b = (ss.РасчНал != null && !String.IsNullOrEmpty(ss.РасчНал.СумНалПред)) ? ss.РасчНал.СумНалПред.GetValue() : 0m;
                            calcSS.PaymentTaxSum = (calcSS.EstateKindCode != "04") ? (calcSS.TaxSumYear - calcSS.PrepaymentSumYear.Value - a) :
                            (((calcSS.TaxSumYear - a) > b) ? b : (calcSS.TaxSumYear - a));

                            calcRepo.Add(calcSS);
                        }
                        catch (Exception ex)
                        {
                            ImportHistory.ImportErrorLogs.AddError(ex);
                        }
                    });
                    Parallel.ForEach(item.ОбъектОблНал, (inf) =>
                    {
                        try
                        {
                            var io = new DeclarationRow();
                            io.Declaration = declare;
                            io.TypeRow = "2";
                            io.CadastralNumber = inf.КадастНом;
                            io.ConditionalNumber = inf.УсловНом;
                            io.InventoryNumber = inf.ИнвентНом;
                            io.OKOF = inf.ОКОФ;
                            io.ResidualCost_3112 = inf.СтОст3112?.GetValue();
                            declRowRepo.Add(io);
                        }
                        catch (Exception ex)
                        {
                            ImportHistory.ImportErrorLogs.AddError(ex);
                        }
                    });
                    Parallel.ForEach(item.РасОБНедИО, (cad) =>
                    {
                        try
                        {
                            var calcCad = new AccountingCalculatedField();
                            calcCad.CadastralNumber = (!String.IsNullOrEmpty(cad.НомКадПом)) ? cad.НомКадПом : cad.НомКадЗдан;
                            calcCad.CalculateDate = DateTime.Parse($"31.12.{declare.Year}");
                            calcCad.CalculationDatasource = "НА";
                            calcCad.Consolidation = Consolidation;
                            calcCad.Declaration = declare;
                            calcCad.EstateKindCode = cad.ВидИмущ.GetEnumAttrValue();
                            calcCad.FactorK = (!String.IsNullOrEmpty(cad.КоэфК)) ? cad.КоэфК.GetValue() : 0m;
                            calcCad.IFNS = declare.AuthorityCode;
                            calcCad.IsCadastralCost = true;                           
                            calcCad.OKTMO = gr.OKTMO;
                            calcCad.OKOF = gr.OKOF;
                            calcCad.PaymentTaxSum = cad.СумНалИсчисл.GetValue() - cad.СумАвИсчисл.GetValue() - cad.СумЛгУмен.GetValue();
                            calcCad.PrepaymentSumYear = cad.СумАвИсчисл?.GetValue();
                            calcCad.TaxBaseID = tbCad;
                            calcCad.TaxBaseValue = cad.НалБаза?.GetValue();
                            calcCad.CadastralNumber = (!String.IsNullOrEmpty(cad.НомКадПом)) ? cad.НомКадПом : cad.НомКадЗдан;
                            calcCad.TaxBaseValue = cad.НалБаза?.GetValue();
                            calcCad.TaxExemption = cad.КодНалЛьг;
                            calcCad.TaxExemptionLow = cad.КодЛгПНС;
                            calcCad.TaxRate = cad.НалСтав;
                            calcCad.TaxSumYear = (cad.СумНалИсчисл != null) ? cad.СумНалИсчисл.GetValue() : 0m;
                            calcCad.Year = Int32.Parse(declare.Year);
                            if (!String.IsNullOrEmpty(cad.ДоляПравСоб))
                            {
                                var share = cad.ДоляПравСоб.Split('/');
                                if (share.Length  == 1)
                                    calcCad.ShareRightNumerator = share[0].GetIntValue();
                                else if (share.Length > 0)
                                    calcCad.ShareRightDenominator = share[1].GetIntValue();
                                
                            }

                            calcRepo.Add(calcCad);
                        }
                        catch (Exception ex)
                        {
                            ImportHistory.ImportErrorLogs.AddError(ex);
                        }
                    });
                }


                _uow.GetRepository<DeclarationRow>().CreateCollection(declRowRepo);
                _uow.GetRepository<AccountingCalculatedField>().CreateCollection(calcRepo);
            }
            catch (Exception ex)
            {
                ImportHistory.ImportErrorLogs.AddError(ex);
            }


        }

        /// <summary>
        /// Импорт расчета авансовых платежей по налогу на ОИ.
        /// </summary>
        /// <param name="obj"></param>
        public void ImportDeclare(Sib.Taxes.Declaration.IMUR.V1_085_00_05_04_02.Файл obj, int? fileID)
        {
            try
            {               
                if (obj == null) return;
                this.ImportHistory.Mnemonic = nameof(DeclarationCalcEstate);

                int fCorrNumb = 0;
               
                Int32.TryParse(obj.Документ.НомКорр, out fCorrNumb);

                if (_uow.GetRepository<Declaration>()
                    .FilterAsNoTracking(f => !f.Hidden && !f.IsHistory && f.FileName == obj.ИдФайл && f.CorrectionNumb.HasValue && f.CorrectionNumb.Value == fCorrNumb)
                    .Any())
                {
                    ImportHistory.ImportErrorLogs.AddError($"Декларация <{obj.ИдФайл}> с номером корректировки <{obj.Документ.НомКорр}> уже существует в Системе.");
                    return;
                }

                var declare = _uow.GetRepository<DeclarationCalcEstate>().Create(new DeclarationCalcEstate());
                declare.FileCardID = fileID;
                declare.FileName = obj.ИдФайл;
                declare.SysVersion = obj.ВерсПрог;
                declare.FormatVersion = obj.ВерсФорм.GetEnumAttrValue();
                if (obj.Документ == null) return;
                declare.KND = obj.Документ.КНД.GetEnumAttrValue();
                declare.FileDate = obj.Документ.ДатаДок.GetDate();
                declare.PeriodCode = obj.Документ.Период.GetEnumAttrValue();
                declare.Year = obj.Документ.ОтчетГод;
                declare.AuthorityCode = obj.Документ.КодНО;
                declare.CorrectionNumb = fCorrNumb;
                declare.LocationCode = obj.Документ.ПоМесту.GetEnumAttrValue();

                if (obj.Документ.СвНП != null)
                {
                    declare.Phone = obj.Документ.СвНП.Тлф;
                    if (obj.Документ.СвНП.НПЮЛ != null)
                    {
                        declare.SubjectName = obj.Документ.СвНП.НПЮЛ.НаимОрг;
                        declare.INN = obj.Документ.СвНП.НПЮЛ.ИННЮЛ;
                        declare.KPP = obj.Документ.СвНП.НПЮЛ.КПП;
                        declare.ReorgFormCode = obj.Документ.СвНП.НПЮЛ.СвРеоргЮЛ?.ФормРеорг.GetEnumAttrValue();
                        declare.ReorgINN = obj.Документ.СвНП.НПЮЛ.СвРеоргЮЛ?.ИННЮЛ;
                        declare.ReorgKPP = obj.Документ.СвНП.НПЮЛ.СвРеоргЮЛ?.КПП;
                        SetConsolidation(declare);
                        if (Consolidation == null)
                            return;
                    }

                }
                if (obj.Документ.Подписант != null)
                {
                    declare.RepresentType = obj.Документ.Подписант.ПрПодп.GetEnumAttrValue();
                    declare.LastName = obj.Документ.Подписант.ФИО?.Фамилия;
                    declare.FirstName = obj.Документ.Подписант.ФИО?.Имя;
                    declare.MiddleName = obj.Документ.Подписант.ФИО?.Отчество;
                    declare.RepresentDoc = obj.Документ.Подписант.СвПред?.НаимДок;
                    declare.RepresentOrg = obj.Документ.Подписант.СвПред?.НаимОрг;
                }

                var declRowRepo = new System.Collections.Concurrent.ConcurrentBag<DeclarationRow>();
                var calcRepo = new System.Collections.Concurrent.ConcurrentBag<AccountingCalculatedField>();

                var tbAvg = _uow.GetRepository<TaxBase>().Filter(f => !f.Hidden && f.Code == "101").FirstOrDefault()?.ID;
                var tbCad = _uow.GetRepository<TaxBase>().Filter(f => !f.Hidden && f.Code == "102").FirstOrDefault()?.ID;
                var periodID = _uow.GetRepository<TaxReportPeriod>().Filter(f => !f.Hidden && !f.IsHistory && f.Code == declare.PeriodCode).FirstOrDefault()?.ID;
                var month = "12";
                switch (declare.PeriodCode)
                {
                    case "21":
                    case "51":                       
                        month = "03";
                        break;
                    case "17":
                    case "47":                       
                        month = "06";
                        break;
                    case "18":
                    case "48":                       
                        month = "09";
                        break;
                    default:
                        break;
                }

                var calcDate = DateTime.ParseExact($"{DateTime.DaysInMonth(Int32.Parse(declare.Year), Int32.Parse(month))}.{month}.{declare.Year}"
                                  , "dd.MM.yyyy", CultureInfo.InvariantCulture);

                foreach (var item in obj.Документ.ИмущАв)
                {
                    var gr = new DeclarationRow();
                    gr.TypeRow = "1";
                    gr.OKTMO = item.ОКТМО;
                    gr.KBK = item.КБК;
                    gr.Sum = item.НалПУ?.GetValue();
                    gr.Declaration = declare;
                    declRowRepo.Add(gr);

                    Parallel.ForEach(item.РасОбДеятРФ, (ss) =>
                    {
                        try
                        {
                            var calcSS = new AccountingCalculatedField();
                            calcSS.CalculateDate = calcDate;
                            calcSS.CalculationDatasource = "НА";
                            calcSS.Consolidation = Consolidation;
                            calcSS.Declaration = declare;
                            calcSS.EstateKindCode = ss.ВидИмущ.GetEnumAttrValue();
                            calcSS.IFNS = declare.AuthorityCode;
                            calcSS.OKTMO = gr.OKTMO;                           
                            calcSS.TaxBaseID = tbAvg;
                            calcSS.TaxExemption = ss.РасчАванПл?.КодНалЛьг;
                            calcSS.TaxExemptionLow = ss.РасчАванПл?.КодЛгПНС;
                            calcSS.TaxRate = ss.РасчАванПл?.НалСтав;
                            calcSS.TaxReportPeriodID = periodID;
                            calcSS.UntaxedAnnualCostAvg = (ss.РасчАванПл != null && !string.IsNullOrEmpty(ss.РасчАванПл.СтИмущНеобл)) ? ss.РасчАванПл.СтИмущНеобл.GetValue() : 0m;
                            calcSS.Year = Int32.Parse(declare.Year);

                            decimal avg = (ss.РасчАванПл != null && !String.IsNullOrEmpty(ss.РасчАванПл.СтИмущ)) ? ss.РасчАванПл.СтИмущ.GetValue() : 0m;
                            decimal preSum = ((ss.РасчАванПл != null && !String.IsNullOrEmpty(ss.РасчАванПл.СумАвИсчисл)) ? ss.РасчАванПл.СумАвИсчисл.GetValue() : 0m) -
                            ((ss.РасчАванПл != null && !String.IsNullOrEmpty(ss.РасчАванПл.СумЛгУмен)) ? ss.РасчАванПл.СумЛгУмен.GetValue() : 0m);
                           
                            switch (declare.PeriodCode)
                            {
                                case "21":
                                case "51":
                                    calcSS.AvgPriceFirstQuarter = avg;
                                    calcSS.PrepaymentSumFirstQuarter = preSum;                                                                                                      
                                    break;
                                case "17":
                                case "47":
                                    calcSS.AvgPriceSecondQuarter = avg;
                                    calcSS.PrepaymentSumSecondQuarter = preSum;                                    
                                    break;
                                case "18":
                                case "48":
                                    calcSS.AvgPriceThirdQuarter = avg;
                                    calcSS.PrepaymentSumThirdQuarter = preSum;                                    
                                    break; 
                                default:
                                    break;
                            }                           
                            
                            if (ss.ДанРасСтПер != null)
                            {
                                calcSS.ResidualCost_01 = ss.ДанРасСтПер?.ОстСтом0101?.СтОстОН?.GetValue();
                                calcSS.ResidualCost_02 = ss.ДанРасСтПер?.ОстСтом0102?.СтОстОН?.GetValue();
                                calcSS.ResidualCost_03 = ss.ДанРасСтПер?.ОстСтом0103?.СтОстОН?.GetValue();
                                calcSS.ResidualCost_04 = ss.ДанРасСтПер?.ОстСтом0104?.СтОстОН?.GetValue();
                                calcSS.ResidualCost_05 = ss.ДанРасСтПер?.ОстСтом0105?.СтОстОН?.GetValue();
                                calcSS.ResidualCost_06 = ss.ДанРасСтПер?.ОстСтом0106?.СтОстОН?.GetValue();
                                calcSS.ResidualCost_07 = ss.ДанРасСтПер?.ОстСтом0107?.СтОстОН?.GetValue();
                                calcSS.ResidualCost_08 = ss.ДанРасСтПер?.ОстСтом0108?.СтОстОН?.GetValue();
                                calcSS.ResidualCost_09 = ss.ДанРасСтПер?.ОстСтом0109?.СтОстОН?.GetValue();
                                calcSS.ResidualCost_10 = ss.ДанРасСтПер?.ОстСтом0110?.СтОстОН?.GetValue();                                

                            }                           
                           
                            calcRepo.Add(calcSS);
                        }
                        catch (Exception ex)
                        {
                            ImportHistory.ImportErrorLogs.AddError(ex);
                        }
                    });
                    Parallel.ForEach(item.ОбъектОблНал, (inf) =>
                    {
                        try
                        {
                            var io = new DeclarationRow();
                            io.Declaration = declare;
                            io.TypeRow = "2";
                            io.CadastralNumber = inf.КадастНом;
                            io.ConditionalNumber = inf.УсловНом;
                            io.InventoryNumber = inf.ИнвентНом;
                            io.OKOF = inf.ОКОФ;
                            io.ResidualCost_End = inf.СтОстВс?.GetValue();
                            declRowRepo.Add(io);
                        }
                        catch (Exception ex)
                        {
                            ImportHistory.ImportErrorLogs.AddError(ex);
                        }
                    });
                    Parallel.ForEach(item.РасОБНедИО, (cad) =>
                    {
                        try
                        {
                            var calcCad = new AccountingCalculatedField();
                            calcCad.CadastralNumber = (!String.IsNullOrEmpty(cad.НомКадПом)) ? cad.НомКадПом : cad.НомКадЗдан;
                            calcCad.CadastralValue = cad.СтКадастр.GetValue();
                            calcCad.CalculateDate = calcDate;
                            calcCad.CalculationDatasource = "НА";
                            calcCad.Consolidation = Consolidation;
                            calcCad.Declaration = declare;
                            calcCad.EstateKindCode = cad.ВидИмущ.GetEnumAttrValue();
                            calcCad.IFNS = declare.AuthorityCode;
                            calcCad.IsCadastralCost = true;
                            calcCad.FactorK = (!string.IsNullOrEmpty(cad.КоэфК))?  cad.КоэфК.GetValue(): 0m;
                            calcCad.OKTMO = gr.OKTMO;
                            calcCad.TaxBaseID = tbCad;
                            calcCad.TaxExemption = cad.КодНалЛьг;
                            calcCad.TaxExemptionLow = cad.КодЛгПНС;
                            calcCad.TaxRate = cad.НалСтав;
                            calcCad.TaxReportPeriodID = periodID;
                            calcCad.Year = Int32.Parse(declare.Year);

                            if (!String.IsNullOrEmpty(cad.ДоляПравСоб))
                            {
                                var share = cad.ДоляПравСоб.Split('/');
                                if (share.Length == 1)
                                    calcCad.ShareRightNumerator = share[0].GetIntValue();
                                else if (share.Length > 0)
                                    calcCad.ShareRightDenominator = share[1].GetIntValue();

                            }

                            decimal preSum = ((!String.IsNullOrEmpty(cad.СумАвИсчисл)) ? cad.СумАвИсчисл.GetValue() : 0m) -
                            ((!String.IsNullOrEmpty(cad.СумЛгУмен)) ? cad.СумЛгУмен.GetValue() : 0m);
                            switch (declare.PeriodCode)
                            {
                                case "21":
                                case "51":                                   
                                    calcCad.PrepaymentSumFirstQuarter = preSum;
                                    break;
                                case "17":
                                case "47":                                    
                                    calcCad.PrepaymentSumSecondQuarter = preSum;
                                    break;
                                case "18":
                                case "48":                                   
                                    calcCad.PrepaymentSumThirdQuarter = preSum;
                                    break;
                                default:
                                    break;
                            }

                            calcRepo.Add(calcCad);
                        }
                        catch (Exception ex)
                        {
                            ImportHistory.ImportErrorLogs.AddError(ex);
                        }
                    });
                }


                _uow.GetRepository<DeclarationRow>().CreateCollection(declRowRepo);
                _uow.GetRepository<AccountingCalculatedField>().CreateCollection(calcRepo);
            }
            catch (Exception ex)
            {
                ImportHistory.ImportErrorLogs.AddError(ex);
            }


        }

        /// <summary>
        /// Импорт декларации по налогу на ТС.
        /// </summary>
        /// <param name="obj"></param>
        public void ImportDeclare(Sib.Taxes.Declaration.TRAND.V1_054_00_05_04_01.Файл obj, int? fileID)
        {
            try
            {
                
                if (obj == null) return;


                this.ImportHistory.Mnemonic = nameof(DeclarationVehicle);

                int fCorrNumb = 0;

                Int32.TryParse(obj.Документ.НомКорр, out fCorrNumb);

                if (_uow.GetRepository<Declaration>()
                    .FilterAsNoTracking(f => !f.Hidden && !f.IsHistory && f.FileName == obj.ИдФайл && f.CorrectionNumb.HasValue && f.CorrectionNumb.Value == fCorrNumb)
                    .Any())
                {
                    ImportHistory.ImportErrorLogs.AddError($"Декларация <{obj.ИдФайл}> с номером корректировки <{obj.Документ.НомКорр}> уже существует в Системе.");
                    return;
                }

                var declare = _uow.GetRepository<DeclarationVehicle>().Create(new DeclarationVehicle());
                declare.FileCardID = fileID;
                declare.FileName = obj.ИдФайл;
                declare.SysVersion = obj.ВерсПрог;
                declare.FormatVersion = obj.ВерсФорм.GetEnumAttrValue();
                if (obj.Документ == null) return;
                declare.KND = obj.Документ.КНД.GetEnumAttrValue();
                declare.FileDate = obj.Документ.ДатаДок.GetDate();
                declare.PeriodCode = obj.Документ.Период.GetEnumAttrValue();
                declare.Year = obj.Документ.ОтчетГод;
                declare.AuthorityCode = obj.Документ.КодНО;
                declare.CorrectionNumb = fCorrNumb;
                declare.LocationCode = obj.Документ.ПоМесту.GetEnumAttrValue();

                if (obj.Документ.СвНП != null)
                {
                    declare.Phone = obj.Документ.СвНП.Тлф;
                    if (obj.Документ.СвНП.НПЮЛ != null)
                    {
                        declare.SubjectName = obj.Документ.СвНП.НПЮЛ.НаимОрг;
                        declare.INN = obj.Документ.СвНП.НПЮЛ.ИННЮЛ;
                        declare.KPP = obj.Документ.СвНП.НПЮЛ.КПП;
                        declare.ReorgFormCode = obj.Документ.СвНП.НПЮЛ.СвРеоргЮЛ?.ФормРеорг.GetEnumAttrValue();
                        declare.ReorgINN = obj.Документ.СвНП.НПЮЛ.СвРеоргЮЛ?.ИННЮЛ;
                        declare.ReorgKPP = obj.Документ.СвНП.НПЮЛ.СвРеоргЮЛ?.КПП;
                        SetConsolidation(declare);
                        if (Consolidation == null)
                            return;
                    }

                }
                if (obj.Документ.Подписант != null)
                {
                    declare.RepresentType = obj.Документ.Подписант.ПрПодп.GetEnumAttrValue();
                    declare.LastName = obj.Документ.Подписант.ФИО?.Фамилия;
                    declare.FirstName = obj.Документ.Подписант.ФИО?.Имя;
                    declare.MiddleName = obj.Документ.Подписант.ФИО?.Отчество;
                    declare.RepresentDoc = obj.Документ.Подписант.СвПред?.НаимДок;
                    declare.RepresentOrg = obj.Документ.Подписант.СвПред?.НаимОрг;
                }

                var declRowRepo = new System.Collections.Concurrent.ConcurrentBag<DeclarationRow>();
                var calcRepo = new System.Collections.Concurrent.ConcurrentBag<AccountingCalculatedField>();

                if (obj.Документ.ТрНалНД != null && obj.Документ.ТрНалНД.СумНалПУ != null)
                    foreach (var item in obj.Документ.ТрНалНД.СумНалПУ.СумПУ)
                    {
                        var gr = new DeclarationRow();
                        gr.TypeRow = "1";
                        gr.OKTMO = item.ОКТМО;
                        gr.KBK = obj.Документ.ТрНалНД.СумНалПУ.КБК;                        
                        gr.PrepaymentSumFirstQuarter = item.АвПУКв1.GetValue();
                        gr.PrepaymentSumSecondQuarter = item.АвПУКв2.GetValue();
                        gr.PrepaymentSumThirdQuarter = item.АвПУКв3.GetValue();
                        gr.Sum = item.НалПУ?.GetValue();                        
                        gr.Declaration = declare;
                        declRowRepo.Add(gr);

                        Parallel.ForEach(item.РасчНалТС, (ss) =>
                        {
                            try
                            {
                                var ts = new AccountingCalculatedField();
                                ts.CalculateDate = DateTime.Parse($"31.12.{declare.Year}");
                                ts.CalculationDatasource = "НА";
                                ts.Consolidation = Consolidation;
                                if (!String.IsNullOrEmpty(ss.ЛьготМесТС))
                                    ts.CountFullMonthsBenefit = ss.ЛьготМесТС.GetIntValue();
                                if (!String.IsNullOrEmpty(ss.ВыпускТС))
                                    ts.CountOfYearsIssue = ss.ВыпускТС.GetIntValue();
                                ts.Declaration = declare;
                                ts.EcoKlass = ss.ЭкологКл.GetEnumAttrValue();
                                ts.EstateKindCode = ss.КодВидТС;
                                ts.FactorKl = ss.КоэфКл;
                                ts.FactorKv = ss.КоэфКв;
                                ts.IFNS = declare.AuthorityCode;
                                ts.OKOF = gr.OKOF;
                                ts.OKTMO = gr.OKTMO;
                                ts.PaymentTaxSum = ss.СумИсчислУпл.GetValue();
                                ts.Share = ss.ДоляТС;
                                if (!String.IsNullOrEmpty(ss.ДоляТС))
                                {
                                    var share = ss.ДоляТС.Split('/');
                                    if (share.Length == 1)
                                        ts.ShareRightNumerator = share[0].GetIntValue();
                                    else if (share.Length > 0)
                                        ts.ShareRightDenominator = share[1].GetIntValue();

                                }
                                ts.TaxBaseMeasureTS = ss.ОКЕИНалБаза;
                                ts.TaxBaseValue = ss.НалБаза;
                                ts.TaxBaseValueTS = ss.НалБаза;
                                ts.TaxDeduction = ss.КодВычет.GetEnumAttrValue();
                                ts.TaxDeductionSum = (!String.IsNullOrEmpty(ss.СумВычет))? ss.СумВычет.GetValue():0m;
                                ts.TaxExemptionLow = ss.ЛьготСнижСтав?.КодСнижСтав;
                                ts.TaxExemptionLowSum = ss.ЛьготСнижСтав?.СумСнижСтав?.GetValue();
                                ts.TaxExemptionFree = ss.ЛьготОсвНал?.КодОсвНал;
                                ts.TaxExemptionFreeSum = ss.ЛьготОсвНал?.СумОсвНал?.GetValue();
                                ts.TaxLow = ss.ЛьготУменСум?.КодУменСум;
                                ts.TaxLowSum = ss.ЛьготУменСум?.СумУменСум?.GetValue();
                                ts.TaxRate = ss.НалСтавка;
                                ts.TaxSumYear = (!String.IsNullOrEmpty(ss.СумИсчисл)) ? ss.СумИсчисл.GetValue(): 0m;
                                ts.VehicleMonthOwn = (!String.IsNullOrEmpty(ss.ЛьготМесТС)) ? Int32.Parse(ss.ЛьготМесТС) : (int?)null;
                                ts.VehicleDeRegDate = ss.ДатаСнРегТС?.GetDate();
                                ts.VehicleKindCode = ss.КодВидТС;
                                ts.VehicleModel = ss.МаркаТС;
                                ts.VehicleTaxFactor = ss.КоэфКп;
                                ts.VehicleRegDate = ss.ДатаРегТС?.GetDate();
                                ts.VehicleSerialNumber = ss.ИдНомТС;
                                ts.VehicleSignNumber = ss.РегЗнакТС;
                                ts.VehicleYearOfIssue = (!String.IsNullOrEmpty(ss.ГодВыпТС))? Int32.Parse(ss.ГодВыпТС) : (int?)null;
                                ts.Year = Int32.Parse(declare.Year);
                                                               

                                calcRepo.Add(ts);
                            }
                            catch (Exception ex)
                            {
                                ImportHistory.ImportErrorLogs.AddError(ex);
                            }
                        });
                        
                    }


                _uow.GetRepository<DeclarationRow>().CreateCollection(declRowRepo);
                _uow.GetRepository<AccountingCalculatedField>().CreateCollection(calcRepo);
            }
            catch (Exception ex)
            {
                ImportHistory.ImportErrorLogs.AddError(ex);
            }


        }


        /// <summary>
        /// Импорт декларации по налогу на ЗУ.
        /// </summary>
        /// <param name="obj"></param>
        public void ImportDeclare(Sib.Taxes.Declaration.ZEMND.V1_075_00_05_05_01.Файл obj, int? fileID)
        {
            try
            {

                if (obj == null) return;

                this.ImportHistory.Mnemonic = nameof(DeclarationVehicle);

                int fCorrNumb = 0;

                Int32.TryParse(obj.Документ.НомКорр, out fCorrNumb);

                if (_uow.GetRepository<Declaration>()
                    .FilterAsNoTracking(f => !f.Hidden && !f.IsHistory && f.FileName == obj.ИдФайл && f.CorrectionNumb.HasValue && f.CorrectionNumb.Value == fCorrNumb)
                    .Any())
                {
                    ImportHistory.ImportErrorLogs.AddError($"Декларация <{obj.ИдФайл}> с номером корректировки <{obj.Документ.НомКорр}> уже существует в Системе.");
                    return;
                }

                var declare = _uow.GetRepository<DeclarationLand>().Create(new DeclarationLand());
                declare.FileCardID = fileID;
                declare.FileName = obj.ИдФайл;
                declare.SysVersion = obj.ВерсПрог;
                declare.FormatVersion = obj.ВерсФорм.GetEnumAttrValue();
                if (obj.Документ == null) return;
                declare.KND = obj.Документ.КНД.GetEnumAttrValue();
                declare.FileDate = obj.Документ.ДатаДок.GetDate();
                declare.PeriodCode = obj.Документ.Период.GetEnumAttrValue();
                declare.Year = obj.Документ.ОтчетГод;
                declare.AuthorityCode = obj.Документ.КодНО;
                declare.CorrectionNumb = fCorrNumb;
                declare.LocationCode = obj.Документ.ПоМесту.GetEnumAttrValue();

                if (obj.Документ.СвНП != null)
                {
                    declare.Phone = obj.Документ.СвНП.Тлф;
                    if (obj.Документ.СвНП.НПЮЛ != null)
                    {
                        declare.SubjectName = obj.Документ.СвНП.НПЮЛ.НаимОрг;
                        declare.INN = obj.Документ.СвНП.НПЮЛ.ИННЮЛ;
                        declare.KPP = obj.Документ.СвНП.НПЮЛ.КПП;
                        declare.ReorgFormCode = obj.Документ.СвНП.НПЮЛ.СвРеоргЮЛ?.ФормРеорг.GetEnumAttrValue();
                        declare.ReorgINN = obj.Документ.СвНП.НПЮЛ.СвРеоргЮЛ?.ИННЮЛ;
                        declare.ReorgKPP = obj.Документ.СвНП.НПЮЛ.СвРеоргЮЛ?.КПП;
                        SetConsolidation(declare);
                        if (Consolidation == null)
                            return;
                    }

                }
                if (obj.Документ.Подписант != null)
                {
                    declare.RepresentType = obj.Документ.Подписант.ПрПодп.GetEnumAttrValue();
                    declare.LastName = obj.Документ.Подписант.ФИО?.Фамилия;
                    declare.FirstName = obj.Документ.Подписант.ФИО?.Имя;
                    declare.MiddleName = obj.Документ.Подписант.ФИО?.Отчество;
                    declare.RepresentDoc = obj.Документ.Подписант.СвПред?.НаимДок;
                    declare.RepresentOrg = obj.Документ.Подписант.СвПред?.НаимОрг;
                }

                var declRowRepo = new System.Collections.Concurrent.ConcurrentBag<DeclarationRow>();
                var calcRepo = new System.Collections.Concurrent.ConcurrentBag<AccountingCalculatedField>();

                if (obj.Документ.ЗемНалНД != null && obj.Документ.ЗемНалНД != null)
                    foreach (var item in obj.Документ.ЗемНалНД.СумПУ)
                    {
                        var gr = new DeclarationRow();
                        gr.TypeRow = "1";
                        gr.KBK = item.КБК;
                        gr.OKTMO = item.ОКТМО;
                        gr.PrepaymentSumFirstQuarter = item.АвПУКв1.GetValue();
                        gr.PrepaymentSumSecondQuarter = item.АвПУКв2.GetValue();
                        gr.PrepaymentSumThirdQuarter = item.АвПУКв3.GetValue();
                        gr.Sum = item.НалПУ?.GetValue();
                        gr.Declaration = declare;
                        declRowRepo.Add(gr);

                        Parallel.ForEach(item.РасчПлатЗН, (ss) =>
                        {
                            try
                            {
                                var land = new AccountingCalculatedField();
                                land.Declaration = declare;
                                land.CadastralNumber = ss.НомКадастрЗУ;
                                land.CadastralValue = ss.СтКадастрЗУ;
                                land.CalculateDate = DateTime.Parse($"31.12.{declare.Year}");
                                land.CalculationDatasource = "НА";
                                land.Consolidation = Consolidation;
                                if (ss.СумНалИсчисл != null && !String.IsNullOrEmpty(ss.СумНалИсчисл.КолМесЛьгот))
                                    land.CountFullMonthsBenefit = ss.СумНалИсчисл.КолМесЛьгот.GetIntValue();
                                if (ss.СумНалИсчисл != null && !String.IsNullOrEmpty(ss.СумНалИсчисл.КолМесВлЗУ))
                                    land.CountFullMonthsLand = ss.СумНалИсчисл.КолМесВлЗУ.GetIntValue();
                                land.EstateKindCode = ss.КатегорЗем;
                                land.FactorKl = ss.СумНалИсчисл?.Кл;
                                land.FactorKv = ss.СумНалИсчисл?.Кв;
                                land.IFNS = declare.AuthorityCode;
                                land.OKOF = gr.OKOF;
                                land.OKTMO = gr.OKTMO;
                              
                                land.Share = ss.ДоляЗУ;
                                if (!String.IsNullOrEmpty(ss.ДоляЗУ))
                                {
                                    var share = ss.ДоляЗУ.Split('/');
                                    if (share.Length == 1)
                                        land.ShareTaxPayerNumerator = share[0].GetIntValue();
                                    else if (share.Length > 0)
                                        land.ShareTaxPayerDenominator = share[1].GetIntValue();

                                }
                                land.TaxBaseValue = ss.ОпрНалБаза?.НалБаза?.GetValue();
                                land.TaxExemptionLow = ss.СумНалИсчисл?.ЛьготСнСтав;
                                land.TaxExemptionFree = ss.СумНалИсчисл?.Льгот395?.КодНалЛьгот;
                                land.TaxExemptionFreeSum = ss.СумНалИсчисл?.Льгот395?.СумЛьг?.GetValue();
                                land.TaxExemptionFreeLand = ss.СумНалИсчисл?.Льгот387_2Осв?.КодНалЛьгот;
                                land.TaxExemptionFreeSumLand = ss.СумНалИсчисл?.Льгот387_2Осв?.СумЛьг?.GetValue();
                                land.TaxLow = ss.СумНалИсчисл?.Льгот387_2УмСум?.КодНалЛьгот;
                                land.TaxLowSum = ss.СумНалИсчисл?.Льгот387_2УмСум?.СумЛьг?.GetValue();
                                land.TaxRate = ss.НалСтав;
                                land.TaxSumYear = (ss.СумНалИсчисл != null && ss.СумНалИсчисл.СумНалИсчисл != null) ? ss.СумНалИсчисл.СумНалИсчисл.GetValue() : 0m;
                                land.Year = Int32.Parse(declare.Year);

                                land.PaymentTaxSum = land.TaxSumYear - (land.TaxLowSum ?? 0m) - (land.TaxExemptionFreeSumLand ?? 0m) - (land.TaxExemptionFreeSum ?? 0m);

                                calcRepo.Add(land);
                            }
                            catch (Exception ex)
                            {
                                ImportHistory.ImportErrorLogs.AddError(ex);
                            }
                        });

                    }


                _uow.GetRepository<DeclarationRow>().CreateCollection(declRowRepo);
                _uow.GetRepository<AccountingCalculatedField>().CreateCollection(calcRepo);
            }
            catch (Exception ex)
            {
                ImportHistory.ImportErrorLogs.AddError(ex);
            }


        }

        /// <summary>
        /// Импорт декларации по налогу на ЗУ.
        /// </summary>
        /// <param name="obj"></param>
        public void ImportDeclare(Sib.Taxes.Declaration.ZEMND.V1_075_00_05_04_01.Файл obj, int? fileID)
        {
            try
            {

                if (obj == null) return;

                this.ImportHistory.Mnemonic = nameof(DeclarationVehicle);

                int fCorrNumb = 0;

                Int32.TryParse(obj.Документ.НомКорр, out fCorrNumb);

                if (_uow.GetRepository<Declaration>()
                    .FilterAsNoTracking(f => !f.Hidden && !f.IsHistory && f.FileName == obj.ИдФайл && f.CorrectionNumb.HasValue && f.CorrectionNumb.Value == fCorrNumb)
                    .Any())
                {
                    ImportHistory.ImportErrorLogs.AddError($"Декларация <{obj.ИдФайл}> с номером корректировки <{obj.Документ.НомКорр}> уже существует в Системе.");
                    return;
                }

                var declare = _uow.GetRepository<DeclarationLand>().Create(new DeclarationLand());
                declare.FileCardID = fileID;
                declare.FileName = obj.ИдФайл;
                declare.SysVersion = obj.ВерсПрог;
                declare.FormatVersion = obj.ВерсФорм.GetEnumAttrValue();
                if (obj.Документ == null) return;
                declare.KND = obj.Документ.КНД.GetEnumAttrValue();
                declare.FileDate = obj.Документ.ДатаДок.GetDate();
                declare.PeriodCode = obj.Документ.Период.GetEnumAttrValue();
                declare.Year = obj.Документ.ОтчетГод;
                declare.AuthorityCode = obj.Документ.КодНО;
                declare.CorrectionNumb = fCorrNumb;
                declare.LocationCode = obj.Документ.ПоМесту.GetEnumAttrValue();

                if (obj.Документ.СвНП != null)
                {
                    declare.Phone = obj.Документ.СвНП.Тлф;
                    if (obj.Документ.СвНП.НПЮЛ != null)
                    {
                        declare.SubjectName = obj.Документ.СвНП.НПЮЛ.НаимОрг;
                        declare.INN = obj.Документ.СвНП.НПЮЛ.ИННЮЛ;
                        declare.KPP = obj.Документ.СвНП.НПЮЛ.КПП;
                        declare.ReorgFormCode = obj.Документ.СвНП.НПЮЛ.СвРеоргЮЛ?.ФормРеорг.GetEnumAttrValue();
                        declare.ReorgINN = obj.Документ.СвНП.НПЮЛ.СвРеоргЮЛ?.ИННЮЛ;
                        declare.ReorgKPP = obj.Документ.СвНП.НПЮЛ.СвРеоргЮЛ?.КПП;
                        SetConsolidation(declare);
                        if (Consolidation == null)
                            return;
                    }

                }
                if (obj.Документ.Подписант != null)
                {
                    declare.RepresentType = obj.Документ.Подписант.ПрПодп.GetEnumAttrValue();
                    declare.LastName = obj.Документ.Подписант.ФИО?.Фамилия;
                    declare.FirstName = obj.Документ.Подписант.ФИО?.Имя;
                    declare.MiddleName = obj.Документ.Подписант.ФИО?.Отчество;
                    declare.RepresentDoc = obj.Документ.Подписант.СвПред?.НаимДок;
                    declare.RepresentOrg = obj.Документ.Подписант.СвПред?.НаимОрг;
                }

                var declRowRepo = new System.Collections.Concurrent.ConcurrentBag<DeclarationRow>();
                var calcRepo = new System.Collections.Concurrent.ConcurrentBag<AccountingCalculatedField>();

                if (obj.Документ.ЗемНалНД != null && obj.Документ.ЗемНалНД != null)
                    foreach (var item in obj.Документ.ЗемНалНД.СумПУ)
                    {
                        var gr = new DeclarationRow();
                        gr.TypeRow = "1";
                        gr.KBK = item.КБК;
                        gr.OKTMO = item.ОКТМО;
                        gr.PrepaymentSumFirstQuarter = item.АвПУКв1.GetValue();
                        gr.PrepaymentSumSecondQuarter = item.АвПУКв2.GetValue();
                        gr.PrepaymentSumThirdQuarter = item.АвПУКв3.GetValue();
                        gr.Sum = item.НалПУ?.GetValue();
                        gr.Declaration = declare;
                        declRowRepo.Add(gr);

                        Parallel.ForEach(item.РасчПлатЗН, (ss) =>
                        {
                            try
                            {
                                var land = new AccountingCalculatedField();
                                land.Declaration = declare;
                                land.CadastralNumber = ss.НомКадастрЗУ;
                                land.CadastralValue = ss.СтКадастрЗУ;
                                land.CalculateDate = DateTime.Parse($"31.12.{declare.Year}");
                                land.CalculationDatasource = "НА";
                                land.Consolidation = Consolidation;
                                if (ss.СумНалИсчисл != null && !String.IsNullOrEmpty(ss.СумНалИсчисл.КолМесЛьгот))
                                    land.CountFullMonthsBenefit = ss.СумНалИсчисл.КолМесЛьгот.GetIntValue();
                                if (ss.СумНалИсчисл != null && !String.IsNullOrEmpty(ss.СумНалИсчисл.КолМесВлЗУ))
                                    land.CountFullMonthsLand = ss.СумНалИсчисл.КолМесВлЗУ.GetIntValue();
                                land.EstateKindCode = ss.КатегорЗем;
                                land.FactorKl = ss.СумНалИсчисл?.Кл;
                                land.FactorKv = ss.СумНалИсчисл?.Кв;
                                land.IFNS = declare.AuthorityCode;
                                land.OKOF = gr.OKOF;
                                land.OKTMO = gr.OKTMO;

                                land.Share = ss.ДоляЗУ;
                                if (!String.IsNullOrEmpty(ss.ДоляЗУ))
                                {
                                    var share = ss.ДоляЗУ.Split('/');
                                    if (share.Length == 1)
                                        land.ShareTaxPayerNumerator = share[0].GetIntValue();
                                    else if (share.Length > 0)
                                        land.ShareTaxPayerDenominator = share[1].GetIntValue();

                                }
                                land.TaxBaseValue = ss.ОпрНалБаза?.НалБаза?.GetValue();
                                land.TaxExemptionLow = ss.СумНалИсчисл?.ЛьготСнСтав;
                                land.TaxExemptionFree = ss.СумНалИсчисл?.Льгот395?.КодНалЛьгот;
                                land.TaxExemptionFreeSum = ss.СумНалИсчисл?.Льгот395?.СумЛьг?.GetValue();
                                land.TaxExemptionFreeLand = ss.СумНалИсчисл?.Льгот387_2Осв?.КодНалЛьгот;
                                land.TaxExemptionFreeSumLand = ss.СумНалИсчисл?.Льгот387_2Осв?.СумЛьг?.GetValue();
                                land.TaxLow = ss.СумНалИсчисл?.Льгот387_2УмСум?.КодНалЛьгот;
                                land.TaxLowSum = ss.СумНалИсчисл?.Льгот387_2УмСум?.СумЛьг?.GetValue();
                                land.TaxRate = ss.НалСтав;
                                land.TaxSumYear = (ss.СумНалИсчисл != null && ss.СумНалИсчисл.СумНалИсчисл != null) ? ss.СумНалИсчисл.СумНалИсчисл.GetValue() : 0m;
                                land.Year = Int32.Parse(declare.Year);

                                land.PaymentTaxSum = land.TaxSumYear - (land.TaxLowSum ?? 0m) - (land.TaxExemptionFreeSumLand ?? 0m) - (land.TaxExemptionFreeSum ?? 0m);

                                calcRepo.Add(land);
                            }
                            catch (Exception ex)
                            {
                                ImportHistory.ImportErrorLogs.AddError(ex);
                            }
                        });

                    }


                _uow.GetRepository<DeclarationRow>().CreateCollection(declRowRepo);
                _uow.GetRepository<AccountingCalculatedField>().CreateCollection(calcRepo);
            }
            catch (Exception ex)
            {
                ImportHistory.ImportErrorLogs.AddError(ex);
            }


        }

        #endregion

    }


    public interface IDeclareMainData
    {

    }
   
  
}
