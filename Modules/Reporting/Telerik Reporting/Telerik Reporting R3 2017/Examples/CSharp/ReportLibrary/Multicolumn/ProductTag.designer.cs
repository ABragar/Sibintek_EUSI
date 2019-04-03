namespace Telerik.Reporting.Examples.CSharp
{
    using System.Data.SqlClient;
    using Telerik.Reporting.Barcodes;

    partial class ProductTagReport
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for Telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.Reporting.Barcodes.EAN128Encoder eaN128Encoder1 = new Telerik.Reporting.Barcodes.EAN128Encoder();
            Telerik.Reporting.Drawing.FormattingRule formattingRule1 = new Telerik.Reporting.Drawing.FormattingRule();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProductTagReport));
            Telerik.Reporting.Drawing.FormattingRule formattingRule2 = new Telerik.Reporting.Drawing.FormattingRule();
            Telerik.Reporting.Drawing.FormattingRule formattingRule3 = new Telerik.Reporting.Drawing.FormattingRule();
            Telerik.Reporting.Drawing.FormattingRule formattingRule4 = new Telerik.Reporting.Drawing.FormattingRule();
            Telerik.Reporting.Drawing.FormattingRule formattingRule5 = new Telerik.Reporting.Drawing.FormattingRule();
            Telerik.Reporting.Drawing.FormattingRule formattingRule6 = new Telerik.Reporting.Drawing.FormattingRule();
            Telerik.Reporting.Drawing.FormattingRule formattingRule7 = new Telerik.Reporting.Drawing.FormattingRule();
            Telerik.Reporting.Drawing.FormattingRule formattingRule8 = new Telerik.Reporting.Drawing.FormattingRule();
            Telerik.Reporting.Drawing.FormattingRule formattingRule9 = new Telerik.Reporting.Drawing.FormattingRule();
            Telerik.Reporting.Drawing.FormattingRule formattingRule10 = new Telerik.Reporting.Drawing.FormattingRule();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            this.detail = new Telerik.Reporting.DetailSection();
            this.panel1 = new Telerik.Reporting.Panel();
            this.txtName = new Telerik.Reporting.TextBox();
            this.pictureBox1 = new Telerik.Reporting.PictureBox();
            this.barcode1 = new Telerik.Reporting.Barcode();
            this.textBox5 = new Telerik.Reporting.TextBox();
            this.textBox6 = new Telerik.Reporting.TextBox();
            this.panel2 = new Telerik.Reporting.Panel();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.textBox4 = new Telerik.Reporting.TextBox();
            this.textBox10 = new Telerik.Reporting.TextBox();
            this.textBox3 = new Telerik.Reporting.TextBox();
            this.textBox7 = new Telerik.Reporting.TextBox();
            this.textBox8 = new Telerik.Reporting.TextBox();
            this.textBox9 = new Telerik.Reporting.TextBox();
            this.textBox11 = new Telerik.Reporting.TextBox();
            this.textBox12 = new Telerik.Reporting.TextBox();
            this.txtTitle = new Telerik.Reporting.TextBox();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.shape1 = new Telerik.Reporting.Shape();
            this.textBox14 = new Telerik.Reporting.TextBox();
            this.panel3 = new Telerik.Reporting.Panel();
            this.shape2 = new Telerik.Reporting.Shape();
            this.sqlDataSource1 = new Telerik.Reporting.SqlDataSource();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(11.430000305175781D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.panel1});
            this.detail.Name = "detail";
            // 
            // panel1
            // 
            this.panel1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.txtName,
            this.pictureBox1,
            this.barcode1,
            this.textBox5,
            this.textBox6,
            this.panel2});
            this.panel1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.013781229965388775D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.panel1.Name = "panel1";
            this.panel1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(60.101001739501953D), Telerik.Reporting.Drawing.Unit.Mm(111.76000213623047D));
            this.panel1.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(241)))), ((int)(((byte)(235)))));
            this.panel1.Style.BackgroundImage.Repeat = Telerik.Reporting.Drawing.BackgroundRepeat.NoRepeat;
            this.panel1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.panel1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Inch(0D);
            // 
            // txtName
            // 
            this.txtName.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.38109993934631348D), Telerik.Reporting.Drawing.Unit.Mm(0D));
            this.txtName.Name = "txtName";
            this.txtName.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.2188005447387695D), Telerik.Reporting.Drawing.Unit.Cm(1.5239999294281006D));
            this.txtName.Style.Color = System.Drawing.Color.Black;
            this.txtName.Style.Font.Bold = true;
            this.txtName.Style.Font.Name = "Segoe UI";
            this.txtName.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(16D);
            this.txtName.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.txtName.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom;
            this.txtName.TextWrap = true;
            this.txtName.Value = "=Fields.ProductName";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.9368867874145508E-05D), Telerik.Reporting.Drawing.Unit.Cm(1.5241997241973877D));
            this.pictureBox1.MimeType = "";
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.0098996162414551D), Telerik.Reporting.Drawing.Unit.Cm(2.5398004055023193D));
            this.pictureBox1.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.ScaleProportional;
            this.pictureBox1.Style.BackgroundColor = System.Drawing.Color.White;
            this.pictureBox1.Style.BorderColor.Default = System.Drawing.Color.Transparent;
            this.pictureBox1.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.pictureBox1.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Inch(0.10000000149011612D);
            this.pictureBox1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.10000000149011612D);
            this.pictureBox1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Inch(0.10000000149011612D);
            this.pictureBox1.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Inch(0.10000000149011612D);
            this.pictureBox1.Style.Visible = true;
            this.pictureBox1.Value = "=Fields.LargePhoto";
            // 
            // barcode1
            // 
            this.barcode1.Angle = 0D;
            this.barcode1.Checksum = true;
            eaN128Encoder1.ShowText = false;
            this.barcode1.Encoder = eaN128Encoder1;
            this.barcode1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(11.761085510253906D), Telerik.Reporting.Drawing.Unit.Mm(88.899993896484375D));
            this.barcode1.Name = "barcode1";
            this.barcode1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(44.2379150390625D), Telerik.Reporting.Drawing.Unit.Mm(20.320003509521484D));
            this.barcode1.Stretch = true;
            this.barcode1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.barcode1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.barcode1.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom;
            this.barcode1.Value = "=Fields.ProductNumber";
            // 
            // textBox5
            // 
            this.textBox5.Angle = 90D;
            this.textBox5.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(7.759087085723877D), Telerik.Reporting.Drawing.Unit.Mm(88.899993896484375D));
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(4D), Telerik.Reporting.Drawing.Unit.Mm(20.320003509521484D));
            this.textBox5.Style.Color = System.Drawing.Color.Black;
            this.textBox5.Style.Font.Italic = false;
            this.textBox5.Style.Font.Name = "Segoe UI";
            this.textBox5.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox5.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.textBox5.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.textBox5.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.textBox5.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox5.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom;
            this.textBox5.Value = "Product No.";
            // 
            // textBox6
            // 
            this.textBox6.Angle = 90D;
            this.textBox6.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(3.8109993934631348D), Telerik.Reporting.Drawing.Unit.Mm(88.899993896484375D));
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(3.9460890293121338D), Telerik.Reporting.Drawing.Unit.Mm(20.320003509521484D));
            this.textBox6.Style.Color = System.Drawing.Color.Black;
            this.textBox6.Style.Font.Bold = true;
            this.textBox6.Style.Font.Name = "Segoe UI";
            this.textBox6.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox6.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.textBox6.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.textBox6.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.textBox6.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox6.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom;
            this.textBox6.Value = "=Fields.ProductNumber";
            // 
            // panel2
            // 
            this.panel2.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox2,
            this.textBox4,
            this.textBox10,
            this.textBox3,
            this.textBox7,
            this.textBox8,
            this.textBox9,
            this.textBox11,
            this.textBox12,
            this.txtTitle,
            this.textBox1,
            this.shape1,
            this.textBox14,
            this.panel3});
            this.panel2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.9378803194267675E-05D), Telerik.Reporting.Drawing.Unit.Inch(1.6000789403915405D));
            this.panel2.Name = "panel2";
            this.panel2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.0100002288818359D), Telerik.Reporting.Drawing.Unit.Inch(1.8998422622680664D));
            // 
            // textBox2
            // 
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D), Telerik.Reporting.Drawing.Unit.Inch(0.099921226501464844D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.9335449934005737D), Telerik.Reporting.Drawing.Unit.Cm(0.50264549255371094D));
            this.textBox2.Style.Font.Bold = false;
            this.textBox2.Style.Font.Name = "Segoe UI";
            this.textBox2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox2.Value = "LIST PRICE:";
            // 
            // textBox4
            // 
            this.textBox4.Format = "{0:C2}";
            this.textBox4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(2.5318536758422852D), Telerik.Reporting.Drawing.Unit.Cm(0.0027453103102743626D));
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.0679464340209961D), Telerik.Reporting.Drawing.Unit.Cm(0.75905394554138184D));
            this.textBox4.Style.Color = System.Drawing.Color.Black;
            this.textBox4.Style.Font.Bold = true;
            this.textBox4.Style.Font.Name = "Segoe UI";
            this.textBox4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(16D);
            this.textBox4.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox4.Value = "=Fields.ListPrice";
            // 
            // textBox10
            // 
            this.textBox10.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D), Telerik.Reporting.Drawing.Unit.Cm(1.7778000831604004D));
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.9335449934005737D), Telerik.Reporting.Drawing.Unit.Inch(0.23999999463558197D));
            this.textBox10.Style.Font.Bold = false;
            this.textBox10.Style.Font.Name = "Segoe UI";
            this.textBox10.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox10.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox10.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox10.Value = "COLOR:";
            // 
            // textBox3
            // 
            this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D), Telerik.Reporting.Drawing.Unit.Cm(2.3876004219055176D));
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.9335449934005737D), Telerik.Reporting.Drawing.Unit.Cm(0.5D));
            this.textBox3.Style.Font.Bold = false;
            this.textBox3.Style.Font.Name = "Segoe UI";
            this.textBox3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox3.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox3.Value = "SIZE:";
            // 
            // textBox7
            // 
            this.textBox7.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(2.5298001766204834D), Telerik.Reporting.Drawing.Unit.Cm(2.3876004219055176D));
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.0983283519744873D), Telerik.Reporting.Drawing.Unit.Cm(0.5D));
            this.textBox7.Style.Font.Name = "Segoe UI";
            this.textBox7.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox7.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox7.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox7.Value = "=IsNull(Fields.Size, \'N/A\') + \" \" + IsNull(Fields.SizeUnitMeasureCode, \'\')";
            // 
            // textBox8
            // 
            this.textBox8.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.14999997615814209D), Telerik.Reporting.Drawing.Unit.Cm(2.887800931930542D));
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.9335449934005737D), Telerik.Reporting.Drawing.Unit.Cm(0.5D));
            this.textBox8.Style.Font.Bold = false;
            this.textBox8.Style.Font.Name = "Segoe UI";
            this.textBox8.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox8.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox8.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox8.Value = "WEIGHT:";
            // 
            // textBox9
            // 
            this.textBox9.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(2.5318536758422852D), Telerik.Reporting.Drawing.Unit.Cm(2.887800931930542D));
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.0962750911712646D), Telerik.Reporting.Drawing.Unit.Cm(0.5D));
            this.textBox9.Style.Font.Name = "Segoe UI";
            this.textBox9.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox9.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox9.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox9.Value = "=IsNull(Fields.Weight, \'N/A\') + \" \" + IsNull(Fields.WeightUnitMeasureCode, \'\')";
            // 
            // textBox11
            // 
            this.textBox11.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D), Telerik.Reporting.Drawing.Unit.Cm(3.75D));
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.9335449934005737D), Telerik.Reporting.Drawing.Unit.Cm(0.5D));
            this.textBox11.Style.Font.Bold = false;
            this.textBox11.Style.Font.Name = "Segoe UI";
            this.textBox11.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox11.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox11.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox11.Value = "VENDOR:";
            // 
            // textBox12
            // 
            this.textBox12.CanGrow = false;
            this.textBox12.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(2.5318536758422852D), Telerik.Reporting.Drawing.Unit.Cm(3.7499995231628418D));
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.0699999332427979D), Telerik.Reporting.Drawing.Unit.Cm(0.5D));
            this.textBox12.Style.Font.Name = "Segoe UI";
            this.textBox12.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox12.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox12.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox12.Value = "=Fields.Vendor";
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(2.5318539142608643D), Telerik.Reporting.Drawing.Unit.Cm(1.0157995223999023D));
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.0999999046325684D), Telerik.Reporting.Drawing.Unit.Cm(0.43683755397796631D));
            this.txtTitle.Style.Color = System.Drawing.Color.Black;
            this.txtTitle.Style.Font.Name = "Segoe UI";
            this.txtTitle.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.txtTitle.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.txtTitle.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.txtTitle.Value = "=Fields.ProductSubName";
            // 
            // textBox1
            // 
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D), Telerik.Reporting.Drawing.Unit.Inch(0.39992108941078186D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.76123809814453125D), Telerik.Reporting.Drawing.Unit.Inch(0.20000012218952179D));
            this.textBox1.Style.Font.Name = "Segoe UI";
            this.textBox1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox1.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox1.Value = "CATEGORY:";
            // 
            // shape1
            // 
            this.shape1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.14999997615814209D), Telerik.Reporting.Drawing.Unit.Inch(1.7899999618530273D));
            this.shape1.Name = "shape1";
            this.shape1.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.shape1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.0546457767486572D), Telerik.Reporting.Drawing.Unit.Inch(0.069053970277309418D));
            this.shape1.Style.LineStyle = Telerik.Reporting.Drawing.LineStyle.Dotted;
            // 
            // textBox14
            // 
            this.textBox14.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.2861794233322144D), Telerik.Reporting.Drawing.Unit.Inch(0.70000046491622925D));
            this.textBox14.Name = "textBox14";
            this.textBox14.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.91846638917922974D), Telerik.Reporting.Drawing.Unit.Inch(0.23992092907428742D));
            this.textBox14.Style.Font.Name = "Segoe UI";
            this.textBox14.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox14.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox14.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox14.Value = "=Fields.Color";
            // 
            // panel3
            // 
            formattingRule1.Filters.Add(new Telerik.Reporting.Filter("=ISNULL(Fields.Color, \'\')", Telerik.Reporting.FilterOperator.Equal, "Multi"));
            formattingRule1.Style.BackgroundImage.ImageData = ((System.Drawing.Image)(resources.GetObject("resource.ImageData")));
            formattingRule1.Style.BackgroundImage.MimeType = "image/png";
            formattingRule1.Style.BackgroundImage.Repeat = Telerik.Reporting.Drawing.BackgroundRepeat.NoRepeat;
            formattingRule2.Filters.Add(new Telerik.Reporting.Filter("=ISNULL(Fields.Color, \'\')", Telerik.Reporting.FilterOperator.Equal, "Silver/Black"));
            formattingRule2.Style.BackgroundImage.ImageData = ((System.Drawing.Image)(resources.GetObject("resource.ImageData1")));
            formattingRule2.Style.BackgroundImage.MimeType = "image/png";
            formattingRule2.Style.BackgroundImage.Repeat = Telerik.Reporting.Drawing.BackgroundRepeat.NoRepeat;
            this.panel3.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule1,
            formattingRule2});
            this.panel3.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.shape2});
            this.panel3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.99598425626754761D), Telerik.Reporting.Drawing.Unit.Inch(0.69992130994796753D));
            this.panel3.Name = "panel3";
            this.panel3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.23957991600036621D), Telerik.Reporting.Drawing.Unit.Inch(0.24007908999919891D));
            // 
            // shape2
            // 
            this.shape2.Bindings.Add(new Telerik.Reporting.Binding("Visible", "=(ISNULL(Fields.Color,\'Multi\')<>\'Multi\')"));
            formattingRule3.Filters.Add(new Telerik.Reporting.Filter("=ISNULL(Fields.Color, \'\')", Telerik.Reporting.FilterOperator.Equal, "Red"));
            formattingRule3.StopIfTrue = true;
            formattingRule3.Style.BackgroundColor = System.Drawing.Color.Red;
            formattingRule4.Filters.Add(new Telerik.Reporting.Filter("=ISNULL(Fields.Color, \'\')", Telerik.Reporting.FilterOperator.Equal, "Blue"));
            formattingRule4.StopIfTrue = true;
            formattingRule4.Style.BackgroundColor = System.Drawing.Color.Blue;
            formattingRule5.Filters.Add(new Telerik.Reporting.Filter("=ISNULL(Fields.Color, \'\')", Telerik.Reporting.FilterOperator.Equal, "Black"));
            formattingRule5.StopIfTrue = true;
            formattingRule5.Style.BackgroundColor = System.Drawing.Color.Black;
            formattingRule6.Filters.Add(new Telerik.Reporting.Filter("=ISNULL(Fields.Color, \'\')", Telerik.Reporting.FilterOperator.Equal, "Silver"));
            formattingRule6.StopIfTrue = true;
            formattingRule6.Style.BackgroundColor = System.Drawing.Color.Silver;
            formattingRule7.Filters.Add(new Telerik.Reporting.Filter("=ISNULL(Fields.Color, \'\')", Telerik.Reporting.FilterOperator.Equal, "Yellow"));
            formattingRule7.StopIfTrue = true;
            formattingRule7.Style.BackgroundColor = System.Drawing.Color.Yellow;
            formattingRule8.Filters.Add(new Telerik.Reporting.Filter("=ISNULL(Fields.Color, \'\')", Telerik.Reporting.FilterOperator.Equal, "White"));
            formattingRule8.StopIfTrue = true;
            formattingRule8.Style.BackgroundColor = System.Drawing.Color.White;
            formattingRule9.Filters.Add(new Telerik.Reporting.Filter("=ISNULL(Fields.Color, \'\')", Telerik.Reporting.FilterOperator.Equal, "Multi"));
            formattingRule9.StopIfTrue = true;
            formattingRule9.Style.Visible = false;
            formattingRule10.Filters.Add(new Telerik.Reporting.Filter("=ISNULL(Fields.Color, \'\')", Telerik.Reporting.FilterOperator.Equal, "Silver/Black"));
            formattingRule10.StopIfTrue = true;
            formattingRule10.Style.Visible = false;
            this.shape2.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule3,
            formattingRule4,
            formattingRule5,
            formattingRule6,
            formattingRule7,
            formattingRule8,
            formattingRule9,
            formattingRule10});
            this.shape2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(7.915496826171875E-05D));
            this.shape2.Name = "shape2";
            this.shape2.ShapeType = new Telerik.Reporting.Drawing.Shapes.EllipseShape(0D);
            this.shape2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.23958000540733337D), Telerik.Reporting.Drawing.Unit.Inch(0.2399999350309372D));
            this.shape2.Style.BackgroundColor = System.Drawing.Color.Transparent;
            this.shape2.Style.BackgroundImage.MimeType = "image/png";
            this.shape2.Style.Color = System.Drawing.Color.Transparent;
            this.shape2.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.shape2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.shape2.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.shape2.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.shape2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionString = "Telerik.Reporting.Examples.CSharp.Properties.Settings.TelerikConnectionString";
            this.sqlDataSource1.Name = "sqlDataSource1";
            this.sqlDataSource1.SelectCommand = resources.GetString("sqlDataSource1.SelectCommand");
            // 
            // ProductTagReport
            // 
            this.DataSource = this.sqlDataSource1;
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.detail});
            this.Name = "ProductTagReport";
            this.PageSettings.ColumnCount = 3;
            this.PageSettings.ColumnSpacing = Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D);
            this.PageSettings.Landscape = false;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Inch(0.30000001192092896D), Telerik.Reporting.Drawing.Unit.Inch(0.40000000596046448D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Letter;
            this.Style.BackgroundColor = System.Drawing.Color.White;
            this.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1});
            this.Width = Telerik.Reporting.Drawing.Unit.Cm(5.5999999046325684D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private DetailSection detail;
        private Panel panel1;
        private TextBox txtName;
        private TextBox txtTitle;
        private TextBox textBox5;
        private TextBox textBox6;
        private PictureBox pictureBox1;
        private TextBox textBox2;
        private TextBox textBox3;
        private TextBox textBox4;
        private TextBox textBox7;
        private TextBox textBox8;
        private TextBox textBox9;
        private TextBox textBox10;
        private TextBox textBox11;
        private TextBox textBox12;
        private SqlDataSource sqlDataSource1;
        private Panel panel2;
        private TextBox textBox1;
        private Barcode barcode1;
        private Shape shape1;
        private TextBox textBox14;
        private Shape shape2;
        private Panel panel3;
    }
}
