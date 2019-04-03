namespace Telerik.Reporting.Examples.CSharp
{
    using Data;

    partial class ReportCatalog
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
            Telerik.Reporting.NavigateToReportAction navigateToReportAction1 = new Telerik.Reporting.NavigateToReportAction();
            Telerik.Reporting.TypeReportSource typeReportSource1 = new Telerik.Reporting.TypeReportSource();
            Telerik.Reporting.NavigateToUrlAction navigateToUrlAction1 = new Telerik.Reporting.NavigateToUrlAction();
            Telerik.Reporting.NavigateToUrlAction navigateToUrlAction2 = new Telerik.Reporting.NavigateToUrlAction();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportCatalog));
            Telerik.Reporting.NavigateToUrlAction navigateToUrlAction3 = new Telerik.Reporting.NavigateToUrlAction();
            Telerik.Reporting.NavigateToUrlAction navigateToUrlAction4 = new Telerik.Reporting.NavigateToUrlAction();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule4 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule5 = new Telerik.Reporting.Drawing.StyleRule();
            this.detail = new Telerik.Reporting.DetailSection();
            this.crosstab1 = new Telerik.Reporting.Crosstab();
            this.panel5 = new Telerik.Reporting.Panel();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.textBox3 = new Telerik.Reporting.TextBox();
            this.objectDataSource1 = new Telerik.Reporting.ObjectDataSource();
            this.pageFooterSection1 = new Telerik.Reporting.PageFooterSection();
            this.textBox6 = new Telerik.Reporting.TextBox();
            this.textBox7 = new Telerik.Reporting.TextBox();
            this.textBox5 = new Telerik.Reporting.TextBox();
            this.shape1 = new Telerik.Reporting.Shape();
            this.pageHeaderSection1 = new Telerik.Reporting.PageHeaderSection();
            this.textBoxReportEmployee = new Telerik.Reporting.TextBox();
            this.shape3 = new Telerik.Reporting.Shape();
            this.textBox4 = new Telerik.Reporting.TextBox();
            this.textBox8 = new Telerik.Reporting.TextBox();
            this.textBox9 = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(5.334200382232666D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.crosstab1});
            this.detail.Name = "detail";
            // 
            // crosstab1
            // 
            this.crosstab1.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(6.380000114440918D)));
            this.crosstab1.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(4.1777081489562988D)));
            this.crosstab1.Body.SetCellContent(0, 0, this.panel5);
            tableGroup1.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.Index%3"));
            tableGroup1.Name = "ColumnIndex";
            this.crosstab1.ColumnGroups.Add(tableGroup1);
            this.crosstab1.DataSource = this.objectDataSource1;
            this.crosstab1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.panel5});
            this.crosstab1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.5027083158493042D), Telerik.Reporting.Drawing.Unit.Cm(0.25400015711784363D));
            this.crosstab1.Name = "crosstab1";
            tableGroup2.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.Index/3"));
            tableGroup2.Name = "RowIndex";
            this.crosstab1.RowGroups.Add(tableGroup2);
            this.crosstab1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.380000114440918D), Telerik.Reporting.Drawing.Unit.Cm(4.1777081489562988D));
            this.crosstab1.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.Index", Telerik.Reporting.SortDirection.Asc));
            // 
            // panel5
            // 
            this.panel5.Bindings.Add(new Telerik.Reporting.Binding("Visible", "=IIF(Fields.Name is null,False,True)"));
            this.panel5.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox1,
            this.textBox2,
            this.textBox3});
            this.panel5.Name = "panel5";
            this.panel5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.380000114440918D), Telerik.Reporting.Drawing.Unit.Cm(4.1777081489562988D));
            // 
            // textBox1
            // 
            this.textBox1.Anchoring = ((Telerik.Reporting.AnchoringStyles)(((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Left) 
            | Telerik.Reporting.AnchoringStyles.Right)));
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.1051244735717773D), Telerik.Reporting.Drawing.Unit.Cm(1.2700996398925781D));
            this.textBox1.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(43)))), ((int)(((byte)(10)))));
            this.textBox1.StyleName = "ExampleHeader";
            this.textBox1.Value = "=Fields.Name";
            // 
            // textBox2
            // 
            this.textBox2.CanGrow = true;
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(1.2701997756958008D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8472914695739746D), Telerik.Reporting.Drawing.Unit.Inch(0.60000008344650269D));
            this.textBox2.StyleName = "ExampleDescription";
            this.textBox2.Value = "=Fields.Description";
            // 
            // textBox3
            // 
            typeReportSource1.TypeName = "=Fields.AssemblyQualifiedName";
            navigateToReportAction1.ReportSource = typeReportSource1;
            this.textBox3.Action = navigateToReportAction1;
            this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(2.7941999435424805D));
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.5999999046325684D), Telerik.Reporting.Drawing.Unit.Inch(0.29440927505493164D));
            this.textBox3.StyleName = "Hyperlink";
            this.textBox3.Value = "View Report";
            // 
            // objectDataSource1
            // 
            this.objectDataSource1.DataMember = "GetReports";
            this.objectDataSource1.DataSource = typeof(Telerik.Reporting.Examples.CSharp.ReportManager);
            this.objectDataSource1.Name = "objectDataSource1";
            // 
            // pageFooterSection1
            // 
            navigateToUrlAction1.Url = "http://www.telerik.com/purchase";
            this.pageFooterSection1.Action = navigateToUrlAction1;
            this.pageFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(6.857999324798584D);
            this.pageFooterSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox6,
            this.textBox7,
            this.textBox5,
            this.shape1});
            this.pageFooterSection1.Name = "pageFooterSection1";
            // 
            // textBox6
            // 
            this.textBox6.Action = navigateToUrlAction1;
            this.textBox6.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.5027083158493042D), Telerik.Reporting.Drawing.Unit.Cm(0.6785462498664856D));
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.1051244735717773D), Telerik.Reporting.Drawing.Unit.Cm(0.9999995231628418D));
            this.textBox6.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(43)))), ((int)(((byte)(10)))));
            this.textBox6.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom;
            this.textBox6.StyleName = "ExampleHeader";
            this.textBox6.Value = "Get Greater Value!";
            // 
            // textBox7
            // 
            this.textBox7.Action = navigateToUrlAction1;
            this.textBox7.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(6.6080331802368164D), Telerik.Reporting.Drawing.Unit.Cm(0.6785462498664856D));
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(9.13996696472168D), Telerik.Reporting.Drawing.Unit.Cm(0.9999995231628418D));
            this.textBox7.Style.Font.Italic = true;
            this.textBox7.Style.Font.Name = "Segoe UI";
            this.textBox7.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.textBox7.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom;
            this.textBox7.Value = "Telerik Reporting is also available as part of \r\nTelerik DevCraft Complete with 1" +
    "4 other products";
            // 
            // textBox5
            // 
            navigateToUrlAction2.Url = "http://www.telerik.com/purchase";
            this.textBox5.Action = navigateToUrlAction2;
            this.textBox5.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.4200000762939453D), Telerik.Reporting.Drawing.Unit.Inch(0.299999862909317D));
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.3699995279312134D), Telerik.Reporting.Drawing.Unit.Inch(0.3608449399471283D));
            this.textBox5.Style.BackgroundImage.ImageData = ((System.Drawing.Image)(resources.GetObject("textBox5.Style.BackgroundImage.ImageData")));
            this.textBox5.Style.BackgroundImage.MimeType = "image/png";
            this.textBox5.Style.BackgroundImage.Repeat = Telerik.Reporting.Drawing.BackgroundRepeat.NoRepeat;
            this.textBox5.Style.Color = System.Drawing.Color.White;
            this.textBox5.Style.Font.Name = "Segoe UI";
            this.textBox5.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(14D);
            this.textBox5.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox5.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox5.Value = "Read more";
            // 
            // shape1
            // 
            this.shape1.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Left | Telerik.Reporting.AnchoringStyles.Right)));
            this.shape1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.1979166716337204D), Telerik.Reporting.Drawing.Unit.Inch(0.1000000610947609D));
            this.shape1.Name = "shape1";
            this.shape1.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.shape1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(7.46999979019165D), Telerik.Reporting.Drawing.Unit.Point(3.75D));
            this.shape1.Stretch = true;
            this.shape1.Style.BorderColor.Default = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(54)))), ((int)(((byte)(33)))));
            this.shape1.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.shape1.Style.BorderWidth.Bottom = Telerik.Reporting.Drawing.Unit.Point(0.5D);
            this.shape1.Style.BorderWidth.Top = Telerik.Reporting.Drawing.Unit.Point(1.5D);
            this.shape1.Style.Color = System.Drawing.Color.Transparent;
            // 
            // pageHeaderSection1
            // 
            this.pageHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(2.5398001670837402D);
            this.pageHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBoxReportEmployee,
            this.shape3,
            this.textBox4,
            this.textBox8,
            this.textBox9});
            this.pageHeaderSection1.Name = "pageHeaderSection1";
            // 
            // textBoxReportEmployee
            // 
            this.textBoxReportEmployee.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D), Telerik.Reporting.Drawing.Unit.Inch(0.41500845551490784D));
            this.textBoxReportEmployee.Name = "textBoxReportEmployee";
            this.textBoxReportEmployee.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(5D), Telerik.Reporting.Drawing.Unit.Inch(0.40000009536743164D));
            this.textBoxReportEmployee.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Top;
            this.textBoxReportEmployee.StyleName = "Header";
            this.textBoxReportEmployee.Value = "Reporting Demos";
            // 
            // shape3
            // 
            this.shape3.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Left | Telerik.Reporting.AnchoringStyles.Right)));
            this.shape3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Inch(0.8399999737739563D));
            this.shape3.Name = "shape3";
            this.shape3.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.shape3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(7.46999979019165D), Telerik.Reporting.Drawing.Unit.Point(2D));
            this.shape3.Stretch = true;
            this.shape3.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.shape3.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            this.shape3.Style.BorderWidth.Bottom = Telerik.Reporting.Drawing.Unit.Point(0.5D);
            this.shape3.Style.BorderWidth.Top = Telerik.Reporting.Drawing.Unit.Point(1.5D);
            this.shape3.Style.Color = System.Drawing.Color.Transparent;
            // 
            // textBox4
            // 
            navigateToUrlAction3.Url = "http://www.telerik.com/support/reporting";
            this.textBox4.Action = navigateToUrlAction3;
            this.textBox4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(17.120002746582031D), Telerik.Reporting.Drawing.Unit.Cm(0.63555306196212769D));
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.3617973327636719D), Telerik.Reporting.Drawing.Unit.Cm(0.41836854815483093D));
            this.textBox4.Style.BackgroundColor = System.Drawing.Color.Transparent;
            this.textBox4.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(43)))), ((int)(((byte)(10)))));
            this.textBox4.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox4.StyleName = "Hyperlink";
            this.textBox4.Value = "SUPPORT »";
            // 
            // textBox8
            // 
            navigateToUrlAction4.Url = "http://tv.telerik.com/products/reporting";
            this.textBox8.Action = navigateToUrlAction4;
            this.textBox8.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(17.120002746582031D), Telerik.Reporting.Drawing.Unit.Cm(1.0541214942932129D));
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.3617961406707764D), Telerik.Reporting.Drawing.Unit.Cm(0.41999998688697815D));
            this.textBox8.Style.BackgroundColor = System.Drawing.Color.Transparent;
            this.textBox8.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(43)))), ((int)(((byte)(10)))));
            this.textBox8.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox8.StyleName = "Hyperlink";
            this.textBox8.Value = "VIDEOS »";
            // 
            // textBox9
            // 
            this.textBox9.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D), Telerik.Reporting.Drawing.Unit.Inch(0.015008449554443359D));
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(5D), Telerik.Reporting.Drawing.Unit.Inch(0.39992132782936096D));
            this.textBox9.StyleName = "Header";
            this.textBox9.Value = "Telerik";
            // 
            // ReportCatalog
            // 
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.detail,
            this.pageFooterSection1,
            this.pageHeaderSection1});
            this.Name = "ReportCatalog";
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Inch(0.30000001192092896D), Telerik.Reporting.Drawing.Unit.Inch(0.40000000596046448D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
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
            styleRule2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            styleRule3.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Hyperlink")});
            styleRule3.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(147)))), ((int)(((byte)(30)))));
            styleRule3.Style.Color = System.Drawing.Color.White;
            styleRule3.Style.Font.Name = "Segoe UI";
            styleRule3.Style.Font.Underline = false;
            styleRule3.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            styleRule3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            styleRule4.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("ExampleHeader")});
            styleRule4.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(89)))), ((int)(((byte)(61)))));
            styleRule4.Style.Font.Bold = true;
            styleRule4.Style.Font.Name = "Segoe UI Light";
            styleRule4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(14D);
            styleRule4.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0D);
            styleRule5.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("ExampleDescription")});
            styleRule5.Style.Font.Name = "Segoe UI";
            styleRule5.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1,
            styleRule2,
            styleRule3,
            styleRule4,
            styleRule5});
            this.Width = Telerik.Reporting.Drawing.Unit.Cm(19.983798980712891D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.DetailSection detail;
        private ObjectDataSource objectDataSource1;
        private PageFooterSection pageFooterSection1;
        private TextBox textBox1;
        private PageHeaderSection pageHeaderSection1;
        private TextBox textBox8;
        private TextBox textBox4;
        private TextBox textBox6;
        private TextBox textBox7;
        private Crosstab crosstab1;
        private Panel panel5;
        private TextBox textBoxReportEmployee;
        private Shape shape3;
        private TextBox textBox5;
        private Shape shape1;
        private TextBox textBox9;
        private TextBox textBox2;
        private TextBox textBox3;
    }
}