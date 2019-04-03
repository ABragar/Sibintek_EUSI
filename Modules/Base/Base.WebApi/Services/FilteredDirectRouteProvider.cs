using System;
using System.Collections.Generic;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace Base.WebApi.Services
{
    public class FilteredDirectRouteProvider : DefaultDirectRouteProvider
    {
        private readonly Func<HttpControllerDescriptor, bool> _filter;

        public FilteredDirectRouteProvider(Func<HttpControllerDescriptor, bool> filter)
        {
            _filter = filter;
        }

        private static readonly IReadOnlyList<RouteEntry> _empty = new RouteEntry[0];

        public override IReadOnlyList<RouteEntry> GetDirectRoutes(HttpControllerDescriptor controllerDescriptor,
            IReadOnlyList<HttpActionDescriptor> actionDescriptors,
            IInlineConstraintResolver constraintResolver)
        {

            
            if (_filter(controllerDescriptor))
                return base.GetDirectRoutes(controllerDescriptor, actionDescriptors, constraintResolver);

            return _empty;

        }


    }
}