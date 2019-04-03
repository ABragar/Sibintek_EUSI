using Base.Attributes;
using System;
using Base.Enums;
using System.Collections.Generic;

namespace Base.Security
{
    public class Permission: BaseObject
    {
        public Permission()
        {
            
        }

        public Permission(Type type)
        {
            FullName = type.GetTypeName();
            AllowRead = AllowWrite = AllowCreate = AllowDelete = AllowNavigate = true;
        }

        public Permission(Type type, TypePermission permissions)
        {
            FullName = type.GetTypeName();
            AllowRead = permissions.HasFlag(TypePermission.Read);
            AllowWrite = permissions.HasFlag(TypePermission.Write);
            AllowCreate = permissions.HasFlag(TypePermission.Create);
            AllowDelete = permissions.HasFlag(TypePermission.Delete);
            AllowNavigate = permissions.HasFlag(TypePermission.Navigate);
        }

        public int RoleID { get; set; }
        public Role Role { get; set; }

        [DetailView(Name = "Объект")]
        [ListView]
        [PropertyDataType(PropertyDataType.ListBaseObjects)]
        public string FullName { get; set; }
        [DetailView(Name = "Чтение")]
        [ListView]
        public bool AllowRead { get; set; }
        [DetailView(Name = "Запись")]
        [ListView]
        public bool AllowWrite { get; set; }
        [DetailView(Name = "Создание")]
        [ListView]
        public bool AllowCreate { get; set; }
        [DetailView(Name = "Удаление")]
        [ListView]
        public bool AllowDelete { get; set; }
        [DetailView(Name = "Навигация")]
        [ListView]
        public bool AllowNavigate { get; set; }

        [DetailView(TabName = "Поля")]
        public virtual ICollection<PropertyPermission> PropertyPermissions { get; set; }
    }
}
