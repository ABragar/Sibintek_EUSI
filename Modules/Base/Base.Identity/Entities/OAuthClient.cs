using Base.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Base.Identity.Entities
{
    public class OAuthClient : BaseObject
    {
        [DetailView]
        [ListView]
        [Required]
        [MaxLength(255)]
        [UniqueIndex("ClientId")]
        public string ClientId { get; set; }
        [DetailView]
        [ListView]
        [MaxLength(255)]
        [Required]
        public string ClientSecret { get; set; }
        [DetailView]
        [ListView]
        [MaxLength(255)]
        public string RedirectUri { get; set; }
        [DetailView]
        [ListView]
        [Required]
        public string Title { get; set; }
        [DetailView]
        [ListView]
        [Required]
        public string Description { get; set; }
        [DetailView]
        [ListView]
        public bool Enabled { get; set; }
        [DetailView]
        public virtual ICollection<OAuthScope> Scopes { get; set; }
    }

}