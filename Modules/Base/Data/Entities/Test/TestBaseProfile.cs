using Base.Attributes;
using Base.Security;
using Base.Translations;
using Base.Utils.Common.Attributes;

namespace Data.Entities.Test
{
    public class TestBaseProfile : BaseProfile
    {
        private static readonly CompiledExpression<TestBaseProfile, string> _fullname =
            DefaultTranslationOf<TestBaseProfile>.Property(x => x.FullName).Is(x => x.LastName + " " + (x.FirstName ?? "").Trim() + " " + (x.MiddleName ?? "").Trim());

        [ListView("ФИО")]
        [DetailView(Visible = false)]
        [FullTextSearchProperty]
        public new string FullName => _fullname.Evaluate(this);

        [DetailView]
        [ListView]
        public string TestProfileField { get; set; }
    }
}