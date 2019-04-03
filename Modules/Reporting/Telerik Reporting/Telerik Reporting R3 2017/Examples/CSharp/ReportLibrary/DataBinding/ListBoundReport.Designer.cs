namespace Telerik.Reporting.Examples.CSharp
{
    partial class ListBoundReport
    {

        #region Component Designer generated code
        /// <summary>
        /// Required method for Telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.Reporting.TableGroup tableGroup1 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup2 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup3 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.Drawing.FormattingRule formattingRule1 = new Telerik.Reporting.Drawing.FormattingRule();
            Telerik.Reporting.Drawing.FormattingRule formattingRule2 = new Telerik.Reporting.Drawing.FormattingRule();
            Telerik.Reporting.Drawing.FormattingRule formattingRule3 = new Telerik.Reporting.Drawing.FormattingRule();
            Telerik.Reporting.Drawing.FormattingRule formattingRule4 = new Telerik.Reporting.Drawing.FormattingRule();
            Telerik.Reporting.Drawing.FormattingRule formattingRule5 = new Telerik.Reporting.Drawing.FormattingRule();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            this.detail = new Telerik.Reporting.DetailSection();
            this.list1 = new Telerik.Reporting.List();
            this.panel1 = new Telerik.Reporting.Panel();
            this.txtModel = new Telerik.Reporting.TextBox();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.txtProductNameLabel = new Telerik.Reporting.TextBox();
            this.txtYearLabel = new Telerik.Reporting.TextBox();
            this.txtManufacturer = new Telerik.Reporting.TextBox();
            this.txtYear = new Telerik.Reporting.TextBox();
            this.panelColorsInStock = new Telerik.Reporting.Panel();
            this.shape1 = new Telerik.Reporting.Shape();
            this.shape2 = new Telerik.Reporting.Shape();
            this.shape3 = new Telerik.Reporting.Shape();
            this.shape4 = new Telerik.Reporting.Shape();
            this.txtColorsInStock = new Telerik.Reporting.TextBox();
            this.pictureBox1 = new Telerik.Reporting.PictureBox();
            this.objectDataSource1 = new Telerik.Reporting.ObjectDataSource();
            this.reportHeaderSection1 = new Telerik.Reporting.ReportHeaderSection();
            this.textBox5 = new Telerik.Reporting.TextBox();
            this.textBox6 = new Telerik.Reporting.TextBox();
            this.shape5 = new Telerik.Reporting.Shape();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Inch(1.9000002145767212D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.list1});
            this.detail.Name = "detail";
            // 
            // list1
            // 
            this.list1.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(20.573799133300781D)));
            this.list1.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Inch(1.8000000715255737D)));
            this.list1.Body.SetCellContent(0, 0, this.panel1);
            tableGroup1.Name = "ColumnGroup1";
            this.list1.ColumnGroups.Add(tableGroup1);
            this.list1.DataSource = this.objectDataSource1;
            this.list1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.panel1});
            this.list1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010002215276472271D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.list1.Name = "list1";
            tableGroup3.Name = "Group1";
            tableGroup2.ChildGroups.Add(tableGroup3);
            tableGroup2.Groupings.Add(new Telerik.Reporting.Grouping(""));
            tableGroup2.Name = "RowGroup1";
            this.list1.RowGroups.Add(tableGroup2);
            this.list1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(20.573799133300781D), Telerik.Reporting.Drawing.Unit.Inch(1.8000000715255737D));
            // 
            // panel1
            // 
            formattingRule1.Filters.Add(new Telerik.Reporting.Filter("=RowNumber() % 2", Telerik.Reporting.FilterOperator.Equal, "=0"));
            formattingRule1.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(231)))), ((int)(((byte)(203)))));
            this.panel1.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule1});
            this.panel1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.txtModel,
            this.textBox1,
            this.txtProductNameLabel,
            this.txtYearLabel,
            this.txtManufacturer,
            this.txtYear,
            this.panelColorsInStock,
            this.txtColorsInStock,
            this.pictureBox1});
            this.panel1.Name = "panel1";
            this.panel1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(20.573799133300781D), Telerik.Reporting.Drawing.Unit.Cm(4.5720000267028809D));
            this.panel1.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(241)))), ((int)(((byte)(235)))));
            this.panel1.Style.BorderColor.Default = System.Drawing.Color.White;
            this.panel1.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.panel1.Style.BorderWidth.Bottom = Telerik.Reporting.Drawing.Unit.Point(13D);
            // 
            // txtModel
            // 
            this.txtModel.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.7999212741851807D), Telerik.Reporting.Drawing.Unit.Inch(0.51771211624145508D));
            this.txtModel.Name = "txtModel";
            this.txtModel.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1000001430511475D), Telerik.Reporting.Drawing.Unit.Inch(0.24999983608722687D));
            this.txtModel.Value = "=Fields.Model";
            // 
            // textBox1
            // 
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.15625D), Telerik.Reporting.Drawing.Unit.Inch(0.1145833358168602D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.3437106609344482D), Telerik.Reporting.Drawing.Unit.Inch(0.25D));
            this.textBox1.Value = "Manufacturer:";
            // 
            // txtProductNameLabel
            // 
            this.txtProductNameLabel.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.15625D), Telerik.Reporting.Drawing.Unit.Inch(0.51771211624145508D));
            this.txtProductNameLabel.Name = "txtProductNameLabel";
            this.txtProductNameLabel.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.3437106609344482D), Telerik.Reporting.Drawing.Unit.Inch(0.25D));
            this.txtProductNameLabel.Value = "Model:";
            // 
            // txtYearLabel
            // 
            this.txtYearLabel.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.15625D), Telerik.Reporting.Drawing.Unit.Inch(0.93884181976318359D));
            this.txtYearLabel.Name = "txtYearLabel";
            this.txtYearLabel.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.3437106609344482D), Telerik.Reporting.Drawing.Unit.Inch(0.25D));
            this.txtYearLabel.Style.Font.Bold = false;
            this.txtYearLabel.Value = "Year:";
            // 
            // txtManufacturer
            // 
            this.txtManufacturer.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.7999606132507324D), Telerik.Reporting.Drawing.Unit.Inch(0.1145833358168602D));
            this.txtManufacturer.Name = "txtManufacturer";
            this.txtManufacturer.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1000001430511475D), Telerik.Reporting.Drawing.Unit.Inch(0.25D));
            this.txtManufacturer.Value = "=Fields.Manufacturer";
            // 
            // txtYear
            // 
            this.txtYear.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.7999212741851807D), Telerik.Reporting.Drawing.Unit.Inch(0.93884181976318359D));
            this.txtYear.Name = "txtYear";
            this.txtYear.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1000001430511475D), Telerik.Reporting.Drawing.Unit.Inch(0.25D));
            this.txtYear.Value = "=Fields.Year";
            // 
            // panelColorsInStock
            // 
            this.panelColorsInStock.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.shape1,
            this.shape2,
            this.shape3,
            this.shape4});
            this.panelColorsInStock.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.7999212741851807D), Telerik.Reporting.Drawing.Unit.Inch(1.309999942779541D));
            this.panelColorsInStock.Name = "panelColorsInStock";
            this.panelColorsInStock.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2089647054672241D), Telerik.Reporting.Drawing.Unit.Inch(0.26115801930427551D));
            // 
            // shape1
            // 
            formattingRule2.Filters.Add(new Telerik.Reporting.Filter("=Fields.AvailableColor.Contains(\'Red\')", Telerik.Reporting.FilterOperator.NotEqual, "=True"));
            formattingRule2.Style.Visible = false;
            this.shape1.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule2});
            this.shape1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.34405517578125D), Telerik.Reporting.Drawing.Unit.Inch(0.05000000074505806D));
            this.shape1.Name = "shape1";
            this.shape1.ShapeType = new Telerik.Reporting.Drawing.Shapes.EllipseShape(0D);
            this.shape1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D));
            this.shape1.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(46)))), ((int)(((byte)(10)))));
            this.shape1.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Point(1D);
            this.shape1.Style.Color = System.Drawing.Color.Transparent;
            this.shape1.Style.LineStyle = Telerik.Reporting.Drawing.LineStyle.Solid;
            this.shape1.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(1D);
            // 
            // shape2
            // 
            formattingRule3.Filters.Add(new Telerik.Reporting.Filter("= Fields.AvailableColor.Contains(\"Orange\")", Telerik.Reporting.FilterOperator.NotEqual, "= True"));
            formattingRule3.Style.Visible = false;
            this.shape2.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule3});
            this.shape2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.949999988079071D), Telerik.Reporting.Drawing.Unit.Inch(0.05000000074505806D));
            this.shape2.Name = "shape2";
            this.shape2.ShapeType = new Telerik.Reporting.Drawing.Shapes.EllipseShape(0D);
            this.shape2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D));
            this.shape2.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(147)))), ((int)(((byte)(30)))));
            this.shape2.Style.Color = System.Drawing.Color.Transparent;
            // 
            // shape3
            // 
            formattingRule4.Filters.Add(new Telerik.Reporting.Filter("= Fields.AvailableColor.Contains(\"Black\")", Telerik.Reporting.FilterOperator.NotEqual, "= True"));
            formattingRule4.Style.Visible = false;
            this.shape3.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule4});
            this.shape3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.9418537198798731E-05D), Telerik.Reporting.Drawing.Unit.Inch(0.049999874085187912D));
            this.shape3.Name = "shape3";
            this.shape3.ShapeType = new Telerik.Reporting.Drawing.Shapes.EllipseShape(0D);
            this.shape3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D));
            this.shape3.Style.BackgroundColor = System.Drawing.Color.Black;
            this.shape3.Style.Color = System.Drawing.Color.Transparent;
            // 
            // shape4
            // 
            formattingRule5.Filters.Add(new Telerik.Reporting.Filter("= Fields.AvailableColor.Contains(\"White\")", Telerik.Reporting.FilterOperator.NotEqual, "= True"));
            formattingRule5.Style.Visible = false;
            this.shape4.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule5});
            this.shape4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.64999997615814209D), Telerik.Reporting.Drawing.Unit.Inch(0.05000000074505806D));
            this.shape4.Name = "shape4";
            this.shape4.ShapeType = new Telerik.Reporting.Drawing.Shapes.EllipseShape(0D);
            this.shape4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D));
            this.shape4.Style.BackgroundColor = System.Drawing.Color.White;
            this.shape4.Style.Color = System.Drawing.Color.Transparent;
            // 
            // txtColorsInStock
            // 
            this.txtColorsInStock.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.15625D), Telerik.Reporting.Drawing.Unit.Inch(1.3388419151306152D));
            this.txtColorsInStock.Name = "txtColorsInStock";
            this.txtColorsInStock.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.3437106609344482D), Telerik.Reporting.Drawing.Unit.Inch(0.25D));
            this.txtColorsInStock.Value = "Colors in Stock:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.pictureBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.4999604225158691D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.pictureBox1.MimeType = "";
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.5999212265014648D), Telerik.Reporting.Drawing.Unit.Cm(4.5718994140625D));
            this.pictureBox1.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.ScaleProportional;
            this.pictureBox1.Value = "=Fields.ImageUrl";
            // 
            // objectDataSource1
            // 
            this.objectDataSource1.DataSource = typeof(Telerik.Reporting.Examples.CSharp.Cars);
            this.objectDataSource1.Name = "objectDataSource1";
            // 
            // reportHeaderSection1
            // 
            this.reportHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(1.1999999284744263D);
            this.reportHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox5,
            this.textBox6,
            this.shape5});
            this.reportHeaderSection1.Name = "reportHeaderSection1";
            // 
            // textBox5
            // 
            this.textBox5.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(3.9339065551757812E-05D));
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.0999603271484375D), Telerik.Reporting.Drawing.Unit.Inch(0.60000002384185791D));
            this.textBox5.Style.BorderColor.Default = System.Drawing.Color.Black;
            this.textBox5.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(34D);
            this.textBox5.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Top;
            this.textBox5.Value = "Dealer Inc.";
            // 
            // textBox6
            // 
            this.textBox6.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.9418537198798731E-05D), Telerik.Reporting.Drawing.Unit.Inch(0.800000011920929D));
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.0999212265014648D), Telerik.Reporting.Drawing.Unit.Inch(0.31657701730728149D));
            this.textBox6.Style.BorderColor.Default = System.Drawing.Color.Black;
            this.textBox6.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox6.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(16D);
            this.textBox6.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom;
            this.textBox6.Value = "Performance cars in stock";
            // 
            // shape5
            // 
            this.shape5.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Left | Telerik.Reporting.AnchoringStyles.Right)));
            this.shape5.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.60011798143386841D));
            this.shape5.Name = "shape5";
            this.shape5.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.shape5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.0999603271484375D), Telerik.Reporting.Drawing.Unit.Point(3.75D));
            this.shape5.Stretch = true;
            this.shape5.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.shape5.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            this.shape5.Style.BorderWidth.Top = Telerik.Reporting.Drawing.Unit.Point(3D);
            this.shape5.Style.Color = System.Drawing.Color.Transparent;
            // 
            // ListBoundReport
            // 
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.detail,
            this.reportHeaderSection1});
            this.Name = "ListBoundReport";
            this.PageSettings.Landscape = false;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Inch(0.30000001192092896D), Telerik.Reporting.Drawing.Unit.Inch(0.40000000596046448D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Letter;
            this.Style.BackgroundColor = System.Drawing.Color.White;
            this.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Header")});
            styleRule1.Style.Font.Bold = true;
            styleRule1.Style.Font.Name = "Segoe UI Light";
            styleRule1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(25D);
            styleRule2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule2.Style.Font.Name = "Segoe UI Light";
            styleRule2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(13D);
            styleRule2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule2.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1,
            styleRule2});
            this.Width = Telerik.Reporting.Drawing.Unit.Inch(8.09999942779541D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private DetailSection detail;
        private TextBox txtModel;
        private TextBox txtYear;
        private TextBox txtProductNameLabel;
        private TextBox txtYearLabel;
        private TextBox textBox1;
        private TextBox txtManufacturer;
        private PictureBox pictureBox1;
        private Panel panelColorsInStock;
        private Shape shape1;
        private Shape shape2;
        private Shape shape3;
        private Shape shape4;
        private List list1;
        private Panel panel1;
        private ObjectDataSource objectDataSource1;
        private TextBox txtColorsInStock;
        private ReportHeaderSection reportHeaderSection1;
        private TextBox textBox5;
        private TextBox textBox6;
        private Shape shape5;


    }
}
