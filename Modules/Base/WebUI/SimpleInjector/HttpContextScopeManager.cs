using System;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Web;
using SimpleInjector;

namespace WebUI.SimpleInjector
{
    public class HttpContextScopeManager
    {
        private readonly string _key = Guid.NewGuid().ToString("N");

        public Scope BeginScope()
        {

            if (GetCurrent() != null)
                throw new InvalidOperationException("not support nester scopes");


            var context = HttpContext.Current;

            var scope = new Scope();


            scope.WhenScopeEnds(() =>
            {
                context.Items.Remove(this);
                CallContext.LogicalSetData(_key, null);
            });

            var wrapper = new ScopeWrapper(scope);

            CallContext.LogicalSetData(_key, wrapper);
            context.Items.Add(this, wrapper);

            return scope;

        }



        public Scope GetCurrent()
        {

            var wrapper = CallContext.LogicalGetData(_key) as ScopeWrapper;
            if (wrapper != null)
                return wrapper.Scope;


            wrapper =  HttpContext.Current?.Items[this] as ScopeWrapper;

            if (wrapper == null)
                return null;

            CallContext.LogicalSetData(_key, wrapper);

            return wrapper.Scope;
        }

        private class ScopeWrapper : MarshalByRefObject
        {
            public ScopeWrapper(Scope scope)
            {
                Scope = scope;
            }

            public Scope Scope { get; }
        }

    }
}