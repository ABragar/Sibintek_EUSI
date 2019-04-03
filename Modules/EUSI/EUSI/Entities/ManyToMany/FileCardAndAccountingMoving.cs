using CorpProp.Entities.Document;
using EUSI.Entities.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUSI.Entities.ManyToMany
{   
    
    /// <summary>
    /// Первичные документы регистра движений ОС.
    /// </summary>
    public class FileCardAndAccountingMoving : Base.ManyToManyAssociation<FileCard, AccountingMoving>
    {
        public FileCardAndAccountingMoving() : base()
        {

        }
    }
}
