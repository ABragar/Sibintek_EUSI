using Base;
using CorpProp.Entities.ProjectActivity;
using CorpProp.Entities.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.ManyToMany
{
    public class SibTaskAndSibUser : ManyToManyAssociation<SibTask, SibUser>
    {
    }
}
