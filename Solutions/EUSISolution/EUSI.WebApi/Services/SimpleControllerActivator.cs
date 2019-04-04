using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Base.Service;

namespace EUSI.WebApi.Services
{
    internal class SimpleControllerActivator : IHttpControllerActivator
    {
        private readonly IServiceLocator _locator;

        public SimpleControllerActivator(IServiceLocator locator)
        {
            _locator = locator;
        }

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {

            if (!typeof(IHttpController).IsAssignableFrom(controllerType))
                throw new InvalidOperationException();


            var controller = (IHttpController)_locator.GetService(controllerType);
            
            return controller;
        }
    }
}