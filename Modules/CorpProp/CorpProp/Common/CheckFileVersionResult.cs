using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Common
{
    public class CheckFileVersionResult
    {
        public IList<int> FileCardIds { get; set; }

        public bool IsError { get; set; }

        public string ErrorMessage { get; set; }

        public string ConfirmMessage { get; set; }
    }
}
