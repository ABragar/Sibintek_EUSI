Imports System.ComponentModel
Imports Telerik.Reporting

''' <summary>
''' A report for printing business cards.
''' </summary>
<Description("Printable product tags arranged in a newspaper-style columns")> _
Partial Public Class ProductTagReport
    Inherits Report
    Public Sub New()
        ''' <summary>
        ''' Required for Telerik Reporting designer support
        ''' </summary>
        InitializeComponent()
    End Sub
End Class
