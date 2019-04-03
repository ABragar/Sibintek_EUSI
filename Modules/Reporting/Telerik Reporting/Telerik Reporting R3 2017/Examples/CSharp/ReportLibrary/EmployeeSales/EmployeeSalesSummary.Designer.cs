namespace Telerik.Reporting.Examples.CSharp
{
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    partial class EmployeeSalesSummary
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.Reporting.NavigateToReportAction navigateToReportAction1 = new Telerik.Reporting.NavigateToReportAction();
            Telerik.Reporting.TypeReportSource typeReportSource1 = new Telerik.Reporting.TypeReportSource();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmployeeSalesSummary));
            Telerik.Reporting.GraphGroup graphGroup1 = new Telerik.Reporting.GraphGroup();
            Telerik.Reporting.Drawing.ColorPalette colorPalette1 = new Telerik.Reporting.Drawing.ColorPalette();
            Telerik.Reporting.GraphTitle graphTitle1 = new Telerik.Reporting.GraphTitle();
            Telerik.Reporting.CategoryScale categoryScale1 = new Telerik.Reporting.CategoryScale();
            Telerik.Reporting.NumericalScale numericalScale1 = new Telerik.Reporting.NumericalScale();
            Telerik.Reporting.GraphGroup graphGroup2 = new Telerik.Reporting.GraphGroup();
            Telerik.Reporting.GraphGroup graphGroup3 = new Telerik.Reporting.GraphGroup();
            Telerik.Reporting.Drawing.ColorPalette colorPalette2 = new Telerik.Reporting.Drawing.ColorPalette();
            Telerik.Reporting.GraphTitle graphTitle2 = new Telerik.Reporting.GraphTitle();
            Telerik.Reporting.CategoryScale categoryScale2 = new Telerik.Reporting.CategoryScale();
            Telerik.Reporting.NumericalScale numericalScale2 = new Telerik.Reporting.NumericalScale();
            Telerik.Reporting.Drawing.ColorPalette colorPalette3 = new Telerik.Reporting.Drawing.ColorPalette();
            Telerik.Reporting.GraphGroup graphGroup4 = new Telerik.Reporting.GraphGroup();
            Telerik.Reporting.TableGroup tableGroup1 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup2 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup3 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup4 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup5 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup6 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup7 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup8 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup9 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup10 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup11 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup12 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.Group group1 = new Telerik.Reporting.Group();
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter2 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule4 = new Telerik.Reporting.Drawing.StyleRule();
            this.textBoxTableTitle = new Telerik.Reporting.TextBox();
            this.textBoxCategoryAmount = new Telerik.Reporting.TextBox();
            this.textBoxCategory = new Telerik.Reporting.TextBox();
            this.textBoxSalesOrderNumber = new Telerik.Reporting.TextBox();
            this.textBoxOrderTotal = new Telerik.Reporting.TextBox();
            this.textBox4 = new Telerik.Reporting.TextBox();
            this.textBox12 = new Telerik.Reporting.TextBox();
            this.textBoxEmployeeSalesTotal = new Telerik.Reporting.TextBox();
            this.textBoxTotalLabel = new Telerik.Reporting.TextBox();
            this.textBox10 = new Telerik.Reporting.TextBox();
            this.groupFooterSection1 = new Telerik.Reporting.GroupFooterSection();
            this.groupHeaderSection1 = new Telerik.Reporting.GroupHeaderSection();
            this.textBoxLogo = new Telerik.Reporting.TextBox();
            this.textBoxDataRange = new Telerik.Reporting.TextBox();
            this.textBoxReportEmployee = new Telerik.Reporting.TextBox();
            this.shape3 = new Telerik.Reporting.Shape();
            this.Employees = new Telerik.Reporting.SqlDataSource();
            this.detail = new Telerik.Reporting.DetailSection();
            this.panelWrapper = new Telerik.Reporting.Panel();
            this.graph1 = new Telerik.Reporting.Graph();
            this.cartesianCoordinateSystem1 = new Telerik.Reporting.CartesianCoordinateSystem();
            this.graphAxis2 = new Telerik.Reporting.GraphAxis();
            this.graphAxis1 = new Telerik.Reporting.GraphAxis();
            this.mainDataSource = new Telerik.Reporting.SqlDataSource();
            this.lineSeries1 = new Telerik.Reporting.AreaSeries();
            this.graph2 = new Telerik.Reporting.Graph();
            this.cartesianCoordinateSystem3 = new Telerik.Reporting.CartesianCoordinateSystem();
            this.graphAxis6 = new Telerik.Reporting.GraphAxis();
            this.graphAxis5 = new Telerik.Reporting.GraphAxis();
            this.barSeries1 = new Telerik.Reporting.BarSeries();
            this.crosstabEmployeeSales = new Telerik.Reporting.Crosstab();
            this.textBox6 = new Telerik.Reporting.TextBox();
            this.textBox5 = new Telerik.Reporting.TextBox();
            this.textBox9 = new Telerik.Reporting.TextBox();
            this.textBox13 = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // textBoxTableTitle
            // 
            this.textBoxTableTitle.Name = "textBoxTableTitle";
            this.textBoxTableTitle.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.0799999237060547D), Telerik.Reporting.Drawing.Unit.Inch(0.47999998927116394D));
            this.textBoxTableTitle.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBoxTableTitle.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(16D);
            this.textBoxTableTitle.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Inch(0.079999998211860657D);
            this.textBoxTableTitle.Value = "{Parameters.ReportDate.Value.ToString(\"MMMM yyyy\")} Order Summary";
            // 
            // textBoxCategoryAmount
            // 
            this.textBoxCategoryAmount.Format = "{0:N2}";
            this.textBoxCategoryAmount.Name = "textBoxCategoryAmount";
            this.textBoxCategoryAmount.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.75D), Telerik.Reporting.Drawing.Unit.Inch(0.23999999463558197D));
            this.textBoxCategoryAmount.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBoxCategoryAmount.Value = "=Sum(Fields.LineTotal)";
            // 
            // textBoxCategory
            // 
            this.textBoxCategory.Name = "textBoxCategory";
            this.textBoxCategory.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(5.130000114440918D), Telerik.Reporting.Drawing.Unit.Inch(0.23999999463558197D));
            this.textBoxCategory.Value = "=Fields.Category";
            // 
            // textBoxSalesOrderNumber
            // 
            typeReportSource1.Parameters.Add(new Telerik.Reporting.Parameter("OrderNumber", "=Fields.SalesOrderNumber"));
            typeReportSource1.Parameters.Add(new Telerik.Reporting.Parameter("ForYear", "=Parameters.ReportDate.Value.Year"));
            typeReportSource1.Parameters.Add(new Telerik.Reporting.Parameter("ForMonth", "=Parameters.ReportDate.Value.Month"));
            typeReportSource1.TypeName = "Telerik.Reporting.Examples.CSharp.Invoice, CSharp.ReportLibrary, Version=1.0.0.0," +
    " Culture=neutral, PublicKeyToken=null";
            navigateToReportAction1.ReportSource = typeReportSource1;
            this.textBoxSalesOrderNumber.Action = navigateToReportAction1;
            this.textBoxSalesOrderNumber.Name = "textBoxSalesOrderNumber";
            this.textBoxSalesOrderNumber.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2100000381469727D), Telerik.Reporting.Drawing.Unit.Inch(0.23999999463558197D));
            this.textBoxSalesOrderNumber.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(105)))), ((int)(((byte)(104)))));
            this.textBoxSalesOrderNumber.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Inch(0.05000000074505806D);
            this.textBoxSalesOrderNumber.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Top;
            this.textBoxSalesOrderNumber.Value = "=Fields.SalesOrderNumber";
            // 
            // textBoxOrderTotal
            // 
            this.textBoxOrderTotal.Format = "{0:N2}";
            this.textBoxOrderTotal.Name = "textBoxOrderTotal";
            this.textBoxOrderTotal.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.75D), Telerik.Reporting.Drawing.Unit.Inch(0.23999999463558197D));
            this.textBoxOrderTotal.Style.BorderColor.Default = System.Drawing.Color.Black;
            this.textBoxOrderTotal.Style.BorderColor.Top = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.textBoxOrderTotal.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBoxOrderTotal.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBoxOrderTotal.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBoxOrderTotal.Value = "=Sum(Fields.LineTotal)";
            // 
            // textBox4
            // 
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(5.130000114440918D), Telerik.Reporting.Drawing.Unit.Inch(0.23999999463558197D));
            this.textBox4.Style.BorderColor.Default = System.Drawing.Color.Black;
            this.textBox4.Style.BorderColor.Top = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.textBox4.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox4.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            // 
            // textBox12
            // 
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2100000381469727D), Telerik.Reporting.Drawing.Unit.Inch(0.23999999463558197D));
            this.textBox12.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            // 
            // textBoxEmployeeSalesTotal
            // 
            this.textBoxEmployeeSalesTotal.Format = "{0:N2}";
            this.textBoxEmployeeSalesTotal.Name = "textBoxEmployeeSalesTotal";
            this.textBoxEmployeeSalesTotal.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.75D), Telerik.Reporting.Drawing.Unit.Inch(0.23999999463558197D));
            this.textBoxEmployeeSalesTotal.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBoxEmployeeSalesTotal.Style.BorderWidth.Bottom = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.textBoxEmployeeSalesTotal.Style.Font.Bold = true;
            this.textBoxEmployeeSalesTotal.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBoxEmployeeSalesTotal.Value = "=Sum(Fields.LineTotal)";
            // 
            // textBoxTotalLabel
            // 
            this.textBoxTotalLabel.Name = "textBoxTotalLabel";
            this.textBoxTotalLabel.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(5.130000114440918D), Telerik.Reporting.Drawing.Unit.Inch(0.23999999463558197D));
            this.textBoxTotalLabel.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBoxTotalLabel.Style.BorderWidth.Bottom = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.textBoxTotalLabel.Value = "Total:";
            // 
            // textBox10
            // 
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2100000381469727D), Telerik.Reporting.Drawing.Unit.Inch(0.23999999463558197D));
            this.textBox10.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox10.Style.BorderWidth.Bottom = Telerik.Reporting.Drawing.Unit.Point(2D);
            // 
            // groupFooterSection1
            // 
            this.groupFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.1272023469209671D);
            this.groupFooterSection1.Name = "groupFooterSection1";
            this.groupFooterSection1.Style.Visible = false;
            // 
            // groupHeaderSection1
            // 
            this.groupHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(1D);
            this.groupHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBoxLogo,
            this.textBoxDataRange,
            this.textBoxReportEmployee,
            this.shape3});
            this.groupHeaderSection1.Name = "groupHeaderSection1";
            this.groupHeaderSection1.PrintOnEveryPage = true;
            // 
            // textBoxLogo
            // 
            this.textBoxLogo.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(3.9339065551757812E-05D));
            this.textBoxLogo.Name = "textBoxLogo";
            this.textBoxLogo.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1972219944000244D), Telerik.Reporting.Drawing.Unit.Inch(0.20000004768371582D));
            this.textBoxLogo.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(47)))), ((int)(((byte)(11)))));
            this.textBoxLogo.Style.Font.Bold = true;
            this.textBoxLogo.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBoxLogo.Value = "SALES REPORT";
            // 
            // textBoxDataRange
            // 
            this.textBoxDataRange.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.6000003814697266D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.textBoxDataRange.Name = "textBoxDataRange";
            this.textBoxDataRange.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.4999597072601318D), Telerik.Reporting.Drawing.Unit.Inch(0.19837446510791779D));
            this.textBoxDataRange.Style.Font.Bold = true;
            this.textBoxDataRange.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBoxDataRange.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBoxDataRange.Value = "=Parameters.ReportDate.Value.ToString(\'MMM yyyy\')";
            // 
            // textBoxReportEmployee
            // 
            this.textBoxReportEmployee.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.20011806488037109D));
            this.textBoxReportEmployee.Name = "textBoxReportEmployee";
            this.textBoxReportEmployee.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.0999603271484375D), Telerik.Reporting.Drawing.Unit.Inch(0.60000002384185791D));
            this.textBoxReportEmployee.StyleName = "Header";
            this.textBoxReportEmployee.Value = "=Parameters.Employee.Label";
            // 
            // shape3
            // 
            this.shape3.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Left | Telerik.Reporting.AnchoringStyles.Right)));
            this.shape3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.80019682645797729D));
            this.shape3.Name = "shape3";
            this.shape3.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.shape3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.0999603271484375D), Telerik.Reporting.Drawing.Unit.Point(2D));
            this.shape3.Stretch = true;
            this.shape3.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.shape3.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            this.shape3.Style.BorderWidth.Bottom = Telerik.Reporting.Drawing.Unit.Point(0.5D);
            this.shape3.Style.BorderWidth.Top = Telerik.Reporting.Drawing.Unit.Point(1.5D);
            this.shape3.Style.Color = System.Drawing.Color.Transparent;
            // 
            // Employees
            // 
            this.Employees.ConnectionString = "Telerik.Reporting.Examples.CSharp.Properties.Settings.TelerikConnectionString";
            this.Employees.Name = "Employees";
            this.Employees.SelectCommand = resources.GetString("Employees.SelectCommand");
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Inch(6.8000006675720215D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.panelWrapper});
            this.detail.KeepTogether = false;
            this.detail.Name = "detail";
            // 
            // panelWrapper
            // 
            this.panelWrapper.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.graph1,
            this.graph2,
            this.crosstabEmployeeSales});
            this.panelWrapper.KeepTogether = false;
            this.panelWrapper.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.9378803194267675E-05D), Telerik.Reporting.Drawing.Unit.Inch(7.8678131103515625E-05D));
            this.panelWrapper.Name = "panelWrapper";
            this.panelWrapper.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.1000394821167D), Telerik.Reporting.Drawing.Unit.Inch(6.79992151260376D));
            // 
            // graph1
            // 
            graphGroup1.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.OrderDate.Month"));
            graphGroup1.Label = "= Fields.OrderDate.ToString(\"MMMM\")";
            graphGroup1.Name = "orderDate.MonthGroup1";
            graphGroup1.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.OrderDate.Month", Telerik.Reporting.SortDirection.Asc));
            this.graph1.CategoryGroups.Add(graphGroup1);
            colorPalette1.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(150)))), ((int)(((byte)(70))))));
            colorPalette1.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(172)))), ((int)(((byte)(198))))));
            colorPalette1.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(100)))), ((int)(((byte)(162))))));
            colorPalette1.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(87)))), ((int)(((byte)(8))))));
            colorPalette1.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(106)))), ((int)(((byte)(124))))));
            colorPalette1.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(59)))), ((int)(((byte)(98))))));
            this.graph1.ColorPalette = colorPalette1;
            this.graph1.CoordinateSystems.Add(this.cartesianCoordinateSystem1);
            this.graph1.DataSource = this.mainDataSource;
            this.graph1.Legend.Position = Telerik.Reporting.GraphItemPosition.RightCenter;
            this.graph1.Legend.Style.LineColor = System.Drawing.Color.LightGray;
            this.graph1.Legend.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.graph1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.9458274841308594E-05D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.graph1.Name = "graph1";
            this.graph1.PlotAreaStyle.LineColor = System.Drawing.Color.LightGray;
            this.graph1.PlotAreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.graph1.Series.Add(this.lineSeries1);
            this.graph1.SeriesGroups.Add(graphGroup2);
            this.graph1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.1000003814697266D), Telerik.Reporting.Drawing.Unit.Inch(2.4000000953674316D));
            this.graph1.Style.Font.Name = "Segoe UI";
            this.graph1.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.graph1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.graph1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.graph1.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            graphTitle1.Position = Telerik.Reporting.GraphItemPosition.TopCenter;
            graphTitle1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(14D);
            graphTitle1.Style.LineColor = System.Drawing.Color.LightGray;
            graphTitle1.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            graphTitle1.Text = "Yearly Sales Comparison";
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
            this.graphAxis2.LabelAngle = -90;
            this.graphAxis2.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis2.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis2.MajorGridLineStyle.Visible = true;
            this.graphAxis2.MajorTickMarkDisplayType = Telerik.Reporting.GraphAxisTickMarkDisplayType.None;
            this.graphAxis2.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis2.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis2.MinorGridLineStyle.Visible = false;
            this.graphAxis2.Name = "graphAxis2";
            this.graphAxis2.Scale = categoryScale1;
            this.graphAxis2.Style.Font.Name = "Segoe UI";
            this.graphAxis2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.graphAxis2.Style.Visible = true;
            // 
            // graphAxis1
            // 
            this.graphAxis1.LabelFormat = "{0:C0}K";
            this.graphAxis1.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis1.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis1.MajorGridLineStyle.Visible = true;
            this.graphAxis1.MajorTickMarkDisplayType = Telerik.Reporting.GraphAxisTickMarkDisplayType.None;
            this.graphAxis1.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis1.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis1.MinorGridLineStyle.Visible = false;
            this.graphAxis1.Name = "graphAxis1";
            this.graphAxis1.Scale = numericalScale1;
            this.graphAxis1.Style.Font.Name = "Segoe UI";
            this.graphAxis1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.graphAxis1.Style.Visible = true;
            // 
            // mainDataSource
            // 
            this.mainDataSource.ConnectionString = "Telerik.Reporting.Examples.CSharp.Properties.Settings.TelerikConnectionString";
            this.mainDataSource.Name = "mainDataSource";
            this.mainDataSource.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@ReportYear", System.Data.DbType.String, null),
            new Telerik.Reporting.SqlDataSourceParameter("@ReportMonth", System.Data.DbType.String, null),
            new Telerik.Reporting.SqlDataSourceParameter("@EmployeeID", System.Data.DbType.Int32, "=Parameters.Employee.Value")});
            this.mainDataSource.SelectCommand = resources.GetString("mainDataSource.SelectCommand");
            // 
            // lineSeries1
            // 
            this.lineSeries1.AreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.lineSeries1.ArrangeMode = Telerik.Reporting.GraphSeriesArrangeMode.Stacked;
            this.lineSeries1.CategoryGroup = graphGroup1;
            this.lineSeries1.CoordinateSystem = this.cartesianCoordinateSystem1;
            this.lineSeries1.DataPointLabel = "=Sum(Fields.LineTotal)";
            this.lineSeries1.DataPointLabelStyle.Visible = false;
            this.lineSeries1.DataPointStyle.Visible = false;
            this.lineSeries1.LegendItem.Value = "=Fields.OrderDate.Year";
            this.lineSeries1.MarkerSize = Telerik.Reporting.Drawing.Unit.Mm(2D);
            this.lineSeries1.Name = "lineSeries1";
            graphGroup2.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.OrderDate.Year"));
            graphGroup2.Name = "orderDate.YearGroup1";
            graphGroup2.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.OrderDate.Year", Telerik.Reporting.SortDirection.Asc));
            this.lineSeries1.SeriesGroup = graphGroup2;
            this.lineSeries1.Y = "=ISNULL(Sum(Fields.LineTotal), 0) / 1000.0";
            // 
            // graph2
            // 
            graphGroup3.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.Category"));
            graphGroup3.Name = "categoryGroup1";
            graphGroup3.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.Category", Telerik.Reporting.SortDirection.Asc));
            this.graph2.CategoryGroups.Add(graphGroup3);
            colorPalette2.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(150)))), ((int)(((byte)(70))))));
            colorPalette2.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(172)))), ((int)(((byte)(198))))));
            colorPalette2.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(100)))), ((int)(((byte)(162))))));
            colorPalette2.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(87)))), ((int)(((byte)(8))))));
            colorPalette2.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(106)))), ((int)(((byte)(124))))));
            colorPalette2.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(59)))), ((int)(((byte)(98))))));
            this.graph2.ColorPalette = colorPalette2;
            this.graph2.CoordinateSystems.Add(this.cartesianCoordinateSystem3);
            this.graph2.DataSource = this.mainDataSource;
            this.graph2.Legend.IsInsidePlotArea = false;
            this.graph2.Legend.Position = Telerik.Reporting.GraphItemPosition.RightCenter;
            this.graph2.Legend.Style.Font.Name = "Segoe UI";
            this.graph2.Legend.Style.LineColor = System.Drawing.Color.LightGray;
            this.graph2.Legend.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.graph2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(2.5999214649200439D));
            this.graph2.Name = "graph2";
            this.graph2.PlotAreaStyle.LineColor = System.Drawing.Color.LightGray;
            this.graph2.PlotAreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.graph2.Series.Add(this.barSeries1);
            this.graph2.SeriesGroups.Add(graphGroup4);
            this.graph2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.1000003814697266D), Telerik.Reporting.Drawing.Unit.Inch(2.4000000953674316D));
            this.graph2.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.graph2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.graph2.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.graph2.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            graphTitle2.Position = Telerik.Reporting.GraphItemPosition.TopCenter;
            graphTitle2.Style.Font.Name = "Segoe UI";
            graphTitle2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(14D);
            graphTitle2.Style.LineColor = System.Drawing.Color.LightGray;
            graphTitle2.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            graphTitle2.Text = "Category Sales Comparison";
            this.graph2.Titles.Add(graphTitle2);
            // 
            // cartesianCoordinateSystem3
            // 
            this.cartesianCoordinateSystem3.Name = "cartesianCoordinateSystem3";
            this.cartesianCoordinateSystem3.XAxis = this.graphAxis6;
            this.cartesianCoordinateSystem3.YAxis = this.graphAxis5;
            // 
            // graphAxis6
            // 
            this.graphAxis6.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis6.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis6.MajorGridLineStyle.Visible = false;
            this.graphAxis6.MajorTickMarkDisplayType = Telerik.Reporting.GraphAxisTickMarkDisplayType.None;
            this.graphAxis6.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis6.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis6.MinorGridLineStyle.Visible = false;
            this.graphAxis6.Name = "graphAxis6";
            this.graphAxis6.Scale = categoryScale2;
            this.graphAxis6.Style.Font.Name = "Segoe UI";
            this.graphAxis6.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            // 
            // graphAxis5
            // 
            this.graphAxis5.LabelFormat = "{0:C0}K";
            this.graphAxis5.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis5.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis5.MajorTickMarkDisplayType = Telerik.Reporting.GraphAxisTickMarkDisplayType.None;
            this.graphAxis5.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis5.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis5.MinorGridLineStyle.Visible = false;
            this.graphAxis5.Name = "graphAxis5";
            this.graphAxis5.Scale = numericalScale2;
            this.graphAxis5.Style.Font.Name = "Segoe UI";
            this.graphAxis5.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            // 
            // barSeries1
            // 
            this.barSeries1.CategoryGroup = graphGroup3;
            colorPalette3.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(150)))), ((int)(((byte)(70))))));
            colorPalette3.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(172)))), ((int)(((byte)(198))))));
            colorPalette3.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(100)))), ((int)(((byte)(162))))));
            colorPalette3.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(87)))), ((int)(((byte)(8))))));
            colorPalette3.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(106)))), ((int)(((byte)(124))))));
            colorPalette3.Colors.Add(System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(59)))), ((int)(((byte)(98))))));
            this.barSeries1.ColorPalette = colorPalette3;
            this.barSeries1.CoordinateSystem = this.cartesianCoordinateSystem3;
            this.barSeries1.DataPointLabel = "=Sum(Fields.LineTotal)";
            this.barSeries1.DataPointLabelStyle.Visible = false;
            this.barSeries1.DataPointStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.barSeries1.DataPointStyle.Visible = true;
            this.barSeries1.LegendItem.Value = "=Fields.OrderDate.Year";
            graphGroup4.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.OrderDate.Year"));
            graphGroup4.Name = "orderDate.YearGroup2";
            graphGroup4.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.OrderDate.Year", Telerik.Reporting.SortDirection.Asc));
            this.barSeries1.SeriesGroup = graphGroup4;
            this.barSeries1.Y = "=Sum(Fields.LineTotal) / 1000.0";
            // 
            // crosstabEmployeeSales
            // 
            this.crosstabEmployeeSales.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0D)));
            this.crosstabEmployeeSales.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Inch(0.48000001907348633D)));
            this.crosstabEmployeeSales.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Inch(0.24000000953674316D)));
            this.crosstabEmployeeSales.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Inch(0.24000000953674316D)));
            this.crosstabEmployeeSales.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Inch(0.24000000953674316D)));
            this.crosstabEmployeeSales.Body.SetCellContent(1, 0, this.textBox6);
            this.crosstabEmployeeSales.Body.SetCellContent(0, 0, this.textBox5);
            this.crosstabEmployeeSales.Body.SetCellContent(2, 0, this.textBox9);
            this.crosstabEmployeeSales.Body.SetCellContent(3, 0, this.textBox13);
            this.crosstabEmployeeSales.ColumnGroups.Add(tableGroup1);
            this.crosstabEmployeeSales.DataSource = this.mainDataSource;
            this.crosstabEmployeeSales.Filters.Add(new Telerik.Reporting.Filter("= Fields.OrderDate.Year", Telerik.Reporting.FilterOperator.Equal, "=Parameters.ReportDate.Value.Year"));
            this.crosstabEmployeeSales.Filters.Add(new Telerik.Reporting.Filter("= Fields.OrderDate.Month", Telerik.Reporting.FilterOperator.Equal, "=Parameters.ReportDate.Value.Month"));
            this.crosstabEmployeeSales.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox6,
            this.textBox5,
            this.textBox9,
            this.textBox13,
            this.textBoxTableTitle,
            this.textBoxSalesOrderNumber,
            this.textBoxCategory,
            this.textBoxCategoryAmount,
            this.textBox12,
            this.textBox4,
            this.textBoxOrderTotal,
            this.textBox10,
            this.textBoxTotalLabel,
            this.textBoxEmployeeSalesTotal});
            this.crosstabEmployeeSales.KeepTogether = false;
            this.crosstabEmployeeSales.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(5.4999213218688965D));
            this.crosstabEmployeeSales.Name = "crosstabEmployeeSales";
            this.crosstabEmployeeSales.NoDataMessage = "= \"No Sales for \" + Parameters.ReportDate.Value.ToString(\"MMMM, yyyy\")";
            this.crosstabEmployeeSales.NoDataStyle.Color = System.Drawing.Color.Red;
            tableGroup2.Name = "RowGroup";
            tableGroup2.ReportItem = this.textBoxTableTitle;
            tableGroup2.Sortings.Add(new Telerik.Reporting.Sorting("1", Telerik.Reporting.SortDirection.Asc));
            tableGroup6.Name = "Detail";
            tableGroup6.ReportItem = this.textBoxCategoryAmount;
            tableGroup5.ChildGroups.Add(tableGroup6);
            tableGroup5.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.Category"));
            tableGroup5.Name = "Category";
            tableGroup5.ReportItem = this.textBoxCategory;
            tableGroup5.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.Category", Telerik.Reporting.SortDirection.Asc));
            tableGroup4.ChildGroups.Add(tableGroup5);
            tableGroup4.Name = "Group6";
            tableGroup4.ReportItem = this.textBoxSalesOrderNumber;
            tableGroup9.Name = "Group2";
            tableGroup9.ReportItem = this.textBoxOrderTotal;
            tableGroup8.ChildGroups.Add(tableGroup9);
            tableGroup8.Name = "Group1";
            tableGroup8.ReportItem = this.textBox4;
            tableGroup7.ChildGroups.Add(tableGroup8);
            tableGroup7.Name = "Group7";
            tableGroup7.ReportItem = this.textBox12;
            tableGroup3.ChildGroups.Add(tableGroup4);
            tableGroup3.ChildGroups.Add(tableGroup7);
            tableGroup3.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.SalesOrderID"));
            tableGroup3.Name = "SalesOrder";
            tableGroup3.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.SalesOrderNumber", Telerik.Reporting.SortDirection.Asc));
            tableGroup12.Name = "Group4";
            tableGroup12.ReportItem = this.textBoxEmployeeSalesTotal;
            tableGroup11.ChildGroups.Add(tableGroup12);
            tableGroup11.Name = "Group5";
            tableGroup11.ReportItem = this.textBoxTotalLabel;
            tableGroup10.ChildGroups.Add(tableGroup11);
            tableGroup10.Name = "Group3";
            tableGroup10.ReportItem = this.textBox10;
            this.crosstabEmployeeSales.RowGroups.Add(tableGroup2);
            this.crosstabEmployeeSales.RowGroups.Add(tableGroup3);
            this.crosstabEmployeeSales.RowGroups.Add(tableGroup10);
            this.crosstabEmployeeSales.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.09000015258789D), Telerik.Reporting.Drawing.Unit.Inch(1.2000000476837158D));
            this.crosstabEmployeeSales.Style.BorderColor.Default = System.Drawing.Color.FromArgb(((int)(((byte)(137)))), ((int)(((byte)(145)))), ((int)(((byte)(164)))));
            // 
            // textBox6
            // 
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.23999999463558197D));
            // 
            // textBox5
            // 
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.47999998927116394D));
            this.textBox5.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox5.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            // 
            // textBox9
            // 
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.23999999463558197D));
            // 
            // textBox13
            // 
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.23999999463558197D));
            // 
            // EmployeeSalesSummary
            // 
            group1.GroupFooter = this.groupFooterSection1;
            group1.GroupHeader = this.groupHeaderSection1;
            group1.Groupings.Add(new Telerik.Reporting.Grouping(""));
            group1.Name = "group1";
            this.Groups.AddRange(new Telerik.Reporting.Group[] {
            group1});
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.groupHeaderSection1,
            this.groupFooterSection1,
            this.detail});
            this.Name = "EmployeeSalesSummary";
            this.PageSettings.Landscape = false;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Inch(0.30000001192092896D), Telerik.Reporting.Drawing.Unit.Inch(0.40000000596046448D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Letter;
            reportParameter1.AutoRefresh = true;
            reportParameter1.Name = "ReportDate";
            reportParameter1.Text = "Report Date";
            reportParameter1.Type = Telerik.Reporting.ReportParameterType.DateTime;
            reportParameter1.Value = "=#2003-07-01#";
            reportParameter1.Visible = true;
            reportParameter2.AutoRefresh = true;
            reportParameter2.AvailableValues.DataSource = this.Employees;
            reportParameter2.AvailableValues.DisplayMember = "= Fields.Employee";
            reportParameter2.AvailableValues.ValueMember = "= Fields.SalesPersonID";
            reportParameter2.Name = "Employee";
            reportParameter2.Text = "Employee";
            reportParameter2.Type = Telerik.Reporting.ReportParameterType.Integer;
            reportParameter2.Value = "=283";
            reportParameter2.Visible = true;
            this.ReportParameters.Add(reportParameter1);
            this.ReportParameters.Add(reportParameter2);
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
            styleRule2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Top;
            styleRule3.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Category")});
            styleRule3.Style.Font.Name = "Segoe UI Light";
            styleRule3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(16D);
            styleRule3.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0D);
            styleRule3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Top;
            styleRule4.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextBox))});
            styleRule4.Style.Font.Name = "Segoe UI";
            styleRule4.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Inch(0D);
            styleRule4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom;
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1,
            styleRule2,
            styleRule3,
            styleRule4});
            this.Width = Telerik.Reporting.Drawing.Unit.Inch(8.09999942779541D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.Panel panelWrapper;
        private GroupFooterSection groupFooterSection1;
        private GroupHeaderSection groupHeaderSection1;
        private Telerik.Reporting.TextBox textBoxLogo;
        private Telerik.Reporting.TextBox textBoxDataRange;
        private Telerik.Reporting.TextBox textBoxReportEmployee;
        private Shape shape3;
        private Reporting.Graph graph1;
        private CartesianCoordinateSystem cartesianCoordinateSystem1;
        private GraphAxis graphAxis2;
        private GraphAxis graphAxis1;
        private SqlDataSource mainDataSource;
        private AreaSeries lineSeries1;
        private Reporting.Graph graph2;
        private CartesianCoordinateSystem cartesianCoordinateSystem3;
        private GraphAxis graphAxis6;
        private GraphAxis graphAxis5;
        private BarSeries barSeries1;
        private SqlDataSource Employees;
        private Crosstab crosstabEmployeeSales;
        private Reporting.TextBox textBox6;
        private Reporting.TextBox textBox5;
        private Reporting.TextBox textBox9;
        private Reporting.TextBox textBox13;
        private Reporting.TextBox textBoxTableTitle;
        private Reporting.TextBox textBoxSalesOrderNumber;
        private Reporting.TextBox textBoxCategory;
        private Reporting.TextBox textBoxCategoryAmount;
        private Reporting.TextBox textBox12;
        private Reporting.TextBox textBox4;
        private Reporting.TextBox textBoxOrderTotal;
        private Reporting.TextBox textBox10;
        private Reporting.TextBox textBoxTotalLabel;
        private Reporting.TextBox textBoxEmployeeSalesTotal;
    }
}