using Base;
using Base.Attributes;
using CorpProp.Attributes;
using CorpProp.Entities.Estate;

namespace CorpProp.Model.Partial.Entities.SeparLand
{
    public class SeparLandLinks14 : BaseObject, IChildrenModel
    {
        [SystemProperty]
        public int? ParentID { get; set; }

        /// <summary>
        /// Получает или задает ИД имущественного комплекса.
        /// </summary>
        [SystemProperty]
        public int? PropertyComplexID { get; set; }


        /// <summary>
        /// Получает или задает имущественный комплекс.
        /// </summary>
//        [Historical]
        [DetailView("ИК", Visible = false)]
        [ListView(Hidden = true)]
        public PropertyComplex PropertyComplex { get; set; }

        /// <summary>
        /// Получаеет или задает ИД ЗУ, на котором расположен объект.
        /// </summary>
        [SystemProperty]
        public int? LandID { get; set; }

        /// <summary>
        /// Получает или задает ЗУ, на котором расположен объект.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Земельный участок",
            Description = "Земельный участок, на котором расположен объект", Visible = false)]
        public Land Land { get; set; }

        /// <summary>
        /// Получает или задает ИД кадастрового объекта' (штрих).
        /// </summary>
        [SystemProperty]
        public int? FakeID { get; set; }

        /// <summary>
        /// Получает или задает кадастровый объект' (штрих).
        /// </summary>
        /// <remarks>
        /// Создается для объединения нескольких МА с одним кадстровым номером в один фэйковый кадастровый объект.
        /// </remarks>
        [ListView(Hidden = true)]
        [DetailView(Name = "Кадастровый объект'", Visible = false, ReadOnly = true)]
        public Cadastral Fake { get; set; }
    }
}
