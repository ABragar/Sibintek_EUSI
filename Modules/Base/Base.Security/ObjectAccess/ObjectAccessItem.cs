using Base.Attributes;
using Base.Entities.Complex;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Base.Security.ObjectAccess
{
    public class ObjectAccessItem : BaseObject
    {
        public LinkBaseObject Object { get; set; }
        public int CreatorID { get; set; }
        [JsonIgnore]
        public virtual User Creator { get; set; }

        [DetailView("Чтение для всех")]
        public bool ReadAll { get; set; }
        [DetailView("Изменение для всех")]
        public bool UpdateAll { get; set; }
        [DetailView("Удаление для всех")]
        public bool DeleteAll { get; set; }
        [DetailView("Изменение доступа для всех")]
        public bool ChangeAccessAll { get; set; }

        [DetailView("Пользователи")]
        public virtual ICollection<UserAccess> Users { get; set; } = new List<UserAccess>();

        [DetailView("Группы пользователей")]
        public virtual ICollection<UserCategoryAccess> UserCategories { get; set; } = new List<UserCategoryAccess>();


        public ObjectAccessItem()
        {
            this.ReadAll = true;
            this.UpdateAll = true;
            this.DeleteAll = true;
            this.ChangeAccessAll = true;
        }

        public ObjectAccessItem(ISecurityUser securityUser, BaseObject obj)
            : this(securityUser.ID, obj)
        { }

        public ObjectAccessItem(ISecurityUser securityUser, Type type, int id)
            : this(securityUser.ID, type, id)
        { }

        public ObjectAccessItem(int userID, BaseObject obj)
            : this()
        {
            this.CreatorID = userID;
            this.Object = new LinkBaseObject(obj);
        }

        public ObjectAccessItem(int userID, Type type, int id)
            : this()
        {
            this.CreatorID = userID;
            this.Object = new LinkBaseObject(type, id);
        }
    }
}
