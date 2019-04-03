using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Attributes;

namespace Base.Contact.Entities
{
    [UiEnum]
    public enum AgentType
    {
        [UiEnumValue("Законный представитель")]
        LegalRepresentative,
        [UiEnumValue("Представитель по договоренности")]
        AgreementRepresentative
    }
}
