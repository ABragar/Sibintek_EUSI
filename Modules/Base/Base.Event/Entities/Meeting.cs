using System.Collections.Generic;
using System.Linq;
using Base.Attributes;
using Base.Contact.Entities;
using Base.EntityFrameworkTypes.Complex;
using Base.Security;

namespace Base.Event.Entities
{
    public class Meeting : Event
    {
        public Meeting()
        {
            this.Location = new Location();
        }

        [DetailView(Name = "Список участников", Order = 10)]
        public virtual ICollection<Participant> Participants { get; set; } = new List<Participant>();

        [DetailView("Расположение", TabName = "[1]Место")]
        [PropertyDataType(PropertyDataType.LocationPoint)]
        public Location Location { get; set; }

        public int ResponsibleID { get; set; }
        [DetailView("Ответственный", Required = true)]
        public virtual User Responsible { get; set; }

        public override IEnumerable<User> GetStakeHolders()
        {
            return new List<User>() { Creator, Responsible };
        }
    }
}