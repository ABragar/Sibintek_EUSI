using CorpProp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.Common
{
    public class CheckImportResult
    {
        public bool IsError { get; set; }

        public string ErrorMessage { get; set; }

        public bool IsConfirmationRequired { get; set; }

        public string ConfirmationItemDescription { get; set; }

        public IConfirmImportChecker Checker { get; set; }
    }
}
