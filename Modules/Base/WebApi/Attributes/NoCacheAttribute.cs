using System;
using System.Web;
using System.Web.Mvc;

namespace WebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class NoCacheAttribute : ActionFilterAttribute//, IResultFilter
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
            filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            filterContext.HttpContext.Response.Cache.SetNoStore();

            base.OnResultExecuting(filterContext);
        }
        //public override void OnResultExecuting(ResultExecutingContext filterContext)
        //{
        //}

        //public override void OnResultExecuted(ResultExecutedContext filterContext)
        //{
        //    var cache = filterContext.HttpContext.Response.Cache;
        //    cache.SetCacheability(HttpCacheability.NoCache);
        //    cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
        //    cache.SetExpires(DateTime.Now.AddYears(-5));
        //    cache.AppendCacheExtension("private");
        //    cache.AppendCacheExtension("no-cache=Set-Cookie");
        //    cache.SetProxyMaxAge(TimeSpan.Zero);
        //}
    }
}
