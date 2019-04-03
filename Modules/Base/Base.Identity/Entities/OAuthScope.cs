using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Base.Attributes;

namespace Base.Identity.Entities
{
    public class OAuthScope: BaseObject
    {
        [DetailView]
        [ListView]
        [Required]
        [MaxLength(255)]
        [UniqueIndex("ScopeName")]
        public string Name { get; set; }
        [DetailView]
        [ListView]
        public bool Enabled { get; set; }
        [DetailView]
        [ListView]
        public string Description { get; set; }

        public virtual ICollection<OAuthClient> Clients { get; set; }
    }
}