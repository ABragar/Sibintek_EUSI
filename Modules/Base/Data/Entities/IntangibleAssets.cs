using Base.Attributes;
using Data.Enums;

namespace Data.Entities
{
    public class IntangibleAssets : PropertyObject
    {
        [DetailView("Тип НМА", Required = true)]
        [ListView]
        public virtual NmaType NmaType { get; set; }

        [DetailView("Автор")]
        public string Author { get; set; }
    }
}
