using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Base.DAL;
using Base.Service;
using CorpProp.Entities.Document;
using CorpProp.Entities.ManyToMany;
using CorpProp.Services.Security;

namespace CorpProp.Services.Document
{
    public class FileCardOneAndFileCardManySevice: SibAccessableObjectCategorizedItemService<FileCardOne>
    {
        public FileCardOneAndFileCardManySevice(IBaseObjectServiceFacade facade) : base(facade)
        {
        }
    }
}
