using Base;
using CorpProp.Entities.CorporateGovernance;
using CorpProp.Entities.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.ManyToMany
{
    public class FileCardAndAppraisal : ManyToManyAssociation<FileCard, Appraisal>
    {
    }
}
