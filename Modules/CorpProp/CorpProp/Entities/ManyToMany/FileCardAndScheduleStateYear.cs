using Base;
using CorpProp.Entities.Document;
using CorpProp.Entities.Law;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.ManyToMany
{
    public class FileCardAndScheduleStateYear : ManyToManyAssociation<FileCard, ScheduleStateYear>
    {
    }
}
