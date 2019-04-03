Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports Telerik.Reporting
Imports Telerik.Reporting.Drawing

Partial Class EmployeeSalesSummary
#Region "Component Designer generated code"
    ''' <summary>
    ''' Required method for telerik Reporting designer support - do not modify
    ''' the contents of this method with the code editor.
    ''' </summary>
    Private Sub InitializeComponent()
        Dim NavigateToReportAction1 As Telerik.Reporting.NavigateToReportAction = New Telerik.Reporting.NavigateToReportAction()
        Dim TypeReportSource1 As Telerik.Reporting.TypeReportSource = New Telerik.Reporting.TypeReportSource()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(EmployeeSalesSummary))
        Dim GraphGroup1 As Telerik.Reporting.GraphGroup = New Telerik.Reporting.GraphGroup()
        Dim ColorPalette1 As Telerik.Reporting.Drawing.ColorPalette = New Telerik.Reporting.Drawing.ColorPalette()
        Dim GraphTitle1 As Telerik.Reporting.GraphTitle = New Telerik.Reporting.GraphTitle()
        Dim CategoryScale1 As Telerik.Reporting.CategoryScale = New Telerik.Reporting.CategoryScale()
        Dim NumericalScale1 As Telerik.Reporting.NumericalScale = New Telerik.Reporting.NumericalScale()
        Dim GraphGroup2 As Telerik.Reporting.GraphGroup = New Telerik.Reporting.GraphGroup()
        Dim GraphGroup3 As Telerik.Reporting.GraphGroup = New Telerik.Reporting.GraphGroup()
        Dim ColorPalette2 As Telerik.Reporting.Drawing.ColorPalette = New Telerik.Reporting.Drawing.ColorPalette()
        Dim GraphTitle2 As Telerik.Reporting.GraphTitle = New Telerik.Reporting.GraphTitle()
        Dim CategoryScale2 As Telerik.Reporting.CategoryScale = New Telerik.Reporting.CategoryScale()
        Dim NumericalScale2 As Telerik.Reporting.NumericalScale = New Telerik.Reporting.NumericalScale()
        Dim ColorPalette3 As Telerik.Reporting.Drawing.ColorPalette = New Telerik.Reporting.Drawing.ColorPalette()
        Dim GraphGroup4 As Telerik.Reporting.GraphGroup = New Telerik.Reporting.GraphGroup()
        Dim TableGroup1 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup2 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup3 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup4 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup5 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup6 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup7 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup8 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup9 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup10 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup11 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup12 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim Group1 As Telerik.Reporting.Group = New Telerik.Reporting.Group()
        Dim ReportParameter1 As Telerik.Reporting.ReportParameter = New Telerik.Reporting.ReportParameter()
        Dim ReportParameter2 As Telerik.Reporting.ReportParameter = New Telerik.Reporting.ReportParameter()
        Dim StyleRule1 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule2 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule3 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule4 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Me.textBoxTableTitle = New Telerik.Reporting.TextBox()
        Me.textBoxCategoryAmount = New Telerik.Reporting.TextBox()
        Me.textBoxCategory = New Telerik.Reporting.TextBox()
        Me.textBoxSalesOrderNumber = New Telerik.Reporting.TextBox()
        Me.textBoxOrderTotal = New Telerik.Reporting.TextBox()
        Me.textBox4 = New Telerik.Reporting.TextBox()
        Me.textBox12 = New Telerik.Reporting.TextBox()
        Me.textBoxEmployeeSalesTotal = New Telerik.Reporting.TextBox()
        Me.textBoxTotalLabel = New Telerik.Reporting.TextBox()
        Me.textBox10 = New Telerik.Reporting.TextBox()
        Me.groupFooterSection1 = New Telerik.Reporting.GroupFooterSection()
        Me.groupHeaderSection1 = New Telerik.Reporting.GroupHeaderSection()
        Me.textBoxLogo = New Telerik.Reporting.TextBox()
        Me.textBoxDataRange = New Telerik.Reporting.TextBox()
        Me.textBoxReportEmployee = New Telerik.Reporting.TextBox()
        Me.shape3 = New Telerik.Reporting.Shape()
        Me.Employees = New Telerik.Reporting.SqlDataSource()
        Me.detail = New Telerik.Reporting.DetailSection()
        Me.panelWrapper = New Telerik.Reporting.Panel()
        Me.graph1 = New Telerik.Reporting.Graph()
        Me.cartesianCoordinateSystem1 = New Telerik.Reporting.CartesianCoordinateSystem()
        Me.graphAxis2 = New Telerik.Reporting.GraphAxis()
        Me.graphAxis1 = New Telerik.Reporting.GraphAxis()
        Me.mainDataSource = New Telerik.Reporting.SqlDataSource()
        Me.lineSeries1 = New Telerik.Reporting.AreaSeries()
        Me.graph2 = New Telerik.Reporting.Graph()
        Me.cartesianCoordinateSystem3 = New Telerik.Reporting.CartesianCoordinateSystem()
        Me.graphAxis6 = New Telerik.Reporting.GraphAxis()
        Me.graphAxis5 = New Telerik.Reporting.GraphAxis()
        Me.barSeries1 = New Telerik.Reporting.BarSeries()
        Me.crosstabEmployeeSales = New Telerik.Reporting.Crosstab()
        Me.textBox6 = New Telerik.Reporting.TextBox()
        Me.textBox5 = New Telerik.Reporting.TextBox()
        Me.textBox9 = New Telerik.Reporting.TextBox()
        Me.textBox13 = New Telerik.Reporting.TextBox()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'textBoxTableTitle
        '
        Me.textBoxTableTitle.Name = "textBoxTableTitle"
        Me.textBoxTableTitle.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.0799999237060547R), Telerik.Reporting.Drawing.Unit.Inch(0.47999998927116394R))
        Me.textBoxTableTitle.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBoxTableTitle.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(16.0R)
        Me.textBoxTableTitle.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Inch(0.079999998211860657R)
        Me.textBoxTableTitle.Value = "{Parameters.ReportDate.Value.ToString(""MMMM yyyy"")} Order Summary"
        '
        'textBoxCategoryAmount
        '
        Me.textBoxCategoryAmount.Format = "{0:N2}"
        Me.textBoxCategoryAmount.Name = "textBoxCategoryAmount"
        Me.textBoxCategoryAmount.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.75R), Telerik.Reporting.Drawing.Unit.Inch(0.23999999463558197R))
        Me.textBoxCategoryAmount.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.textBoxCategoryAmount.Value = "=Sum(Fields.LineTotal)"
        '
        'textBoxCategory
        '
        Me.textBoxCategory.Name = "textBoxCategory"
        Me.textBoxCategory.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(5.130000114440918R), Telerik.Reporting.Drawing.Unit.Inch(0.23999999463558197R))
        Me.textBoxCategory.Value = "=Fields.Category"
        '
        'textBoxSalesOrderNumber
        '
        TypeReportSource1.Parameters.Add(New Telerik.Reporting.Parameter("OrderNumber", "=Fields.SalesOrderNumber"))
        TypeReportSource1.Parameters.Add(New Telerik.Reporting.Parameter("ForYear", "=Parameters.ReportDate.Value.Year"))
        TypeReportSource1.Parameters.Add(New Telerik.Reporting.Parameter("ForMonth", "=Parameters.ReportDate.Value.Month"))
        TypeReportSource1.TypeName = "Invoice, VB.ReportLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
        NavigateToReportAction1.ReportSource = TypeReportSource1
        Me.textBoxSalesOrderNumber.Action = NavigateToReportAction1
        Me.textBoxSalesOrderNumber.Name = "textBoxSalesOrderNumber"
        Me.textBoxSalesOrderNumber.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2100000381469727R), Telerik.Reporting.Drawing.Unit.Inch(0.23999999463558197R))
        Me.textBoxSalesOrderNumber.Style.Color = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(105, Byte), Integer), CType(CType(104, Byte), Integer))
        Me.textBoxSalesOrderNumber.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Inch(0.05000000074505806R)
        Me.textBoxSalesOrderNumber.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Top
        Me.textBoxSalesOrderNumber.Value = "=Fields.SalesOrderNumber"
        '
        'textBoxOrderTotal
        '
        Me.textBoxOrderTotal.Format = "{0:N2}"
        Me.textBoxOrderTotal.Name = "textBoxOrderTotal"
        Me.textBoxOrderTotal.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.75R), Telerik.Reporting.Drawing.Unit.Inch(0.23999999463558197R))
        Me.textBoxOrderTotal.Style.BorderColor.Default = System.Drawing.Color.Black
        Me.textBoxOrderTotal.Style.BorderColor.Top = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.textBoxOrderTotal.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBoxOrderTotal.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBoxOrderTotal.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.textBoxOrderTotal.Value = "=Sum(Fields.LineTotal)"
        '
        'textBox4
        '
        Me.textBox4.Name = "textBox4"
        Me.textBox4.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(5.130000114440918R), Telerik.Reporting.Drawing.Unit.Inch(0.23999999463558197R))
        Me.textBox4.Style.BorderColor.Default = System.Drawing.Color.Black
        Me.textBox4.Style.BorderColor.Top = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.textBox4.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBox4.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid
        '
        'textBox12
        '
        Me.textBox12.Name = "textBox12"
        Me.textBox12.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2100000381469727R), Telerik.Reporting.Drawing.Unit.Inch(0.23999999463558197R))
        Me.textBox12.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        '
        'textBoxEmployeeSalesTotal
        '
        Me.textBoxEmployeeSalesTotal.Format = "{0:N2}"
        Me.textBoxEmployeeSalesTotal.Name = "textBoxEmployeeSalesTotal"
        Me.textBoxEmployeeSalesTotal.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.75R), Telerik.Reporting.Drawing.Unit.Inch(0.23999999463558197R))
        Me.textBoxEmployeeSalesTotal.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBoxEmployeeSalesTotal.Style.BorderWidth.Bottom = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        Me.textBoxEmployeeSalesTotal.Style.Font.Bold = True
        Me.textBoxEmployeeSalesTotal.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.textBoxEmployeeSalesTotal.Value = "=Sum(Fields.LineTotal)"
        '
        'textBoxTotalLabel
        '
        Me.textBoxTotalLabel.Name = "textBoxTotalLabel"
        Me.textBoxTotalLabel.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(5.130000114440918R), Telerik.Reporting.Drawing.Unit.Inch(0.23999999463558197R))
        Me.textBoxTotalLabel.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBoxTotalLabel.Style.BorderWidth.Bottom = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        Me.textBoxTotalLabel.Value = "Total:"
        '
        'textBox10
        '
        Me.textBox10.Name = "textBox10"
        Me.textBox10.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2100000381469727R), Telerik.Reporting.Drawing.Unit.Inch(0.23999999463558197R))
        Me.textBox10.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBox10.Style.BorderWidth.Bottom = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        '
        'groupFooterSection1
        '
        Me.groupFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.1272023469209671R)
        Me.groupFooterSection1.Name = "groupFooterSection1"
        Me.groupFooterSection1.Style.Visible = False
        '
        'groupHeaderSection1
        '
        Me.groupHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(1.0R)
        Me.groupHeaderSection1.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.textBoxLogo, Me.textBoxDataRange, Me.textBoxReportEmployee, Me.shape3})
        Me.groupHeaderSection1.Name = "groupHeaderSection1"
        Me.groupHeaderSection1.PrintOnEveryPage = True
        '
        'textBoxLogo
        '
        Me.textBoxLogo.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.000039339065551757813R))
        Me.textBoxLogo.Name = "textBoxLogo"
        Me.textBoxLogo.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1972219944000244R), Telerik.Reporting.Drawing.Unit.Inch(0.20000004768371582R))
        Me.textBoxLogo.Style.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(11, Byte), Integer))
        Me.textBoxLogo.Style.Font.Bold = True
        Me.textBoxLogo.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        Me.textBoxLogo.Value = "SALES REPORT"
        '
        'textBoxDataRange
        '
        Me.textBoxDataRange.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.6000003814697266R), Telerik.Reporting.Drawing.Unit.Inch(0.0R))
        Me.textBoxDataRange.Name = "textBoxDataRange"
        Me.textBoxDataRange.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.4999597072601318R), Telerik.Reporting.Drawing.Unit.Inch(0.19837446510791779R))
        Me.textBoxDataRange.Style.Font.Bold = True
        Me.textBoxDataRange.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        Me.textBoxDataRange.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.textBoxDataRange.Value = "=Parameters.ReportDate.Value.ToString('MMM yyyy')"
        '
        'textBoxReportEmployee
        '
        Me.textBoxReportEmployee.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.20011806488037109R))
        Me.textBoxReportEmployee.Name = "textBoxReportEmployee"
        Me.textBoxReportEmployee.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.0999603271484375R), Telerik.Reporting.Drawing.Unit.Inch(0.60000002384185791R))
        Me.textBoxReportEmployee.StyleName = "Header"
        Me.textBoxReportEmployee.Value = "=Parameters.Employee.Label"
        '
        'shape3
        '
        Me.shape3.Anchoring = CType((Telerik.Reporting.AnchoringStyles.Left Or Telerik.Reporting.AnchoringStyles.Right), Telerik.Reporting.AnchoringStyles)
        Me.shape3.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.80019682645797729R))
        Me.shape3.Name = "shape3"
        Me.shape3.ShapeType = New Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW)
        Me.shape3.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.0999603271484375R), Telerik.Reporting.Drawing.Unit.Point(2.0R))
        Me.shape3.Stretch = True
        Me.shape3.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.shape3.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid
        Me.shape3.Style.BorderWidth.Bottom = Telerik.Reporting.Drawing.Unit.Point(0.5R)
        Me.shape3.Style.BorderWidth.Top = Telerik.Reporting.Drawing.Unit.Point(1.5R)
        Me.shape3.Style.Color = System.Drawing.Color.Transparent
        '
        'Employees
        '
        Me.Employees.ConnectionString = "My.MySettings.TelerikConnectionString"
        Me.Employees.Name = "Employees"
        Me.Employees.SelectCommand = resources.GetString("Employees.SelectCommand")
        '
        'detail
        '
        Me.detail.Height = Telerik.Reporting.Drawing.Unit.Inch(6.8000006675720215R)
        Me.detail.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.panelWrapper})
        Me.detail.KeepTogether = False
        Me.detail.Name = "detail"
        '
        'panelWrapper
        '
        Me.panelWrapper.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.graph1, Me.graph2, Me.crosstabEmployeeSales})
        Me.panelWrapper.KeepTogether = False
        Me.panelWrapper.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.000039378803194267675R), Telerik.Reporting.Drawing.Unit.Inch(0.000078678131103515625R))
        Me.panelWrapper.Name = "panelWrapper"
        Me.panelWrapper.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.1000394821167R), Telerik.Reporting.Drawing.Unit.Inch(6.79992151260376R))
        '
        'graph1
        '
        GraphGroup1.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.OrderDate.Month"))
        GraphGroup1.Label = "= Fields.OrderDate.ToString(""MMMM"")"
        GraphGroup1.Name = "orderDate.MonthGroup1"
        GraphGroup1.Sortings.Add(New Telerik.Reporting.Sorting("=Fields.OrderDate.Month", Telerik.Reporting.SortDirection.Asc))
        Me.graph1.CategoryGroups.Add(GraphGroup1)
        ColorPalette1.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(150, Byte), Integer), CType(CType(70, Byte), Integer)))
        ColorPalette1.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(75, Byte), Integer), CType(CType(172, Byte), Integer), CType(CType(198, Byte), Integer)))
        ColorPalette1.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(162, Byte), Integer)))
        ColorPalette1.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(182, Byte), Integer), CType(CType(87, Byte), Integer), CType(CType(8, Byte), Integer)))
        ColorPalette1.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(39, Byte), Integer), CType(CType(106, Byte), Integer), CType(CType(124, Byte), Integer)))
        ColorPalette1.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(59, Byte), Integer), CType(CType(98, Byte), Integer)))
        Me.graph1.ColorPalette = ColorPalette1
        Me.graph1.CoordinateSystems.Add(Me.cartesianCoordinateSystem1)
        Me.graph1.DataSource = Me.mainDataSource
        Me.graph1.Legend.Position = Telerik.Reporting.GraphItemPosition.RightCenter
        Me.graph1.Legend.Style.LineColor = System.Drawing.Color.LightGray
        Me.graph1.Legend.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.graph1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.000039458274841308594R), Telerik.Reporting.Drawing.Unit.Inch(0.0R))
        Me.graph1.Name = "graph1"
        Me.graph1.PlotAreaStyle.LineColor = System.Drawing.Color.LightGray
        Me.graph1.PlotAreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.graph1.Series.Add(Me.lineSeries1)
        Me.graph1.SeriesGroups.Add(GraphGroup2)
        Me.graph1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.1000003814697266R), Telerik.Reporting.Drawing.Unit.Inch(2.4000000953674316R))
        Me.graph1.Style.Font.Name = "Segoe UI"
        Me.graph1.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        Me.graph1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        Me.graph1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        Me.graph1.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        GraphTitle1.Position = Telerik.Reporting.GraphItemPosition.TopCenter
        GraphTitle1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(14.0R)
        GraphTitle1.Style.LineColor = System.Drawing.Color.LightGray
        GraphTitle1.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        GraphTitle1.Text = "Yearly Sales Comparison"
        Me.graph1.Titles.Add(GraphTitle1)
        '
        'cartesianCoordinateSystem1
        '
        Me.cartesianCoordinateSystem1.Name = "cartesianCoordinateSystem1"
        Me.cartesianCoordinateSystem1.XAxis = Me.graphAxis2
        Me.cartesianCoordinateSystem1.YAxis = Me.graphAxis1
        '
        'graphAxis2
        '
        Me.graphAxis2.LabelAngle = -90
        Me.graphAxis2.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis2.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis2.MajorGridLineStyle.Visible = True
        Me.graphAxis2.MajorTickMarkDisplayType = Telerik.Reporting.GraphAxisTickMarkDisplayType.None
        Me.graphAxis2.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis2.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis2.MinorGridLineStyle.Visible = False
        Me.graphAxis2.Name = "graphAxis2"
        Me.graphAxis2.Scale = CategoryScale1
        Me.graphAxis2.Style.Font.Name = "Segoe UI"
        Me.graphAxis2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8.0R)
        Me.graphAxis2.Style.Visible = True
        '
        'graphAxis1
        '
        Me.graphAxis1.LabelFormat = "{0:C0}K"
        Me.graphAxis1.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis1.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis1.MajorGridLineStyle.Visible = True
        Me.graphAxis1.MajorTickMarkDisplayType = Telerik.Reporting.GraphAxisTickMarkDisplayType.None
        Me.graphAxis1.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis1.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis1.MinorGridLineStyle.Visible = False
        Me.graphAxis1.Name = "graphAxis1"
        Me.graphAxis1.Scale = NumericalScale1
        Me.graphAxis1.Style.Font.Name = "Segoe UI"
        Me.graphAxis1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8.0R)
        Me.graphAxis1.Style.Visible = True
        '
        'mainDataSource
        '
        Me.mainDataSource.ConnectionString = "My.MySettings.TelerikConnectionString"
        Me.mainDataSource.Name = "mainDataSource"
        Me.mainDataSource.Parameters.AddRange(New Telerik.Reporting.SqlDataSourceParameter() {New Telerik.Reporting.SqlDataSourceParameter("@ReportYear", System.Data.DbType.[String], Nothing), New Telerik.Reporting.SqlDataSourceParameter("@ReportMonth", System.Data.DbType.[String], Nothing), New Telerik.Reporting.SqlDataSourceParameter("@EmployeeID", System.Data.DbType.Int32, "=Parameters.Employee.Value")})
        Me.mainDataSource.SelectCommand = resources.GetString("mainDataSource.SelectCommand")
        '
        'lineSeries1
        '
        Me.lineSeries1.AreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.lineSeries1.ArrangeMode = Telerik.Reporting.GraphSeriesArrangeMode.Stacked
        Me.lineSeries1.CategoryGroup = GraphGroup1
        Me.lineSeries1.CoordinateSystem = Me.cartesianCoordinateSystem1
        Me.lineSeries1.DataPointLabel = "=Sum(Fields.LineTotal)"
        Me.lineSeries1.DataPointLabelStyle.Visible = False
        Me.lineSeries1.DataPointStyle.Visible = False
        Me.lineSeries1.LegendItem.Value = "=Fields.OrderDate.Year"
        Me.lineSeries1.MarkerSize = Telerik.Reporting.Drawing.Unit.Mm(2.0R)
        Me.lineSeries1.Name = "lineSeries1"
        GraphGroup2.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.OrderDate.Year"))
        GraphGroup2.Name = "orderDate.YearGroup1"
        GraphGroup2.Sortings.Add(New Telerik.Reporting.Sorting("=Fields.OrderDate.Year", Telerik.Reporting.SortDirection.Asc))
        Me.lineSeries1.SeriesGroup = GraphGroup2
        Me.lineSeries1.Y = "=ISNULL(Sum(Fields.LineTotal), 0) / 1000.0"
        '
        'graph2
        '
        GraphGroup3.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.Category"))
        GraphGroup3.Name = "categoryGroup1"
        GraphGroup3.Sortings.Add(New Telerik.Reporting.Sorting("=Fields.Category", Telerik.Reporting.SortDirection.Asc))
        Me.graph2.CategoryGroups.Add(GraphGroup3)
        ColorPalette2.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(150, Byte), Integer), CType(CType(70, Byte), Integer)))
        ColorPalette2.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(75, Byte), Integer), CType(CType(172, Byte), Integer), CType(CType(198, Byte), Integer)))
        ColorPalette2.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(162, Byte), Integer)))
        ColorPalette2.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(182, Byte), Integer), CType(CType(87, Byte), Integer), CType(CType(8, Byte), Integer)))
        ColorPalette2.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(39, Byte), Integer), CType(CType(106, Byte), Integer), CType(CType(124, Byte), Integer)))
        ColorPalette2.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(59, Byte), Integer), CType(CType(98, Byte), Integer)))
        Me.graph2.ColorPalette = ColorPalette2
        Me.graph2.CoordinateSystems.Add(Me.cartesianCoordinateSystem3)
        Me.graph2.DataSource = Me.mainDataSource
        Me.graph2.Legend.IsInsidePlotArea = False
        Me.graph2.Legend.Position = Telerik.Reporting.GraphItemPosition.RightCenter
        Me.graph2.Legend.Style.Font.Name = "Segoe UI"
        Me.graph2.Legend.Style.LineColor = System.Drawing.Color.LightGray
        Me.graph2.Legend.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.graph2.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(2.5999214649200439R))
        Me.graph2.Name = "graph2"
        Me.graph2.PlotAreaStyle.LineColor = System.Drawing.Color.LightGray
        Me.graph2.PlotAreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.graph2.Series.Add(Me.barSeries1)
        Me.graph2.SeriesGroups.Add(GraphGroup4)
        Me.graph2.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.1000003814697266R), Telerik.Reporting.Drawing.Unit.Inch(2.4000000953674316R))
        Me.graph2.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        Me.graph2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        Me.graph2.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        Me.graph2.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        GraphTitle2.Position = Telerik.Reporting.GraphItemPosition.TopCenter
        GraphTitle2.Style.Font.Name = "Segoe UI"
        GraphTitle2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(14.0R)
        GraphTitle2.Style.LineColor = System.Drawing.Color.LightGray
        GraphTitle2.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        GraphTitle2.Text = "Category Sales Comparison"
        Me.graph2.Titles.Add(GraphTitle2)
        '
        'cartesianCoordinateSystem3
        '
        Me.cartesianCoordinateSystem3.Name = "cartesianCoordinateSystem3"
        Me.cartesianCoordinateSystem3.XAxis = Me.graphAxis6
        Me.cartesianCoordinateSystem3.YAxis = Me.graphAxis5
        '
        'graphAxis6
        '
        Me.graphAxis6.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis6.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis6.MajorGridLineStyle.Visible = False
        Me.graphAxis6.MajorTickMarkDisplayType = Telerik.Reporting.GraphAxisTickMarkDisplayType.None
        Me.graphAxis6.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis6.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis6.MinorGridLineStyle.Visible = False
        Me.graphAxis6.Name = "graphAxis6"
        Me.graphAxis6.Scale = CategoryScale2
        Me.graphAxis6.Style.Font.Name = "Segoe UI"
        Me.graphAxis6.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8.0R)
        '
        'graphAxis5
        '
        Me.graphAxis5.LabelFormat = "{0:C0}K"
        Me.graphAxis5.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis5.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis5.MajorTickMarkDisplayType = Telerik.Reporting.GraphAxisTickMarkDisplayType.None
        Me.graphAxis5.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis5.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis5.MinorGridLineStyle.Visible = False
        Me.graphAxis5.Name = "graphAxis5"
        Me.graphAxis5.Scale = NumericalScale2
        Me.graphAxis5.Style.Font.Name = "Segoe UI"
        Me.graphAxis5.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8.0R)
        '
        'barSeries1
        '
        Me.barSeries1.CategoryGroup = GraphGroup3
        ColorPalette3.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(150, Byte), Integer), CType(CType(70, Byte), Integer)))
        ColorPalette3.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(75, Byte), Integer), CType(CType(172, Byte), Integer), CType(CType(198, Byte), Integer)))
        ColorPalette3.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(162, Byte), Integer)))
        ColorPalette3.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(182, Byte), Integer), CType(CType(87, Byte), Integer), CType(CType(8, Byte), Integer)))
        ColorPalette3.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(39, Byte), Integer), CType(CType(106, Byte), Integer), CType(CType(124, Byte), Integer)))
        ColorPalette3.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(59, Byte), Integer), CType(CType(98, Byte), Integer)))
        Me.barSeries1.ColorPalette = ColorPalette3
        Me.barSeries1.CoordinateSystem = Me.cartesianCoordinateSystem3
        Me.barSeries1.DataPointLabel = "=Sum(Fields.LineTotal)"
        Me.barSeries1.DataPointLabelStyle.Visible = False
        Me.barSeries1.DataPointStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.barSeries1.DataPointStyle.Visible = True
        Me.barSeries1.LegendItem.Value = "=Fields.OrderDate.Year"
        GraphGroup4.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.OrderDate.Year"))
        GraphGroup4.Name = "orderDate.YearGroup2"
        GraphGroup4.Sortings.Add(New Telerik.Reporting.Sorting("=Fields.OrderDate.Year", Telerik.Reporting.SortDirection.Asc))
        Me.barSeries1.SeriesGroup = GraphGroup4
        Me.barSeries1.Y = "=Sum(Fields.LineTotal) / 1000.0"
        '
        'crosstabEmployeeSales
        '
        Me.crosstabEmployeeSales.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.0R)))
        Me.crosstabEmployeeSales.Body.Rows.Add(New Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Inch(0.48000001907348633R)))
        Me.crosstabEmployeeSales.Body.Rows.Add(New Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Inch(0.24000000953674316R)))
        Me.crosstabEmployeeSales.Body.Rows.Add(New Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Inch(0.24000000953674316R)))
        Me.crosstabEmployeeSales.Body.Rows.Add(New Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Inch(0.24000000953674316R)))
        Me.crosstabEmployeeSales.Body.SetCellContent(1, 0, Me.textBox6)
        Me.crosstabEmployeeSales.Body.SetCellContent(0, 0, Me.textBox5)
        Me.crosstabEmployeeSales.Body.SetCellContent(2, 0, Me.textBox9)
        Me.crosstabEmployeeSales.Body.SetCellContent(3, 0, Me.textBox13)
        Me.crosstabEmployeeSales.ColumnGroups.Add(TableGroup1)
        Me.crosstabEmployeeSales.DataSource = Me.mainDataSource
        Me.crosstabEmployeeSales.Filters.Add(New Telerik.Reporting.Filter("= Fields.OrderDate.Year", Telerik.Reporting.FilterOperator.Equal, "=Parameters.ReportDate.Value.Year"))
        Me.crosstabEmployeeSales.Filters.Add(New Telerik.Reporting.Filter("= Fields.OrderDate.Month", Telerik.Reporting.FilterOperator.Equal, "=Parameters.ReportDate.Value.Month"))
        Me.crosstabEmployeeSales.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.textBox6, Me.textBox5, Me.textBox9, Me.textBox13, Me.textBoxTableTitle, Me.textBoxSalesOrderNumber, Me.textBoxCategory, Me.textBoxCategoryAmount, Me.textBox12, Me.textBox4, Me.textBoxOrderTotal, Me.textBox10, Me.textBoxTotalLabel, Me.textBoxEmployeeSalesTotal})
        Me.crosstabEmployeeSales.KeepTogether = False
        Me.crosstabEmployeeSales.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(5.4999213218688965R))
        Me.crosstabEmployeeSales.Name = "crosstabEmployeeSales"
        Me.crosstabEmployeeSales.NoDataMessage = "= ""No Sales for "" + Parameters.ReportDate.Value.ToString(""MMMM, yyyy"")"
        Me.crosstabEmployeeSales.NoDataStyle.Color = System.Drawing.Color.Red
        TableGroup2.Name = "RowGroup"
        TableGroup2.ReportItem = Me.textBoxTableTitle
        TableGroup2.Sortings.Add(New Telerik.Reporting.Sorting("1", Telerik.Reporting.SortDirection.Asc))
        TableGroup6.Name = "Detail"
        TableGroup6.ReportItem = Me.textBoxCategoryAmount
        TableGroup5.ChildGroups.Add(TableGroup6)
        TableGroup5.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.Category"))
        TableGroup5.Name = "Category"
        TableGroup5.ReportItem = Me.textBoxCategory
        TableGroup5.Sortings.Add(New Telerik.Reporting.Sorting("=Fields.Category", Telerik.Reporting.SortDirection.Asc))
        TableGroup4.ChildGroups.Add(TableGroup5)
        TableGroup4.Name = "Group6"
        TableGroup4.ReportItem = Me.textBoxSalesOrderNumber
        TableGroup9.Name = "Group2"
        TableGroup9.ReportItem = Me.textBoxOrderTotal
        TableGroup8.ChildGroups.Add(TableGroup9)
        TableGroup8.Name = "Group1"
        TableGroup8.ReportItem = Me.textBox4
        TableGroup7.ChildGroups.Add(TableGroup8)
        TableGroup7.Name = "Group7"
        TableGroup7.ReportItem = Me.textBox12
        TableGroup3.ChildGroups.Add(TableGroup4)
        TableGroup3.ChildGroups.Add(TableGroup7)
        TableGroup3.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.SalesOrderID"))
        TableGroup3.Name = "SalesOrder"
        TableGroup3.Sortings.Add(New Telerik.Reporting.Sorting("=Fields.SalesOrderNumber", Telerik.Reporting.SortDirection.Asc))
        TableGroup12.Name = "Group4"
        TableGroup12.ReportItem = Me.textBoxEmployeeSalesTotal
        TableGroup11.ChildGroups.Add(TableGroup12)
        TableGroup11.Name = "Group5"
        TableGroup11.ReportItem = Me.textBoxTotalLabel
        TableGroup10.ChildGroups.Add(TableGroup11)
        TableGroup10.Name = "Group3"
        TableGroup10.ReportItem = Me.textBox10
        Me.crosstabEmployeeSales.RowGroups.Add(TableGroup2)
        Me.crosstabEmployeeSales.RowGroups.Add(TableGroup3)
        Me.crosstabEmployeeSales.RowGroups.Add(TableGroup10)
        Me.crosstabEmployeeSales.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.09000015258789R), Telerik.Reporting.Drawing.Unit.Inch(1.2000000476837158R))
        Me.crosstabEmployeeSales.Style.BorderColor.Default = System.Drawing.Color.FromArgb(CType(CType(137, Byte), Integer), CType(CType(145, Byte), Integer), CType(CType(164, Byte), Integer))
        '
        'textBox6
        '
        Me.textBox6.Name = "textBox6"
        Me.textBox6.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.23999999463558197R))
        '
        'textBox5
        '
        Me.textBox5.Name = "textBox5"
        Me.textBox5.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.47999998927116394R))
        Me.textBox5.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None
        Me.textBox5.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None
        '
        'textBox9
        '
        Me.textBox9.Name = "textBox9"
        Me.textBox9.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.23999999463558197R))
        '
        'textBox13
        '
        Me.textBox13.Name = "textBox13"
        Me.textBox13.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.23999999463558197R))
        '
        'EmployeeSalesSummary
        '
        Group1.GroupFooter = Me.groupFooterSection1
        Group1.GroupHeader = Me.groupHeaderSection1
        Group1.Groupings.Add(New Telerik.Reporting.Grouping(""))
        Group1.Name = "group1"
        Me.Groups.AddRange(New Telerik.Reporting.Group() {Group1})
        Me.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.groupHeaderSection1, Me.groupFooterSection1, Me.detail})
        Me.Name = "EmployeeSalesSummary"
        Me.PageSettings.Landscape = False
        Me.PageSettings.Margins = New Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224R), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224R), Telerik.Reporting.Drawing.Unit.Inch(0.30000001192092896R), Telerik.Reporting.Drawing.Unit.Inch(0.40000000596046448R))
        Me.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Letter
        ReportParameter1.AutoRefresh = True
        ReportParameter1.Name = "ReportDate"
        ReportParameter1.Text = "Report Date"
        ReportParameter1.Type = Telerik.Reporting.ReportParameterType.DateTime
        ReportParameter1.Value = "=#2003-07-01#"
        ReportParameter1.Visible = True
        ReportParameter2.AutoRefresh = True
        ReportParameter2.AvailableValues.DataSource = Me.Employees
        ReportParameter2.AvailableValues.DisplayMember = "= Fields.Employee"
        ReportParameter2.AvailableValues.ValueMember = "= Fields.SalesPersonID"
        ReportParameter2.Name = "Employee"
        ReportParameter2.Text = "Employee"
        ReportParameter2.Type = Telerik.Reporting.ReportParameterType.[Integer]
        ReportParameter2.Value = "=283"
        ReportParameter2.Visible = True
        Me.ReportParameters.Add(ReportParameter1)
        Me.ReportParameters.Add(ReportParameter2)
        StyleRule1.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.TextItemBase)), New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.HtmlTextBox))})
        StyleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        StyleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        StyleRule2.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.StyleSelector("Header")})
        StyleRule2.Style.Font.Bold = True
        StyleRule2.Style.Font.Name = "Segoe UI Light"
        StyleRule2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(25.0R)
        StyleRule2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        StyleRule2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Top
        StyleRule3.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.StyleSelector("Category")})
        StyleRule3.Style.Font.Name = "Segoe UI Light"
        StyleRule3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(16.0R)
        StyleRule3.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        StyleRule3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Top
        StyleRule4.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.TextBox))})
        StyleRule4.Style.Font.Name = "Segoe UI"
        StyleRule4.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        StyleRule4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom
        Me.StyleSheet.AddRange(New Telerik.Reporting.Drawing.StyleRule() {StyleRule1, StyleRule2, StyleRule3, StyleRule4})
        Me.Width = Telerik.Reporting.Drawing.Unit.Inch(8.09999942779541R)
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
#End Region

    Private detail As Telerik.Reporting.DetailSection
    Private panelWrapper As Telerik.Reporting.Panel
    Private groupFooterSection1 As GroupFooterSection
    Private groupHeaderSection1 As GroupHeaderSection
    Private textBoxLogo As Telerik.Reporting.TextBox
    Private textBoxDataRange As Telerik.Reporting.TextBox
    Private textBoxReportEmployee As Telerik.Reporting.TextBox
    Private shape3 As Shape
    Private graph1 As Telerik.Reporting.Graph
    Private cartesianCoordinateSystem1 As CartesianCoordinateSystem
    Private graphAxis2 As GraphAxis
    Private graphAxis1 As GraphAxis
    Private mainDataSource As SqlDataSource
    Private lineSeries1 As AreaSeries
    Private graph2 As Telerik.Reporting.Graph
    Private cartesianCoordinateSystem3 As CartesianCoordinateSystem
    Private graphAxis6 As GraphAxis
    Private graphAxis5 As GraphAxis
    Private barSeries1 As BarSeries
    Private Employees As SqlDataSource
    Private crosstabEmployeeSales As Crosstab
    Private textBox6 As Telerik.Reporting.TextBox
    Private textBox5 As Telerik.Reporting.TextBox
    Private textBox9 As Telerik.Reporting.TextBox
    Private textBox13 As Telerik.Reporting.TextBox
    Private textBoxTableTitle As Telerik.Reporting.TextBox
    Private textBoxSalesOrderNumber As Telerik.Reporting.TextBox
    Private textBoxCategory As Telerik.Reporting.TextBox
    Private textBoxCategoryAmount As Telerik.Reporting.TextBox
    Private textBox12 As Telerik.Reporting.TextBox
    Private textBox4 As Telerik.Reporting.TextBox
    Private textBoxOrderTotal As Telerik.Reporting.TextBox
    Private textBox10 As Telerik.Reporting.TextBox
    Private textBoxTotalLabel As Telerik.Reporting.TextBox
    Private textBoxEmployeeSalesTotal As Telerik.Reporting.TextBox
End Class