using System;

namespace Base.Service
{
    public interface IExecutionContextScopeManager
    {
        bool InScope { get; }
        IDisposable BeginScope();
    }
}