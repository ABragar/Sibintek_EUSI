using Base;
using Base.Attributes;
using CorpProp.Entities.NSI;

namespace CorpProp.Entities.Estate
{
    public class EstateCostAspect : BaseObject
    {
        public EstateCostAspect()
        {
        }

        public int CostOfID { get; set; }

        public virtual Estate CostOf { get; set; }

        [SystemProperty]
        public int? DepreciationMethodRSBUID { get; set; }

        [DetailView("Метод амортизации (РСБУ)", Visible = false)]
        [ListView("Метод амортизации (РСБУ)", Visible = false)]
        public DepreciationMethodRSBU DepreciationMethodRSBU { get; set; }

        [SystemProperty]
        public int? DepreciationMethodNUID { get; set; }

        [DetailView("Метод амортизации (НУ)", Visible = false)]
        [ListView("Метод амортизации (НУ)", Visible = false)]
        public DepreciationMethodNU DepreciationMethodNU { get; set; }

        [SystemProperty]
        public int? DepreciationMethodMSFOID { get; set; }

        [DetailView("Метод амортизации (МСФО)", Visible = false)]
        [ListView("Метод амортизации (МСФО)", Visible = false)]
        public DepreciationMethodMSFO DepreciationMethodMSFO { get; set; }

        [DetailView("СПИ по РСБУ", Visible = false)]
        [ListView("СПИ по РСБУ", Visible = false)]
        public int? UsefulForRSBU { get; set; }

        [DetailView("СПИ по НУ", Visible = false)]
        [ListView("СПИ по НУ", Visible = false)]
        public int? UsefulForNU { get; set; }

        /// <summary>
        /// Получает или задает рыночную стоимость.
        /// </summary>
        /// <remarks>Последняя актуальная рыночная стоимость объекта оценки.	ОИ.Оценка.Объект оценки</remarks>
        [DetailView(Visible = false)]
        public decimal? MarketCost { get; set; }
    }
}