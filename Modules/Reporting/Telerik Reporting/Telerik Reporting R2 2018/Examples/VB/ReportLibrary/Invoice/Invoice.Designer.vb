
Imports Telerik.Reporting

Partial Class Invoice
#Region "Component Designer generated code"
    ''' <summary>
    ''' Required method for telerik Reporting designer support - do not modify
    ''' the contents of this method with the code editor.
    ''' </summary>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Invoice))
        Dim TypeReportSource1 As Telerik.Reporting.TypeReportSource = New Telerik.Reporting.TypeReportSource()
        Dim Group1 As Telerik.Reporting.Group = New Telerik.Reporting.Group()
        Dim TextWatermark1 As Telerik.Reporting.Drawing.TextWatermark = New Telerik.Reporting.Drawing.TextWatermark()
        Dim ReportParameter1 As Telerik.Reporting.ReportParameter = New Telerik.Reporting.ReportParameter()
        Dim ReportParameter2 As Telerik.Reporting.ReportParameter = New Telerik.Reporting.ReportParameter()
        Dim ReportParameter3 As Telerik.Reporting.ReportParameter = New Telerik.Reporting.ReportParameter()
        Dim StyleRule1 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule2 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule3 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Me.groupFooterSection1 = New Telerik.Reporting.GroupFooterSection()
        Me.groupHeaderSection1 = New Telerik.Reporting.GroupHeaderSection()
        Me.panel5 = New Telerik.Reporting.Panel()
        Me.Text1 = New Telerik.Reporting.TextBox()
        Me.barcode2 = New Telerik.Reporting.Barcode()
        Me.pictureBox1 = New Telerik.Reporting.PictureBox()
        Me.textBox1 = New Telerik.Reporting.TextBox()
        Me.panel3 = New Telerik.Reporting.Panel()
        Me.Id1 = New Telerik.Reporting.TextBox()
        Me.Text8 = New Telerik.Reporting.TextBox()
        Me.Field6 = New Telerik.Reporting.TextBox()
        Me.shape3 = New Telerik.Reporting.Shape()
        Me.OrderNumbersDataSource = New Telerik.Reporting.SqlDataSource()
        Me.detail = New Telerik.Reporting.DetailSection()
        Me.panel4 = New Telerik.Reporting.Panel()
        Me.textBox14 = New Telerik.Reporting.TextBox()
        Me.textBox10 = New Telerik.Reporting.TextBox()
        Me.textBox9 = New Telerik.Reporting.TextBox()
        Me.textBox8 = New Telerik.Reporting.TextBox()
        Me.textBox6 = New Telerik.Reporting.TextBox()
        Me.textBox4 = New Telerik.Reporting.TextBox()
        Me.textBox12 = New Telerik.Reporting.TextBox()
        Me.textBox13 = New Telerik.Reporting.TextBox()
        Me.textBox11 = New Telerik.Reporting.TextBox()
        Me.textBox16 = New Telerik.Reporting.TextBox()
        Me.textBox17 = New Telerik.Reporting.TextBox()
        Me.textBox18 = New Telerik.Reporting.TextBox()
        Me.textBox19 = New Telerik.Reporting.TextBox()
        Me.textBox21 = New Telerik.Reporting.TextBox()
        Me.panel1 = New Telerik.Reporting.Panel()
        Me.textBox24 = New Telerik.Reporting.TextBox()
        Me.textBox23 = New Telerik.Reporting.TextBox()
        Me.textBox25 = New Telerik.Reporting.TextBox()
        Me.textBox26 = New Telerik.Reporting.TextBox()
        Me.textBox27 = New Telerik.Reporting.TextBox()
        Me.textBox30 = New Telerik.Reporting.TextBox()
        Me.textBox29 = New Telerik.Reporting.TextBox()
        Me.textBox28 = New Telerik.Reporting.TextBox()
        Me.textBox32 = New Telerik.Reporting.TextBox()
        Me.textBox31 = New Telerik.Reporting.TextBox()
        Me.panel2 = New Telerik.Reporting.Panel()
        Me.textBox7 = New Telerik.Reporting.TextBox()
        Me.textBox2 = New Telerik.Reporting.TextBox()
        Me.textBox3 = New Telerik.Reporting.TextBox()
        Me.subReport1 = New Telerik.Reporting.SubReport()
        Me.InvoicesDataSource = New Telerik.Reporting.SqlDataSource()
        Me.pageFooterSection1 = New Telerik.Reporting.PageFooterSection()
        Me.textBox5 = New Telerik.Reporting.TextBox()
        Me.textBox22 = New Telerik.Reporting.TextBox()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'groupFooterSection1
        '
        Me.groupFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.23541705310344696R)
        Me.groupFooterSection1.Name = "groupFooterSection1"
        Me.groupFooterSection1.Style.Visible = False
        '
        'groupHeaderSection1
        '
        Me.groupHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(1.4500000476837158R)
        Me.groupHeaderSection1.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.panel5})
        Me.groupHeaderSection1.Name = "groupHeaderSection1"
        '
        'panel5
        '
        Me.panel5.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.Text1, Me.barcode2, Me.pictureBox1, Me.textBox1, Me.panel3})
        Me.panel5.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.010456085205078125R), Telerik.Reporting.Drawing.Unit.Inch(0.000039339065551757813R))
        Me.panel5.Name = "panel5"
        Me.panel5.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(20.547340393066406R), Telerik.Reporting.Drawing.Unit.Cm(3.668447732925415R))
        '
        'Text1
        '
        Me.Text1.CanGrow = False
        Me.Text1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(5.3865165710449219R), Telerik.Reporting.Drawing.Unit.Cm(0.73998814821243286R))
        Me.Text1.Name = "Text1"
        Me.Text1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.0666971206665039R), Telerik.Reporting.Drawing.Unit.Cm(1.3915178775787354R))
        Me.Text1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        Me.Text1.Value = "275 Grove St., Suite 2-400, Newton, MA 02466" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Phone: +1.888.365.2779" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Fax: +1.617" & _
    ".249.2116"
        '
        'barcode2
        '
        Me.barcode2.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(15.098536491394043R), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666R))
        Me.barcode2.Name = "barcode2"
        Me.barcode2.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.448702335357666R), Telerik.Reporting.Drawing.Unit.Cm(2.1811349391937256R))
        Me.barcode2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.barcode2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom
        Me.barcode2.Value = "=Fields.PurchaseOrderNumber"
        '
        'pictureBox1
        '
        Me.pictureBox1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.0R))
        Me.pictureBox1.MimeType = "image/jpeg"
        Me.pictureBox1.Name = "pictureBox1"
        Me.pictureBox1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.8914073705673218R), Telerik.Reporting.Drawing.Unit.Inch(0.8587145209312439R))
        Me.pictureBox1.Value = CType(resources.GetObject("pictureBox1.Value"), Object)
        '
        'textBox1
        '
        Me.textBox1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.1206760406494141R), Telerik.Reporting.Drawing.Unit.Inch(0.000039378803194267675R))
        Me.textBox1.Name = "textBox1"
        Me.textBox1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.9106760025024414R), Telerik.Reporting.Drawing.Unit.Inch(0.29121589660644531R))
        Me.textBox1.Style.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(11, Byte), Integer))
        Me.textBox1.Value = "AdventureWorks"
        '
        'panel3
        '
        Me.panel3.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.Id1, Me.Text8, Me.Field6, Me.shape3})
        Me.panel3.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.000039418537198798731R), Telerik.Reporting.Drawing.Unit.Inch(0.93754321336746216R))
        Me.panel3.Name = "panel3"
        Me.panel3.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.0894250869750977R), Telerik.Reporting.Drawing.Unit.Inch(0.50285667181015015R))
        '
        'Id1
        '
        Me.Id1.CanGrow = False
        Me.Id1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.0R), Telerik.Reporting.Drawing.Unit.Cm(0.0R))
        Me.Id1.Name = "Id1"
        Me.Id1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(10.239533424377441R), Telerik.Reporting.Drawing.Unit.Inch(0.44999998807907104R))
        Me.Id1.Style.BackgroundColor = System.Drawing.Color.Empty
        Me.Id1.Style.BorderColor.Default = System.Drawing.Color.Black
        Me.Id1.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None
        Me.Id1.StyleName = "Header"
        Me.Id1.Value = "Sales Order: {Fields.SalesOrderID}"
        '
        'Text8
        '
        Me.Text8.CanGrow = False
        Me.Text8.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(13.574539184570313R), Telerik.Reporting.Drawing.Unit.Cm(0.0R))
        Me.Text8.Name = "Text8"
        Me.Text8.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.5236968994140625R), Telerik.Reporting.Drawing.Unit.Inch(0.44999998807907104R))
        Me.Text8.Style.BackgroundColor = System.Drawing.Color.Empty
        Me.Text8.Style.Visible = True
        Me.Text8.StyleName = "Header"
        Me.Text8.Value = "Date:"
        '
        'Field6
        '
        Me.Field6.CanGrow = False
        Me.Field6.Format = "{0:d}"
        Me.Field6.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(15.133413314819336R), Telerik.Reporting.Drawing.Unit.Cm(0.0R))
        Me.Field6.Name = "Field6"
        Me.Field6.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.4136261940002441R), Telerik.Reporting.Drawing.Unit.Inch(0.44999998807907104R))
        Me.Field6.Style.BackgroundColor = System.Drawing.Color.Empty
        Me.Field6.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.Field6.StyleName = "Header"
        Me.Field6.Value = "= Fields.OrderDate"
        '
        'shape3
        '
        Me.shape3.Anchoring = CType((Telerik.Reporting.AnchoringStyles.Left Or Telerik.Reporting.AnchoringStyles.Right), Telerik.Reporting.AnchoringStyles)
        Me.shape3.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.47507891058921814R))
        Me.shape3.Name = "shape3"
        Me.shape3.ShapeType = New Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW)
        Me.shape3.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.0893850326538086R), Telerik.Reporting.Drawing.Unit.Point(2.0R))
        Me.shape3.Stretch = True
        Me.shape3.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.shape3.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid
        Me.shape3.Style.BorderWidth.Bottom = Telerik.Reporting.Drawing.Unit.Point(0.5R)
        Me.shape3.Style.BorderWidth.Top = Telerik.Reporting.Drawing.Unit.Point(1.5R)
        Me.shape3.Style.Color = System.Drawing.Color.Transparent
        '
        'OrderNumbersDataSource
        '
        Me.OrderNumbersDataSource.ConnectionString = "My.MySettings.TelerikConnectionString"
        Me.OrderNumbersDataSource.Name = "OrderNumbers"
        Me.OrderNumbersDataSource.Parameters.AddRange(New Telerik.Reporting.SqlDataSourceParameter() {New Telerik.Reporting.SqlDataSourceParameter("@ForYear", System.Data.DbType.[String], "=Parameters.ForYear.Value"), New Telerik.Reporting.SqlDataSourceParameter("@ForMonth", System.Data.DbType.[String], "=Parameters.ForMonth.Value")})
        Me.OrderNumbersDataSource.SelectCommand = resources.GetString("OrderNumbersDataSource.SelectCommand")
        '
        'detail
        '
        Me.detail.Height = Telerik.Reporting.Drawing.Unit.Inch(3.2155463695526123R)
        Me.detail.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.panel4, Me.subReport1})
        Me.detail.KeepTogether = False
        Me.detail.Name = "detail"
        '
        'panel4
        '
        Me.panel4.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.textBox14, Me.textBox10, Me.textBox9, Me.textBox8, Me.textBox6, Me.textBox4, Me.textBox12, Me.textBox13, Me.textBox11, Me.textBox16, Me.textBox17, Me.textBox18, Me.textBox19, Me.textBox21, Me.panel1, Me.panel2, Me.textBox2, Me.textBox3})
        Me.panel4.KeepTogether = False
        Me.panel4.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.0R))
        Me.panel4.Name = "panel4"
        Me.panel4.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.0998811721801758R), Telerik.Reporting.Drawing.Unit.Inch(2.71911358833313R))
        '
        'textBox14
        '
        Me.textBox14.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.8959121704101562R), Telerik.Reporting.Drawing.Unit.Inch(0.59330624341964722R))
        Me.textBox14.Name = "textBox14"
        Me.textBox14.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7913684844970703R), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985R))
        Me.textBox14.Value = "=Fields.Store"
        '
        'textBox10
        '
        Me.textBox10.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.010495503433048725R), Telerik.Reporting.Drawing.Unit.Inch(1.5699173212051392R))
        Me.textBox10.Name = "textBox10"
        Me.textBox10.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.2792060375213623R), Telerik.Reporting.Drawing.Unit.Inch(0.29374995827674866R))
        Me.textBox10.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(13.0R)
        Me.textBox10.Value = "=Fields.CustPhone"
        '
        'textBox9
        '
        Me.textBox9.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.80837249755859375R))
        Me.textBox9.Name = "textBox9"
        Me.textBox9.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.9018634557724R), Telerik.Reporting.Drawing.Unit.Inch(0.2571415901184082R))
        Me.textBox9.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(13.0R)
        Me.textBox9.Value = "=Fields.CustFirstName + "" "" + Fields.CustLastName"
        '
        'textBox8
        '
        Me.textBox8.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.010456085205078125R), Telerik.Reporting.Drawing.Unit.Inch(0.60829383134841919R))
        Me.textBox8.Name = "textBox8"
        Me.textBox8.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.72496062517166138R), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985R))
        Me.textBox8.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        Me.textBox8.Value = "CONTACT:"
        '
        'textBox6
        '
        Me.textBox6.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.8959121704101562R), Telerik.Reporting.Drawing.Unit.Inch(1.5699173212051392R))
        Me.textBox6.Name = "textBox6"
        Me.textBox6.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7913684844970703R), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985R))
        Me.textBox6.Value = "=Fields.BillCountryRegion"
        '
        'textBox4
        '
        Me.textBox4.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.8959121704101562R), Telerik.Reporting.Drawing.Unit.Inch(1.3698383569717407R))
        Me.textBox4.Name = "textBox4"
        Me.textBox4.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7913684844970703R), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985R))
        Me.textBox4.Value = "=Fields.BillPostalCode + "" "" + Fields.BillCity"
        '
        'textBox12
        '
        Me.textBox12.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.8959121704101562R), Telerik.Reporting.Drawing.Unit.Inch(1.1697593927383423R))
        Me.textBox12.Name = "textBox12"
        Me.textBox12.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.8776596784591675R), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985R))
        Me.textBox12.Value = "=IsNull(Fields.BillAddress2, 'No secondary Address')"
        '
        'textBox13
        '
        Me.textBox13.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.8959121704101562R), Telerik.Reporting.Drawing.Unit.Inch(0.9696807861328125R))
        Me.textBox13.Name = "textBox13"
        Me.textBox13.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7913684844970703R), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985R))
        Me.textBox13.Value = "=Fields.BillAddress1"
        '
        'textBox11
        '
        Me.textBox11.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(1.3698387145996094R))
        Me.textBox11.Name = "textBox11"
        Me.textBox11.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.72496062517166138R), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985R))
        Me.textBox11.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        Me.textBox11.Value = "PHONE:"
        '
        'textBox16
        '
        Me.textBox16.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.9547619819641113R), Telerik.Reporting.Drawing.Unit.Inch(0.57831829786300659R))
        Me.textBox16.Name = "textBox16"
        Me.textBox16.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1450793743133545R), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985R))
        Me.textBox16.Value = "=Fields.Store"
        '
        'textBox17
        '
        Me.textBox17.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.9547619819641113R), Telerik.Reporting.Drawing.Unit.Inch(0.96968048810958862R))
        Me.textBox17.Name = "textBox17"
        Me.textBox17.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1450793743133545R), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985R))
        Me.textBox17.Value = "=Fields.ShipAddress1"
        '
        'textBox18
        '
        Me.textBox18.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.9547619819641113R), Telerik.Reporting.Drawing.Unit.Inch(1.1697593927383423R))
        Me.textBox18.Name = "textBox18"
        Me.textBox18.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1450793743133545R), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985R))
        Me.textBox18.Value = "=IsNull(Fields.ShipAddress2, 'No secondary Address')"
        '
        'textBox19
        '
        Me.textBox19.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.9547619819641113R), Telerik.Reporting.Drawing.Unit.Inch(1.3698383569717407R))
        Me.textBox19.Name = "textBox19"
        Me.textBox19.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1450793743133545R), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985R))
        Me.textBox19.Value = "=Fields.ShipPostalCode + "" "" + Fields.ShipCity + "", "" + Fields.ShipStateProvince " & _
    ""
        '
        'textBox21
        '
        Me.textBox21.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.9547619819641113R), Telerik.Reporting.Drawing.Unit.Inch(1.5699173212051392R))
        Me.textBox21.Name = "textBox21"
        Me.textBox21.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1450793743133545R), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985R))
        Me.textBox21.Value = "=Fields.ShipCountryRegion"
        '
        'panel1
        '
        Me.panel1.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.textBox24, Me.textBox23, Me.textBox25, Me.textBox26, Me.textBox27, Me.textBox30, Me.textBox29, Me.textBox28, Me.textBox32, Me.textBox31})
        Me.panel1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(2.0311710834503174R))
        Me.panel1.Name = "panel1"
        Me.panel1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.0998411178588867R), Telerik.Reporting.Drawing.Unit.Inch(0.55303496122360229R))
        '
        'textBox24
        '
        Me.textBox24.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.000039418537198798731R))
        Me.textBox24.Name = "textBox24"
        Me.textBox24.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.1395833492279053R), Telerik.Reporting.Drawing.Unit.Inch(0.3425000011920929R))
        Me.textBox24.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBox24.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBox24.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Point(0.5R)
        Me.textBox24.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        Me.textBox24.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.textBox24.Value = "DATE:"
        '
        'textBox23
        '
        Me.textBox23.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.1396621465682983R), Telerik.Reporting.Drawing.Unit.Inch(0.000039418537198798731R))
        Me.textBox23.Name = "textBox23"
        Me.textBox23.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7561712265014648R), Telerik.Reporting.Drawing.Unit.Inch(0.3425000011920929R))
        Me.textBox23.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBox23.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBox23.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Point(0.5R)
        Me.textBox23.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        Me.textBox23.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.textBox23.Value = "ORDER DATE:"
        '
        'textBox25
        '
        Me.textBox25.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.8959121704101562R), Telerik.Reporting.Drawing.Unit.Inch(0.000039418537198798731R))
        Me.textBox25.Name = "textBox25"
        Me.textBox25.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.6796835660934448R), Telerik.Reporting.Drawing.Unit.Inch(0.3425000011920929R))
        Me.textBox25.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBox25.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBox25.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Point(0.5R)
        Me.textBox25.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        Me.textBox25.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.textBox25.Value = "SALES PERSON:"
        '
        'textBox26
        '
        Me.textBox26.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.5756745338439941R), Telerik.Reporting.Drawing.Unit.Inch(0.000039418537198798731R))
        Me.textBox26.Name = "textBox26"
        Me.textBox26.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.3790086507797241R), Telerik.Reporting.Drawing.Unit.Inch(0.3425000011920929R))
        Me.textBox26.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBox26.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBox26.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Point(0.5R)
        Me.textBox26.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        Me.textBox26.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.textBox26.Value = "PURCHASE ORDER:"
        '
        'textBox27
        '
        Me.textBox27.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.9547619819641113R), Telerik.Reporting.Drawing.Unit.Inch(0.000039418537198798731R))
        Me.textBox27.Name = "textBox27"
        Me.textBox27.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1450793743133545R), Telerik.Reporting.Drawing.Unit.Inch(0.3425000011920929R))
        Me.textBox27.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBox27.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBox27.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Point(0.5R)
        Me.textBox27.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        Me.textBox27.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.textBox27.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.textBox27.Value = "SHIPMENT METHOD"
        '
        'textBox30
        '
        Me.textBox30.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.8959121704101562R), Telerik.Reporting.Drawing.Unit.Inch(0.34261831641197205R))
        Me.textBox30.Name = "textBox30"
        Me.textBox30.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.6796832084655762R), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985R))
        Me.textBox30.Value = "=Fields.SalesFirstName + "" "" + Fields.SalesLastName"
        '
        'textBox29
        '
        Me.textBox29.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.5756745338439941R), Telerik.Reporting.Drawing.Unit.Inch(0.34261831641197205R))
        Me.textBox29.Name = "textBox29"
        Me.textBox29.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.3790086507797241R), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985R))
        Me.textBox29.Value = "=Fields.PurchaseOrderNumber"
        '
        'textBox28
        '
        Me.textBox28.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.9547619819641113R), Telerik.Reporting.Drawing.Unit.Inch(0.34261831641197205R))
        Me.textBox28.Name = "textBox28"
        Me.textBox28.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1450397968292236R), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985R))
        Me.textBox28.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.textBox28.Value = "=Fields.ShipMethod"
        '
        'textBox32
        '
        Me.textBox32.Format = "{0:d}"
        Me.textBox32.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.1396620273590088R), Telerik.Reporting.Drawing.Unit.Inch(0.34261831641197205R))
        Me.textBox32.Name = "textBox32"
        Me.textBox32.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7561712265014648R), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985R))
        Me.textBox32.Value = "=Fields.OrderDate"
        '
        'textBox31
        '
        Me.textBox31.Format = "{0:d}"
        Me.textBox31.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.000039339065551757813R), Telerik.Reporting.Drawing.Unit.Inch(0.34261831641197205R))
        Me.textBox31.Name = "textBox31"
        Me.textBox31.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.1395833492279053R), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985R))
        Me.textBox31.Value = "=Now()"
        '
        'panel2
        '
        Me.panel2.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.textBox7})
        Me.panel2.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.000039418537198798731R))
        Me.panel2.Name = "panel2"
        Me.panel2.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.0998411178588867R), Telerik.Reporting.Drawing.Unit.Inch(0.45000001788139343R))
        Me.panel2.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.panel2.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Point(0.5R)
        '
        'textBox7
        '
        Me.textBox7.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.0R))
        Me.textBox7.Name = "textBox7"
        Me.textBox7.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.9041273593902588R), Telerik.Reporting.Drawing.Unit.Inch(0.44999998807907104R))
        Me.textBox7.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.textBox7.StyleName = "Header"
        Me.textBox7.Value = "Customer Details"
        '
        'textBox2
        '
        Me.textBox2.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.1311321258544922R), Telerik.Reporting.Drawing.Unit.Inch(0.60829383134841919R))
        Me.textBox2.Name = "textBox2"
        Me.textBox2.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.66167718172073364R), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985R))
        Me.textBox2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        Me.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.textBox2.Value = "ADDRESS:"
        '
        'textBox3
        '
        Me.textBox3.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.35480260848999R), Telerik.Reporting.Drawing.Unit.Inch(0.57831794023513794R))
        Me.textBox3.Name = "textBox3"
        Me.textBox3.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.56537884473800659R), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985R))
        Me.textBox3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        Me.textBox3.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.textBox3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Top
        Me.textBox3.Value = "SHIP TO:"
        '
        'subReport1
        '
        Me.subReport1.KeepTogether = False
        Me.subReport1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.010495503433048725R), Telerik.Reporting.Drawing.Unit.Inch(2.865025520324707R))
        Me.subReport1.Name = "subReport1"
        TypeReportSource1.Parameters.Add(New Telerik.Reporting.Parameter("SaledOrderID", "=Fields.SalesOrderID"))
        TypeReportSource1.TypeName = "SalesOrderDetails, VB.ReportLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyT" & _
    "oken=null"
        Me.subReport1.ReportSource = TypeReportSource1
        Me.subReport1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.0893068313598633R), Telerik.Reporting.Drawing.Unit.Inch(0.27500006556510925R))
        '
        'InvoicesDataSource
        '
        Me.InvoicesDataSource.ConnectionString = "My.MySettings.TelerikConnectionString"
        Me.InvoicesDataSource.Name = "Invoices"
        Me.InvoicesDataSource.Parameters.AddRange(New Telerik.Reporting.SqlDataSourceParameter() {New Telerik.Reporting.SqlDataSourceParameter("@SalesOrderNumber", System.Data.DbType.[String], "=Parameters.OrderNumber.Value")})
        Me.InvoicesDataSource.SelectCommand = resources.GetString("InvoicesDataSource.SelectCommand")
        '
        'pageFooterSection1
        '
        Me.pageFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.2291666716337204R)
        Me.pageFooterSection1.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.textBox5, Me.textBox22})
        Me.pageFooterSection1.Name = "pageFooterSection1"
        Me.pageFooterSection1.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        '
        'textBox5
        '
        Me.textBox5.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.0R))
        Me.textBox5.Name = "textBox5"
        Me.textBox5.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8958332538604736R), Telerik.Reporting.Drawing.Unit.Inch(0.22912724316120148R))
        Me.textBox5.Value = "=PageExec(""lineTotalDataTextBox"",Count(Fields.ProductNumber)) + "" products, "" + P" & _
    "ageExec(""lineTotalDataTextBox"",Sum(Fields.OrderQty)) + "" items on page  "" + Page" & _
    "Number"
        '
        'textBox22
        '
        Me.textBox22.Format = "{0:C2}"
        Me.textBox22.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.9685325622558594R), Telerik.Reporting.Drawing.Unit.Inch(0.000039418537198798731R))
        Me.textBox22.Name = "textBox22"
        Me.textBox22.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1312694549560547R), Telerik.Reporting.Drawing.Unit.Inch(0.22908782958984375R))
        Me.textBox22.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.textBox22.Value = "=PageExec(""lineTotalDataTextBox"",sum(LineTotal))"
        '
        'Invoice
        '
        Me.DataSource = Me.InvoicesDataSource
        Me.Filters.Add(New Telerik.Reporting.Filter("=Fields.SalesOrderNumber", Telerik.Reporting.FilterOperator.Equal, "=Parameters.OrderNumber.Value"))
        Group1.GroupFooter = Me.groupFooterSection1
        Group1.GroupHeader = Me.groupHeaderSection1
        Group1.Name = "group1"
        Me.Groups.AddRange(New Telerik.Reporting.Group() {Group1})
        Me.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.groupHeaderSection1, Me.groupFooterSection1, Me.detail, Me.pageFooterSection1})
        Me.Name = "Invoice"
        Me.PageSettings.Landscape = False
        Me.PageSettings.Margins = New Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224R), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224R), Telerik.Reporting.Drawing.Unit.Inch(0.30000001192092896R), Telerik.Reporting.Drawing.Unit.Inch(0.40000000596046448R))
        Me.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Letter
        TextWatermark1.Color = System.Drawing.Color.Black
        TextWatermark1.Font.Bold = False
        TextWatermark1.Font.Italic = False
        TextWatermark1.Font.Name = "Tahoma"
        TextWatermark1.Font.Size = Telerik.Reporting.Drawing.Unit.Point(150.0R)
        TextWatermark1.Font.Strikeout = False
        TextWatermark1.Font.Underline = False
        TextWatermark1.Opacity = 0.02R
        TextWatermark1.Orientation = Telerik.Reporting.Drawing.WatermarkOrientation.Diagonal
        TextWatermark1.Position = Telerik.Reporting.Drawing.WatermarkPosition.Front
        TextWatermark1.PrintOnFirstPage = True
        TextWatermark1.PrintOnLastPage = True
        TextWatermark1.Text = "Original"
        Me.PageSettings.Watermarks.Add(TextWatermark1)
        ReportParameter1.AutoRefresh = True
        ReportParameter1.AvailableValues.DataSource = Me.OrderNumbersDataSource
        ReportParameter1.AvailableValues.DisplayMember = "SalesOrderNumber"
        ReportParameter1.AvailableValues.Sortings.Add(New Telerik.Reporting.Sorting("=Fields.SalesOrderNumber", Telerik.Reporting.SortDirection.Asc))
        ReportParameter1.AvailableValues.ValueMember = "SalesOrderNumber"
        ReportParameter1.Name = "OrderNumber"
        ReportParameter1.Text = "Order #"
        ReportParameter1.Value = "=First(Fields.SalesOrderNumber)"
        ReportParameter1.Visible = True
        ReportParameter2.Name = "ForYear"
        ReportParameter2.Type = Telerik.Reporting.ReportParameterType.[Integer]
        ReportParameter2.Value = "=2003"
        ReportParameter3.Name = "ForMonth"
        ReportParameter3.Type = Telerik.Reporting.ReportParameterType.[Integer]
        ReportParameter3.Value = "=7"
        Me.ReportParameters.Add(ReportParameter1)
        Me.ReportParameters.Add(ReportParameter2)
        Me.ReportParameters.Add(ReportParameter3)
        Me.Style.BackgroundColor = System.Drawing.Color.White
        StyleRule1.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.TextItemBase)), New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.HtmlTextBox))})
        StyleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        StyleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        StyleRule2.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.TextBox))})
        StyleRule2.Style.Font.Name = "Segoe UI"
        StyleRule2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10.0R)
        StyleRule3.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.StyleSelector("Header")})
        StyleRule3.Style.Font.Bold = True
        StyleRule3.Style.Font.Name = "Segoe UI Light"
        StyleRule3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(16.0R)
        StyleRule3.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.StyleSheet.AddRange(New Telerik.Reporting.Drawing.StyleRule() {StyleRule1, StyleRule2, StyleRule3})
        Me.Width = Telerik.Reporting.Drawing.Unit.Inch(8.09999942779541R)
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
#End Region

    Private detail As Telerik.Reporting.DetailSection
    Private subReport1 As SubReport
    Private textBox11 As Telerik.Reporting.TextBox
    Private textBox10 As Telerik.Reporting.TextBox
    Private textBox9 As Telerik.Reporting.TextBox
    Private textBox8 As Telerik.Reporting.TextBox
    Private textBox7 As Telerik.Reporting.TextBox
    Private textBox6 As Telerik.Reporting.TextBox
    Private textBox4 As Telerik.Reporting.TextBox
    Private textBox12 As Telerik.Reporting.TextBox
    Private textBox13 As Telerik.Reporting.TextBox
    Private textBox14 As Telerik.Reporting.TextBox
    Private textBox16 As Telerik.Reporting.TextBox
    Private textBox17 As Telerik.Reporting.TextBox
    Private textBox18 As Telerik.Reporting.TextBox
    Private textBox19 As Telerik.Reporting.TextBox
    Private textBox21 As Telerik.Reporting.TextBox
    Private textBox23 As Telerik.Reporting.TextBox
    Private textBox24 As Telerik.Reporting.TextBox
    Private textBox25 As Telerik.Reporting.TextBox
    Private textBox26 As Telerik.Reporting.TextBox
    Private textBox27 As Telerik.Reporting.TextBox
    Private textBox28 As Telerik.Reporting.TextBox
    Private textBox29 As Telerik.Reporting.TextBox
    Private textBox30 As Telerik.Reporting.TextBox
    Private textBox31 As Telerik.Reporting.TextBox
    Private textBox32 As Telerik.Reporting.TextBox
    Private panel1 As Telerik.Reporting.Panel
    Private group1 As Group
    Private groupFooterSection1 As GroupFooterSection
    Private groupHeaderSection1 As GroupHeaderSection
    Private panel5 As Telerik.Reporting.Panel
    Private Text1 As Telerik.Reporting.TextBox
    Private Field6 As Telerik.Reporting.TextBox
    Private Text8 As Telerik.Reporting.TextBox
    Private Id1 As Telerik.Reporting.TextBox
    Private barcode2 As Barcode
    Private panel2 As Telerik.Reporting.Panel
    Private pictureBox1 As Telerik.Reporting.PictureBox
    Private textBox1 As Telerik.Reporting.TextBox
    Private panel3 As Telerik.Reporting.Panel
    Private panel4 As Telerik.Reporting.Panel
    Private textBox2 As Telerik.Reporting.TextBox
    Private textBox3 As Telerik.Reporting.TextBox
    Private InvoicesDataSource As SqlDataSource
    Private OrderNumbersDataSource As SqlDataSource
    Private pageFooterSection1 As PageFooterSection
    Private textBox5 As Telerik.Reporting.TextBox
    Private textBox22 As Telerik.Reporting.TextBox
    Private shape3 As Shape
End Class