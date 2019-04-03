using Base.Attributes;
using CorpProp.Entities.Base;
using EstateFolder = CorpProp.Entities.Estate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;
using CorpProp.Helpers;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Accounting;
using Base.Translations;
using Base.DAL;

namespace CorpProp.Entities.CorporateGovernance
{
    /// <summary>
    /// Представляет объект оценки.
    /// </summary>
    [EnableFullTextSearch]
    public class EstateAppraisal : TypeObject
    {
        private static readonly CompiledExpression<EstateAppraisal, string> _RegionOfAppaisal =
        DefaultTranslationOf<EstateAppraisal>
       .Property(x => x.RegionOfAppaisal)
       .Is(x => (x.Appraisal != null && x.Appraisal.SibRegion != null) ? x.Appraisal.SibRegion.Name : "");

        private static readonly CompiledExpression<EstateAppraisal, string> _AppraiserOfAppaisal =
        DefaultTranslationOf<EstateAppraisal>
       .Property(x => x.AppraiserOfAppaisal)
       .Is(x => (x.Appraisal != null && x.Appraisal.Appraiser != null) ? x.Appraisal.Appraiser.ShortName : "");

        private static readonly CompiledExpression<EstateAppraisal, decimal?> _AOResidualCost =
        DefaultTranslationOf<EstateAppraisal>
       .Property(x => x.AOResidualCost)
       .Is(x => (x.AccountingObject != null && x.AccountingObject.ResidualCost != null) ? x.AccountingObject.ResidualCost : 0m);

        private static readonly CompiledExpression<EstateAppraisal, DateTime?> _DateOfAppaisal =
        DefaultTranslationOf<EstateAppraisal>
       .Property(x => x.DateOfAppaisal)
       .Is(x => (x.Appraisal != null && x.Appraisal.AppraisalDate != null) ? x.Appraisal.AppraisalDate : new DateTime(0));

        private static readonly CompiledExpression<EstateAppraisal, string> _SocietyOfAppaisal =
        DefaultTranslationOf<EstateAppraisal>
       .Property(x => x.SocietyOfAppaisal)
       .Is(x => (x.Appraisal != null && x.Appraisal.Customer != null) ? x.Appraisal.Customer.ShortName : "");

        /// <summary>
        /// Инициализирует новый экземпляр класса EstateAppraisal.
        /// </summary>
        public EstateAppraisal(): base()
        {
            
        }

       


        /// <summary>
        /// Получает или задает ИД типа оценки.
        /// </summary>
        public int? AppraisalTypeID { get; set; }

        /// <summary>
        /// Получает или задает тип оценки.
        /// </summary>
        /// <remarks>
        /// Внутренний справочник: рыночная стоимость объекта, рыночная стоимость права аренды и пр.
        /// </remarks>      
        [FullTextSearchProperty]
        [ListView(Visible = false)]
        [DetailView(Name = "Тип оценки", Order = 0, TabName = CaptionHelper.DefaultTabName, Required = true, Visible = true)]
        public virtual AppraisalType AppraisalType { get; set; }

        public int? EstateAppraisalTypeID { get; set; }


        /// <summary>
        /// Получает или задает тип объекта оценки (версия 1).
        /// </summary>
        [FullTextSearchProperty]
        [ListView(Visible = false)]
        [DetailView(Name = "Тип объекта оценки", Order = 1, TabName = CaptionHelper.DefaultTabName, Visible = false)]
        public virtual EstateAppraisalType EstateAppraisalType { get; set; }

        /// <summary>
        /// Получает ИД Балансодержателя ОБУ
        /// </summary>
        public int? AOOwnerID { get; set; }

        /// <summary>
        /// Получает Балансодержателя ОБУ
        /// </summary>
        [FullTextSearchProperty]
        [ListView(Visible = true)]
        [DetailView(Name = "Балансодержатель объекта", Order = 2, TabName = CaptionHelper.DefaultTabName)]
        public Subject.Society AOOwner { get; set; }

        /// <summary>
        /// Получает или задает краткое описание объекта оценки.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Краткое описание объекта оценки", Order = 3, TabName = CaptionHelper.DefaultTabName, Required = false)]
        public string ShortDescriptionObjectAppraisal { get; set; }
     
        /// <summary>
        /// Получает или задает объект ID БУ
        /// </summary>
        public int? AccountingObjectID { get; set; }

        [ListView]
        [DetailView(Name = "Объект БУ", Order = 4, TabName = CaptionHelper.DefaultTabName, Required = false)]
        public virtual AccountingObject AccountingObject { get; set; }

        public int? AppTypeID { get; set; }
        /// <summary>
        /// Получает или задает тип объекта оценки (версия справочника 2 2).
        /// </summary>
        [FullTextSearchProperty]
        [ListView(Visible = false)]
        [DetailView(Name = "Тип объекта оценки", Order = 5, TabName = CaptionHelper.DefaultTabName)]
        public virtual AppType AppType { get; set; }

        /// <summary>
        /// Получает Субъект РФ оценки.
        /// </summary>
        
        [ListView(Visible = true)]
        [DetailView(Name = "Субъект РФ", Order = 6, TabName = CaptionHelper.DefaultTabName)]
        [PropertyDataType(PropertyDataType.Text)]
        public string RegionOfAppaisal => _RegionOfAppaisal.Evaluate(this);


        /// <summary>
        /// Получает Балансовая стоимость по РСБУ связанного ОБУ.
        /// </summary>
        [FullTextSearchProperty]
        [ListView(Visible = true)]
        [DetailView(Name = "Балансовая стоимость по РСБУ, руб.", Order = 7, TabName = CaptionHelper.DefaultTabName)]
        [PropertyDataType(PropertyDataType.Text)]
        public decimal? AOResidualCost => _AOResidualCost.Evaluate(this);

        /// <summary>
        /// Получает или задает Балансовую стоимость.
        /// </summary>
        [ListView]
        [DetailView(Name = "Балансовая стоимость на дату оценки", Order = 8, TabName = CaptionHelper.DefaultTabName)]
        [PropertyDataType("Sib_Decimal2")]
        public decimal BalanceCost { get; set; }


        /// <summary>
        /// Получает или задает рыночную стоимость, без НДС.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Cтоимость, без НДС", Order = 9, TabName = CaptionHelper.DefaultTabName, Required = true)]
        public decimal MarketPriceWithoutVAT { get; set; }
        
        /// <summary>
        /// Получает дату связанной оценки
        /// </summary>
        [FullTextSearchProperty]
        [ListView(Visible = true)]
        [DetailView(Name = "Дата оценки", Order = 10, TabName = CaptionHelper.DefaultTabName)]
       
        public DateTime? DateOfAppaisal => _DateOfAppaisal.Evaluate(this);

    
        /// <summary>
        /// Получает Оценочная организация оценки.
        /// </summary>
        [FullTextSearchProperty]
        [ListView(Visible = true)]
        // [DetailView(Name = "Оценочная организация", Order = 24, TabName = CaptionHelper.DefaultTabName, Group = "Отчет", Visible = false)]
        [DetailView(Name = "Оценочная организация")]
        [PropertyDataType(PropertyDataType.Text)]
        public string AppraiserOfAppaisal => _AppraiserOfAppaisal.Evaluate(this);
        
        
        /// <summary>
        /// Получает ОГ заказчик оценки.
        /// </summary>

        [ListView(Visible = true)]
        //[DetailView(Name = "ОГ заказчик оценки", Order = 8, TabName = CaptionHelper.DefaultTabName, Group = "Оценка")]
        [DetailView(Name = "ОГ заказчик оценки")]
        [PropertyDataType(PropertyDataType.Text)]
        public string SocietyOfAppaisal => _SocietyOfAppaisal.Evaluate(this);

        /// <summary>
        /// Получает или задает ИД оценки.
        /// </summary>
        [SystemProperty]
        public int? AppraisalID { get; set; }

        /// <summary>
        /// Получает или задает оценку.
        /// </summary>       
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Оценка", Order = 11, TabName = CaptionHelper.DefaultTabName, Required = true)]
        public virtual Appraisal Appraisal { get; set; }

        /// <summary>
        /// Получает или задает Остаточную стоимость на дату оценки.
        /// </summary>
        [FullTextSearchProperty]
        [ListView(Visible = false)]
        [DetailView(Name = "Остаточная ст-ть на дату оценки", Order = 12, TabName = CaptionHelper.DefaultTabName, Required = false, Visible = false)]
        public decimal ResidualCost { get; set; }

       
        /// <summary>
        /// Вычисление стоимости объекта оценки.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="obj">Объект оценки.</param>
        /// <param name="newAppraisalId">Текущая оценка.</param>
        /// <param name="oldAppraisalId">Предыдущая оценка.</param>
        public void CalculateEstateAppraisalCost(IUnitOfWork uofw, int? newAppraisalId, int? oldAppraisalId)
        {
            //if ((newAppraisalId == oldAppraisalId) && this.ID != 0)
            //{
            //    return;
            //}

            var appraisalRepo = uofw.GetRepository<Appraisal>();
            var estappr = uofw.GetRepository<EstateAppraisal>();
            var oldAppraisal = oldAppraisalId != null ? appraisalRepo.Find(oldAppraisalId) : null;
            var newAppraisal = newAppraisalId != null ? appraisalRepo.Find(newAppraisalId) : null;

            var newEstates = newAppraisalId != null ? estappr.Filter(w => w.AppraisalID == newAppraisalId) : null;
            var oldEstates = oldAppraisalId != null ? estappr.Filter(w => w.AppraisalID == oldAppraisalId) : null;

            var newSum = newEstates != null && newEstates.Count() > 0 ? newEstates.Sum(s => s.MarketPriceWithoutVAT) : 0;
            var newResidualSum = newEstates != null && newEstates.Count() > 0 ? (decimal?)(newEstates.Where(w => w.AccountingObject != null).Sum(s => s.AccountingObject.ResidualCost) ?? 0) : 0;
            var oldSum = oldEstates != null && oldEstates.Count() > 0 ? oldEstates.Sum(s => s.MarketPriceWithoutVAT) : 0;
            var oldResidualSum = oldEstates != null && oldEstates.Count() > 0 ? (decimal?)(oldEstates.Where(w => w.AccountingObject != null).Sum(s => s.AccountingObject.ResidualCost) ?? 0) : 0;

            if (newAppraisalId == oldAppraisalId)
            {
                newSum = newSum - this.MarketPriceWithoutVAT < 0 ? 0 : newSum - this.MarketPriceWithoutVAT;
                newResidualSum = newResidualSum - this.ResidualCost < 0 ? 0 : newResidualSum - this.ResidualCost;
            }

            oldSum = oldSum - this.MarketPriceWithoutVAT < 0 ? 0 : oldSum - this.MarketPriceWithoutVAT;
            oldResidualSum = oldResidualSum - this.AOResidualCost < 0 ? 0 : oldResidualSum - this.AOResidualCost;
            newSum += this.MarketPriceWithoutVAT;
            newResidualSum += this.ResidualCost;

            if (oldAppraisal != null)
            {
                oldAppraisal.MarketAppraisalCost = (decimal)oldResidualSum;
                oldAppraisal.EstateAppraisalCost = oldSum;
                appraisalRepo.Update(oldAppraisal);
            }

            if (newAppraisal != null)
            {
                newAppraisal.MarketAppraisalCost = (decimal)newResidualSum;
                newAppraisal.EstateAppraisalCost = newSum;
                appraisalRepo.Update(newAppraisal);
            }
        }


    }
}
