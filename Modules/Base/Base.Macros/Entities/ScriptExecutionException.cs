using System;
using System.Runtime.Serialization;

namespace Base.Macros.Entities
{
    [Serializable]
    public class ScriptExecutionException : Exception
    {
        public ScriptExecutionException()
        {
        }

        public ScriptExecutionException(string message)
            : base(message)
        {
        }

        public ScriptExecutionException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected ScriptExecutionException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}