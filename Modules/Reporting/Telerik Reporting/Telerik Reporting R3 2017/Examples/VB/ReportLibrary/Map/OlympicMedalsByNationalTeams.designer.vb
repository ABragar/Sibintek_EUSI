Partial Class OlympicMedalsByNationalTeams
    
    'NOTE: The following procedure is required by the telerik Reporting Designer
    'It can be modified using the telerik Reporting Designer.  
    'Do not modify it using the code editor.
    Private Sub InitializeComponent()
        Dim FormattingRule1 As Telerik.Reporting.Drawing.FormattingRule = New Telerik.Reporting.Drawing.FormattingRule()
        Dim FormattingRule2 As Telerik.Reporting.Drawing.FormattingRule = New Telerik.Reporting.Drawing.FormattingRule()
        Dim FormattingRule3 As Telerik.Reporting.Drawing.FormattingRule = New Telerik.Reporting.Drawing.FormattingRule()
        Dim NavigateToReportAction1 As Telerik.Reporting.NavigateToReportAction = New Telerik.Reporting.NavigateToReportAction()
        Dim TypeReportSource1 As Telerik.Reporting.TypeReportSource = New Telerik.Reporting.TypeReportSource()
        Dim FormattingRule4 As Telerik.Reporting.Drawing.FormattingRule = New Telerik.Reporting.Drawing.FormattingRule()
        Dim DataColumn1 As Telerik.Reporting.DataColumn = New Telerik.Reporting.DataColumn()
        Dim DataColumn2 As Telerik.Reporting.DataColumn = New Telerik.Reporting.DataColumn()
        Dim ColorPalette1 As Telerik.Reporting.Drawing.ColorPalette = New Telerik.Reporting.Drawing.ColorPalette()
        Dim GeoLocationMapGroup1 As Telerik.Reporting.GeoLocationMapGroup = New Telerik.Reporting.GeoLocationMapGroup()
        Dim MapLegend1 As Telerik.Reporting.MapLegend = New Telerik.Reporting.MapLegend()
        Dim MercatorProjection1 As Telerik.Reporting.MercatorProjection = New Telerik.Reporting.MercatorProjection()
        Dim GenericTileProvider1 As Telerik.Reporting.GenericTileProvider = New Telerik.Reporting.GenericTileProvider()
        Dim MapTitle1 As Telerik.Reporting.MapTitle = New Telerik.Reporting.MapTitle()
        Dim DataColumn3 As Telerik.Reporting.DataColumn = New Telerik.Reporting.DataColumn()
        Dim DataColumn4 As Telerik.Reporting.DataColumn = New Telerik.Reporting.DataColumn()
        Dim DataColumn5 As Telerik.Reporting.DataColumn = New Telerik.Reporting.DataColumn()
        Dim DataColumn6 As Telerik.Reporting.DataColumn = New Telerik.Reporting.DataColumn()
        Dim DataColumn7 As Telerik.Reporting.DataColumn = New Telerik.Reporting.DataColumn()
        Dim DataColumn8 As Telerik.Reporting.DataColumn = New Telerik.Reporting.DataColumn()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(OlympicMedalsByNationalTeams))
        Dim ColorPalette2 As Telerik.Reporting.Drawing.ColorPalette = New Telerik.Reporting.Drawing.ColorPalette()
        Dim FormattingRule5 As Telerik.Reporting.Drawing.FormattingRule = New Telerik.Reporting.Drawing.FormattingRule()
        Dim FormattingRule6 As Telerik.Reporting.Drawing.FormattingRule = New Telerik.Reporting.Drawing.FormattingRule()
        Dim MapGroup1 As Telerik.Reporting.MapGroup = New Telerik.Reporting.MapGroup()
        Dim MapGroup2 As Telerik.Reporting.MapGroup = New Telerik.Reporting.MapGroup()
        Dim TableGroup1 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup2 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup3 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup4 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim FormattingRule7 As Telerik.Reporting.Drawing.FormattingRule = New Telerik.Reporting.Drawing.FormattingRule()
        Dim FormattingRule8 As Telerik.Reporting.Drawing.FormattingRule = New Telerik.Reporting.Drawing.FormattingRule()
        Dim GraphGroup1 As Telerik.Reporting.GraphGroup = New Telerik.Reporting.GraphGroup()
        Dim ColorPalette3 As Telerik.Reporting.Drawing.ColorPalette = New Telerik.Reporting.Drawing.ColorPalette()
        Dim GraphTitle1 As Telerik.Reporting.GraphTitle = New Telerik.Reporting.GraphTitle()
        Dim CategoryScale1 As Telerik.Reporting.CategoryScale = New Telerik.Reporting.CategoryScale()
        Dim NumericalScale1 As Telerik.Reporting.NumericalScale = New Telerik.Reporting.NumericalScale()
        Dim GraphGroup2 As Telerik.Reporting.GraphGroup = New Telerik.Reporting.GraphGroup()
        Dim ReportParameter1 As Telerik.Reporting.ReportParameter = New Telerik.Reporting.ReportParameter()
        Dim ReportParameter2 As Telerik.Reporting.ReportParameter = New Telerik.Reporting.ReportParameter()
        Dim StyleRule1 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule2 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule3 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule4 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim DescendantSelector1 As Telerik.Reporting.Drawing.DescendantSelector = New Telerik.Reporting.Drawing.DescendantSelector()
        Dim StyleRule5 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim DescendantSelector2 As Telerik.Reporting.Drawing.DescendantSelector = New Telerik.Reporting.Drawing.DescendantSelector()
        Dim StyleRule6 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim DescendantSelector3 As Telerik.Reporting.Drawing.DescendantSelector = New Telerik.Reporting.Drawing.DescendantSelector()
        Dim StyleRule7 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim DescendantSelector4 As Telerik.Reporting.Drawing.DescendantSelector = New Telerik.Reporting.Drawing.DescendantSelector()
        Dim StyleRule8 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule9 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim DescendantSelector5 As Telerik.Reporting.Drawing.DescendantSelector = New Telerik.Reporting.Drawing.DescendantSelector()
        Dim StyleRule10 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim DescendantSelector6 As Telerik.Reporting.Drawing.DescendantSelector = New Telerik.Reporting.Drawing.DescendantSelector()
        Dim StyleRule11 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim DescendantSelector7 As Telerik.Reporting.Drawing.DescendantSelector = New Telerik.Reporting.Drawing.DescendantSelector()
        Dim StyleRule12 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim DescendantSelector8 As Telerik.Reporting.Drawing.DescendantSelector = New Telerik.Reporting.Drawing.DescendantSelector()
        Me.textBox1 = New Telerik.Reporting.TextBox()
        Me.textBox2 = New Telerik.Reporting.TextBox()
        Me.textBox3 = New Telerik.Reporting.TextBox()
        Me.textBox4 = New Telerik.Reporting.TextBox()
        Me.associationsData = New Telerik.Reporting.CsvDataSource()
        Me.detailSection1 = New Telerik.Reporting.DetailSection()
        Me.map1 = New Telerik.Reporting.Map()
        Me.medalsData = New Telerik.Reporting.CsvDataSource()
        Me.PointMapSeries1 = New Telerik.Reporting.PointMapSeries()
        Me.columnMapSeries1 = New Telerik.Reporting.ColumnMapSeries()
        Me.crosstab1 = New Telerik.Reporting.Crosstab()
        Me.textBox6 = New Telerik.Reporting.TextBox()
        Me.textBox7 = New Telerik.Reporting.TextBox()
        Me.textBox8 = New Telerik.Reporting.TextBox()
        Me.textBox9 = New Telerik.Reporting.TextBox()
        Me.textBox5 = New Telerik.Reporting.TextBox()
        Me.graph1 = New Telerik.Reporting.Graph()
        Me.cartesianCoordinateSystem1 = New Telerik.Reporting.CartesianCoordinateSystem()
        Me.graphAxis2 = New Telerik.Reporting.GraphAxis()
        Me.GraphAxis1 = New Telerik.Reporting.GraphAxis()
        Me.barSeries1 = New Telerik.Reporting.BarSeries()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'textBox1
        '
        FormattingRule1.Filters.Add(New Telerik.Reporting.Filter("=Fields.Place", Telerik.Reporting.FilterOperator.Equal, "=1"))
        FormattingRule1.Style.BackgroundColor = System.Drawing.Color.White
        FormattingRule1.Style.Color = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(184, Byte), Integer), CType(CType(1, Byte), Integer))
        FormattingRule2.Filters.Add(New Telerik.Reporting.Filter("=Fields.Place", Telerik.Reporting.FilterOperator.Equal, "=2"))
        FormattingRule2.Style.BackgroundColor = System.Drawing.Color.White
        FormattingRule2.Style.Color = System.Drawing.Color.DimGray
        FormattingRule2.Style.Font.Bold = False
        FormattingRule3.Filters.Add(New Telerik.Reporting.Filter("=Fields.Place", Telerik.Reporting.FilterOperator.Equal, "=3"))
        FormattingRule3.Style.BackgroundColor = System.Drawing.Color.White
        FormattingRule3.Style.Color = System.Drawing.Color.DarkGoldenrod
        Me.textBox1.ConditionalFormatting.AddRange(New Telerik.Reporting.Drawing.FormattingRule() {FormattingRule1, FormattingRule2, FormattingRule3})
        Me.textBox1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0R), Telerik.Reporting.Drawing.Unit.Cm(0R))
        Me.textBox1.Name = "textBox1"
        Me.textBox1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.5766671895980835R), Telerik.Reporting.Drawing.Unit.Cm(0.58728313446044922R))
        Me.textBox1.Style.BorderColor.Bottom = System.Drawing.Color.Gray
        Me.textBox1.Style.BorderWidth.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.textBox1.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Cm(0R)
        Me.textBox1.Style.Font.Name = "Segoe UI"
        Me.textBox1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10.0R)
        Me.textBox1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.textBox1.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.textBox1.StyleName = "Normal.TableHeader"
        Me.textBox1.Value = "= Fields.Medal"
        '
        'textBox2
        '
        Me.textBox2.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0R), Telerik.Reporting.Drawing.Unit.Cm(0R))
        Me.textBox2.Name = "textBox2"
        Me.textBox2.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.4708322286605835R), Telerik.Reporting.Drawing.Unit.Cm(0.58728313446044922R))
        Me.textBox2.Style.BorderColor.Bottom = System.Drawing.Color.Gray
        Me.textBox2.Style.BorderWidth.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.textBox2.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Cm(0R)
        Me.textBox2.Style.Font.Bold = False
        Me.textBox2.Style.Font.Name = "Segoe UI"
        Me.textBox2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10.0R)
        Me.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.textBox2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.textBox2.StyleName = "Normal.GrandTotal"
        Me.textBox2.Value = "Total"
        '
        'textBox3
        '
        TypeReportSource1.Parameters.Add(New Telerik.Reporting.Parameter("associationParam", "=Parameters.associationParam.Value"))
        TypeReportSource1.Parameters.Add(New Telerik.Reporting.Parameter("teamParam", "=ReportItem.Text"))
        TypeReportSource1.TypeName = "OlympicMedalsByNationalTeams, VB.ReportLibrary, Version=1.0.0.0, Culture=neutral," &
    " PublicKeyToken=null"
        NavigateToReportAction1.ReportSource = TypeReportSource1
        Me.textBox3.Action = NavigateToReportAction1
        FormattingRule4.Filters.Add(New Telerik.Reporting.Filter("=Fields.Team", Telerik.Reporting.FilterOperator.Equal, "=Parameters.teamParam.Value"))
        FormattingRule4.Style.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(202, Byte), Integer))
        Me.textBox3.ConditionalFormatting.AddRange(New Telerik.Reporting.Drawing.FormattingRule() {FormattingRule4})
        Me.textBox3.Name = "textBox3"
        Me.textBox3.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.0583336353302R), Telerik.Reporting.Drawing.Unit.Cm(0.617507815361023R))
        Me.textBox3.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Cm(0R)
        Me.textBox3.Style.Font.Name = "Segoe UI"
        Me.textBox3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        Me.textBox3.Style.Font.Underline = True
        Me.textBox3.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(5.0R)
        Me.textBox3.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Cm(0R)
        Me.textBox3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.textBox3.StyleName = ""
        Me.textBox3.Value = "=Fields.Team"
        '
        'textBox4
        '
        Me.textBox4.Name = "textBox4"
        Me.textBox4.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.0583338737487793R), Telerik.Reporting.Drawing.Unit.Cm(0.64229214191436768R))
        Me.textBox4.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Cm(0R)
        Me.textBox4.Style.Font.Name = "Segoe UI"
        Me.textBox4.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(5.0R)
        Me.textBox4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.textBox4.Style.Visible = False
        Me.textBox4.StyleName = ""
        Me.textBox4.Value = "Total"
        '
        'associationsData
        '
        DataColumn1.Name = "Id"
        DataColumn1.Type = Telerik.Reporting.SimpleType.[Integer]
        DataColumn2.Name = "Name"
        Me.associationsData.Columns.Add(DataColumn1)
        Me.associationsData.Columns.Add(DataColumn2)
        Me.associationsData.FieldSeparators = New Char() {Global.Microsoft.VisualBasic.ChrW(44), Global.Microsoft.VisualBasic.ChrW(59)}
        Me.associationsData.HasHeaders = True
        Me.associationsData.Name = "associationsData"
        Me.associationsData.RecordSeparators = New Char() {Global.Microsoft.VisualBasic.ChrW(13), Global.Microsoft.VisualBasic.ChrW(10)}
        Me.associationsData.Source = "Id;Name" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "1;Pan-American Sports Organization" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "2;European Olympic Committees" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "3;Oly" &
    "mpic Council of Asia" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "4;Oceania National Olympic Committees" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "5;Association of Na" &
    "tional Olympic Committees of Africa"
        '
        'detailSection1
        '
        Me.detailSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(18.539999008178711R)
        Me.detailSection1.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.map1, Me.crosstab1, Me.graph1})
        Me.detailSection1.Name = "detailSection1"
        '
        'map1
        '
        ColorPalette1.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(204, Byte), Integer), CType(CType(51, Byte), Integer)))
        ColorPalette1.Colors.Add(System.Drawing.Color.Silver)
        ColorPalette1.Colors.Add(System.Drawing.Color.DarkGoldenrod)
        Me.map1.ColorPalette = ColorPalette1
        Me.map1.DataSource = Me.medalsData
        GeoLocationMapGroup1.Filters.Add(New Telerik.Reporting.Filter("=Fields.AssociationId", Telerik.Reporting.FilterOperator.[In], "=Parameters.associationParam.Value"))
        GeoLocationMapGroup1.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.Team"))
        GeoLocationMapGroup1.Name = "teamGroup"
        GeoLocationMapGroup1.Sortings.Add(New Telerik.Reporting.Sorting("=Fields.Team", Telerik.Reporting.SortDirection.Asc))
        Me.map1.GeoLocationGroups.Add(GeoLocationMapGroup1)
        MapLegend1.Position = Telerik.Reporting.GraphItemPosition.TopRight
        MapLegend1.Style.Font.Name = "Segoe UI"
        MapLegend1.Style.LineColor = System.Drawing.Color.LightGray
        MapLegend1.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0R)
        MapLegend1.TitleStyle.Font.Bold = True
        Me.map1.Legends.Add(MapLegend1)
        Me.map1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0R), Telerik.Reporting.Drawing.Unit.Cm(0R))
        Me.map1.Meridians.Style.LineColor = System.Drawing.Color.LightGray
        Me.map1.Name = "map1"
        Me.map1.Parallels.Style.LineColor = System.Drawing.Color.LightGray
        Me.map1.PlotAreaStyle.LineColor = System.Drawing.Color.LightGray
        Me.map1.PlotAreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0R)
        Me.map1.Projection = MercatorProjection1
        Me.map1.ScaleLegend.ItemStyle.BorderColor.Default = System.Drawing.Color.White
        Me.map1.ScaleLegend.ItemStyle.LineColor = System.Drawing.Color.DodgerBlue
        Me.map1.ScaleLegend.ItemStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(2.0R)
        Me.map1.ScaleLegend.ItemStyle.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(5.0R)
        Me.map1.ScaleLegend.ItemStyle.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(5.0R)
        Me.map1.ScaleLegend.ItemStyle.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.map1.ScaleLegend.ScaleUnits = CType((Telerik.Reporting.DistanceUnitType.Metric Or Telerik.Reporting.DistanceUnitType.Imperial), Telerik.Reporting.DistanceUnitType)
        Me.map1.ScaleLegend.Style.Font.Name = "Segoe UI"
        Me.map1.ScaleLegend.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8.0R)
        Me.map1.ScaleLegend.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom
        Me.map1.Series.Add(Me.PointMapSeries1)
        Me.map1.Series.Add(Me.columnMapSeries1)
        Me.map1.SeriesGroups.Add(MapGroup1)
        Me.map1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(18.540000915527344R), Telerik.Reporting.Drawing.Unit.Cm(18.540000915527344R))
        Me.map1.Style.Font.Name = "Segoe UI Light"
        GenericTileProvider1.Attribution = "© OpenStreetMap contributors"
        GenericTileProvider1.LogoUrl = "http://wiki.openstreetmap.org/w/images/thumb/7/79/Public-images-osm_logo.svg/32px" &
    "-Public-images-osm_logo.svg.png"
        GenericTileProvider1.UrlSubdomains.Add("a")
        GenericTileProvider1.UrlSubdomains.Add("b")
        GenericTileProvider1.UrlSubdomains.Add("c")
        GenericTileProvider1.UrlTemplate = "http://{subdomain}.tile.openstreetmap.org/{zoom}/{X}/{Y}.png"
        Me.map1.TileProvider = GenericTileProvider1
        MapTitle1.Position = Telerik.Reporting.GraphItemPosition.TopLeft
        MapTitle1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(14.0R)
        MapTitle1.Style.LineColor = System.Drawing.Color.LightGray
        MapTitle1.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0R)
        MapTitle1.Text = "Countries with the most medals in the Olympic Games"
        Me.map1.Titles.Add(MapTitle1)
        '
        'medalsData
        '
        Me.medalsData.CalculatedFields.AddRange(New Telerik.Reporting.CalculatedField() {New Telerik.Reporting.CalculatedField("Medal", GetType(String), "=IIF(Fields.Place = 1, 'Gold', " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "IIF(Fields.Place = 2, 'Silver', " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "IIF(Fields.Pla" &
                    "ce = 3, 'Bronze', 'none')))")})
        DataColumn3.Name = "Team"
        DataColumn4.Name = "Latitude"
        DataColumn4.Type = Telerik.Reporting.SimpleType.[Decimal]
        DataColumn5.Name = "Longitude"
        DataColumn5.Type = Telerik.Reporting.SimpleType.[Decimal]
        DataColumn6.Name = "AssociationId"
        DataColumn6.Type = Telerik.Reporting.SimpleType.[Integer]
        DataColumn7.Name = "Place"
        DataColumn7.Type = Telerik.Reporting.SimpleType.[Integer]
        DataColumn8.Name = "Count"
        DataColumn8.Type = Telerik.Reporting.SimpleType.[Integer]
        Me.medalsData.Columns.Add(DataColumn3)
        Me.medalsData.Columns.Add(DataColumn4)
        Me.medalsData.Columns.Add(DataColumn5)
        Me.medalsData.Columns.Add(DataColumn6)
        Me.medalsData.Columns.Add(DataColumn7)
        Me.medalsData.Columns.Add(DataColumn8)
        Me.medalsData.FieldSeparators = New Char() {Global.Microsoft.VisualBasic.ChrW(44)}
        Me.medalsData.HasHeaders = True
        Me.medalsData.Name = "medalsData"
        Me.medalsData.RecordSeparators = New Char() {Global.Microsoft.VisualBasic.ChrW(13), Global.Microsoft.VisualBasic.ChrW(10)}
        Me.medalsData.Source = resources.GetString("medalsData.Source")
        '
        'PointMapSeries1
        '
        ColorPalette2.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(200, Byte), Integer)))
        ColorPalette2.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(110, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer)))
        Me.PointMapSeries1.ColorPalette = ColorPalette2
        FormattingRule5.Filters.Add(New Telerik.Reporting.Filter("=Fields.Team", Telerik.Reporting.FilterOperator.Equal, "=Parameters.teamParam.Value"))
        FormattingRule5.Style.LineColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        FormattingRule5.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        Me.PointMapSeries1.DataPointConditionalFormatting.AddRange(New Telerik.Reporting.Drawing.FormattingRule() {FormattingRule5})
        Me.PointMapSeries1.DataPointLabel = "=Fields.Team"
        FormattingRule6.Filters.Add(New Telerik.Reporting.Filter("=Fields.Team", Telerik.Reporting.FilterOperator.Equal, "=Parameters.teamParam.Value"))
        FormattingRule6.Style.Font.Bold = True
        Me.PointMapSeries1.DataPointLabelConditionalFormatting.AddRange(New Telerik.Reporting.Drawing.FormattingRule() {FormattingRule6})
        Me.PointMapSeries1.DataPointLabelStyle.Color = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.PointMapSeries1.DataPointLabelStyle.Font.Name = "Segoe UI Symbol"
        Me.PointMapSeries1.DataPointLabelStyle.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8.0R)
        Me.PointMapSeries1.DataPointLabelStyle.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.PointMapSeries1.DataPointStyle.BackgroundColor = System.Drawing.Color.White
        Me.PointMapSeries1.DataPointStyle.LineColor = System.Drawing.Color.DarkSlateBlue
        Me.PointMapSeries1.DataPointStyle.Visible = True
        Me.PointMapSeries1.GeoLocationGroup = GeoLocationMapGroup1
        Me.PointMapSeries1.Latitude = "=Fields.Latitude"
        Me.PointMapSeries1.LegendItem.Style.BackgroundColor = System.Drawing.Color.Transparent
        Me.PointMapSeries1.LegendItem.Style.LineColor = System.Drawing.Color.Transparent
        Me.PointMapSeries1.LegendItem.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0R)
        Me.PointMapSeries1.LegendItem.Value = "=Fields.Medal"
        Me.PointMapSeries1.Longitude = "=Fields.Longitude"
        Me.PointMapSeries1.MarkerMaxSize = Telerik.Reporting.Drawing.Unit.Cm(2.0R)
        Me.PointMapSeries1.MarkerMinSize = Telerik.Reporting.Drawing.Unit.Cm(1.0R)
        Me.PointMapSeries1.MarkerType = Telerik.Reporting.DataPointMarkerType.Circle
        MapGroup2.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.Place"))
        MapGroup2.Name = "medalGroup"
        MapGroup2.Sortings.Add(New Telerik.Reporting.Sorting("=Fields.Place", Telerik.Reporting.SortDirection.Asc))
        MapGroup1.ChildGroups.Add(MapGroup2)
        MapGroup1.Name = "seriesGroup"
        Me.PointMapSeries1.SeriesGroup = MapGroup1
        Me.PointMapSeries1.Size = "=Sum(Fields.Count)"
        '
        'columnMapSeries1
        '
        Me.columnMapSeries1.DataPointStyle.Visible = True
        Me.columnMapSeries1.GeoLocationGroup = GeoLocationMapGroup1
        Me.columnMapSeries1.Latitude = "=Fields.Latitude"
        Me.columnMapSeries1.LegendItem.MarkStyle.LineColor = System.Drawing.Color.LightSlateGray
        Me.columnMapSeries1.LegendItem.MarkStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.columnMapSeries1.LegendItem.Style.BackgroundColor = System.Drawing.Color.Transparent
        Me.columnMapSeries1.LegendItem.Style.LineColor = System.Drawing.Color.Transparent
        Me.columnMapSeries1.LegendItem.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0R)
        Me.columnMapSeries1.LegendItem.Value = "= Fields.Medal"
        Me.columnMapSeries1.Longitude = "=Fields.Longitude"
        Me.columnMapSeries1.MarkerMaxSize = Telerik.Reporting.Drawing.Unit.Mm(10.0R)
        Me.columnMapSeries1.MarkerMinSize = Telerik.Reporting.Drawing.Unit.Mm(6.0R)
        Me.columnMapSeries1.MarkerType = Telerik.Reporting.DataPointMarkerType.Circle
        Me.columnMapSeries1.Name = "columnMapSeries1"
        Me.columnMapSeries1.SeriesGroup = MapGroup1
        Me.columnMapSeries1.Size = "=Sum(Fields.Count)"
        '
        'crosstab1
        '
        Me.crosstab1.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.62073510885238647R)))
        Me.crosstab1.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.57906776666641235R)))
        Me.crosstab1.Body.Rows.Add(New Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Inch(0.24311330914497376R)))
        Me.crosstab1.Body.Rows.Add(New Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Inch(0.25287094712257385R)))
        Me.crosstab1.Body.SetCellContent(0, 0, Me.textBox6)
        Me.crosstab1.Body.SetCellContent(0, 1, Me.textBox7)
        Me.crosstab1.Body.SetCellContent(1, 0, Me.textBox8)
        Me.crosstab1.Body.SetCellContent(1, 1, Me.textBox9)
        TableGroup1.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.Place"))
        TableGroup1.Name = "Medal1"
        TableGroup1.ReportItem = Me.textBox1
        TableGroup1.Sortings.Add(New Telerik.Reporting.Sorting("=Fields.Place", Telerik.Reporting.SortDirection.Asc))
        TableGroup2.ReportItem = Me.textBox2
        Me.crosstab1.ColumnGroups.Add(TableGroup1)
        Me.crosstab1.ColumnGroups.Add(TableGroup2)
        Me.crosstab1.Corner.SetCellContent(0, 0, Me.textBox5)
        Me.crosstab1.DataSource = Me.medalsData
        Me.crosstab1.Filters.Add(New Telerik.Reporting.Filter("=Fields.AssociationId", Telerik.Reporting.FilterOperator.[In], "=Parameters.associationParam.Value"))
        Me.crosstab1.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.textBox5, Me.textBox6, Me.textBox7, Me.textBox8, Me.textBox9, Me.textBox1, Me.textBox2, Me.textBox3, Me.textBox4})
        Me.crosstab1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(18.799999237060547R), Telerik.Reporting.Drawing.Unit.Pixel(48.0R))
        Me.crosstab1.Name = "crosstab1"
        TableGroup3.Filters.Add(New Telerik.Reporting.Filter("=Fields.AssociationId", Telerik.Reporting.FilterOperator.[In], "=Parameters.associationParam.Value"))
        TableGroup3.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.Team"))
        TableGroup3.Name = "Team1"
        TableGroup3.ReportItem = Me.textBox3
        TableGroup3.Sortings.Add(New Telerik.Reporting.Sorting("=Sum(Fields.Count)", Telerik.Reporting.SortDirection.Desc))
        TableGroup4.ReportItem = Me.textBox4
        Me.crosstab1.RowGroups.Add(TableGroup3)
        Me.crosstab1.RowGroups.Add(TableGroup4)
        Me.crosstab1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.4038712978363037R), Telerik.Reporting.Drawing.Unit.Inch(0.72719806432724R))
        Me.crosstab1.StyleName = ""
        '
        'textBox6
        '
        FormattingRule7.Filters.Add(New Telerik.Reporting.Filter("=Fields.Team", Telerik.Reporting.FilterOperator.Equal, "=Parameters.teamParam.Value"))
        FormattingRule7.Style.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(202, Byte), Integer))
        Me.textBox6.ConditionalFormatting.AddRange(New Telerik.Reporting.Drawing.FormattingRule() {FormattingRule7})
        Me.textBox6.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0R), Telerik.Reporting.Drawing.Unit.Cm(0R))
        Me.textBox6.Name = "textBox6"
        Me.textBox6.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.5766671895980835R), Telerik.Reporting.Drawing.Unit.Cm(0.617507815361023R))
        Me.textBox6.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Cm(0R)
        Me.textBox6.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        Me.textBox6.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.textBox6.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.textBox6.StyleName = ""
        Me.textBox6.Value = "=Sum(Fields.Count)"
        '
        'textBox7
        '
        FormattingRule8.Filters.Add(New Telerik.Reporting.Filter("=Fields.Team", Telerik.Reporting.FilterOperator.Equal, "=Parameters.teamParam.Value"))
        FormattingRule8.Style.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(202, Byte), Integer))
        Me.textBox7.ConditionalFormatting.AddRange(New Telerik.Reporting.Drawing.FormattingRule() {FormattingRule8})
        Me.textBox7.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0R), Telerik.Reporting.Drawing.Unit.Cm(0R))
        Me.textBox7.Name = "textBox7"
        Me.textBox7.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.470832109451294R), Telerik.Reporting.Drawing.Unit.Cm(0.617507815361023R))
        Me.textBox7.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Cm(0R)
        Me.textBox7.Style.Font.Bold = False
        Me.textBox7.Style.Font.Name = "Segoe UI"
        Me.textBox7.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        Me.textBox7.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.textBox7.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.textBox7.Value = "=Sum(Fields.Count)"
        '
        'textBox8
        '
        Me.textBox8.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0R), Telerik.Reporting.Drawing.Unit.Cm(0R))
        Me.textBox8.Name = "textBox8"
        Me.textBox8.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.5766671895980835R), Telerik.Reporting.Drawing.Unit.Cm(0.64229214191436768R))
        Me.textBox8.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Cm(0R)
        Me.textBox8.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.textBox8.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.textBox8.Style.Visible = False
        Me.textBox8.StyleName = ""
        Me.textBox8.Value = "=Sum(Fields.Count)"
        '
        'textBox9
        '
        Me.textBox9.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0R), Telerik.Reporting.Drawing.Unit.Cm(0R))
        Me.textBox9.Name = "textBox9"
        Me.textBox9.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.4708322286605835R), Telerik.Reporting.Drawing.Unit.Cm(0.64229214191436768R))
        Me.textBox9.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Cm(0R)
        Me.textBox9.Style.Font.Bold = False
        Me.textBox9.Style.Font.Name = "Segoe UI"
        Me.textBox9.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.textBox9.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.textBox9.Style.Visible = False
        Me.textBox9.StyleName = ""
        Me.textBox9.Value = "=Sum(Fields.Count)"
        '
        'textBox5
        '
        Me.textBox5.Action = Nothing
        Me.textBox5.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0R), Telerik.Reporting.Drawing.Unit.Cm(0R))
        Me.textBox5.Name = "textBox5"
        Me.textBox5.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.0583338737487793R), Telerik.Reporting.Drawing.Unit.Cm(0.58728313446044922R))
        Me.textBox5.Style.BorderColor.Bottom = System.Drawing.Color.Gray
        Me.textBox5.Style.BorderWidth.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.textBox5.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Cm(0R)
        Me.textBox5.Style.Font.Name = "Segoe UI"
        Me.textBox5.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10.0R)
        Me.textBox5.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.textBox5.StyleName = "Normal.TableHeader"
        Me.textBox5.Value = "Team"
        '
        'graph1
        '
        Me.graph1.Anchoring = Telerik.Reporting.AnchoringStyles.Top
        GraphGroup1.Name = "categoryGroup"
        Me.graph1.CategoryGroups.Add(GraphGroup1)
        ColorPalette3.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(204, Byte), Integer), CType(CType(51, Byte), Integer)))
        ColorPalette3.Colors.Add(System.Drawing.Color.DarkGray)
        ColorPalette3.Colors.Add(System.Drawing.Color.DarkGoldenrod)
        Me.graph1.ColorPalette = ColorPalette3
        Me.graph1.CoordinateSystems.Add(Me.cartesianCoordinateSystem1)
        Me.graph1.DataSource = Me.medalsData
        Me.graph1.Filters.Add(New Telerik.Reporting.Filter("=Fields.Team", Telerik.Reporting.FilterOperator.Equal, "=Parameters.teamParam.Value"))
        Me.graph1.Legend.Style.LineColor = System.Drawing.Color.LightGray
        Me.graph1.Legend.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0R)
        Me.graph1.Legend.Style.Visible = False
        Me.graph1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(18.799999237060547R), Telerik.Reporting.Drawing.Unit.Cm(14.140000343322754R))
        Me.graph1.Name = "graph1"
        Me.graph1.PlotAreaStyle.LineColor = System.Drawing.Color.LightGray
        Me.graph1.PlotAreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0R)
        Me.graph1.Series.Add(Me.barSeries1)
        Me.graph1.SeriesGroups.Add(GraphGroup2)
        Me.graph1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.1350011825561523R), Telerik.Reporting.Drawing.Unit.Cm(4.3997998237609863R))
        Me.graph1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        GraphTitle1.Position = Telerik.Reporting.GraphItemPosition.TopLeft
        GraphTitle1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10.0R)
        GraphTitle1.Style.LineColor = System.Drawing.Color.LightGray
        GraphTitle1.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(0R)
        GraphTitle1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(20.0R)
        GraphTitle1.Text = "= Fields.Team+'''s medals'"
        Me.graph1.Titles.Add(GraphTitle1)
        '
        'cartesianCoordinateSystem1
        '
        Me.cartesianCoordinateSystem1.Name = "cartesianCoordinateSystem1"
        Me.cartesianCoordinateSystem1.XAxis = Me.graphAxis2
        Me.cartesianCoordinateSystem1.YAxis = Me.GraphAxis1
        '
        'graphAxis2
        '
        Me.graphAxis2.LabelPlacement = Telerik.Reporting.GraphAxisLabelPlacement.None
        Me.graphAxis2.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis2.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis2.MajorGridLineStyle.Visible = False
        Me.graphAxis2.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis2.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis2.MinorGridLineStyle.Visible = False
        Me.graphAxis2.Name = "graphAxis2"
        Me.graphAxis2.Scale = CategoryScale1
        Me.graphAxis2.Style.Visible = False
        '
        'GraphAxis1
        '
        Me.GraphAxis1.LabelPlacement = Telerik.Reporting.GraphAxisLabelPlacement.NextToAxis
        Me.GraphAxis1.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.GraphAxis1.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.GraphAxis1.MajorGridLineStyle.Visible = False
        Me.GraphAxis1.MajorTickMarkDisplayType = Telerik.Reporting.GraphAxisTickMarkDisplayType.None
        Me.GraphAxis1.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.GraphAxis1.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.GraphAxis1.MinorGridLineStyle.Visible = False
        Me.GraphAxis1.Name = "GraphAxis2"
        NumericalScale1.Minimum = 0R
        Me.GraphAxis1.Scale = NumericalScale1
        Me.GraphAxis1.Style.Visible = False
        '
        'barSeries1
        '
        Me.barSeries1.CategoryGroup = GraphGroup1
        Me.barSeries1.CoordinateSystem = Me.cartesianCoordinateSystem1
        Me.barSeries1.DataPointLabel = "=Sum(Fields.Count)"
        Me.barSeries1.DataPointLabelAlignment = Telerik.Reporting.BarDataPointLabelAlignment.InsideEnd
        Me.barSeries1.DataPointLabelStyle.Visible = True
        Me.barSeries1.DataPointStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Point(0R)
        Me.barSeries1.DataPointStyle.Visible = True
        Me.barSeries1.LegendItem.Style.BackgroundColor = System.Drawing.Color.Transparent
        Me.barSeries1.LegendItem.Style.LineColor = System.Drawing.Color.Transparent
        Me.barSeries1.LegendItem.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0R)
        Me.barSeries1.LegendItem.Value = "=Fields.Medal"
        Me.barSeries1.Name = "barSeries1"
        GraphGroup2.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.Place"))
        GraphGroup2.Name = "medalGroup1"
        GraphGroup2.Sortings.Add(New Telerik.Reporting.Sorting("=Fields.Place", Telerik.Reporting.SortDirection.Asc))
        Me.barSeries1.SeriesGroup = GraphGroup2
        Me.barSeries1.Y = "=Sum(Fields.Count)"
        '
        'OlympicMedalsByNationalTeams
        '
        Me.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.detailSection1})
        Me.Name = "OlympicMedals"
        Me.PageSettings.Landscape = True
        Me.PageSettings.Margins = New Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0.5R), Telerik.Reporting.Drawing.Unit.Inch(0.5R), Telerik.Reporting.Drawing.Unit.Inch(0.89999997615814209R), Telerik.Reporting.Drawing.Unit.Inch(0.89999997615814209R))
        Me.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.LetterExtra
        ReportParameter1.AutoRefresh = True
        ReportParameter1.AvailableValues.DataSource = Me.associationsData
        ReportParameter1.AvailableValues.DisplayMember = "= Fields.Name"
        ReportParameter1.AvailableValues.ValueMember = "= Fields.Id"
        ReportParameter1.MultiValue = True
        ReportParameter1.Name = "associationParam"
        ReportParameter1.Text = "Association"
        ReportParameter1.Value = "=Array(1)"
        ReportParameter1.Visible = True
        ReportParameter2.AllowNull = True
        ReportParameter2.AutoRefresh = True
        ReportParameter2.Name = "teamParam"
        ReportParameter2.Value = "United States"
        Me.ReportParameters.Add(ReportParameter1)
        Me.ReportParameters.Add(ReportParameter2)
        Me.Style.BackgroundColor = System.Drawing.Color.White
        Me.Style.Font.Name = "Segoe UI"
        StyleRule1.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.TextItemBase)), New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.HtmlTextBox))})
        StyleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        StyleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        StyleRule2.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.TextItemBase)), New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.HtmlTextBox))})
        StyleRule2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        StyleRule2.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        StyleRule3.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.StyleSelector(GetType(Telerik.Reporting.Table), "Solstice.TableNormal")})
        StyleRule3.Style.BorderColor.Default = System.Drawing.Color.Black
        StyleRule3.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        StyleRule3.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        StyleRule3.Style.Color = System.Drawing.Color.FromArgb(CType(CType(58, Byte), Integer), CType(CType(29, Byte), Integer), CType(CType(20, Byte), Integer))
        StyleRule3.Style.Font.Name = "Gill Sans MT"
        StyleRule3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        DescendantSelector1.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.Table)), New Telerik.Reporting.Drawing.StyleSelector(GetType(Telerik.Reporting.ReportItem), "Solstice.TableGroup")})
        StyleRule4.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {DescendantSelector1})
        StyleRule4.Style.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(222, Byte), Integer), CType(CType(201, Byte), Integer))
        StyleRule4.Style.BorderColor.Default = System.Drawing.Color.Black
        StyleRule4.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        StyleRule4.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        StyleRule4.Style.Color = System.Drawing.Color.FromArgb(CType(CType(58, Byte), Integer), CType(CType(29, Byte), Integer), CType(CType(20, Byte), Integer))
        StyleRule4.Style.Font.Name = "Gill Sans MT"
        StyleRule4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        DescendantSelector2.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.Table)), New Telerik.Reporting.Drawing.StyleSelector(GetType(Telerik.Reporting.ReportItem), "Solstice.GrandTotal")})
        StyleRule5.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {DescendantSelector2})
        StyleRule5.Style.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(213, Byte), Integer), CType(CType(202, Byte), Integer), CType(CType(189, Byte), Integer))
        StyleRule5.Style.BorderColor.Default = System.Drawing.Color.Black
        StyleRule5.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        StyleRule5.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        StyleRule5.Style.Color = System.Drawing.Color.FromArgb(CType(CType(58, Byte), Integer), CType(CType(29, Byte), Integer), CType(CType(20, Byte), Integer))
        StyleRule5.Style.Font.Bold = True
        StyleRule5.Style.Font.Italic = False
        StyleRule5.Style.Font.Name = "Gill Sans MT"
        StyleRule5.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10.0R)
        StyleRule5.Style.Font.Strikeout = False
        StyleRule5.Style.Font.Underline = False
        DescendantSelector3.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.Table)), New Telerik.Reporting.Drawing.StyleSelector(GetType(Telerik.Reporting.ReportItem), "Solstice.TableHeader")})
        StyleRule6.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {DescendantSelector3})
        StyleRule6.Style.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(79, Byte), Integer), CType(CType(39, Byte), Integer), CType(CType(28, Byte), Integer))
        StyleRule6.Style.BorderColor.Default = System.Drawing.Color.Black
        StyleRule6.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        StyleRule6.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        StyleRule6.Style.Color = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(222, Byte), Integer), CType(CType(201, Byte), Integer))
        StyleRule6.Style.Font.Name = "Gill Sans MT"
        StyleRule6.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10.0R)
        StyleRule6.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        DescendantSelector4.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.Table)), New Telerik.Reporting.Drawing.StyleSelector(GetType(Telerik.Reporting.ReportItem), "Solstice.TableBody")})
        StyleRule7.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {DescendantSelector4})
        StyleRule7.Style.BorderColor.Default = System.Drawing.Color.Black
        StyleRule7.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        StyleRule7.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        StyleRule7.Style.Color = System.Drawing.Color.FromArgb(CType(CType(58, Byte), Integer), CType(CType(29, Byte), Integer), CType(CType(20, Byte), Integer))
        StyleRule7.Style.Font.Name = "Gill Sans MT"
        StyleRule7.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        StyleRule8.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.StyleSelector(GetType(Telerik.Reporting.Table), "Normal.TableNormal")})
        StyleRule8.Style.BorderColor.Default = System.Drawing.Color.Black
        StyleRule8.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        StyleRule8.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        StyleRule8.Style.Color = System.Drawing.Color.Black
        StyleRule8.Style.Font.Name = "Tahoma"
        StyleRule8.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        DescendantSelector5.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.Table)), New Telerik.Reporting.Drawing.StyleSelector(GetType(Telerik.Reporting.ReportItem), "Normal.TableGroup")})
        StyleRule9.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {DescendantSelector5})
        StyleRule9.Style.BorderColor.Default = System.Drawing.Color.Black
        StyleRule9.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        StyleRule9.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        StyleRule9.Style.Font.Name = "Tahoma"
        StyleRule9.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        DescendantSelector6.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.Table)), New Telerik.Reporting.Drawing.StyleSelector(GetType(Telerik.Reporting.ReportItem), "Normal.TableHeader")})
        StyleRule10.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {DescendantSelector6})
        StyleRule10.Style.BorderColor.Default = System.Drawing.Color.Black
        StyleRule10.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        StyleRule10.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        StyleRule10.Style.Font.Name = "Tahoma"
        StyleRule10.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10.0R)
        StyleRule10.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        DescendantSelector7.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.Table)), New Telerik.Reporting.Drawing.StyleSelector(GetType(Telerik.Reporting.ReportItem), "Normal.TableBody")})
        StyleRule11.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {DescendantSelector7})
        StyleRule11.Style.BorderColor.Default = System.Drawing.Color.Black
        StyleRule11.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        StyleRule11.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        StyleRule11.Style.Font.Name = "Tahoma"
        StyleRule11.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        DescendantSelector8.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.Table)), New Telerik.Reporting.Drawing.StyleSelector(GetType(Telerik.Reporting.ReportItem), "Normal.GrandTotal")})
        StyleRule12.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {DescendantSelector8})
        StyleRule12.Style.BorderColor.Default = System.Drawing.Color.Black
        StyleRule12.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        StyleRule12.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        StyleRule12.Style.Font.Bold = True
        StyleRule12.Style.Font.Italic = False
        StyleRule12.Style.Font.Name = "Tahoma"
        StyleRule12.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10.0R)
        StyleRule12.Style.Font.Strikeout = False
        StyleRule12.Style.Font.Underline = False
        Me.StyleSheet.AddRange(New Telerik.Reporting.Drawing.StyleRule() {StyleRule1, StyleRule2, StyleRule3, StyleRule4, StyleRule5, StyleRule6, StyleRule7, StyleRule8, StyleRule9, StyleRule10, StyleRule11, StyleRule12})
        Me.Width = Telerik.Reporting.Drawing.Unit.Cm(25.1460018157959R)
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Friend WithEvents associationsData As Telerik.Reporting.CsvDataSource
    Friend WithEvents detailSection1 As Telerik.Reporting.DetailSection
    Friend WithEvents map1 As Telerik.Reporting.Map
    Friend WithEvents PointMapSeries1 As Telerik.Reporting.PointMapSeries
    Friend WithEvents columnMapSeries1 As Telerik.Reporting.ColumnMapSeries
    Friend WithEvents crosstab1 As Telerik.Reporting.Crosstab
    Friend WithEvents textBox6 As Telerik.Reporting.TextBox
    Friend WithEvents textBox7 As Telerik.Reporting.TextBox
    Friend WithEvents textBox8 As Telerik.Reporting.TextBox
    Friend WithEvents textBox9 As Telerik.Reporting.TextBox
    Friend WithEvents textBox1 As Telerik.Reporting.TextBox
    Friend WithEvents textBox2 As Telerik.Reporting.TextBox
    Friend WithEvents textBox5 As Telerik.Reporting.TextBox
    Friend WithEvents textBox3 As Telerik.Reporting.TextBox
    Friend WithEvents textBox4 As Telerik.Reporting.TextBox
    Friend WithEvents graph1 As Telerik.Reporting.Graph
    Friend WithEvents cartesianCoordinateSystem1 As Telerik.Reporting.CartesianCoordinateSystem
    Friend WithEvents graphAxis2 As Telerik.Reporting.GraphAxis
    Friend WithEvents GraphAxis1 As Telerik.Reporting.GraphAxis
    Friend WithEvents barSeries1 As Telerik.Reporting.BarSeries
    Friend WithEvents medalsData As Telerik.Reporting.CsvDataSource
End Class