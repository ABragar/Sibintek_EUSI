using Base;
using CorpProp.Entities.CorporateGovernance;
using CorpProp.Entities.Document;
using CorpProp.Entities.Law;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.ManyToMany
{
  
    /// <summary>
    /// Связь правоустанавливающего документа и права. 
    /// </summary>
    public class FileCardAndLegalRight : ManyToManyAssociation<FileCard, Right>
    {
        public FileCardAndLegalRight() { }

    }
}
