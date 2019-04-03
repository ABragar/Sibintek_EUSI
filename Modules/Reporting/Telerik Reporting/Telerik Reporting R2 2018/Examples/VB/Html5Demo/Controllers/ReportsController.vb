Imports Telerik.Reporting.Cache.Interfaces
Imports Telerik.Reporting.Services.Engine
Imports Telerik.Reporting.Services.WebApi
Imports System.IO

Public Class ReportsController
    Inherits ReportsControllerBase

    Shared configurationInstance As Telerik.Reporting.Services.ReportServiceConfiguration

    Shared Sub New()
        Dim appPath = HttpContext.Current.Server.MapPath("~/")
        Dim reportsPath = Path.Combine(appPath, "..\..\..\Report Designer\Examples")

        Dim resolver = New ReportFileResolver(reportsPath) _
                       .AddFallbackResolver(New ReportTypeResolver())

        Dim reportServiceConfiguration As New Telerik.Reporting.Services.ReportServiceConfiguration()
        reportServiceConfiguration.HostAppId = "Html5DemoApp"
        reportServiceConfiguration.ReportResolver = resolver
        reportServiceConfiguration.Storage = New Telerik.Reporting.Cache.File.FileStorage()
        ' reportServiceConfiguration.ReportSharingTimeout = 0
        ' reportServiceConfiguration.ClientSessionTimeout = 15
        configurationInstance = reportServiceConfiguration
    End Sub

    Public Sub New()
        Me.ReportServiceConfiguration = configurationInstance
    End Sub
End Class
