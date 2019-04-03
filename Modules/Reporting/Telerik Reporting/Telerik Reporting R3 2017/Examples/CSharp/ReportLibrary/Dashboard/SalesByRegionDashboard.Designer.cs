namespace Telerik.Reporting.Examples.CSharp
{
    partial class SalesByRegionDashboard
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SalesByRegionDashboard));
            Telerik.Reporting.Drawing.ColorPalette colorPalette1 = new Telerik.Reporting.Drawing.ColorPalette();
            Telerik.Reporting.GeoLocationMapGroup geoLocationMapGroup1 = new Telerik.Reporting.GeoLocationMapGroup();
            Telerik.Reporting.MapLegend mapLegend1 = new Telerik.Reporting.MapLegend();
            Telerik.Reporting.MercatorProjection mercatorProjection1 = new Telerik.Reporting.MercatorProjection();
            Telerik.Reporting.GenericTileProvider genericTileProvider1 = new Telerik.Reporting.GenericTileProvider();
            Telerik.Reporting.MapTitle mapTitle1 = new Telerik.Reporting.MapTitle();
            Telerik.Reporting.MapGroup mapGroup1 = new Telerik.Reporting.MapGroup();
            Telerik.Reporting.MapGroup mapGroup2 = new Telerik.Reporting.MapGroup();
            Telerik.Reporting.GraphGroup graphGroup1 = new Telerik.Reporting.GraphGroup();
            Telerik.Reporting.Drawing.ColorPalette colorPalette2 = new Telerik.Reporting.Drawing.ColorPalette();
            Telerik.Reporting.GraphTitle graphTitle1 = new Telerik.Reporting.GraphTitle();
            Telerik.Reporting.DateTimeScale dateTimeScale1 = new Telerik.Reporting.DateTimeScale();
            Telerik.Reporting.NumericalScale numericalScale1 = new Telerik.Reporting.NumericalScale();
            Telerik.Reporting.GraphGroup graphGroup2 = new Telerik.Reporting.GraphGroup();
            Telerik.Reporting.GraphGroup graphGroup3 = new Telerik.Reporting.GraphGroup();
            Telerik.Reporting.Drawing.ColorPalette colorPalette3 = new Telerik.Reporting.Drawing.ColorPalette();
            Telerik.Reporting.GraphTitle graphTitle2 = new Telerik.Reporting.GraphTitle();
            Telerik.Reporting.NumericalScale numericalScale2 = new Telerik.Reporting.NumericalScale();
            Telerik.Reporting.CategoryScale categoryScale1 = new Telerik.Reporting.CategoryScale();
            Telerik.Reporting.GraphGroup graphGroup4 = new Telerik.Reporting.GraphGroup();
            Telerik.Reporting.GraphGroup graphGroup5 = new Telerik.Reporting.GraphGroup();
            Telerik.Reporting.Drawing.ColorPalette colorPalette4 = new Telerik.Reporting.Drawing.ColorPalette();
            Telerik.Reporting.GraphTitle graphTitle3 = new Telerik.Reporting.GraphTitle();
            Telerik.Reporting.NumericalScale numericalScale3 = new Telerik.Reporting.NumericalScale();
            Telerik.Reporting.CategoryScale categoryScale2 = new Telerik.Reporting.CategoryScale();
            Telerik.Reporting.GraphGroup graphGroup6 = new Telerik.Reporting.GraphGroup();
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter2 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter3 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter4 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            this.yearsData = new Telerik.Reporting.SqlDataSource();
            this.countryData = new Telerik.Reporting.SqlDataSource();
            this.productCategoriesData = new Telerik.Reporting.SqlDataSource();
            this.salesData = new Telerik.Reporting.SqlDataSource();
            this.detailSection1 = new Telerik.Reporting.DetailSection();
            this.map1 = new Telerik.Reporting.Map();
            this.pieMapSeries1 = new Telerik.Reporting.PieMapSeries();
            this.graph1 = new Telerik.Reporting.Graph();
            this.cartesianCoordinateSystem1 = new Telerik.Reporting.CartesianCoordinateSystem();
            this.graphAxis2 = new Telerik.Reporting.GraphAxis();
            this.graphAxis1 = new Telerik.Reporting.GraphAxis();
            this.lineSeries1 = new Telerik.Reporting.LineSeries();
            this.graph2 = new Telerik.Reporting.Graph();
            this.cartesianCoordinateSystem2 = new Telerik.Reporting.CartesianCoordinateSystem();
            this.graphAxis3 = new Telerik.Reporting.GraphAxis();
            this.graphAxis4 = new Telerik.Reporting.GraphAxis();
            this.barSeries1 = new Telerik.Reporting.BarSeries();
            this.graph3 = new Telerik.Reporting.Graph();
            this.cartesianCoordinateSystem3 = new Telerik.Reporting.CartesianCoordinateSystem();
            this.graphAxis5 = new Telerik.Reporting.GraphAxis();
            this.graphAxis6 = new Telerik.Reporting.GraphAxis();
            this.barSeries2 = new Telerik.Reporting.BarSeries();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.textBox2 = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // yearsData
            // 
            this.yearsData.ConnectionString = "Telerik.Reporting.Examples.CSharp.Properties.Settings.TelerikConnectionString";
            this.yearsData.Name = "yearsData";
            this.yearsData.SelectCommand = "SELECT DISTINCT DatePart(YEAR, soh.OrderDate) AS Year\r\nFROM Sales.SalesOrderHeade" +
    "r AS soh\r\nORDER BY DatePart(YEAR, soh.OrderDate) ASC";
            // 
            // countryData
            // 
            this.countryData.ConnectionString = "Telerik.Reporting.Examples.CSharp.Properties.Settings.TelerikConnectionString";
            this.countryData.Name = "countryData";
            this.countryData.SelectCommand = resources.GetString("countryData.SelectCommand");
            // 
            // productCategoriesData
            // 
            this.productCategoriesData.ConnectionString = "Telerik.Reporting.Examples.CSharp.Properties.Settings.TelerikConnectionString";
            this.productCategoriesData.Name = "productCategoriesData";
            this.productCategoriesData.SelectCommand = "SELECT ProductCategoryID, [Name] FROM Production.ProductCategory";
            // 
            // salesData
            // 
            this.salesData.ConnectionString = "Telerik.Reporting.Examples.CSharp.Properties.Settings.TelerikConnectionString";
            this.salesData.Name = "salesData";
            this.salesData.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@country", System.Data.DbType.String, "=Parameters.country.Value"),
            new Telerik.Reporting.SqlDataSourceParameter("@year", System.Data.DbType.Int32, "=Parameters.year.Value"),
            new Telerik.Reporting.SqlDataSourceParameter("@category", System.Data.DbType.Int32, "=Parameters.category.Value")});
            this.salesData.SelectCommand = resources.GetString("salesData.SelectCommand");
            // 
            // detailSection1
            // 
            this.detailSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(8.070866584777832D);
            this.detailSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.map1,
            this.graph1,
            this.graph2,
            this.graph3,
            this.textBox1,
            this.textBox2});
            this.detailSection1.Name = "detailSection1";
            this.detailSection1.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Mm(5D);
            this.detailSection1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Mm(5D);
            this.detailSection1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Mm(5D);
            this.detailSection1.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Mm(5D);
            // 
            // map1
            // 
            colorPalette1.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(80)))), ((int)(((byte)(77))))));
            colorPalette1.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(128)))), ((int)(((byte)(100)))), ((int)(((byte)(162))))));
            colorPalette1.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(247)))), ((int)(((byte)(150)))), ((int)(((byte)(70))))));
            colorPalette1.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(119)))), ((int)(((byte)(44)))), ((int)(((byte)(42))))));
            colorPalette1.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))), ((int)(((byte)(98))))));
            colorPalette1.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(182)))), ((int)(((byte)(87)))), ((int)(((byte)(8))))));
            this.map1.ColorPalette = colorPalette1;
            this.map1.DataSource = this.salesData;
            this.map1.Filters.Add(new Telerik.Reporting.Filter("=Fields.Country", Telerik.Reporting.FilterOperator.In, "=@country"));
            this.map1.Filters.Add(new Telerik.Reporting.Filter("=Fields.OrderDate.Year", Telerik.Reporting.FilterOperator.In, "=@year"));
            geoLocationMapGroup1.Filters.Add(new Telerik.Reporting.Filter("=Sum(Fields.LineTotal)", Telerik.Reporting.FilterOperator.TopN, "=CInt(Parameters.topN.Value)"));
            geoLocationMapGroup1.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.State"));
            geoLocationMapGroup1.Name = "stateGroup";
            geoLocationMapGroup1.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.State", Telerik.Reporting.SortDirection.Asc));
            this.map1.GeoLocationGroups.Add(geoLocationMapGroup1);
            mapLegend1.IsInsidePlotArea = false;
            mapLegend1.Position = Telerik.Reporting.GraphItemPosition.BottomLeft;
            mapLegend1.Style.LineColor = System.Drawing.Color.LightGray;
            mapLegend1.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0D);
            mapLegend1.Style.Visible = true;
            this.map1.Legends.Add(mapLegend1);
            this.map1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.50010007619857788D), Telerik.Reporting.Drawing.Unit.Cm(3.0002996921539307D));
            this.map1.Meridians.Style.LineColor = System.Drawing.Color.LightGray;
            this.map1.Name = "map1";
            this.map1.Parallels.Style.LineColor = System.Drawing.Color.LightGray;
            this.map1.PlotAreaStyle.LineColor = System.Drawing.Color.LightGray;
            this.map1.PlotAreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0D);
            this.map1.Projection = mercatorProjection1;
            this.map1.ScaleLegend.ItemStyle.BorderColor.Default = System.Drawing.Color.White;
            this.map1.ScaleLegend.ItemStyle.Font.Name = "Segoe UI Light";
            this.map1.ScaleLegend.ItemStyle.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.map1.ScaleLegend.ItemStyle.LineColor = System.Drawing.Color.DodgerBlue;
            this.map1.ScaleLegend.ItemStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(2D);
            this.map1.ScaleLegend.ItemStyle.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(5D);
            this.map1.ScaleLegend.ItemStyle.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(5D);
            this.map1.ScaleLegend.ItemStyle.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.map1.ScaleLegend.ScaleUnits = Telerik.Reporting.DistanceUnitType.Imperial;
            this.map1.ScaleLegend.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom;
            this.map1.Series.Add(this.pieMapSeries1);
            this.map1.SeriesGroups.Add(mapGroup1);
            this.map1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(14.499899864196777D), Telerik.Reporting.Drawing.Unit.Cm(11.499997138977051D));
            this.map1.Style.Font.Name = "Segoe UI Light";
            this.map1.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.map1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.map1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.map1.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            genericTileProvider1.Attribution = "© OpenStreetMap contributors";
            genericTileProvider1.LogoUrl = "http://wiki.openstreetmap.org/w/images/thumb/7/79/Public-images-osm_logo.svg/32px-Public-images-osm_logo.svg.png";
            genericTileProvider1.UrlSubdomains.Add("a");
            genericTileProvider1.UrlSubdomains.Add("b");
            genericTileProvider1.UrlSubdomains.Add("c");
            genericTileProvider1.UrlTemplate = "http://{subdomain}.tile.openstreetmap.org/{zoom}/{X}/{Y}.png";
            this.map1.TileProvider = genericTileProvider1;
            mapTitle1.Position = Telerik.Reporting.GraphItemPosition.TopLeft;
            mapTitle1.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(152)))), ((int)(((byte)(152)))), ((int)(((byte)(152)))));
            mapTitle1.Style.Font.Bold = false;
            mapTitle1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            mapTitle1.Style.LineColor = System.Drawing.Color.LightGray;
            mapTitle1.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0D);
            mapTitle1.Style.Visible = true;
            mapTitle1.Text = "SALES VOLUME BY COUNTRY";
            this.map1.Titles.Add(mapTitle1);
            // 
            // pieMapSeries1
            // 
            this.pieMapSeries1.DataPointLabel = "=Format(\'{0:N4}\', Sum(Fields.LineTotal)/1e6)";
            this.pieMapSeries1.DataPointLabelStyle.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.pieMapSeries1.DataPointLabelStyle.Visible = false;
            this.pieMapSeries1.GeoLocationGroup = geoLocationMapGroup1;
            this.pieMapSeries1.Latitude = "=(Fields.Lat)";
            this.pieMapSeries1.LegendItem.MarkStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Point(0D);
            this.pieMapSeries1.LegendItem.Style.BackgroundColor = System.Drawing.Color.Transparent;
            this.pieMapSeries1.LegendItem.Style.LineColor = System.Drawing.Color.Transparent;
            this.pieMapSeries1.LegendItem.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0D);
            this.pieMapSeries1.LegendItem.Value = "= Fields.ProductSubCategory";
            this.pieMapSeries1.Longitude = "=(Fields.Long)";
            this.pieMapSeries1.MarkerMaxSize = Telerik.Reporting.Drawing.Unit.Mm(15D);
            this.pieMapSeries1.MarkerMinSize = Telerik.Reporting.Drawing.Unit.Mm(8D);
            mapGroup2.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.ProductSubCategory"));
            mapGroup2.Name = "ProductSubCategoryGroup";
            mapGroup2.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.ProductSubCategory", Telerik.Reporting.SortDirection.Asc));
            mapGroup1.ChildGroups.Add(mapGroup2);
            mapGroup1.Name = "seriesGroup";
            this.pieMapSeries1.SeriesGroup = mapGroup1;
            this.pieMapSeries1.Size = "=Sum(Fields.LineTotal)";
            this.pieMapSeries1.ToolTip.Title = "= Format('{0}, {1}', Fields.State, Fields.Country)";
            this.pieMapSeries1.ToolTip.Text = "=Format('Total: ${0:0.00}M', Sum(Fields.LineTotal)/1e6)";
            // 
            // graph1
            // 
            graphGroup1.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.OrderDate.Month"));
            graphGroup1.Name = "orderDateMonthGroup";
            graphGroup1.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.OrderDate.Month", Telerik.Reporting.SortDirection.Asc));
            this.graph1.CategoryGroups.Add(graphGroup1);
            colorPalette2.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(192)))), ((int)(((byte)(80)))), ((int)(((byte)(77))))));
            colorPalette2.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(128)))), ((int)(((byte)(100)))), ((int)(((byte)(162))))));
            colorPalette2.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(247)))), ((int)(((byte)(150)))), ((int)(((byte)(70))))));
            colorPalette2.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(119)))), ((int)(((byte)(44)))), ((int)(((byte)(42))))));
            colorPalette2.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))), ((int)(((byte)(98))))));
            colorPalette2.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(182)))), ((int)(((byte)(87)))), ((int)(((byte)(8))))));
            this.graph1.ColorPalette = colorPalette2;
            this.graph1.CoordinateSystems.Add(this.cartesianCoordinateSystem1);
            this.graph1.DataSource = this.salesData;
            this.graph1.Filters.Add(new Telerik.Reporting.Filter("=Fields.Country", Telerik.Reporting.FilterOperator.In, "=@country"));
            this.graph1.Filters.Add(new Telerik.Reporting.Filter("=Fields.OrderDate.Year", Telerik.Reporting.FilterOperator.In, "=@year"));
            this.graph1.Legend.Position = Telerik.Reporting.GraphItemPosition.BottomCenter;
            this.graph1.Legend.Style.LineColor = System.Drawing.Color.LightGray;
            this.graph1.Legend.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0D);
            this.graph1.Legend.Style.Visible = false;
            this.graph1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.50010007619857788D), Telerik.Reporting.Drawing.Unit.Cm(14.500496864318848D));
            this.graph1.Name = "graph1";
            this.graph1.PlotAreaStyle.LineColor = System.Drawing.Color.LightGray;
            this.graph1.PlotAreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0D);
            this.graph1.Series.Add(this.lineSeries1);
            this.graph1.SeriesGroups.Add(graphGroup2);
            this.graph1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(14.499899864196777D), Telerik.Reporting.Drawing.Unit.Cm(5.5D));
            this.graph1.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.graph1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.graph1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.graph1.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            graphTitle1.Position = Telerik.Reporting.GraphItemPosition.TopLeft;
            graphTitle1.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(152)))), ((int)(((byte)(152)))), ((int)(((byte)(152)))));
            graphTitle1.Style.Font.Bold = false;
            graphTitle1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            graphTitle1.Style.LineColor = System.Drawing.Color.LightGray;
            graphTitle1.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0D);
            graphTitle1.Text = "SALES VOLUME, MOUNTLY (in millions USD)";
            this.graph1.Titles.Add(graphTitle1);
            // 
            // cartesianCoordinateSystem1
            // 
            this.cartesianCoordinateSystem1.Name = "cartesianCoordinateSystem1";
            this.cartesianCoordinateSystem1.XAxis = this.graphAxis2;
            this.cartesianCoordinateSystem1.YAxis = this.graphAxis1;
            // 
            // graphAxis2
            // 
            this.graphAxis2.LabelFormat = "{0:MMM}";
            this.graphAxis2.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis2.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis2.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis2.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis2.MinorGridLineStyle.Visible = false;
            this.graphAxis2.Name = "graphAxis2";
            dateTimeScale1.BaseUnit = Telerik.Reporting.DateTimeScaleUnits.Months;
            this.graphAxis2.Scale = dateTimeScale1;
            this.graphAxis2.Title = "";
            this.graphAxis2.TitlePlacement = Telerik.Reporting.GraphAxisTitlePlacement.AtMaximum;
            // 
            // graphAxis1
            // 
            this.graphAxis1.LabelFormat = "${0}M";
            this.graphAxis1.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis1.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis1.MajorGridLineStyle.Visible = true;
            this.graphAxis1.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis1.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis1.MinorGridLineStyle.Visible = false;
            this.graphAxis1.Name = "graphAxis1";
            this.graphAxis1.Scale = numericalScale1;
            this.graphAxis1.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0D);
            this.graphAxis1.Style.Visible = true;
            this.graphAxis1.Title = "";
            this.graphAxis1.TitlePlacement = Telerik.Reporting.GraphAxisTitlePlacement.AtMaximum;
            // 
            // lineSeries1
            // 
            this.lineSeries1.CategoryGroup = graphGroup1;
            this.lineSeries1.CoordinateSystem = this.cartesianCoordinateSystem1;
            this.lineSeries1.DataPointLabel = "=Sum(Fields.LineTotal)";
            this.lineSeries1.DataPointLabelStyle.Visible = false;
            this.lineSeries1.DataPointStyle.Visible = false;
            this.lineSeries1.LegendItem.Style.BackgroundColor = System.Drawing.Color.Transparent;
            this.lineSeries1.LegendItem.Style.LineColor = System.Drawing.Color.Transparent;
            this.lineSeries1.LegendItem.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0D);
            this.lineSeries1.LegendItem.Value = "=Fields.OrderDate.Year";
            this.lineSeries1.LineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.lineSeries1.MarkerMaxSize = Telerik.Reporting.Drawing.Unit.Pixel(50D);
            this.lineSeries1.MarkerMinSize = Telerik.Reporting.Drawing.Unit.Pixel(5D);
            this.lineSeries1.MarkerSize = Telerik.Reporting.Drawing.Unit.Pixel(5D);
            this.lineSeries1.Name = "lineSeries1";
            graphGroup2.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.OrderDate.Year"));
            graphGroup2.Name = "orderDateYearGroup";
            graphGroup2.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.OrderDate.Year", Telerik.Reporting.SortDirection.Asc));
            this.lineSeries1.SeriesGroup = graphGroup2;
            this.lineSeries1.Size = null;
            this.lineSeries1.X = "=Date(1, Fields.OrderDate.Month, 1)";
            this.lineSeries1.Y = "=Sum(Fields.LineTotal)/1e6";
            // 
            // graph2
            // 
            graphGroup3.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.ProductCategory"));
            graphGroup3.Name = "productCategoryGroup";
            graphGroup3.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.ProductCategory", Telerik.Reporting.SortDirection.Asc));
            this.graph2.CategoryGroups.Add(graphGroup3);
            colorPalette3.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(192)))), ((int)(((byte)(80)))), ((int)(((byte)(77))))));
            colorPalette3.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(128)))), ((int)(((byte)(100)))), ((int)(((byte)(162))))));
            colorPalette3.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(247)))), ((int)(((byte)(150)))), ((int)(((byte)(70))))));
            colorPalette3.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(119)))), ((int)(((byte)(44)))), ((int)(((byte)(42))))));
            colorPalette3.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))), ((int)(((byte)(98))))));
            colorPalette3.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(182)))), ((int)(((byte)(87)))), ((int)(((byte)(8))))));
            this.graph2.ColorPalette = colorPalette3;
            this.graph2.CoordinateSystems.Add(this.cartesianCoordinateSystem2);
            this.graph2.DataSource = this.salesData;
            this.graph2.Filters.Add(new Telerik.Reporting.Filter("=Fields.Country", Telerik.Reporting.FilterOperator.In, "=@country"));
            this.graph2.Filters.Add(new Telerik.Reporting.Filter("=Fields.OrderDate.Year", Telerik.Reporting.FilterOperator.In, "=@year"));
            this.graph2.Legend.Position = Telerik.Reporting.GraphItemPosition.RightCenter;
            this.graph2.Legend.Style.LineColor = System.Drawing.Color.LightGray;
            this.graph2.Legend.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0D);
            this.graph2.Legend.Style.Visible = true;
            this.graph2.Legend.Width = Telerik.Reporting.Drawing.Unit.Cm(3D);
            this.graph2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(15.000298500061035D), Telerik.Reporting.Drawing.Unit.Cm(14.500496864318848D));
            this.graph2.Name = "graph2";
            this.graph2.PlotAreaStyle.LineColor = System.Drawing.Color.LightGray;
            this.graph2.PlotAreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0D);
            this.graph2.Series.Add(this.barSeries1);
            this.graph2.SeriesGroups.Add(graphGroup4);
            this.graph2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(14.499902725219727D), Telerik.Reporting.Drawing.Unit.Cm(5.5D));
            this.graph2.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.graph2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.graph2.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.graph2.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            graphTitle2.Position = Telerik.Reporting.GraphItemPosition.TopLeft;
            graphTitle2.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(152)))), ((int)(((byte)(152)))), ((int)(((byte)(152)))));
            graphTitle2.Style.Font.Bold = false;
            graphTitle2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            graphTitle2.Style.LineColor = System.Drawing.Color.LightGray;
            graphTitle2.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0D);
            graphTitle2.Text = "SALES VOLUME BY PRODUCT CATEGORY (in millions USD)";
            this.graph2.Titles.Add(graphTitle2);
            // 
            // cartesianCoordinateSystem2
            // 
            this.cartesianCoordinateSystem2.Name = "cartesianCoordinateSystem2";
            this.cartesianCoordinateSystem2.XAxis = this.graphAxis3;
            this.cartesianCoordinateSystem2.YAxis = this.graphAxis4;
            // 
            // graphAxis3
            // 
            this.graphAxis3.LabelFormat = "${0}M";
            this.graphAxis3.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis3.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis3.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis3.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis3.MinorGridLineStyle.Visible = false;
            this.graphAxis3.Name = "graphAxis3";
            this.graphAxis3.Scale = numericalScale2;
            this.graphAxis3.Title = "";
            this.graphAxis3.TitlePlacement = Telerik.Reporting.GraphAxisTitlePlacement.AtMaximum;
            // 
            // graphAxis4
            // 
            this.graphAxis4.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis4.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis4.MajorGridLineStyle.Visible = false;
            this.graphAxis4.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis4.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis4.MinorGridLineStyle.Visible = false;
            this.graphAxis4.Name = "graphAxis4";
            this.graphAxis4.Scale = categoryScale1;
            this.graphAxis4.Size = Telerik.Reporting.Drawing.Unit.Cm(2.5D);
            this.graphAxis4.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0D);
            // 
            // barSeries1
            // 
            this.barSeries1.ArrangeMode = Telerik.Reporting.GraphSeriesArrangeMode.Stacked;
            this.barSeries1.CategoryGroup = graphGroup3;
            this.barSeries1.CoordinateSystem = this.cartesianCoordinateSystem2;
            this.barSeries1.DataPointLabel = "=Sum(Fields.LineTotal)";
            this.barSeries1.DataPointLabelStyle.Visible = false;
            this.barSeries1.DataPointStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0D);
            this.barSeries1.DataPointStyle.Visible = true;
            this.barSeries1.LegendItem.Style.BackgroundColor = System.Drawing.Color.Transparent;
            this.barSeries1.LegendItem.Style.LineColor = System.Drawing.Color.Transparent;
            this.barSeries1.LegendItem.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0D);
            this.barSeries1.LegendItem.Value = "=Fields.OrderDate.Year";
            this.barSeries1.Name = "barSeries1";
            graphGroup4.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.OrderDate.Year"));
            graphGroup4.Name = "orderDateYearGroup1";
            graphGroup4.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.OrderDate.Year", Telerik.Reporting.SortDirection.Asc));
            this.barSeries1.SeriesGroup = graphGroup4;
            this.barSeries1.X = "=Sum(Fields.LineTotal)/1e6";
            this.barSeries1.ToolTip.Title = "=Format('Year {0}', Fields.OrderDate.Year)";
            this.barSeries1.ToolTip.Text = "=Format('${0:0.00}M', Sum(Fields.LineTotal)/1e6)";
            // 
            // graph3
            // 
            graphGroup5.Filters.Add(new Telerik.Reporting.Filter("=Sum(Fields.LineTotal)", Telerik.Reporting.FilterOperator.TopN, "=CInt(Parameters.topN.Value)"));
            graphGroup5.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.State"));
            graphGroup5.Name = "stateGroup1";
            graphGroup5.Sortings.Add(new Telerik.Reporting.Sorting("=Sum(Fields.LineTotal)", Telerik.Reporting.SortDirection.Asc));
            this.graph3.CategoryGroups.Add(graphGroup5);
            colorPalette4.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(192)))), ((int)(((byte)(80)))), ((int)(((byte)(77))))));
            colorPalette4.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(128)))), ((int)(((byte)(100)))), ((int)(((byte)(162))))));
            colorPalette4.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(247)))), ((int)(((byte)(150)))), ((int)(((byte)(70))))));
            colorPalette4.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(119)))), ((int)(((byte)(44)))), ((int)(((byte)(42))))));
            colorPalette4.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))), ((int)(((byte)(98))))));
            colorPalette4.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(182)))), ((int)(((byte)(87)))), ((int)(((byte)(8))))));
            this.graph3.ColorPalette = colorPalette4;
            this.graph3.CoordinateSystems.Add(this.cartesianCoordinateSystem3);
            this.graph3.DataSource = this.salesData;
            this.graph3.Filters.Add(new Telerik.Reporting.Filter("=Fields.Country", Telerik.Reporting.FilterOperator.In, "=@country"));
            this.graph3.Filters.Add(new Telerik.Reporting.Filter("=Fields.OrderDate.Year", Telerik.Reporting.FilterOperator.In, "=@year"));
            this.graph3.Legend.IsInsidePlotArea = false;
            this.graph3.Legend.Position = Telerik.Reporting.GraphItemPosition.BottomLeft;
            this.graph3.Legend.Style.LineColor = System.Drawing.Color.LightGray;
            this.graph3.Legend.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0D);
            this.graph3.Legend.Style.Visible = true;
            this.graph3.Legend.Width = Telerik.Reporting.Drawing.Unit.Cm(3D);
            this.graph3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(15D), Telerik.Reporting.Drawing.Unit.Cm(3.000300407409668D));
            this.graph3.Name = "graph3";
            this.graph3.PlotAreaStyle.LineColor = System.Drawing.Color.LightGray;
            this.graph3.PlotAreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0D);
            this.graph3.Series.Add(this.barSeries2);
            this.graph3.SeriesGroups.Add(graphGroup6);
            this.graph3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(14.500203132629395D), Telerik.Reporting.Drawing.Unit.Cm(11.499997138977051D));
            this.graph3.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.graph3.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.graph3.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            graphTitle3.Position = Telerik.Reporting.GraphItemPosition.TopLeft;
            graphTitle3.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(152)))), ((int)(((byte)(152)))), ((int)(((byte)(152)))));
            graphTitle3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            graphTitle3.Style.LineColor = System.Drawing.Color.LightGray;
            graphTitle3.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0D);
            graphTitle3.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(26D);
            graphTitle3.Text = "TOP {Parameters.topN.Value} STATES";
            this.graph3.Titles.Add(graphTitle3);
            // 
            // cartesianCoordinateSystem3
            // 
            this.cartesianCoordinateSystem3.Name = "cartesianCoordinateSystem3";
            this.cartesianCoordinateSystem3.XAxis = this.graphAxis5;
            this.cartesianCoordinateSystem3.YAxis = this.graphAxis6;
            // 
            // graphAxis5
            // 
            this.graphAxis5.LabelFormat = "${0}M";
            this.graphAxis5.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis5.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis5.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis5.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis5.MinorGridLineStyle.Visible = false;
            this.graphAxis5.Name = "graphAxis5";
            this.graphAxis5.Scale = numericalScale3;
            // 
            // graphAxis6
            // 
            this.graphAxis6.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis6.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis6.MajorGridLineStyle.Visible = false;
            this.graphAxis6.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis6.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis6.MinorGridLineStyle.Visible = false;
            this.graphAxis6.MinSize = Telerik.Reporting.Drawing.Unit.Cm(2.7999999523162842D);
            this.graphAxis6.Name = "graphAxis6";
            categoryScale2.SpacingSlotCount = 0.5D;
            this.graphAxis6.Scale = categoryScale2;
            this.graphAxis6.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(0D);
            this.graphAxis6.Style.Visible = true;
            // 
            // barSeries2
            // 
            this.barSeries2.ArrangeMode = Telerik.Reporting.GraphSeriesArrangeMode.Stacked;
            this.barSeries2.CategoryGroup = graphGroup5;
            this.barSeries2.CoordinateSystem = this.cartesianCoordinateSystem3;
            this.barSeries2.DataPointLabel = "=Sum(Fields.LineTotal)/1e6";
            this.barSeries2.DataPointLabelStyle.Visible = false;
            this.barSeries2.DataPointStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0D);
            this.barSeries2.DataPointStyle.Visible = true;
            this.barSeries2.LegendItem.Style.BackgroundColor = System.Drawing.Color.Transparent;
            this.barSeries2.LegendItem.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.barSeries2.LegendItem.Style.LineColor = System.Drawing.Color.Transparent;
            this.barSeries2.LegendItem.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0D);
            this.barSeries2.LegendItem.Value = "=Fields.ProductSubCategory";
            this.barSeries2.Name = "barSeries2";
            graphGroup6.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.ProductSubCategory"));
            graphGroup6.Name = "productCategoryGroup1";
            graphGroup6.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.ProductSubCategory", Telerik.Reporting.SortDirection.Asc));
            this.barSeries2.SeriesGroup = graphGroup6;
            this.barSeries2.X = "=Sum(Fields.LineTotal)/1e6";
            this.barSeries2.ToolTip.Title = "=Format('Sales in {0}, {1}', Fields.State, Fields.Country)";
            this.barSeries2.ToolTip.Text = "=Format('{0}: ${1:0.00}M', Fields.ProductSubCategory, Sum(Fields.LineTotal)/1e6)";
            // 
            // textBox1
            // 
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.19688987731933594D), Telerik.Reporting.Drawing.Unit.Inch(0.19688980281352997D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(11.417283058166504D), Telerik.Reporting.Drawing.Unit.Inch(0.590511679649353D));
            this.textBox1.Style.Font.Bold = true;
            this.textBox1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(25D);
            this.textBox1.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(34D);
            this.textBox1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.textBox1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.textBox1.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox1.StyleName = "Header";
            this.textBox1.Value = "{Parameters.category.Label} Sales Distribution By Region";
            // 
            // textBox2
            // 
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.49959805607795715D), Telerik.Reporting.Drawing.Unit.Cm(2.0001997947692871D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(29.000308990478516D), Telerik.Reporting.Drawing.Unit.Cm(0.99989980459213257D));
            this.textBox2.Style.Font.Bold = false;
            this.textBox2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(14D);
            this.textBox2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.textBox2.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox2.Value = "REGION: {Join(\', \', Parameters.country.Value)}\r\nPERIOD: {Join(\', \', Parameters.ye" +
    "ar.Value)}";
            // 
            // SalesByRegionDashboard
            // 
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.detailSection1});
            this.Name = "SalesByRegionDashboard";
            this.PageSettings.Landscape = false;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(1D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.PageSettings.PaperSize = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(14D), Telerik.Reporting.Drawing.Unit.Mm(297D));
            reportParameter1.AutoRefresh = true;
            reportParameter1.AvailableValues.DataSource = this.yearsData;
            reportParameter1.AvailableValues.ValueMember = "= Fields.Year";
            reportParameter1.MultiValue = true;
            reportParameter1.Name = "year";
            reportParameter1.Text = "Year";
            reportParameter1.Type = Telerik.Reporting.ReportParameterType.Integer;
            reportParameter1.Value = "=Array(2003, 2004)";
            reportParameter1.Visible = true;
            reportParameter2.AutoRefresh = true;
            reportParameter2.AvailableValues.DataSource = this.countryData;
            reportParameter2.AvailableValues.ValueMember = "= Fields.Country";
            reportParameter2.MultiValue = true;
            reportParameter2.Name = "country";
            reportParameter2.Text = "Country";
            reportParameter2.Value = "=Array(\'United States\', \'Canada\')";
            reportParameter2.Visible = true;
            reportParameter3.AutoRefresh = true;
            reportParameter3.AvailableValues.DataSource = this.productCategoriesData;
            reportParameter3.AvailableValues.DisplayMember = "= Fields.Name";
            reportParameter3.AvailableValues.ValueMember = "= Fields.ProductCategoryID";
            reportParameter3.Name = "category";
            reportParameter3.Text = "Category";
            reportParameter3.Value = "=1";
            reportParameter4.AutoRefresh = true;
            reportParameter4.Name = "topN";
            reportParameter4.Text = "Top ¹ States";
            reportParameter4.Type = Telerik.Reporting.ReportParameterType.Integer;
            reportParameter4.Value = "12";
            this.ReportParameters.Add(reportParameter1);
            this.ReportParameters.Add(reportParameter2);
            this.ReportParameters.Add(reportParameter3);
            this.ReportParameters.Add(reportParameter4);
            this.Style.BackgroundColor = System.Drawing.Color.White;
            this.Style.Font.Name = "Segoe UI Light";
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
            this.TocText = "Sales By Region";
            this.Width = Telerik.Reporting.Drawing.Unit.Inch(11.81102466583252D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.SqlDataSource yearsData;
        private Telerik.Reporting.SqlDataSource countryData;
        private Telerik.Reporting.SqlDataSource productCategoriesData;
        private Telerik.Reporting.SqlDataSource salesData;
        private Telerik.Reporting.DetailSection detailSection1;
        private Telerik.Reporting.Map map1;
        private Telerik.Reporting.PieMapSeries pieMapSeries1;
        private Telerik.Reporting.Graph graph1;
        private Telerik.Reporting.CartesianCoordinateSystem cartesianCoordinateSystem1;
        private Telerik.Reporting.GraphAxis graphAxis2;
        private Telerik.Reporting.GraphAxis graphAxis1;
        private Telerik.Reporting.LineSeries lineSeries1;
        private Telerik.Reporting.Graph graph2;
        private Telerik.Reporting.CartesianCoordinateSystem cartesianCoordinateSystem2;
        private Telerik.Reporting.GraphAxis graphAxis3;
        private Telerik.Reporting.GraphAxis graphAxis4;
        private Telerik.Reporting.BarSeries barSeries1;
        private Telerik.Reporting.Graph graph3;
        private Telerik.Reporting.CartesianCoordinateSystem cartesianCoordinateSystem3;
        private Telerik.Reporting.GraphAxis graphAxis5;
        private Telerik.Reporting.GraphAxis graphAxis6;
        private Telerik.Reporting.BarSeries barSeries2;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.TextBox textBox2;

    }
}