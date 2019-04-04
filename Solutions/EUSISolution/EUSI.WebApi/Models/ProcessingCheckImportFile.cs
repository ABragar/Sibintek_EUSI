using CorpProp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUSI.WebApi.Models
{
    public class ProcessingCheckImportFile
    {
        public List<string> FileDescriptions { get; set; }

        public IList<int> FileCardIds { get; set; }

        public IConfirmImportChecker Checker { get; set; }
    }
}
