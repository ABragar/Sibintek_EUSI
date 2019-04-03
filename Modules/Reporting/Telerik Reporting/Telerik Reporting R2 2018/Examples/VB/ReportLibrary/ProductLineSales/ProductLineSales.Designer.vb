Imports System.ComponentModel
Imports System.Drawing
Imports Telerik.Reporting
Imports Telerik.Reporting.Drawing

Partial Class ProductLineSales
	#Region "Component Designer generated code"
	''' <summary>
	''' Required method for telerik Reporting designer support - do not modify
	''' the contents of this method with the code editor.
	''' </summary>
	Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ProductLineSales))
        Dim GraphGroup1 As Telerik.Reporting.GraphGroup = New Telerik.Reporting.GraphGroup()
        Dim ColorPalette1 As Telerik.Reporting.Drawing.ColorPalette = New Telerik.Reporting.Drawing.ColorPalette()
        Dim GraphTitle1 As Telerik.Reporting.GraphTitle = New Telerik.Reporting.GraphTitle()
        Dim NumericalScale1 As Telerik.Reporting.NumericalScale = New Telerik.Reporting.NumericalScale()
        Dim CategoryScale1 As Telerik.Reporting.CategoryScale = New Telerik.Reporting.CategoryScale()
        Dim GraphGroup2 As Telerik.Reporting.GraphGroup = New Telerik.Reporting.GraphGroup()
        Dim GraphGroup3 As Telerik.Reporting.GraphGroup = New Telerik.Reporting.GraphGroup()
        Dim ColorPalette2 As Telerik.Reporting.Drawing.ColorPalette = New Telerik.Reporting.Drawing.ColorPalette()
        Dim GraphTitle2 As Telerik.Reporting.GraphTitle = New Telerik.Reporting.GraphTitle()
        Dim NumericalScale2 As Telerik.Reporting.NumericalScale = New Telerik.Reporting.NumericalScale()
        Dim CategoryScale2 As Telerik.Reporting.CategoryScale = New Telerik.Reporting.CategoryScale()
        Dim GraphGroup4 As Telerik.Reporting.GraphGroup = New Telerik.Reporting.GraphGroup()
        Dim Group1 As Telerik.Reporting.Group = New Telerik.Reporting.Group()
        Dim ReportParameter1 As Telerik.Reporting.ReportParameter = New Telerik.Reporting.ReportParameter()
        Dim ReportParameter2 As Telerik.Reporting.ReportParameter = New Telerik.Reporting.ReportParameter()
        Dim ReportParameter3 As Telerik.Reporting.ReportParameter = New Telerik.Reporting.ReportParameter()
        Dim ReportParameter4 As Telerik.Reporting.ReportParameter = New Telerik.Reporting.ReportParameter()
        Dim StyleRule1 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule2 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule3 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule4 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Me.groupFooterSection1 = New Telerik.Reporting.GroupFooterSection()
        Me.groupHeaderSection1 = New Telerik.Reporting.GroupHeaderSection()
        Me.textBoxCategories = New Telerik.Reporting.TextBox()
        Me.textBoxTitle = New Telerik.Reporting.TextBox()
        Me.textBoxTimeInterval = New Telerik.Reporting.TextBox()
        Me.shape3 = New Telerik.Reporting.Shape()
        Me.productCategoriesDataSource = New Telerik.Reporting.SqlDataSource()
        Me.productSubcategoriesDataSource = New Telerik.Reporting.SqlDataSource()
        Me.detail = New Telerik.Reporting.DetailSection()
        Me.textBoxTopEmployeesHeader = New Telerik.Reporting.TextBox()
        Me.textBoxTopStoresHeader = New Telerik.Reporting.TextBox()
        Me.graph1 = New Telerik.Reporting.Graph()
        Me.cartesianCoordinateSystem2 = New Telerik.Reporting.CartesianCoordinateSystem()
        Me.graphAxis3 = New Telerik.Reporting.GraphAxis()
        Me.graphAxis4 = New Telerik.Reporting.GraphAxis()
        Me.employeesDataSource = New Telerik.Reporting.SqlDataSource()
        Me.barSeries1 = New Telerik.Reporting.BarSeries()
        Me.graph2 = New Telerik.Reporting.Graph()
        Me.polarCoordinateSystem3 = New Telerik.Reporting.PolarCoordinateSystem()
        Me.graphAxis7 = New Telerik.Reporting.GraphAxis()
        Me.graphAxis8 = New Telerik.Reporting.GraphAxis()
        Me.storesDataSource = New Telerik.Reporting.SqlDataSource()
        Me.barSeries2 = New Telerik.Reporting.BarSeries()
        Me.reportFooterSection1 = New Telerik.Reporting.ReportFooterSection()
        Me.textBoxReportFooter = New Telerik.Reporting.TextBox()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'groupFooterSection1
        '
        Me.groupFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.0520833320915699R)
        Me.groupFooterSection1.Name = "groupFooterSection1"
        Me.groupFooterSection1.Style.Visible = False
        '
        'groupHeaderSection1
        '
        Me.groupHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(1.1999999284744263R)
        Me.groupHeaderSection1.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.textBoxCategories, Me.textBoxTitle, Me.textBoxTimeInterval, Me.shape3})
        Me.groupHeaderSection1.Name = "groupHeaderSection1"
        '
        'textBoxCategories
        '
        Me.textBoxCategories.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.21311600506305695R))
        Me.textBoxCategories.Name = "textBoxCategories"
        Me.textBoxCategories.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.0999212265014648R), Telerik.Reporting.Drawing.Unit.Inch(0.60000002384185791R))
        Me.textBoxCategories.StyleName = "Header"
        Me.textBoxCategories.Value = "= Parameters.ProductCategory+"" » ""+Join("", "", Parameters.ProductSubcategory.Value" & _
    ")"
        '
        'textBoxTitle
        '
        Me.textBoxTitle.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.000039339065551757813R))
        Me.textBoxTitle.Name = "textBoxTitle"
        Me.textBoxTitle.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.3020832538604736R), Telerik.Reporting.Drawing.Unit.Inch(0.21299798786640167R))
        Me.textBoxTitle.Style.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(11, Byte), Integer))
        Me.textBoxTitle.Style.Font.Bold = True
        Me.textBoxTitle.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        Me.textBoxTitle.Value = "PRODUCT LINE SALES"
        '
        'textBoxTimeInterval
        '
        Me.textBoxTimeInterval.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.4558329582214355R), Telerik.Reporting.Drawing.Unit.Inch(0.000039339065551757813R))
        Me.textBoxTimeInterval.Name = "textBoxTimeInterval"
        Me.textBoxTimeInterval.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.64408802986145R), Telerik.Reporting.Drawing.Unit.Inch(0.21299798786640167R))
        Me.textBoxTimeInterval.Style.Font.Bold = True
        Me.textBoxTimeInterval.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.textBoxTimeInterval.Value = "{Parameters.FromDate.Value.ToString(""MMM yyyy"")} - {Parameters.ToDate.Value.ToStr" & _
    "ing(""MMM yyyy"")}"
        '
        'shape3
        '
        Me.shape3.Anchoring = CType((Telerik.Reporting.AnchoringStyles.Left Or Telerik.Reporting.AnchoringStyles.Right), Telerik.Reporting.AnchoringStyles)
        Me.shape3.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.000039378803194267675R), Telerik.Reporting.Drawing.Unit.Inch(0.83958339691162109R))
        Me.shape3.Name = "shape3"
        Me.shape3.ShapeType = New Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW)
        Me.shape3.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.1099214553833R), Telerik.Reporting.Drawing.Unit.Point(3.75R))
        Me.shape3.Stretch = True
        Me.shape3.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.shape3.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid
        Me.shape3.Style.BorderWidth.Top = Telerik.Reporting.Drawing.Unit.Point(3.0R)
        Me.shape3.Style.Color = System.Drawing.Color.Transparent
        '
        'productCategoriesDataSource
        '
        Me.productCategoriesDataSource.ConnectionString = "My.MySettings.TelerikConnectionString"
        Me.productCategoriesDataSource.Name = "productCategoriesDataSource"
        Me.productCategoriesDataSource.SelectCommand = "SELECT Production.ProductCategory.Name AS ProductCategory" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "FROM Production.Produc" & _
    "tCategory"
        '
        'productSubcategoriesDataSource
        '
        Me.productSubcategoriesDataSource.ConnectionString = "My.MySettings.TelerikConnectionString"
        Me.productSubcategoriesDataSource.Name = "productSubcategoriesDataSource"
        Me.productSubcategoriesDataSource.Parameters.AddRange(New Telerik.Reporting.SqlDataSourceParameter() {New Telerik.Reporting.SqlDataSourceParameter("@productCategory", System.Data.DbType.[String], "=Parameters.ProductCategory.Value")})
        Me.productSubcategoriesDataSource.SelectCommand = resources.GetString("productSubcategoriesDataSource.SelectCommand")
        '
        'detail
        '
        Me.detail.Height = Telerik.Reporting.Drawing.Unit.Inch(7.1000003814697266R)
        Me.detail.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.textBoxTopEmployeesHeader, Me.textBoxTopStoresHeader, Me.graph1, Me.graph2})
        Me.detail.Name = "detail"
        '
        'textBoxTopEmployeesHeader
        '
        Me.textBoxTopEmployeesHeader.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.000039378803194267675R), Telerik.Reporting.Drawing.Unit.Inch(0.0R))
        Me.textBoxTopEmployeesHeader.Name = "textBoxTopEmployeesHeader"
        Me.textBoxTopEmployeesHeader.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.0998811721801758R), Telerik.Reporting.Drawing.Unit.Inch(0.30000004172325134R))
        Me.textBoxTopEmployeesHeader.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBoxTopEmployeesHeader.Style.Color = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(105, Byte), Integer), CType(CType(104, Byte), Integer))
        Me.textBoxTopEmployeesHeader.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Top
        Me.textBoxTopEmployeesHeader.StyleName = "Category"
        Me.textBoxTopEmployeesHeader.TocText = "Top 10 Employees"
        Me.textBoxTopEmployeesHeader.Value = "Top 10 Employees"
        '
        'textBoxTopStoresHeader
        '
        Me.textBoxTopStoresHeader.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(3.5999999046325684R))
        Me.textBoxTopStoresHeader.Name = "textBoxTopStoresHeader"
        Me.textBoxTopStoresHeader.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.0999603271484375R), Telerik.Reporting.Drawing.Unit.Inch(0.3333333432674408R))
        Me.textBoxTopStoresHeader.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBoxTopStoresHeader.Style.Color = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(105, Byte), Integer), CType(CType(104, Byte), Integer))
        Me.textBoxTopStoresHeader.StyleName = "Category"
        Me.textBoxTopStoresHeader.TocText = "Top 10 Stores"
        Me.textBoxTopStoresHeader.Value = "Top 10 Stores"
        '
        'graph1
        '
        GraphGroup1.Filters.Add(New Telerik.Reporting.Filter("=Sum(Fields.SubTotal)", Telerik.Reporting.FilterOperator.TopN, "=10"))
        GraphGroup1.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.Employee"))
        GraphGroup1.Name = "employeeGroup1"
        GraphGroup1.Sortings.Add(New Telerik.Reporting.Sorting("=Sum(Fields.SubTotal)", Telerik.Reporting.SortDirection.Asc))
        Me.graph1.CategoryGroups.Add(GraphGroup1)
        ColorPalette1.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(21, Byte), Integer), CType(CType(198, Byte), Integer), CType(CType(198, Byte), Integer)))
        Me.graph1.ColorPalette = ColorPalette1
        Me.graph1.CoordinateSystems.Add(Me.cartesianCoordinateSystem2)
        Me.graph1.DataSource = Me.employeesDataSource
        Me.graph1.Legend.Style.LineColor = System.Drawing.Color.LightGray
        Me.graph1.Legend.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.graph1.Legend.Style.Visible = False
        Me.graph1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0099999997764825821R), Telerik.Reporting.Drawing.Unit.Inch(0.40000000596046448R))
        Me.graph1.Name = "graph1"
        Me.graph1.PlotAreaStyle.LineColor = System.Drawing.Color.LightGray
        Me.graph1.PlotAreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.graph1.Series.Add(Me.barSeries1)
        Me.graph1.SeriesGroups.Add(GraphGroup2)
        Me.graph1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.1000003814697266R), Telerik.Reporting.Drawing.Unit.Inch(2.9300000667572021R))
        Me.graph1.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        Me.graph1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        Me.graph1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        Me.graph1.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        GraphTitle1.Position = Telerik.Reporting.GraphItemPosition.TopCenter
        GraphTitle1.Style.LineColor = System.Drawing.Color.LightGray
        GraphTitle1.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        GraphTitle1.Style.Visible = False
        GraphTitle1.Text = "graph1"
        Me.graph1.Titles.Add(GraphTitle1)
        '
        'cartesianCoordinateSystem2
        '
        Me.cartesianCoordinateSystem2.Name = "cartesianCoordinateSystem2"
        Me.cartesianCoordinateSystem2.XAxis = Me.graphAxis3
        Me.cartesianCoordinateSystem2.YAxis = Me.graphAxis4
        '
        'graphAxis3
        '
        Me.graphAxis3.LabelFormat = "{0:C0}K"
        Me.graphAxis3.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis3.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis3.MajorTickMarkDisplayType = Telerik.Reporting.GraphAxisTickMarkDisplayType.None
        Me.graphAxis3.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis3.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis3.MinorGridLineStyle.Visible = False
        Me.graphAxis3.Name = "graphAxis3"
        Me.graphAxis3.Scale = NumericalScale1
        '
        'graphAxis4
        '
        Me.graphAxis4.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis4.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis4.MajorGridLineStyle.Visible = False
        Me.graphAxis4.MajorTickMarkDisplayType = Telerik.Reporting.GraphAxisTickMarkDisplayType.None
        Me.graphAxis4.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis4.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis4.MinorGridLineStyle.Visible = False
        Me.graphAxis4.Name = "graphAxis4"
        CategoryScale1.SpacingSlotCount = 0.4R
        Me.graphAxis4.Scale = CategoryScale1
        '
        'employeesDataSource
        '
        Me.employeesDataSource.ConnectionString = "My.MySettings.TelerikConnectionString"
        Me.employeesDataSource.Name = "employeesDataSource"
        Me.employeesDataSource.Parameters.AddRange(New Telerik.Reporting.SqlDataSourceParameter() {New Telerik.Reporting.SqlDataSourceParameter("@productCategory", System.Data.DbType.[String], "=Parameters.ProductCategory.Value"), New Telerik.Reporting.SqlDataSourceParameter("@FromDate", System.Data.DbType.DateTime, "=Parameters.FromDate.Value"), New Telerik.Reporting.SqlDataSourceParameter("@ToDate", System.Data.DbType.DateTime, "=Parameters.ToDate.Value"), New Telerik.Reporting.SqlDataSourceParameter("@productSubCategory", System.Data.DbType.[String], "=Parameters.ProductSubcategory.Value")})
        Me.employeesDataSource.SelectCommand = resources.GetString("employeesDataSource.SelectCommand")
        '
        'barSeries1
        '
        Me.barSeries1.CategoryGroup = GraphGroup1
        Me.barSeries1.CoordinateSystem = Me.cartesianCoordinateSystem2
        Me.barSeries1.DataPointLabel = "=Sum(Fields.SubTotal)"
        Me.barSeries1.DataPointLabelStyle.Visible = False
        Me.barSeries1.DataPointStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.barSeries1.DataPointStyle.Visible = True
        Me.barSeries1.LegendItem.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(0.0R)
        Me.barSeries1.LegendItem.Value = "SubTotal"
        GraphGroup2.Name = "seriesGroup1"
        Me.barSeries1.SeriesGroup = GraphGroup2
        Me.barSeries1.X = "=Sum(Fields.SubTotal) /1000.0"
        '
        'graph2
        '
        GraphGroup3.Name = "categoryGroup1"
        Me.graph2.CategoryGroups.Add(GraphGroup3)
        ColorPalette2.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(92, Byte), Integer), CType(CType(25, Byte), Integer), CType(CType(18, Byte), Integer)))
        ColorPalette2.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(194, Byte), Integer), CType(CType(46, Byte), Integer), CType(CType(10, Byte), Integer)))
        ColorPalette2.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(88, Byte), Integer), CType(CType(55, Byte), Integer)))
        ColorPalette2.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(177, Byte), Integer), CType(CType(90, Byte), Integer), CType(CType(40, Byte), Integer)))
        ColorPalette2.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(202, Byte), Integer), CType(CType(137, Byte), Integer), CType(CType(104, Byte), Integer)))
        ColorPalette2.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(211, Byte), Integer), CType(CType(191, Byte), Integer), CType(CType(105, Byte), Integer)))
        ColorPalette2.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(137, Byte), Integer), CType(CType(203, Byte), Integer), CType(CType(42, Byte), Integer)))
        ColorPalette2.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(180, Byte), Integer), CType(CType(114, Byte), Integer)))
        ColorPalette2.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(21, Byte), Integer), CType(CType(198, Byte), Integer), CType(CType(198, Byte), Integer)))
        ColorPalette2.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(105, Byte), Integer), CType(CType(104, Byte), Integer)))
        Me.graph2.ColorPalette = ColorPalette2
        Me.graph2.CoordinateSystems.Add(Me.polarCoordinateSystem3)
        Me.graph2.DataSource = Me.storesDataSource
        Me.graph2.Legend.Style.Font.Name = "Segoe UI"
        Me.graph2.Legend.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8.0R)
        Me.graph2.Legend.Style.LineColor = System.Drawing.Color.LightGray
        Me.graph2.Legend.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.graph2.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0099999997764825821R), Telerik.Reporting.Drawing.Unit.Inch(4.0999999046325684R))
        Me.graph2.Name = "graph2"
        Me.graph2.PlotAreaStyle.LineColor = System.Drawing.Color.LightGray
        Me.graph2.PlotAreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.graph2.Series.Add(Me.barSeries2)
        Me.graph2.SeriesGroups.Add(GraphGroup4)
        Me.graph2.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(7.190000057220459R), Telerik.Reporting.Drawing.Unit.Inch(2.9300000667572021R))
        Me.graph2.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(20.0R)
        Me.graph2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        Me.graph2.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(30.0R)
        Me.graph2.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(20.0R)
        GraphTitle2.Position = Telerik.Reporting.GraphItemPosition.TopCenter
        GraphTitle2.Style.LineColor = System.Drawing.Color.LightGray
        GraphTitle2.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.graph2.Titles.Add(GraphTitle2)
        '
        'polarCoordinateSystem3
        '
        Me.polarCoordinateSystem3.AngularAxis = Me.graphAxis7
        Me.polarCoordinateSystem3.Name = "polarCoordinateSystem3"
        Me.polarCoordinateSystem3.RadialAxis = Me.graphAxis8
        '
        'graphAxis7
        '
        Me.graphAxis7.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis7.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis7.MajorGridLineStyle.Visible = False
        Me.graphAxis7.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis7.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis7.MinorGridLineStyle.Visible = False
        Me.graphAxis7.Name = "graphAxis7"
        Me.graphAxis7.Scale = NumericalScale2
        Me.graphAxis7.Style.Visible = False
        '
        'graphAxis8
        '
        Me.graphAxis8.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis8.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis8.MajorGridLineStyle.Visible = False
        Me.graphAxis8.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis8.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis8.MinorGridLineStyle.Visible = False
        Me.graphAxis8.Name = "graphAxis8"
        CategoryScale2.PositionMode = Telerik.Reporting.AxisPositionMode.OnTicks
        CategoryScale2.SpacingSlotCount = 0.0R
        Me.graphAxis8.Scale = CategoryScale2
        Me.graphAxis8.Style.Visible = False
        '
        'storesDataSource
        '
        Me.storesDataSource.ConnectionString = "My.MySettings.TelerikConnectionString"
        Me.storesDataSource.Name = "storesDataSource"
        Me.storesDataSource.Parameters.AddRange(New Telerik.Reporting.SqlDataSourceParameter() {New Telerik.Reporting.SqlDataSourceParameter("@productCategory", System.Data.DbType.[String], "=Parameters.ProductCategory.Value"), New Telerik.Reporting.SqlDataSourceParameter("@FromDate", System.Data.DbType.DateTime, "=Parameters.FromDate.Value"), New Telerik.Reporting.SqlDataSourceParameter("@ToDate", System.Data.DbType.DateTime, "=Parameters.ToDate.Value")})
        Me.storesDataSource.SelectCommand = resources.GetString("storesDataSource.SelectCommand")
        '
        'barSeries2
        '
        Me.barSeries2.ArrangeMode = Telerik.Reporting.GraphSeriesArrangeMode.Stacked100
        Me.barSeries2.CategoryGroup = GraphGroup3
        Me.barSeries2.CoordinateSystem = Me.polarCoordinateSystem3
        Me.barSeries2.DataPointLabel = "=Sum(Fields.SubTotal)/1000.0"
        Me.barSeries2.DataPointLabelFormat = "{0:C0}K"
        Me.barSeries2.DataPointLabelStyle.Visible = True
        Me.barSeries2.DataPointStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.barSeries2.DataPointStyle.Visible = True
        Me.barSeries2.LegendItem.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(0.0R)
        Me.barSeries2.LegendItem.Value = "=Fields.StoreName"
        GraphGroup4.Filters.Add(New Telerik.Reporting.Filter("=Sum(Fields.SubTotal)", Telerik.Reporting.FilterOperator.TopN, "=10"))
        GraphGroup4.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.StoreName"))
        GraphGroup4.Name = "storeNameGroup1"
        GraphGroup4.Sortings.Add(New Telerik.Reporting.Sorting("=Sum(Fields.SubTotal)", Telerik.Reporting.SortDirection.Asc))
        Me.barSeries2.SeriesGroup = GraphGroup4
        Me.barSeries2.X = "=Sum(Fields.SubTotal)"
        '
        'reportFooterSection1
        '
        Me.reportFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.30000001192092896R)
        Me.reportFooterSection1.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.textBoxReportFooter})
        Me.reportFooterSection1.Name = "reportFooterSection1"
        '
        'textBoxReportFooter
        '
        Me.textBoxReportFooter.Name = "textBoxReportFooter"
        Me.textBoxReportFooter.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.0999212265014648R), Telerik.Reporting.Drawing.Unit.Inch(0.30000001192092896R))
        Me.textBoxReportFooter.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBoxReportFooter.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.textBoxReportFooter.Value = "AdventureWorks {Now().Year}"
        '
        'ProductLineSales
        '
        Me.Culture = New System.Globalization.CultureInfo("en-US")
        Me.DocumentName = "= Parameters.ProductCategory.Value + 'Sales(' + Join("", "", Parameters.ProductSubc" & _
    "ategory.Value) + ')[' + NOW().ToString('MMMM yyyy') + ']'"
        Group1.GroupFooter = Me.groupFooterSection1
        Group1.GroupHeader = Me.groupHeaderSection1
        Group1.Name = "group1"
        Me.Groups.AddRange(New Telerik.Reporting.Group() {Group1})
        Me.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.groupHeaderSection1, Me.groupFooterSection1, Me.detail, Me.reportFooterSection1})
        Me.Name = "ProductLineSales"
        Me.PageSettings.Landscape = False
        Me.PageSettings.Margins = New Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224R), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224R), Telerik.Reporting.Drawing.Unit.Inch(0.30000001192092896R), Telerik.Reporting.Drawing.Unit.Inch(0.40000000596046448R))
        Me.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Letter
        ReportParameter1.AutoRefresh = True
        ReportParameter1.AvailableValues.DataSource = Me.productCategoriesDataSource
        ReportParameter1.AvailableValues.Sortings.Add(New Telerik.Reporting.Sorting("=Fields.ProductCategory", Telerik.Reporting.SortDirection.Asc))
        ReportParameter1.AvailableValues.ValueMember = "ProductCategory"
        ReportParameter1.Name = "ProductCategory"
        ReportParameter1.Text = "Category"
        ReportParameter1.Value = "Bikes"
        ReportParameter1.Visible = True
        ReportParameter2.AutoRefresh = True
        ReportParameter2.AvailableValues.DataSource = Me.productSubcategoriesDataSource
        ReportParameter2.AvailableValues.Sortings.Add(New Telerik.Reporting.Sorting("=Fields.ProductSubcategory", Telerik.Reporting.SortDirection.Asc))
        ReportParameter2.AvailableValues.ValueMember = "ProductSubcategory"
        ReportParameter2.MultiValue = True
        ReportParameter2.Name = "ProductSubcategory"
        ReportParameter2.Text = "Subcategory"
        ReportParameter2.Value = "=First(Fields.ProductSubcategory)"
        ReportParameter2.Visible = True
        ReportParameter3.Name = "FromDate"
        ReportParameter3.Type = Telerik.Reporting.ReportParameterType.DateTime
        ReportParameter3.Value = "=#2001-01-01#"
        ReportParameter4.Name = "ToDate"
        ReportParameter4.Type = Telerik.Reporting.ReportParameterType.DateTime
        ReportParameter4.Value = "=#2004-12-31#"
        Me.ReportParameters.Add(ReportParameter1)
        Me.ReportParameters.Add(ReportParameter2)
        Me.ReportParameters.Add(ReportParameter3)
        Me.ReportParameters.Add(ReportParameter4)
        StyleRule1.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.TextItemBase)), New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.HtmlTextBox))})
        StyleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        StyleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        StyleRule2.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.StyleSelector("Header")})
        StyleRule2.Style.Font.Bold = True
        StyleRule2.Style.Font.Name = "Segoe UI Light"
        StyleRule2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(25.0R)
        StyleRule2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        StyleRule3.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.TextBox))})
        StyleRule3.Style.Font.Name = "Segoe UI"
        StyleRule3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        StyleRule3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        StyleRule4.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.StyleSelector("Category")})
        StyleRule4.Style.Font.Name = "Segoe UI"
        StyleRule4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(13.0R)
        StyleRule4.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.StyleSheet.AddRange(New Telerik.Reporting.Drawing.StyleRule() {StyleRule1, StyleRule2, StyleRule3, StyleRule4})
        Me.TocText = "Product Line Sales"
        Me.Width = Telerik.Reporting.Drawing.Unit.Inch(8.1100006103515625R)
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
	#End Region

	Private detail As DetailSection
	Private textBoxTopEmployeesHeader As TextBox
	Private textBoxTopStoresHeader As TextBox
	Private groupFooterSection1 As GroupFooterSection
	Private groupHeaderSection1 As GroupHeaderSection
	Private textBoxCategories As TextBox
	Private textBoxTitle As TextBox
	Private reportFooterSection1 As ReportFooterSection
	Private textBoxReportFooter As TextBox
	Private textBoxTimeInterval As TextBox
	Private shape3 As Shape
	Private graph1 As Graph
	Private cartesianCoordinateSystem2 As CartesianCoordinateSystem
	Private graphAxis3 As GraphAxis
	Private graphAxis4 As GraphAxis
	Private employeesDataSource As SqlDataSource
	Private barSeries1 As BarSeries
	Private graph2 As Graph
	Private polarCoordinateSystem3 As PolarCoordinateSystem
	Private graphAxis7 As GraphAxis
	Private graphAxis8 As GraphAxis
	Private storesDataSource As SqlDataSource
	Private barSeries2 As BarSeries
	Private productCategoriesDataSource As SqlDataSource
	Private productSubcategoriesDataSource As SqlDataSource
End Class