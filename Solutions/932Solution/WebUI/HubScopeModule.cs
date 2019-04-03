using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Base.Service;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace WebUI
{
    public class HubScopeModule : IHubPipelineModule
    {
        private readonly IExecutionContextScopeManager _scope_manager;
        

        public HubScopeModule(IExecutionContextScopeManager scope_manager)
        {
            _scope_manager = scope_manager;
            
        }

        public Func<IHubIncomingInvokerContext, Task<object>> BuildIncoming(
            Func<IHubIncomingInvokerContext, Task<object>> invoke)
        {
            return async context =>
            {
                using (_scope_manager.BeginScope())
                {
                    return await invoke(context);
                }
                
                
            };
        }

        public Func<IHubOutgoingInvokerContext, Task> BuildOutgoing(Func<IHubOutgoingInvokerContext, Task> send)
        {
            return send;
        }


        public Func<IHub, Task> BuildConnect(Func<IHub, Task> connect)
        {
            return async hub =>
            {
                using (_scope_manager.BeginScope())
                {
                    await connect(hub);
                }
            };
        }


        public Func<IHub, Task> BuildReconnect(Func<IHub, Task> reconnect)
        {
            return async hub =>
            {
                using (_scope_manager.BeginScope())
                {
                    await reconnect(hub);
                }
            };
        }


        public Func<IHub, bool, Task> BuildDisconnect(Func<IHub, bool, Task> disconnect)
        {
            return async (hub,x) =>
            {
                using (_scope_manager.BeginScope())
                {
                    await disconnect(hub, x);
                }
            };
        }


        public Func<HubDescriptor, IRequest, bool> BuildAuthorizeConnect(Func<HubDescriptor, IRequest, bool> authorizeConnect)
        {
            return authorizeConnect;
            //return (x,request) =>
            //{
            //    return authorizeConnect(x, request);

            //};
        }


        public Func<HubDescriptor, IRequest, IList<string>, IList<string>> BuildRejoiningGroups(
            Func<HubDescriptor, IRequest, IList<string>, IList<string>> rejoiningGroups)
        {

            return rejoiningGroups;
            //return (x,reguest,group) =>
            //{
            //    return rejoiningGroups(x, reguest, group);

            //};
        }
    }
}