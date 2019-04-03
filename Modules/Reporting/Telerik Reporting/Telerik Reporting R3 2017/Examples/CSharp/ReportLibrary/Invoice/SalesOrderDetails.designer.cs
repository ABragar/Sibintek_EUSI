namespace Telerik.Reporting.Examples.CSharp
{
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    partial class SalesOrderDetails
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.Reporting.SortingAction sortingAction1 = new Telerik.Reporting.SortingAction();
            Telerik.Reporting.SortingAction sortingAction2 = new Telerik.Reporting.SortingAction();
            Telerik.Reporting.SortingAction sortingAction3 = new Telerik.Reporting.SortingAction();
            Telerik.Reporting.SortingAction sortingAction4 = new Telerik.Reporting.SortingAction();
            Telerik.Reporting.SortingAction sortingAction5 = new Telerik.Reporting.SortingAction();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SalesOrderDetails));
            Telerik.Reporting.Group group1 = new Telerik.Reporting.Group();
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule4 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule5 = new Telerik.Reporting.Drawing.StyleRule();
            this.labelsGroupFooter = new Telerik.Reporting.GroupFooterSection();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.labelsGroupHeader = new Telerik.Reporting.GroupHeaderSection();
            this.panel1 = new Telerik.Reporting.Panel();
            this.productNumberCaptionTextBox = new Telerik.Reporting.TextBox();
            this.nameCaptionTextBox = new Telerik.Reporting.TextBox();
            this.orderQtyCaptionTextBox = new Telerik.Reporting.TextBox();
            this.unitPriceCaptionTextBox = new Telerik.Reporting.TextBox();
            this.lineTotalCaptionTextBox = new Telerik.Reporting.TextBox();
            this.detail = new Telerik.Reporting.DetailSection();
            this.productNumberDataTextBox = new Telerik.Reporting.TextBox();
            this.nameDataTextBox = new Telerik.Reporting.TextBox();
            this.orderQtyDataTextBox = new Telerik.Reporting.TextBox();
            this.unitPriceDataTextBox = new Telerik.Reporting.TextBox();
            this.lineTotalDataTextBox = new Telerik.Reporting.TextBox();
            this.sqlDataSource1 = new Telerik.Reporting.SqlDataSource();
            this.reportHeaderSection1 = new Telerik.Reporting.ReportHeaderSection();
            this.textBox3 = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // labelsGroupFooter
            // 
            this.labelsGroupFooter.Height = Telerik.Reporting.Drawing.Unit.Inch(0.47999998927116394D);
            this.labelsGroupFooter.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox1,
            this.textBox2});
            this.labelsGroupFooter.Name = "labelsGroupFooter";
            // 
            // textBox1
            // 
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.9418537198798731E-05D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(6.1228375434875488D), Telerik.Reporting.Drawing.Unit.Inch(0.47996059060096741D));
            this.textBox1.StyleName = "Total";
            this.textBox1.Value = "Total";
            // 
            // textBox2
            // 
            this.textBox2.Format = "{0:C2}";
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.1229557991027832D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.9669651985168457D), Telerik.Reporting.Drawing.Unit.Inch(0.47996059060096741D));
            this.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox2.StyleName = "Total";
            this.textBox2.Value = "=Sum(Fields.LineTotal)";
            // 
            // labelsGroupHeader
            // 
            this.labelsGroupHeader.Height = Telerik.Reporting.Drawing.Unit.Inch(0.47999998927116394D);
            this.labelsGroupHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.panel1});
            this.labelsGroupHeader.Name = "labelsGroupHeader";
            this.labelsGroupHeader.PrintOnEveryPage = true;
            // 
            // panel1
            // 
            this.panel1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.productNumberCaptionTextBox,
            this.nameCaptionTextBox,
            this.orderQtyCaptionTextBox,
            this.unitPriceCaptionTextBox,
            this.lineTotalCaptionTextBox});
            this.panel1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.panel1.Name = "panel1";
            this.panel1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.089961051940918D), Telerik.Reporting.Drawing.Unit.Inch(0.47999998927116394D));
            // 
            // productNumberCaptionTextBox
            // 
            sortingAction1.SortingExpression = "= Fields.ProductNumber";
            sortingAction1.Targets.AddRange(new Telerik.Reporting.IActionTarget[] {
            this});
            this.productNumberCaptionTextBox.Action = sortingAction1;
            this.productNumberCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(3.9378803194267675E-05D));
            this.productNumberCaptionTextBox.Name = "productNumberCaptionTextBox";
            this.productNumberCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.4000000953674316D), Telerik.Reporting.Drawing.Unit.Inch(0.47988176345825195D));
            this.productNumberCaptionTextBox.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            this.productNumberCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.productNumberCaptionTextBox.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.productNumberCaptionTextBox.StyleName = "Caption";
            this.productNumberCaptionTextBox.Value = "Product No.";
            // 
            // nameCaptionTextBox
            // 
            sortingAction2.SortingExpression = "= Fields.Name";
            sortingAction2.Targets.AddRange(new Telerik.Reporting.IActionTarget[] {
            this});
            this.nameCaptionTextBox.Action = sortingAction2;
            this.nameCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.4000000953674316D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.nameCaptionTextBox.Name = "nameCaptionTextBox";
            this.nameCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.5D), Telerik.Reporting.Drawing.Unit.Inch(0.47992119193077087D));
            this.nameCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.nameCaptionTextBox.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.nameCaptionTextBox.StyleName = "Caption";
            this.nameCaptionTextBox.Value = "Name";
            // 
            // orderQtyCaptionTextBox
            // 
            sortingAction3.SortingExpression = "= Fields.OrderQty";
            sortingAction3.Targets.AddRange(new Telerik.Reporting.IActionTarget[] {
            this});
            this.orderQtyCaptionTextBox.Action = sortingAction3;
            this.orderQtyCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.9000792503356934D), Telerik.Reporting.Drawing.Unit.Inch(-1.2417634032146907E-08D));
            this.orderQtyCaptionTextBox.Name = "orderQtyCaptionTextBox";
            this.orderQtyCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.999882161617279D), Telerik.Reporting.Drawing.Unit.Inch(0.47992119193077087D));
            this.orderQtyCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.orderQtyCaptionTextBox.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.orderQtyCaptionTextBox.StyleName = "Caption";
            this.orderQtyCaptionTextBox.Value = "Quantity";
            // 
            // unitPriceCaptionTextBox
            // 
            sortingAction4.SortingExpression = "= Fields.UnitPrice";
            sortingAction4.Targets.AddRange(new Telerik.Reporting.IActionTarget[] {
            this});
            this.unitPriceCaptionTextBox.Action = sortingAction4;
            this.unitPriceCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.9000000953674316D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.unitPriceCaptionTextBox.Name = "unitPriceCaptionTextBox";
            this.unitPriceCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.0999211072921753D), Telerik.Reporting.Drawing.Unit.Inch(0.47992119193077087D));
            this.unitPriceCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.unitPriceCaptionTextBox.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.unitPriceCaptionTextBox.StyleName = "Caption";
            this.unitPriceCaptionTextBox.Value = "Unit Price";
            // 
            // lineTotalCaptionTextBox
            // 
            sortingAction5.SortingExpression = "= Fields.LineTotal";
            sortingAction5.Targets.AddRange(new Telerik.Reporting.IActionTarget[] {
            this});
            this.lineTotalCaptionTextBox.Action = sortingAction5;
            this.lineTotalCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(7D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.lineTotalCaptionTextBox.Name = "lineTotalCaptionTextBox";
            this.lineTotalCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.0899604558944702D), Telerik.Reporting.Drawing.Unit.Inch(0.47992119193077087D));
            this.lineTotalCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.lineTotalCaptionTextBox.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.lineTotalCaptionTextBox.StyleName = "Caption";
            this.lineTotalCaptionTextBox.Value = "Line Total";
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Inch(0.47999998927116394D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.productNumberDataTextBox,
            this.nameDataTextBox,
            this.orderQtyDataTextBox,
            this.unitPriceDataTextBox,
            this.lineTotalDataTextBox});
            this.detail.Name = "detail";
            // 
            // productNumberDataTextBox
            // 
            this.productNumberDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.productNumberDataTextBox.Name = "productNumberDataTextBox";
            this.productNumberDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.4000000953674316D), Telerik.Reporting.Drawing.Unit.Inch(0.47992119193077087D));
            this.productNumberDataTextBox.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            this.productNumberDataTextBox.StyleName = "Data";
            this.productNumberDataTextBox.Value = "=Fields.ProductNumber";
            // 
            // nameDataTextBox
            // 
            this.nameDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.4000000953674316D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.nameDataTextBox.Name = "nameDataTextBox";
            this.nameDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.4999992847442627D), Telerik.Reporting.Drawing.Unit.Inch(0.47992119193077087D));
            this.nameDataTextBox.StyleName = "Data";
            this.nameDataTextBox.Value = "=Fields.Name";
            // 
            // orderQtyDataTextBox
            // 
            this.orderQtyDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.9000787734985352D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.orderQtyDataTextBox.Name = "orderQtyDataTextBox";
            this.orderQtyDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.99984258413314819D), Telerik.Reporting.Drawing.Unit.Inch(0.47992119193077087D));
            this.orderQtyDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.orderQtyDataTextBox.StyleName = "Data";
            this.orderQtyDataTextBox.Value = "=Fields.OrderQty";
            // 
            // unitPriceDataTextBox
            // 
            this.unitPriceDataTextBox.Format = "{0:C2}";
            this.unitPriceDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.9000000953674316D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.unitPriceDataTextBox.Name = "unitPriceDataTextBox";
            this.unitPriceDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.0999206304550171D), Telerik.Reporting.Drawing.Unit.Inch(0.47992119193077087D));
            this.unitPriceDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.unitPriceDataTextBox.StyleName = "Data";
            this.unitPriceDataTextBox.Value = "=Fields.UnitPrice";
            // 
            // lineTotalDataTextBox
            // 
            this.lineTotalDataTextBox.Format = "{0:C2}";
            this.lineTotalDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(7D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.lineTotalDataTextBox.Name = "lineTotalDataTextBox";
            this.lineTotalDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.0899215936660767D), Telerik.Reporting.Drawing.Unit.Inch(0.47992119193077087D));
            this.lineTotalDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.lineTotalDataTextBox.StyleName = "Data";
            this.lineTotalDataTextBox.Value = "=Fields.LineTotal";
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionString = "Telerik.Reporting.Examples.CSharp.Properties.Settings.TelerikConnectionString";
            this.sqlDataSource1.Name = "sqlDataSource1";
            this.sqlDataSource1.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@SalesOrderID", System.Data.DbType.Int32, "=Parameters.SaledOrderID.Value")});
            this.sqlDataSource1.SelectCommand = resources.GetString("sqlDataSource1.SelectCommand");
            // 
            // reportHeaderSection1
            // 
            this.reportHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.5D);
            this.reportHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox3});
            this.reportHeaderSection1.Name = "reportHeaderSection1";
            // 
            // textBox3
            // 
            this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(3.9339065551757812E-05D));
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.089961051940918D), Telerik.Reporting.Drawing.Unit.Inch(0.49996066093444824D));
            this.textBox3.StyleName = "Title";
            this.textBox3.Value = "Order Details";
            // 
            // SalesOrderDetails
            // 
            this.DataSource = this.sqlDataSource1;
            this.Filters.Add(new Telerik.Reporting.Filter("=Fields.SalesOrderID", Telerik.Reporting.FilterOperator.Equal, "=Parameters.SaledOrderID.Value"));
            group1.GroupFooter = this.labelsGroupFooter;
            group1.GroupHeader = this.labelsGroupHeader;
            group1.Name = "labelsGroup";
            this.Groups.AddRange(new Telerik.Reporting.Group[] {
            group1});
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.labelsGroupHeader,
            this.labelsGroupFooter,
            this.detail,
            this.reportHeaderSection1});
            this.Name = "SalesOrderDetails";
            this.PageSettings.Landscape = false;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Letter;
            reportParameter1.Name = "SaledOrderID";
            reportParameter1.Value = "=43860";
            this.ReportParameters.Add(reportParameter1);
            this.Style.BackgroundColor = System.Drawing.Color.White;
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Title")});
            styleRule2.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            styleRule2.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Point(0.5D);
            styleRule2.Style.Font.Bold = true;
            styleRule2.Style.Font.Name = "Segoe UI Light";
            styleRule2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(16D);
            styleRule2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0D);
            styleRule2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            styleRule3.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Caption")});
            styleRule3.Style.BorderColor.Default = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            styleRule3.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            styleRule3.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
            styleRule3.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(161)))), ((int)(((byte)(82)))));
            styleRule3.Style.Font.Name = "Segoe UI";
            styleRule3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            styleRule3.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Inch(0.079999998211860657D);
            styleRule3.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.10000000149011612D);
            styleRule3.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Inch(0.10000000149011612D);
            styleRule3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom;
            styleRule4.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Data")});
            styleRule4.Style.BorderColor.Default = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            styleRule4.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            styleRule4.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
            styleRule4.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            styleRule4.Style.Font.Name = "Segoe UI";
            styleRule4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            styleRule4.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Inch(0.079999998211860657D);
            styleRule4.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.10000000149011612D);
            styleRule4.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Inch(0.10000000149011612D);
            styleRule4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom;
            styleRule5.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Total")});
            styleRule5.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            styleRule5.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            styleRule5.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            styleRule5.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Point(0.5D);
            styleRule5.Style.Font.Bold = true;
            styleRule5.Style.Font.Name = "Segoe UI";
            styleRule5.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            styleRule5.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Inch(0.079999998211860657D);
            styleRule5.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.10000000149011612D);
            styleRule5.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Inch(0.10000000149011612D);
            styleRule5.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Inch(0D);
            styleRule5.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom;
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1,
            styleRule2,
            styleRule3,
            styleRule4,
            styleRule5});
            this.Width = Telerik.Reporting.Drawing.Unit.Inch(8.09000015258789D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private GroupHeaderSection labelsGroupHeader;
        private Telerik.Reporting.TextBox productNumberCaptionTextBox;
        private Telerik.Reporting.TextBox nameCaptionTextBox;
        private Telerik.Reporting.TextBox orderQtyCaptionTextBox;
        private Telerik.Reporting.TextBox unitPriceCaptionTextBox;
        private Telerik.Reporting.TextBox lineTotalCaptionTextBox;
        private GroupFooterSection labelsGroupFooter;
        private DetailSection detail;
        private Telerik.Reporting.TextBox productNumberDataTextBox;
        private Telerik.Reporting.TextBox nameDataTextBox;
        private Telerik.Reporting.TextBox orderQtyDataTextBox;
        private Telerik.Reporting.TextBox unitPriceDataTextBox;
        private Telerik.Reporting.TextBox lineTotalDataTextBox;
        private Telerik.Reporting.Panel panel1;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.TextBox textBox2;
        private SqlDataSource sqlDataSource1;
        private ReportHeaderSection reportHeaderSection1;
        private Reporting.TextBox textBox3;

    }
}