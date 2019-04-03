using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Base.Service;
using Hangfire;
using Hangfire.Common;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using Hangfire.States;
using Owin;
using AppContext = Base.Ambient.AppContext;

namespace Base.Hangfire
{

    public class HangFireService : IHangFireService
    {
        private readonly IServiceLocator _locator;

        private class Activator : JobActivator
        {
            private readonly IServiceLocator _locator;
            private readonly IExecutionContextScopeManager _scope_manager;
            public Activator(IServiceLocator locator)
            {
                if (locator == null)
                    throw new ArgumentNullException(nameof(locator));

                _locator = locator;

                _scope_manager = _locator.GetService<IExecutionContextScopeManager>();
            }

            public override object ActivateJob(Type jobType)
            {
                return _locator.GetService(jobType);
            }

            public override JobActivatorScope BeginScope()
            {
                return new ExecutionContextScope(_locator, _scope_manager.BeginScope());
            }

            private class ExecutionContextScope : JobActivatorScope
            {
                private readonly IServiceLocator _locator;
                private readonly IDisposable _scope;

                public ExecutionContextScope(IServiceLocator locator,IDisposable scope)
                {
                    _locator = locator;
                    _scope = scope;
                }

                public override void DisposeScope()
                {
                    _scope.Dispose();
                }

                public override object Resolve(Type type)
                {
                    return _locator.GetService(type);
                }
            }

        }


        private BackgroundJobServer _job_server;
        private RecurringJobManager _manager;
        private BackgroundJobClient _backgroundJobClient;
        private SqlServerStorage _storage;

        public HangFireService(IServiceLocator locator)
        {
            if (locator == null)
                throw new ArgumentNullException(nameof(locator));

            _locator = locator;
        }

        public void Run(string name_or_connection_string)
        {

            _storage = new SqlServerStorage(name_or_connection_string);
            _manager = new RecurringJobManager(_storage);
            _backgroundJobClient = new BackgroundJobClient(_storage);
            _job_server = new BackgroundJobServer(new BackgroundJobServerOptions
            {
                Activator = new Activator(_locator)
            }, _storage);
        }

        public void Dispose()
        {

            if (_job_server != null)
            {
                _job_server.Dispose();
                _job_server = null;

            }
        }


        void CheckServiceRegistration<T>()
            where T : class
        {
            var factory = _locator.GetService<IServiceFactory<T>>();
        }

        public void Execute<TService>(int id) where TService : class, IHangFireClient
        {
            CheckServiceRegistration<TService>();

            Expression<Action<TService>> expr = x => x.Process(id);

            var job = Job.FromExpression(expr);

            _backgroundJobClient.Create(job, new EnqueuedState());
        }

        public void Register<TService>(int id, string cron) where TService : class, IHangFireClient
        {

            Expression<Action<TService>> expr = x => x.Process(id);

            var job = Job.FromExpression(expr);


            var name = job + "." + id;

            Register<TService>(name,job,cron);
        }

        public void Register<TService>(Expression<Action<TService>> expression, string cron)
            where TService : class
        {

            var job = Job.FromExpression(expression);
            Register<TService>(job.ToString(), job, cron);
        }

        private void Register<TService>(string name, Job job, string cron)
            where TService : class
        {
            if (cron != null)
            {
                CheckServiceRegistration<TService>();

                _manager.AddOrUpdate(name, job, cron);
            }
            else
            {
                _manager.RemoveIfExists(name);
            }
        }

        public void MapDashboard(IAppBuilder app, string path, string apppath)
        {
            if (_storage == null)
                throw new InvalidOperationException("Hangfire service must be initialized before mapping dashboard.");

            app.UseHangfireDashboard(path, new DashboardOptions()
            {
                AuthorizationFilters = new[] { new Filter() },
                AppPath = apppath
            }, _storage);
        }

        private class Filter : IAuthorizationFilter
        {
            public bool Authorize(IDictionary<string, object> owinEnvironment)
            {

                return AppContext.SecurityUser?.IsAdmin == true;
            }
        }

    }
}