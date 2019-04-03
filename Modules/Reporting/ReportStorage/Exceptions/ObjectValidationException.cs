using System;
using System.Runtime.Serialization;

namespace ReportStorage.Exceptions
{
    public class ObjectValidationException : Exception
    {
        public ObjectValidationException()
        {
        }

        public ObjectValidationException(string message) : base(message)
        {
        }

        public ObjectValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ObjectValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}