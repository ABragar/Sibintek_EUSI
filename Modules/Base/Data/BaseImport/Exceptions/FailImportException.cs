using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.BaseImport.Exceptions
{
    public class FailImportException : Exception
    {
        public FailImportException()
        {
            
        }

        public FailImportException(string message) : base(message)
        {
            
        }
    }
}
