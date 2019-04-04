using System;
using System.Threading.Tasks;
using Base.Service.Log;
using Microsoft.Owin;

namespace WebUI
{
    public class OwinExceptionHandler : OwinMiddleware
    {
        private readonly ILogService _logger;
        public OwinExceptionHandler(OwinMiddleware next, ILogService logger) : base(next)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _logger = logger;
        }

        public override async Task Invoke(IOwinContext context)
        {
            try
            {
                await Next.Invoke(context);
            }
            catch (Exception e)
            {
                string strErr = "";
                if (context != null && context.Request != null && context.Request.PathBase != null)
                {
                    strErr = context.Request.PathBase.ToString();
                }

                strErr = (!string.IsNullOrEmpty(strErr)) ? string.Format("{0}: {1}", e.Message, strErr) : e.Message;

                _logger.Log(e, strErr);
            }

        }
    }
}







