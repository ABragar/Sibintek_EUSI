using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base
{
    public interface ITreeItem
    {
        int ID { get; set; }
        int? ParentID { get; set; }
        bool hasChildren { get; }
    }
}
