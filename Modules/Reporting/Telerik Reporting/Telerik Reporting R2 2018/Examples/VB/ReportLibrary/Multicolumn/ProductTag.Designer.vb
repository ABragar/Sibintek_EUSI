Imports Telerik.Reporting
Imports System.Data.SqlClient
Imports Telerik.Reporting.Barcodes


Partial Class ProductTagReport
#Region "Component Designer generated code"
    ''' <summary>
    ''' Required method for Telerik Reporting designer support - do not modify
    ''' the contents of this method with the code editor.
    ''' </summary>
    Private Sub InitializeComponent()
        Dim EaN128Encoder1 As Telerik.Reporting.Barcodes.EAN128Encoder = New Telerik.Reporting.Barcodes.EAN128Encoder()
        Dim FormattingRule1 As Telerik.Reporting.Drawing.FormattingRule = New Telerik.Reporting.Drawing.FormattingRule()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ProductTagReport))
        Dim FormattingRule2 As Telerik.Reporting.Drawing.FormattingRule = New Telerik.Reporting.Drawing.FormattingRule()
        Dim FormattingRule3 As Telerik.Reporting.Drawing.FormattingRule = New Telerik.Reporting.Drawing.FormattingRule()
        Dim FormattingRule4 As Telerik.Reporting.Drawing.FormattingRule = New Telerik.Reporting.Drawing.FormattingRule()
        Dim FormattingRule5 As Telerik.Reporting.Drawing.FormattingRule = New Telerik.Reporting.Drawing.FormattingRule()
        Dim FormattingRule6 As Telerik.Reporting.Drawing.FormattingRule = New Telerik.Reporting.Drawing.FormattingRule()
        Dim FormattingRule7 As Telerik.Reporting.Drawing.FormattingRule = New Telerik.Reporting.Drawing.FormattingRule()
        Dim FormattingRule8 As Telerik.Reporting.Drawing.FormattingRule = New Telerik.Reporting.Drawing.FormattingRule()
        Dim FormattingRule9 As Telerik.Reporting.Drawing.FormattingRule = New Telerik.Reporting.Drawing.FormattingRule()
        Dim FormattingRule10 As Telerik.Reporting.Drawing.FormattingRule = New Telerik.Reporting.Drawing.FormattingRule()
        Dim StyleRule1 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Me.detail = New Telerik.Reporting.DetailSection()
        Me.panel1 = New Telerik.Reporting.Panel()
        Me.txtName = New Telerik.Reporting.TextBox()
        Me.pictureBox1 = New Telerik.Reporting.PictureBox()
        Me.barcode1 = New Telerik.Reporting.Barcode()
        Me.textBox5 = New Telerik.Reporting.TextBox()
        Me.textBox6 = New Telerik.Reporting.TextBox()
        Me.panel2 = New Telerik.Reporting.Panel()
        Me.textBox2 = New Telerik.Reporting.TextBox()
        Me.textBox4 = New Telerik.Reporting.TextBox()
        Me.textBox10 = New Telerik.Reporting.TextBox()
        Me.textBox3 = New Telerik.Reporting.TextBox()
        Me.textBox7 = New Telerik.Reporting.TextBox()
        Me.textBox8 = New Telerik.Reporting.TextBox()
        Me.textBox9 = New Telerik.Reporting.TextBox()
        Me.textBox11 = New Telerik.Reporting.TextBox()
        Me.textBox12 = New Telerik.Reporting.TextBox()
        Me.txtTitle = New Telerik.Reporting.TextBox()
        Me.textBox1 = New Telerik.Reporting.TextBox()
        Me.shape1 = New Telerik.Reporting.Shape()
        Me.textBox14 = New Telerik.Reporting.TextBox()
        Me.panel3 = New Telerik.Reporting.Panel()
        Me.shape2 = New Telerik.Reporting.Shape()
        Me.sqlDataSource1 = New Telerik.Reporting.SqlDataSource()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'detail
        '
        Me.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(11.430000305175781R)
        Me.detail.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.panel1})
        Me.detail.Name = "detail"
        '
        'panel1
        '
        Me.panel1.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.txtName, Me.pictureBox1, Me.barcode1, Me.textBox5, Me.textBox6, Me.panel2})
        Me.panel1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.013781229965388775R), Telerik.Reporting.Drawing.Unit.Inch(0.0R))
        Me.panel1.Name = "panel1"
        Me.panel1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(60.101001739501953R), Telerik.Reporting.Drawing.Unit.Mm(111.76000213623047R))
        Me.panel1.Style.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(242, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.panel1.Style.BackgroundImage.Repeat = Telerik.Reporting.Drawing.BackgroundRepeat.NoRepeat
        Me.panel1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.panel1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        '
        'txtName
        '
        Me.txtName.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.38109993934631348R), Telerik.Reporting.Drawing.Unit.Mm(0.0R))
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.2188005447387695R), Telerik.Reporting.Drawing.Unit.Cm(1.5239999294281006R))
        Me.txtName.Style.Color = System.Drawing.Color.Black
        Me.txtName.Style.Font.Bold = True
        Me.txtName.Style.Font.Name = "Segoe UI"
        Me.txtName.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(16.0R)
        Me.txtName.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left
        Me.txtName.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom
        Me.txtName.TextWrap = True
        Me.txtName.Value = "=Fields.ProductName"
        '
        'pictureBox1
        '
        Me.pictureBox1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.000039368867874145508R), Telerik.Reporting.Drawing.Unit.Cm(1.5241997241973877R))
        Me.pictureBox1.MimeType = ""
        Me.pictureBox1.Name = "pictureBox1"
        Me.pictureBox1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.0098996162414551R), Telerik.Reporting.Drawing.Unit.Cm(2.5398004055023193R))
        Me.pictureBox1.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.ScaleProportional
        Me.pictureBox1.Style.BackgroundColor = System.Drawing.Color.White
        Me.pictureBox1.Style.BorderColor.Default = System.Drawing.Color.Transparent
        Me.pictureBox1.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None
        Me.pictureBox1.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Inch(0.10000000149011612R)
        Me.pictureBox1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.10000000149011612R)
        Me.pictureBox1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Inch(0.10000000149011612R)
        Me.pictureBox1.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Inch(0.10000000149011612R)
        Me.pictureBox1.Style.Visible = True
        Me.pictureBox1.Value = "=Fields.LargePhoto"
        '
        'barcode1
        '
        Me.barcode1.Angle = 0.0R
        Me.barcode1.Checksum = True
        EaN128Encoder1.ShowText = False
        Me.barcode1.Encoder = EaN128Encoder1
        Me.barcode1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(11.761085510253906R), Telerik.Reporting.Drawing.Unit.Mm(88.899993896484375R))
        Me.barcode1.Name = "barcode1"
        Me.barcode1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(44.2379150390625R), Telerik.Reporting.Drawing.Unit.Mm(20.320003509521484R))
        Me.barcode1.Stretch = True
        Me.barcode1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8.0R)
        Me.barcode1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.barcode1.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom
        Me.barcode1.Value = "=Fields.ProductNumber"
        '
        'textBox5
        '
        Me.textBox5.Angle = 90.0R
        Me.textBox5.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(7.759087085723877R), Telerik.Reporting.Drawing.Unit.Mm(88.899993896484375R))
        Me.textBox5.Name = "textBox5"
        Me.textBox5.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(4.0R), Telerik.Reporting.Drawing.Unit.Mm(20.320003509521484R))
        Me.textBox5.Style.Color = System.Drawing.Color.Black
        Me.textBox5.Style.Font.Italic = False
        Me.textBox5.Style.Font.Name = "Segoe UI"
        Me.textBox5.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8.0R)
        Me.textBox5.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.textBox5.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.textBox5.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        Me.textBox5.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left
        Me.textBox5.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom
        Me.textBox5.Value = "Product No."
        '
        'textBox6
        '
        Me.textBox6.Angle = 90.0R
        Me.textBox6.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(3.8109993934631348R), Telerik.Reporting.Drawing.Unit.Mm(88.899993896484375R))
        Me.textBox6.Name = "textBox6"
        Me.textBox6.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(3.9460890293121338R), Telerik.Reporting.Drawing.Unit.Mm(20.320003509521484R))
        Me.textBox6.Style.Color = System.Drawing.Color.Black
        Me.textBox6.Style.Font.Bold = True
        Me.textBox6.Style.Font.Name = "Segoe UI"
        Me.textBox6.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        Me.textBox6.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.textBox6.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.textBox6.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        Me.textBox6.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left
        Me.textBox6.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom
        Me.textBox6.Value = "=Fields.ProductNumber"
        '
        'panel2
        '
        Me.panel2.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.textBox2, Me.textBox4, Me.textBox10, Me.textBox3, Me.textBox7, Me.textBox8, Me.textBox9, Me.textBox11, Me.textBox12, Me.txtTitle, Me.textBox1, Me.shape1, Me.textBox14, Me.panel3})
        Me.panel2.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.000039378803194267675R), Telerik.Reporting.Drawing.Unit.Inch(1.6000789403915405R))
        Me.panel2.Name = "panel2"
        Me.panel2.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.0100002288818359R), Telerik.Reporting.Drawing.Unit.Inch(1.8998422622680664R))
        '
        'textBox2
        '
        Me.textBox2.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448R), Telerik.Reporting.Drawing.Unit.Inch(0.099921226501464844R))
        Me.textBox2.Name = "textBox2"
        Me.textBox2.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.9335449934005737R), Telerik.Reporting.Drawing.Unit.Cm(0.50264549255371094R))
        Me.textBox2.Style.Font.Bold = False
        Me.textBox2.Style.Font.Name = "Segoe UI"
        Me.textBox2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        Me.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left
        Me.textBox2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.textBox2.Value = "LIST PRICE:"
        '
        'textBox4
        '
        Me.textBox4.Format = "{0:C2}"
        Me.textBox4.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(2.5318536758422852R), Telerik.Reporting.Drawing.Unit.Cm(0.0027453103102743626R))
        Me.textBox4.Name = "textBox4"
        Me.textBox4.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.0679464340209961R), Telerik.Reporting.Drawing.Unit.Cm(0.75905394554138184R))
        Me.textBox4.Style.Color = System.Drawing.Color.Black
        Me.textBox4.Style.Font.Bold = True
        Me.textBox4.Style.Font.Name = "Segoe UI"
        Me.textBox4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(16.0R)
        Me.textBox4.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left
        Me.textBox4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.textBox4.Value = "=Fields.ListPrice"
        '
        'textBox10
        '
        Me.textBox10.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448R), Telerik.Reporting.Drawing.Unit.Cm(1.7778000831604004R))
        Me.textBox10.Name = "textBox10"
        Me.textBox10.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.9335449934005737R), Telerik.Reporting.Drawing.Unit.Inch(0.23999999463558197R))
        Me.textBox10.Style.Font.Bold = False
        Me.textBox10.Style.Font.Name = "Segoe UI"
        Me.textBox10.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        Me.textBox10.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left
        Me.textBox10.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.textBox10.Value = "COLOR:"
        '
        'textBox3
        '
        Me.textBox3.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448R), Telerik.Reporting.Drawing.Unit.Cm(2.3876004219055176R))
        Me.textBox3.Name = "textBox3"
        Me.textBox3.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.9335449934005737R), Telerik.Reporting.Drawing.Unit.Cm(0.5R))
        Me.textBox3.Style.Font.Bold = False
        Me.textBox3.Style.Font.Name = "Segoe UI"
        Me.textBox3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        Me.textBox3.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left
        Me.textBox3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.textBox3.Value = "SIZE:"
        '
        'textBox7
        '
        Me.textBox7.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(2.5298001766204834R), Telerik.Reporting.Drawing.Unit.Cm(2.3876004219055176R))
        Me.textBox7.Name = "textBox7"
        Me.textBox7.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.0983283519744873R), Telerik.Reporting.Drawing.Unit.Cm(0.5R))
        Me.textBox7.Style.Font.Name = "Segoe UI"
        Me.textBox7.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        Me.textBox7.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left
        Me.textBox7.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.textBox7.Value = "=IsNull(Fields.Size, 'N/A') + "" "" + IsNull(Fields.SizeUnitMeasureCode, '')"
        '
        'textBox8
        '
        Me.textBox8.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.14999997615814209R), Telerik.Reporting.Drawing.Unit.Cm(2.887800931930542R))
        Me.textBox8.Name = "textBox8"
        Me.textBox8.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.9335449934005737R), Telerik.Reporting.Drawing.Unit.Cm(0.5R))
        Me.textBox8.Style.Font.Bold = False
        Me.textBox8.Style.Font.Name = "Segoe UI"
        Me.textBox8.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        Me.textBox8.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left
        Me.textBox8.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.textBox8.Value = "WEIGHT:"
        '
        'textBox9
        '
        Me.textBox9.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(2.5318536758422852R), Telerik.Reporting.Drawing.Unit.Cm(2.887800931930542R))
        Me.textBox9.Name = "textBox9"
        Me.textBox9.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.0962750911712646R), Telerik.Reporting.Drawing.Unit.Cm(0.5R))
        Me.textBox9.Style.Font.Name = "Segoe UI"
        Me.textBox9.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        Me.textBox9.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left
        Me.textBox9.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.textBox9.Value = "=IsNull(Fields.Weight, 'N/A') + "" "" + IsNull(Fields.WeightUnitMeasureCode, '')"
        '
        'textBox11
        '
        Me.textBox11.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448R), Telerik.Reporting.Drawing.Unit.Cm(3.75R))
        Me.textBox11.Name = "textBox11"
        Me.textBox11.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.9335449934005737R), Telerik.Reporting.Drawing.Unit.Cm(0.5R))
        Me.textBox11.Style.Font.Bold = False
        Me.textBox11.Style.Font.Name = "Segoe UI"
        Me.textBox11.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        Me.textBox11.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left
        Me.textBox11.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.textBox11.Value = "VENDOR:"
        '
        'textBox12
        '
        Me.textBox12.CanGrow = False
        Me.textBox12.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(2.5318536758422852R), Telerik.Reporting.Drawing.Unit.Cm(3.7499995231628418R))
        Me.textBox12.Name = "textBox12"
        Me.textBox12.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.0699999332427979R), Telerik.Reporting.Drawing.Unit.Cm(0.5R))
        Me.textBox12.Style.Font.Name = "Segoe UI"
        Me.textBox12.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        Me.textBox12.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left
        Me.textBox12.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.textBox12.Value = "=Fields.Vendor"
        '
        'txtTitle
        '
        Me.txtTitle.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(2.5318539142608643R), Telerik.Reporting.Drawing.Unit.Cm(1.0157995223999023R))
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.0999999046325684R), Telerik.Reporting.Drawing.Unit.Cm(0.43683755397796631R))
        Me.txtTitle.Style.Color = System.Drawing.Color.Black
        Me.txtTitle.Style.Font.Name = "Segoe UI"
        Me.txtTitle.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        Me.txtTitle.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left
        Me.txtTitle.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.txtTitle.Value = "=Fields.ProductSubName"
        '
        'textBox1
        '
        Me.textBox1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448R), Telerik.Reporting.Drawing.Unit.Inch(0.39992108941078186R))
        Me.textBox1.Name = "textBox1"
        Me.textBox1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.76123809814453125R), Telerik.Reporting.Drawing.Unit.Inch(0.20000012218952179R))
        Me.textBox1.Style.Font.Name = "Segoe UI"
        Me.textBox1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        Me.textBox1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left
        Me.textBox1.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.textBox1.Value = "CATEGORY:"
        '
        'shape1
        '
        Me.shape1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.14999997615814209R), Telerik.Reporting.Drawing.Unit.Inch(1.7899999618530273R))
        Me.shape1.Name = "shape1"
        Me.shape1.ShapeType = New Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW)
        Me.shape1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.0546457767486572R), Telerik.Reporting.Drawing.Unit.Inch(0.069053970277309418R))
        Me.shape1.Style.LineStyle = Telerik.Reporting.Drawing.LineStyle.Dotted
        '
        'textBox14
        '
        Me.textBox14.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.2861794233322144R), Telerik.Reporting.Drawing.Unit.Inch(0.70000046491622925R))
        Me.textBox14.Name = "textBox14"
        Me.textBox14.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.91846638917922974R), Telerik.Reporting.Drawing.Unit.Inch(0.23992092907428741R))
        Me.textBox14.Style.Font.Name = "Segoe UI"
        Me.textBox14.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        Me.textBox14.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left
        Me.textBox14.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.textBox14.Value = "=Fields.Color"
        '
        'panel3
        '
        FormattingRule1.Filters.Add(New Telerik.Reporting.Filter("=ISNULL(Fields.Color, '')", Telerik.Reporting.FilterOperator.Equal, "Multi"))
        FormattingRule1.Style.BackgroundImage.ImageData = CType(resources.GetObject("resource.ImageData"), System.Drawing.Image)
        FormattingRule1.Style.BackgroundImage.MimeType = "image/png"
        FormattingRule1.Style.BackgroundImage.Repeat = Telerik.Reporting.Drawing.BackgroundRepeat.NoRepeat
        FormattingRule2.Filters.Add(New Telerik.Reporting.Filter("=ISNULL(Fields.Color, '')", Telerik.Reporting.FilterOperator.Equal, "Silver/Black"))
        FormattingRule2.Style.BackgroundImage.ImageData = CType(resources.GetObject("resource.ImageData1"), System.Drawing.Image)
        FormattingRule2.Style.BackgroundImage.MimeType = "image/png"
        FormattingRule2.Style.BackgroundImage.Repeat = Telerik.Reporting.Drawing.BackgroundRepeat.NoRepeat
        Me.panel3.ConditionalFormatting.AddRange(New Telerik.Reporting.Drawing.FormattingRule() {FormattingRule1, FormattingRule2})
        Me.panel3.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.shape2})
        Me.panel3.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.99598425626754761R), Telerik.Reporting.Drawing.Unit.Inch(0.69992130994796753R))
        Me.panel3.Name = "panel3"
        Me.panel3.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.23957991600036621R), Telerik.Reporting.Drawing.Unit.Inch(0.24007908999919891R))
        '
        'shape2
        '
        Me.shape2.Bindings.Add(New Telerik.Reporting.Binding("Visible", "=(ISNULL(Fields.Color,'Multi')<>'Multi')"))
        FormattingRule3.Filters.Add(New Telerik.Reporting.Filter("=ISNULL(Fields.Color, '')", Telerik.Reporting.FilterOperator.Equal, "Red"))
        FormattingRule3.StopIfTrue = True
        FormattingRule3.Style.BackgroundColor = System.Drawing.Color.Red
        FormattingRule4.Filters.Add(New Telerik.Reporting.Filter("=ISNULL(Fields.Color, '')", Telerik.Reporting.FilterOperator.Equal, "Blue"))
        FormattingRule4.StopIfTrue = True
        FormattingRule4.Style.BackgroundColor = System.Drawing.Color.Blue
        FormattingRule5.Filters.Add(New Telerik.Reporting.Filter("=ISNULL(Fields.Color, '')", Telerik.Reporting.FilterOperator.Equal, "Black"))
        FormattingRule5.StopIfTrue = True
        FormattingRule5.Style.BackgroundColor = System.Drawing.Color.Black
        FormattingRule6.Filters.Add(New Telerik.Reporting.Filter("=ISNULL(Fields.Color, '')", Telerik.Reporting.FilterOperator.Equal, "Silver"))
        FormattingRule6.StopIfTrue = True
        FormattingRule6.Style.BackgroundColor = System.Drawing.Color.Silver
        FormattingRule7.Filters.Add(New Telerik.Reporting.Filter("=ISNULL(Fields.Color, '')", Telerik.Reporting.FilterOperator.Equal, "Yellow"))
        FormattingRule7.StopIfTrue = True
        FormattingRule7.Style.BackgroundColor = System.Drawing.Color.Yellow
        FormattingRule8.Filters.Add(New Telerik.Reporting.Filter("=ISNULL(Fields.Color, '')", Telerik.Reporting.FilterOperator.Equal, "White"))
        FormattingRule8.StopIfTrue = True
        FormattingRule8.Style.BackgroundColor = System.Drawing.Color.White
        FormattingRule9.Filters.Add(New Telerik.Reporting.Filter("=ISNULL(Fields.Color, '')", Telerik.Reporting.FilterOperator.Equal, "Multi"))
        FormattingRule9.StopIfTrue = True
        FormattingRule9.Style.Visible = False
        FormattingRule10.Filters.Add(New Telerik.Reporting.Filter("=ISNULL(Fields.Color, '')", Telerik.Reporting.FilterOperator.Equal, "Silver/Black"))
        FormattingRule10.StopIfTrue = True
        FormattingRule10.Style.Visible = False
        Me.shape2.ConditionalFormatting.AddRange(New Telerik.Reporting.Drawing.FormattingRule() {FormattingRule3, FormattingRule4, FormattingRule5, FormattingRule6, FormattingRule7, FormattingRule8, FormattingRule9, FormattingRule10})
        Me.shape2.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.00007915496826171875R))
        Me.shape2.Name = "shape2"
        Me.shape2.ShapeType = New Telerik.Reporting.Drawing.Shapes.EllipseShape(0.0R)
        Me.shape2.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.23958000540733337R), Telerik.Reporting.Drawing.Unit.Inch(0.23999993503093719R))
        Me.shape2.Style.BackgroundColor = System.Drawing.Color.Transparent
        Me.shape2.Style.BackgroundImage.MimeType = "image/png"
        Me.shape2.Style.Color = System.Drawing.Color.Transparent
        Me.shape2.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        Me.shape2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        Me.shape2.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        Me.shape2.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        Me.shape2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        '
        'sqlDataSource1
        '
        Me.sqlDataSource1.ConnectionString = "My.MySettings.TelerikConnectionString"
        Me.sqlDataSource1.Name = "sqlDataSource1"
        Me.sqlDataSource1.SelectCommand = resources.GetString("sqlDataSource1.SelectCommand")
        '
        'ProductTagReport
        '
        Me.DataSource = Me.sqlDataSource1
        Me.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.detail})
        Me.Name = "ProductTagReport"
        Me.PageSettings.ColumnCount = 3
        Me.PageSettings.ColumnSpacing = Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791R)
        Me.PageSettings.Landscape = False
        Me.PageSettings.Margins = New Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224R), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224R), Telerik.Reporting.Drawing.Unit.Inch(0.30000001192092896R), Telerik.Reporting.Drawing.Unit.Inch(0.40000000596046448R))
        Me.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Letter
        Me.Style.BackgroundColor = System.Drawing.Color.White
        Me.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        StyleRule1.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.TextItemBase)), New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.HtmlTextBox))})
        StyleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        StyleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        Me.StyleSheet.AddRange(New Telerik.Reporting.Drawing.StyleRule() {StyleRule1})
        Me.Width = Telerik.Reporting.Drawing.Unit.Cm(5.5999999046325684R)
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
#End Region

    Private detail As DetailSection
    Private panel1 As Panel
    Private txtName As TextBox
    Private txtTitle As TextBox
    Private textBox5 As TextBox
    Private textBox6 As TextBox
    Private pictureBox1 As PictureBox
    Private textBox2 As TextBox
    Private textBox3 As TextBox
    Private textBox4 As TextBox
    Private textBox7 As TextBox
    Private textBox8 As TextBox
    Private textBox9 As TextBox
    Private textBox10 As TextBox
    Private textBox11 As TextBox
    Private textBox12 As TextBox
    Private sqlDataSource1 As SqlDataSource
    Private panel2 As Panel
    Private textBox1 As TextBox
    Private barcode1 As Barcode
    Private shape1 As Shape
    Private textBox14 As TextBox
    Private shape2 As Shape
    Private panel3 As Panel
End Class
