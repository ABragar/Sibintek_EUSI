namespace Telerik.Reporting.Examples.CSharp
{
    partial class PopulationDensity
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.Reporting.DataColumn dataColumn1 = new Telerik.Reporting.DataColumn();
            Telerik.Reporting.DataColumn dataColumn2 = new Telerik.Reporting.DataColumn();
            Telerik.Reporting.DataColumn dataColumn3 = new Telerik.Reporting.DataColumn();
            Telerik.Reporting.DataColumn dataColumn4 = new Telerik.Reporting.DataColumn();
            Telerik.Reporting.DataColumn dataColumn5 = new Telerik.Reporting.DataColumn();
            Telerik.Reporting.DataColumn dataColumn6 = new Telerik.Reporting.DataColumn();
            Telerik.Reporting.DataColumn dataColumn7 = new Telerik.Reporting.DataColumn();
            Telerik.Reporting.DataColumn dataColumn8 = new Telerik.Reporting.DataColumn();
            Telerik.Reporting.DataColumn dataColumn9 = new Telerik.Reporting.DataColumn();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PopulationDensity));
            Telerik.Reporting.ShapeMapGroup shapeMapGroup1 = new Telerik.Reporting.ShapeMapGroup();
            Telerik.Reporting.MapLegend mapLegend1 = new Telerik.Reporting.MapLegend();
            Telerik.Reporting.MercatorProjection mercatorProjection1 = new Telerik.Reporting.MercatorProjection();
            Telerik.Reporting.MapTitle mapTitle1 = new Telerik.Reporting.MapTitle();
            Telerik.Reporting.Drawing.GradientPalette gradientPalette1 = new Telerik.Reporting.Drawing.GradientPalette();
            Telerik.Reporting.MapGroup mapGroup1 = new Telerik.Reporting.MapGroup();
            Telerik.Reporting.EsriShapeFileSourceType esriShapeFileSourceType1 = new Telerik.Reporting.EsriShapeFileSourceType();
            Telerik.Reporting.MapRelationPair mapRelationPair1 = new Telerik.Reporting.MapRelationPair();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            this.populationData = new Telerik.Reporting.CsvDataSource();
            this.detailSection1 = new Telerik.Reporting.DetailSection();
            this.map2 = new Telerik.Reporting.Map();
            this.shapeMapSeries1 = new Telerik.Reporting.ShapeMapSeries();
            this.reportHeaderSection1 = new Telerik.Reporting.ReportHeaderSection();
            this.textBoxTitle = new Telerik.Reporting.TextBox();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.reportFooterSection1 = new Telerik.Reporting.ReportFooterSection();
            this.htmlTextBox1 = new Telerik.Reporting.HtmlTextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // populationData
            // 
            dataColumn1.Name = "Pos.";
            dataColumn1.Type = Telerik.Reporting.SimpleType.Integer;
            dataColumn2.Name = "Country";
            dataColumn3.Name = "Area (km2)";
            dataColumn3.Type = Telerik.Reporting.SimpleType.Float;
            dataColumn4.Name = "Area (mi2)";
            dataColumn4.Type = Telerik.Reporting.SimpleType.Float;
            dataColumn5.Name = "Population";
            dataColumn5.Type = Telerik.Reporting.SimpleType.Integer;
            dataColumn6.Name = "Density (pop./km2)";
            dataColumn6.Type = Telerik.Reporting.SimpleType.Float;
            dataColumn7.Name = "Density (pop./mi2)";
            dataColumn7.Type = Telerik.Reporting.SimpleType.Float;
            dataColumn8.Name = "Date";
            dataColumn8.Type = Telerik.Reporting.SimpleType.DateTime;
            dataColumn9.Name = "Population source";
            this.populationData.Columns.Add(dataColumn1);
            this.populationData.Columns.Add(dataColumn2);
            this.populationData.Columns.Add(dataColumn3);
            this.populationData.Columns.Add(dataColumn4);
            this.populationData.Columns.Add(dataColumn5);
            this.populationData.Columns.Add(dataColumn6);
            this.populationData.Columns.Add(dataColumn7);
            this.populationData.Columns.Add(dataColumn8);
            this.populationData.Columns.Add(dataColumn9);
            this.populationData.DateTimeFormat = "MMMM dd, yyyy";
            this.populationData.FieldSeparators = new char[] {
        ';'};
            this.populationData.HasHeaders = true;
            this.populationData.Name = "populationData";
            this.populationData.RecordSeparators = new char[] {
        '\r',
        '\n'};
            this.populationData.Source = resources.GetString("populationData.Source");
            this.populationData.ThousandSeparator = ",";
            // 
            // detailSection1
            // 
            this.detailSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(14.199999809265137D);
            this.detailSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.map2});
            this.detailSection1.Name = "detailSection1";
            // 
            // map2
            // 
            this.map2.ColorPalette = null;
            this.map2.DataSource = this.populationData;
            shapeMapGroup1.Groupings.Add(new Telerik.Reporting.Grouping("= Fields.CNTRY_NAME"));
            shapeMapGroup1.Name = "categoryGroup1";
            this.map2.GeoLocationGroups.Add(shapeMapGroup1);
            mapLegend1.Style.LineColor = System.Drawing.Color.LightGray;
            mapLegend1.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0D);
            mapLegend1.Title = "people/km²";
            mapLegend1.TitleStyle.Font.Bold = true;
            mapLegend1.TitleStyle.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(6D);
            mapLegend1.TitleStyle.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.map2.Legends.Add(mapLegend1);
            this.map2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.19999989867210388D), Telerik.Reporting.Drawing.Unit.Cm(0.30000004172325134D));
            this.map2.Meridians.Style.LineColor = System.Drawing.Color.LightGray;
            this.map2.Name = "map2";
            this.map2.Parallels.Style.LineColor = System.Drawing.Color.LightGray;
            this.map2.PlotAreaStyle.LineColor = System.Drawing.Color.LightGray;
            this.map2.PlotAreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0D);
            this.map2.Projection = mercatorProjection1;
            this.map2.ScaleLegend.ItemStyle.BorderColor.Default = System.Drawing.Color.White;
            this.map2.ScaleLegend.ItemStyle.LineColor = System.Drawing.Color.DodgerBlue;
            this.map2.ScaleLegend.ItemStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(2D);
            this.map2.ScaleLegend.ItemStyle.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(5D);
            this.map2.ScaleLegend.ItemStyle.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(5D);
            this.map2.ScaleLegend.ItemStyle.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.map2.ScaleLegend.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom;
            this.map2.Series.Add(this.shapeMapSeries1);
            this.map2.SeriesGroups.Add(mapGroup1);
            this.map2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(16.700000762939453D), Telerik.Reporting.Drawing.Unit.Cm(13.59999942779541D));
            this.map2.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.map2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.map2.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.map2.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            mapTitle1.Style.LineColor = System.Drawing.Color.LightGray;
            mapTitle1.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0D);
            mapTitle1.Text = "";
            this.map2.Titles.Add(mapTitle1);
            // 
            // shapeMapSeries1
            // 
            this.shapeMapSeries1.ColorData = "= Sum(Fields.[Density (pop./km2)])";
            gradientPalette1.BeginColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(226)))), ((int)(((byte)(213)))));
            gradientPalette1.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(106)))), ((int)(((byte)(0)))));
            this.shapeMapSeries1.ColorPalette = gradientPalette1;
            this.shapeMapSeries1.ColorsCount = 8;
            this.shapeMapSeries1.DataPointLabel = "{Fields.CNTRY_NAME}\r\n({Sum(Fields.[Density (pop./km2)])})";
            this.shapeMapSeries1.DataPointLabelStyle.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.shapeMapSeries1.DataPointLabelStyle.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.shapeMapSeries1.DataPointLabelStyle.Visible = false;
            this.shapeMapSeries1.DataPointStyle.LineColor = System.Drawing.Color.Gray;
            this.shapeMapSeries1.DataPointStyle.Visible = true;
            this.shapeMapSeries1.LegendItem.Value = "=Format(\'{0:N0} - {1:N0}\', RangeMin, RangeMax)";
            this.shapeMapSeries1.Name = "shapeMapSeries1";
            this.shapeMapSeries1.RangeGrouping = Telerik.Reporting.ShapeMapSeries.RangeGroupings.EqualDistribution;
            mapGroup1.Name = "seriesGroup1";
            this.shapeMapSeries1.SeriesGroup = mapGroup1;
            this.shapeMapSeries1.ShapeMapGroup = shapeMapGroup1;
            esriShapeFileSourceType1.FileName = "Resources/world.shp";
            mapRelationPair1.AnalyticalField = "= Fields.Country";
            mapRelationPair1.ShapeFileField = "= Fields.CNTRY_NAME";
            esriShapeFileSourceType1.RelationPairs.Add(mapRelationPair1);
            this.shapeMapSeries1.SourceType = esriShapeFileSourceType1;
            this.shapeMapSeries1.ToolTip.Text = "=Format(\'Area: {0} km²;\r\nPopulation: {1};\r\nDensity: {2:0.00} ppl/km²\', Fields.SQK" +
    "M, Fields.POP_CNTRY, Fields.POP_CNTRY/Fields.SQKM)";
            this.shapeMapSeries1.ToolTip.Title = "=Fields.CNTRY_NAME";
            // 
            // reportHeaderSection1
            // 
            this.reportHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(2.1999995708465576D);
            this.reportHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBoxTitle,
            this.textBox2});
            this.reportHeaderSection1.Name = "reportHeaderSection1";
            // 
            // textBoxTitle
            // 
            this.textBoxTitle.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.19999989867210388D), Telerik.Reporting.Drawing.Unit.Cm(0.30000004172325134D));
            this.textBoxTitle.Name = "textBoxTitle";
            this.textBoxTitle.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000001907348633D), Telerik.Reporting.Drawing.Unit.Cm(0.800000011920929D));
            this.textBoxTitle.Style.Font.Bold = false;
            this.textBoxTitle.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(14D);
            this.textBoxTitle.Value = "World population density";
            // 
            // textBox2
            // 
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.19999989867210388D), Telerik.Reporting.Drawing.Unit.Cm(1.6000001430511475D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(13.799998283386231D), Telerik.Reporting.Drawing.Unit.Cm(0.50019979476928711D));
            this.textBox2.Value = "The choropleth below shows the 100 most populated countries";
            // 
            // reportFooterSection1
            // 
            this.reportFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(1.1999986171722412D);
            this.reportFooterSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.htmlTextBox1});
            this.reportFooterSection1.Name = "reportFooterSection1";
            // 
            // htmlTextBox1
            // 
            this.htmlTextBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.19999989867210388D), Telerik.Reporting.Drawing.Unit.Cm(0.59999889135360718D));
            this.htmlTextBox1.Name = "htmlTextBox1";
            this.htmlTextBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(13.799997329711914D), Telerik.Reporting.Drawing.Unit.Cm(0.49980011582374573D));
            this.htmlTextBox1.Value = "The analytical data is obtained from <a href=\"http://en.wikipedia.org/wiki/List_o" +
    "f_sovereign_states_and_dependent_territories_by_population_density\" target=\"_bla" +
    "nk\">Wikipedia</a>.";
            // 
            // PopulationDensity
            // 
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.detailSection1,
            this.reportHeaderSection1,
            this.reportFooterSection1});
            this.Name = "PopulationDensity";
            this.PageSettings.Landscape = false;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0.800000011920929D), Telerik.Reporting.Drawing.Unit.Inch(0.800000011920929D), Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(1D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Letter;
            this.Style.Font.Name = "Segoe UI";
            this.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule2.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1,
            styleRule2});
            this.Width = Telerik.Reporting.Drawing.Unit.Inch(6.7322831153869629D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private CsvDataSource populationData;
        private DetailSection detailSection1;
        private Map map2;
        private ShapeMapSeries shapeMapSeries1;
        private ReportHeaderSection reportHeaderSection1;
        private TextBox textBoxTitle;
        private TextBox textBox2;
        private ReportFooterSection reportFooterSection1;
        private HtmlTextBox htmlTextBox1;

    }
}