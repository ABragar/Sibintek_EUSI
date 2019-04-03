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
   
    /// <summary>
    /// Связь правоудостоверяющего документа и права. 
    /// </summary>
    public class FileCardAndCertificateRight : ManyToManyAssociation<FileCard, Right>
    {
        public FileCardAndCertificateRight()
        {

        }
    }
}
