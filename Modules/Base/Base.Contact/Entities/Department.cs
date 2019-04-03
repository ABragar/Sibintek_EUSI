using Base.Attributes;
using Base.EntityFrameworkTypes.Complex;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Base.Contact.Entities
{
    public class Department : HCategory
    {
        public Department()
        {
            DepartmentLocation = new Location();
        }

        #region HCategory
        [JsonIgnore]
        [ForeignKey("ParentID")]
        public virtual Department Parent_ { get; set; }
        [JsonIgnore]
        public virtual ICollection<Department> Children_ { get; set; }
        public override HCategory Parent => this.Parent_;
        public override IEnumerable<HCategory> Children => Children_ ?? Enumerable.Empty<HCategory>();
        #endregion

        public int CompanyID { get; set; }
        [DetailView("Компания", Visible = false)]
        public virtual Company Company { get; set; }

        [DetailView(TabName = "[9]Расположение")]
        [PropertyDataType(PropertyDataType.LocationPoint)]
        public Location DepartmentLocation { get; set; }
    }
}