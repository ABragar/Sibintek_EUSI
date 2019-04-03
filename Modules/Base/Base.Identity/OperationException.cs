using System;
using System.Collections.Generic;

namespace Base.Identity
{
    public class OperationException : Exception
    {
        internal OperationException(IReadOnlyCollection<string> messages) : base( string.Join(", ", messages))
        {
            Messages = messages;
        }

        public IReadOnlyCollection<string> Messages { get; }
    }
}