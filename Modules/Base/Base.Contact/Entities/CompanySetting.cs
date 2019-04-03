using Base.Attributes;
using Base.Settings;

namespace Base.Contact.Entities
{
    public class CompanySetting : SettingItem
    {
        [DetailView("Компания"), ListView]
        public virtual SimpleCompany Company { get; set; }
    }
}
