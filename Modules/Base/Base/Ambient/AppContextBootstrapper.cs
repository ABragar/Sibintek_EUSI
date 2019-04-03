using Base.Security;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Base.Service;

namespace Base.Ambient
{
    public class AppContextBootstrapper : IAppContextBootstrapper
    {
        private readonly IServiceFactory<IUserContextScope> _user_context_scope_factory;
        private readonly IExecutionContextScopeManager _scope_manager;

        private readonly IDateTimeProvider _date_time_provider;
        public AppContextBootstrapper(IDateTimeProvider dateTimeProvider, IServiceFactory<IUserContextScope> user_context_scope_factory,IExecutionContextScopeManager scope_manager)
        {
            _user_context_scope_factory = user_context_scope_factory;
            _scope_manager = scope_manager;

            if (dateTimeProvider == null)
                throw new ArgumentNullException(nameof(dateTimeProvider));

            _date_time_provider = dateTimeProvider;

            AppContext.SetContextService(this);
        }


        public ISecurityUser GetSecurityUser()
        {
            return _scope_manager.InScope ? _user_context_scope_factory.GetService().CurrentUser : null;
        }

        public IDisposable LocalContextSecurity(Func<ISecurityUser> securityUserfunc)
        {

            return _user_context_scope_factory.GetService().Push(securityUserfunc);

        }
        public IDisposable LocalContextSecurity(ISecurityUser user)
        {

            return _user_context_scope_factory.GetService().Push(()=>user);

        }

        public IDateTimeProvider GetDateTimeProvider()
        {
            return _date_time_provider;
        }


    }

    public interface IUserContextScope : IDisposable
    {
        IDisposable Push(Func<ISecurityUser> user_func);

        ISecurityUser CurrentUser { get; }

    }

    public class UserContextScope : IUserContextScope
    {

        public IDisposable Push(Func<ISecurityUser> user_func)
        {

            _current_entry = new UserContextScopeEntry(this, user_func, _current_entry);
            return _current_entry;
        }


        private UserContextScopeEntry _current_entry;
        public ISecurityUser CurrentUser => _current_entry?.User;

        private void Pop(UserContextScopeEntry entry)
        {
            if (_current_entry != entry)
                throw new InvalidOperationException("not current entry");


            _current_entry = entry.PreviusEntry;
        }

        public void Dispose()
        {
            //TODO

            _current_entry = null;

        }

        private class UserContextScopeEntry : IDisposable
        {
            private readonly UserContextScope _scope;
            
            internal readonly UserContextScopeEntry PreviusEntry;

            private readonly Lazy<ISecurityUser> _user;
            internal ISecurityUser User => _user.Value;

            public UserContextScopeEntry(UserContextScope scope, Func<ISecurityUser> user_func, UserContextScopeEntry previus_stage)
            {
                if (user_func == null)
                    throw new ArgumentNullException(nameof(user_func));


                _scope = scope;
                _user = new Lazy<ISecurityUser>(user_func);

                PreviusEntry = previus_stage;
            }

            public void Dispose()
            {
                _scope.Pop(this);
            }
        }


    }
}