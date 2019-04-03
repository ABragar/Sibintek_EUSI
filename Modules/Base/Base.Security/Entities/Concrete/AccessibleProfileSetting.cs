using Base.Attributes;
using Base.Translations;

namespace Base.Security.Entities.Concrete
{
    public class ProfileAccess : BaseObject
    {
        private static readonly CompiledExpression<ProfileAccess, string> _title = DefaultTranslationOf<ProfileAccess>
            .Property(x => x.Title).Is(x => x.ProfileMnemonic);

        [DetailView("Наименование")]
        [ListView(Order = 3, Filterable = false, Sortable = false, Width = 240)]
        public string Title => _title.Evaluate(this);

        [ListView]
        [DetailView("Профиль", ReadOnly = true)]
        public string ProfileMnemonic { get; set; }


    }
}
