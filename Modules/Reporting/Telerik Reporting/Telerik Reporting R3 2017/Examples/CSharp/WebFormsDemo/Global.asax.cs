using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using Telerik.Reporting.Services.WebApi;

namespace CSharp.WebFormsDemo
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            Telerik.Reporting.Services.WebApi.ReportsControllerConfiguration.RegisterRoutes(GlobalConfiguration.Configuration);
        }
    }
}