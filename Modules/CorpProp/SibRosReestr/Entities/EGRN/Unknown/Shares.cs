using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Размер доли в праве
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("Shares")]
    public class Shares
    {

        [System.Xml.Serialization.XmlElementAttribute("builder_share", typeof(BuilderShare))]
        [System.Xml.Serialization.XmlElementAttribute("builder_share_with_object", typeof(BuilderShareWithObject))]
        [System.Xml.Serialization.XmlElementAttribute("room_owners_share", typeof(RoomOwnersShare))]
        [System.Xml.Serialization.XmlElementAttribute("share", typeof(Share))]
        [System.Xml.Serialization.XmlElementAttribute("share_bal_hectare", typeof(ShareBalHectare))]
        [System.Xml.Serialization.XmlElementAttribute("share_hectare", typeof(ShareHectare))]
        [System.Xml.Serialization.XmlElementAttribute("share_unknown", typeof(ShareUnknown))]
        public object Item { get; set; }
    }
}
