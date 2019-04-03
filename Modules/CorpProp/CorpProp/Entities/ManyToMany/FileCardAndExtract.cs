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
    /// Связь докмуентов и выписок.
    /// </summary>
    public class FileCardAndExtract : ManyToManyAssociation<FileCard, Extract>
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса FileCardAndExtract.
        /// </summary>
        public FileCardAndExtract()
        {

        }
    }
}
