using Base;
using Base.Attributes;
using CorpProp.Entities.NSI;

namespace CorpProp.Entities.Estate
{
    public class EstateClassificatorsAspect : BaseObject
    {
        public int ClassificatorsOfID { get; set; }
        public Estate ClassificatorsOf { get; set; }

        public EstateClassificatorsAspect()
        {
        }

        [SystemProperty]
        public int? AddonOKOFID { get; set; }

        [DetailView("Доп. код ОКОФ", Visible = false)]
        [ListView("Доп. код ОКОФ", Visible = false)]
        public AddonOKOF AddonOKOF { get; set; }

        /// <summary>
        /// Получает или задает ИД ОКОФ-94.
        /// </summary>
        [SystemProperty]
        public int? OKOF94ID { get; set; }

        /// <summary>
        /// Получает или задает ОКОФ-94.
        /// </summary>
        [DetailView("ОКОФ", Visible = false)]
        public OKOF94 OKOF94 { get; set; }

        /// <summary>
        /// Получает или задает ИД ОКОФ-2014.
        /// </summary>
        [SystemProperty]
        public int? OKOF2014ID { get; set; }

        /// <summary>
        /// Получает или задает ОКОФ-2014.
        /// </summary>
        [DetailView(Visible = false)]
        public OKOF2014 OKOF2014 { get; set; }

        /// <summary>
        /// Получает или задает ИД OKTMO.
        /// </summary>
        [SystemProperty]
        public int? OKTMOID { get; set; }

        /// <summary>
        /// Получает или задает OKTMO.
        /// </summary>
        [DetailView(Visible = false)]
        public OKTMO OKTMO { get; set; }
    }
}