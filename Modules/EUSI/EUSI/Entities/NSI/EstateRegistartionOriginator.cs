using Base.Attributes;
using Base.Translations;
using Base.UI.ViewModal;
using CorpProp.Entities.Base;
using CorpProp.Entities.NSI;
using CorpProp.Helpers;

namespace EUSI.Entities.NSI
{
    /// <summary>
    /// Представляет инициатора заявок на регистрацию.
    /// </summary>
    public class EstateRegistrationOriginator : DictObject
    {
        private static readonly CompiledExpression<EstateRegistrationOriginator, string> _Title =
            DefaultTranslationOf<EstateRegistrationOriginator>
                .Property(x => x.Title).Is(x => x.Code + "/" + x.LastName + " " + x.FirstName + " " + x.Patronymic);

        public EstateRegistrationOriginator() : base()
        {

        }

        /// <summary>
        /// Получает код + наименование.
        /// </summary>
        [ListView(Visible = false)]
        [DetailView(Name = "Наименование", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [SystemProperty]
        public string Title => _Title.Evaluate(this);

        /// <summary>
        /// Отчество.
        /// </summary>
        [DetailView(Name = "Отчество", Visible = true, TabName = CaptionHelper.DefaultTabName)]
        [ListView(Name = "Отчество", Visible = true)]
        [PropertyDataType(PropertyDataType.Text)]
        [SystemProperty]
        public string Patronymic { get; set; }

        /// <summary>
        /// Имя.
        /// </summary>
        [DetailView(Name = "Имя", Visible = true, TabName = CaptionHelper.DefaultTabName)]
        [ListView(Name = "Имя", Visible = true)]
        [PropertyDataType(PropertyDataType.Text)]
        [SystemProperty]
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия.
        /// </summary>
        [DetailView(Name = "Фамилия", Visible = true, TabName = CaptionHelper.DefaultTabName)]
        [ListView(Name = "Фамилия", Visible = true)]
        [PropertyDataType(PropertyDataType.Text)]
        [SystemProperty]
        public string LastName { get; set; }

        /// <summary>
        /// Почта.
        /// </summary>
        [DetailView(Name = "E-mail", Visible = true, TabName = CaptionHelper.DefaultTabName)]
        [ListView(Name = "E-mail", Visible = true)]
        [PropertyDataType(PropertyDataType.EmailAddress)]
        [SystemProperty]
        public string ContactEmail { get; set; }

        /// <summary>
        /// БЕ.
        /// </summary>
        [DetailView(Name = "БЕ", Visible = true, TabName = CaptionHelper.DefaultTabName)]
        [ListView(Name = "БЕ", Visible = true)]
        public Consolidation Consolidation { get; set; }
        
        public int? ConsolidationID { get; set; }
    }
}
