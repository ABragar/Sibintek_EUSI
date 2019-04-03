Imports System.ComponentModel
Imports Telerik.Reporting

<Description("A collection of Product-related reports")>
Public Class ReportBook
    Inherits Telerik.Reporting.ReportBook
    Public Sub New()
        AddTocTemplate()

        AddReports()
    End Sub

    Private Sub AddTocTemplate()
        Dim TocReportSource As TypeReportSource = New TypeReportSource()
        TocReportSource.TypeName = GetType(ReportBookToc).AssemblyQualifiedName
        Me.TocReportSource = TocReportSource
    End Sub

    Private Sub AddReports()
        Me.ReportSources.Add(New TypeReportSource() With {
                .TypeName = GetType(SalesByRegionDashboardPart).AssemblyQualifiedName
            })

        Me.ReportSources.Add(New TypeReportSource() With {
                .TypeName = GetType(DashboardPart).AssemblyQualifiedName
            })

        Me.ReportSources.Add(New TypeReportSource() With {
                .TypeName = GetType(ProductSalesPart).AssemblyQualifiedName
            })

        Me.ReportSources.Add(New TypeReportSource() With {
                .TypeName = GetType(ProductCatalog).AssemblyQualifiedName
            })

        Me.ReportSources.Add(New TypeReportSource() With {
                .TypeName = GetType(ProductLineSalesPart).AssemblyQualifiedName
            })
    End Sub

    <Browsable(False)>
    Private Class SalesByRegionDashboardPart
        Inherits SalesByRegionDashboard
        Public Sub New()
            Me.DocumentMapText = "Sales By Region"
        End Sub
    End Class

    <Browsable(False)>
    Private Class DashboardPart
        Inherits Dashboard
        Public Sub New()
            Me.DocumentMapText = "Dashboard"
        End Sub
    End Class

    <Browsable(False)>
    Private Class ProductSalesPart
        Inherits ProductSales
        Public Sub New()
            Me.DocumentMapText = "Product Sales"
        End Sub
    End Class

    <Browsable(False)>
    Private Class ProductLineSalesPart
        Inherits ProductLineSales
        Public Sub New()
            Me.DocumentMapText = "Product Line Sales"
        End Sub
    End Class
End Class
