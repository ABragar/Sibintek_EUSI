using Base;
using CorpProp.Entities.Document;
using CorpProp.Entities.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.ManyToMany
{
    
    public class FileCardAndResponse : ManyToManyAssociation<FileCard, Response>
    {
    }
}
