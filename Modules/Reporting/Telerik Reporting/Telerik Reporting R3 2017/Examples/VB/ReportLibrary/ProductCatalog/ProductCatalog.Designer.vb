
Imports System.ComponentModel
Imports System.Drawing
Imports Telerik.Reporting
Imports Telerik.Reporting.Drawing

Partial Class ProductCatalog
#Region "Component Designer generated code"
    ''' <summary>
    ''' Required method for telerik Reporting designer support - do not modify
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
        Dim FormattingRule1 As Telerik.Reporting.Drawing.FormattingRule = New Telerik.Reporting.Drawing.FormattingRule()
        Dim FormattingRule2 As Telerik.Reporting.Drawing.FormattingRule = New Telerik.Reporting.Drawing.FormattingRule()
        Dim FormattingRule3 As Telerik.Reporting.Drawing.FormattingRule = New Telerik.Reporting.Drawing.FormattingRule()
        Dim FormattingRule4 As Telerik.Reporting.Drawing.FormattingRule = New Telerik.Reporting.Drawing.FormattingRule()
        Dim FormattingRule5 As Telerik.Reporting.Drawing.FormattingRule = New Telerik.Reporting.Drawing.FormattingRule()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ProductCatalog))
        Dim TocLevel1 As Telerik.Reporting.TocLevel = New Telerik.Reporting.TocLevel()
        Dim TocLevel2 As Telerik.Reporting.TocLevel = New Telerik.Reporting.TocLevel()
        Dim TocLevel3 As Telerik.Reporting.TocLevel = New Telerik.Reporting.TocLevel()
        Dim Group1 As Telerik.Reporting.Group = New Telerik.Reporting.Group()
        Dim Group2 As Telerik.Reporting.Group = New Telerik.Reporting.Group()
        Dim Group3 As Telerik.Reporting.Group = New Telerik.Reporting.Group()
        Dim TextWatermark1 As Telerik.Reporting.Drawing.TextWatermark = New Telerik.Reporting.Drawing.TextWatermark()
        Dim ReportParameter1 As Telerik.Reporting.ReportParameter = New Telerik.Reporting.ReportParameter()
        Dim StyleRule1 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule2 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule3 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule4 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule5 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim DescendantSelector1 As Telerik.Reporting.Drawing.DescendantSelector = New Telerik.Reporting.Drawing.DescendantSelector()
        Me.textBox5 = New Telerik.Reporting.TextBox()
        Me.textBox14 = New Telerik.Reporting.TextBox()
        Me.textBox15 = New Telerik.Reporting.TextBox()
        Me.textBox7 = New Telerik.Reporting.TextBox()
        Me.textBox9 = New Telerik.Reporting.TextBox()
        Me.textBox19 = New Telerik.Reporting.TextBox()
        Me.textBox21 = New Telerik.Reporting.TextBox()
        Me.groupFooterSection1 = New Telerik.Reporting.GroupFooterSection()
        Me.groupHeaderSection1 = New Telerik.Reporting.GroupHeaderSection()
        Me.textBoxProductCategoryLabel = New Telerik.Reporting.TextBox()
        Me.textBox3 = New Telerik.Reporting.TextBox()
        Me.shape3 = New Telerik.Reporting.Shape()
        Me.groupFooterSection2 = New Telerik.Reporting.GroupFooterSection()
        Me.groupHeaderSection2 = New Telerik.Reporting.GroupHeaderSection()
        Me.textBox4 = New Telerik.Reporting.TextBox()
        Me.groupFooterSection3 = New Telerik.Reporting.GroupFooterSection()
        Me.groupHeaderSection3 = New Telerik.Reporting.GroupHeaderSection()
        Me.panel3 = New Telerik.Reporting.Panel()
        Me.pictureBox1 = New Telerik.Reporting.PictureBox()
        Me.textBox1 = New Telerik.Reporting.TextBox()
        Me.table1 = New Telerik.Reporting.Table()
        Me.textBox16 = New Telerik.Reporting.TextBox()
        Me.textBox17 = New Telerik.Reporting.TextBox()
        Me.textBox8 = New Telerik.Reporting.TextBox()
        Me.textBox18 = New Telerik.Reporting.TextBox()
        Me.textBox20 = New Telerik.Reporting.TextBox()
        Me.textBox22 = New Telerik.Reporting.TextBox()
        Me.panel7 = New Telerik.Reporting.Panel()
        Me.shape1 = New Telerik.Reporting.Shape()
        Me.textBox10 = New Telerik.Reporting.TextBox()
        Me.textBox6 = New Telerik.Reporting.TextBox()
        Me.sqlDataSource2 = New Telerik.Reporting.SqlDataSource()
        Me.detailSection1 = New Telerik.Reporting.DetailSection()
        Me.sqlDataSource1 = New Telerik.Reporting.SqlDataSource()
        Me.tocSection1 = New Telerik.Reporting.TocSection()
        Me.pageHeaderSection1 = New Telerik.Reporting.PageHeaderSection()
        Me.textBox2 = New Telerik.Reporting.TextBox()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'textBox5
        '
        Me.textBox5.Name = "textBox5"
        Me.textBox5.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.95442020893096924R), Telerik.Reporting.Drawing.Unit.Inch(0.27081868052482605R))
        Me.textBox5.Style.Color = System.Drawing.Color.FromArgb(CType(CType(177, Byte), Integer), CType(CType(161, Byte), Integer), CType(CType(82, Byte), Integer))
        Me.textBox5.Value = "Product No."
        '
        'textBox14
        '
        Me.textBox14.Name = "textBox14"
        Me.textBox14.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.2207286357879639R), Telerik.Reporting.Drawing.Unit.Cm(0.68787944316864014R))
        Me.textBox14.Style.Color = System.Drawing.Color.FromArgb(CType(CType(177, Byte), Integer), CType(CType(161, Byte), Integer), CType(CType(82, Byte), Integer))
        Me.textBox14.Value = "Product"
        '
        'textBox15
        '
        Me.textBox15.Name = "textBox15"
        Me.textBox15.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.0693451166152954R), Telerik.Reporting.Drawing.Unit.Cm(0.68787944316864014R))
        Me.textBox15.Style.Color = System.Drawing.Color.FromArgb(CType(CType(177, Byte), Integer), CType(CType(161, Byte), Integer), CType(CType(82, Byte), Integer))
        Me.textBox15.Value = "Color"
        '
        'textBox7
        '
        Me.textBox7.Name = "textBox7"
        Me.textBox7.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.63820755481719971R), Telerik.Reporting.Drawing.Unit.Cm(0.68787944316864014R))
        Me.textBox7.Style.Color = System.Drawing.Color.FromArgb(CType(CType(177, Byte), Integer), CType(CType(161, Byte), Integer), CType(CType(82, Byte), Integer))
        Me.textBox7.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.textBox7.Value = "Size"
        '
        'textBox9
        '
        Me.textBox9.Name = "textBox9"
        Me.textBox9.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.63375592231750488R), Telerik.Reporting.Drawing.Unit.Cm(0.68787944316864014R))
        Me.textBox9.Style.Color = System.Drawing.Color.FromArgb(CType(CType(177, Byte), Integer), CType(CType(161, Byte), Integer), CType(CType(82, Byte), Integer))
        Me.textBox9.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.textBox9.Value = "Weight"
        '
        'textBox19
        '
        Me.textBox19.Name = "textBox19"
        Me.textBox19.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.0724959373474121R), Telerik.Reporting.Drawing.Unit.Cm(0.68787944316864014R))
        Me.textBox19.Style.Color = System.Drawing.Color.FromArgb(CType(CType(177, Byte), Integer), CType(CType(161, Byte), Integer), CType(CType(82, Byte), Integer))
        Me.textBox19.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.textBox19.Value = "Dealer"
        '
        'textBox21
        '
        Me.textBox21.Name = "textBox21"
        Me.textBox21.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.1005908250808716R), Telerik.Reporting.Drawing.Unit.Cm(0.68787944316864014R))
        Me.textBox21.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None
        Me.textBox21.Style.Color = System.Drawing.Color.FromArgb(CType(CType(177, Byte), Integer), CType(CType(161, Byte), Integer), CType(CType(82, Byte), Integer))
        Me.textBox21.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.textBox21.Value = "List Price"
        '
        'groupFooterSection1
        '
        Me.groupFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.21875R)
        Me.groupFooterSection1.Name = "groupFooterSection1"
        Me.groupFooterSection1.Style.Visible = False
        '
        'groupHeaderSection1
        '
        Me.groupHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(1.5R)
        Me.groupHeaderSection1.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.textBoxProductCategoryLabel, Me.textBox3, Me.shape3})
        Me.groupHeaderSection1.Name = "groupHeaderSection1"
        Me.groupHeaderSection1.PrintOnEveryPage = True
        '
        'textBoxProductCategoryLabel
        '
        Me.textBoxProductCategoryLabel.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.000039418537198798731R), Telerik.Reporting.Drawing.Unit.Inch(0.90000009536743164R))
        Me.textBoxProductCategoryLabel.Name = "textBoxProductCategoryLabel"
        Me.textBoxProductCategoryLabel.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.0999212265014648R), Telerik.Reporting.Drawing.Unit.Inch(0.3999999463558197R))
        Me.textBoxProductCategoryLabel.StyleName = "Category"
        Me.textBoxProductCategoryLabel.Value = "=Fields.ProductCategory"
        '
        'textBox3
        '
        Me.textBox3.Culture = New System.Globalization.CultureInfo("en")
        Me.textBox3.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0R), Telerik.Reporting.Drawing.Unit.Inch(0.000039339065551757812R))
        Me.textBox3.Name = "textBox3"
        Me.textBox3.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.0999603271484375R), Telerik.Reporting.Drawing.Unit.Inch(0.60000002384185791R))
        Me.textBox3.StyleName = "Header"
        Me.textBox3.Value = "Product Catalog"
        '
        'shape3
        '
        Me.shape3.Anchoring = CType((Telerik.Reporting.AnchoringStyles.Left Or Telerik.Reporting.AnchoringStyles.Right), Telerik.Reporting.AnchoringStyles)
        Me.shape3.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0R), Telerik.Reporting.Drawing.Unit.Inch(0.60011798143386841R))
        Me.shape3.Name = "shape3"
        Me.shape3.ShapeType = New Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW)
        Me.shape3.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.0999603271484375R), Telerik.Reporting.Drawing.Unit.Point(2.0R))
        Me.shape3.Stretch = True
        Me.shape3.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.shape3.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid
        Me.shape3.Style.BorderWidth.Bottom = Telerik.Reporting.Drawing.Unit.Point(0.5R)
        Me.shape3.Style.BorderWidth.Top = Telerik.Reporting.Drawing.Unit.Point(1.5R)
        Me.shape3.Style.Color = System.Drawing.Color.Transparent
        '
        'groupFooterSection2
        '
        Me.groupFooterSection2.Height = Telerik.Reporting.Drawing.Unit.Inch(0.1979166716337204R)
        Me.groupFooterSection2.Name = "groupFooterSection2"
        Me.groupFooterSection2.Style.Visible = False
        '
        'groupHeaderSection2
        '
        Me.groupHeaderSection2.Height = Telerik.Reporting.Drawing.Unit.Inch(0.40000000596046448R)
        Me.groupHeaderSection2.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.textBox4})
        Me.groupHeaderSection2.KeepTogether = False
        Me.groupHeaderSection2.Name = "groupHeaderSection2"
        Me.groupHeaderSection2.PrintOnEveryPage = True
        '
        'textBox4
        '
        Me.textBox4.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0R), Telerik.Reporting.Drawing.Unit.Inch(0R))
        Me.textBox4.Name = "textBox4"
        Me.textBox4.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.0999603271484375R), Telerik.Reporting.Drawing.Unit.Inch(0.30000010132789612R))
        Me.textBox4.StyleName = "SubCategory"
        Me.textBox4.Value = "=Fields.ProductSubCategory"
        '
        'groupFooterSection3
        '
        Me.groupFooterSection3.Height = Telerik.Reporting.Drawing.Unit.Inch(0.14999993145465851R)
        Me.groupFooterSection3.Name = "groupFooterSection3"
        '
        'groupHeaderSection3
        '
        Me.groupHeaderSection3.Height = Telerik.Reporting.Drawing.Unit.Inch(1.5000003576278687R)
        Me.groupHeaderSection3.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.panel3})
        Me.groupHeaderSection3.Name = "groupHeaderSection3"
        Me.groupHeaderSection3.PrintOnEveryPage = True
        '
        'panel3
        '
        Me.panel3.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.pictureBox1, Me.textBox1, Me.table1, Me.textBox6})
        Me.panel3.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.19999949634075165R), Telerik.Reporting.Drawing.Unit.Inch(0R))
        Me.panel3.Name = "panel3"
        Me.panel3.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(7.7000007629394531R), Telerik.Reporting.Drawing.Unit.Inch(1.4530587196350098R))
        '
        'pictureBox1
        '
        Me.pictureBox1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.000039418537198798731R), Telerik.Reporting.Drawing.Unit.Inch(0.000039418537198798731R))
        Me.pictureBox1.MimeType = ""
        Me.pictureBox1.Name = "pictureBox1"
        Me.pictureBox1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.3372377157211304R), Telerik.Reporting.Drawing.Unit.Inch(0.89988160133361816R))
        Me.pictureBox1.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.ScaleProportional
        Me.pictureBox1.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.pictureBox1.Style.Color = System.Drawing.Color.White
        Me.pictureBox1.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.pictureBox1.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Point(1.0R)
        Me.pictureBox1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(1.0R)
        Me.pictureBox1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(1.0R)
        Me.pictureBox1.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Point(1.0R)
        Me.pictureBox1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.pictureBox1.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.pictureBox1.Value = "=Fields.LargePhoto"
        '
        'textBox1
        '
        Me.textBox1.Bindings.Add(New Telerik.Reporting.Binding("Style.Font.Name", "= IIf(Parameters.CultureID.Value = 'zh-cht', 'Microsoft JhengHei', IIf(Parameters" &
            ".CultureID.Value = 'th', 'Microsoft Sans Serif', 'Segoe UI Light'))"))
        Me.textBox1.CanShrink = True
        Me.textBox1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.2445292472839355R), Telerik.Reporting.Drawing.Unit.Inch(0.000039418537198798731R))
        Me.textBox1.Name = "textBox1"
        Me.textBox1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.2999608516693115R), Telerik.Reporting.Drawing.Unit.Inch(0.89988160133361816R))
        Me.textBox1.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None
        Me.textBox1.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None
        Me.textBox1.Style.Font.Name = "Segoe UI Light"
        Me.textBox1.Value = "=Fields.Description"
        '
        'table1
        '
        Me.table1.Bindings.Add(New Telerik.Reporting.Binding("DataSource", "=ReportItem.DataObject"))
        Me.table1.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.954420268535614R)))
        Me.table1.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(2.220726490020752R)))
        Me.table1.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(1.0693440437316895R)))
        Me.table1.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.638206958770752R)))
        Me.table1.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.63375532627105713R)))
        Me.table1.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(1.0724958181381226R)))
        Me.table1.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(1.1005909442901611R)))
        Me.table1.Body.Rows.Add(New Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.716888964176178R)))
        Me.table1.Body.SetCellContent(0, 0, Me.textBox16)
        Me.table1.Body.SetCellContent(0, 1, Me.textBox17)
        Me.table1.Body.SetCellContent(0, 3, Me.textBox8)
        Me.table1.Body.SetCellContent(0, 4, Me.textBox18)
        Me.table1.Body.SetCellContent(0, 5, Me.textBox20)
        Me.table1.Body.SetCellContent(0, 6, Me.textBox22)
        Me.table1.Body.SetCellContent(0, 2, Me.panel7)
        TableGroup1.Name = "tableGroup1"
        TableGroup1.ReportItem = Me.textBox5
        TableGroup2.Name = "tableGroup2"
        TableGroup2.ReportItem = Me.textBox14
        TableGroup3.Name = "tableGroup3"
        TableGroup3.ReportItem = Me.textBox15
        TableGroup4.Name = "tableGroup4"
        TableGroup4.ReportItem = Me.textBox7
        TableGroup5.Name = "tableGroup5"
        TableGroup5.ReportItem = Me.textBox9
        TableGroup6.Name = "tableGroup6"
        TableGroup6.ReportItem = Me.textBox19
        TableGroup7.Name = "tableGroup7"
        TableGroup7.ReportItem = Me.textBox21
        Me.table1.ColumnGroups.Add(TableGroup1)
        Me.table1.ColumnGroups.Add(TableGroup2)
        Me.table1.ColumnGroups.Add(TableGroup3)
        Me.table1.ColumnGroups.Add(TableGroup4)
        Me.table1.ColumnGroups.Add(TableGroup5)
        Me.table1.ColumnGroups.Add(TableGroup6)
        Me.table1.ColumnGroups.Add(TableGroup7)
        Me.table1.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.textBox16, Me.textBox17, Me.panel7, Me.textBox8, Me.textBox18, Me.textBox20, Me.textBox22, Me.textBox5, Me.textBox14, Me.textBox15, Me.textBox7, Me.textBox9, Me.textBox19, Me.textBox21})
        Me.table1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00009987076191464439R), Telerik.Reporting.Drawing.Unit.Cm(2.2860004901885986R))
        Me.table1.Name = "table1"
        TableGroup8.Groupings.Add(New Telerik.Reporting.Grouping(""))
        TableGroup8.Name = "detailGroup"
        Me.table1.RowGroups.Add(TableGroup8)
        Me.table1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(7.689539909362793R), Telerik.Reporting.Drawing.Unit.Cm(1.4047684669494629R))
        '
        'textBox16
        '
        Me.textBox16.Name = "textBox16"
        Me.textBox16.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.95442020893096924R), Telerik.Reporting.Drawing.Unit.Cm(0.716888964176178R))
        Me.textBox16.Value = "=Fields.ProductNumber"
        '
        'textBox17
        '
        Me.textBox17.Name = "textBox17"
        Me.textBox17.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.2207286357879639R), Telerik.Reporting.Drawing.Unit.Cm(0.716888964176178R))
        Me.textBox17.Value = "=Fields.ProductName"
        '
        'textBox8
        '
        Me.textBox8.Name = "textBox8"
        Me.textBox8.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.63820755481719971R), Telerik.Reporting.Drawing.Unit.Cm(0.716888964176178R))
        Me.textBox8.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.textBox8.Value = "=IsNull(Fields.Size, 'N/A')"
        '
        'textBox18
        '
        Me.textBox18.Name = "textBox18"
        Me.textBox18.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.63375592231750488R), Telerik.Reporting.Drawing.Unit.Cm(0.716888964176178R))
        Me.textBox18.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.textBox18.Value = "=IsNull(Fields.Weight, 'N/A')"
        '
        'textBox20
        '
        Me.textBox20.Format = "{0:C2}"
        Me.textBox20.Name = "textBox20"
        Me.textBox20.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.0724959373474121R), Telerik.Reporting.Drawing.Unit.Cm(0.716888964176178R))
        Me.textBox20.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.textBox20.Value = "=ISNULL(Fields.StandardCost, ""Call"")"
        '
        'textBox22
        '
        Me.textBox22.Format = "{0:C2}"
        Me.textBox22.Name = "textBox22"
        Me.textBox22.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.1005908250808716R), Telerik.Reporting.Drawing.Unit.Cm(0.716888964176178R))
        Me.textBox22.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None
        Me.textBox22.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.textBox22.Value = "=IsNull(Fields.ListPrice, 'Call')"
        '
        'panel7
        '
        Me.panel7.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.shape1, Me.textBox10})
        Me.panel7.Name = "panel7"
        Me.panel7.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.0693451166152954R), Telerik.Reporting.Drawing.Unit.Cm(0.716888964176178R))
        Me.panel7.Style.BorderColor.Default = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.panel7.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.panel7.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid
        Me.panel7.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.panel7.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        '
        'shape1
        '
        FormattingRule1.Filters.Add(New Telerik.Reporting.Filter("= ToLower(Fields.Color)", Telerik.Reporting.FilterOperator.Equal, "red"))
        FormattingRule1.StopIfTrue = True
        FormattingRule1.Style.BackgroundColor = System.Drawing.Color.Red
        FormattingRule2.Filters.Add(New Telerik.Reporting.Filter("= ToLower(Fields.Color)", Telerik.Reporting.FilterOperator.Equal, "blue"))
        FormattingRule2.StopIfTrue = True
        FormattingRule2.Style.BackgroundColor = System.Drawing.Color.Blue
        FormattingRule3.Filters.Add(New Telerik.Reporting.Filter("= ToLower(Fields.Color)", Telerik.Reporting.FilterOperator.Equal, "black"))
        FormattingRule3.StopIfTrue = True
        FormattingRule3.Style.BackgroundColor = System.Drawing.Color.Black
        FormattingRule4.Filters.Add(New Telerik.Reporting.Filter("= ToLower(Fields.Color)", Telerik.Reporting.FilterOperator.Equal, "yellow"))
        FormattingRule4.StopIfTrue = True
        FormattingRule4.Style.BackgroundColor = System.Drawing.Color.Yellow
        FormattingRule5.Filters.Add(New Telerik.Reporting.Filter("= ToLower(Fields.Color)", Telerik.Reporting.FilterOperator.Equal, "silver"))
        FormattingRule5.StopIfTrue = True
        FormattingRule5.Style.BackgroundColor = System.Drawing.Color.Silver
        Me.shape1.ConditionalFormatting.AddRange(New Telerik.Reporting.Drawing.FormattingRule() {FormattingRule1, FormattingRule2, FormattingRule3, FormattingRule4, FormattingRule5})
        Me.shape1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.0793650820851326R), Telerik.Reporting.Drawing.Unit.Cm(0.07025589793920517R))
        Me.shape1.Name = "shape1"
        Me.shape1.ShapeType = New Telerik.Reporting.Drawing.Shapes.EllipseShape(0R)
        Me.shape1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.26041331887245178R), Telerik.Reporting.Drawing.Unit.Inch(0.22303469479084015R))
        Me.shape1.Style.Color = System.Drawing.Color.Transparent
        '
        'textBox10
        '
        Me.textBox10.Format = "{0:C2}"
        Me.textBox10.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.8202979564666748R), Telerik.Reporting.Drawing.Unit.Cm(0.07025589793920517R))
        Me.textBox10.Name = "textBox10"
        Me.textBox10.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.6804002523422241R), Telerik.Reporting.Drawing.Unit.Inch(0.25454029440879822R))
        Me.textBox10.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None
        Me.textBox10.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None
        Me.textBox10.Value = "= IsNull(Fields.Color, 'N/A')"
        '
        'textBox6
        '
        Me.textBox6.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.4648761749267578R), Telerik.Reporting.Drawing.Unit.Inch(0R))
        Me.textBox6.Name = "textBox6"
        Me.textBox6.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.0351245403289795R), Telerik.Reporting.Drawing.Unit.Inch(0.30000028014183044R))
        Me.textBox6.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None
        Me.textBox6.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None
        Me.textBox6.StyleName = "SubCategory"
        Me.textBox6.Value = "=Fields.ProductModel"
        '
        'sqlDataSource2
        '
        Me.sqlDataSource2.ConnectionString = "My.MySettings.TelerikConnectionString"
        Me.sqlDataSource2.Name = "sqlDataSource2"
        Me.sqlDataSource2.SelectCommand = "SELECT RTRIM(CultureID) AS CultureID, Name FROM Production.Culture WHERE (NOT (RT" &
    "RIM(CultureID) IN ('es', '')))"
        '
        'detailSection1
        '
        Me.detailSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.13208325207233429R)
        Me.detailSection1.Name = "detailSection1"
        Me.detailSection1.Style.BorderColor.Default = System.Drawing.Color.FromArgb(CType(CType(137, Byte), Integer), CType(CType(145, Byte), Integer), CType(CType(164, Byte), Integer))
        Me.detailSection1.Style.BorderWidth.Left = Telerik.Reporting.Drawing.Unit.Point(3.0R)
        Me.detailSection1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8.0R)
        Me.detailSection1.Style.Visible = False
        '
        'sqlDataSource1
        '
        Me.sqlDataSource1.ConnectionString = "My.MySettings.TelerikConnectionString"
        Me.sqlDataSource1.Name = "sqlDataSource1"
        Me.sqlDataSource1.Parameters.AddRange(New Telerik.Reporting.SqlDataSourceParameter() {New Telerik.Reporting.SqlDataSourceParameter("@cultureID", System.Data.DbType.[String], "=Parameters('CultureID').Value")})
        Me.sqlDataSource1.SelectCommand = resources.GetString("sqlDataSource1.SelectCommand")
        '
        'tocSection1
        '
        Me.tocSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(1.5R)
        Me.tocSection1.KeepTogether = False
        TocLevel1.LeaderSymbol = Global.Microsoft.VisualBasic.ChrW(0)
        TocLevel1.Name = "Level1"
        TocLevel1.Style.Font.Bold = True
        TocLevel1.Style.Font.Name = "Segoe UI Light"
        TocLevel1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(16.0R)
        TocLevel1.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Point(5.0R)
        TocLevel1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        TocLevel1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        TocLevel1.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Point(5.0R)
        TocLevel2.Name = "Level2"
        TocLevel2.Style.Font.Name = "Segoe UI Light"
        TocLevel2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(13.0R)
        TocLevel2.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Point(1.0R)
        TocLevel2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(32.0R)
        TocLevel2.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        TocLevel2.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Point(1.0R)
        TocLevel3.Name = "Level3"
        TocLevel3.Style.Font.Name = "Segoe UI Light"
        TocLevel3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(13.0R)
        TocLevel3.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Point(1.0R)
        TocLevel3.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(52.0R)
        TocLevel3.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        TocLevel3.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Point(1.0R)
        Me.tocSection1.Levels.Add(TocLevel1)
        Me.tocSection1.Levels.Add(TocLevel2)
        Me.tocSection1.Levels.Add(TocLevel3)
        Me.tocSection1.Name = "TocSection1"
        Me.tocSection1.PageBreak = Telerik.Reporting.PageBreak.After
        Me.tocSection1.Title = "Table of Contents"
        Me.tocSection1.TitleStyle.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.tocSection1.TitleStyle.BorderWidth.Bottom = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        Me.tocSection1.TitleStyle.Font.Bold = True
        Me.tocSection1.TitleStyle.Font.Name = "Segoe UI Light"
        Me.tocSection1.TitleStyle.Font.Size = Telerik.Reporting.Drawing.Unit.Point(25.0R)
        Me.tocSection1.TitleStyle.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Point(10.0R)
        '
        ' pageHeaderSection1
        '
        Me.pageHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985R)
        Me.pageHeaderSection1.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.textBox2})
        Me.pageHeaderSection1.Name = "pageHeaderSection1"
        '
        ' textBox2
        '
        Me.textBox2.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.2000002861022949R), Telerik.Reporting.Drawing.Unit.Inch(0.000039339065551757812R))
        Me.textBox2.Name = "textBox2"
        Me.textBox2.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.8999993801116943R), Telerik.Reporting.Drawing.Unit.Inch(0.19996063411235809R))
        Me.textBox2.Style.Font.Name = "Segoe UI Light"
        Me.textBox2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10.0R)
        Me.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.textBox2.Value = "= PageExec('group1', Fields.ProductCategory) + ', page ' + PageNumber('group1', Fields.ProductCategory) + ' of ' + PageCount('group1', Fields.ProductCategory)"
        '
        'ProductCatalog
        '
        Me.Bindings.Add(New Telerik.Reporting.Binding("Culture", "=Parameters.CultureID.Value"))
        Me.DataSource = Me.sqlDataSource1
        Me.DocumentName = "='ProductCatalog.' + Parameters.CultureID.Value"
        Group1.DocumentMapText = "=Fields.ProductCategory"
        Group1.GroupFooter = Me.groupFooterSection1
        Group1.GroupHeader = Me.groupHeaderSection1
        Group1.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.ProductCategory"))
        Group1.GroupKeepTogether = Telerik.Reporting.GroupKeepTogether.All
        Group1.Name = "group1"
        Group1.Sortings.Add(New Telerik.Reporting.Sorting("=Fields.ProductCategory", Telerik.Reporting.SortDirection.Asc))
        Group1.TocText = "=Fields.ProductCategory"
        Group2.DocumentMapText = "=Fields.ProductSubCategory"
        Group2.GroupFooter = Me.groupFooterSection2
        Group2.GroupHeader = Me.groupHeaderSection2
        Group2.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.ProductSubCategory"))
        Group2.GroupKeepTogether = Telerik.Reporting.GroupKeepTogether.FirstDetail
        Group2.Name = "group2"
        Group2.Sortings.Add(New Telerik.Reporting.Sorting("=Fields.ProductSubCategory", Telerik.Reporting.SortDirection.Asc))
        Group2.TocText = "=Fields.ProductSubCategory"
        Group3.DocumentMapText = "=Fields.ProductModel"
        Group3.GroupFooter = Me.groupFooterSection3
        Group3.GroupHeader = Me.groupHeaderSection3
        Group3.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.ProductModel"))
        Group3.GroupKeepTogether = Telerik.Reporting.GroupKeepTogether.FirstDetail
        Group3.Name = "group3"
        Group3.Sortings.Add(New Telerik.Reporting.Sorting("=Fields.ProductModel", Telerik.Reporting.SortDirection.Asc))
        Me.Groups.AddRange(New Telerik.Reporting.Group() {Group1, Group2, Group3})
        Me.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.groupHeaderSection1, Me.groupFooterSection1, Me.groupHeaderSection2, Me.groupFooterSection2, Me.groupHeaderSection3, Me.groupFooterSection3, Me.detailSection1, Me.tocSection1, Me.pageHeaderSection1})
        Me.Name = "ProductCatalog"
        Me.PageSettings.Landscape = False
        Me.PageSettings.Margins = New Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224R), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224R), Telerik.Reporting.Drawing.Unit.Inch(0.30000001192092896R), Telerik.Reporting.Drawing.Unit.Inch(0.40000000596046448R))
        Me.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Letter
        TextWatermark1.Color = System.Drawing.Color.FromArgb(CType(CType(137, Byte), Integer), CType(CType(145, Byte), Integer), CType(CType(164, Byte), Integer))
        TextWatermark1.Font.Name = "Tahoma"
        TextWatermark1.Font.Size = Telerik.Reporting.Drawing.Unit.Point(100.0R)
        TextWatermark1.Opacity = 0.06R
        TextWatermark1.Orientation = Telerik.Reporting.Drawing.WatermarkOrientation.Vertical
        TextWatermark1.Position = Telerik.Reporting.Drawing.WatermarkPosition.Front
        TextWatermark1.Text = "=PageExec(""textBoxProductCategoryLabel"", First(Fields.ProductCategory))"
        Me.PageSettings.Watermarks.Add(TextWatermark1)
        ReportParameter1.AutoRefresh = True
        ReportParameter1.AvailableValues.DataSource = Me.sqlDataSource2
        ReportParameter1.AvailableValues.DisplayMember = "Name"
        ReportParameter1.AvailableValues.ValueMember = "CultureID"
        ReportParameter1.Name = "CultureID"
        ReportParameter1.Text = "Language"
        ReportParameter1.Value = "en"
        ReportParameter1.Visible = True
        Me.ReportParameters.Add(ReportParameter1)
        StyleRule1.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.TextItemBase)), New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.HtmlTextBox))})
        StyleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        StyleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        StyleRule2.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.StyleSelector("Header")})
        StyleRule2.Style.Font.Bold = True
        StyleRule2.Style.Font.Name = "Segoe UI Light"
        StyleRule2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(25.0R)
        StyleRule2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0R)
        StyleRule3.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.StyleSelector("Category")})
        StyleRule3.Style.BorderColor.Default = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(7, Byte), Integer), CType(CType(5, Byte), Integer))
        StyleRule3.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        StyleRule3.Style.BorderWidth.Bottom = Telerik.Reporting.Drawing.Unit.Point(1.0R)
        StyleRule3.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Point(0.5R)
        StyleRule3.Style.Font.Name = "Segoe UI Light"
        StyleRule3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(16.0R)
        StyleRule3.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0R)
        StyleRule4.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.StyleSelector("SubCategory")})
        StyleRule4.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        StyleRule4.Style.BorderWidth.Bottom = Telerik.Reporting.Drawing.Unit.Point(0.5R)
        StyleRule4.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Point(0.5R)
        StyleRule4.Style.Font.Name = "Segoe UI Light"
        StyleRule4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(13.0R)
        StyleRule4.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0R)
        DescendantSelector1.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.GroupHeaderSection)), New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.Panel)), New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.TextBox))})
        StyleRule5.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {DescendantSelector1})
        StyleRule5.Style.BorderColor.Default = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer))
        StyleRule5.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        StyleRule5.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid
        StyleRule5.Style.Font.Name = "Segoe UI"
        StyleRule5.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10.0R)
        StyleRule5.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Inch(0.039999999105930328R)
        StyleRule5.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.05000000074505806R)
        StyleRule5.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Inch(0.05000000074505806R)
        StyleRule5.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom
        Me.StyleSheet.AddRange(New Telerik.Reporting.Drawing.StyleRule() {StyleRule1, StyleRule2, StyleRule3, StyleRule4, StyleRule5})
        Me.TocText = "Product Catalog"
        Me.Width = Telerik.Reporting.Drawing.Unit.Inch(8.09999942779541R)
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
#End Region

    Private groupHeaderSection1 As GroupHeaderSection
    Private textBoxProductCategoryLabel As TextBox
    Private groupFooterSection1 As GroupFooterSection
    Private groupHeaderSection2 As GroupHeaderSection
    Private textBox4 As TextBox
    Private groupFooterSection2 As GroupFooterSection
    Private groupHeaderSection3 As GroupHeaderSection
    Private groupFooterSection3 As GroupFooterSection
    Private detailSection1 As DetailSection
    Private textBox3 As TextBox
    Private textBox6 As TextBox
    Private panel3 As Panel
    Private pictureBox1 As PictureBox
    Private textBox1 As TextBox
    Private table1 As Table
    Private textBox5 As TextBox
    Private textBox14 As TextBox
    Private textBox15 As TextBox
    Private textBox16 As TextBox
    Private textBox17 As TextBox
    Private textBox8 As TextBox
    Private textBox7 As TextBox
    Private textBox18 As TextBox
    Private textBox9 As TextBox
    Private textBox20 As TextBox
    Private textBox19 As TextBox
    Private textBox22 As TextBox
    Private textBox21 As TextBox
    Private panel7 As Panel
    Private shape1 As Shape
    Private textBox10 As TextBox
    Private sqlDataSource1 As SqlDataSource
    Private sqlDataSource2 As SqlDataSource
    Private shape3 As Shape
    Private tocSection1 As TocSection
    Private pageHeaderSection1 As PageHeaderSection
    Private textBox2 As TextBox
End Class