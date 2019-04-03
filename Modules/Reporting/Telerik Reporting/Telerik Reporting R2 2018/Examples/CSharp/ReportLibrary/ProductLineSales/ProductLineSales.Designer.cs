namespace Telerik.Reporting.Examples.CSharp
{
    using System.ComponentModel;
    using System.Drawing;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    partial class ProductLineSales
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProductLineSales));
            Telerik.Reporting.GraphGroup graphGroup1 = new Telerik.Reporting.GraphGroup();
            Telerik.Reporting.Drawing.ColorPalette colorPalette1 = new Telerik.Reporting.Drawing.ColorPalette();
            Telerik.Reporting.GraphTitle graphTitle1 = new Telerik.Reporting.GraphTitle();
            Telerik.Reporting.NumericalScale numericalScale1 = new Telerik.Reporting.NumericalScale();
            Telerik.Reporting.CategoryScale categoryScale1 = new Telerik.Reporting.CategoryScale();
            Telerik.Reporting.GraphGroup graphGroup2 = new Telerik.Reporting.GraphGroup();
            Telerik.Reporting.GraphGroup graphGroup3 = new Telerik.Reporting.GraphGroup();
            Telerik.Reporting.Drawing.ColorPalette colorPalette2 = new Telerik.Reporting.Drawing.ColorPalette();
            Telerik.Reporting.GraphTitle graphTitle2 = new Telerik.Reporting.GraphTitle();
            Telerik.Reporting.NumericalScale numericalScale2 = new Telerik.Reporting.NumericalScale();
            Telerik.Reporting.CategoryScale categoryScale2 = new Telerik.Reporting.CategoryScale();
            Telerik.Reporting.GraphGroup graphGroup4 = new Telerik.Reporting.GraphGroup();
            Telerik.Reporting.Group group1 = new Telerik.Reporting.Group();
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter2 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter3 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter4 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule4 = new Telerik.Reporting.Drawing.StyleRule();
            this.groupFooterSection1 = new Telerik.Reporting.GroupFooterSection();
            this.groupHeaderSection1 = new Telerik.Reporting.GroupHeaderSection();
            this.textBoxCategories = new Telerik.Reporting.TextBox();
            this.textBoxTitle = new Telerik.Reporting.TextBox();
            this.textBoxTimeInterval = new Telerik.Reporting.TextBox();
            this.shape3 = new Telerik.Reporting.Shape();
            this.productCategoriesDataSource = new Telerik.Reporting.SqlDataSource();
            this.productSubcategoriesDataSource = new Telerik.Reporting.SqlDataSource();
            this.detail = new Telerik.Reporting.DetailSection();
            this.textBoxTopEmployeesHeader = new Telerik.Reporting.TextBox();
            this.textBoxTopStoresHeader = new Telerik.Reporting.TextBox();
            this.graph1 = new Telerik.Reporting.Graph();
            this.cartesianCoordinateSystem2 = new Telerik.Reporting.CartesianCoordinateSystem();
            this.graphAxis3 = new Telerik.Reporting.GraphAxis();
            this.graphAxis4 = new Telerik.Reporting.GraphAxis();
            this.employeesDataSource = new Telerik.Reporting.SqlDataSource();
            this.barSeries1 = new Telerik.Reporting.BarSeries();
            this.graph2 = new Telerik.Reporting.Graph();
            this.polarCoordinateSystem3 = new Telerik.Reporting.PolarCoordinateSystem();
            this.graphAxis7 = new Telerik.Reporting.GraphAxis();
            this.graphAxis8 = new Telerik.Reporting.GraphAxis();
            this.storesDataSource = new Telerik.Reporting.SqlDataSource();
            this.barSeries2 = new Telerik.Reporting.BarSeries();
            this.reportFooterSection1 = new Telerik.Reporting.ReportFooterSection();
            this.textBoxReportFooter = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // groupFooterSection1
            // 
            this.groupFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.0520833320915699D);
            this.groupFooterSection1.Name = "groupFooterSection1";
            this.groupFooterSection1.Style.Visible = false;
            // 
            // groupHeaderSection1
            // 
            this.groupHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(1.1999999284744263D);
            this.groupHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBoxCategories,
            this.textBoxTitle,
            this.textBoxTimeInterval,
            this.shape3});
            this.groupHeaderSection1.Name = "groupHeaderSection1";
            // 
            // textBoxCategories
            // 
            this.textBoxCategories.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.21311600506305695D));
            this.textBoxCategories.Name = "textBoxCategories";
            this.textBoxCategories.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.0999212265014648D), Telerik.Reporting.Drawing.Unit.Inch(0.60000002384185791D));
            this.textBoxCategories.StyleName = "Header";
            this.textBoxCategories.Value = "= Parameters.ProductCategory+\" » \"+Join(\", \", Parameters.ProductSubcategory.Value" +
    ")";
            // 
            // textBoxTitle
            // 
            this.textBoxTitle.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(3.9339065551757812E-05D));
            this.textBoxTitle.Name = "textBoxTitle";
            this.textBoxTitle.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.3020832538604736D), Telerik.Reporting.Drawing.Unit.Inch(0.21299798786640167D));
            this.textBoxTitle.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(47)))), ((int)(((byte)(11)))));
            this.textBoxTitle.Style.Font.Bold = true;
            this.textBoxTitle.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBoxTitle.Value = "PRODUCT LINE SALES";
            // 
            // textBoxTimeInterval
            // 
            this.textBoxTimeInterval.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.4558329582214355D), Telerik.Reporting.Drawing.Unit.Inch(3.9339065551757812E-05D));
            this.textBoxTimeInterval.Name = "textBoxTimeInterval";
            this.textBoxTimeInterval.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.64408802986145D), Telerik.Reporting.Drawing.Unit.Inch(0.21299798786640167D));
            this.textBoxTimeInterval.Style.Font.Bold = true;
            this.textBoxTimeInterval.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBoxTimeInterval.Value = "{Parameters.FromDate.Value.ToString(\"MMM yyyy\")} - {Parameters.ToDate.Value.ToStr" +
    "ing(\"MMM yyyy\")}";
            // 
            // shape3
            // 
            this.shape3.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Left | Telerik.Reporting.AnchoringStyles.Right)));
            this.shape3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.9378803194267675E-05D), Telerik.Reporting.Drawing.Unit.Inch(0.83958339691162109D));
            this.shape3.Name = "shape3";
            this.shape3.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.shape3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.1099214553833D), Telerik.Reporting.Drawing.Unit.Point(3.75D));
            this.shape3.Stretch = true;
            this.shape3.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.shape3.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            this.shape3.Style.BorderWidth.Top = Telerik.Reporting.Drawing.Unit.Point(3D);
            this.shape3.Style.Color = System.Drawing.Color.Transparent;
            // 
            // productCategoriesDataSource
            // 
            this.productCategoriesDataSource.ConnectionString = "Telerik.Reporting.Examples.CSharp.Properties.Settings.TelerikConnectionString";
            this.productCategoriesDataSource.Name = "productCategoriesDataSource";
            this.productCategoriesDataSource.SelectCommand = "SELECT Production.ProductCategory.Name AS ProductCategory\r\nFROM Production.Produc" +
    "tCategory";
            // 
            // productSubcategoriesDataSource
            // 
            this.productSubcategoriesDataSource.ConnectionString = "Telerik.Reporting.Examples.CSharp.Properties.Settings.TelerikConnectionString";
            this.productSubcategoriesDataSource.Name = "productSubcategoriesDataSource";
            this.productSubcategoriesDataSource.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@productCategory", System.Data.DbType.String, "=Parameters.ProductCategory.Value")});
            this.productSubcategoriesDataSource.SelectCommand = resources.GetString("productSubcategoriesDataSource.SelectCommand");
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Inch(7.1000003814697266D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBoxTopEmployeesHeader,
            this.textBoxTopStoresHeader,
            this.graph1,
            this.graph2});
            this.detail.Name = "detail";
            // 
            // textBoxTopEmployeesHeader
            // 
            this.textBoxTopEmployeesHeader.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.9378803194267675E-05D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.textBoxTopEmployeesHeader.Name = "textBoxTopEmployeesHeader";
            this.textBoxTopEmployeesHeader.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.0998811721801758D), Telerik.Reporting.Drawing.Unit.Inch(0.30000004172325134D));
            this.textBoxTopEmployeesHeader.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBoxTopEmployeesHeader.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(105)))), ((int)(((byte)(104)))));
            this.textBoxTopEmployeesHeader.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Top;
            this.textBoxTopEmployeesHeader.StyleName = "Category";
            this.textBoxTopEmployeesHeader.TocText = "Top 10 Employees";
            this.textBoxTopEmployeesHeader.Value = "Top 10 Employees";
            // 
            // textBoxTopStoresHeader
            // 
            this.textBoxTopStoresHeader.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(3.5999999046325684D));
            this.textBoxTopStoresHeader.Name = "textBoxTopStoresHeader";
            this.textBoxTopStoresHeader.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.0999603271484375D), Telerik.Reporting.Drawing.Unit.Inch(0.3333333432674408D));
            this.textBoxTopStoresHeader.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBoxTopStoresHeader.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(105)))), ((int)(((byte)(104)))));
            this.textBoxTopStoresHeader.StyleName = "Category";
            this.textBoxTopStoresHeader.TocText = "Top 10 Stores";
            this.textBoxTopStoresHeader.Value = "Top 10 Stores";
            // 
            // graph1
            // 
            graphGroup1.Filters.Add(new Telerik.Reporting.Filter("=Sum(Fields.SubTotal)", Telerik.Reporting.FilterOperator.TopN, "=10"));
            graphGroup1.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.Employee"));
            graphGroup1.Name = "employeeGroup1";
            graphGroup1.Sortings.Add(new Telerik.Reporting.Sorting("=Sum(Fields.SubTotal)", Telerik.Reporting.SortDirection.Asc));
            this.graph1.CategoryGroups.Add(graphGroup1);
            colorPalette1.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(198)))), ((int)(((byte)(198))))));
            this.graph1.ColorPalette = colorPalette1;
            this.graph1.CoordinateSystems.Add(this.cartesianCoordinateSystem2);
            this.graph1.DataSource = this.employeesDataSource;
            this.graph1.Legend.Style.LineColor = System.Drawing.Color.LightGray;
            this.graph1.Legend.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.graph1.Legend.Style.Visible = false;
            this.graph1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0099999997764825821D), Telerik.Reporting.Drawing.Unit.Inch(0.40000000596046448D));
            this.graph1.Name = "graph1";
            this.graph1.PlotAreaStyle.LineColor = System.Drawing.Color.LightGray;
            this.graph1.PlotAreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.graph1.Series.Add(this.barSeries1);
            this.graph1.SeriesGroups.Add(graphGroup2);
            this.graph1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.1000003814697266D), Telerik.Reporting.Drawing.Unit.Inch(2.9300000667572021D));
            this.graph1.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.graph1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.graph1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.graph1.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            graphTitle1.Position = Telerik.Reporting.GraphItemPosition.TopCenter;
            graphTitle1.Style.LineColor = System.Drawing.Color.LightGray;
            graphTitle1.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            graphTitle1.Style.Visible = false;
            graphTitle1.Text = "graph1";
            this.graph1.Titles.Add(graphTitle1);
            // 
            // cartesianCoordinateSystem2
            // 
            this.cartesianCoordinateSystem2.Name = "cartesianCoordinateSystem2";
            this.cartesianCoordinateSystem2.XAxis = this.graphAxis3;
            this.cartesianCoordinateSystem2.YAxis = this.graphAxis4;
            // 
            // graphAxis3
            // 
            this.graphAxis3.LabelFormat = "{0:C0}K";
            this.graphAxis3.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis3.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis3.MajorTickMarkDisplayType = Telerik.Reporting.GraphAxisTickMarkDisplayType.None;
            this.graphAxis3.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis3.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis3.MinorGridLineStyle.Visible = false;
            this.graphAxis3.Name = "graphAxis3";
            this.graphAxis3.Scale = numericalScale1;
            // 
            // graphAxis4
            // 
            this.graphAxis4.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis4.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis4.MajorGridLineStyle.Visible = false;
            this.graphAxis4.MajorTickMarkDisplayType = Telerik.Reporting.GraphAxisTickMarkDisplayType.None;
            this.graphAxis4.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis4.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis4.MinorGridLineStyle.Visible = false;
            this.graphAxis4.Name = "graphAxis4";
            categoryScale1.SpacingSlotCount = 0.4D;
            this.graphAxis4.Scale = categoryScale1;
            // 
            // employeesDataSource
            // 
            this.employeesDataSource.ConnectionString = "Telerik.Reporting.Examples.CSharp.Properties.Settings.TelerikConnectionString";
            this.employeesDataSource.Name = "employeesDataSource";
            this.employeesDataSource.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@productCategory", System.Data.DbType.String, "=Parameters.ProductCategory.Value"),
            new Telerik.Reporting.SqlDataSourceParameter("@FromDate", System.Data.DbType.DateTime, "=Parameters.FromDate.Value"),
            new Telerik.Reporting.SqlDataSourceParameter("@ToDate", System.Data.DbType.DateTime, "=Parameters.ToDate.Value"),
            new Telerik.Reporting.SqlDataSourceParameter("@productSubCategory", System.Data.DbType.String, "=Parameters.ProductSubcategory.Value")});
            this.employeesDataSource.SelectCommand = resources.GetString("employeesDataSource.SelectCommand");
            // 
            // barSeries1
            // 
            this.barSeries1.CategoryGroup = graphGroup1;
            this.barSeries1.CoordinateSystem = this.cartesianCoordinateSystem2;
            this.barSeries1.DataPointLabel = "=Sum(Fields.SubTotal)";
            this.barSeries1.DataPointLabelStyle.Visible = false;
            this.barSeries1.DataPointStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.barSeries1.DataPointStyle.Visible = true;
            this.barSeries1.LegendItem.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(0D);
            this.barSeries1.LegendItem.Value = "SubTotal";
            graphGroup2.Name = "seriesGroup1";
            this.barSeries1.SeriesGroup = graphGroup2;
            this.barSeries1.X = "=Sum(Fields.SubTotal) /1000.0";
            // 
            // graph2
            // 
            graphGroup3.Name = "categoryGroup1";
            this.graph2.CategoryGroups.Add(graphGroup3);
            colorPalette2.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(25)))), ((int)(((byte)(18))))));
            colorPalette2.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(46)))), ((int)(((byte)(10))))));
            colorPalette2.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(88)))), ((int)(((byte)(55))))));
            colorPalette2.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(90)))), ((int)(((byte)(40))))));
            colorPalette2.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(137)))), ((int)(((byte)(104))))));
            colorPalette2.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(191)))), ((int)(((byte)(105))))));
            colorPalette2.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(137)))), ((int)(((byte)(203)))), ((int)(((byte)(42))))));
            colorPalette2.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(180)))), ((int)(((byte)(114))))));
            colorPalette2.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(198)))), ((int)(((byte)(198))))));
            colorPalette2.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(105)))), ((int)(((byte)(104))))));
            this.graph2.ColorPalette = colorPalette2;
            this.graph2.CoordinateSystems.Add(this.polarCoordinateSystem3);
            this.graph2.DataSource = this.storesDataSource;
            this.graph2.Legend.Style.Font.Name = "Segoe UI";
            this.graph2.Legend.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.graph2.Legend.Style.LineColor = System.Drawing.Color.LightGray;
            this.graph2.Legend.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.graph2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0099999997764825821D), Telerik.Reporting.Drawing.Unit.Inch(4.0999999046325684D));
            this.graph2.Name = "graph2";
            this.graph2.PlotAreaStyle.LineColor = System.Drawing.Color.LightGray;
            this.graph2.PlotAreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.graph2.Series.Add(this.barSeries2);
            this.graph2.SeriesGroups.Add(graphGroup4);
            this.graph2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(7.190000057220459D), Telerik.Reporting.Drawing.Unit.Inch(2.9300000667572021D));
            this.graph2.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(20D);
            this.graph2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.graph2.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(30D);
            this.graph2.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(20D);
            graphTitle2.Position = Telerik.Reporting.GraphItemPosition.TopCenter;
            graphTitle2.Style.LineColor = System.Drawing.Color.LightGray;
            graphTitle2.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.graph2.Titles.Add(graphTitle2);
            // 
            // polarCoordinateSystem3
            // 
            this.polarCoordinateSystem3.AngularAxis = this.graphAxis7;
            this.polarCoordinateSystem3.Name = "polarCoordinateSystem3";
            this.polarCoordinateSystem3.RadialAxis = this.graphAxis8;
            // 
            // graphAxis7
            // 
            this.graphAxis7.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis7.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis7.MajorGridLineStyle.Visible = false;
            this.graphAxis7.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis7.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis7.MinorGridLineStyle.Visible = false;
            this.graphAxis7.Name = "graphAxis7";
            this.graphAxis7.Scale = numericalScale2;
            this.graphAxis7.Style.Visible = false;
            // 
            // graphAxis8
            // 
            this.graphAxis8.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis8.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis8.MajorGridLineStyle.Visible = false;
            this.graphAxis8.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis8.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis8.MinorGridLineStyle.Visible = false;
            this.graphAxis8.Name = "graphAxis8";
            categoryScale2.PositionMode = Telerik.Reporting.AxisPositionMode.OnTicks;
            categoryScale2.SpacingSlotCount = 0D;
            this.graphAxis8.Scale = categoryScale2;
            this.graphAxis8.Style.Visible = false;
            // 
            // storesDataSource
            // 
            this.storesDataSource.ConnectionString = "Telerik.Reporting.Examples.CSharp.Properties.Settings.TelerikConnectionString";
            this.storesDataSource.Name = "storesDataSource";
            this.storesDataSource.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@productCategory", System.Data.DbType.String, "=Parameters.ProductCategory.Value"),
            new Telerik.Reporting.SqlDataSourceParameter("@FromDate", System.Data.DbType.DateTime, "=Parameters.FromDate.Value"),
            new Telerik.Reporting.SqlDataSourceParameter("@ToDate", System.Data.DbType.DateTime, "=Parameters.ToDate.Value")});
            this.storesDataSource.SelectCommand = resources.GetString("storesDataSource.SelectCommand");
            // 
            // barSeries2
            // 
            this.barSeries2.ArrangeMode = Telerik.Reporting.GraphSeriesArrangeMode.Stacked100;
            this.barSeries2.CategoryGroup = graphGroup3;
            this.barSeries2.CoordinateSystem = this.polarCoordinateSystem3;
            this.barSeries2.DataPointLabel = "=Sum(Fields.SubTotal)/1000.0";
            this.barSeries2.DataPointLabelFormat = "{0:C0}K";
            this.barSeries2.DataPointLabelStyle.Visible = true;
            this.barSeries2.DataPointStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.barSeries2.DataPointStyle.Visible = true;
            this.barSeries2.LegendItem.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(0D);
            this.barSeries2.LegendItem.Value = "=Fields.StoreName";
            graphGroup4.Filters.Add(new Telerik.Reporting.Filter("=Sum(Fields.SubTotal)", Telerik.Reporting.FilterOperator.TopN, "=10"));
            graphGroup4.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.StoreName"));
            graphGroup4.Name = "storeNameGroup1";
            graphGroup4.Sortings.Add(new Telerik.Reporting.Sorting("=Sum(Fields.SubTotal)", Telerik.Reporting.SortDirection.Asc));
            this.barSeries2.SeriesGroup = graphGroup4;
            this.barSeries2.X = "=Sum(Fields.SubTotal)";
            // 
            // reportFooterSection1
            // 
            this.reportFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.30000001192092896D);
            this.reportFooterSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBoxReportFooter});
            this.reportFooterSection1.Name = "reportFooterSection1";
            // 
            // textBoxReportFooter
            // 
            this.textBoxReportFooter.Name = "textBoxReportFooter";
            this.textBoxReportFooter.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.0999212265014648D), Telerik.Reporting.Drawing.Unit.Inch(0.30000001192092896D));
            this.textBoxReportFooter.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBoxReportFooter.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBoxReportFooter.Value = "AdventureWorks {Now().Year}";
            // 
            // ProductLineSales
            // 
            this.Culture = new System.Globalization.CultureInfo("en-US");
            this.DocumentName = "= Parameters.ProductCategory.Value + \'Sales(\' + Join(\", \", Parameters.ProductSubc" +
    "ategory.Value) + \')[\' + NOW().ToString(\'MMMM yyyy\') + \']\'";
            group1.GroupFooter = this.groupFooterSection1;
            group1.GroupHeader = this.groupHeaderSection1;
            group1.Name = "group1";
            this.Groups.AddRange(new Telerik.Reporting.Group[] {
            group1});
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.groupHeaderSection1,
            this.groupFooterSection1,
            this.detail,
            this.reportFooterSection1});
            this.Name = "ProductLineSales";
            this.PageSettings.Landscape = false;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Inch(0.30000001192092896D), Telerik.Reporting.Drawing.Unit.Inch(0.40000000596046448D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Letter;
            reportParameter1.AutoRefresh = true;
            reportParameter1.AvailableValues.DataSource = this.productCategoriesDataSource;
            reportParameter1.AvailableValues.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.ProductCategory", Telerik.Reporting.SortDirection.Asc));
            reportParameter1.AvailableValues.ValueMember = "ProductCategory";
            reportParameter1.Name = "ProductCategory";
            reportParameter1.Text = "Category";
            reportParameter1.Value = "Bikes";
            reportParameter1.Visible = true;
            reportParameter2.AutoRefresh = true;
            reportParameter2.AvailableValues.DataSource = this.productSubcategoriesDataSource;
            reportParameter2.AvailableValues.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.ProductSubcategory", Telerik.Reporting.SortDirection.Asc));
            reportParameter2.AvailableValues.ValueMember = "ProductSubcategory";
            reportParameter2.MultiValue = true;
            reportParameter2.Name = "ProductSubcategory";
            reportParameter2.Text = "Subcategory";
            reportParameter2.Value = "=First(Fields.ProductSubcategory)";
            reportParameter2.Visible = true;
            reportParameter3.Name = "FromDate";
            reportParameter3.Type = Telerik.Reporting.ReportParameterType.DateTime;
            reportParameter3.Value = "=#2001-01-01#";
            reportParameter4.Name = "ToDate";
            reportParameter4.Type = Telerik.Reporting.ReportParameterType.DateTime;
            reportParameter4.Value = "=#2004-12-31#";
            this.ReportParameters.Add(reportParameter1);
            this.ReportParameters.Add(reportParameter2);
            this.ReportParameters.Add(reportParameter3);
            this.ReportParameters.Add(reportParameter4);
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Header")});
            styleRule2.Style.Font.Bold = true;
            styleRule2.Style.Font.Name = "Segoe UI Light";
            styleRule2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(25D);
            styleRule2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0D);
            styleRule3.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextBox))});
            styleRule3.Style.Font.Name = "Segoe UI";
            styleRule3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            styleRule3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            styleRule4.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Category")});
            styleRule4.Style.Font.Name = "Segoe UI";
            styleRule4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(13D);
            styleRule4.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1,
            styleRule2,
            styleRule3,
            styleRule4});
            this.TocText = "Product Line Sales";
            this.Width = Telerik.Reporting.Drawing.Unit.Inch(8.1100006103515625D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private DetailSection detail;
        private TextBox textBoxTopEmployeesHeader;
        private TextBox textBoxTopStoresHeader;
        private GroupFooterSection groupFooterSection1;
        private GroupHeaderSection groupHeaderSection1;
        private TextBox textBoxCategories;
        private TextBox textBoxTitle;
        private ReportFooterSection reportFooterSection1;
        private TextBox textBoxReportFooter;
        private TextBox textBoxTimeInterval;
        private Shape shape3;
        private Reporting.Graph graph1;
        private CartesianCoordinateSystem cartesianCoordinateSystem2;
        private GraphAxis graphAxis3;
        private GraphAxis graphAxis4;
        private SqlDataSource employeesDataSource;
        private BarSeries barSeries1;
        private Reporting.Graph graph2;
        private PolarCoordinateSystem polarCoordinateSystem3;
        private GraphAxis graphAxis7;
        private GraphAxis graphAxis8;
        private SqlDataSource storesDataSource;
        private BarSeries barSeries2;
        private SqlDataSource productCategoriesDataSource;
        private SqlDataSource productSubcategoriesDataSource;
    }
}