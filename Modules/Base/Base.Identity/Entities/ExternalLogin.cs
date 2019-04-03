using System.ComponentModel.DataAnnotations;
using Base.Attributes;

namespace Base.Identity.Entities
{
    public class ExternalLogin : BaseObject
    {
        [UniqueIndex("User_Providers", 0)]
        public int AccountId { get; set; }

        public AccountEntry Account { get; set; }

        [MaxLength(255)]
        [Required]
        [UniqueIndex("User_Providers", 1)]
        [UniqueIndex("Provider_Key", 0)]
        public string ProviderKey { get; set; }

        [MaxLength(255)]
        [Required]
        [UniqueIndex("Provider_Key", 1)]
        public string UserKey { get; set; }
    }
}