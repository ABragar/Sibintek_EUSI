Imports System.Web.SessionState
Imports Telerik.Reporting.Services.WebApi
Imports System.Web.Http

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ReportsControllerConfiguration.RegisterRoutes(GlobalConfiguration.Configuration)
    End Sub

End Class