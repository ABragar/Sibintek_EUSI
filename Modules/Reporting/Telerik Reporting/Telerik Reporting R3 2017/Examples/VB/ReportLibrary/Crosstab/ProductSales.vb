Imports System
Imports System.ComponentModel

''' <summary>
''' Summary description for ProductSales.
''' </summary>

<Description("Crosstab summary of AdventureWorks sales, grouped by product category over a 4 year period. This is about 120 000 rows of data.")> _
Partial Public Class ProductSales
    Inherits Telerik.Reporting.Report
    Public Sub New()
        ''' <summary>
        ''' Required for telerik Reporting designer support
        ''' </summary>

        '
        ' TODO: Add any constructor code after InitializeComponent call
        '
        InitializeComponent()
    End Sub
End Class
