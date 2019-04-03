Partial Class PopulationDensity

    'NOTE: The following procedure is required by the telerik Reporting Designer
    'It can be modified using the telerik Reporting Designer.  
    'Do not modify it using the code editor.
    Private Sub InitializeComponent()
        Dim DataColumn1 As Telerik.Reporting.DataColumn = New Telerik.Reporting.DataColumn()
        Dim DataColumn2 As Telerik.Reporting.DataColumn = New Telerik.Reporting.DataColumn()
        Dim DataColumn3 As Telerik.Reporting.DataColumn = New Telerik.Reporting.DataColumn()
        Dim DataColumn4 As Telerik.Reporting.DataColumn = New Telerik.Reporting.DataColumn()
        Dim DataColumn5 As Telerik.Reporting.DataColumn = New Telerik.Reporting.DataColumn()
        Dim DataColumn6 As Telerik.Reporting.DataColumn = New Telerik.Reporting.DataColumn()
        Dim DataColumn7 As Telerik.Reporting.DataColumn = New Telerik.Reporting.DataColumn()
        Dim DataColumn8 As Telerik.Reporting.DataColumn = New Telerik.Reporting.DataColumn()
        Dim DataColumn9 As Telerik.Reporting.DataColumn = New Telerik.Reporting.DataColumn()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PopulationDensity))
        Dim ShapeMapGroup1 As Telerik.Reporting.ShapeMapGroup = New Telerik.Reporting.ShapeMapGroup()
        Dim MapLegend1 As Telerik.Reporting.MapLegend = New Telerik.Reporting.MapLegend()
        Dim MercatorProjection1 As Telerik.Reporting.MercatorProjection = New Telerik.Reporting.MercatorProjection()
        Dim MapTitle1 As Telerik.Reporting.MapTitle = New Telerik.Reporting.MapTitle()
        Dim GradientPalette1 As Telerik.Reporting.Drawing.GradientPalette = New Telerik.Reporting.Drawing.GradientPalette()
        Dim MapGroup1 As Telerik.Reporting.MapGroup = New Telerik.Reporting.MapGroup()
        Dim EsriShapeFileSourceType1 As Telerik.Reporting.EsriShapeFileSourceType = New Telerik.Reporting.EsriShapeFileSourceType()
        Dim MapRelationPair1 As Telerik.Reporting.MapRelationPair = New Telerik.Reporting.MapRelationPair()
        Dim StyleRule1 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule2 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Me.populationData = New Telerik.Reporting.CsvDataSource()
        Me.detailSection1 = New Telerik.Reporting.DetailSection()
        Me.map2 = New Telerik.Reporting.Map()
        Me.shapeMapSeries1 = New Telerik.Reporting.ShapeMapSeries()
        Me.reportHeaderSection1 = New Telerik.Reporting.ReportHeaderSection()
        Me.textBoxTitle = New Telerik.Reporting.TextBox()
        Me.textBox2 = New Telerik.Reporting.TextBox()
        Me.reportFooterSection1 = New Telerik.Reporting.ReportFooterSection()
        Me.htmlTextBox1 = New Telerik.Reporting.HtmlTextBox()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'populationData
        '
        DataColumn1.Name = "Pos."
        DataColumn1.Type = Telerik.Reporting.SimpleType.[Integer]
        DataColumn2.Name = "Country"
        DataColumn3.Name = "Area (km2)"
        DataColumn3.Type = Telerik.Reporting.SimpleType.Float
        DataColumn4.Name = "Area (mi2)"
        DataColumn4.Type = Telerik.Reporting.SimpleType.Float
        DataColumn5.Name = "Population"
        DataColumn5.Type = Telerik.Reporting.SimpleType.[Integer]
        DataColumn6.Name = "Density (pop./km2)"
        DataColumn6.Type = Telerik.Reporting.SimpleType.Float
        DataColumn7.Name = "Density (pop./mi2)"
        DataColumn7.Type = Telerik.Reporting.SimpleType.Float
        DataColumn8.Name = "Date"
        DataColumn8.Type = Telerik.Reporting.SimpleType.DateTime
        DataColumn9.Name = "Population source"
        Me.populationData.Columns.Add(DataColumn1)
        Me.populationData.Columns.Add(DataColumn2)
        Me.populationData.Columns.Add(DataColumn3)
        Me.populationData.Columns.Add(DataColumn4)
        Me.populationData.Columns.Add(DataColumn5)
        Me.populationData.Columns.Add(DataColumn6)
        Me.populationData.Columns.Add(DataColumn7)
        Me.populationData.Columns.Add(DataColumn8)
        Me.populationData.Columns.Add(DataColumn9)
        Me.populationData.DateTimeFormat = "MMMM dd, yyyy"
        Me.populationData.FieldSeparators = New Char() {Global.Microsoft.VisualBasic.ChrW(59)}
        Me.populationData.HasHeaders = True
        Me.populationData.Name = "populationData"
        Me.populationData.RecordSeparators = New Char() {Global.Microsoft.VisualBasic.ChrW(13), Global.Microsoft.VisualBasic.ChrW(10)}
        Me.populationData.Source = resources.GetString("populationData.Source")
        Me.populationData.ThousandSeparator = ","
        '
        'detailSection1
        '
        Me.detailSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(14.199999809265137R)
        Me.detailSection1.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.map2})
        Me.detailSection1.Name = "detailSection1"
        '
        'map2
        '
        Me.map2.ColorPalette = Nothing
        Me.map2.DataSource = Me.populationData
        ShapeMapGroup1.Groupings.Add(New Telerik.Reporting.Grouping("= Fields.CNTRY_NAME"))
        ShapeMapGroup1.Name = "categoryGroup1"
        Me.map2.GeoLocationGroups.Add(ShapeMapGroup1)
        MapLegend1.Style.LineColor = System.Drawing.Color.LightGray
        MapLegend1.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0R)
        MapLegend1.Title = "people/km²"
        MapLegend1.TitleStyle.Font.Bold = True
        MapLegend1.TitleStyle.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(6.0R)
        MapLegend1.TitleStyle.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.map2.Legends.Add(MapLegend1)
        Me.map2.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.19999989867210388R), Telerik.Reporting.Drawing.Unit.Cm(0.30000004172325134R))
        Me.map2.Meridians.Style.LineColor = System.Drawing.Color.LightGray
        Me.map2.Name = "map2"
        Me.map2.Parallels.Style.LineColor = System.Drawing.Color.LightGray
        Me.map2.PlotAreaStyle.LineColor = System.Drawing.Color.LightGray
        Me.map2.PlotAreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0R)
        Me.map2.Projection = MercatorProjection1
        Me.map2.ScaleLegend.ItemStyle.BorderColor.Default = System.Drawing.Color.White
        Me.map2.ScaleLegend.ItemStyle.LineColor = System.Drawing.Color.DodgerBlue
        Me.map2.ScaleLegend.ItemStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(2.0R)
        Me.map2.ScaleLegend.ItemStyle.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(5.0R)
        Me.map2.ScaleLegend.ItemStyle.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(5.0R)
        Me.map2.ScaleLegend.ItemStyle.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.map2.ScaleLegend.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom
        Me.map2.Series.Add(Me.shapeMapSeries1)
        Me.map2.SeriesGroups.Add(MapGroup1)
        Me.map2.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(16.700000762939453R), Telerik.Reporting.Drawing.Unit.Cm(13.59999942779541R))
        Me.map2.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        Me.map2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        Me.map2.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        Me.map2.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        MapTitle1.Style.LineColor = System.Drawing.Color.LightGray
        MapTitle1.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0R)
        MapTitle1.Text = ""
        Me.map2.Titles.Add(MapTitle1)
        '
        'shapeMapSeries1
        '
        Me.shapeMapSeries1.ColorData = "= Sum(Fields.[Density (pop./km2)])"
        GradientPalette1.BeginColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(213, Byte), Integer))
        GradientPalette1.EndColor = System.Drawing.Color.FromArgb(CType(CType(213, Byte), Integer), CType(CType(106, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.shapeMapSeries1.ColorPalette = GradientPalette1
        Me.shapeMapSeries1.ColorsCount = 8
        Me.shapeMapSeries1.DataPointLabel = "{Fields.CNTRY_NAME}" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "({Sum(Fields.[Density (pop./km2)])})"
        Me.shapeMapSeries1.DataPointLabelStyle.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8.0R)
        Me.shapeMapSeries1.DataPointLabelStyle.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.shapeMapSeries1.DataPointLabelStyle.Visible = False
        Me.shapeMapSeries1.DataPointStyle.LineColor = System.Drawing.Color.Gray
        Me.shapeMapSeries1.DataPointStyle.Visible = True
        Me.shapeMapSeries1.LegendItem.Value = "=Format('{0:N0} - {1:N0}', RangeMin, RangeMax)"
        Me.shapeMapSeries1.Name = "shapeMapSeries1"
        Me.shapeMapSeries1.RangeGrouping = Telerik.Reporting.ShapeMapSeries.RangeGroupings.EqualDistribution
        MapGroup1.Name = "seriesGroup1"
        Me.shapeMapSeries1.SeriesGroup = MapGroup1
        Me.shapeMapSeries1.ShapeMapGroup = ShapeMapGroup1
        EsriShapeFileSourceType1.FileName = "Resources/world.shp"
        MapRelationPair1.AnalyticalField = "= Fields.Country"
        MapRelationPair1.ShapeFileField = "= Fields.CNTRY_NAME"
        EsriShapeFileSourceType1.RelationPairs.Add(MapRelationPair1)
        Me.shapeMapSeries1.SourceType = EsriShapeFileSourceType1
        Me.shapeMapSeries1.ToolTip.Text = "=Format('Area: {0} km²; " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Population: {1}; " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Density: {2:0.00} ppl/km²', Fields.S" &
    "QKM, Fields.POP_CNTRY, Fields.POP_CNTRY/Fields.SQKM)"
        Me.shapeMapSeries1.ToolTip.Title = "=Fields.CNTRY_NAME"
        '
        'reportHeaderSection1
        '
        Me.reportHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(2.1999995708465576R)
        Me.reportHeaderSection1.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.textBoxTitle, Me.textBox2})
        Me.reportHeaderSection1.Name = "reportHeaderSection1"
        '
        'textBoxTitle
        '
        Me.textBoxTitle.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.19999989867210388R), Telerik.Reporting.Drawing.Unit.Cm(0.30000004172325134R))
        Me.textBoxTitle.Name = "textBox1"
        Me.textBoxTitle.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000001907348633R), Telerik.Reporting.Drawing.Unit.Cm(0.800000011920929R))
        Me.textBoxTitle.Style.Font.Bold = False
        Me.textBoxTitle.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(14.0R)
        Me.textBoxTitle.Value = "World population density"
        '
        'textBox2
        '
        Me.textBox2.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.19999989867210388R), Telerik.Reporting.Drawing.Unit.Cm(1.6000001430511475R))
        Me.textBox2.Name = "textBox2"
        Me.textBox2.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(13.799998283386231R), Telerik.Reporting.Drawing.Unit.Cm(0.50019979476928711R))
        Me.textBox2.Value = "The choropleth below shows the 100 most populated countries"
        '
        'reportFooterSection1
        '
        Me.reportFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(1.1999986171722412R)
        Me.reportFooterSection1.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.htmlTextBox1})
        Me.reportFooterSection1.Name = "reportFooterSection1"
        '
        'htmlTextBox1
        '
        Me.htmlTextBox1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.19999989867210388R), Telerik.Reporting.Drawing.Unit.Cm(0.59999889135360718R))
        Me.htmlTextBox1.Name = "htmlTextBox1"
        Me.htmlTextBox1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(13.799997329711914R), Telerik.Reporting.Drawing.Unit.Cm(0.49980011582374573R))
        Me.htmlTextBox1.Value = "The analytical data is obtained from <a href=""http://en.wikipedia.org/wiki/List_o" &
    "f_sovereign_states_and_dependent_territories_by_population_density"" target=""_bla" &
    "nk"">Wikipedia</a>."
        '
        'PopulationDensity
        '
        Me.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.detailSection1, Me.reportHeaderSection1, Me.reportFooterSection1})
        Me.Name = "PopulationDensity"
        Me.PageSettings.Landscape = False
        Me.PageSettings.Margins = New Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0.800000011920929R), Telerik.Reporting.Drawing.Unit.Inch(0.800000011920929R), Telerik.Reporting.Drawing.Unit.Inch(1.0R), Telerik.Reporting.Drawing.Unit.Inch(1.0R))
        Me.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Letter
        Me.Style.Font.Name = "Segoe UI"
        Me.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        StyleRule1.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.TextItemBase)), New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.HtmlTextBox))})
        StyleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        StyleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        StyleRule2.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.TextItemBase)), New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.HtmlTextBox))})
        StyleRule2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        StyleRule2.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        Me.StyleSheet.AddRange(New Telerik.Reporting.Drawing.StyleRule() {StyleRule1, StyleRule2})
        Me.Width = Telerik.Reporting.Drawing.Unit.Inch(6.7322831153869629R)
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Friend WithEvents populationData As Telerik.Reporting.CsvDataSource
    Friend WithEvents detailSection1 As Telerik.Reporting.DetailSection
    Friend WithEvents map2 As Telerik.Reporting.Map
    Friend WithEvents shapeMapSeries1 As Telerik.Reporting.ShapeMapSeries
    Friend WithEvents reportHeaderSection1 As Telerik.Reporting.ReportHeaderSection
    Friend WithEvents textBoxTitle As Telerik.Reporting.TextBox
    Friend WithEvents textBox2 As Telerik.Reporting.TextBox
    Friend WithEvents reportFooterSection1 As Telerik.Reporting.ReportFooterSection
    Friend WithEvents htmlTextBox1 As Telerik.Reporting.HtmlTextBox
End Class