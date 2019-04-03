using Base;
using CorpProp.Entities.Document;
using CorpProp.Entities.DocumentFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.ManyToMany
{
  
    public class FileCardAndDoc : ManyToManyAssociation<FileCard, Doc>
    {
    }

}
