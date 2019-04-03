
Imports Telerik.Reporting
Imports Telerik.Reporting.Barcodes

Partial Class BarcodesReport
#Region "Component Designer generated code"
    ''' <summary>
    ''' Required method for Telerik Reporting designer support - do not modify
    ''' the contents of this method with the code editor.
    ''' </summary>
    Private Sub InitializeComponent()
        Dim TableGroup1 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup2 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup3 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup4 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup5 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup6 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup7 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup8 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup9 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup10 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup11 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup12 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup13 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup14 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup15 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup16 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup17 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup18 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup19 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup20 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup21 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup22 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim Code93Encoder1 As Telerik.Reporting.Barcodes.Code93Encoder = New Telerik.Reporting.Barcodes.Code93Encoder()
        Dim Code93ExtendedEncoder1 As Telerik.Reporting.Barcodes.Code93ExtendedEncoder = New Telerik.Reporting.Barcodes.Code93ExtendedEncoder()
        Dim Code128Encoder1 As Telerik.Reporting.Barcodes.Code128Encoder = New Telerik.Reporting.Barcodes.Code128Encoder()
        Dim Code128AEncoder1 As Telerik.Reporting.Barcodes.Code128AEncoder = New Telerik.Reporting.Barcodes.Code128AEncoder()
        Dim Code128BEncoder1 As Telerik.Reporting.Barcodes.Code128BEncoder = New Telerik.Reporting.Barcodes.Code128BEncoder()
        Dim Code128CEncoder1 As Telerik.Reporting.Barcodes.Code128CEncoder = New Telerik.Reporting.Barcodes.Code128CEncoder()
        Dim CodeMSIEncoder1 As Telerik.Reporting.Barcodes.CodeMSIEncoder = New Telerik.Reporting.Barcodes.CodeMSIEncoder()
        Dim EaN8Encoder1 As Telerik.Reporting.Barcodes.EAN8Encoder = New Telerik.Reporting.Barcodes.EAN8Encoder()
        Dim EaN13Encoder1 As Telerik.Reporting.Barcodes.EAN13Encoder = New Telerik.Reporting.Barcodes.EAN13Encoder()
        Dim EaN128Encoder1 As Telerik.Reporting.Barcodes.EAN128Encoder = New Telerik.Reporting.Barcodes.EAN128Encoder()
        Dim PostnetEncoder1 As Telerik.Reporting.Barcodes.PostnetEncoder = New Telerik.Reporting.Barcodes.PostnetEncoder()
        Dim UpcaEncoder1 As Telerik.Reporting.Barcodes.UPCAEncoder = New Telerik.Reporting.Barcodes.UPCAEncoder()
        Dim Code25InterleavedEncoder1 As Telerik.Reporting.Barcodes.Code25InterleavedEncoder = New Telerik.Reporting.Barcodes.Code25InterleavedEncoder()
        Dim Code39Encoder1 As Telerik.Reporting.Barcodes.Code39Encoder = New Telerik.Reporting.Barcodes.Code39Encoder()
        Dim Code39ExtendedEncoder1 As Telerik.Reporting.Barcodes.Code39ExtendedEncoder = New Telerik.Reporting.Barcodes.Code39ExtendedEncoder()
        Dim CodabarEncoder1 As Telerik.Reporting.Barcodes.CodabarEncoder = New Telerik.Reporting.Barcodes.CodabarEncoder()
        Dim Code11Encoder1 As Telerik.Reporting.Barcodes.Code11Encoder = New Telerik.Reporting.Barcodes.Code11Encoder()
        Dim Code25StandardEncoder1 As Telerik.Reporting.Barcodes.Code25StandardEncoder = New Telerik.Reporting.Barcodes.Code25StandardEncoder()
        Dim UpceEncoder1 As Telerik.Reporting.Barcodes.UPCEEncoder = New Telerik.Reporting.Barcodes.UPCEEncoder()
        Dim UpcSupplement2Encoder1 As Telerik.Reporting.Barcodes.UPCSupplement2Encoder = New Telerik.Reporting.Barcodes.UPCSupplement2Encoder()
        Dim UpcSupplement5Encoder1 As Telerik.Reporting.Barcodes.UPCSupplement5Encoder = New Telerik.Reporting.Barcodes.UPCSupplement5Encoder()
        Dim QrCodeEncoder1 As Telerik.Reporting.Barcodes.QRCodeEncoder = New Telerik.Reporting.Barcodes.QRCodeEncoder()
        Dim QrCodeEncoder2 As Telerik.Reporting.Barcodes.QRCodeEncoder = New Telerik.Reporting.Barcodes.QRCodeEncoder()
        Dim PdF417Encoder1 As Telerik.Reporting.Barcodes.PDF417Encoder = New Telerik.Reporting.Barcodes.PDF417Encoder()
        Dim StyleRule1 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule2 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule3 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule4 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Me.detail = New Telerik.Reporting.DetailSection()
        Me.table1 = New Telerik.Reporting.Table()
        Me.textBox22 = New Telerik.Reporting.TextBox()
        Me.textBox23 = New Telerik.Reporting.TextBox()
        Me.textBox24 = New Telerik.Reporting.TextBox()
        Me.textBox12 = New Telerik.Reporting.TextBox()
        Me.textBox11 = New Telerik.Reporting.TextBox()
        Me.textBox10 = New Telerik.Reporting.TextBox()
        Me.barcode7 = New Telerik.Reporting.Barcode()
        Me.barcode8 = New Telerik.Reporting.Barcode()
        Me.barcode9 = New Telerik.Reporting.Barcode()
        Me.textBox18 = New Telerik.Reporting.TextBox()
        Me.textBox17 = New Telerik.Reporting.TextBox()
        Me.textBox16 = New Telerik.Reporting.TextBox()
        Me.barcode12 = New Telerik.Reporting.Barcode()
        Me.barcode11 = New Telerik.Reporting.Barcode()
        Me.barcode10 = New Telerik.Reporting.Barcode()
        Me.textBox15 = New Telerik.Reporting.TextBox()
        Me.textBox14 = New Telerik.Reporting.TextBox()
        Me.textBox13 = New Telerik.Reporting.TextBox()
        Me.textBox33 = New Telerik.Reporting.TextBox()
        Me.textBox32 = New Telerik.Reporting.TextBox()
        Me.textBox31 = New Telerik.Reporting.TextBox()
        Me.barcode13 = New Telerik.Reporting.Barcode()
        Me.barcode14 = New Telerik.Reporting.Barcode()
        Me.barcode15 = New Telerik.Reporting.Barcode()
        Me.textBox21 = New Telerik.Reporting.TextBox()
        Me.textBox20 = New Telerik.Reporting.TextBox()
        Me.textBox19 = New Telerik.Reporting.TextBox()
        Me.barcode18 = New Telerik.Reporting.Barcode()
        Me.barcode17 = New Telerik.Reporting.Barcode()
        Me.barcode16 = New Telerik.Reporting.Barcode()
        Me.textBox30 = New Telerik.Reporting.TextBox()
        Me.textBox29 = New Telerik.Reporting.TextBox()
        Me.textBox28 = New Telerik.Reporting.TextBox()
        Me.barcode6 = New Telerik.Reporting.Barcode()
        Me.barcode5 = New Telerik.Reporting.Barcode()
        Me.barcode4 = New Telerik.Reporting.Barcode()
        Me.barcode1 = New Telerik.Reporting.Barcode()
        Me.barcode2 = New Telerik.Reporting.Barcode()
        Me.barcode3 = New Telerik.Reporting.Barcode()
        Me.barcode19 = New Telerik.Reporting.Barcode()
        Me.barcode20 = New Telerik.Reporting.Barcode()
        Me.barcode21 = New Telerik.Reporting.Barcode()
        Me.panel1 = New Telerik.Reporting.Panel()
        Me.shape3 = New Telerik.Reporting.Shape()
        Me.textBoxReportEmployee = New Telerik.Reporting.TextBox()
        Me.textBox1 = New Telerik.Reporting.TextBox()
        Me.textBox2 = New Telerik.Reporting.TextBox()
        Me.textBox3 = New Telerik.Reporting.TextBox()
        Me.textBox4 = New Telerik.Reporting.TextBox()
        Me.textBox5 = New Telerik.Reporting.TextBox()
        Me.textBox6 = New Telerik.Reporting.TextBox()
        Me.textBox7 = New Telerik.Reporting.TextBox()
        Me.textBox8 = New Telerik.Reporting.TextBox()
        Me.textBox9 = New Telerik.Reporting.TextBox()
        Me.textBox25 = New Telerik.Reporting.TextBox()
        Me.textBox26 = New Telerik.Reporting.TextBox()
        Me.textBox27 = New Telerik.Reporting.TextBox()
        Me.textBox34 = New Telerik.Reporting.TextBox()
        Me.textBox35 = New Telerik.Reporting.TextBox()
        Me.textBox36 = New Telerik.Reporting.TextBox()
        Me.textBox37 = New Telerik.Reporting.TextBox()
        Me.textBox38 = New Telerik.Reporting.TextBox()
        Me.textBox39 = New Telerik.Reporting.TextBox()
        Me.textBox40 = New Telerik.Reporting.TextBox()
        Me.textBox41 = New Telerik.Reporting.TextBox()
        Me.textBox42 = New Telerik.Reporting.TextBox()
        Me.textBox43 = New Telerik.Reporting.TextBox()
        Me.textBox44 = New Telerik.Reporting.TextBox()
        Me.textBox45 = New Telerik.Reporting.TextBox()
        Me.textBox46 = New Telerik.Reporting.TextBox()
        Me.textBox47 = New Telerik.Reporting.TextBox()
        Me.textBox48 = New Telerik.Reporting.TextBox()
        Me.textBox49 = New Telerik.Reporting.TextBox()
        Me.TextBox51 = New Telerik.Reporting.TextBox()
        Me.TextBox53 = New Telerik.Reporting.TextBox()
        Me.TextBox56 = New Telerik.Reporting.TextBox()
        Me.TextBox58 = New Telerik.Reporting.TextBox()
        Me.TextBox60 = New Telerik.Reporting.TextBox()
        Me.TextBox55 = New Telerik.Reporting.TextBox()
        Me.TextBox57 = New Telerik.Reporting.TextBox()
        Me.Barcode22 = New Telerik.Reporting.Barcode()
        Me.Barcode23 = New Telerik.Reporting.Barcode()
        Me.Barcode24 = New Telerik.Reporting.Barcode()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'detail
        '
        Me.detail.Height = Telerik.Reporting.Drawing.Unit.Mm(283.32638549804687R)
        Me.detail.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.table1})
        Me.detail.KeepTogether = False
        Me.detail.Name = "detail"
        '
        'table1
        '
        Me.table1.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(5.8000006675720215R)))
        Me.table1.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(1.0000001192092896R)))
        Me.table1.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(5.8000006675720215R)))
        Me.table1.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(1.0000001192092896R)))
        Me.table1.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(5.8000006675720215R)))
        Me.table1.Body.Rows.Add(New Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(1.7325024604797363R)))
        Me.table1.Body.Rows.Add(New Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(1.0000001192092896R)))
        Me.table1.Body.Rows.Add(New Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(2.5R)))
        Me.table1.Body.Rows.Add(New Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.75000005960464478R)))
        Me.table1.Body.Rows.Add(New Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(2.5000007152557373R)))
        Me.table1.Body.Rows.Add(New Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.75000005960464478R)))
        Me.table1.Body.Rows.Add(New Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(2.5000007152557373R)))
        Me.table1.Body.Rows.Add(New Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.75000005960464478R)))
        Me.table1.Body.Rows.Add(New Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(2.5000007152557373R)))
        Me.table1.Body.Rows.Add(New Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.75000005960464478R)))
        Me.table1.Body.Rows.Add(New Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(2.5000007152557373R)))
        Me.table1.Body.Rows.Add(New Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.75000005960464478R)))
        Me.table1.Body.Rows.Add(New Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(2.5000007152557373R)))
        Me.table1.Body.Rows.Add(New Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.75000041723251343R)))
        Me.table1.Body.Rows.Add(New Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(2.5000007152557373R)))
        Me.table1.Body.Rows.Add(New Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.75000005960464478R)))
        Me.table1.Body.Rows.Add(New Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(2.5000007152557373R)))
        Me.table1.Body.SetCellContent(3, 0, Me.textBox22)
        Me.table1.Body.SetCellContent(3, 2, Me.textBox23)
        Me.table1.Body.SetCellContent(3, 4, Me.textBox24)
        Me.table1.Body.SetCellContent(5, 4, Me.textBox12)
        Me.table1.Body.SetCellContent(5, 2, Me.textBox11)
        Me.table1.Body.SetCellContent(5, 0, Me.textBox10)
        Me.table1.Body.SetCellContent(8, 0, Me.barcode7)
        Me.table1.Body.SetCellContent(8, 2, Me.barcode8)
        Me.table1.Body.SetCellContent(8, 4, Me.barcode9)
        Me.table1.Body.SetCellContent(7, 4, Me.textBox18)
        Me.table1.Body.SetCellContent(7, 2, Me.textBox17)
        Me.table1.Body.SetCellContent(7, 0, Me.textBox16)
        Me.table1.Body.SetCellContent(10, 0, Me.barcode12)
        Me.table1.Body.SetCellContent(10, 2, Me.barcode11)
        Me.table1.Body.SetCellContent(10, 4, Me.barcode10)
        Me.table1.Body.SetCellContent(9, 4, Me.textBox15)
        Me.table1.Body.SetCellContent(9, 2, Me.textBox14)
        Me.table1.Body.SetCellContent(9, 0, Me.textBox13)
        Me.table1.Body.SetCellContent(11, 4, Me.textBox33)
        Me.table1.Body.SetCellContent(11, 2, Me.textBox32)
        Me.table1.Body.SetCellContent(11, 0, Me.textBox31)
        Me.table1.Body.SetCellContent(12, 0, Me.barcode13)
        Me.table1.Body.SetCellContent(12, 2, Me.barcode14)
        Me.table1.Body.SetCellContent(12, 4, Me.barcode15)
        Me.table1.Body.SetCellContent(13, 4, Me.textBox21)
        Me.table1.Body.SetCellContent(13, 2, Me.textBox20)
        Me.table1.Body.SetCellContent(13, 0, Me.textBox19)
        Me.table1.Body.SetCellContent(14, 0, Me.barcode18)
        Me.table1.Body.SetCellContent(14, 2, Me.barcode17)
        Me.table1.Body.SetCellContent(14, 4, Me.barcode16)
        Me.table1.Body.SetCellContent(15, 4, Me.textBox30)
        Me.table1.Body.SetCellContent(15, 2, Me.textBox29)
        Me.table1.Body.SetCellContent(15, 0, Me.textBox28)
        Me.table1.Body.SetCellContent(6, 0, Me.barcode6)
        Me.table1.Body.SetCellContent(6, 2, Me.barcode5)
        Me.table1.Body.SetCellContent(6, 4, Me.barcode4)
        Me.table1.Body.SetCellContent(4, 0, Me.barcode1)
        Me.table1.Body.SetCellContent(4, 2, Me.barcode2)
        Me.table1.Body.SetCellContent(4, 4, Me.barcode3)
        Me.table1.Body.SetCellContent(16, 0, Me.barcode19)
        Me.table1.Body.SetCellContent(16, 2, Me.barcode20)
        Me.table1.Body.SetCellContent(16, 4, Me.barcode21)
        Me.table1.Body.SetCellContent(0, 0, Me.panel1, 1, 5)
        Me.table1.Body.SetCellContent(3, 1, Me.textBox1)
        Me.table1.Body.SetCellContent(4, 1, Me.textBox2)
        Me.table1.Body.SetCellContent(5, 1, Me.textBox3)
        Me.table1.Body.SetCellContent(6, 1, Me.textBox4)
        Me.table1.Body.SetCellContent(7, 1, Me.textBox5)
        Me.table1.Body.SetCellContent(8, 1, Me.textBox6)
        Me.table1.Body.SetCellContent(9, 1, Me.textBox7)
        Me.table1.Body.SetCellContent(10, 1, Me.textBox8)
        Me.table1.Body.SetCellContent(11, 1, Me.textBox9)
        Me.table1.Body.SetCellContent(12, 1, Me.textBox25)
        Me.table1.Body.SetCellContent(13, 1, Me.textBox26)
        Me.table1.Body.SetCellContent(14, 1, Me.textBox27)
        Me.table1.Body.SetCellContent(15, 1, Me.textBox34)
        Me.table1.Body.SetCellContent(16, 1, Me.textBox35)
        Me.table1.Body.SetCellContent(3, 3, Me.textBox36)
        Me.table1.Body.SetCellContent(4, 3, Me.textBox37)
        Me.table1.Body.SetCellContent(5, 3, Me.textBox38)
        Me.table1.Body.SetCellContent(6, 3, Me.textBox39)
        Me.table1.Body.SetCellContent(7, 3, Me.textBox40)
        Me.table1.Body.SetCellContent(8, 3, Me.textBox41)
        Me.table1.Body.SetCellContent(9, 3, Me.textBox42)
        Me.table1.Body.SetCellContent(10, 3, Me.textBox43)
        Me.table1.Body.SetCellContent(11, 3, Me.textBox44)
        Me.table1.Body.SetCellContent(12, 3, Me.textBox45)
        Me.table1.Body.SetCellContent(13, 3, Me.textBox46)
        Me.table1.Body.SetCellContent(14, 3, Me.textBox47)
        Me.table1.Body.SetCellContent(15, 3, Me.textBox48)
        Me.table1.Body.SetCellContent(16, 3, Me.textBox49)
        Me.table1.Body.SetCellContent(2, 1, Me.TextBox51)
        Me.table1.Body.SetCellContent(2, 3, Me.TextBox53)
        Me.table1.Body.SetCellContent(1, 1, Me.TextBox56)
        Me.table1.Body.SetCellContent(1, 3, Me.TextBox58)
        Me.table1.Body.SetCellContent(1, 0, Me.TextBox60)
        Me.table1.Body.SetCellContent(1, 2, Me.TextBox55)
        Me.table1.Body.SetCellContent(1, 4, Me.TextBox57)
        Me.table1.Body.SetCellContent(2, 0, Me.Barcode22)
        Me.table1.Body.SetCellContent(2, 2, Me.Barcode23)
        Me.table1.Body.SetCellContent(2, 4, Me.Barcode24)
        TableGroup2.Name = "Group2"
        TableGroup4.Name = "Group3"
        Me.table1.ColumnGroups.Add(TableGroup1)
        Me.table1.ColumnGroups.Add(TableGroup2)
        Me.table1.ColumnGroups.Add(TableGroup3)
        Me.table1.ColumnGroups.Add(TableGroup4)
        Me.table1.ColumnGroups.Add(TableGroup5)
        Me.table1.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.textBox22, Me.textBox23, Me.textBox24, Me.textBox12, Me.textBox11, Me.textBox10, Me.barcode7, Me.barcode8, Me.barcode9, Me.textBox18, Me.textBox17, Me.textBox16, Me.barcode12, Me.barcode11, Me.barcode10, Me.textBox15, Me.textBox14, Me.textBox13, Me.textBox33, Me.textBox32, Me.textBox31, Me.barcode13, Me.barcode14, Me.barcode15, Me.textBox21, Me.textBox20, Me.textBox19, Me.barcode18, Me.barcode17, Me.barcode16, Me.textBox30, Me.textBox29, Me.textBox28, Me.barcode6, Me.barcode5, Me.barcode4, Me.barcode1, Me.barcode2, Me.barcode3, Me.barcode19, Me.barcode20, Me.barcode21, Me.panel1, Me.textBox1, Me.textBox2, Me.textBox3, Me.textBox4, Me.textBox5, Me.textBox6, Me.textBox7, Me.textBox8, Me.textBox9, Me.textBox25, Me.textBox26, Me.textBox27, Me.textBox34, Me.textBox35, Me.textBox36, Me.textBox37, Me.textBox38, Me.textBox39, Me.textBox40, Me.textBox41, Me.textBox42, Me.textBox43, Me.textBox44, Me.textBox45, Me.textBox46, Me.textBox47, Me.textBox48, Me.textBox49, Me.TextBox51, Me.TextBox53, Me.TextBox56, Me.TextBox58, Me.TextBox60, Me.TextBox55, Me.TextBox57, Me.Barcode22, Me.Barcode23, Me.Barcode24})
        Me.table1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(0.00099921226501464844R), Telerik.Reporting.Drawing.Unit.Mm(0.0010011057602241635R))
        Me.table1.Name = "table1"
        TableGroup6.Name = "Group1"
        TableGroup7.Name = "group1"
        TableGroup8.Name = "group"
        Me.table1.RowGroups.Add(TableGroup6)
        Me.table1.RowGroups.Add(TableGroup7)
        Me.table1.RowGroups.Add(TableGroup8)
        Me.table1.RowGroups.Add(TableGroup9)
        Me.table1.RowGroups.Add(TableGroup10)
        Me.table1.RowGroups.Add(TableGroup11)
        Me.table1.RowGroups.Add(TableGroup12)
        Me.table1.RowGroups.Add(TableGroup13)
        Me.table1.RowGroups.Add(TableGroup14)
        Me.table1.RowGroups.Add(TableGroup15)
        Me.table1.RowGroups.Add(TableGroup16)
        Me.table1.RowGroups.Add(TableGroup17)
        Me.table1.RowGroups.Add(TableGroup18)
        Me.table1.RowGroups.Add(TableGroup19)
        Me.table1.RowGroups.Add(TableGroup20)
        Me.table1.RowGroups.Add(TableGroup21)
        Me.table1.RowGroups.Add(TableGroup22)
        Me.table1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(19.400001525878906R), Telerik.Reporting.Drawing.Unit.Cm(27.982505798339844R))
        '
        'textBox22
        '
        Me.textBox22.Name = "textBox22"
        Me.textBox22.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000001907348633R), Telerik.Reporting.Drawing.Unit.Cm(0.75R))
        Me.textBox22.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None
        Me.textBox22.Value = "Codabar"
        '
        'textBox23
        '
        Me.textBox23.Name = "textBox23"
        Me.textBox23.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000001907348633R), Telerik.Reporting.Drawing.Unit.Cm(0.75R))
        Me.textBox23.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None
        Me.textBox23.Value = "Code11"
        '
        'textBox24
        '
        Me.textBox24.Name = "textBox24"
        Me.textBox24.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000001907348633R), Telerik.Reporting.Drawing.Unit.Cm(0.75R))
        Me.textBox24.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None
        Me.textBox24.Value = "Code25Standard"
        '
        'textBox12
        '
        Me.textBox12.Name = "textBox12"
        Me.textBox12.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000001907348633R), Telerik.Reporting.Drawing.Unit.Cm(0.75000005960464478R))
        Me.textBox12.Value = "Code39Extended"
        '
        'textBox11
        '
        Me.textBox11.Name = "textBox11"
        Me.textBox11.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000001907348633R), Telerik.Reporting.Drawing.Unit.Cm(0.75000005960464478R))
        Me.textBox11.Value = "Code39"
        '
        'textBox10
        '
        Me.textBox10.Name = "textBox10"
        Me.textBox10.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000001907348633R), Telerik.Reporting.Drawing.Unit.Cm(0.75000005960464478R))
        Me.textBox10.Value = "Code25Interleaved"
        '
        'barcode7
        '
        Me.barcode7.Encoder = Code93Encoder1
        Me.barcode7.Name = "barcode7"
        Me.barcode7.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000006675720215R), Telerik.Reporting.Drawing.Unit.Cm(2.5R))
        Me.barcode7.Value = "0123456789"
        '
        'barcode8
        '
        Me.barcode8.Encoder = Code93ExtendedEncoder1
        Me.barcode8.Name = "barcode8"
        Me.barcode8.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000006675720215R), Telerik.Reporting.Drawing.Unit.Cm(2.5R))
        Me.barcode8.Value = "0123456789"
        '
        'barcode9
        '
        Me.barcode9.Encoder = Code128Encoder1
        Me.barcode9.Name = "barcode9"
        Me.barcode9.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000006675720215R), Telerik.Reporting.Drawing.Unit.Cm(2.5R))
        Me.barcode9.Value = "0123456789"
        '
        'textBox18
        '
        Me.textBox18.Name = "textBox18"
        Me.textBox18.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000001907348633R), Telerik.Reporting.Drawing.Unit.Cm(0.75R))
        Me.textBox18.Value = "Code128"
        '
        'textBox17
        '
        Me.textBox17.Name = "textBox17"
        Me.textBox17.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000001907348633R), Telerik.Reporting.Drawing.Unit.Cm(0.75R))
        Me.textBox17.Value = "Code93Extended"
        '
        'textBox16
        '
        Me.textBox16.Name = "textBox16"
        Me.textBox16.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000001907348633R), Telerik.Reporting.Drawing.Unit.Cm(0.75R))
        Me.textBox16.Value = "Code93"
        '
        'barcode12
        '
        Me.barcode12.Encoder = Code128AEncoder1
        Me.barcode12.Name = "barcode12"
        Me.barcode12.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000006675720215R), Telerik.Reporting.Drawing.Unit.Cm(2.5R))
        Me.barcode12.Value = "01234567"
        '
        'barcode11
        '
        Me.barcode11.Encoder = Code128BEncoder1
        Me.barcode11.Name = "barcode11"
        Me.barcode11.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000006675720215R), Telerik.Reporting.Drawing.Unit.Cm(2.5R))
        Me.barcode11.Value = "01234567"
        '
        'barcode10
        '
        Me.barcode10.Encoder = Code128CEncoder1
        Me.barcode10.Name = "barcode10"
        Me.barcode10.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000006675720215R), Telerik.Reporting.Drawing.Unit.Cm(2.5R))
        Me.barcode10.Value = "0123456789"
        '
        'textBox15
        '
        Me.textBox15.Name = "textBox15"
        Me.textBox15.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000001907348633R), Telerik.Reporting.Drawing.Unit.Cm(0.75000005960464478R))
        Me.textBox15.Value = "Code128C"
        '
        'textBox14
        '
        Me.textBox14.Name = "textBox14"
        Me.textBox14.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000001907348633R), Telerik.Reporting.Drawing.Unit.Cm(0.75000005960464478R))
        Me.textBox14.Value = "Code128B"
        '
        'textBox13
        '
        Me.textBox13.Name = "textBox13"
        Me.textBox13.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000001907348633R), Telerik.Reporting.Drawing.Unit.Cm(0.75000005960464478R))
        Me.textBox13.Value = "Code128A"
        '
        'textBox33
        '
        Me.textBox33.Name = "textBox33"
        Me.textBox33.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000001907348633R), Telerik.Reporting.Drawing.Unit.Cm(0.75R))
        Me.textBox33.Value = "EAN13"
        '
        'textBox32
        '
        Me.textBox32.Name = "textBox32"
        Me.textBox32.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000001907348633R), Telerik.Reporting.Drawing.Unit.Cm(0.75R))
        Me.textBox32.Value = "EAN8"
        '
        'textBox31
        '
        Me.textBox31.Name = "textBox31"
        Me.textBox31.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000001907348633R), Telerik.Reporting.Drawing.Unit.Cm(0.75R))
        Me.textBox31.Value = "CodeMSI"
        '
        'barcode13
        '
        Me.barcode13.Encoder = CodeMSIEncoder1
        Me.barcode13.Name = "barcode13"
        Me.barcode13.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000006675720215R), Telerik.Reporting.Drawing.Unit.Cm(2.5R))
        Me.barcode13.Value = "012345678"
        '
        'barcode14
        '
        Me.barcode14.Encoder = EaN8Encoder1
        Me.barcode14.Name = "barcode14"
        Me.barcode14.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000006675720215R), Telerik.Reporting.Drawing.Unit.Cm(2.5R))
        Me.barcode14.Value = "0123456"
        '
        'barcode15
        '
        Me.barcode15.Encoder = EaN13Encoder1
        Me.barcode15.Name = "barcode15"
        Me.barcode15.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000006675720215R), Telerik.Reporting.Drawing.Unit.Cm(2.5R))
        Me.barcode15.Value = "012345678912"
        '
        'textBox21
        '
        Me.textBox21.Name = "textBox21"
        Me.textBox21.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000006675720215R), Telerik.Reporting.Drawing.Unit.Cm(0.75R))
        Me.textBox21.Value = "UPCA"
        '
        'textBox20
        '
        Me.textBox20.Name = "textBox20"
        Me.textBox20.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000006675720215R), Telerik.Reporting.Drawing.Unit.Cm(0.75R))
        Me.textBox20.Value = "Postnet"
        '
        'textBox19
        '
        Me.textBox19.Name = "textBox19"
        Me.textBox19.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000006675720215R), Telerik.Reporting.Drawing.Unit.Cm(0.75R))
        Me.textBox19.Value = "EAN128"
        '
        'barcode18
        '
        Me.barcode18.Encoder = EaN128Encoder1
        Me.barcode18.Name = "barcode18"
        Me.barcode18.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000006675720215R), Telerik.Reporting.Drawing.Unit.Cm(2.5R))
        Me.barcode18.Value = "0123456789"
        '
        'barcode17
        '
        Me.barcode17.Encoder = PostnetEncoder1
        Me.barcode17.Name = "barcode17"
        Me.barcode17.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000006675720215R), Telerik.Reporting.Drawing.Unit.Cm(2.5R))
        Me.barcode17.Value = "0123456789"
        '
        'barcode16
        '
        Me.barcode16.Encoder = UpcaEncoder1
        Me.barcode16.Name = "barcode16"
        Me.barcode16.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000006675720215R), Telerik.Reporting.Drawing.Unit.Cm(2.5R))
        Me.barcode16.Value = "0123456789"
        '
        'textBox30
        '
        Me.textBox30.Name = "textBox30"
        Me.textBox30.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000006675720215R), Telerik.Reporting.Drawing.Unit.Cm(0.75R))
        Me.textBox30.Value = "UPCSupplement5"
        '
        'textBox29
        '
        Me.textBox29.Name = "textBox29"
        Me.textBox29.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000006675720215R), Telerik.Reporting.Drawing.Unit.Cm(0.75R))
        Me.textBox29.Value = "UPCSupplement2"
        '
        'textBox28
        '
        Me.textBox28.Name = "textBox28"
        Me.textBox28.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000006675720215R), Telerik.Reporting.Drawing.Unit.Cm(0.75R))
        Me.textBox28.Value = "UPCE"
        '
        'barcode6
        '
        Me.barcode6.Encoder = Code25InterleavedEncoder1
        Me.barcode6.Name = "barcode6"
        Me.barcode6.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000006675720215R), Telerik.Reporting.Drawing.Unit.Cm(2.5R))
        Me.barcode6.Value = "0123456789"
        '
        'barcode5
        '
        Me.barcode5.Encoder = Code39Encoder1
        Me.barcode5.Name = "barcode5"
        Me.barcode5.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000006675720215R), Telerik.Reporting.Drawing.Unit.Cm(2.5R))
        Me.barcode5.Value = "0123456"
        '
        'barcode4
        '
        Me.barcode4.Encoder = Code39ExtendedEncoder1
        Me.barcode4.Name = "barcode4"
        Me.barcode4.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000006675720215R), Telerik.Reporting.Drawing.Unit.Cm(2.5R))
        Me.barcode4.Value = "0123456"
        '
        'barcode1
        '
        Me.barcode1.Encoder = CodabarEncoder1
        Me.barcode1.Name = "barcode1"
        Me.barcode1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000006675720215R), Telerik.Reporting.Drawing.Unit.Cm(2.5R))
        Me.barcode1.Value = "0123456789"
        '
        'barcode2
        '
        Me.barcode2.Encoder = Code11Encoder1
        Me.barcode2.Name = "barcode2"
        Me.barcode2.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000006675720215R), Telerik.Reporting.Drawing.Unit.Cm(2.5R))
        Me.barcode2.Value = "0123456789"
        '
        'barcode3
        '
        Me.barcode3.Encoder = Code25StandardEncoder1
        Me.barcode3.Name = "barcode3"
        Me.barcode3.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000006675720215R), Telerik.Reporting.Drawing.Unit.Cm(2.5R))
        Me.barcode3.Value = "01234567"
        '
        'barcode19
        '
        Me.barcode19.Encoder = UpceEncoder1
        Me.barcode19.Name = "barcode19"
        Me.barcode19.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000001907348633R), Telerik.Reporting.Drawing.Unit.Cm(2.5R))
        Me.barcode19.Value = "1200000789"
        '
        'barcode20
        '
        Me.barcode20.Encoder = UpcSupplement2Encoder1
        Me.barcode20.Name = "barcode20"
        Me.barcode20.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000001907348633R), Telerik.Reporting.Drawing.Unit.Cm(2.5R))
        Me.barcode20.Value = "12"
        '
        'barcode21
        '
        Me.barcode21.Encoder = UpcSupplement5Encoder1
        Me.barcode21.Name = "barcode21"
        Me.barcode21.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000001907348633R), Telerik.Reporting.Drawing.Unit.Cm(2.5R))
        Me.barcode21.Value = "01234"
        '
        'panel1
        '
        Me.panel1.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.shape3, Me.textBoxReportEmployee})
        Me.panel1.Name = "panel1"
        Me.panel1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(194.00001525878906R), Telerik.Reporting.Drawing.Unit.Mm(17.325016021728516R))
        Me.panel1.Style.Font.Bold = True
        '
        'shape3
        '
        Me.shape3.Anchoring = CType((Telerik.Reporting.AnchoringStyles.Left Or Telerik.Reporting.AnchoringStyles.Right), Telerik.Reporting.AnchoringStyles)
        Me.shape3.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.6000787615776062R))
        Me.shape3.Name = "shape3"
        Me.shape3.ShapeType = New Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW)
        Me.shape3.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(193.99900817871094R), Telerik.Reporting.Drawing.Unit.Point(2.0R))
        Me.shape3.Stretch = True
        Me.shape3.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.shape3.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid
        Me.shape3.Style.BorderWidth.Bottom = Telerik.Reporting.Drawing.Unit.Point(0.5R)
        Me.shape3.Style.BorderWidth.Top = Telerik.Reporting.Drawing.Unit.Point(1.5R)
        Me.shape3.Style.Color = System.Drawing.Color.Transparent
        '
        'textBoxReportEmployee
        '
        Me.textBoxReportEmployee.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.0R))
        Me.textBoxReportEmployee.Name = "textBoxReportEmployee"
        Me.textBoxReportEmployee.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(7.63775634765625R), Telerik.Reporting.Drawing.Unit.Inch(0.60000002384185791R))
        Me.textBoxReportEmployee.StyleName = "Header"
        Me.textBoxReportEmployee.Value = "Barcode Types"
        '
        'textBox1
        '
        Me.textBox1.Name = "textBox1"
        Me.textBox1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(10.000000953674316R), Telerik.Reporting.Drawing.Unit.Cm(0.75R))
        '
        'textBox2
        '
        Me.textBox2.Name = "textBox2"
        Me.textBox2.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(10.000000953674316R), Telerik.Reporting.Drawing.Unit.Cm(2.5R))
        '
        'textBox3
        '
        Me.textBox3.Name = "textBox3"
        Me.textBox3.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(10.000000953674316R), Telerik.Reporting.Drawing.Unit.Cm(0.75000005960464478R))
        '
        'textBox4
        '
        Me.textBox4.Name = "textBox4"
        Me.textBox4.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(10.000000953674316R), Telerik.Reporting.Drawing.Unit.Cm(2.5R))
        '
        'textBox5
        '
        Me.textBox5.Name = "textBox5"
        Me.textBox5.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(10.000000953674316R), Telerik.Reporting.Drawing.Unit.Cm(0.75R))
        '
        'textBox6
        '
        Me.textBox6.Name = "textBox6"
        Me.textBox6.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(10.000000953674316R), Telerik.Reporting.Drawing.Unit.Cm(2.5R))
        '
        'textBox7
        '
        Me.textBox7.Name = "textBox7"
        Me.textBox7.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(10.000000953674316R), Telerik.Reporting.Drawing.Unit.Cm(0.75000005960464478R))
        '
        'textBox8
        '
        Me.textBox8.Name = "textBox8"
        Me.textBox8.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(10.000000953674316R), Telerik.Reporting.Drawing.Unit.Cm(2.5R))
        '
        'textBox9
        '
        Me.textBox9.Name = "textBox9"
        Me.textBox9.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(10.000000953674316R), Telerik.Reporting.Drawing.Unit.Cm(0.75R))
        '
        'textBox25
        '
        Me.textBox25.Name = "textBox25"
        Me.textBox25.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(10.000000953674316R), Telerik.Reporting.Drawing.Unit.Cm(2.5R))
        '
        'textBox26
        '
        Me.textBox26.Name = "textBox26"
        Me.textBox26.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(10.000000953674316R), Telerik.Reporting.Drawing.Unit.Cm(0.75R))
        '
        'textBox27
        '
        Me.textBox27.Name = "textBox27"
        Me.textBox27.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(10.000000953674316R), Telerik.Reporting.Drawing.Unit.Cm(2.5R))
        '
        'textBox34
        '
        Me.textBox34.Name = "textBox34"
        Me.textBox34.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(10.000000953674316R), Telerik.Reporting.Drawing.Unit.Cm(0.75R))
        '
        'textBox35
        '
        Me.textBox35.Name = "textBox35"
        Me.textBox35.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(10.000000953674316R), Telerik.Reporting.Drawing.Unit.Cm(2.5R))
        '
        'textBox36
        '
        Me.textBox36.Name = "textBox36"
        Me.textBox36.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(10.000000953674316R), Telerik.Reporting.Drawing.Unit.Cm(0.75R))
        '
        'textBox37
        '
        Me.textBox37.Name = "textBox37"
        Me.textBox37.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(10.000000953674316R), Telerik.Reporting.Drawing.Unit.Cm(2.5R))
        '
        'textBox38
        '
        Me.textBox38.Name = "textBox38"
        Me.textBox38.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(10.000000953674316R), Telerik.Reporting.Drawing.Unit.Cm(0.75000005960464478R))
        '
        'textBox39
        '
        Me.textBox39.Name = "textBox39"
        Me.textBox39.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(10.000000953674316R), Telerik.Reporting.Drawing.Unit.Cm(2.5R))
        '
        'textBox40
        '
        Me.textBox40.Name = "textBox40"
        Me.textBox40.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(10.000000953674316R), Telerik.Reporting.Drawing.Unit.Cm(0.75R))
        '
        'textBox41
        '
        Me.textBox41.Name = "textBox41"
        Me.textBox41.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(10.000000953674316R), Telerik.Reporting.Drawing.Unit.Cm(2.5R))
        '
        'textBox42
        '
        Me.textBox42.Name = "textBox42"
        Me.textBox42.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(10.000000953674316R), Telerik.Reporting.Drawing.Unit.Cm(0.75000005960464478R))
        '
        'textBox43
        '
        Me.textBox43.Name = "textBox43"
        Me.textBox43.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(10.000000953674316R), Telerik.Reporting.Drawing.Unit.Cm(2.5R))
        '
        'textBox44
        '
        Me.textBox44.Name = "textBox44"
        Me.textBox44.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(10.000000953674316R), Telerik.Reporting.Drawing.Unit.Cm(0.75R))
        '
        'textBox45
        '
        Me.textBox45.Name = "textBox45"
        Me.textBox45.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(10.000000953674316R), Telerik.Reporting.Drawing.Unit.Cm(2.5R))
        '
        'textBox46
        '
        Me.textBox46.Name = "textBox46"
        Me.textBox46.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(10.000000953674316R), Telerik.Reporting.Drawing.Unit.Cm(0.75R))
        '
        'textBox47
        '
        Me.textBox47.Name = "textBox47"
        Me.textBox47.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(10.000000953674316R), Telerik.Reporting.Drawing.Unit.Cm(2.5R))
        '
        'textBox48
        '
        Me.textBox48.Name = "textBox48"
        Me.textBox48.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(10.000000953674316R), Telerik.Reporting.Drawing.Unit.Cm(0.75R))
        '
        'textBox49
        '
        Me.textBox49.Name = "textBox49"
        Me.textBox49.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(10.000000953674316R), Telerik.Reporting.Drawing.Unit.Cm(2.5R))
        '
        'TextBox51
        '
        Me.TextBox51.Name = "TextBox51"
        Me.TextBox51.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.0000001192092896R), Telerik.Reporting.Drawing.Unit.Mm(24.999998092651367R))
        Me.TextBox51.StyleName = ""
        '
        'TextBox53
        '
        Me.TextBox53.Name = "TextBox53"
        Me.TextBox53.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.0000001192092896R), Telerik.Reporting.Drawing.Unit.Mm(24.999998092651367R))
        Me.TextBox53.StyleName = ""
        '
        'TextBox56
        '
        Me.TextBox56.Name = "TextBox56"
        Me.TextBox56.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.0000001192092896R), Telerik.Reporting.Drawing.Unit.Mm(10.0R))
        Me.TextBox56.StyleName = ""
        '
        'TextBox58
        '
        Me.TextBox58.Name = "TextBox58"
        Me.TextBox58.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.0000001192092896R), Telerik.Reporting.Drawing.Unit.Mm(10.0R))
        Me.TextBox58.StyleName = ""
        '
        'TextBox60
        '
        Me.TextBox60.Name = "TextBox60"
        Me.TextBox60.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000006675720215R), Telerik.Reporting.Drawing.Unit.Mm(10.0R))
        Me.TextBox60.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None
        Me.TextBox60.Value = "QR Code"
        '
        'TextBox55
        '
        Me.TextBox55.Name = "TextBox55"
        Me.TextBox55.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000006675720215R), Telerik.Reporting.Drawing.Unit.Mm(10.0R))
        Me.TextBox55.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None
        Me.TextBox55.Value = "QR Code"
        '
        'TextBox57
        '
        Me.TextBox57.Name = "TextBox57"
        Me.TextBox57.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000006675720215R), Telerik.Reporting.Drawing.Unit.Mm(10.0R))
        Me.TextBox57.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None
        Me.TextBox57.Value = "PDF417"
        '
        'Barcode22
        '
        QrCodeEncoder1.ErrorCorrectionLevel = Telerik.Reporting.Barcodes.QRCode.ErrorCorrectionLevel.H
        Me.Barcode22.Encoder = QrCodeEncoder1
        Me.Barcode22.Name = "Barcode22"
        Me.Barcode22.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000006675720215R), Telerik.Reporting.Drawing.Unit.Mm(24.999998092651367R))
        Me.Barcode22.Stretch = True
        Me.Barcode22.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Cm(0.0R)
        Me.Barcode22.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Cm(0.0R)
        Me.Barcode22.Value = "http://demos.telerik.com/reporting"
        '
        'Barcode23
        '
        QrCodeEncoder2.Version = 3
        Me.Barcode23.Encoder = QrCodeEncoder2
        Me.Barcode23.Name = "Barcode23"
        Me.Barcode23.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000006675720215R), Telerik.Reporting.Drawing.Unit.Mm(24.999998092651367R))
        Me.Barcode23.Stretch = True
        Me.Barcode23.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Cm(0.0R)
        Me.Barcode23.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Cm(0.0R)
        Me.Barcode23.Value = "http://www.telerik.com"
        '
        'Barcode24
        '
        Me.Barcode24.Encoder = PdF417Encoder1
        Me.Barcode24.Name = "Barcode24"
        Me.Barcode24.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8000006675720215R), Telerik.Reporting.Drawing.Unit.Mm(24.999998092651367R))
        Me.Barcode24.Stretch = True
        Me.Barcode24.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Cm(0.25R)
        Me.Barcode24.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Cm(0.25R)
        Me.Barcode24.Value = "0123456789"
        '
        'BarcodesReport
        '
        Me.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.detail})
        Me.Name = "BarcodesReport"
        Me.PageSettings.Landscape = False
        Me.PageSettings.Margins = New Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0.30000001192092896R), Telerik.Reporting.Drawing.Unit.Inch(0.30000001192092896R), Telerik.Reporting.Drawing.Unit.Inch(0.40000000596046448R), Telerik.Reporting.Drawing.Unit.Inch(0.10000000149011612R))
        Me.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4
        StyleRule1.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.TextItemBase)), New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.HtmlTextBox))})
        StyleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        StyleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        StyleRule2.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.StyleSelector("Header")})
        StyleRule2.Style.Color = System.Drawing.Color.Black
        StyleRule2.Style.Font.Bold = True
        StyleRule2.Style.Font.Name = "Segoe UI Light"
        StyleRule2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(25.0R)
        StyleRule2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left
        StyleRule3.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.TextBox))})
        StyleRule3.Style.BackgroundColor = System.Drawing.Color.Transparent
        StyleRule3.Style.BorderColor.Default = System.Drawing.Color.Black
        StyleRule3.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None
        StyleRule3.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None
        StyleRule3.Style.Color = System.Drawing.Color.FromArgb(CType(CType(177, Byte), Integer), CType(CType(161, Byte), Integer), CType(CType(82, Byte), Integer))
        StyleRule3.Style.Font.Bold = True
        StyleRule3.Style.Font.Name = "Segoe UI Light"
        StyleRule3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(13.0R)
        StyleRule3.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        StyleRule3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        StyleRule4.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.Barcode))})
        StyleRule4.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        StyleRule4.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid
        StyleRule4.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Point(0.5R)
        StyleRule4.Style.Font.Bold = True
        StyleRule4.Style.Font.Name = "Segoe UI"
        StyleRule4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        StyleRule4.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Cm(0.5R)
        StyleRule4.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Cm(0.30000001192092896R)
        StyleRule4.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        StyleRule4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom
        Me.StyleSheet.AddRange(New Telerik.Reporting.Drawing.StyleRule() {StyleRule1, StyleRule2, StyleRule3, StyleRule4})
        Me.UnitOfMeasure = Telerik.Reporting.Drawing.UnitType.Mm
        Me.Width = Telerik.Reporting.Drawing.Unit.Mm(194.75900268554687R)
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
#End Region
    Private detail As DetailSection
    Private barcode1 As Barcode
    Private barcode2 As Barcode
    Private barcode3 As Barcode
    Private barcode4 As Barcode
    Private barcode5 As Barcode
    Private barcode6 As Barcode
    Private barcode7 As Barcode
    Private barcode8 As Barcode
    Private barcode9 As Barcode
    Private barcode10 As Barcode
    Private barcode11 As Barcode
    Private barcode12 As Barcode
    Private barcode13 As Barcode
    Private barcode14 As Barcode
    Private barcode15 As Barcode
    Private barcode16 As Barcode
    Private barcode17 As Barcode
    Private barcode18 As Barcode
    Private barcode19 As Barcode
    Private barcode20 As Barcode
    Private barcode21 As Barcode
    Private table1 As Table
    Private textBox22 As TextBox
    Private textBox23 As TextBox
    Private textBox24 As TextBox
    Private textBox30 As TextBox
    Private textBox29 As TextBox
    Private textBox28 As TextBox
    Private textBox15 As TextBox
    Private textBox14 As TextBox
    Private textBox13 As TextBox
    Private textBox33 As TextBox
    Private textBox32 As TextBox
    Private textBox31 As TextBox
    Private textBox21 As TextBox
    Private textBox20 As TextBox
    Private textBox19 As TextBox
    Private textBox12 As TextBox
    Private textBox11 As TextBox
    Private textBox10 As TextBox
    Private textBox18 As TextBox
    Private textBox17 As TextBox
    Private textBox16 As TextBox
    Private panel1 As Panel
    Private shape3 As Shape
    Private textBoxReportEmployee As TextBox
    Private textBox1 As TextBox
    Private textBox2 As TextBox
    Private textBox3 As TextBox
    Private textBox4 As TextBox
    Private textBox5 As TextBox
    Private textBox6 As TextBox
    Private textBox7 As TextBox
    Private textBox8 As TextBox
    Private textBox9 As TextBox
    Private textBox25 As TextBox
    Private textBox26 As TextBox
    Private textBox27 As TextBox
    Private textBox34 As TextBox
    Private textBox35 As TextBox
    Private textBox36 As TextBox
    Private textBox37 As TextBox
    Private textBox38 As TextBox
    Private textBox39 As TextBox
    Private textBox40 As TextBox
    Private textBox41 As TextBox
    Private textBox42 As TextBox
    Private textBox43 As TextBox
    Private textBox44 As TextBox
    Private textBox45 As TextBox
    Private textBox46 As TextBox
    Private textBox47 As TextBox
    Private textBox48 As TextBox
    Private textBox49 As TextBox
    Friend WithEvents TextBox51 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox53 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox56 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox58 As Telerik.Reporting.TextBox
    Private WithEvents TextBox60 As Telerik.Reporting.TextBox
    Private WithEvents TextBox55 As Telerik.Reporting.TextBox
    Private WithEvents TextBox57 As Telerik.Reporting.TextBox
    Private WithEvents Barcode22 As Telerik.Reporting.Barcode
    Private WithEvents Barcode23 As Telerik.Reporting.Barcode
    Private WithEvents Barcode24 As Telerik.Reporting.Barcode
End Class
