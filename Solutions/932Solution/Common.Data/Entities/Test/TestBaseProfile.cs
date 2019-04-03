using Base.Attributes;
using Base.Security;

namespace Common.Data.Entities.Test
{
    public class TestBaseProfile : BaseProfile
    {
        [DetailView]
        [ListView]
        public string TestProfileField { get; set; }
    }
}