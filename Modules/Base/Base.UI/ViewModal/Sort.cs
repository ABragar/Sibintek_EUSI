using System.ComponentModel;

namespace Base.UI.ViewModal
{
    public class Sort
    {        
        public ListSortDirection Order { get; set; }
        public string Property { get; set; }

        public Sort Copy()
        {
            return new Sort()
            {
                Order = this.Order,
                Property = this.Property
            };
        }
    }
}