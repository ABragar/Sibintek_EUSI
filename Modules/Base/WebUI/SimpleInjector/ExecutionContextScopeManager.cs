using System;
using System.Threading.Tasks;
using Base.Service;
using SimpleInjector;
using SimpleInjector.Extensions.ExecutionContextScoping;

namespace WebUI.SimpleInjector
{



    public class ExecutionContextScopeManager : IExecutionContextScopeManager
    {
        private readonly Container _container;
        private readonly ExecutionContextScopeLifestyle _scoped_lifestyle;

        public ExecutionContextScopeManager(Container container, ExecutionContextScopeLifestyle scoped_lifestyle)
        {
            _container = container;
            _scoped_lifestyle = scoped_lifestyle;
        }

        public bool InScope => _scoped_lifestyle.GetCurrentScope(_container) != null;

        public IDisposable BeginScope()
        {
            return _container.BeginExecutionContextScope();
        }
    }
}