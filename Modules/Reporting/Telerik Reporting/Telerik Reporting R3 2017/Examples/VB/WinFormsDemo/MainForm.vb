Imports System.Windows.Forms
Imports Telerik.Reporting

Partial Public Class MainForm
    Inherits Form
    Public Sub New(ByVal report As IReportDocument)
        InitializeComponent()
    End Sub

    Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.reportViewer1.RefreshReport()
    End Sub
End Class
