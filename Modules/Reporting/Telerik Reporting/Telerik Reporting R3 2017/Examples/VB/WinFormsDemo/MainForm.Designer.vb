Imports Telerik.ReportViewer.WinForms

Partial Class MainForm
    ''' <summary>
    ''' Required designer variable.
    ''' </summary>
    Private components As System.ComponentModel.IContainer = Nothing
    Private reportViewer1 As Telerik.ReportViewer.WinForms.ReportViewer

    ''' <summary>
    ''' Clean up any resources being used.
    ''' </summary>
    ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso (components IsNot Nothing) Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

#Region "Windows Form Designer generated code"

    ''' <summary>
    ''' Required method for Designer support - do not modify
    ''' the contents of this method with the code editor.
    ''' </summary>
    Private Sub InitializeComponent()
        Dim TypeReportSource1 As Telerik.Reporting.TypeReportSource = New Telerik.Reporting.TypeReportSource()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.reportViewer1 = New Telerik.ReportViewer.WinForms.ReportViewer()
        Me.SuspendLayout()
        '
        'reportViewer1
        '
        Me.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.reportViewer1.Location = New System.Drawing.Point(0, 0)
        Me.reportViewer1.Name = "reportViewer1"
        TypeReportSource1.TypeName = "ReportCatalog, VB.ReportLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken" & _
    "=null"
        Me.reportViewer1.ReportSource = TypeReportSource1
        Me.reportViewer1.Size = New System.Drawing.Size(992, 573)
        Me.reportViewer1.TabIndex = 0
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(992, 573)
        Me.Controls.Add(Me.reportViewer1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "MainForm"
        Me.Text = "Telerik Reporting VB Demo"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)

    End Sub


#End Region
End Class
