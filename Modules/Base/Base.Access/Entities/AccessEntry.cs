using System.Collections.Generic;
using Base.Attributes;

namespace Base.Access.Entities
{
    public class AccessEntry : BaseObject
    {
        public int? OwnerID { get; set; }
        public string ForType { get; set; }
        [DetailView("Чтение для всех")]
        public bool ReadAll { get; set; }
        [DetailView("Изменение для всех")]
        public bool UpdateAll { get; set; }
        [DetailView("Удаление для всех")]
        public bool DeleteAll { get; set; }
        [DetailView("Пользователи")]
        public virtual ICollection<UserAccess> Users { get; set; } = new List<UserAccess>();
        [DetailView("Группы пользователей")]
        public virtual ICollection<UserCategoryAccess> UserCategories { get; set; } = new List<UserCategoryAccess>();
    }

    public class CompiledAccessEntry : BaseObject, ICompiledAccessEntry
    {
        public int AccessEntryID { get; set; }
        public int CreatorID { get; set; }
        public string UserIds { get; set; }
        public string UserCategoryIds { get; set; }
    }
}
