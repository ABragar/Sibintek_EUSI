using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Base.Service;
using Owin;

namespace Base.Hangfire
{
    public interface IHangFireService: IDisposable
    {
        void Run(string name_or_connection_string);
        void Register<TService>(int id,string cron)
            where TService : class,IHangFireClient;

        void Register<TService>(Expression<Action<TService>> expression, string cron) where TService : class ;
        void MapDashboard(IAppBuilder app, string path = "/Hangfire", string apppath = "/Dashboard");
        void Execute<TService>(int id) where TService : class, IHangFireClient;
    }
}