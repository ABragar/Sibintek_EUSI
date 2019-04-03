namespace Telerik.Reporting.Examples.CSharp
{
    using Telerik.Reporting;

    partial class Invoice
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.Reporting.Barcodes.Code128Encoder code128Encoder1 = new Telerik.Reporting.Barcodes.Code128Encoder();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Invoice));
            Telerik.Reporting.TypeReportSource typeReportSource1 = new Telerik.Reporting.TypeReportSource();
            Telerik.Reporting.Group group1 = new Telerik.Reporting.Group();
            Telerik.Reporting.Drawing.TextWatermark textWatermark1 = new Telerik.Reporting.Drawing.TextWatermark();
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter2 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter3 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
            this.groupFooterSection1 = new Telerik.Reporting.GroupFooterSection();
            this.groupHeaderSection1 = new Telerik.Reporting.GroupHeaderSection();
            this.panel5 = new Telerik.Reporting.Panel();
            this.Text1 = new Telerik.Reporting.TextBox();
            this.barcode2 = new Telerik.Reporting.Barcode();
            this.pictureBox1 = new Telerik.Reporting.PictureBox();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.panel3 = new Telerik.Reporting.Panel();
            this.Id1 = new Telerik.Reporting.TextBox();
            this.Text8 = new Telerik.Reporting.TextBox();
            this.Field6 = new Telerik.Reporting.TextBox();
            this.shape3 = new Telerik.Reporting.Shape();
            this.OrderNumbersDataSource = new Telerik.Reporting.SqlDataSource();
            this.detail = new Telerik.Reporting.DetailSection();
            this.panel4 = new Telerik.Reporting.Panel();
            this.textBox14 = new Telerik.Reporting.TextBox();
            this.textBox10 = new Telerik.Reporting.TextBox();
            this.textBox9 = new Telerik.Reporting.TextBox();
            this.textBox8 = new Telerik.Reporting.TextBox();
            this.textBox6 = new Telerik.Reporting.TextBox();
            this.textBox4 = new Telerik.Reporting.TextBox();
            this.textBox12 = new Telerik.Reporting.TextBox();
            this.textBox13 = new Telerik.Reporting.TextBox();
            this.textBox11 = new Telerik.Reporting.TextBox();
            this.textBox16 = new Telerik.Reporting.TextBox();
            this.textBox17 = new Telerik.Reporting.TextBox();
            this.textBox18 = new Telerik.Reporting.TextBox();
            this.textBox19 = new Telerik.Reporting.TextBox();
            this.textBox21 = new Telerik.Reporting.TextBox();
            this.panel1 = new Telerik.Reporting.Panel();
            this.textBox24 = new Telerik.Reporting.TextBox();
            this.textBox23 = new Telerik.Reporting.TextBox();
            this.textBox25 = new Telerik.Reporting.TextBox();
            this.textBox26 = new Telerik.Reporting.TextBox();
            this.textBox27 = new Telerik.Reporting.TextBox();
            this.textBox30 = new Telerik.Reporting.TextBox();
            this.textBox29 = new Telerik.Reporting.TextBox();
            this.textBox28 = new Telerik.Reporting.TextBox();
            this.textBox32 = new Telerik.Reporting.TextBox();
            this.textBox31 = new Telerik.Reporting.TextBox();
            this.panel2 = new Telerik.Reporting.Panel();
            this.textBox7 = new Telerik.Reporting.TextBox();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.textBox3 = new Telerik.Reporting.TextBox();
            this.subReport1 = new Telerik.Reporting.SubReport();
            this.InvoicesDataSource = new Telerik.Reporting.SqlDataSource();
            this.pageFooterSection1 = new Telerik.Reporting.PageFooterSection();
            this.textBox5 = new Telerik.Reporting.TextBox();
            this.textBox22 = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // groupFooterSection1
            // 
            this.groupFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.23541705310344696D);
            this.groupFooterSection1.Name = "groupFooterSection1";
            this.groupFooterSection1.Style.Visible = false;
            // 
            // groupHeaderSection1
            // 
            this.groupHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(1.4500000476837158D);
            this.groupHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.panel5});
            this.groupHeaderSection1.Name = "groupHeaderSection1";
            // 
            // panel5
            // 
            this.panel5.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.Text1,
            this.barcode2,
            this.pictureBox1,
            this.textBox1,
            this.panel3});
            this.panel5.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.010456085205078125D), Telerik.Reporting.Drawing.Unit.Inch(3.9339065551757812E-05D));
            this.panel5.Name = "panel5";
            this.panel5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(20.547340393066406D), Telerik.Reporting.Drawing.Unit.Cm(3.668447732925415D));
            // 
            // Text1
            // 
            this.Text1.CanGrow = false;
            this.Text1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(5.3865165710449219D), Telerik.Reporting.Drawing.Unit.Cm(0.73998814821243286D));
            this.Text1.Name = "Text1";
            this.Text1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.0666971206665039D), Telerik.Reporting.Drawing.Unit.Cm(1.3915178775787354D));
            this.Text1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.Text1.Value = "275 Grove St., Suite 2-400, Newton, MA 02466\r\nPhone: +1.888.365.2779\r\nFax: +1.617" +
    ".249.2116";
            // 
            // barcode2
            // 
            this.barcode2.Encoder = code128Encoder1;
            this.barcode2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(15.098536491394043D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.barcode2.Name = "barcode2";
            this.barcode2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.448702335357666D), Telerik.Reporting.Drawing.Unit.Cm(2.1811349391937256D));
            this.barcode2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.barcode2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom;
            this.barcode2.Value = "=Fields.PurchaseOrderNumber";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.pictureBox1.MimeType = "image/jpeg";
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.8914073705673218D), Telerik.Reporting.Drawing.Unit.Inch(0.8587145209312439D));
            this.pictureBox1.Value = ((object)(resources.GetObject("pictureBox1.Value")));
            // 
            // textBox1
            // 
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.1206760406494141D), Telerik.Reporting.Drawing.Unit.Inch(3.9378803194267675E-05D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.9106760025024414D), Telerik.Reporting.Drawing.Unit.Inch(0.29121589660644531D));
            this.textBox1.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(47)))), ((int)(((byte)(11)))));
            this.textBox1.Value = "AdventureWorks";
            // 
            // panel3
            // 
            this.panel3.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.Id1,
            this.Text8,
            this.Field6,
            this.shape3});
            this.panel3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.9418537198798731E-05D), Telerik.Reporting.Drawing.Unit.Inch(0.93754321336746216D));
            this.panel3.Name = "panel3";
            this.panel3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.0894250869750977D), Telerik.Reporting.Drawing.Unit.Inch(0.50285667181015015D));
            // 
            // Id1
            // 
            this.Id1.CanGrow = false;
            this.Id1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.Id1.Name = "Id1";
            this.Id1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(10.239533424377441D), Telerik.Reporting.Drawing.Unit.Inch(0.44999998807907104D));
            this.Id1.Style.BackgroundColor = System.Drawing.Color.Empty;
            this.Id1.Style.BorderColor.Default = System.Drawing.Color.Black;
            this.Id1.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.Id1.StyleName = "Header";
            this.Id1.Value = "Sales Order: {Fields.SalesOrderID}";
            // 
            // Text8
            // 
            this.Text8.CanGrow = false;
            this.Text8.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(13.574539184570313D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.Text8.Name = "Text8";
            this.Text8.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.5236968994140625D), Telerik.Reporting.Drawing.Unit.Inch(0.44999998807907104D));
            this.Text8.Style.BackgroundColor = System.Drawing.Color.Empty;
            this.Text8.Style.Visible = true;
            this.Text8.StyleName = "Header";
            this.Text8.Value = "Date:";
            // 
            // Field6
            // 
            this.Field6.CanGrow = false;
            this.Field6.Format = "{0:d}";
            this.Field6.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(15.133413314819336D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.Field6.Name = "Field6";
            this.Field6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.4136261940002441D), Telerik.Reporting.Drawing.Unit.Inch(0.44999998807907104D));
            this.Field6.Style.BackgroundColor = System.Drawing.Color.Empty;
            this.Field6.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.Field6.StyleName = "Header";
            this.Field6.Value = "= Fields.OrderDate";
            // 
            // shape3
            // 
            this.shape3.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Left | Telerik.Reporting.AnchoringStyles.Right)));
            this.shape3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.47507891058921814D));
            this.shape3.Name = "shape3";
            this.shape3.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.shape3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.0893850326538086D), Telerik.Reporting.Drawing.Unit.Point(2D));
            this.shape3.Stretch = true;
            this.shape3.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.shape3.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            this.shape3.Style.BorderWidth.Bottom = Telerik.Reporting.Drawing.Unit.Point(0.5D);
            this.shape3.Style.BorderWidth.Top = Telerik.Reporting.Drawing.Unit.Point(1.5D);
            this.shape3.Style.Color = System.Drawing.Color.Transparent;
            // 
            // OrderNumbersDataSource
            // 
            this.OrderNumbersDataSource.ConnectionString = "Telerik.Reporting.Examples.CSharp.Properties.Settings.TelerikConnectionString";
            this.OrderNumbersDataSource.Name = "OrderNumbers";
            this.OrderNumbersDataSource.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@ForYear", System.Data.DbType.String, "=Parameters.ForYear.Value"),
            new Telerik.Reporting.SqlDataSourceParameter("@ForMonth", System.Data.DbType.String, "=Parameters.ForMonth.Value")});
            this.OrderNumbersDataSource.SelectCommand = resources.GetString("OrderNumbersDataSource.SelectCommand");
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Inch(3.2155463695526123D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.panel4,
            this.subReport1});
            this.detail.KeepTogether = false;
            this.detail.Name = "detail";
            // 
            // panel4
            // 
            this.panel4.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox14,
            this.textBox10,
            this.textBox9,
            this.textBox8,
            this.textBox6,
            this.textBox4,
            this.textBox12,
            this.textBox13,
            this.textBox11,
            this.textBox16,
            this.textBox17,
            this.textBox18,
            this.textBox19,
            this.textBox21,
            this.panel1,
            this.panel2,
            this.textBox2,
            this.textBox3});
            this.panel4.KeepTogether = false;
            this.panel4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.panel4.Name = "panel4";
            this.panel4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.0998811721801758D), Telerik.Reporting.Drawing.Unit.Inch(2.71911358833313D));
            // 
            // textBox14
            // 
            this.textBox14.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.8959121704101562D), Telerik.Reporting.Drawing.Unit.Inch(0.59330624341964722D));
            this.textBox14.Name = "textBox14";
            this.textBox14.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7913684844970703D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D));
            this.textBox14.Value = "=Fields.Store";
            // 
            // textBox10
            // 
            this.textBox10.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.010495503433048725D), Telerik.Reporting.Drawing.Unit.Inch(1.5699173212051392D));
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.2792060375213623D), Telerik.Reporting.Drawing.Unit.Inch(0.29374995827674866D));
            this.textBox10.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(13D);
            this.textBox10.Value = "=Fields.CustPhone";
            // 
            // textBox9
            // 
            this.textBox9.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.80837249755859375D));
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.9018634557724D), Telerik.Reporting.Drawing.Unit.Inch(0.2571415901184082D));
            this.textBox9.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(13D);
            this.textBox9.Value = "=Fields.CustFirstName + \" \" + Fields.CustLastName";
            // 
            // textBox8
            // 
            this.textBox8.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.010456085205078125D), Telerik.Reporting.Drawing.Unit.Inch(0.60829383134841919D));
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.72496062517166138D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D));
            this.textBox8.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox8.Value = "CONTACT:";
            // 
            // textBox6
            // 
            this.textBox6.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.8959121704101562D), Telerik.Reporting.Drawing.Unit.Inch(1.5699173212051392D));
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7913684844970703D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D));
            this.textBox6.Value = "=Fields.BillCountryRegion";
            // 
            // textBox4
            // 
            this.textBox4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.8959121704101562D), Telerik.Reporting.Drawing.Unit.Inch(1.3698383569717407D));
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7913684844970703D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D));
            this.textBox4.Value = "=Fields.BillPostalCode + \" \" + Fields.BillCity";
            // 
            // textBox12
            // 
            this.textBox12.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.8959121704101562D), Telerik.Reporting.Drawing.Unit.Inch(1.1697593927383423D));
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.8776596784591675D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D));
            this.textBox12.Value = "=IsNull(Fields.BillAddress2, \'No secondary Address\')";
            // 
            // textBox13
            // 
            this.textBox13.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.8959121704101562D), Telerik.Reporting.Drawing.Unit.Inch(0.9696807861328125D));
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7913684844970703D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D));
            this.textBox13.Value = "=Fields.BillAddress1";
            // 
            // textBox11
            // 
            this.textBox11.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(1.3698387145996094D));
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.72496062517166138D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D));
            this.textBox11.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox11.Value = "PHONE:";
            // 
            // textBox16
            // 
            this.textBox16.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.9547619819641113D), Telerik.Reporting.Drawing.Unit.Inch(0.57831829786300659D));
            this.textBox16.Name = "textBox16";
            this.textBox16.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1450793743133545D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D));
            this.textBox16.Value = "=Fields.Store";
            // 
            // textBox17
            // 
            this.textBox17.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.9547619819641113D), Telerik.Reporting.Drawing.Unit.Inch(0.96968048810958862D));
            this.textBox17.Name = "textBox17";
            this.textBox17.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1450793743133545D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D));
            this.textBox17.Value = "=Fields.ShipAddress1";
            // 
            // textBox18
            // 
            this.textBox18.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.9547619819641113D), Telerik.Reporting.Drawing.Unit.Inch(1.1697593927383423D));
            this.textBox18.Name = "textBox18";
            this.textBox18.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1450793743133545D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D));
            this.textBox18.Value = "=IsNull(Fields.ShipAddress2, \'No secondary Address\')";
            // 
            // textBox19
            // 
            this.textBox19.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.9547619819641113D), Telerik.Reporting.Drawing.Unit.Inch(1.3698383569717407D));
            this.textBox19.Name = "textBox19";
            this.textBox19.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1450793743133545D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D));
            this.textBox19.Value = "=Fields.ShipPostalCode + \" \" + Fields.ShipCity + \", \" + Fields.ShipStateProvince " +
    "";
            // 
            // textBox21
            // 
            this.textBox21.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.9547619819641113D), Telerik.Reporting.Drawing.Unit.Inch(1.5699173212051392D));
            this.textBox21.Name = "textBox21";
            this.textBox21.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1450793743133545D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D));
            this.textBox21.Value = "=Fields.ShipCountryRegion";
            // 
            // panel1
            // 
            this.panel1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox24,
            this.textBox23,
            this.textBox25,
            this.textBox26,
            this.textBox27,
            this.textBox30,
            this.textBox29,
            this.textBox28,
            this.textBox32,
            this.textBox31});
            this.panel1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(2.0311710834503174D));
            this.panel1.Name = "panel1";
            this.panel1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.0998411178588867D), Telerik.Reporting.Drawing.Unit.Inch(0.55303496122360229D));
            // 
            // textBox24
            // 
            this.textBox24.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(3.9418537198798731E-05D));
            this.textBox24.Name = "textBox24";
            this.textBox24.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.1395833492279053D), Telerik.Reporting.Drawing.Unit.Inch(0.3425000011920929D));
            this.textBox24.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox24.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox24.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Point(0.5D);
            this.textBox24.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox24.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox24.Value = "DATE:";
            // 
            // textBox23
            // 
            this.textBox23.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.1396621465682983D), Telerik.Reporting.Drawing.Unit.Inch(3.9418537198798731E-05D));
            this.textBox23.Name = "textBox23";
            this.textBox23.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7561712265014648D), Telerik.Reporting.Drawing.Unit.Inch(0.3425000011920929D));
            this.textBox23.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox23.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox23.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Point(0.5D);
            this.textBox23.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox23.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox23.Value = "ORDER DATE:";
            // 
            // textBox25
            // 
            this.textBox25.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.8959121704101562D), Telerik.Reporting.Drawing.Unit.Inch(3.9418537198798731E-05D));
            this.textBox25.Name = "textBox25";
            this.textBox25.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.6796835660934448D), Telerik.Reporting.Drawing.Unit.Inch(0.3425000011920929D));
            this.textBox25.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox25.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox25.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Point(0.5D);
            this.textBox25.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox25.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox25.Value = "SALES PERSON:";
            // 
            // textBox26
            // 
            this.textBox26.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.5756745338439941D), Telerik.Reporting.Drawing.Unit.Inch(3.9418537198798731E-05D));
            this.textBox26.Name = "textBox26";
            this.textBox26.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.3790086507797241D), Telerik.Reporting.Drawing.Unit.Inch(0.3425000011920929D));
            this.textBox26.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox26.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox26.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Point(0.5D);
            this.textBox26.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox26.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox26.Value = "PURCHASE ORDER:";
            // 
            // textBox27
            // 
            this.textBox27.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.9547619819641113D), Telerik.Reporting.Drawing.Unit.Inch(3.9418537198798731E-05D));
            this.textBox27.Name = "textBox27";
            this.textBox27.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1450793743133545D), Telerik.Reporting.Drawing.Unit.Inch(0.3425000011920929D));
            this.textBox27.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox27.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox27.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Point(0.5D);
            this.textBox27.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox27.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox27.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox27.Value = "SHIPMENT METHOD";
            // 
            // textBox30
            // 
            this.textBox30.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.8959121704101562D), Telerik.Reporting.Drawing.Unit.Inch(0.34261831641197205D));
            this.textBox30.Name = "textBox30";
            this.textBox30.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.6796832084655762D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D));
            this.textBox30.Value = "=Fields.SalesFirstName + \" \" + Fields.SalesLastName";
            // 
            // textBox29
            // 
            this.textBox29.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.5756745338439941D), Telerik.Reporting.Drawing.Unit.Inch(0.34261831641197205D));
            this.textBox29.Name = "textBox29";
            this.textBox29.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.3790086507797241D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D));
            this.textBox29.Value = "=Fields.PurchaseOrderNumber";
            // 
            // textBox28
            // 
            this.textBox28.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.9547619819641113D), Telerik.Reporting.Drawing.Unit.Inch(0.34261831641197205D));
            this.textBox28.Name = "textBox28";
            this.textBox28.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1450397968292236D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D));
            this.textBox28.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox28.Value = "=Fields.ShipMethod";
            // 
            // textBox32
            // 
            this.textBox32.Format = "{0:d}";
            this.textBox32.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.1396620273590088D), Telerik.Reporting.Drawing.Unit.Inch(0.34261831641197205D));
            this.textBox32.Name = "textBox32";
            this.textBox32.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7561712265014648D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D));
            this.textBox32.Value = "=Fields.OrderDate";
            // 
            // textBox31
            // 
            this.textBox31.Format = "{0:d}";
            this.textBox31.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.9339065551757812E-05D), Telerik.Reporting.Drawing.Unit.Inch(0.34261831641197205D));
            this.textBox31.Name = "textBox31";
            this.textBox31.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.1395833492279053D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D));
            this.textBox31.Value = "=Now()";
            // 
            // panel2
            // 
            this.panel2.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox7});
            this.panel2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(3.9418537198798731E-05D));
            this.panel2.Name = "panel2";
            this.panel2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.0998411178588867D), Telerik.Reporting.Drawing.Unit.Inch(0.45000001788139343D));
            this.panel2.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.panel2.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Point(0.5D);
            // 
            // textBox7
            // 
            this.textBox7.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.9041273593902588D), Telerik.Reporting.Drawing.Unit.Inch(0.44999998807907104D));
            this.textBox7.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox7.StyleName = "Header";
            this.textBox7.Value = "Customer Details";
            // 
            // textBox2
            // 
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.1311321258544922D), Telerik.Reporting.Drawing.Unit.Inch(0.60829383134841919D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.66167718172073364D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D));
            this.textBox2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox2.Value = "ADDRESS:";
            // 
            // textBox3
            // 
            this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.35480260848999D), Telerik.Reporting.Drawing.Unit.Inch(0.57831794023513794D));
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.56537884473800659D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D));
            this.textBox3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox3.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Top;
            this.textBox3.Value = "SHIP TO:";
            // 
            // subReport1
            // 
            this.subReport1.KeepTogether = false;
            this.subReport1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.010495503433048725D), Telerik.Reporting.Drawing.Unit.Inch(2.865025520324707D));
            this.subReport1.Name = "subReport1";
            typeReportSource1.Parameters.Add(new Telerik.Reporting.Parameter("SaledOrderID", "=Fields.SalesOrderID"));
            typeReportSource1.TypeName = "Telerik.Reporting.Examples.CSharp.SalesOrderDetails, CSharp.ReportLibrary, Versio" +
    "n=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            this.subReport1.ReportSource = typeReportSource1;
            this.subReport1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.0893068313598633D), Telerik.Reporting.Drawing.Unit.Inch(0.27500006556510925D));
            // 
            // InvoicesDataSource
            // 
            this.InvoicesDataSource.ConnectionString = "Telerik.Reporting.Examples.CSharp.Properties.Settings.TelerikConnectionString";
            this.InvoicesDataSource.Name = "Invoices";
            this.InvoicesDataSource.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@SalesOrderNumber", System.Data.DbType.String, "=Parameters.OrderNumber.Value")});
            this.InvoicesDataSource.SelectCommand = resources.GetString("InvoicesDataSource.SelectCommand");
            // 
            // pageFooterSection1
            // 
            this.pageFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.2291666716337204D);
            this.pageFooterSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox5,
            this.textBox22});
            this.pageFooterSection1.Name = "pageFooterSection1";
            this.pageFooterSection1.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            // 
            // textBox5
            // 
            this.textBox5.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8958332538604736D), Telerik.Reporting.Drawing.Unit.Inch(0.22912724316120148D));
            this.textBox5.Value = "=PageExec(\"lineTotalDataTextBox\",Count(Fields.ProductNumber)) + \" products, \" + P" +
    "ageExec(\"lineTotalDataTextBox\",Sum(Fields.OrderQty)) + \" items on page  \" + Page" +
    "Number";
            // 
            // textBox22
            // 
            this.textBox22.Format = "{0:C2}";
            this.textBox22.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.9685325622558594D), Telerik.Reporting.Drawing.Unit.Inch(3.9418537198798731E-05D));
            this.textBox22.Name = "textBox22";
            this.textBox22.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1312694549560547D), Telerik.Reporting.Drawing.Unit.Inch(0.22908782958984375D));
            this.textBox22.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox22.Value = "=PageExec(\"lineTotalDataTextBox\",sum(LineTotal))";
            // 
            // Invoice
            // 
            this.DataSource = this.InvoicesDataSource;
            this.Filters.Add(new Telerik.Reporting.Filter("=Fields.SalesOrderNumber", Telerik.Reporting.FilterOperator.Equal, "=Parameters.OrderNumber.Value"));
            group1.GroupFooter = this.groupFooterSection1;
            group1.GroupHeader = this.groupHeaderSection1;
            group1.Name = "group1";
            this.Groups.AddRange(new Telerik.Reporting.Group[] {
            group1});
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.groupHeaderSection1,
            this.groupFooterSection1,
            this.detail,
            this.pageFooterSection1});
            this.Name = "Invoice";
            this.PageSettings.Landscape = false;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Inch(0.30000001192092896D), Telerik.Reporting.Drawing.Unit.Inch(0.40000000596046448D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Letter;
            textWatermark1.Color = System.Drawing.Color.Black;
            textWatermark1.Font.Bold = false;
            textWatermark1.Font.Italic = false;
            textWatermark1.Font.Name = "Tahoma";
            textWatermark1.Font.Size = Telerik.Reporting.Drawing.Unit.Point(150D);
            textWatermark1.Font.Strikeout = false;
            textWatermark1.Font.Underline = false;
            textWatermark1.Opacity = 0.02D;
            textWatermark1.Orientation = Telerik.Reporting.Drawing.WatermarkOrientation.Diagonal;
            textWatermark1.Position = Telerik.Reporting.Drawing.WatermarkPosition.Front;
            textWatermark1.PrintOnFirstPage = true;
            textWatermark1.PrintOnLastPage = true;
            textWatermark1.Text = "Original";
            this.PageSettings.Watermarks.Add(textWatermark1);
            reportParameter1.AutoRefresh = true;
            reportParameter1.AvailableValues.DataSource = this.OrderNumbersDataSource;
            reportParameter1.AvailableValues.DisplayMember = "SalesOrderNumber";
            reportParameter1.AvailableValues.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.SalesOrderNumber", Telerik.Reporting.SortDirection.Asc));
            reportParameter1.AvailableValues.ValueMember = "SalesOrderNumber";
            reportParameter1.Name = "OrderNumber";
            reportParameter1.Text = "Order #";
            reportParameter1.Value = "=First(Fields.SalesOrderNumber)";
            reportParameter1.Visible = true;
            reportParameter2.Name = "ForYear";
            reportParameter2.Type = Telerik.Reporting.ReportParameterType.Integer;
            reportParameter2.Value = "=2003";
            reportParameter3.Name = "ForMonth";
            reportParameter3.Type = Telerik.Reporting.ReportParameterType.Integer;
            reportParameter3.Value = "=7";
            this.ReportParameters.Add(reportParameter1);
            this.ReportParameters.Add(reportParameter2);
            this.ReportParameters.Add(reportParameter3);
            this.Style.BackgroundColor = System.Drawing.Color.White;
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextBox))});
            styleRule2.Style.Font.Name = "Segoe UI";
            styleRule2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            styleRule3.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Header")});
            styleRule3.Style.Font.Bold = true;
            styleRule3.Style.Font.Name = "Segoe UI Light";
            styleRule3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(16D);
            styleRule3.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1,
            styleRule2,
            styleRule3});
            this.Width = Telerik.Reporting.Drawing.Unit.Inch(8.09999942779541D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.DetailSection detail;
        private SubReport subReport1;
        private Telerik.Reporting.TextBox textBox11;
        private Telerik.Reporting.TextBox textBox10;
        private Telerik.Reporting.TextBox textBox9;
        private Telerik.Reporting.TextBox textBox8;
        private Telerik.Reporting.TextBox textBox7;
        private Telerik.Reporting.TextBox textBox6;
        private Telerik.Reporting.TextBox textBox4;
        private Telerik.Reporting.TextBox textBox12;
        private Telerik.Reporting.TextBox textBox13;
        private Telerik.Reporting.TextBox textBox14;
        private Telerik.Reporting.TextBox textBox16;
        private Telerik.Reporting.TextBox textBox17;
        private Telerik.Reporting.TextBox textBox18;
        private Telerik.Reporting.TextBox textBox19;
        private Telerik.Reporting.TextBox textBox21;
        private Telerik.Reporting.TextBox textBox23;
        private Telerik.Reporting.TextBox textBox24;
        private Telerik.Reporting.TextBox textBox25;
        private Telerik.Reporting.TextBox textBox26;
        private Telerik.Reporting.TextBox textBox27;
        private Telerik.Reporting.TextBox textBox28;
        private Telerik.Reporting.TextBox textBox29;
        private Telerik.Reporting.TextBox textBox30;
        private Telerik.Reporting.TextBox textBox31;
        private Telerik.Reporting.TextBox textBox32;
        private Telerik.Reporting.Panel panel1;
        private GroupFooterSection groupFooterSection1;
        private GroupHeaderSection groupHeaderSection1;
        private Telerik.Reporting.Panel panel5;
        private Telerik.Reporting.TextBox Text1;
        private Telerik.Reporting.TextBox Field6;
        private Telerik.Reporting.TextBox Text8;
        private Telerik.Reporting.TextBox Id1;
        private Barcode barcode2;
        private Telerik.Reporting.Panel panel2;
        private Telerik.Reporting.PictureBox pictureBox1;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.Panel panel3;
        private Telerik.Reporting.Panel panel4;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.TextBox textBox3;
        private SqlDataSource InvoicesDataSource;
        private SqlDataSource OrderNumbersDataSource;
        private PageFooterSection pageFooterSection1;
        private Reporting.TextBox textBox5;
        private Telerik.Reporting.TextBox textBox22;
        private Shape shape3;
    }
}