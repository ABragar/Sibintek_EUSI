Partial Class ReportBookToc

    'NOTE: The following procedure is required by the telerik Reporting Designer
    'It can be modified using the telerik Reporting Designer.  
    'Do not modify it using the code editor.
    Private Sub InitializeComponent()
        Dim TocLevel1 As Telerik.Reporting.TocLevel = New Telerik.Reporting.TocLevel()
        Dim TocLevel2 As Telerik.Reporting.TocLevel = New Telerik.Reporting.TocLevel()
        Dim TocLevel3 As Telerik.Reporting.TocLevel = New Telerik.Reporting.TocLevel()
        Dim StyleRule1 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule2 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Me.detailSection1 = New Telerik.Reporting.DetailSection()
        Me.tocSection1 = New Telerik.Reporting.TocSection()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'detailSection1
        '
        Me.detailSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.30000004172325134R)
        Me.detailSection1.Name = "detailSection1"
        Me.detailSection1.Style.Visible = False
        '
        'tocSection1
        '
        Me.tocSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(1.8000000715255737R)
        Me.tocSection1.KeepTogether = False
        TocLevel1.LeaderSymbol = Global.Microsoft.VisualBasic.ChrW(0)
        TocLevel1.Name = "Level1"
        TocLevel1.Style.Font.Bold = True
        TocLevel1.Style.Font.Name = "Segoe UI Light"
        TocLevel1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(16.0R)
        TocLevel1.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Point(5.0R)
        TocLevel1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        TocLevel1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        TocLevel1.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Point(5.0R)
        TocLevel2.Name = "Level2"
        TocLevel2.Style.Font.Name = "Segoe UI Light"
        TocLevel2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(13.0R)
        TocLevel2.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Point(1.0R)
        TocLevel2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(32.0R)
        TocLevel2.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        TocLevel2.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Point(1.0R)
        TocLevel3.Name = "Level3"
        TocLevel3.Style.Font.Name = "Segoe UI Light"
        TocLevel3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12.0R)
        TocLevel3.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Point(1.0R)
        TocLevel3.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(52.0R)
        TocLevel3.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        TocLevel3.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Point(1.0R)
        Me.tocSection1.Levels.Add(TocLevel1)
        Me.tocSection1.Levels.Add(TocLevel2)
        Me.tocSection1.Levels.Add(TocLevel3)
        Me.tocSection1.Name = "tocSection1"
        Me.tocSection1.Title = "Report Book Table of Contents"
        Me.tocSection1.TitleStyle.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.tocSection1.TitleStyle.BorderWidth.Bottom = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        Me.tocSection1.TitleStyle.Font.Bold = True
        Me.tocSection1.TitleStyle.Font.Name = "Segoe UI Light"
        Me.tocSection1.TitleStyle.Font.Size = Telerik.Reporting.Drawing.Unit.Point(25.0R)
        Me.tocSection1.TitleStyle.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Point(10.0R)
        '
        'ReportBookToc
        '
        Me.DocumentMapText = "Table of Contents"
        Me.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.detailSection1, Me.tocSection1})
        Me.Name = "ReportBookToc"
        Me.PageSettings.ColumnCount = 1
        Me.PageSettings.ColumnSpacing = Telerik.Reporting.Drawing.Unit.Inch(0R)
        Me.PageSettings.Landscape = False
        Me.PageSettings.Margins = New Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224R), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224R), Telerik.Reporting.Drawing.Unit.Inch(0.30000001192092896R), Telerik.Reporting.Drawing.Unit.Inch(0.40000000596046448R))
        Me.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Letter
        StyleRule1.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.TextItemBase)), New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.HtmlTextBox))})
        StyleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        StyleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        StyleRule2.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.TextItemBase)), New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.HtmlTextBox))})
        StyleRule2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        StyleRule2.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        Me.StyleSheet.AddRange(New Telerik.Reporting.Drawing.StyleRule() {StyleRule1, StyleRule2})
        Me.Width = Telerik.Reporting.Drawing.Unit.Inch(8.0R)
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub

    Friend WithEvents detailSection1 As Telerik.Reporting.DetailSection
    Friend WithEvents tocSection1 As Telerik.Reporting.TocSection
End Class