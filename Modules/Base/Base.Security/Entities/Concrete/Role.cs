using Base.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Base.Enums;

namespace Base.Security
{
    public class Role : BaseObject
    {
        [DetailView(Name = "Наименование", Required = true)]
        [MaxLength(255)]
        [ListView]
        public string Name { get; set; }

        [DetailView("Системная роль")]
        public SystemRole SystemRole { get; set; }

        //sib

        /// <summary>
        /// Получает ии задает код (системное имя) роли.
        /// </summary>
        [SystemProperty]
        [ListView(Visible = false)]
        [DetailView("Код")]
        public string Code { get; set; }

        //end sib

        [DetailView(TabName = "[1]Разрешения")]
        public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();

        //ManyToMany
        public ICollection<UserCategory> Categories { get; set; }

        public Role() : base()
        {
            SystemRole = SystemRole.Base;
        }

      
    }
}
