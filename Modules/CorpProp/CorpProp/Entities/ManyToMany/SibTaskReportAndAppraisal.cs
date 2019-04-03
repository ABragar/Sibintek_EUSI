using Base;
using CorpProp.Entities.CorporateGovernance;
using CorpProp.Entities.ProjectActivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.ManyToMany
{
    public class SibTaskReportAndAppraisal : ManyToManyAssociation<SibTaskReport, Appraisal>
    {
    }
}
