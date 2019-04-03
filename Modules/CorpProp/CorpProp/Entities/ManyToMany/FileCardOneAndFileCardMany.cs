using Base;
using CorpProp.Entities.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.ManyToMany
{
  
    public class FileCardOneAndFileCardMany : ManyToManyAssociation<FileCardOne, FileCardMany> 
    {

        public FileCardOneAndFileCardMany(): base()
        {

        }

        public FileCardOneAndFileCardMany(FileCardOne left, FileCardMany right) : base()
        {
            ObjLeft = left;
            ObjRigth = right;
        }
    }
}
