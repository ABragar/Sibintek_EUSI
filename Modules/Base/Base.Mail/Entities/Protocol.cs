using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Attributes;

namespace Base.Mail.Entities
{
    [UiEnum]
    public enum Protocol
    {
        [UiEnumValue("IMAP")]
        Imap,
        [UiEnumValue("POP3")]
        Pop3
    }
}
