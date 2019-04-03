using Base;
using Base.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Model.Partial.Entities.SeparLand
{
    public class SeparLandRight10 : BaseObject, IChildrenModel
    {
        [SystemProperty]
        public int? ParentID { get; set; }

        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string RighHolder { get; set; }

        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string ShareText { get; set; }

        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string RightKindCode { get; set; }

        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string RighKindAndShare { get; set; }
        [DetailView(Visible = false)]
        public DateTime? RighRegDate { get; set; }

        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string RightRegNumber { get; set; }
    }
}
