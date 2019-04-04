using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Kendo.Mvc.Extensions;

namespace WebUI.Areas.Account
{
    public static class Extensions
    {
        public static RouteValueDictionary FromCurrent(this UrlHelper helper, object routeValues = null)
        {
            return helper.RequestContext.GetRouteValueFromCurrent(routeValues);
        }



        public static string GetReturnUrl(this UrlHelper helper)
        {
            var url = helper.RequestContext.HttpContext.Request.QueryString["ReturnUrl"];

            return url != null && helper.IsLocalUrl(url) ? url : "/";
        }



        public static string AbsoluteActionUrl(this UrlHelper url, string actionName, string controllerName, object routeValues)
        {
            string scheme = url.RequestContext.HttpContext.Request.Url.Scheme;

            return url.Action(actionName, controllerName, routeValues, scheme);
        }



        public static TResult FromRoot<TModel, TResult>(this TModel model, TResult result)
        {
            return result;
        }


        public static RouteValueDictionary GetRouteValueFromCurrent(this RequestContext context, object routeValues = null)
        {



            var values = new RouteValueDictionary(context.HttpContext.Request.RequestContext.RouteData.Values);

            var query = context.HttpContext.Request.QueryString;

            values.AddRange(Enumerable.Range(0, query.Count).Select(i => new KeyValuePair<string, object>(query.GetKey(i), query[i])));


            if (routeValues != null)
            {
                foreach (var pair in new RouteValueDictionary(routeValues))
                {
                    if (pair.Value == null)
                        values.Remove(pair.Key);
                    else
                        values[pair.Key] = pair.Value;
                }
            }
            return values;
        }

    }
}