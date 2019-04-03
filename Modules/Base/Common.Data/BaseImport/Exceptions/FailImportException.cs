using System;

namespace Common.Data.BaseImport.Exceptions
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
