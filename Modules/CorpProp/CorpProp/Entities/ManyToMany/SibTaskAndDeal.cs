using Base;
using CorpProp.Entities.DocumentFlow;
using CorpProp.Entities.ProjectActivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.ManyToMany
{
    public class SibTaskAndDeal : ManyToManyAssociation<SibTask, SibDeal>
    {
    }
}
