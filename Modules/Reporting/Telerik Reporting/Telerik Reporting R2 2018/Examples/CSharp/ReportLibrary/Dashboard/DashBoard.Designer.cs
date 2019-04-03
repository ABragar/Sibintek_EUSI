namespace Telerik.Reporting.Examples.CSharp
{
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    partial class Dashboard
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.Reporting.TableGroup tableGroup1 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup2 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup3 = new Telerik.Reporting.TableGroup();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Dashboard));
            Telerik.Reporting.GraphGroup graphGroup1 = new Telerik.Reporting.GraphGroup();
            Telerik.Reporting.Drawing.ColorPalette colorPalette1 = new Telerik.Reporting.Drawing.ColorPalette();
            Telerik.Reporting.NumericalScale numericalScale1 = new Telerik.Reporting.NumericalScale();
            Telerik.Reporting.CategoryScale categoryScale1 = new Telerik.Reporting.CategoryScale();
            Telerik.Reporting.GraphGroup graphGroup2 = new Telerik.Reporting.GraphGroup();
            Telerik.Reporting.GraphGroup graphGroup3 = new Telerik.Reporting.GraphGroup();
            Telerik.Reporting.Drawing.ColorPalette colorPalette2 = new Telerik.Reporting.Drawing.ColorPalette();
            Telerik.Reporting.NumericalScale numericalScale2 = new Telerik.Reporting.NumericalScale();
            Telerik.Reporting.CategoryScale categoryScale2 = new Telerik.Reporting.CategoryScale();
            Telerik.Reporting.GraphGroup graphGroup4 = new Telerik.Reporting.GraphGroup();
            Telerik.Reporting.TableGroup tableGroup4 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup5 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup6 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.GraphGroup graphGroup5 = new Telerik.Reporting.GraphGroup();
            Telerik.Reporting.Drawing.ColorPalette colorPalette3 = new Telerik.Reporting.Drawing.ColorPalette();
            Telerik.Reporting.NumericalScale numericalScale3 = new Telerik.Reporting.NumericalScale();
            Telerik.Reporting.CategoryScale categoryScale3 = new Telerik.Reporting.CategoryScale();
            Telerik.Reporting.GraphGroup graphGroup6 = new Telerik.Reporting.GraphGroup();
            Telerik.Reporting.GraphGroup graphGroup7 = new Telerik.Reporting.GraphGroup();
            Telerik.Reporting.Drawing.ColorPalette colorPalette4 = new Telerik.Reporting.Drawing.ColorPalette();
            Telerik.Reporting.NumericalScale numericalScale4 = new Telerik.Reporting.NumericalScale();
            Telerik.Reporting.CategoryScale categoryScale4 = new Telerik.Reporting.CategoryScale();
            Telerik.Reporting.GraphGroup graphGroup8 = new Telerik.Reporting.GraphGroup();
            Telerik.Reporting.TableGroup tableGroup7 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup8 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup9 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.GraphGroup graphGroup9 = new Telerik.Reporting.GraphGroup();
            Telerik.Reporting.Drawing.ColorPalette colorPalette5 = new Telerik.Reporting.Drawing.ColorPalette();
            Telerik.Reporting.NumericalScale numericalScale5 = new Telerik.Reporting.NumericalScale();
            Telerik.Reporting.CategoryScale categoryScale5 = new Telerik.Reporting.CategoryScale();
            Telerik.Reporting.GraphGroup graphGroup10 = new Telerik.Reporting.GraphGroup();
            Telerik.Reporting.GraphGroup graphGroup11 = new Telerik.Reporting.GraphGroup();
            Telerik.Reporting.Drawing.ColorPalette colorPalette6 = new Telerik.Reporting.Drawing.ColorPalette();
            Telerik.Reporting.NumericalScale numericalScale6 = new Telerik.Reporting.NumericalScale();
            Telerik.Reporting.CategoryScale categoryScale6 = new Telerik.Reporting.CategoryScale();
            Telerik.Reporting.GraphGroup graphGroup12 = new Telerik.Reporting.GraphGroup();
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule4 = new Telerik.Reporting.Drawing.StyleRule();
            this.spLabelTotalQ = new Telerik.Reporting.TextBox();
            this.spLabelTotal = new Telerik.Reporting.TextBox();
            this.spLabelSalesPerson = new Telerik.Reporting.TextBox();
            this.sLabelTotalQ = new Telerik.Reporting.TextBox();
            this.sLabelTotal = new Telerik.Reporting.TextBox();
            this.sLabelStore = new Telerik.Reporting.TextBox();
            this.pLabelTotalQ = new Telerik.Reporting.TextBox();
            this.pLabelTotal = new Telerik.Reporting.TextBox();
            this.pProductName = new Telerik.Reporting.TextBox();
            this.yearDataSource = new Telerik.Reporting.SqlDataSource();
            this.detail = new Telerik.Reporting.DetailSection();
            this.panel2 = new Telerik.Reporting.Panel();
            this.panel3 = new Telerik.Reporting.Panel();
            this.SalesPersonQuarter = new Telerik.Reporting.Crosstab();
            this.spTotalQ = new Telerik.Reporting.TextBox();
            this.spTotal = new Telerik.Reporting.TextBox();
            this.spTextCorner = new Telerik.Reporting.TextBox();
            this.mainDataSource = new Telerik.Reporting.SqlDataSource();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.textBox11 = new Telerik.Reporting.TextBox();
            this.textBox12 = new Telerik.Reporting.TextBox();
            this.textBox3 = new Telerik.Reporting.TextBox();
            this.SalesPersonQuarterBar = new Telerik.Reporting.Graph();
            this.cartesianCoordinateSystem1 = new Telerik.Reporting.CartesianCoordinateSystem();
            this.graphAxis3 = new Telerik.Reporting.GraphAxis();
            this.graphAxis4 = new Telerik.Reporting.GraphAxis();
            this.barSeries1 = new Telerik.Reporting.BarSeries();
            this.SalesPersonQuarterPie = new Telerik.Reporting.Graph();
            this.polarCoordinateSystem1 = new Telerik.Reporting.PolarCoordinateSystem();
            this.graphAxis1 = new Telerik.Reporting.GraphAxis();
            this.graphAxis2 = new Telerik.Reporting.GraphAxis();
            this.barSeries2 = new Telerik.Reporting.BarSeries();
            this.panel1 = new Telerik.Reporting.Panel();
            this.panel4 = new Telerik.Reporting.Panel();
            this.StoreQuarter = new Telerik.Reporting.Crosstab();
            this.sTotalQ = new Telerik.Reporting.TextBox();
            this.sTotal = new Telerik.Reporting.TextBox();
            this.sTextCorner = new Telerik.Reporting.TextBox();
            this.textBox9 = new Telerik.Reporting.TextBox();
            this.textBox13 = new Telerik.Reporting.TextBox();
            this.textBox14 = new Telerik.Reporting.TextBox();
            this.textBox7 = new Telerik.Reporting.TextBox();
            this.StoreQuarterBar = new Telerik.Reporting.Graph();
            this.cartesianCoordinateSystem2 = new Telerik.Reporting.CartesianCoordinateSystem();
            this.graphAxis5 = new Telerik.Reporting.GraphAxis();
            this.graphAxis6 = new Telerik.Reporting.GraphAxis();
            this.barSeries3 = new Telerik.Reporting.BarSeries();
            this.StoreQuarterPie = new Telerik.Reporting.Graph();
            this.polarCoordinateSystem2 = new Telerik.Reporting.PolarCoordinateSystem();
            this.graphAxis7 = new Telerik.Reporting.GraphAxis();
            this.graphAxis8 = new Telerik.Reporting.GraphAxis();
            this.barSeries4 = new Telerik.Reporting.BarSeries();
            this.panelSalesByProduct = new Telerik.Reporting.Panel();
            this.panel6 = new Telerik.Reporting.Panel();
            this.ProductQuarter = new Telerik.Reporting.Crosstab();
            this.pTotalQ = new Telerik.Reporting.TextBox();
            this.pTotal = new Telerik.Reporting.TextBox();
            this.pTextCorner = new Telerik.Reporting.TextBox();
            this.textBox10 = new Telerik.Reporting.TextBox();
            this.textBox15 = new Telerik.Reporting.TextBox();
            this.textBox16 = new Telerik.Reporting.TextBox();
            this.textBox8 = new Telerik.Reporting.TextBox();
            this.ProductQuarterPie = new Telerik.Reporting.Graph();
            this.polarCoordinateSystem3 = new Telerik.Reporting.PolarCoordinateSystem();
            this.graphAxis9 = new Telerik.Reporting.GraphAxis();
            this.graphAxis10 = new Telerik.Reporting.GraphAxis();
            this.barSeries5 = new Telerik.Reporting.BarSeries();
            this.ProductQuarterBar = new Telerik.Reporting.Graph();
            this.cartesianCoordinateSystem3 = new Telerik.Reporting.CartesianCoordinateSystem();
            this.graphAxis11 = new Telerik.Reporting.GraphAxis();
            this.graphAxis12 = new Telerik.Reporting.GraphAxis();
            this.barSeries6 = new Telerik.Reporting.BarSeries();
            this.textBox5 = new Telerik.Reporting.TextBox();
            this.reportHeaderSection1 = new Telerik.Reporting.ReportHeaderSection();
            this.panel5 = new Telerik.Reporting.Panel();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.shape5 = new Telerik.Reporting.Shape();
            this.textBox6 = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize) (this)).BeginInit();
            // 
            // spLabelTotalQ
            // 
            this.spLabelTotalQ.Name = "spLabelTotalQ";
            this.spLabelTotalQ.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.38188979029655457D), Telerik.Reporting.Drawing.Unit.Inch(0.24000000953674316D));
            this.spLabelTotalQ.Style.Color = System.Drawing.Color.FromArgb(((int) (((byte) (105)))), ((int) (((byte) (40)))), ((int) (((byte) (40)))));
            this.spLabelTotalQ.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.spLabelTotalQ.Value = "=\'Q\'+ Quarter(Fields.OrderDate)";
            // 
            // spLabelTotal
            // 
            this.spLabelTotal.Name = "spLabelTotal";
            this.spLabelTotal.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.38188979029655457D), Telerik.Reporting.Drawing.Unit.Inch(0.24000000953674316D));
            this.spLabelTotal.Style.Color = System.Drawing.Color.FromArgb(((int) (((byte) (105)))), ((int) (((byte) (40)))), ((int) (((byte) (40)))));
            this.spLabelTotal.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.spLabelTotal.Value = "Total";
            // 
            // spLabelSalesPerson
            // 
            this.spLabelSalesPerson.Name = "spLabelSalesPerson";
            this.spLabelSalesPerson.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.5D), Telerik.Reporting.Drawing.Unit.Inch(0.24015748500823975D));
            this.spLabelSalesPerson.Value = "=Fields.SalesPersonFullName";
            // 
            // sLabelTotalQ
            // 
            this.sLabelTotalQ.Name = "sLabelTotalQ";
            this.sLabelTotalQ.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.38188979029655457D), Telerik.Reporting.Drawing.Unit.Inch(0.24000000953674316D));
            this.sLabelTotalQ.Style.Color = System.Drawing.Color.FromArgb(((int) (((byte) (0)))), ((int) (((byte) (105)))), ((int) (((byte) (104)))));
            this.sLabelTotalQ.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.sLabelTotalQ.Value = "=\'Q\'+Quarter(Fields.OrderDate)";
            // 
            // sLabelTotal
            // 
            this.sLabelTotal.Name = "sLabelTotal";
            this.sLabelTotal.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.38188979029655457D), Telerik.Reporting.Drawing.Unit.Inch(0.24000000953674316D));
            this.sLabelTotal.Style.Color = System.Drawing.Color.FromArgb(((int) (((byte) (0)))), ((int) (((byte) (105)))), ((int) (((byte) (104)))));
            this.sLabelTotal.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.sLabelTotal.Value = "Total";
            // 
            // sLabelStore
            // 
            this.sLabelStore.Name = "sLabelStore";
            this.sLabelStore.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.5D), Telerik.Reporting.Drawing.Unit.Inch(0.24015748500823975D));
            this.sLabelStore.Value = "=Fields.StoreName";
            // 
            // pLabelTotalQ
            // 
            this.pLabelTotalQ.Name = "pLabelTotalQ";
            this.pLabelTotalQ.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.38188979029655457D), Telerik.Reporting.Drawing.Unit.Inch(0.24000000953674316D));
            this.pLabelTotalQ.Style.Color = System.Drawing.Color.FromArgb(((int) (((byte) (236)))), ((int) (((byte) (57)))), ((int) (((byte) (27)))));
            this.pLabelTotalQ.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.pLabelTotalQ.Value = "=\'Q\'+Quarter(Fields.OrderDate)";
            // 
            // pLabelTotal
            // 
            this.pLabelTotal.Name = "pLabelTotal";
            this.pLabelTotal.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.38188979029655457D), Telerik.Reporting.Drawing.Unit.Inch(0.24000000953674316D));
            this.pLabelTotal.Style.Color = System.Drawing.Color.FromArgb(((int) (((byte) (236)))), ((int) (((byte) (57)))), ((int) (((byte) (27)))));
            this.pLabelTotal.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.pLabelTotal.Value = "Total";
            // 
            // pProductName
            // 
            this.pProductName.Name = "pProductName";
            this.pProductName.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.5D), Telerik.Reporting.Drawing.Unit.Inch(0.24015748500823975D));
            this.pProductName.Value = "=Fields.ProductName";
            // 
            // yearDataSource
            // 
            this.yearDataSource.ConnectionString = "Telerik.Reporting.Examples.CSharp.Properties.Settings.TelerikConnectionString";
            this.yearDataSource.Name = "yearDataSource";
            this.yearDataSource.SelectCommand = "SELECT DISTINCT YEAR(OrderDate) AS Year\r\nFROM         Sales.SalesOrderHeader\r\nORD" +
    "ER BY Year";
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Inch(7.5407319068908691D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.panel2,
            this.panel1,
            this.panelSalesByProduct});
            this.detail.Name = "detail";
            this.detail.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(5D);
            // 
            // panel2
            // 
            this.panel2.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.panel3,
            this.textBox11,
            this.textBox12,
            this.textBox3,
            this.SalesPersonQuarterBar,
            this.SalesPersonQuarterPie});
            this.panel2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(7.8837074397597462E-05D));
            this.panel2.Name = "panel2";
            this.panel2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(10.40000057220459D), Telerik.Reporting.Drawing.Unit.Inch(2.5D));
            // 
            // panel3
            // 
            this.panel3.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.SalesPersonQuarter,
            this.textBox2});
            this.panel3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.47999998927116394D));
            this.panel3.Name = "panel3";
            this.panel3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.5999999046325684D), Telerik.Reporting.Drawing.Unit.Inch(2.0199999809265137D));
            // 
            // SalesPersonQuarter
            // 
            this.SalesPersonQuarter.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.38188984990119934D)));
            this.SalesPersonQuarter.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.38188984990119934D)));
            this.SalesPersonQuarter.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Inch(0.24015745520591736D)));
            this.SalesPersonQuarter.Body.SetCellContent(0, 0, this.spTotalQ);
            this.SalesPersonQuarter.Body.SetCellContent(0, 1, this.spTotal);
            tableGroup1.Groupings.Add(new Telerik.Reporting.Grouping("=Quarter(Fields.OrderDate)"));
            tableGroup1.Name = "Quater";
            tableGroup1.ReportItem = this.spLabelTotalQ;
            tableGroup1.Sortings.Add(new Telerik.Reporting.Sorting("=Quarter(Fields.OrderDate)", Telerik.Reporting.SortDirection.Asc));
            tableGroup2.ReportItem = this.spLabelTotal;
            this.SalesPersonQuarter.ColumnGroups.Add(tableGroup1);
            this.SalesPersonQuarter.ColumnGroups.Add(tableGroup2);
            this.SalesPersonQuarter.Corner.SetCellContent(0, 0, this.spTextCorner);
            this.SalesPersonQuarter.DataSource = this.mainDataSource;
            this.SalesPersonQuarter.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.spTextCorner,
            this.spTotalQ,
            this.spTotal,
            this.spLabelTotalQ,
            this.spLabelTotal,
            this.spLabelSalesPerson});
            this.SalesPersonQuarter.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.31999999284744263D));
            this.SalesPersonQuarter.Name = "SalesPersonQuarter";
            tableGroup3.Filters.Add(new Telerik.Reporting.Filter("=Sum(Fields.LineTotal)", Telerik.Reporting.FilterOperator.TopN, "=5"));
            tableGroup3.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.SalesPersonFullName"));
            tableGroup3.Name = "SalesPersonFullName";
            tableGroup3.ReportItem = this.spLabelSalesPerson;
            tableGroup3.Sortings.Add(new Telerik.Reporting.Sorting("=Sum(Fields.LineTotal)", Telerik.Reporting.SortDirection.Desc));
            this.SalesPersonQuarter.RowGroups.Add(tableGroup3);
            this.SalesPersonQuarter.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.2637796401977539D), Telerik.Reporting.Drawing.Unit.Inch(0.48015749454498291D));
            this.SalesPersonQuarter.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.SalesPersonQuarter.Style.Font.Name = "Segoe UI";
            this.SalesPersonQuarter.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.SalesPersonQuarter.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            // 
            // spTotalQ
            // 
            this.spTotalQ.Format = "{0:#.}";
            this.spTotalQ.Name = "spTotalQ";
            this.spTotalQ.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.38188979029655457D), Telerik.Reporting.Drawing.Unit.Inch(0.24015748500823975D));
            this.spTotalQ.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.spTotalQ.Value = "=Sum(Fields.LineTotal)";
            // 
            // spTotal
            // 
            this.spTotal.Format = "{0:#.}";
            this.spTotal.Name = "spTotal";
            this.spTotal.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.38188979029655457D), Telerik.Reporting.Drawing.Unit.Inch(0.24015748500823975D));
            this.spTotal.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.spTotal.Value = "=Sum(Fields.LineTotal)";
            // 
            // spTextCorner
            // 
            this.spTextCorner.Name = "spTextCorner";
            this.spTextCorner.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.5D), Telerik.Reporting.Drawing.Unit.Inch(0.23999999463558197D));
            this.spTextCorner.Style.Color = System.Drawing.Color.FromArgb(((int) (((byte) (105)))), ((int) (((byte) (40)))), ((int) (((byte) (40)))));
            this.spTextCorner.Value = "Sales Person";
            // 
            // mainDataSource
            // 
            this.mainDataSource.ConnectionString = "Telerik.Reporting.Examples.CSharp.Properties.Settings.TelerikConnectionString";
            this.mainDataSource.Name = "mainDataSource";
            this.mainDataSource.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@Year", System.Data.DbType.Int32, "=Parameters.ReportYear.Value")});
            this.mainDataSource.SelectCommand = resources.GetString("mainDataSource.SelectCommand");
            // 
            // textBox2
            // 
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.5999999046325684D), Telerik.Reporting.Drawing.Unit.Inch(0.31999999284744263D));
            this.textBox2.StyleName = "legend";
            this.textBox2.Value = "SALES AMOUNT IN USD (THOUSANDS)";
            // 
            // textBox11
            // 
            this.textBox11.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.5999999046325684D), Telerik.Reporting.Drawing.Unit.Inch(0.47999998927116394D));
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.0799999237060547D), Telerik.Reporting.Drawing.Unit.Inch(0.31999999284744263D));
            this.textBox11.StyleName = "legend";
            this.textBox11.Value = "YEARLY SALES DISTRIBUTION";
            // 
            // textBox12
            // 
            this.textBox12.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.679999828338623D), Telerik.Reporting.Drawing.Unit.Inch(0.47999998927116394D));
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.7100000381469727D), Telerik.Reporting.Drawing.Unit.Inch(0.31999999284744263D));
            this.textBox12.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox12.StyleName = "legend";
            this.textBox12.Value = "QUARTERLY SALES DISTRIBUTION";
            // 
            // textBox3
            // 
            this.textBox3.Anchoring = ((Telerik.Reporting.AnchoringStyles) ((Telerik.Reporting.AnchoringStyles.Left | Telerik.Reporting.AnchoringStyles.Right)));
            this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(10.40000057220459D), Telerik.Reporting.Drawing.Unit.Inch(0.36000001430511475D));
            this.textBox3.Style.BorderColor.Bottom = System.Drawing.Color.FromArgb(((int) (((byte) (202)))), ((int) (((byte) (137)))), ((int) (((byte) (104)))));
            this.textBox3.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox3.Style.Color = System.Drawing.Color.FromArgb(((int) (((byte) (236)))), ((int) (((byte) (57)))), ((int) (((byte) (27)))));
            this.textBox3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(16D);
            this.textBox3.TocText = "Top 5 Performing Agents";
            this.textBox3.Value = "Top 5 performing agents";
            // 
            // SalesPersonQuarterBar
            // 
            graphGroup1.Filters.Add(new Telerik.Reporting.Filter("=Sum(Fields.LineTotal)", Telerik.Reporting.FilterOperator.TopN, "=5"));
            graphGroup1.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.SalesPersonFullName"));
            graphGroup1.Label = "= Fields.SalesPersonFullName";
            graphGroup1.Name = "SalesPersonGroup";
            graphGroup1.Sortings.Add(new Telerik.Reporting.Sorting("=Sum(Fields.LineTotal)", Telerik.Reporting.SortDirection.Asc));
            this.SalesPersonQuarterBar.CategoryGroups.Add(graphGroup1);
            colorPalette1.Colors.Add(System.Drawing.Color.FromArgb(((int) (((byte) (105)))), ((int) (((byte) (40)))), ((int) (((byte) (40))))));
            colorPalette1.Colors.Add(System.Drawing.Color.FromArgb(((int) (((byte) (196)))), ((int) (((byte) (102)))), ((int) (((byte) (0))))));
            colorPalette1.Colors.Add(System.Drawing.Color.FromArgb(((int) (((byte) (255)))), ((int) (((byte) (147)))), ((int) (((byte) (30))))));
            colorPalette1.Colors.Add(System.Drawing.Color.FromArgb(((int) (((byte) (177)))), ((int) (((byte) (161)))), ((int) (((byte) (82))))));
            colorPalette1.Colors.Add(System.Drawing.Color.FromArgb(((int) (((byte) (211)))), ((int) (((byte) (191)))), ((int) (((byte) (105))))));
            this.SalesPersonQuarterBar.ColorPalette = colorPalette1;
            this.SalesPersonQuarterBar.CoordinateSystems.Add(this.cartesianCoordinateSystem1);
            this.SalesPersonQuarterBar.DataSource = this.mainDataSource;
            this.SalesPersonQuarterBar.Legend.Position = Telerik.Reporting.GraphItemPosition.TopCenter;
            this.SalesPersonQuarterBar.Legend.Style.LineColor = System.Drawing.Color.LightGray;
            this.SalesPersonQuarterBar.Legend.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.SalesPersonQuarterBar.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(169.66999816894531D), Telerik.Reporting.Drawing.Unit.Mm(20.319999694824219D));
            this.SalesPersonQuarterBar.Name = "SalesPersonQuarterBar";
            this.SalesPersonQuarterBar.PlotAreaStyle.LineColor = System.Drawing.Color.LightGray;
            this.SalesPersonQuarterBar.PlotAreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.SalesPersonQuarterBar.Series.Add(this.barSeries1);
            this.SalesPersonQuarterBar.SeriesGroups.Add(graphGroup2);
            this.SalesPersonQuarterBar.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(94.489997863769531D), Telerik.Reporting.Drawing.Unit.Mm(43.180000305175781D));
            this.SalesPersonQuarterBar.Style.Font.Name = "Arial";
            this.SalesPersonQuarterBar.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.SalesPersonQuarterBar.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(5D);
            this.SalesPersonQuarterBar.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.SalesPersonQuarterBar.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.SalesPersonQuarterBar.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            // 
            // cartesianCoordinateSystem1
            // 
            this.cartesianCoordinateSystem1.Name = "cartesianCoordinateSystem1";
            this.cartesianCoordinateSystem1.XAxis = this.graphAxis3;
            this.cartesianCoordinateSystem1.YAxis = this.graphAxis4;
            // 
            // graphAxis3
            // 
            this.graphAxis3.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis3.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis3.MajorGridLineStyle.Visible = false;
            this.graphAxis3.MajorTickMarkDisplayType = Telerik.Reporting.GraphAxisTickMarkDisplayType.Outside;
            this.graphAxis3.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis3.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis3.MinorGridLineStyle.Visible = false;
            this.graphAxis3.Name = "graphAxis3";
            this.graphAxis3.Scale = numericalScale1;
            this.graphAxis3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
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
            this.graphAxis4.Size = Telerik.Reporting.Drawing.Unit.Inch(1.2999999523162842D);
            this.graphAxis4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7D);
            this.graphAxis4.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.graphAxis4.Style.Visible = true;
            // 
            // barSeries1
            // 
            this.barSeries1.ArrangeMode = Telerik.Reporting.GraphSeriesArrangeMode.Stacked;
            this.barSeries1.CategoryGroup = graphGroup1;
            this.barSeries1.CoordinateSystem = this.cartesianCoordinateSystem1;
            this.barSeries1.DataPointLabel = "=Sum(Fields.LineTotal)";
            this.barSeries1.DataPointLabelStyle.Visible = false;
            this.barSeries1.DataPointStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.barSeries1.DataPointStyle.Visible = true;
            this.barSeries1.LegendItem.Value = "=\'Q\' + Quarter(Fields.OrderDate)";
            graphGroup2.Groupings.Add(new Telerik.Reporting.Grouping("= Quarter(Fields.OrderDate)"));
            graphGroup2.Label = "=\'Q\' + Quarter(Fields.OrderDate)";
            graphGroup2.Name = "OrderDate.QuarterGroup";
            graphGroup2.Sortings.Add(new Telerik.Reporting.Sorting("= Quarter(Fields.OrderDate)", Telerik.Reporting.SortDirection.Asc));
            this.barSeries1.SeriesGroup = graphGroup2;
            this.barSeries1.X = "=Sum(Fields.LineTotal)";
            this.barSeries1.ToolTip.Title = "=Format('Q{0} results for {1}', Quarter(Fields.OrderDate), Fields.SalesPersonFullName)";
            this.barSeries1.ToolTip.Text = "=Format('${0:N2} K', Sum(Fields.LineTotal))";
            // 
            // SalesPersonQuarterPie
            // 
            graphGroup3.Name = "categoryGroup";
            this.SalesPersonQuarterPie.CategoryGroups.Add(graphGroup3);
            colorPalette2.Colors.Add(System.Drawing.Color.FromArgb(((int) (((byte) (105)))), ((int) (((byte) (40)))), ((int) (((byte) (40))))));
            colorPalette2.Colors.Add(System.Drawing.Color.FromArgb(((int) (((byte) (196)))), ((int) (((byte) (102)))), ((int) (((byte) (0))))));
            colorPalette2.Colors.Add(System.Drawing.Color.FromArgb(((int) (((byte) (255)))), ((int) (((byte) (147)))), ((int) (((byte) (30))))));
            colorPalette2.Colors.Add(System.Drawing.Color.FromArgb(((int) (((byte) (177)))), ((int) (((byte) (161)))), ((int) (((byte) (82))))));
            colorPalette2.Colors.Add(System.Drawing.Color.FromArgb(((int) (((byte) (211)))), ((int) (((byte) (191)))), ((int) (((byte) (105))))));
            this.SalesPersonQuarterPie.ColorPalette = colorPalette2;
            this.SalesPersonQuarterPie.CoordinateSystems.Add(this.polarCoordinateSystem1);
            this.SalesPersonQuarterPie.Culture = new System.Globalization.CultureInfo("");
            this.SalesPersonQuarterPie.DataSource = this.mainDataSource;
            this.SalesPersonQuarterPie.Legend.Height = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.SalesPersonQuarterPie.Legend.Position = Telerik.Reporting.GraphItemPosition.LeftCenter;
            this.SalesPersonQuarterPie.Legend.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7D);
            this.SalesPersonQuarterPie.Legend.Style.LineColor = System.Drawing.Color.LightGray;
            this.SalesPersonQuarterPie.Legend.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.SalesPersonQuarterPie.Legend.Width = Telerik.Reporting.Drawing.Unit.Pixel(100D);
            this.SalesPersonQuarterPie.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(91.44000244140625D), Telerik.Reporting.Drawing.Unit.Mm(20.319999694824219D));
            this.SalesPersonQuarterPie.Name = "SalesPersonQuarterPie";
            this.SalesPersonQuarterPie.PlotAreaStyle.LineColor = System.Drawing.Color.LightGray;
            this.SalesPersonQuarterPie.PlotAreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.SalesPersonQuarterPie.Series.Add(this.barSeries2);
            this.SalesPersonQuarterPie.SeriesGroups.Add(graphGroup4);
            this.SalesPersonQuarterPie.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(78.2300033569336D), Telerik.Reporting.Drawing.Unit.Mm(43.180000305175781D));
            this.SalesPersonQuarterPie.Style.Font.Name = "Arial";
            this.SalesPersonQuarterPie.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(5D);
            this.SalesPersonQuarterPie.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(0D);
            this.SalesPersonQuarterPie.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.SalesPersonQuarterPie.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(5D);
            // 
            // polarCoordinateSystem1
            // 
            this.polarCoordinateSystem1.AngularAxis = this.graphAxis1;
            this.polarCoordinateSystem1.Name = "polarCoordinateSystem1";
            this.polarCoordinateSystem1.RadialAxis = this.graphAxis2;
            // 
            // graphAxis1
            // 
            this.graphAxis1.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis1.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis1.MajorGridLineStyle.Visible = false;
            this.graphAxis1.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis1.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis1.MinorGridLineStyle.Visible = false;
            this.graphAxis1.Name = "graphAxis1";
            this.graphAxis1.Scale = numericalScale2;
            this.graphAxis1.Style.Visible = false;
            // 
            // graphAxis2
            // 
            this.graphAxis2.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis2.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis2.MajorGridLineStyle.Visible = false;
            this.graphAxis2.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis2.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis2.MinorGridLineStyle.Visible = false;
            this.graphAxis2.Name = "graphAxis2";
            categoryScale2.PositionMode = Telerik.Reporting.AxisPositionMode.OnTicks;
            categoryScale2.SpacingSlotCount = 0D;
            this.graphAxis2.Scale = categoryScale2;
            this.graphAxis2.Style.Visible = false;
            // 
            // barSeries2
            // 
            this.barSeries2.ArrangeMode = Telerik.Reporting.GraphSeriesArrangeMode.Stacked100;
            this.barSeries2.CategoryGroup = graphGroup3;
            this.barSeries2.CoordinateSystem = this.polarCoordinateSystem1;
            this.barSeries2.DataPointLabel = "=Sum(Fields.LineTotal)";
            this.barSeries2.DataPointLabelStyle.Visible = false;
            this.barSeries2.DataPointStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.barSeries2.DataPointStyle.Visible = true;
            this.barSeries2.LegendItem.Value = "= Fields.SalesPersonFullName";
            graphGroup4.Filters.Add(new Telerik.Reporting.Filter("=Sum(Fields.LineTotal)", Telerik.Reporting.FilterOperator.TopN, "=5"));
            graphGroup4.Groupings.Add(new Telerik.Reporting.Grouping("= Fields.SalesPersonFullName"));
            graphGroup4.Name = "SalesPersonGroup";
            graphGroup4.Sortings.Add(new Telerik.Reporting.Sorting("= Sum(Fields.LineTotal)", Telerik.Reporting.SortDirection.Desc));
            this.barSeries2.SeriesGroup = graphGroup4;
            this.barSeries2.X = "=Sum(Fields.LineTotal)";
            this.barSeries2.ToolTip.Title = "= Fields.SalesPersonFullName";
            this.barSeries2.ToolTip.Text = "=Format('Total: ${0:N2} K', Sum(Fields.LineTotal))";
            // 
            // panel1
            // 
            this.panel1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.panel4,
            this.textBox13,
            this.textBox14,
            this.textBox7,
            this.StoreQuarterBar,
            this.StoreQuarterPie});
            this.panel1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(2.5001578330993652D));
            this.panel1.Name = "panel1";
            this.panel1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(10.399999618530273D), Telerik.Reporting.Drawing.Unit.Inch(2.5199999809265137D));
            // 
            // panel4
            // 
            this.panel4.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.StoreQuarter,
            this.textBox9});
            this.panel4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.5D));
            this.panel4.Name = "panel4";
            this.panel4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.5999999046325684D), Telerik.Reporting.Drawing.Unit.Inch(2.0199999809265137D));
            // 
            // StoreQuarter
            // 
            this.StoreQuarter.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.38188984990119934D)));
            this.StoreQuarter.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.38188984990119934D)));
            this.StoreQuarter.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Inch(0.24015745520591736D)));
            this.StoreQuarter.Body.SetCellContent(0, 0, this.sTotalQ);
            this.StoreQuarter.Body.SetCellContent(0, 1, this.sTotal);
            tableGroup4.Groupings.Add(new Telerik.Reporting.Grouping("=Quarter(Fields.OrderDate)"));
            tableGroup4.Name = "Quater";
            tableGroup4.ReportItem = this.sLabelTotalQ;
            tableGroup4.Sortings.Add(new Telerik.Reporting.Sorting("=Quarter(Fields.OrderDate)", Telerik.Reporting.SortDirection.Asc));
            tableGroup5.ReportItem = this.sLabelTotal;
            this.StoreQuarter.ColumnGroups.Add(tableGroup4);
            this.StoreQuarter.ColumnGroups.Add(tableGroup5);
            this.StoreQuarter.Corner.SetCellContent(0, 0, this.sTextCorner);
            this.StoreQuarter.DataSource = this.mainDataSource;
            this.StoreQuarter.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.sTextCorner,
            this.sTotalQ,
            this.sTotal,
            this.sLabelTotalQ,
            this.sLabelTotal,
            this.sLabelStore});
            this.StoreQuarter.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.31999999284744263D));
            this.StoreQuarter.Name = "StoreQuarter";
            tableGroup6.Filters.Add(new Telerik.Reporting.Filter("=Sum(Fields.LineTotal)", Telerik.Reporting.FilterOperator.TopN, "=5"));
            tableGroup6.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.StoreName"));
            tableGroup6.Name = "StoreName";
            tableGroup6.ReportItem = this.sLabelStore;
            tableGroup6.Sortings.Add(new Telerik.Reporting.Sorting("=Sum(Fields.LineTotal)", Telerik.Reporting.SortDirection.Desc));
            this.StoreQuarter.RowGroups.Add(tableGroup6);
            this.StoreQuarter.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.2637796401977539D), Telerik.Reporting.Drawing.Unit.Inch(0.48015749454498291D));
            this.StoreQuarter.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            // 
            // sTotalQ
            // 
            this.sTotalQ.Format = "{0:#.}";
            this.sTotalQ.Name = "sTotalQ";
            this.sTotalQ.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.38188979029655457D), Telerik.Reporting.Drawing.Unit.Inch(0.24015748500823975D));
            this.sTotalQ.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.sTotalQ.Value = "=Sum(Fields.LineTotal)";
            // 
            // sTotal
            // 
            this.sTotal.Format = "{0:#.}";
            this.sTotal.Name = "sTotal";
            this.sTotal.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.38188979029655457D), Telerik.Reporting.Drawing.Unit.Inch(0.24015748500823975D));
            this.sTotal.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.sTotal.Value = "=Sum(Fields.LineTotal)";
            // 
            // sTextCorner
            // 
            this.sTextCorner.Name = "sTextCorner";
            this.sTextCorner.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.5D), Telerik.Reporting.Drawing.Unit.Inch(0.23999999463558197D));
            this.sTextCorner.Style.Color = System.Drawing.Color.FromArgb(((int) (((byte) (0)))), ((int) (((byte) (105)))), ((int) (((byte) (104)))));
            this.sTextCorner.Value = "Store";
            // 
            // textBox9
            // 
            this.textBox9.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.7000000476837158D), Telerik.Reporting.Drawing.Unit.Inch(0.31999999284744263D));
            this.textBox9.StyleName = "legend";
            this.textBox9.Value = "SALES AMOUNT IN USD (THOUSANDS)";
            // 
            // textBox13
            // 
            this.textBox13.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.5999999046325684D), Telerik.Reporting.Drawing.Unit.Inch(0.5D));
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.0799999237060547D), Telerik.Reporting.Drawing.Unit.Inch(0.31999999284744263D));
            this.textBox13.StyleName = "legend";
            this.textBox13.Value = "YEARLY SALES DISTRIBUTION";
            // 
            // textBox14
            // 
            this.textBox14.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.679999828338623D), Telerik.Reporting.Drawing.Unit.Inch(0.5D));
            this.textBox14.Name = "textBox14";
            this.textBox14.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.7200000286102295D), Telerik.Reporting.Drawing.Unit.Inch(0.31999999284744263D));
            this.textBox14.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox14.StyleName = "legend";
            this.textBox14.Value = "QUARTERLY SALES DISTRIBUTION";
            // 
            // textBox7
            // 
            this.textBox7.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(10.399999618530273D), Telerik.Reporting.Drawing.Unit.Inch(0.36000001430511475D));
            this.textBox7.Style.BorderColor.Bottom = System.Drawing.Color.FromArgb(((int) (((byte) (202)))), ((int) (((byte) (137)))), ((int) (((byte) (104)))));
            this.textBox7.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox7.Style.Color = System.Drawing.Color.FromArgb(((int) (((byte) (236)))), ((int) (((byte) (57)))), ((int) (((byte) (27)))));
            this.textBox7.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(16D);
            this.textBox7.TocText = "Top 5 Performing Stores";
            this.textBox7.Value = "Top 5 performing stores";
            // 
            // StoreQuarterBar
            // 
            graphGroup5.Filters.Add(new Telerik.Reporting.Filter("=Sum(Fields.LineTotal)", Telerik.Reporting.FilterOperator.TopN, "=5"));
            graphGroup5.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.StoreName"));
            graphGroup5.Name = "ProductNameGroup";
            graphGroup5.Sortings.Add(new Telerik.Reporting.Sorting("=Sum(Fields.LineTotal)", Telerik.Reporting.SortDirection.Asc));
            this.StoreQuarterBar.CategoryGroups.Add(graphGroup5);
            colorPalette3.Colors.Add(System.Drawing.Color.FromArgb(((int) (((byte) (0)))), ((int) (((byte) (105)))), ((int) (((byte) (104))))));
            colorPalette3.Colors.Add(System.Drawing.Color.FromArgb(((int) (((byte) (88)))), ((int) (((byte) (168)))), ((int) (((byte) (35))))));
            colorPalette3.Colors.Add(System.Drawing.Color.FromArgb(((int) (((byte) (137)))), ((int) (((byte) (203)))), ((int) (((byte) (42))))));
            colorPalette3.Colors.Add(System.Drawing.Color.FromArgb(((int) (((byte) (34)))), ((int) (((byte) (181)))), ((int) (((byte) (115))))));
            colorPalette3.Colors.Add(System.Drawing.Color.FromArgb(((int) (((byte) (32)))), ((int) (((byte) (176)))), ((int) (((byte) (212))))));
            this.StoreQuarterBar.ColorPalette = colorPalette3;
            this.StoreQuarterBar.CoordinateSystems.Add(this.cartesianCoordinateSystem2);
            this.StoreQuarterBar.DataSource = this.mainDataSource;
            this.StoreQuarterBar.Legend.Position = Telerik.Reporting.GraphItemPosition.TopCenter;
            this.StoreQuarterBar.Legend.Style.LineColor = System.Drawing.Color.LightGray;
            this.StoreQuarterBar.Legend.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.StoreQuarterBar.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(169.66999816894531D), Telerik.Reporting.Drawing.Unit.Mm(20.829999923706055D));
            this.StoreQuarterBar.Name = "StoreQuarterBar";
            this.StoreQuarterBar.PlotAreaStyle.LineColor = System.Drawing.Color.LightGray;
            this.StoreQuarterBar.PlotAreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.StoreQuarterBar.Series.Add(this.barSeries3);
            this.StoreQuarterBar.SeriesGroups.Add(graphGroup6);
            this.StoreQuarterBar.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(94.489997863769531D), Telerik.Reporting.Drawing.Unit.Mm(43.180000305175781D));
            this.StoreQuarterBar.Style.Font.Name = "Segoe UI";
            this.StoreQuarterBar.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.StoreQuarterBar.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(5D);
            this.StoreQuarterBar.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.StoreQuarterBar.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.StoreQuarterBar.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            // 
            // cartesianCoordinateSystem2
            // 
            this.cartesianCoordinateSystem2.Name = "cartesianCoordinateSystem1";
            this.cartesianCoordinateSystem2.XAxis = this.graphAxis5;
            this.cartesianCoordinateSystem2.YAxis = this.graphAxis6;
            // 
            // graphAxis5
            // 
            this.graphAxis5.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis5.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis5.MajorGridLineStyle.Visible = false;
            this.graphAxis5.MajorTickMarkDisplayType = Telerik.Reporting.GraphAxisTickMarkDisplayType.Outside;
            this.graphAxis5.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis5.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis5.MinorGridLineStyle.Visible = false;
            this.graphAxis5.Name = "graphAxis3";
            this.graphAxis5.Scale = numericalScale3;
            this.graphAxis5.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
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
            this.graphAxis6.Name = "graphAxis4";
            categoryScale3.SpacingSlotCount = 0.4D;
            this.graphAxis6.Scale = categoryScale3;
            this.graphAxis6.Size = Telerik.Reporting.Drawing.Unit.Inch(1.2999999523162842D);
            this.graphAxis6.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7D);
            this.graphAxis6.Style.Visible = true;
            // 
            // barSeries3
            // 
            this.barSeries3.ArrangeMode = Telerik.Reporting.GraphSeriesArrangeMode.Stacked;
            this.barSeries3.CategoryGroup = graphGroup5;
            this.barSeries3.CoordinateSystem = this.cartesianCoordinateSystem2;
            this.barSeries3.DataPointLabel = "=Sum(Fields.LineTotal)";
            this.barSeries3.DataPointLabelStyle.Visible = false;
            this.barSeries3.DataPointStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.barSeries3.DataPointStyle.Visible = true;
            this.barSeries3.LegendItem.Value = "=\'Q\' + Quarter(Fields.OrderDate)";
            graphGroup6.Groupings.Add(new Telerik.Reporting.Grouping("= Quarter(Fields.OrderDate)"));
            graphGroup6.Label = "=\'Q\' + Quarter(Fields.OrderDate)";
            graphGroup6.Name = "OrderDate.QuarterGroup";
            graphGroup6.Sortings.Add(new Telerik.Reporting.Sorting("= Quarter(Fields.OrderDate)", Telerik.Reporting.SortDirection.Asc));
            this.barSeries3.SeriesGroup = graphGroup6;
            this.barSeries3.X = "=Sum(Fields.LineTotal)";
            this.barSeries3.ToolTip.Title = "=Format('Q{0} results for {1}', Quarter(Fields.OrderDate), Fields.StoreName)";
            this.barSeries3.ToolTip.Text = "=Format('${0:N2} K', Sum(Fields.LineTotal))";
            // 
            // StoreQuarterPie
            // 
            graphGroup7.Name = "categoryGroup";
            this.StoreQuarterPie.CategoryGroups.Add(graphGroup7);
            colorPalette4.Colors.Add(System.Drawing.Color.FromArgb(((int) (((byte) (0)))), ((int) (((byte) (105)))), ((int) (((byte) (104))))));
            colorPalette4.Colors.Add(System.Drawing.Color.FromArgb(((int) (((byte) (88)))), ((int) (((byte) (168)))), ((int) (((byte) (35))))));
            colorPalette4.Colors.Add(System.Drawing.Color.FromArgb(((int) (((byte) (137)))), ((int) (((byte) (203)))), ((int) (((byte) (42))))));
            colorPalette4.Colors.Add(System.Drawing.Color.FromArgb(((int) (((byte) (34)))), ((int) (((byte) (181)))), ((int) (((byte) (115))))));
            colorPalette4.Colors.Add(System.Drawing.Color.FromArgb(((int) (((byte) (32)))), ((int) (((byte) (176)))), ((int) (((byte) (212))))));
            this.StoreQuarterPie.ColorPalette = colorPalette4;
            this.StoreQuarterPie.CoordinateSystems.Add(this.polarCoordinateSystem2);
            this.StoreQuarterPie.Culture = new System.Globalization.CultureInfo("");
            this.StoreQuarterPie.DataSource = this.mainDataSource;
            this.StoreQuarterPie.Legend.Height = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.StoreQuarterPie.Legend.Position = Telerik.Reporting.GraphItemPosition.LeftCenter;
            this.StoreQuarterPie.Legend.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7D);
            this.StoreQuarterPie.Legend.Style.LineColor = System.Drawing.Color.LightGray;
            this.StoreQuarterPie.Legend.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.StoreQuarterPie.Legend.Width = Telerik.Reporting.Drawing.Unit.Pixel(100D);
            this.StoreQuarterPie.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(91.44000244140625D), Telerik.Reporting.Drawing.Unit.Mm(20.829999923706055D));
            this.StoreQuarterPie.Name = "StoreQuarterPie";
            this.StoreQuarterPie.PlotAreaStyle.LineColor = System.Drawing.Color.LightGray;
            this.StoreQuarterPie.PlotAreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.StoreQuarterPie.Series.Add(this.barSeries4);
            this.StoreQuarterPie.SeriesGroups.Add(graphGroup8);
            this.StoreQuarterPie.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(78.2300033569336D), Telerik.Reporting.Drawing.Unit.Mm(43.180000305175781D));
            this.StoreQuarterPie.Style.Font.Name = "Segoe UI";
            this.StoreQuarterPie.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(5D);
            this.StoreQuarterPie.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(0D);
            this.StoreQuarterPie.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.StoreQuarterPie.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(5D);
            // 
            // polarCoordinateSystem2
            // 
            this.polarCoordinateSystem2.AngularAxis = this.graphAxis7;
            this.polarCoordinateSystem2.Name = "polarCoordinateSystem1";
            this.polarCoordinateSystem2.RadialAxis = this.graphAxis8;
            // 
            // graphAxis7
            // 
            this.graphAxis7.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis7.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis7.MajorGridLineStyle.Visible = false;
            this.graphAxis7.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis7.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis7.MinorGridLineStyle.Visible = false;
            this.graphAxis7.Name = "graphAxis1";
            this.graphAxis7.Scale = numericalScale4;
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
            this.graphAxis8.Name = "graphAxis2";
            categoryScale4.PositionMode = Telerik.Reporting.AxisPositionMode.OnTicks;
            categoryScale4.SpacingSlotCount = 0D;
            this.graphAxis8.Scale = categoryScale4;
            this.graphAxis8.Style.Visible = false;
            // 
            // barSeries4
            // 
            this.barSeries4.ArrangeMode = Telerik.Reporting.GraphSeriesArrangeMode.Stacked100;
            this.barSeries4.CategoryGroup = graphGroup7;
            this.barSeries4.CoordinateSystem = this.polarCoordinateSystem2;
            this.barSeries4.DataPointLabel = "=Sum(Fields.LineTotal)";
            this.barSeries4.DataPointLabelStyle.Visible = false;
            this.barSeries4.DataPointStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.barSeries4.DataPointStyle.Visible = true;
            this.barSeries4.LegendItem.Value = "= Fields.StoreName";
            graphGroup8.Filters.Add(new Telerik.Reporting.Filter("=Sum(Fields.LineTotal)", Telerik.Reporting.FilterOperator.TopN, "=5"));
            graphGroup8.Groupings.Add(new Telerik.Reporting.Grouping("= Fields.StoreName"));
            graphGroup8.Name = "ProductStoreGroup";
            graphGroup8.Sortings.Add(new Telerik.Reporting.Sorting("=Sum(Fields.LineTotal)", Telerik.Reporting.SortDirection.Desc));
            this.barSeries4.SeriesGroup = graphGroup8;
            this.barSeries4.X = "=Sum(Fields.LineTotal)";
            this.barSeries4.ToolTip.Title = "=Fields.StoreName";
            this.barSeries4.ToolTip.Text = "=Format('Total: ${0:N2} K', Sum(Fields.LineTotal))";
            // 
            // panelSalesByProduct
            // 
            this.panelSalesByProduct.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.panel6,
            this.textBox15,
            this.textBox16,
            this.textBox8,
            this.ProductQuarterPie,
            this.ProductQuarterBar});
            this.panelSalesByProduct.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(5.0203156471252441D));
            this.panelSalesByProduct.Name = "panelSalesByProduct";
            this.panelSalesByProduct.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(10.399999618530273D), Telerik.Reporting.Drawing.Unit.Inch(2.520416259765625D));
            // 
            // panel6
            // 
            this.panel6.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.ProductQuarter,
            this.textBox10});
            this.panel6.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.5D));
            this.panel6.Name = "panel6";
            this.panel6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.5999999046325684D), Telerik.Reporting.Drawing.Unit.Inch(2.0199999809265137D));
            // 
            // ProductQuarter
            // 
            this.ProductQuarter.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.38188984990119934D)));
            this.ProductQuarter.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.38188984990119934D)));
            this.ProductQuarter.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Inch(0.24015745520591736D)));
            this.ProductQuarter.Body.SetCellContent(0, 0, this.pTotalQ);
            this.ProductQuarter.Body.SetCellContent(0, 1, this.pTotal);
            tableGroup7.Groupings.Add(new Telerik.Reporting.Grouping("=Quarter(Fields.OrderDate)"));
            tableGroup7.Name = "Quater";
            tableGroup7.ReportItem = this.pLabelTotalQ;
            tableGroup7.Sortings.Add(new Telerik.Reporting.Sorting("=Quarter(Fields.OrderDate)", Telerik.Reporting.SortDirection.Asc));
            tableGroup8.ReportItem = this.pLabelTotal;
            this.ProductQuarter.ColumnGroups.Add(tableGroup7);
            this.ProductQuarter.ColumnGroups.Add(tableGroup8);
            this.ProductQuarter.Corner.SetCellContent(0, 0, this.pTextCorner);
            this.ProductQuarter.DataSource = this.mainDataSource;
            this.ProductQuarter.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pTextCorner,
            this.pTotalQ,
            this.pTotal,
            this.pLabelTotalQ,
            this.pLabelTotal,
            this.pProductName});
            this.ProductQuarter.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.31999999284744263D));
            this.ProductQuarter.Name = "ProductQuarter";
            tableGroup9.Filters.Add(new Telerik.Reporting.Filter("=Sum(Fields.LineTotal)", Telerik.Reporting.FilterOperator.TopN, "=5"));
            tableGroup9.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.ProductName"));
            tableGroup9.Name = "ProductNameGroup";
            tableGroup9.ReportItem = this.pProductName;
            tableGroup9.Sortings.Add(new Telerik.Reporting.Sorting("=Sum(Fields.LineTotal)", Telerik.Reporting.SortDirection.Desc));
            this.ProductQuarter.RowGroups.Add(tableGroup9);
            this.ProductQuarter.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.2637796401977539D), Telerik.Reporting.Drawing.Unit.Inch(0.48015749454498291D));
            this.ProductQuarter.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            // 
            // pTotalQ
            // 
            this.pTotalQ.Format = "{0:#.}";
            this.pTotalQ.Name = "pTotalQ";
            this.pTotalQ.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.38188979029655457D), Telerik.Reporting.Drawing.Unit.Inch(0.24015748500823975D));
            this.pTotalQ.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.pTotalQ.Value = "=Sum(Fields.LineTotal)";
            // 
            // pTotal
            // 
            this.pTotal.Format = "{0:#.}";
            this.pTotal.Name = "pTotal";
            this.pTotal.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.38188979029655457D), Telerik.Reporting.Drawing.Unit.Inch(0.24015748500823975D));
            this.pTotal.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.pTotal.Value = "=Sum(Fields.LineTotal)";
            // 
            // pTextCorner
            // 
            this.pTextCorner.Name = "pTextCorner";
            this.pTextCorner.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.5D), Telerik.Reporting.Drawing.Unit.Inch(0.23999999463558197D));
            this.pTextCorner.Style.Color = System.Drawing.Color.FromArgb(((int) (((byte) (236)))), ((int) (((byte) (57)))), ((int) (((byte) (27)))));
            this.pTextCorner.Value = "Product";
            // 
            // textBox10
            // 
            this.textBox10.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.7000000476837158D), Telerik.Reporting.Drawing.Unit.Inch(0.31999999284744263D));
            this.textBox10.StyleName = "legend";
            this.textBox10.Value = "SALES AMOUNT IN USD (THOUSANDS)";
            // 
            // textBox15
            // 
            this.textBox15.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.5999999046325684D), Telerik.Reporting.Drawing.Unit.Inch(0.5D));
            this.textBox15.Name = "textBox15";
            this.textBox15.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.0799999237060547D), Telerik.Reporting.Drawing.Unit.Inch(0.31999999284744263D));
            this.textBox15.StyleName = "legend";
            this.textBox15.Value = "YEARLY SALES DISTRIBUTION";
            // 
            // textBox16
            // 
            this.textBox16.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.679999828338623D), Telerik.Reporting.Drawing.Unit.Inch(0.5D));
            this.textBox16.Name = "textBox16";
            this.textBox16.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.7200000286102295D), Telerik.Reporting.Drawing.Unit.Inch(0.31999999284744263D));
            this.textBox16.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox16.StyleName = "legend";
            this.textBox16.Value = "QUARTERLY SALES DISTRIBUTION";
            // 
            // textBox8
            // 
            this.textBox8.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(10.380000114440918D), Telerik.Reporting.Drawing.Unit.Inch(0.36000001430511475D));
            this.textBox8.Style.BorderColor.Bottom = System.Drawing.Color.FromArgb(((int) (((byte) (202)))), ((int) (((byte) (137)))), ((int) (((byte) (104)))));
            this.textBox8.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox8.Style.Color = System.Drawing.Color.FromArgb(((int) (((byte) (236)))), ((int) (((byte) (57)))), ((int) (((byte) (27)))));
            this.textBox8.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(16D);
            this.textBox8.TocText = "Top 5 Performing Products";
            this.textBox8.Value = "Top 5 performing products";
            // 
            // ProductQuarterPie
            // 
            graphGroup9.Name = "categoryGroup";
            this.ProductQuarterPie.CategoryGroups.Add(graphGroup9);
            colorPalette5.Colors.Add(System.Drawing.Color.FromArgb(((int) (((byte) (25)))), ((int) (((byte) (7)))), ((int) (((byte) (5))))));
            colorPalette5.Colors.Add(System.Drawing.Color.FromArgb(((int) (((byte) (87)))), ((int) (((byte) (35)))), ((int) (((byte) (17))))));
            colorPalette5.Colors.Add(System.Drawing.Color.FromArgb(((int) (((byte) (195)))), ((int) (((byte) (47)))), ((int) (((byte) (11))))));
            colorPalette5.Colors.Add(System.Drawing.Color.FromArgb(((int) (((byte) (236)))), ((int) (((byte) (57)))), ((int) (((byte) (27))))));
            colorPalette5.Colors.Add(System.Drawing.Color.FromArgb(((int) (((byte) (255)))), ((int) (((byte) (89)))), ((int) (((byte) (56))))));
            this.ProductQuarterPie.ColorPalette = colorPalette5;
            this.ProductQuarterPie.CoordinateSystems.Add(this.polarCoordinateSystem3);
            this.ProductQuarterPie.Culture = new System.Globalization.CultureInfo("");
            this.ProductQuarterPie.DataSource = this.mainDataSource;
            this.ProductQuarterPie.Legend.Height = Telerik.Reporting.Drawing.Unit.Pixel(300D);
            this.ProductQuarterPie.Legend.Position = Telerik.Reporting.GraphItemPosition.LeftCenter;
            this.ProductQuarterPie.Legend.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7D);
            this.ProductQuarterPie.Legend.Style.LineColor = System.Drawing.Color.LightGray;
            this.ProductQuarterPie.Legend.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.ProductQuarterPie.Legend.Width = Telerik.Reporting.Drawing.Unit.Pixel(100D);
            this.ProductQuarterPie.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(91.44000244140625D), Telerik.Reporting.Drawing.Unit.Mm(20.829994201660156D));
            this.ProductQuarterPie.Name = "ProductQuarterPie";
            this.ProductQuarterPie.PlotAreaStyle.LineColor = System.Drawing.Color.LightGray;
            this.ProductQuarterPie.PlotAreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.ProductQuarterPie.Series.Add(this.barSeries5);
            this.ProductQuarterPie.SeriesGroups.Add(graphGroup10);
            this.ProductQuarterPie.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(78.2300033569336D), Telerik.Reporting.Drawing.Unit.Mm(43.180000305175781D));
            this.ProductQuarterPie.Style.Font.Name = "Segoe UI";
            this.ProductQuarterPie.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.ProductQuarterPie.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(5D);
            this.ProductQuarterPie.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(0D);
            this.ProductQuarterPie.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.ProductQuarterPie.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(5D);
            // 
            // polarCoordinateSystem3
            // 
            this.polarCoordinateSystem3.AngularAxis = this.graphAxis9;
            this.polarCoordinateSystem3.Name = "polarCoordinateSystem1";
            this.polarCoordinateSystem3.RadialAxis = this.graphAxis10;
            // 
            // graphAxis9
            // 
            this.graphAxis9.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis9.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis9.MajorGridLineStyle.Visible = false;
            this.graphAxis9.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis9.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis9.MinorGridLineStyle.Visible = false;
            this.graphAxis9.Name = "graphAxis1";
            this.graphAxis9.Scale = numericalScale5;
            this.graphAxis9.Style.Visible = false;
            // 
            // graphAxis10
            // 
            this.graphAxis10.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis10.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis10.MajorGridLineStyle.Visible = false;
            this.graphAxis10.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis10.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis10.MinorGridLineStyle.Visible = false;
            this.graphAxis10.Name = "graphAxis2";
            categoryScale5.PositionMode = Telerik.Reporting.AxisPositionMode.OnTicks;
            categoryScale5.SpacingSlotCount = 0D;
            this.graphAxis10.Scale = categoryScale5;
            this.graphAxis10.Style.Visible = false;
            // 
            // barSeries5
            // 
            this.barSeries5.ArrangeMode = Telerik.Reporting.GraphSeriesArrangeMode.Stacked100;
            this.barSeries5.CategoryGroup = graphGroup9;
            this.barSeries5.CoordinateSystem = this.polarCoordinateSystem3;
            this.barSeries5.DataPointLabel = "=Sum(Fields.LineTotal)";
            this.barSeries5.DataPointLabelStyle.Visible = false;
            this.barSeries5.DataPointStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.barSeries5.DataPointStyle.Visible = true;
            this.barSeries5.LegendItem.Value = "=Fields.ProductName";
            graphGroup10.Filters.Add(new Telerik.Reporting.Filter("=Sum(Fields.LineTotal)", Telerik.Reporting.FilterOperator.TopN, "=5"));
            graphGroup10.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.ProductName"));
            graphGroup10.Name = "ProductNameGroup";
            graphGroup10.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.ProductName", Telerik.Reporting.SortDirection.Asc));
            this.barSeries5.SeriesGroup = graphGroup10;
            this.barSeries5.X = "=Sum(Fields.LineTotal)";
            this.barSeries5.ToolTip.Title = "=Fields.ProductName";
            this.barSeries5.ToolTip.Text = "=Format('Total: ${0:N2} K', Sum(Fields.LineTotal))";
            // 
            // ProductQuarterBar
            // 
            graphGroup11.Filters.Add(new Telerik.Reporting.Filter("=Sum(Fields.LineTotal)", Telerik.Reporting.FilterOperator.TopN, "=5"));
            graphGroup11.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.ProductName"));
            graphGroup11.Name = "ProductNameGroup";
            graphGroup11.Sortings.Add(new Telerik.Reporting.Sorting("=Sum(Fields.LineTotal)", Telerik.Reporting.SortDirection.Asc));
            this.ProductQuarterBar.CategoryGroups.Add(graphGroup11);
            colorPalette6.Colors.Add(System.Drawing.Color.FromArgb(((int) (((byte) (25)))), ((int) (((byte) (7)))), ((int) (((byte) (5))))));
            colorPalette6.Colors.Add(System.Drawing.Color.FromArgb(((int) (((byte) (87)))), ((int) (((byte) (35)))), ((int) (((byte) (17))))));
            colorPalette6.Colors.Add(System.Drawing.Color.FromArgb(((int) (((byte) (195)))), ((int) (((byte) (47)))), ((int) (((byte) (11))))));
            colorPalette6.Colors.Add(System.Drawing.Color.FromArgb(((int) (((byte) (236)))), ((int) (((byte) (57)))), ((int) (((byte) (27))))));
            colorPalette6.Colors.Add(System.Drawing.Color.FromArgb(((int) (((byte) (255)))), ((int) (((byte) (89)))), ((int) (((byte) (56))))));
            this.ProductQuarterBar.ColorPalette = colorPalette6;
            this.ProductQuarterBar.CoordinateSystems.Add(this.cartesianCoordinateSystem3);
            this.ProductQuarterBar.DataSource = this.mainDataSource;
            this.ProductQuarterBar.Legend.Position = Telerik.Reporting.GraphItemPosition.TopCenter;
            this.ProductQuarterBar.Legend.Style.LineColor = System.Drawing.Color.LightGray;
            this.ProductQuarterBar.Legend.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.ProductQuarterBar.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(169.66999816894531D), Telerik.Reporting.Drawing.Unit.Mm(20.829999923706055D));
            this.ProductQuarterBar.Name = "ProductQuarterBar";
            this.ProductQuarterBar.PlotAreaStyle.LineColor = System.Drawing.Color.LightGray;
            this.ProductQuarterBar.PlotAreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.ProductQuarterBar.Series.Add(this.barSeries6);
            this.ProductQuarterBar.SeriesGroups.Add(graphGroup12);
            this.ProductQuarterBar.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(94.489997863769531D), Telerik.Reporting.Drawing.Unit.Mm(43.180000305175781D));
            this.ProductQuarterBar.Style.Font.Name = "Segoe UI";
            this.ProductQuarterBar.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.ProductQuarterBar.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(5D);
            this.ProductQuarterBar.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.ProductQuarterBar.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.ProductQuarterBar.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            // 
            // cartesianCoordinateSystem3
            // 
            this.cartesianCoordinateSystem3.Name = "cartesianCoordinateSystem1";
            this.cartesianCoordinateSystem3.XAxis = this.graphAxis11;
            this.cartesianCoordinateSystem3.YAxis = this.graphAxis12;
            // 
            // graphAxis11
            // 
            this.graphAxis11.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis11.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis11.MajorGridLineStyle.Visible = false;
            this.graphAxis11.MajorTickMarkDisplayType = Telerik.Reporting.GraphAxisTickMarkDisplayType.Outside;
            this.graphAxis11.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis11.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis11.MinorGridLineStyle.Visible = false;
            this.graphAxis11.Name = "graphAxis3";
            this.graphAxis11.Scale = numericalScale6;
            this.graphAxis11.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.graphAxis11.TitlePlacement = Telerik.Reporting.GraphAxisTitlePlacement.Auto;
            // 
            // graphAxis12
            // 
            this.graphAxis12.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis12.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis12.MajorGridLineStyle.Visible = false;
            this.graphAxis12.MajorTickMarkDisplayType = Telerik.Reporting.GraphAxisTickMarkDisplayType.None;
            this.graphAxis12.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis12.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis12.MinorGridLineStyle.Visible = false;
            this.graphAxis12.Name = "graphAxis4";
            categoryScale6.SpacingSlotCount = 0.4D;
            this.graphAxis12.Scale = categoryScale6;
            this.graphAxis12.Size = Telerik.Reporting.Drawing.Unit.Inch(1.2999999523162842D);
            this.graphAxis12.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7D);
            this.graphAxis12.Style.Visible = true;
            // 
            // barSeries6
            // 
            this.barSeries6.ArrangeMode = Telerik.Reporting.GraphSeriesArrangeMode.Stacked;
            this.barSeries6.CategoryGroup = graphGroup11;
            this.barSeries6.CoordinateSystem = this.cartesianCoordinateSystem3;
            this.barSeries6.DataPointLabel = "=Sum(Fields.LineTotal)";
            this.barSeries6.DataPointLabelStyle.Visible = false;
            this.barSeries6.DataPointStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.barSeries6.DataPointStyle.Visible = true;
            this.barSeries6.LegendItem.Value = "=\'Q\' + Quarter(Fields.OrderDate)";
            graphGroup12.Groupings.Add(new Telerik.Reporting.Grouping("= Quarter(Fields.OrderDate)"));
            graphGroup12.Label = "=\'Q\' + Quarter(Fields.OrderDate)";
            graphGroup12.Name = "OrderDate.QuarterGroup";
            graphGroup12.Sortings.Add(new Telerik.Reporting.Sorting("= Quarter(Fields.OrderDate)", Telerik.Reporting.SortDirection.Asc));
            this.barSeries6.SeriesGroup = graphGroup12;
            this.barSeries6.X = "=Sum(Fields.LineTotal)";
            this.barSeries6.ToolTip.Title = "=Format('Q{0} results for {1}', Quarter(Fields.OrderDate), Fields.ProductName)";
            this.barSeries6.ToolTip.Text = "=Format('${0:N2} K', Sum(Fields.LineTotal))";
            // 
            // textBox5
            // 
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.4895833432674408D), Telerik.Reporting.Drawing.Unit.Inch(0.17708373069763184D));
            // 
            // reportHeaderSection1
            // 
            this.reportHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.70003944635391235D);
            this.reportHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.panel5});
            this.reportHeaderSection1.Name = "reportHeaderSection1";
            this.reportHeaderSection1.Style.Visible = true;
            // 
            // panel5
            // 
            this.panel5.Anchoring = ((Telerik.Reporting.AnchoringStyles) ((Telerik.Reporting.AnchoringStyles.Left | Telerik.Reporting.AnchoringStyles.Right)));
            this.panel5.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox1,
            this.shape5,
            this.textBox6});
            this.panel5.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(0.068051397800445557D), Telerik.Reporting.Drawing.Unit.Mm(0.0010012307902798057D));
            this.panel5.Name = "panel5";
            this.panel5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(10.407281875610352D), Telerik.Reporting.Drawing.Unit.Mm(17.780000686645508D));
            // 
            // textBox1
            // 
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0026791889686137438D), Telerik.Reporting.Drawing.Unit.Inch(3.9339065551757812E-05D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(4.2124595642089844D), Telerik.Reporting.Drawing.Unit.Inch(0.59992140531539917D));
            this.textBox1.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(34D);
            this.textBox1.StyleName = "Header";
            this.textBox1.Value = "Quarterly Sales";
            // 
            // shape5
            // 
            this.shape5.Anchoring = ((Telerik.Reporting.AnchoringStyles) ((Telerik.Reporting.AnchoringStyles.Left | Telerik.Reporting.AnchoringStyles.Right)));
            this.shape5.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0026791889686137438D), Telerik.Reporting.Drawing.Unit.Inch(0.60003942251205444D));
            this.shape5.Name = "shape5";
            this.shape5.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.shape5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(10.399999618530273D), Telerik.Reporting.Drawing.Unit.Point(3.75D));
            this.shape5.Stretch = true;
            this.shape5.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.shape5.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            this.shape5.Style.BorderWidth.Top = Telerik.Reporting.Drawing.Unit.Point(3D);
            this.shape5.Style.Color = System.Drawing.Color.Transparent;
            // 
            // textBox6
            // 
            this.textBox6.Anchoring = Telerik.Reporting.AnchoringStyles.Right;
            this.textBox6.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(8.25732135772705D), Telerik.Reporting.Drawing.Unit.Inch(0.22485215961933136D));
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1199996471405029D), Telerik.Reporting.Drawing.Unit.Inch(0.22011041641235352D));
            this.textBox6.Style.Color = System.Drawing.Color.FromArgb(((int) (((byte) (195)))), ((int) (((byte) (47)))), ((int) (((byte) (11)))));
            this.textBox6.Style.Font.Name = "Segoe UI";
            this.textBox6.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox6.Value = "[INTERNAL PURPOSES ONLY]";
            // 
            // Dashboard
            // 
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.detail,
            this.reportHeaderSection1});
            this.Name = "Dashboard";
            this.PageSettings.Landscape = true;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Inch(0.10000000149011612D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Letter;
            reportParameter1.AutoRefresh = true;
            reportParameter1.AvailableValues.DataSource = this.yearDataSource;
            reportParameter1.AvailableValues.DisplayMember = "Year";
            reportParameter1.AvailableValues.ValueMember = "Year";
            reportParameter1.Name = "ReportYear";
            reportParameter1.Text = "Sales for Year";
            reportParameter1.Type = Telerik.Reporting.ReportParameterType.Integer;
            reportParameter1.Value = "=2001";
            reportParameter1.Visible = true;
            this.ReportParameters.Add(reportParameter1);
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
            styleRule3.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextBox))});
            styleRule3.Style.Font.Name = "Segoe UI Light";
            styleRule3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            styleRule3.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(21D);
            styleRule4.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("legend")});
            styleRule4.Style.Color = System.Drawing.Color.FromArgb(((int) (((byte) (152)))), ((int) (((byte) (152)))), ((int) (((byte) (152)))));
            styleRule4.Style.Font.Name = "Segoe UI Light";
            styleRule4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1,
            styleRule2,
            styleRule3,
            styleRule4});
            this.TocText = "Dashboard";
            this.UnitOfMeasure = Telerik.Reporting.Drawing.UnitType.Mm;
            this.Width = Telerik.Reporting.Drawing.Unit.Inch(10.409999847412109D);
            ((System.ComponentModel.ISupportInitialize) (this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.TextBox textBox5;
        private ReportHeaderSection reportHeaderSection1;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.TextBox textBox6;
        private Shape shape5;
        private Reporting.Panel panel5;
        private SqlDataSource yearDataSource;
        private Reporting.Panel panel2;
        private Reporting.Panel panel3;
        private Crosstab SalesPersonQuarter;
        private Reporting.TextBox spTotalQ;
        private Reporting.TextBox spTotal;
        private Reporting.TextBox spLabelTotalQ;
        private Reporting.TextBox spLabelTotal;
        private Reporting.TextBox spTextCorner;
        private SqlDataSource mainDataSource;
        private Reporting.TextBox spLabelSalesPerson;
        private Reporting.TextBox textBox2;
        private Reporting.TextBox textBox11;
        private Reporting.TextBox textBox12;
        private Reporting.TextBox textBox3;
        private Reporting.Graph SalesPersonQuarterBar;
        private CartesianCoordinateSystem cartesianCoordinateSystem1;
        private GraphAxis graphAxis3;
        private GraphAxis graphAxis4;
        private BarSeries barSeries1;
        private Reporting.Graph SalesPersonQuarterPie;
        private PolarCoordinateSystem polarCoordinateSystem1;
        private GraphAxis graphAxis1;
        private GraphAxis graphAxis2;
        private BarSeries barSeries2;
        private Reporting.Panel panel1;
        private Reporting.Panel panel4;
        private Crosstab StoreQuarter;
        private Reporting.TextBox sTotalQ;
        private Reporting.TextBox sTotal;
        private Reporting.TextBox sLabelTotalQ;
        private Reporting.TextBox sLabelTotal;
        private Reporting.TextBox sTextCorner;
        private Reporting.TextBox sLabelStore;
        private Reporting.TextBox textBox9;
        private Reporting.TextBox textBox13;
        private Reporting.TextBox textBox14;
        private Reporting.TextBox textBox7;
        private Reporting.Graph StoreQuarterBar;
        private CartesianCoordinateSystem cartesianCoordinateSystem2;
        private GraphAxis graphAxis5;
        private GraphAxis graphAxis6;
        private BarSeries barSeries3;
        private Reporting.Graph StoreQuarterPie;
        private PolarCoordinateSystem polarCoordinateSystem2;
        private GraphAxis graphAxis7;
        private GraphAxis graphAxis8;
        private BarSeries barSeries4;
        private Reporting.Panel panelSalesByProduct;
        private Reporting.Panel panel6;
        private Crosstab ProductQuarter;
        private Reporting.TextBox pTotalQ;
        private Reporting.TextBox pTotal;
        private Reporting.TextBox pLabelTotalQ;
        private Reporting.TextBox pLabelTotal;
        private Reporting.TextBox pTextCorner;
        private Reporting.TextBox pProductName;
        private Reporting.TextBox textBox10;
        private Reporting.TextBox textBox15;
        private Reporting.TextBox textBox16;
        private Reporting.TextBox textBox8;
        private Reporting.Graph ProductQuarterPie;
        private PolarCoordinateSystem polarCoordinateSystem3;
        private GraphAxis graphAxis9;
        private GraphAxis graphAxis10;
        private BarSeries barSeries5;
        private Reporting.Graph ProductQuarterBar;
        private CartesianCoordinateSystem cartesianCoordinateSystem3;
        private GraphAxis graphAxis11;
        private GraphAxis graphAxis12;
        private BarSeries barSeries6;
    }
}