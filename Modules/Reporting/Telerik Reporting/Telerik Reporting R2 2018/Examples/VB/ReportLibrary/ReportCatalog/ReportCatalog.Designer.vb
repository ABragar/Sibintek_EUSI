
Imports Telerik.Reporting
Imports Telerik.Reporting.Data

Partial Class ReportCatalog
#Region "Component Designer generated code"
    ''' <summary>
    ''' Required method for telerik Reporting designer support - do not modify
    ''' the contents of this method with the code editor.
    ''' </summary>
    Private Sub InitializeComponent()
        Dim TableGroup1 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup2 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim NavigateToReportAction1 As Telerik.Reporting.NavigateToReportAction = New Telerik.Reporting.NavigateToReportAction()
        Dim TypeReportSource1 As Telerik.Reporting.TypeReportSource = New Telerik.Reporting.TypeReportSource()
        Dim NavigateToUrlAction1 As Telerik.Reporting.NavigateToUrlAction = New Telerik.Reporting.NavigateToUrlAction()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ReportCatalog))
        Dim NavigateToUrlAction2 As Telerik.Reporting.NavigateToUrlAction = New Telerik.Reporting.NavigateToUrlAction()
        Dim NavigateToUrlAction3 As Telerik.Reporting.NavigateToUrlAction = New Telerik.Reporting.NavigateToUrlAction()
        Dim StyleRule1 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule2 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule3 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule4 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule5 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Me.detail = New Telerik.Reporting.DetailSection()
        Me.crosstab1 = New Telerik.Reporting.Crosstab()
        Me.panel5 = New Telerik.Reporting.Panel()
        Me.textBox1 = New Telerik.Reporting.TextBox()
        Me.textBox2 = New Telerik.Reporting.TextBox()
        Me.textBox3 = New Telerik.Reporting.TextBox()
        Me.objectDataSource1 = New Telerik.Reporting.ObjectDataSource()
        Me.pageFooterSection1 = New Telerik.Reporting.PageFooterSection()
        Me.textBox6 = New Telerik.Reporting.TextBox()
        Me.textBox7 = New Telerik.Reporting.TextBox()
        Me.textBox5 = New Telerik.Reporting.TextBox()
        Me.shape1 = New Telerik.Reporting.Shape()
        Me.pageHeaderSection1 = New Telerik.Reporting.PageHeaderSection()
        Me.textBoxReportEmployee = New Telerik.Reporting.TextBox()
        Me.shape3 = New Telerik.Reporting.Shape()
        Me.textBox4 = New Telerik.Reporting.TextBox()
        Me.textBox8 = New Telerik.Reporting.TextBox()
        Me.textBox9 = New Telerik.Reporting.TextBox()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'detail
        '
        Me.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(5.334200382232666R)
        Me.detail.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.crosstab1})
        Me.detail.Name = "detail"
        '
        'crosstab1
        '
        Me.crosstab1.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(6.380000114440918R)))
        Me.crosstab1.Body.Rows.Add(New Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(4.1777081489562988R)))
        Me.crosstab1.Body.SetCellContent(0, 0, Me.panel5)
        TableGroup1.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.Index%3"))
        TableGroup1.Name = "ColumnIndex"
        Me.crosstab1.ColumnGroups.Add(TableGroup1)
        Me.crosstab1.DataSource = Me.objectDataSource1
        Me.crosstab1.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.panel5})
        Me.crosstab1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.5027083158493042R), Telerik.Reporting.Drawing.Unit.Cm(0.25400015711784363R))
        Me.crosstab1.Name = "crosstab1"
        TableGroup2.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.Index/3"))
        TableGroup2.Name = "RowIndex"
        Me.crosstab1.RowGroups.Add(TableGroup2)
        Me.crosstab1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.380000114440918R), Telerik.Reporting.Drawing.Unit.Cm(4.1777081489562988R))
        Me.crosstab1.Sortings.Add(New Telerik.Reporting.Sorting("=Fields.Index", Telerik.Reporting.SortDirection.Asc))
        '
        'panel5
        '
        Me.panel5.Bindings.Add(New Telerik.Reporting.Binding("Visible", "=IIF(Fields.Name is null,False,True)"))
        Me.panel5.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.textBox1, Me.textBox2, Me.textBox3})
        Me.panel5.Name = "panel5"
        Me.panel5.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.380000114440918R), Telerik.Reporting.Drawing.Unit.Cm(4.1777081489562988R))
        '
        'textBox1
        '
        Me.textBox1.Anchoring = CType(((Telerik.Reporting.AnchoringStyles.Top Or Telerik.Reporting.AnchoringStyles.Left) _
            Or Telerik.Reporting.AnchoringStyles.Right), Telerik.Reporting.AnchoringStyles)
        Me.textBox1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.0R), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666R))
        Me.textBox1.Name = "textBox1"
        Me.textBox1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.1051244735717773R), Telerik.Reporting.Drawing.Unit.Cm(1.2700996398925781R))
        Me.textBox1.Style.Color = System.Drawing.Color.FromArgb(CType(CType(175, Byte), Integer), CType(CType(43, Byte), Integer), CType(CType(10, Byte), Integer))
        Me.textBox1.StyleName = "ExampleHeader"
        Me.textBox1.Value = "=Fields.Name"
        '
        'textBox2
        '
        Me.textBox2.CanGrow = True
        Me.textBox2.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.0R), Telerik.Reporting.Drawing.Unit.Cm(1.2701997756958008R))
        Me.textBox2.Name = "textBox2"
        Me.textBox2.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8472914695739746R), Telerik.Reporting.Drawing.Unit.Inch(0.60000008344650269R))
        Me.textBox2.StyleName = "ExampleDescription"
        Me.textBox2.Value = "=Fields.Description"
        '
        'textBox3
        '
        TypeReportSource1.TypeName = "=Fields.AssemblyQualifiedName"
        NavigateToReportAction1.ReportSource = TypeReportSource1
        Me.textBox3.Action = NavigateToReportAction1
        Me.textBox3.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.0R), Telerik.Reporting.Drawing.Unit.Cm(2.7941999435424805R))
        Me.textBox3.Name = "textBox3"
        Me.textBox3.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.5999999046325684R), Telerik.Reporting.Drawing.Unit.Inch(0.29440927505493164R))
        Me.textBox3.StyleName = "Hyperlink"
        Me.textBox3.Value = "View Report"
        '
        'objectDataSource1
        '
        Me.objectDataSource1.DataMember = "GetReports"
        Me.objectDataSource1.DataSource = GetType(ReportManager)
        Me.objectDataSource1.Name = "objectDataSource1"
        '
        'pageFooterSection1
        '
        NavigateToUrlAction1.Url = "http://www.telerik.com/purchase"
        Me.pageFooterSection1.Action = NavigateToUrlAction1
        Me.pageFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(6.857999324798584R)
        Me.pageFooterSection1.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.textBox6, Me.textBox7, Me.textBox5, Me.shape1})
        Me.pageFooterSection1.Name = "pageFooterSection1"
        '
        'textBox6
        '
        Me.textBox6.Action = NavigateToUrlAction1
        Me.textBox6.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.5027083158493042R), Telerik.Reporting.Drawing.Unit.Cm(0.6785462498664856R))
        Me.textBox6.Name = "textBox6"
        Me.textBox6.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.1051244735717773R), Telerik.Reporting.Drawing.Unit.Cm(0.9999995231628418R))
        Me.textBox6.Style.Color = System.Drawing.Color.FromArgb(CType(CType(175, Byte), Integer), CType(CType(43, Byte), Integer), CType(CType(10, Byte), Integer))
        Me.textBox6.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom
        Me.textBox6.StyleName = "ExampleHeader"
        Me.textBox6.Value = "Get Greater Value!"
        '
        'textBox7
        '
        Me.textBox7.Action = NavigateToUrlAction1
        Me.textBox7.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(6.6080331802368164R), Telerik.Reporting.Drawing.Unit.Cm(0.6785462498664856R))
        Me.textBox7.Name = "textBox7"
        Me.textBox7.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(9.13996696472168R), Telerik.Reporting.Drawing.Unit.Cm(0.9999995231628418R))
        Me.textBox7.Style.Font.Italic = True
        Me.textBox7.Style.Font.Name = "Segoe UI"
        Me.textBox7.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10.0R)
        Me.textBox7.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom
        Me.textBox7.Value = "Telerik Reporting is also available as part of " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Telerik DevCraft Complete with 14 other products"
        '
        'textBox5
        '
        Me.textBox5.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.4200000762939453R), Telerik.Reporting.Drawing.Unit.Inch(0.299999862909317R))
        Me.textBox5.Name = "textBox5"
        Me.textBox5.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.3699995279312134R), Telerik.Reporting.Drawing.Unit.Inch(0.3608449399471283R))
        Me.textBox5.Style.BackgroundImage.ImageData = CType(resources.GetObject("textBox5.Style.BackgroundImage.ImageData"), System.Drawing.Image)
        Me.textBox5.Style.BackgroundImage.MimeType = "image/png"
        Me.textBox5.Style.BackgroundImage.Repeat = Telerik.Reporting.Drawing.BackgroundRepeat.NoRepeat
        Me.textBox5.Style.Color = System.Drawing.Color.White
        Me.textBox5.Style.Font.Name = "Segoe UI"
        Me.textBox5.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(14.0R)
        Me.textBox5.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.textBox5.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.textBox5.Value = "Read more"
        '
        'shape1
        '
        Me.shape1.Anchoring = CType((Telerik.Reporting.AnchoringStyles.Left Or Telerik.Reporting.AnchoringStyles.Right), Telerik.Reporting.AnchoringStyles)
        Me.shape1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.1979166716337204R), Telerik.Reporting.Drawing.Unit.Inch(0.10000006109476089R))
        Me.shape1.Name = "shape1"
        Me.shape1.ShapeType = New Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW)
        Me.shape1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(7.46999979019165R), Telerik.Reporting.Drawing.Unit.Point(3.75R))
        Me.shape1.Stretch = True
        Me.shape1.Style.BorderColor.Default = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(54, Byte), Integer), CType(CType(33, Byte), Integer))
        Me.shape1.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.shape1.Style.BorderWidth.Bottom = Telerik.Reporting.Drawing.Unit.Point(0.5R)
        Me.shape1.Style.BorderWidth.Top = Telerik.Reporting.Drawing.Unit.Point(1.5R)
        Me.shape1.Style.Color = System.Drawing.Color.Transparent
        '
        'pageHeaderSection1
        '
        Me.pageHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(2.5398001670837402R)
        Me.pageHeaderSection1.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.textBoxReportEmployee, Me.shape3, Me.textBox4, Me.textBox8, Me.textBox9})
        Me.pageHeaderSection1.Name = "pageHeaderSection1"
        '
        'textBoxReportEmployee
        '
        Me.textBoxReportEmployee.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985R), Telerik.Reporting.Drawing.Unit.Inch(0.41500845551490784R))
        Me.textBoxReportEmployee.Name = "textBoxReportEmployee"
        Me.textBoxReportEmployee.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(5.0R), Telerik.Reporting.Drawing.Unit.Inch(0.40000009536743164R))
        Me.textBoxReportEmployee.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Top
        Me.textBoxReportEmployee.StyleName = "Header"
        Me.textBoxReportEmployee.Value = "Reporting Demos"
        '
        'shape3
        '
        Me.shape3.Anchoring = CType((Telerik.Reporting.AnchoringStyles.Left Or Telerik.Reporting.AnchoringStyles.Right), Telerik.Reporting.AnchoringStyles)
        Me.shape3.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224R), Telerik.Reporting.Drawing.Unit.Inch(0.8399999737739563R))
        Me.shape3.Name = "shape3"
        Me.shape3.ShapeType = New Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW)
        Me.shape3.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(7.46999979019165R), Telerik.Reporting.Drawing.Unit.Point(2.0R))
        Me.shape3.Stretch = True
        Me.shape3.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.shape3.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid
        Me.shape3.Style.BorderWidth.Bottom = Telerik.Reporting.Drawing.Unit.Point(0.5R)
        Me.shape3.Style.BorderWidth.Top = Telerik.Reporting.Drawing.Unit.Point(1.5R)
        Me.shape3.Style.Color = System.Drawing.Color.Transparent
        '
        'textBox4
        '
        NavigateToUrlAction2.Url = "http://www.telerik.com/support/reporting"
        Me.textBox4.Action = NavigateToUrlAction2
        Me.textBox4.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(17.120002746582031R), Telerik.Reporting.Drawing.Unit.Cm(0.63555306196212769R))
        Me.textBox4.Name = "textBox4"
        Me.textBox4.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.3617973327636719R), Telerik.Reporting.Drawing.Unit.Cm(0.41836854815483093R))
        Me.textBox4.Style.BackgroundColor = System.Drawing.Color.Transparent
        Me.textBox4.Style.Color = System.Drawing.Color.FromArgb(CType(CType(175, Byte), Integer), CType(CType(43, Byte), Integer), CType(CType(10, Byte), Integer))
        Me.textBox4.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.textBox4.StyleName = "Hyperlink"
        Me.textBox4.Value = "SUPPORT »"
        '
        'textBox8
        '
        NavigateToUrlAction3.Url = "http://tv.telerik.com/products/reporting"
        Me.textBox8.Action = NavigateToUrlAction3
        Me.textBox8.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(17.120002746582031R), Telerik.Reporting.Drawing.Unit.Cm(1.0541214942932129R))
        Me.textBox8.Name = "textBox8"
        Me.textBox8.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.3617961406707764R), Telerik.Reporting.Drawing.Unit.Cm(0.41999998688697815R))
        Me.textBox8.Style.BackgroundColor = System.Drawing.Color.Transparent
        Me.textBox8.Style.Color = System.Drawing.Color.FromArgb(CType(CType(175, Byte), Integer), CType(CType(43, Byte), Integer), CType(CType(10, Byte), Integer))
        Me.textBox8.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.textBox8.StyleName = "Hyperlink"
        Me.textBox8.Value = "VIDEOS »"
        '
        'textBox9
        '
        Me.textBox9.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985R), Telerik.Reporting.Drawing.Unit.Inch(0.015008449554443359R))
        Me.textBox9.Name = "textBox9"
        Me.textBox9.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(5.0R), Telerik.Reporting.Drawing.Unit.Inch(0.39992132782936096R))
        Me.textBox9.StyleName = "Header"
        Me.textBox9.Value = "Telerik"
        '
        'ReportCatalog
        '
        Me.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.detail, Me.pageFooterSection1, Me.pageHeaderSection1})
        Me.Name = "ReportCatalog"
        Me.PageSettings.Margins = New Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224R), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224R), Telerik.Reporting.Drawing.Unit.Inch(0.30000001192092896R), Telerik.Reporting.Drawing.Unit.Inch(0.40000000596046448R))
        Me.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4
        StyleRule1.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.TextItemBase)), New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.HtmlTextBox))})
        StyleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        StyleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        StyleRule2.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.StyleSelector("Header")})
        StyleRule2.Style.Font.Bold = True
        StyleRule2.Style.Font.Name = "Segoe UI Light"
        StyleRule2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(25.0R)
        StyleRule2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        StyleRule2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left
        StyleRule3.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.StyleSelector("Hyperlink")})
        StyleRule3.Style.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(147, Byte), Integer), CType(CType(30, Byte), Integer))
        StyleRule3.Style.Color = System.Drawing.Color.White
        StyleRule3.Style.Font.Name = "Segoe UI"
        StyleRule3.Style.Font.Underline = False
        StyleRule3.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        StyleRule3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        StyleRule4.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.StyleSelector("ExampleHeader")})
        StyleRule4.Style.Color = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(89, Byte), Integer), CType(CType(61, Byte), Integer))
        StyleRule4.Style.Font.Bold = True
        StyleRule4.Style.Font.Name = "Segoe UI Light"
        StyleRule4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(14.0R)
        StyleRule4.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        StyleRule5.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.StyleSelector("ExampleDescription")})
        StyleRule5.Style.Font.Name = "Segoe UI"
        StyleRule5.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.StyleSheet.AddRange(New Telerik.Reporting.Drawing.StyleRule() {StyleRule1, StyleRule2, StyleRule3, StyleRule4, StyleRule5})
        Me.Width = Telerik.Reporting.Drawing.Unit.Cm(19.983798980712891R)
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
#End Region

    Private detail As Telerik.Reporting.DetailSection
    Private objectDataSource1 As ObjectDataSource
    Private pageFooterSection1 As PageFooterSection
    Private textBox1 As TextBox
    Private pageHeaderSection1 As PageHeaderSection
    Private textBox8 As TextBox
    Private textBox4 As TextBox
    Private textBox6 As TextBox
    Private textBox7 As TextBox
    Private crosstab1 As Crosstab
    Private panel5 As Panel
    Private textBoxReportEmployee As TextBox
    Private shape3 As Shape
    Private textBox5 As TextBox
    Private shape1 As Shape
    Private textBox9 As TextBox
    Private textBox2 As TextBox
    Private textBox3 As TextBox
End Class