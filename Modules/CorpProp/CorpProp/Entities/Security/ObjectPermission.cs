using Base;
using Base.Attributes;
using Base.Security;
using Base.Utils.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enums = Base.Enums;

namespace CorpProp.Entities.Security
{
    /// <summary>
    /// Разрешения на объект.
    /// </summary>
    [EnableFullTextSearch]
    public class ObjectPermission : BaseObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса ObjectPermission.
        /// </summary>
        public ObjectPermission() : base()
        {
            AllowRead = AllowWrite = AllowDelete = false;           
        }

        public ObjectPermission(Permission typePermission, Enums.TypePermission permissions, string criteria = "") : base()
        {            
            AllowRead = permissions.HasFlag(Enums.TypePermission.Read);
            AllowWrite = permissions.HasFlag(Enums.TypePermission.Write);            
            AllowDelete = permissions.HasFlag(Enums.TypePermission.Delete);
            TypePermission = typePermission;
            Role = typePermission?.Role;
            Criteria = criteria;
        }

        /// <summary>
        /// Получает или задает ИД роли.
        /// </summary>
        [SystemProperty]
        public int? RoleID { get; set; }

        /// <summary>
        /// Получает или задает роль.
        /// </summary>
        [ListView(Visible = false)]
        [DetailView(Name = "Роль", Visible = false)]
        public virtual Role Role { get; set; }

        /// <summary>
        /// Получает или задает ИД разрешения на тип.
        /// </summary>
        public int? TypePermissionID { get; set; }

        /// <summary>
        /// Получает или задает разрешение на тип.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Разрешение на тип", Visible = false)]
        public virtual Permission TypePermission { get; set; }

        /// <summary>
        /// Получает или задает критерий доступа к экземплярам.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Критерий", Required = true)]
        //[PropertyDataType(PropertyDataType.Text)]
        public string Criteria { get; set; }

        /// <summary>
        /// Получает или задает разрешение на чтение.
        /// </summary>
        [DetailView(Name = "Чтение")]
        [ListView]
        public bool AllowRead { get; set; }

        //невозможно дать разрешение на создание экземпляров по условию.
        ///// <summary>
        ///// Получает или задает разрешение на создание.
        ///// </summary>
        //[DetailView(Name = "Создание")]
        //[ListView]
        //public bool AllowCreate { get; set; }

        /// <summary>
        /// Получает или задает разрешение на запись.
        /// </summary>
        [DetailView(Name = "Редактирование")]
        [ListView]
        public bool AllowWrite { get; set; }
       

        /// <summary>
        /// Получает или задает разрешение на удаление.
        /// </summary>
        [DetailView(Name = "Удаление")]
        [ListView]
        public bool AllowDelete { get; set; }
    }
   
}
