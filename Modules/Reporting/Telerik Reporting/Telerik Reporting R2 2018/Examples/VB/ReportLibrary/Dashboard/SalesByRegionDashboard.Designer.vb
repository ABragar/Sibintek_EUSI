Partial Class SalesByRegionDashboard
    
    'NOTE: The following procedure is required by the telerik Reporting Designer
    'It can be modified using the telerik Reporting Designer.  
    'Do not modify it using the code editor.
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SalesByRegionDashboard))
        Dim ColorPalette1 As Telerik.Reporting.Drawing.ColorPalette = New Telerik.Reporting.Drawing.ColorPalette()
        Dim GeoLocationMapGroup1 As Telerik.Reporting.GeoLocationMapGroup = New Telerik.Reporting.GeoLocationMapGroup()
        Dim MapLegend1 As Telerik.Reporting.MapLegend = New Telerik.Reporting.MapLegend()
        Dim MercatorProjection1 As Telerik.Reporting.MercatorProjection = New Telerik.Reporting.MercatorProjection()
        Dim GenericTileProvider1 As Telerik.Reporting.GenericTileProvider = New Telerik.Reporting.GenericTileProvider()
        Dim MapTitle1 As Telerik.Reporting.MapTitle = New Telerik.Reporting.MapTitle()
        Dim MapGroup1 As Telerik.Reporting.MapGroup = New Telerik.Reporting.MapGroup()
        Dim MapGroup2 As Telerik.Reporting.MapGroup = New Telerik.Reporting.MapGroup()
        Dim GraphGroup1 As Telerik.Reporting.GraphGroup = New Telerik.Reporting.GraphGroup()
        Dim ColorPalette2 As Telerik.Reporting.Drawing.ColorPalette = New Telerik.Reporting.Drawing.ColorPalette()
        Dim GraphTitle1 As Telerik.Reporting.GraphTitle = New Telerik.Reporting.GraphTitle()
        Dim DateTimeScale1 As Telerik.Reporting.DateTimeScale = New Telerik.Reporting.DateTimeScale()
        Dim NumericalScale1 As Telerik.Reporting.NumericalScale = New Telerik.Reporting.NumericalScale()
        Dim GraphGroup2 As Telerik.Reporting.GraphGroup = New Telerik.Reporting.GraphGroup()
        Dim GraphGroup3 As Telerik.Reporting.GraphGroup = New Telerik.Reporting.GraphGroup()
        Dim ColorPalette3 As Telerik.Reporting.Drawing.ColorPalette = New Telerik.Reporting.Drawing.ColorPalette()
        Dim GraphTitle2 As Telerik.Reporting.GraphTitle = New Telerik.Reporting.GraphTitle()
        Dim NumericalScale2 As Telerik.Reporting.NumericalScale = New Telerik.Reporting.NumericalScale()
        Dim CategoryScale1 As Telerik.Reporting.CategoryScale = New Telerik.Reporting.CategoryScale()
        Dim GraphGroup4 As Telerik.Reporting.GraphGroup = New Telerik.Reporting.GraphGroup()
        Dim GraphGroup5 As Telerik.Reporting.GraphGroup = New Telerik.Reporting.GraphGroup()
        Dim ColorPalette4 As Telerik.Reporting.Drawing.ColorPalette = New Telerik.Reporting.Drawing.ColorPalette()
        Dim GraphTitle3 As Telerik.Reporting.GraphTitle = New Telerik.Reporting.GraphTitle()
        Dim NumericalScale3 As Telerik.Reporting.NumericalScale = New Telerik.Reporting.NumericalScale()
        Dim CategoryScale2 As Telerik.Reporting.CategoryScale = New Telerik.Reporting.CategoryScale()
        Dim GraphGroup6 As Telerik.Reporting.GraphGroup = New Telerik.Reporting.GraphGroup()
        Dim ReportParameter1 As Telerik.Reporting.ReportParameter = New Telerik.Reporting.ReportParameter()
        Dim ReportParameter2 As Telerik.Reporting.ReportParameter = New Telerik.Reporting.ReportParameter()
        Dim ReportParameter3 As Telerik.Reporting.ReportParameter = New Telerik.Reporting.ReportParameter()
        Dim ReportParameter4 As Telerik.Reporting.ReportParameter = New Telerik.Reporting.ReportParameter()
        Dim StyleRule1 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule2 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Me.yearsData = New Telerik.Reporting.SqlDataSource()
        Me.countryData = New Telerik.Reporting.SqlDataSource()
        Me.productCategoriesData = New Telerik.Reporting.SqlDataSource()
        Me.salesData = New Telerik.Reporting.SqlDataSource()
        Me.detailSection1 = New Telerik.Reporting.DetailSection()
        Me.map1 = New Telerik.Reporting.Map()
        Me.PieMapSeries1 = New Telerik.Reporting.PieMapSeries()
        Me.graph1 = New Telerik.Reporting.Graph()
        Me.cartesianCoordinateSystem1 = New Telerik.Reporting.CartesianCoordinateSystem()
        Me.graphAxis2 = New Telerik.Reporting.GraphAxis()
        Me.graphAxis1 = New Telerik.Reporting.GraphAxis()
        Me.lineSeries1 = New Telerik.Reporting.LineSeries()
        Me.graph2 = New Telerik.Reporting.Graph()
        Me.cartesianCoordinateSystem2 = New Telerik.Reporting.CartesianCoordinateSystem()
        Me.graphAxis3 = New Telerik.Reporting.GraphAxis()
        Me.graphAxis4 = New Telerik.Reporting.GraphAxis()
        Me.barSeries1 = New Telerik.Reporting.BarSeries()
        Me.graph3 = New Telerik.Reporting.Graph()
        Me.cartesianCoordinateSystem3 = New Telerik.Reporting.CartesianCoordinateSystem()
        Me.graphAxis5 = New Telerik.Reporting.GraphAxis()
        Me.graphAxis6 = New Telerik.Reporting.GraphAxis()
        Me.barSeries2 = New Telerik.Reporting.BarSeries()
        Me.textBox1 = New Telerik.Reporting.TextBox()
        Me.textBox2 = New Telerik.Reporting.TextBox()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'yearsData
        '
        Me.yearsData.ConnectionString = "My.MySettings.TelerikConnectionString"
        Me.yearsData.Name = "yearsData"
        Me.yearsData.SelectCommand = "SELECT DISTINCT DatePart(YEAR, soh.OrderDate) AS Year" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "FROM Sales.SalesOrderHeade" & _
    "r AS soh" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "ORDER BY DatePart(YEAR, soh.OrderDate) ASC"
        '
        'countryData
        '
        Me.countryData.ConnectionString = "My.MySettings.TelerikConnectionString"
        Me.countryData.Name = "countryData"
        Me.countryData.SelectCommand = resources.GetString("countryData.SelectCommand")
        '
        'productCategoriesData
        '
        Me.productCategoriesData.ConnectionString = "My.MySettings.TelerikConnectionString"
        Me.productCategoriesData.Name = "productCategoriesData"
        Me.productCategoriesData.SelectCommand = "SELECT Production.ProductCategory.ProductCategoryID, " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Production.ProductCategory" & _
    ".Name" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "FROM Production.ProductCategory"
        '
        'salesData
        '
        Me.salesData.ConnectionString = "My.MySettings.TelerikConnectionString"
        Me.salesData.Name = "salesData"
        Me.salesData.Parameters.AddRange(New Telerik.Reporting.SqlDataSourceParameter() {New Telerik.Reporting.SqlDataSourceParameter("@country", System.Data.DbType.[String], "=Parameters.country.Value"), New Telerik.Reporting.SqlDataSourceParameter("@year", System.Data.DbType.Int32, "=Parameters.year.Value"), New Telerik.Reporting.SqlDataSourceParameter("@category", System.Data.DbType.Int32, "=Parameters.category.Value")})
        Me.salesData.SelectCommand = resources.GetString("salesData.SelectCommand")
        '
        'detailSection1
        '
        Me.detailSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(8.070866584777832R)
        Me.detailSection1.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.map1, Me.graph1, Me.graph2, Me.graph3, Me.textBox1, Me.textBox2})
        Me.detailSection1.Name = "detailSection1"
        Me.detailSection1.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Mm(5.0R)
        Me.detailSection1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Mm(5.0R)
        Me.detailSection1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Mm(5.0R)
        Me.detailSection1.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Mm(5.0R)
        '
        'map1
        '
        ColorPalette1.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(77, Byte), Integer)))
        ColorPalette1.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(162, Byte), Integer)))
        ColorPalette1.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(150, Byte), Integer), CType(CType(70, Byte), Integer)))
        ColorPalette1.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(119, Byte), Integer), CType(CType(44, Byte), Integer), CType(CType(42, Byte), Integer)))
        ColorPalette1.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(59, Byte), Integer), CType(CType(98, Byte), Integer)))
        ColorPalette1.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(182, Byte), Integer), CType(CType(87, Byte), Integer), CType(CType(8, Byte), Integer)))
        Me.map1.ColorPalette = ColorPalette1
        Me.map1.DataSource = Me.salesData
        Me.map1.Filters.Add(New Telerik.Reporting.Filter("=Fields.Country", Telerik.Reporting.FilterOperator.[In], "=@country"))
        Me.map1.Filters.Add(New Telerik.Reporting.Filter("=Fields.OrderDate.Year", Telerik.Reporting.FilterOperator.[In], "=@year"))
        GeoLocationMapGroup1.Filters.Add(New Telerik.Reporting.Filter("=Sum(Fields.LineTotal)", Telerik.Reporting.FilterOperator.TopN, "=CInt(Parameters.topN.Value)"))
        GeoLocationMapGroup1.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.State"))
        GeoLocationMapGroup1.Name = "stateGroup"
        GeoLocationMapGroup1.Sortings.Add(New Telerik.Reporting.Sorting("=Fields.State", Telerik.Reporting.SortDirection.Asc))
        Me.map1.GeoLocationGroups.Add(GeoLocationMapGroup1)
        MapLegend1.IsInsidePlotArea = False
        MapLegend1.Position = Telerik.Reporting.GraphItemPosition.BottomLeft
        MapLegend1.Style.LineColor = System.Drawing.Color.LightGray
        MapLegend1.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0.0R)
        MapLegend1.Style.Visible = True
        Me.map1.Legends.Add(MapLegend1)
        Me.map1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.50010007619857788R), Telerik.Reporting.Drawing.Unit.Cm(3.0002996921539307R))
        Me.map1.Meridians.Style.LineColor = System.Drawing.Color.LightGray
        Me.map1.Name = "map1"
        Me.map1.Parallels.Style.LineColor = System.Drawing.Color.LightGray
        Me.map1.PlotAreaStyle.LineColor = System.Drawing.Color.LightGray
        Me.map1.PlotAreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0.0R)
        Me.map1.Projection = MercatorProjection1
        Me.map1.ScaleLegend.ItemStyle.BorderColor.Default = System.Drawing.Color.White
        Me.map1.ScaleLegend.ItemStyle.Font.Name = "Segoe UI Light"
        Me.map1.ScaleLegend.ItemStyle.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8.0R)
        Me.map1.ScaleLegend.ItemStyle.LineColor = System.Drawing.Color.DodgerBlue
        Me.map1.ScaleLegend.ItemStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(2.0R)
        Me.map1.ScaleLegend.ItemStyle.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(5.0R)
        Me.map1.ScaleLegend.ItemStyle.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(5.0R)
        Me.map1.ScaleLegend.ItemStyle.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.map1.ScaleLegend.ScaleUnits = Telerik.Reporting.DistanceUnitType.Imperial
        Me.map1.ScaleLegend.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom
        Me.map1.Series.Add(Me.PieMapSeries1)
        Me.map1.SeriesGroups.Add(MapGroup1)
        Me.map1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(14.499899864196777R), Telerik.Reporting.Drawing.Unit.Cm(11.499997138977051R))
        Me.map1.Style.Font.Name = "Segoe UI Light"
        Me.map1.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        Me.map1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        Me.map1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        Me.map1.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        GenericTileProvider1.Attribution = "© OpenStreetMap contributors"
        GenericTileProvider1.LogoUrl = "http://wiki.openstreetmap.org/w/images/thumb/7/79/Public-images-osm_logo.svg/32px-Public-images-osm_logo.svg.png"
        GenericTileProvider1.UrlSubdomains.Add("a")
        GenericTileProvider1.UrlSubdomains.Add("b")
        GenericTileProvider1.UrlSubdomains.Add("c")
        GenericTileProvider1.UrlTemplate = "http://{subdomain}.tile.openstreetmap.org/{zoom}/{X}/{Y}.png"
        Me.map1.TileProvider = GenericTileProvider1
        MapTitle1.Position = Telerik.Reporting.GraphItemPosition.TopLeft
        MapTitle1.Style.Color = System.Drawing.Color.FromArgb(CType(CType(152, Byte), Integer), CType(CType(152, Byte), Integer), CType(CType(152, Byte), Integer))
        MapTitle1.Style.Font.Bold = False
        MapTitle1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        MapTitle1.Style.LineColor = System.Drawing.Color.LightGray
        MapTitle1.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0.0R)
        MapTitle1.Style.Visible = True
        MapTitle1.Text = "SALES VOLUME BY COUNTRY"
        Me.map1.Titles.Add(MapTitle1)
        '
        'PieMapSeries1
        '
        Me.PieMapSeries1.DataPointLabel = "=Format('{0:N4}', Sum(Fields.LineTotal)/1e6)"
        Me.PieMapSeries1.DataPointLabelStyle.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8.0R)
        Me.PieMapSeries1.DataPointLabelStyle.Visible = False
        Me.PieMapSeries1.GeoLocationGroup = GeoLocationMapGroup1
        Me.PieMapSeries1.Latitude = "=(Fields.Lat)"
        Me.PieMapSeries1.LegendItem.MarkStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Point(0.0R)
        Me.PieMapSeries1.LegendItem.Style.BackgroundColor = System.Drawing.Color.Transparent
        Me.PieMapSeries1.LegendItem.Style.LineColor = System.Drawing.Color.Transparent
        Me.PieMapSeries1.LegendItem.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0.0R)
        Me.PieMapSeries1.LegendItem.Value = "= Fields.ProductSubCategory"
        Me.PieMapSeries1.Longitude = "=(Fields.Long)"
        Me.PieMapSeries1.MarkerMaxSize = Telerik.Reporting.Drawing.Unit.Mm(15.0R)
        Me.PieMapSeries1.MarkerMinSize = Telerik.Reporting.Drawing.Unit.Mm(8.0R)
        MapGroup2.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.ProductSubCategory"))
        MapGroup2.Name = "ProductSubCategoryGroup"
        MapGroup2.Sortings.Add(New Telerik.Reporting.Sorting("=Fields.ProductSubCategory", Telerik.Reporting.SortDirection.Asc))
        MapGroup1.ChildGroups.Add(MapGroup2)
        MapGroup1.Name = "seriesGroup"
        Me.PieMapSeries1.SeriesGroup = MapGroup1
        Me.PieMapSeries1.Size = "=Sum(Fields.LineTotal)"
        Me.PieMapSeries1.ToolTip.Title = "= Format('{0}, {1}', Fields.State, Fields.Country)"
        Me.PieMapSeries1.ToolTip.Text = "=Format('Total: ${0:0.00}M', Sum(Fields.LineTotal)/1e6)"
        '
        'graph1
        '
        GraphGroup1.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.OrderDate.Month"))
        GraphGroup1.Name = "orderDateMonthGroup"
        GraphGroup1.Sortings.Add(New Telerik.Reporting.Sorting("=Fields.OrderDate.Month", Telerik.Reporting.SortDirection.Asc))
        Me.graph1.CategoryGroups.Add(GraphGroup1)
        ColorPalette2.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(210, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(77, Byte), Integer)))
        ColorPalette2.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(210, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(162, Byte), Integer)))
        ColorPalette2.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(210, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(150, Byte), Integer), CType(CType(70, Byte), Integer)))
        ColorPalette2.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(210, Byte), Integer), CType(CType(119, Byte), Integer), CType(CType(44, Byte), Integer), CType(CType(42, Byte), Integer)))
        ColorPalette2.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(210, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(59, Byte), Integer), CType(CType(98, Byte), Integer)))
        ColorPalette2.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(210, Byte), Integer), CType(CType(182, Byte), Integer), CType(CType(87, Byte), Integer), CType(CType(8, Byte), Integer)))
        Me.graph1.ColorPalette = ColorPalette2
        Me.graph1.CoordinateSystems.Add(Me.cartesianCoordinateSystem1)
        Me.graph1.DataSource = Me.salesData
        Me.graph1.Filters.Add(New Telerik.Reporting.Filter("=Fields.Country", Telerik.Reporting.FilterOperator.[In], "=@country"))
        Me.graph1.Filters.Add(New Telerik.Reporting.Filter("=Fields.OrderDate.Year", Telerik.Reporting.FilterOperator.[In], "=@year"))
        Me.graph1.Legend.Position = Telerik.Reporting.GraphItemPosition.BottomCenter
        Me.graph1.Legend.Style.LineColor = System.Drawing.Color.LightGray
        Me.graph1.Legend.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0.0R)
        Me.graph1.Legend.Style.Visible = False
        Me.graph1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.50010007619857788R), Telerik.Reporting.Drawing.Unit.Cm(14.500496864318848R))
        Me.graph1.Name = "graph1"
        Me.graph1.PlotAreaStyle.LineColor = System.Drawing.Color.LightGray
        Me.graph1.PlotAreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0.0R)
        Me.graph1.Series.Add(Me.lineSeries1)
        Me.graph1.SeriesGroups.Add(GraphGroup2)
        Me.graph1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(14.499899864196777R), Telerik.Reporting.Drawing.Unit.Cm(5.5R))
        Me.graph1.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        Me.graph1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        Me.graph1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        Me.graph1.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        GraphTitle1.Position = Telerik.Reporting.GraphItemPosition.TopLeft
        GraphTitle1.Style.Color = System.Drawing.Color.FromArgb(CType(CType(152, Byte), Integer), CType(CType(152, Byte), Integer), CType(CType(152, Byte), Integer))
        GraphTitle1.Style.Font.Bold = False
        GraphTitle1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        GraphTitle1.Style.LineColor = System.Drawing.Color.LightGray
        GraphTitle1.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0.0R)
        GraphTitle1.Text = "SALES VOLUME, MOUNTLY (in millions USD)"
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
        Me.graphAxis2.LabelFormat = "{0:MMM}"
        Me.graphAxis2.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis2.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis2.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis2.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis2.MinorGridLineStyle.Visible = False
        Me.graphAxis2.Name = "graphAxis2"
        DateTimeScale1.BaseUnit = Telerik.Reporting.DateTimeScaleUnits.Months
        Me.graphAxis2.Scale = DateTimeScale1
        Me.graphAxis2.Title = ""
        Me.graphAxis2.TitlePlacement = Telerik.Reporting.GraphAxisTitlePlacement.AtMaximum
        '
        'graphAxis1
        '
        Me.graphAxis1.LabelFormat = "${0}M"
        Me.graphAxis1.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis1.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis1.MajorGridLineStyle.Visible = True
        Me.graphAxis1.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis1.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis1.MinorGridLineStyle.Visible = False
        Me.graphAxis1.Name = "graphAxis1"
        Me.graphAxis1.Scale = NumericalScale1
        Me.graphAxis1.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0.0R)
        Me.graphAxis1.Style.Visible = True
        Me.graphAxis1.Title = ""
        Me.graphAxis1.TitlePlacement = Telerik.Reporting.GraphAxisTitlePlacement.AtMaximum
        '
        'lineSeries1
        '
        Me.lineSeries1.CategoryGroup = GraphGroup1
        Me.lineSeries1.CoordinateSystem = Me.cartesianCoordinateSystem1
        Me.lineSeries1.DataPointLabel = "=Sum(Fields.LineTotal)"
        Me.lineSeries1.DataPointLabelStyle.Visible = False
        Me.lineSeries1.DataPointStyle.Visible = False
        Me.lineSeries1.LegendItem.Style.BackgroundColor = System.Drawing.Color.Transparent
        Me.lineSeries1.LegendItem.Style.LineColor = System.Drawing.Color.Transparent
        Me.lineSeries1.LegendItem.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0.0R)
        Me.lineSeries1.LegendItem.Value = "=Fields.OrderDate.Year"
        Me.lineSeries1.LineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.lineSeries1.MarkerMaxSize = Telerik.Reporting.Drawing.Unit.Pixel(50.0R)
        Me.lineSeries1.MarkerMinSize = Telerik.Reporting.Drawing.Unit.Pixel(5.0R)
        Me.lineSeries1.MarkerSize = Telerik.Reporting.Drawing.Unit.Pixel(5.0R)
        Me.lineSeries1.Name = "lineSeries1"
        GraphGroup2.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.OrderDate.Year"))
        GraphGroup2.Name = "orderDateYearGroup"
        GraphGroup2.Sortings.Add(New Telerik.Reporting.Sorting("=Fields.OrderDate.Year", Telerik.Reporting.SortDirection.Asc))
        Me.lineSeries1.SeriesGroup = GraphGroup2
        Me.lineSeries1.Size = Nothing
        Me.lineSeries1.X = "=Date(1, Fields.OrderDate.Month, 1)"
        Me.lineSeries1.Y = "=Sum(Fields.LineTotal)/1e6"
        '
        'graph2
        '
        GraphGroup3.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.ProductCategory"))
        GraphGroup3.Name = "productCategoryGroup"
        GraphGroup3.Sortings.Add(New Telerik.Reporting.Sorting("=Fields.ProductCategory", Telerik.Reporting.SortDirection.Asc))
        Me.graph2.CategoryGroups.Add(GraphGroup3)
        ColorPalette3.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(210, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(77, Byte), Integer)))
        ColorPalette3.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(210, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(162, Byte), Integer)))
        ColorPalette3.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(210, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(150, Byte), Integer), CType(CType(70, Byte), Integer)))
        ColorPalette3.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(210, Byte), Integer), CType(CType(119, Byte), Integer), CType(CType(44, Byte), Integer), CType(CType(42, Byte), Integer)))
        ColorPalette3.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(210, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(59, Byte), Integer), CType(CType(98, Byte), Integer)))
        ColorPalette3.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(210, Byte), Integer), CType(CType(182, Byte), Integer), CType(CType(87, Byte), Integer), CType(CType(8, Byte), Integer)))
        Me.graph2.ColorPalette = ColorPalette3
        Me.graph2.CoordinateSystems.Add(Me.cartesianCoordinateSystem2)
        Me.graph2.DataSource = Me.salesData
        Me.graph2.Filters.Add(New Telerik.Reporting.Filter("=Fields.Country", Telerik.Reporting.FilterOperator.[In], "=@country"))
        Me.graph2.Filters.Add(New Telerik.Reporting.Filter("=Fields.OrderDate.Year", Telerik.Reporting.FilterOperator.[In], "=@year"))
        Me.graph2.Legend.Position = Telerik.Reporting.GraphItemPosition.RightCenter
        Me.graph2.Legend.Style.LineColor = System.Drawing.Color.LightGray
        Me.graph2.Legend.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0.0R)
        Me.graph2.Legend.Style.Visible = True
        Me.graph2.Legend.Width = Telerik.Reporting.Drawing.Unit.Cm(3.0R)
        Me.graph2.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(15.000298500061035R), Telerik.Reporting.Drawing.Unit.Cm(14.500496864318848R))
        Me.graph2.Name = "graph2"
        Me.graph2.PlotAreaStyle.LineColor = System.Drawing.Color.LightGray
        Me.graph2.PlotAreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0.0R)
        Me.graph2.Series.Add(Me.barSeries1)
        Me.graph2.SeriesGroups.Add(GraphGroup4)
        Me.graph2.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(14.499902725219727R), Telerik.Reporting.Drawing.Unit.Cm(5.5R))
        Me.graph2.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        Me.graph2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        Me.graph2.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        Me.graph2.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        GraphTitle2.Position = Telerik.Reporting.GraphItemPosition.TopLeft
        GraphTitle2.Style.Color = System.Drawing.Color.FromArgb(CType(CType(152, Byte), Integer), CType(CType(152, Byte), Integer), CType(CType(152, Byte), Integer))
        GraphTitle2.Style.Font.Bold = False
        GraphTitle2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        GraphTitle2.Style.LineColor = System.Drawing.Color.LightGray
        GraphTitle2.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0.0R)
        GraphTitle2.Text = "SALES VOLUME BY PRODUCT CATEGORY (in millions USD)"
        Me.graph2.Titles.Add(GraphTitle2)
        '
        'cartesianCoordinateSystem2
        '
        Me.cartesianCoordinateSystem2.Name = "cartesianCoordinateSystem2"
        Me.cartesianCoordinateSystem2.XAxis = Me.graphAxis3
        Me.cartesianCoordinateSystem2.YAxis = Me.graphAxis4
        '
        'graphAxis3
        '
        Me.graphAxis3.LabelFormat = "${0}M"
        Me.graphAxis3.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis3.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis3.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis3.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis3.MinorGridLineStyle.Visible = False
        Me.graphAxis3.Name = "graphAxis3"
        Me.graphAxis3.Scale = NumericalScale2
        Me.graphAxis3.Title = ""
        Me.graphAxis3.TitlePlacement = Telerik.Reporting.GraphAxisTitlePlacement.AtMaximum
        '
        'graphAxis4
        '
        Me.graphAxis4.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis4.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis4.MajorGridLineStyle.Visible = False
        Me.graphAxis4.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis4.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis4.MinorGridLineStyle.Visible = False
        Me.graphAxis4.Name = "graphAxis4"
        Me.graphAxis4.Scale = CategoryScale1
        Me.graphAxis4.Size = Telerik.Reporting.Drawing.Unit.Cm(2.5R)
        Me.graphAxis4.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0.0R)
        '
        'barSeries1
        '
        Me.barSeries1.ArrangeMode = Telerik.Reporting.GraphSeriesArrangeMode.Stacked
        Me.barSeries1.CategoryGroup = GraphGroup3
        Me.barSeries1.CoordinateSystem = Me.cartesianCoordinateSystem2
        Me.barSeries1.DataPointLabel = "=Sum(Fields.LineTotal)"
        Me.barSeries1.DataPointLabelStyle.Visible = False
        Me.barSeries1.DataPointStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0.0R)
        Me.barSeries1.DataPointStyle.Visible = True
        Me.barSeries1.LegendItem.Style.BackgroundColor = System.Drawing.Color.Transparent
        Me.barSeries1.LegendItem.Style.LineColor = System.Drawing.Color.Transparent
        Me.barSeries1.LegendItem.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0.0R)
        Me.barSeries1.LegendItem.Value = "=Fields.OrderDate.Year"
        Me.barSeries1.Name = "barSeries1"
        GraphGroup4.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.OrderDate.Year"))
        GraphGroup4.Name = "orderDateYearGroup1"
        GraphGroup4.Sortings.Add(New Telerik.Reporting.Sorting("=Fields.OrderDate.Year", Telerik.Reporting.SortDirection.Asc))
        Me.barSeries1.SeriesGroup = GraphGroup4
        Me.barSeries1.X = "=Sum(Fields.LineTotal)/1e6"
        Me.barSeries1.ToolTip.Title = "=Format('Year {0}', Fields.OrderDate.Year)"
        Me.barSeries1.ToolTip.Text = "=Format('${0:0.00}M', Sum(Fields.LineTotal)/1e6)"
        '
        'graph3
        '
        GraphGroup5.Filters.Add(New Telerik.Reporting.Filter("=Sum(Fields.LineTotal)", Telerik.Reporting.FilterOperator.TopN, "=CInt(Parameters.topN.Value)"))
        GraphGroup5.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.State"))
        GraphGroup5.Name = "stateGroup1"
        GraphGroup5.Sortings.Add(New Telerik.Reporting.Sorting("=Sum(Fields.LineTotal)", Telerik.Reporting.SortDirection.Asc))
        Me.graph3.CategoryGroups.Add(GraphGroup5)
        ColorPalette4.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(210, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(77, Byte), Integer)))
        ColorPalette4.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(210, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(162, Byte), Integer)))
        ColorPalette4.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(210, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(150, Byte), Integer), CType(CType(70, Byte), Integer)))
        ColorPalette4.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(210, Byte), Integer), CType(CType(119, Byte), Integer), CType(CType(44, Byte), Integer), CType(CType(42, Byte), Integer)))
        ColorPalette4.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(210, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(59, Byte), Integer), CType(CType(98, Byte), Integer)))
        ColorPalette4.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(210, Byte), Integer), CType(CType(182, Byte), Integer), CType(CType(87, Byte), Integer), CType(CType(8, Byte), Integer)))
        Me.graph3.ColorPalette = ColorPalette4
        Me.graph3.CoordinateSystems.Add(Me.cartesianCoordinateSystem3)
        Me.graph3.DataSource = Me.salesData
        Me.graph3.Filters.Add(New Telerik.Reporting.Filter("=Fields.Country", Telerik.Reporting.FilterOperator.[In], "=@country"))
        Me.graph3.Filters.Add(New Telerik.Reporting.Filter("=Fields.OrderDate.Year", Telerik.Reporting.FilterOperator.[In], "=@year"))
        Me.graph3.Legend.IsInsidePlotArea = False
        Me.graph3.Legend.Position = Telerik.Reporting.GraphItemPosition.BottomLeft
        Me.graph3.Legend.Style.LineColor = System.Drawing.Color.LightGray
        Me.graph3.Legend.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0.0R)
        Me.graph3.Legend.Style.Visible = True
        Me.graph3.Legend.Width = Telerik.Reporting.Drawing.Unit.Cm(3.0R)
        Me.graph3.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(15.0R), Telerik.Reporting.Drawing.Unit.Cm(3.000300407409668R))
        Me.graph3.Name = "graph3"
        Me.graph3.PlotAreaStyle.LineColor = System.Drawing.Color.LightGray
        Me.graph3.PlotAreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0.0R)
        Me.graph3.Series.Add(Me.barSeries2)
        Me.graph3.SeriesGroups.Add(GraphGroup6)
        Me.graph3.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(14.500203132629395R), Telerik.Reporting.Drawing.Unit.Cm(11.499997138977051R))
        Me.graph3.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        Me.graph3.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        Me.graph3.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        GraphTitle3.Position = Telerik.Reporting.GraphItemPosition.TopLeft
        GraphTitle3.Style.Color = System.Drawing.Color.FromArgb(CType(CType(152, Byte), Integer), CType(CType(152, Byte), Integer), CType(CType(152, Byte), Integer))
        GraphTitle3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        GraphTitle3.Style.LineColor = System.Drawing.Color.LightGray
        GraphTitle3.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0.0R)
        GraphTitle3.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(26.0R)
        GraphTitle3.Text = "TOP {Parameters.topN.Value} STATES"
        Me.graph3.Titles.Add(GraphTitle3)
        '
        'cartesianCoordinateSystem3
        '
        Me.cartesianCoordinateSystem3.Name = "cartesianCoordinateSystem3"
        Me.cartesianCoordinateSystem3.XAxis = Me.graphAxis5
        Me.cartesianCoordinateSystem3.YAxis = Me.graphAxis6
        '
        'graphAxis5
        '
        Me.graphAxis5.LabelFormat = "${0}M"
        Me.graphAxis5.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis5.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis5.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis5.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis5.MinorGridLineStyle.Visible = False
        Me.graphAxis5.Name = "graphAxis5"
        Me.graphAxis5.Scale = NumericalScale3
        '
        'graphAxis6
        '
        Me.graphAxis6.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis6.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis6.MajorGridLineStyle.Visible = False
        Me.graphAxis6.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis6.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis6.MinorGridLineStyle.Visible = False
        Me.graphAxis6.MinSize = Telerik.Reporting.Drawing.Unit.Cm(2.7999999523162842R)
        Me.graphAxis6.Name = "graphAxis6"
        CategoryScale2.SpacingSlotCount = 0.5R
        Me.graphAxis6.Scale = CategoryScale2
        Me.graphAxis6.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(0.0R)
        Me.graphAxis6.Style.Visible = True
        '
        'barSeries2
        '
        Me.barSeries2.ArrangeMode = Telerik.Reporting.GraphSeriesArrangeMode.Stacked
        Me.barSeries2.CategoryGroup = GraphGroup5
        Me.barSeries2.CoordinateSystem = Me.cartesianCoordinateSystem3
        Me.barSeries2.DataPointLabel = "=Sum(Fields.LineTotal)/1e6"
        Me.barSeries2.DataPointLabelStyle.Visible = False
        Me.barSeries2.DataPointStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0.0R)
        Me.barSeries2.DataPointStyle.Visible = True
        Me.barSeries2.LegendItem.Style.BackgroundColor = System.Drawing.Color.Transparent
        Me.barSeries2.LegendItem.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8.0R)
        Me.barSeries2.LegendItem.Style.LineColor = System.Drawing.Color.Transparent
        Me.barSeries2.LegendItem.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0.0R)
        Me.barSeries2.LegendItem.Value = "=Fields.ProductSubCategory"
        Me.barSeries2.Name = "barSeries2"
        GraphGroup6.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.ProductSubCategory"))
        GraphGroup6.Name = "productCategoryGroup1"
        GraphGroup6.Sortings.Add(New Telerik.Reporting.Sorting("=Fields.ProductSubCategory", Telerik.Reporting.SortDirection.Asc))
        Me.barSeries2.SeriesGroup = GraphGroup6
        Me.barSeries2.X = "=Sum(Fields.LineTotal)/1e6"
        Me.barSeries2.ToolTip.Title = "=Format('Sales in {0}, {1}', Fields.State, Fields.Country)"
        Me.barSeries2.ToolTip.Text = "=Format('{0}: ${1:0.00}M', Fields.ProductSubCategory, Sum(Fields.LineTotal)/1e6)"
        '
        'textBox1
        '
        Me.textBox1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.19688987731933594R), Telerik.Reporting.Drawing.Unit.Inch(0.19688980281352997R))
        Me.textBox1.Name = "textBox1"
        Me.textBox1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(11.417283058166504R), Telerik.Reporting.Drawing.Unit.Inch(0.590511679649353R))
        Me.textBox1.Style.Font.Bold = True
        Me.textBox1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(25.0R)
        Me.textBox1.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(34.0R)
        Me.textBox1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        Me.textBox1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        Me.textBox1.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.textBox1.StyleName = "Header"
        Me.textBox1.Value = "{Parameters.category.Label} Sales Distribution By Region"
        '
        'textBox2
        '
        Me.textBox2.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.49959805607795715R), Telerik.Reporting.Drawing.Unit.Cm(2.0001997947692871R))
        Me.textBox2.Name = "textBox2"
        Me.textBox2.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(29.000308990478516R), Telerik.Reporting.Drawing.Unit.Cm(0.99989980459213257R))
        Me.textBox2.Style.Font.Bold = False
        Me.textBox2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(14.0R)
        Me.textBox2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        Me.textBox2.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        Me.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left
        Me.textBox2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.textBox2.Value = "REGION: {Join(', ', Parameters.country.Value)}" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "PERIOD: {Join(', ', Parameters.ye" & _
    "ar.Value)}"
        '
        'SalesByRegionDashboard
        '
        Me.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.detailSection1})
        Me.Name = "SalesByRegionDashboard"
        Me.PageSettings.Landscape = False
        Me.PageSettings.Margins = New Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(1.0R), Telerik.Reporting.Drawing.Unit.Inch(1.0R), Telerik.Reporting.Drawing.Unit.Inch(1.0R), Telerik.Reporting.Drawing.Unit.Inch(1.0R))
        Me.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Custom
        Me.PageSettings.PaperSize = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(14.0R), Telerik.Reporting.Drawing.Unit.Mm(297.0R))
        ReportParameter1.AutoRefresh = True
        ReportParameter1.AvailableValues.DataSource = Me.yearsData
        ReportParameter1.AvailableValues.ValueMember = "= Fields.Year"
        ReportParameter1.MultiValue = True
        ReportParameter1.Name = "year"
        ReportParameter1.Text = "Year"
        ReportParameter1.Type = Telerik.Reporting.ReportParameterType.[Integer]
        ReportParameter1.Value = "=Array(2003, 2004)"
        ReportParameter1.Visible = True
        ReportParameter2.AutoRefresh = True
        ReportParameter2.AvailableValues.DataSource = Me.countryData
        ReportParameter2.AvailableValues.ValueMember = "= Fields.Country"
        ReportParameter2.MultiValue = True
        ReportParameter2.Name = "country"
        ReportParameter2.Text = "Country"
        ReportParameter2.Value = "=Array('United States', 'Canada')"
        ReportParameter2.Visible = True
        ReportParameter3.AutoRefresh = True
        ReportParameter3.AvailableValues.DataSource = Me.productCategoriesData
        ReportParameter3.AvailableValues.DisplayMember = "= Fields.Name"
        ReportParameter3.AvailableValues.ValueMember = "= Fields.ProductCategoryId"
        ReportParameter3.Name = "category"
        ReportParameter3.Text = "Category"
        ReportParameter3.Value = "=1"
        ReportParameter4.AutoRefresh = True
        ReportParameter4.Name = "topN"
        ReportParameter4.Text = "Top ¹ States"
        ReportParameter4.Type = Telerik.Reporting.ReportParameterType.[Integer]
        ReportParameter4.Value = "12"
        Me.ReportParameters.Add(ReportParameter1)
        Me.ReportParameters.Add(ReportParameter2)
        Me.ReportParameters.Add(ReportParameter3)
        Me.ReportParameters.Add(ReportParameter4)
        Me.Style.BackgroundColor = System.Drawing.Color.White
        Me.Style.Font.Name = "Segoe UI Light"
        StyleRule1.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.TextItemBase)), New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.HtmlTextBox))})
        StyleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        StyleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        StyleRule2.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.TextItemBase)), New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.HtmlTextBox))})
        StyleRule2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        StyleRule2.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        Me.StyleSheet.AddRange(New Telerik.Reporting.Drawing.StyleRule() {StyleRule1, StyleRule2})
        Me.TocText = "Sales By Region"
        Me.Width = Telerik.Reporting.Drawing.Unit.Inch(11.81102466583252R)
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Friend WithEvents yearsData As Telerik.Reporting.SqlDataSource
    Friend WithEvents countryData As Telerik.Reporting.SqlDataSource
    Friend WithEvents productCategoriesData As Telerik.Reporting.SqlDataSource
    Friend WithEvents salesData As Telerik.Reporting.SqlDataSource
    Friend WithEvents detailSection1 As Telerik.Reporting.DetailSection
    Friend WithEvents map1 As Telerik.Reporting.Map
    Friend WithEvents PieMapSeries1 As Telerik.Reporting.PieMapSeries
    Friend WithEvents graph1 As Telerik.Reporting.Graph
    Friend WithEvents cartesianCoordinateSystem1 As Telerik.Reporting.CartesianCoordinateSystem
    Friend WithEvents graphAxis2 As Telerik.Reporting.GraphAxis
    Friend WithEvents graphAxis1 As Telerik.Reporting.GraphAxis
    Friend WithEvents lineSeries1 As Telerik.Reporting.LineSeries
    Friend WithEvents graph2 As Telerik.Reporting.Graph
    Friend WithEvents cartesianCoordinateSystem2 As Telerik.Reporting.CartesianCoordinateSystem
    Friend WithEvents graphAxis3 As Telerik.Reporting.GraphAxis
    Friend WithEvents graphAxis4 As Telerik.Reporting.GraphAxis
    Friend WithEvents barSeries1 As Telerik.Reporting.BarSeries
    Friend WithEvents graph3 As Telerik.Reporting.Graph
    Friend WithEvents cartesianCoordinateSystem3 As Telerik.Reporting.CartesianCoordinateSystem
    Friend WithEvents graphAxis5 As Telerik.Reporting.GraphAxis
    Friend WithEvents graphAxis6 As Telerik.Reporting.GraphAxis
    Friend WithEvents barSeries2 As Telerik.Reporting.BarSeries
    Friend WithEvents textBox1 As Telerik.Reporting.TextBox
    Friend WithEvents textBox2 As Telerik.Reporting.TextBox
End Class