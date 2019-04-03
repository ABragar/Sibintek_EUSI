using System.ComponentModel.DataAnnotations.Schema;
using Base.Attributes;
using Base.Utils.Common.Attributes;

namespace Base.Contact.Entities
{
    [EnableFullTextSearch]
    public class FamilyMember : BaseObject
    {
        [ListView]
        [DetailView("Родственная связь")]
        public Kinship Kinship { get; set; }
        

        public int? OwnerID { get; set; }
        [InverseProperty("Family")]
        public virtual BaseEmployee Owner { get; set; }

        public int? RelativeID { get; set; }
        [ListView]
        [DetailView("Родственник")]
        public virtual BaseEmployee Relative { get; set; }
    }
}