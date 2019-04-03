
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports Telerik.Reporting
Imports Telerik.Reporting.Drawing

Partial Class SalesOrderDetails
#Region "Component Designer generated code"
    ''' <summary>
    ''' Required method for telerik Reporting designer support - do not modify
    ''' the contents of this method with the code editor.
    ''' </summary>
    Private Sub InitializeComponent()
        Dim SortingAction1 As Telerik.Reporting.SortingAction = New Telerik.Reporting.SortingAction()
        Dim SortingAction2 As Telerik.Reporting.SortingAction = New Telerik.Reporting.SortingAction()
        Dim SortingAction3 As Telerik.Reporting.SortingAction = New Telerik.Reporting.SortingAction()
        Dim SortingAction4 As Telerik.Reporting.SortingAction = New Telerik.Reporting.SortingAction()
        Dim SortingAction5 As Telerik.Reporting.SortingAction = New Telerik.Reporting.SortingAction()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SalesOrderDetails))
        Dim Group1 As Telerik.Reporting.Group = New Telerik.Reporting.Group()
        Dim ReportParameter1 As Telerik.Reporting.ReportParameter = New Telerik.Reporting.ReportParameter()
        Dim StyleRule1 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule2 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule3 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule4 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule5 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Me.labelsGroupFooter = New Telerik.Reporting.GroupFooterSection()
        Me.textBox1 = New Telerik.Reporting.TextBox()
        Me.textBox2 = New Telerik.Reporting.TextBox()
        Me.labelsGroupHeader = New Telerik.Reporting.GroupHeaderSection()
        Me.panel1 = New Telerik.Reporting.Panel()
        Me.productNumberCaptionTextBox = New Telerik.Reporting.TextBox()
        Me.nameCaptionTextBox = New Telerik.Reporting.TextBox()
        Me.orderQtyCaptionTextBox = New Telerik.Reporting.TextBox()
        Me.unitPriceCaptionTextBox = New Telerik.Reporting.TextBox()
        Me.lineTotalCaptionTextBox = New Telerik.Reporting.TextBox()
        Me.detail = New Telerik.Reporting.DetailSection()
        Me.productNumberDataTextBox = New Telerik.Reporting.TextBox()
        Me.nameDataTextBox = New Telerik.Reporting.TextBox()
        Me.orderQtyDataTextBox = New Telerik.Reporting.TextBox()
        Me.unitPriceDataTextBox = New Telerik.Reporting.TextBox()
        Me.lineTotalDataTextBox = New Telerik.Reporting.TextBox()
        Me.sqlDataSource1 = New Telerik.Reporting.SqlDataSource()
        Me.reportHeaderSection1 = New Telerik.Reporting.ReportHeaderSection()
        Me.textBox3 = New Telerik.Reporting.TextBox()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'labelsGroupFooter
        '
        Me.labelsGroupFooter.Height = Telerik.Reporting.Drawing.Unit.Inch(0.47999998927116394R)
        Me.labelsGroupFooter.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.textBox1, Me.textBox2})
        Me.labelsGroupFooter.Name = "labelsGroupFooter"
        '
        'textBox1
        '
        Me.textBox1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.000039418537198798731R), Telerik.Reporting.Drawing.Unit.Inch(0.0R))
        Me.textBox1.Name = "textBox1"
        Me.textBox1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(6.1228375434875488R), Telerik.Reporting.Drawing.Unit.Inch(0.47996059060096741R))
        Me.textBox1.StyleName = "Total"
        Me.textBox1.Value = "Total"
        '
        'textBox2
        '
        Me.textBox2.Format = "{0:C2}"
        Me.textBox2.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.1229557991027832R), Telerik.Reporting.Drawing.Unit.Inch(0.0R))
        Me.textBox2.Name = "textBox2"
        Me.textBox2.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.9669651985168457R), Telerik.Reporting.Drawing.Unit.Inch(0.47996059060096741R))
        Me.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.textBox2.StyleName = "Total"
        Me.textBox2.Value = "=Sum(Fields.LineTotal)"
        '
        'labelsGroupHeader
        '
        Me.labelsGroupHeader.Height = Telerik.Reporting.Drawing.Unit.Inch(0.47999998927116394R)
        Me.labelsGroupHeader.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.panel1})
        Me.labelsGroupHeader.Name = "labelsGroupHeader"
        Me.labelsGroupHeader.PrintOnEveryPage = True
        '
        'panel1
        '
        Me.panel1.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.productNumberCaptionTextBox, Me.nameCaptionTextBox, Me.orderQtyCaptionTextBox, Me.unitPriceCaptionTextBox, Me.lineTotalCaptionTextBox})
        Me.panel1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.0R))
        Me.panel1.Name = "panel1"
        Me.panel1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.089961051940918R), Telerik.Reporting.Drawing.Unit.Inch(0.47999998927116394R))
        '
        'productNumberCaptionTextBox
        '
        SortingAction1.SortingExpression = "= Fields.ProductNumber"
        SortingAction1.Targets.AddRange(New Telerik.Reporting.IActionTarget() {Me})
        Me.productNumberCaptionTextBox.Action = SortingAction1
        Me.productNumberCaptionTextBox.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.000039378803194267675R))
        Me.productNumberCaptionTextBox.Name = "productNumberCaptionTextBox"
        Me.productNumberCaptionTextBox.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.4000000953674316R), Telerik.Reporting.Drawing.Unit.Inch(0.47988176345825195R))
        Me.productNumberCaptionTextBox.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None
        Me.productNumberCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left
        Me.productNumberCaptionTextBox.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.productNumberCaptionTextBox.StyleName = "Caption"
        Me.productNumberCaptionTextBox.Value = "Product No."
        '
        'nameCaptionTextBox
        '
        SortingAction2.SortingExpression = "= Fields.Name"
        SortingAction2.Targets.AddRange(New Telerik.Reporting.IActionTarget() {Me})
        Me.nameCaptionTextBox.Action = SortingAction2
        Me.nameCaptionTextBox.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.4000000953674316R), Telerik.Reporting.Drawing.Unit.Inch(0.0R))
        Me.nameCaptionTextBox.Name = "nameCaptionTextBox"
        Me.nameCaptionTextBox.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.4999213218688965R), Telerik.Reporting.Drawing.Unit.Inch(0.47992119193077087R))
        Me.nameCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left
        Me.nameCaptionTextBox.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.nameCaptionTextBox.StyleName = "Caption"
        Me.nameCaptionTextBox.Value = "Name"
        '
        'orderQtyCaptionTextBox
        '
        SortingAction3.SortingExpression = "= Fields.OrderQty"
        SortingAction3.Targets.AddRange(New Telerik.Reporting.IActionTarget() {Me})
        Me.orderQtyCaptionTextBox.Action = SortingAction3
        Me.orderQtyCaptionTextBox.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.9000000953674316R), Telerik.Reporting.Drawing.Unit.Inch(-0.000000012417634032146907R))
        Me.orderQtyCaptionTextBox.Name = "orderQtyCaptionTextBox"
        Me.orderQtyCaptionTextBox.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.9999997615814209R), Telerik.Reporting.Drawing.Unit.Inch(0.47992119193077087R))
        Me.orderQtyCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left
        Me.orderQtyCaptionTextBox.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.orderQtyCaptionTextBox.StyleName = "Caption"
        Me.orderQtyCaptionTextBox.Value = "Quantity"
        '
        'unitPriceCaptionTextBox
        '
        SortingAction4.SortingExpression = "= Fields.UnitPrice"
        SortingAction4.Targets.AddRange(New Telerik.Reporting.IActionTarget() {Me})
        Me.unitPriceCaptionTextBox.Action = SortingAction4
        Me.unitPriceCaptionTextBox.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.9000000953674316R), Telerik.Reporting.Drawing.Unit.Inch(0.0R))
        Me.unitPriceCaptionTextBox.Name = "unitPriceCaptionTextBox"
        Me.unitPriceCaptionTextBox.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.0999211072921753R), Telerik.Reporting.Drawing.Unit.Inch(0.47992119193077087R))
        Me.unitPriceCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left
        Me.unitPriceCaptionTextBox.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.unitPriceCaptionTextBox.StyleName = "Caption"
        Me.unitPriceCaptionTextBox.Value = "Unit Price"
        '
        'lineTotalCaptionTextBox
        '
        SortingAction5.SortingExpression = "= Fields.LineTotal"
        SortingAction5.Targets.AddRange(New Telerik.Reporting.IActionTarget() {Me})
        Me.lineTotalCaptionTextBox.Action = SortingAction5
        Me.lineTotalCaptionTextBox.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.9999604225158691R), Telerik.Reporting.Drawing.Unit.Inch(0.0R))
        Me.lineTotalCaptionTextBox.Name = "lineTotalCaptionTextBox"
        Me.lineTotalCaptionTextBox.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.0899604558944702R), Telerik.Reporting.Drawing.Unit.Inch(0.47992119193077087R))
        Me.lineTotalCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left
        Me.lineTotalCaptionTextBox.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.lineTotalCaptionTextBox.StyleName = "Caption"
        Me.lineTotalCaptionTextBox.Value = "Line Total"
        '
        'detail
        '
        Me.detail.Height = Telerik.Reporting.Drawing.Unit.Inch(0.47999998927116394R)
        Me.detail.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.productNumberDataTextBox, Me.nameDataTextBox, Me.orderQtyDataTextBox, Me.unitPriceDataTextBox, Me.lineTotalDataTextBox})
        Me.detail.Name = "detail"
        '
        'productNumberDataTextBox
        '
        Me.productNumberDataTextBox.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.0R))
        Me.productNumberDataTextBox.Name = "productNumberDataTextBox"
        Me.productNumberDataTextBox.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.4000000953674316R), Telerik.Reporting.Drawing.Unit.Inch(0.47992119193077087R))
        Me.productNumberDataTextBox.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None
        Me.productNumberDataTextBox.StyleName = "Data"
        Me.productNumberDataTextBox.Value = "=Fields.ProductNumber"
        '
        'nameDataTextBox
        '
        Me.nameDataTextBox.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.4000000953674316R), Telerik.Reporting.Drawing.Unit.Inch(0.0R))
        Me.nameDataTextBox.Name = "nameDataTextBox"
        Me.nameDataTextBox.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.4999206066131592R), Telerik.Reporting.Drawing.Unit.Inch(0.47992119193077087R))
        Me.nameDataTextBox.StyleName = "Data"
        Me.nameDataTextBox.Value = "=Fields.Name"
        '
        'orderQtyDataTextBox
        '
        Me.orderQtyDataTextBox.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.9000401496887207R), Telerik.Reporting.Drawing.Unit.Inch(0.0R))
        Me.orderQtyDataTextBox.Name = "orderQtyDataTextBox"
        Me.orderQtyDataTextBox.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.99996018409729R), Telerik.Reporting.Drawing.Unit.Inch(0.47992119193077087R))
        Me.orderQtyDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.orderQtyDataTextBox.StyleName = "Data"
        Me.orderQtyDataTextBox.Value = "=Fields.OrderQty"
        '
        'unitPriceDataTextBox
        '
        Me.unitPriceDataTextBox.Format = "{0:C2}"
        Me.unitPriceDataTextBox.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.9000000953674316R), Telerik.Reporting.Drawing.Unit.Inch(0.0R))
        Me.unitPriceDataTextBox.Name = "unitPriceDataTextBox"
        Me.unitPriceDataTextBox.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.0999212265014648R), Telerik.Reporting.Drawing.Unit.Inch(0.47992119193077087R))
        Me.unitPriceDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.unitPriceDataTextBox.StyleName = "Data"
        Me.unitPriceDataTextBox.Value = "=Fields.UnitPrice"
        '
        'lineTotalDataTextBox
        '
        Me.lineTotalDataTextBox.Format = "{0:C2}"
        Me.lineTotalDataTextBox.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(7.0R), Telerik.Reporting.Drawing.Unit.Inch(0.0R))
        Me.lineTotalDataTextBox.Name = "lineTotalDataTextBox"
        Me.lineTotalDataTextBox.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.0899215936660767R), Telerik.Reporting.Drawing.Unit.Inch(0.47992119193077087R))
        Me.lineTotalDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.lineTotalDataTextBox.StyleName = "Data"
        Me.lineTotalDataTextBox.Value = "=Fields.LineTotal"
        '
        'sqlDataSource1
        '
        Me.sqlDataSource1.ConnectionString = "My.MySettings.TelerikConnectionString"
        Me.sqlDataSource1.Name = "sqlDataSource1"
        Me.sqlDataSource1.Parameters.AddRange(New Telerik.Reporting.SqlDataSourceParameter() {New Telerik.Reporting.SqlDataSourceParameter("@SalesOrderID", System.Data.DbType.Int32, "=Parameters.SaledOrderID.Value")})
        Me.sqlDataSource1.SelectCommand = resources.GetString("sqlDataSource1.SelectCommand")
        '
        'reportHeaderSection1
        '
        Me.reportHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.5R)
        Me.reportHeaderSection1.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.textBox3})
        Me.reportHeaderSection1.Name = "reportHeaderSection1"
        '
        'textBox3
        '
        Me.textBox3.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.000039339065551757813R))
        Me.textBox3.Name = "textBox3"
        Me.textBox3.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.089961051940918R), Telerik.Reporting.Drawing.Unit.Inch(0.49996066093444824R))
        Me.textBox3.StyleName = "Title"
        Me.textBox3.Value = "Order Details"
        '
        'SalesOrderDetails
        '
        Me.DataSource = Me.sqlDataSource1
        Me.Filters.Add(New Telerik.Reporting.Filter("=Fields.SalesOrderID", Telerik.Reporting.FilterOperator.Equal, "=Parameters.SaledOrderID.Value"))
        Group1.GroupFooter = Me.labelsGroupFooter
        Group1.GroupHeader = Me.labelsGroupHeader
        Group1.Name = "labelsGroup"
        Me.Groups.AddRange(New Telerik.Reporting.Group() {Group1})
        Me.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.labelsGroupHeader, Me.labelsGroupFooter, Me.detail, Me.reportHeaderSection1})
        Me.Name = "SalesOrderDetails"
        Me.PageSettings.Landscape = False
        Me.PageSettings.Margins = New Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.0R))
        Me.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Letter
        ReportParameter1.Name = "SaledOrderID"
        ReportParameter1.Value = "=43860"
        Me.ReportParameters.Add(ReportParameter1)
        Me.Style.BackgroundColor = System.Drawing.Color.White
        StyleRule1.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.TextItemBase)), New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.HtmlTextBox))})
        StyleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        StyleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        StyleRule2.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.StyleSelector("Title")})
        StyleRule2.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        StyleRule2.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Point(0.5R)
        StyleRule2.Style.Font.Bold = True
        StyleRule2.Style.Font.Name = "Segoe UI Light"
        StyleRule2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(16.0R)
        StyleRule2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        StyleRule2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        StyleRule3.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.StyleSelector("Caption")})
        StyleRule3.Style.BorderColor.Default = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer))
        StyleRule3.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None
        StyleRule3.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid
        StyleRule3.Style.Color = System.Drawing.Color.FromArgb(CType(CType(177, Byte), Integer), CType(CType(161, Byte), Integer), CType(CType(82, Byte), Integer))
        StyleRule3.Style.Font.Name = "Segoe UI"
        StyleRule3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10.0R)
        StyleRule3.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Inch(0.079999998211860657R)
        StyleRule3.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.10000000149011612R)
        StyleRule3.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Inch(0.10000000149011612R)
        StyleRule3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom
        StyleRule4.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.StyleSelector("Data")})
        StyleRule4.Style.BorderColor.Default = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer))
        StyleRule4.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None
        StyleRule4.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid
        StyleRule4.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid
        StyleRule4.Style.Font.Name = "Segoe UI"
        StyleRule4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10.0R)
        StyleRule4.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Inch(0.079999998211860657R)
        StyleRule4.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.10000000149011612R)
        StyleRule4.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Inch(0.10000000149011612R)
        StyleRule4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom
        StyleRule5.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.StyleSelector("Total")})
        StyleRule5.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        StyleRule5.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None
        StyleRule5.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid
        StyleRule5.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Point(0.5R)
        StyleRule5.Style.Font.Bold = True
        StyleRule5.Style.Font.Name = "Segoe UI"
        StyleRule5.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10.0R)
        StyleRule5.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Inch(0.079999998211860657R)
        StyleRule5.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.10000000149011612R)
        StyleRule5.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Inch(0.10000000149011612R)
        StyleRule5.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        StyleRule5.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom
        Me.StyleSheet.AddRange(New Telerik.Reporting.Drawing.StyleRule() {StyleRule1, StyleRule2, StyleRule3, StyleRule4, StyleRule5})
        Me.Width = Telerik.Reporting.Drawing.Unit.Inch(8.09000015258789R)
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
#End Region

    Private labelsGroupHeader As GroupHeaderSection
    Private productNumberCaptionTextBox As Telerik.Reporting.TextBox
    Private nameCaptionTextBox As Telerik.Reporting.TextBox
    Private orderQtyCaptionTextBox As Telerik.Reporting.TextBox
    Private unitPriceCaptionTextBox As Telerik.Reporting.TextBox
    Private lineTotalCaptionTextBox As Telerik.Reporting.TextBox
    Private labelsGroupFooter As GroupFooterSection
    Private detail As DetailSection
    Private productNumberDataTextBox As Telerik.Reporting.TextBox
    Private nameDataTextBox As Telerik.Reporting.TextBox
    Private orderQtyDataTextBox As Telerik.Reporting.TextBox
    Private unitPriceDataTextBox As Telerik.Reporting.TextBox
    Private lineTotalDataTextBox As Telerik.Reporting.TextBox
    Private panel1 As Telerik.Reporting.Panel
    Private textBox1 As Telerik.Reporting.TextBox
    Private textBox2 As Telerik.Reporting.TextBox
    Private sqlDataSource1 As SqlDataSource
    Private reportHeaderSection1 As ReportHeaderSection
    Private textBox3 As Telerik.Reporting.TextBox

End Class