using System;
using System.Runtime.Serialization;

namespace Base.Map.Filters
{
    public class FilterDefinitionNotFoundException : Exception
    {
        public FilterDefinitionNotFoundException()
        {
        }

        public FilterDefinitionNotFoundException(string message) : base(message)
        {
        }

        public FilterDefinitionNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FilterDefinitionNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}