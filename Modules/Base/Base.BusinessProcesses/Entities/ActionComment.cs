using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Attributes;

namespace Base.BusinessProcesses.Entities
{
    public class ActionComment : BaseObject
    {
        [DetailView("Комментарий", HideLabel = true)]
        public string Message { get; set; }
        [PropertyDataType(PropertyDataType.File, Params = "Select=false")]
        [DetailView("Файл", HideLabel = true, Visible = false)]
        public FileData File { get; set; }
    }
}
