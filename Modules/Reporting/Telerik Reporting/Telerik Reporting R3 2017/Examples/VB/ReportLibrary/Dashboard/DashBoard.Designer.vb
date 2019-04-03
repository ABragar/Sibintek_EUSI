Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports Telerik.Reporting
Imports Telerik.Reporting.Drawing

Partial Class Dashboard
#Region "Component Designer generated code"
    ''' <summary>
    ''' Required method for telerik Reporting designer support - do not modify
    ''' the contents of this method with the code editor.
    ''' </summary>
    Private Sub InitializeComponent()
        Dim TableGroup1 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup2 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup3 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Dashboard))
        Dim GraphGroup1 As Telerik.Reporting.GraphGroup = New Telerik.Reporting.GraphGroup()
        Dim ColorPalette1 As Telerik.Reporting.Drawing.ColorPalette = New Telerik.Reporting.Drawing.ColorPalette()
        Dim NumericalScale1 As Telerik.Reporting.NumericalScale = New Telerik.Reporting.NumericalScale()
        Dim CategoryScale1 As Telerik.Reporting.CategoryScale = New Telerik.Reporting.CategoryScale()
        Dim GraphGroup2 As Telerik.Reporting.GraphGroup = New Telerik.Reporting.GraphGroup()
        Dim TableGroup4 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup5 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup6 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim GraphGroup5 As Telerik.Reporting.GraphGroup = New Telerik.Reporting.GraphGroup()
        Dim ColorPalette3 As Telerik.Reporting.Drawing.ColorPalette = New Telerik.Reporting.Drawing.ColorPalette()
        Dim NumericalScale3 As Telerik.Reporting.NumericalScale = New Telerik.Reporting.NumericalScale()
        Dim CategoryScale3 As Telerik.Reporting.CategoryScale = New Telerik.Reporting.CategoryScale()
        Dim GraphGroup6 As Telerik.Reporting.GraphGroup = New Telerik.Reporting.GraphGroup()
        Dim TableGroup7 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup8 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup9 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim GraphGroup9 As Telerik.Reporting.GraphGroup = New Telerik.Reporting.GraphGroup()
        Dim ColorPalette5 As Telerik.Reporting.Drawing.ColorPalette = New Telerik.Reporting.Drawing.ColorPalette()
        Dim NumericalScale5 As Telerik.Reporting.NumericalScale = New Telerik.Reporting.NumericalScale()
        Dim CategoryScale5 As Telerik.Reporting.CategoryScale = New Telerik.Reporting.CategoryScale()
        Dim GraphGroup10 As Telerik.Reporting.GraphGroup = New Telerik.Reporting.GraphGroup()
        Dim GraphGroup3 As Telerik.Reporting.GraphGroup = New Telerik.Reporting.GraphGroup()
        Dim ColorPalette2 As Telerik.Reporting.Drawing.ColorPalette = New Telerik.Reporting.Drawing.ColorPalette()
        Dim NumericalScale2 As Telerik.Reporting.NumericalScale = New Telerik.Reporting.NumericalScale()
        Dim CategoryScale2 As Telerik.Reporting.CategoryScale = New Telerik.Reporting.CategoryScale()
        Dim GraphGroup4 As Telerik.Reporting.GraphGroup = New Telerik.Reporting.GraphGroup()
        Dim GraphGroup7 As Telerik.Reporting.GraphGroup = New Telerik.Reporting.GraphGroup()
        Dim ColorPalette4 As Telerik.Reporting.Drawing.ColorPalette = New Telerik.Reporting.Drawing.ColorPalette()
        Dim NumericalScale4 As Telerik.Reporting.NumericalScale = New Telerik.Reporting.NumericalScale()
        Dim CategoryScale4 As Telerik.Reporting.CategoryScale = New Telerik.Reporting.CategoryScale()
        Dim GraphGroup8 As Telerik.Reporting.GraphGroup = New Telerik.Reporting.GraphGroup()
        Dim GraphGroup11 As Telerik.Reporting.GraphGroup = New Telerik.Reporting.GraphGroup()
        Dim ColorPalette6 As Telerik.Reporting.Drawing.ColorPalette = New Telerik.Reporting.Drawing.ColorPalette()
        Dim NumericalScale6 As Telerik.Reporting.NumericalScale = New Telerik.Reporting.NumericalScale()
        Dim CategoryScale6 As Telerik.Reporting.CategoryScale = New Telerik.Reporting.CategoryScale()
        Dim GraphGroup12 As Telerik.Reporting.GraphGroup = New Telerik.Reporting.GraphGroup()
        Dim ReportParameter1 As Telerik.Reporting.ReportParameter = New Telerik.Reporting.ReportParameter()
        Dim StyleRule1 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule2 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule3 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule4 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Me.spLabelTotalQ = New Telerik.Reporting.TextBox()
        Me.spLabelTotal = New Telerik.Reporting.TextBox()
        Me.spLabelSalesPerson = New Telerik.Reporting.TextBox()
        Me.sLabelTotalQ = New Telerik.Reporting.TextBox()
        Me.sLabelTotal = New Telerik.Reporting.TextBox()
        Me.sLabelStore = New Telerik.Reporting.TextBox()
        Me.pLabelTotalQ = New Telerik.Reporting.TextBox()
        Me.pLabelTotal = New Telerik.Reporting.TextBox()
        Me.pProductName = New Telerik.Reporting.TextBox()
        Me.yearDataSource = New Telerik.Reporting.SqlDataSource()
        Me.detail = New Telerik.Reporting.DetailSection()
        Me.panel2 = New Telerik.Reporting.Panel()
        Me.panel3 = New Telerik.Reporting.Panel()
        Me.SalesPersonQuarter = New Telerik.Reporting.Crosstab()
        Me.spTotalQ = New Telerik.Reporting.TextBox()
        Me.spTotal = New Telerik.Reporting.TextBox()
        Me.spTextCorner = New Telerik.Reporting.TextBox()
        Me.mainDataSource = New Telerik.Reporting.SqlDataSource()
        Me.textBox2 = New Telerik.Reporting.TextBox()
        Me.textBox11 = New Telerik.Reporting.TextBox()
        Me.textBox12 = New Telerik.Reporting.TextBox()
        Me.textBox3 = New Telerik.Reporting.TextBox()
        Me.SalesPersonQuarterBar = New Telerik.Reporting.Graph()
        Me.cartesianCoordinateSystem1 = New Telerik.Reporting.CartesianCoordinateSystem()
        Me.graphAxis3 = New Telerik.Reporting.GraphAxis()
        Me.graphAxis4 = New Telerik.Reporting.GraphAxis()
        Me.barSeries1 = New Telerik.Reporting.BarSeries()
        Me.panel1 = New Telerik.Reporting.Panel()
        Me.panel4 = New Telerik.Reporting.Panel()
        Me.StoreQuarter = New Telerik.Reporting.Crosstab()
        Me.sTotalQ = New Telerik.Reporting.TextBox()
        Me.sTotal = New Telerik.Reporting.TextBox()
        Me.sTextCorner = New Telerik.Reporting.TextBox()
        Me.textBox9 = New Telerik.Reporting.TextBox()
        Me.textBox13 = New Telerik.Reporting.TextBox()
        Me.textBox14 = New Telerik.Reporting.TextBox()
        Me.textBox7 = New Telerik.Reporting.TextBox()
        Me.StoreQuarterBar = New Telerik.Reporting.Graph()
        Me.cartesianCoordinateSystem2 = New Telerik.Reporting.CartesianCoordinateSystem()
        Me.graphAxis5 = New Telerik.Reporting.GraphAxis()
        Me.graphAxis6 = New Telerik.Reporting.GraphAxis()
        Me.barSeries3 = New Telerik.Reporting.BarSeries()
        Me.panelSalesByProduct = New Telerik.Reporting.Panel()
        Me.panel6 = New Telerik.Reporting.Panel()
        Me.ProductQuarter = New Telerik.Reporting.Crosstab()
        Me.pTotalQ = New Telerik.Reporting.TextBox()
        Me.pTotal = New Telerik.Reporting.TextBox()
        Me.pTextCorner = New Telerik.Reporting.TextBox()
        Me.textBox10 = New Telerik.Reporting.TextBox()
        Me.textBox15 = New Telerik.Reporting.TextBox()
        Me.textBox16 = New Telerik.Reporting.TextBox()
        Me.textBox8 = New Telerik.Reporting.TextBox()
        Me.ProductQuarterBar = New Telerik.Reporting.Graph()
        Me.cartesianCoordinateSystem3 = New Telerik.Reporting.CartesianCoordinateSystem()
        Me.graphAxis11 = New Telerik.Reporting.GraphAxis()
        Me.graphAxis12 = New Telerik.Reporting.GraphAxis()
        Me.barSeries6 = New Telerik.Reporting.BarSeries()
        Me.textBox5 = New Telerik.Reporting.TextBox()
        Me.reportHeaderSection1 = New Telerik.Reporting.ReportHeaderSection()
        Me.panel5 = New Telerik.Reporting.Panel()
        Me.textBox1 = New Telerik.Reporting.TextBox()
        Me.shape5 = New Telerik.Reporting.Shape()
        Me.textBox6 = New Telerik.Reporting.TextBox()
        Me.SalesPersonQuarterPie = New Telerik.Reporting.Graph()
        Me.polarCoordinateSystem1 = New Telerik.Reporting.PolarCoordinateSystem()
        Me.graphAxis1 = New Telerik.Reporting.GraphAxis()
        Me.graphAxis2 = New Telerik.Reporting.GraphAxis()
        Me.barSeries2 = New Telerik.Reporting.BarSeries()
        Me.Graph1 = New Telerik.Reporting.Graph()
        Me.polarCoordinateSystem2 = New Telerik.Reporting.PolarCoordinateSystem()
        Me.graphAxis7 = New Telerik.Reporting.GraphAxis()
        Me.graphAxis8 = New Telerik.Reporting.GraphAxis()
        Me.barSeries4 = New Telerik.Reporting.BarSeries()
        Me.ProductQuarterPie = New Telerik.Reporting.Graph()
        Me.polarCoordinateSystem3 = New Telerik.Reporting.PolarCoordinateSystem()
        Me.graphAxis9 = New Telerik.Reporting.GraphAxis()
        Me.graphAxis10 = New Telerik.Reporting.GraphAxis()
        Me.barSeries5 = New Telerik.Reporting.BarSeries()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'spLabelTotalQ
        '
        Me.spLabelTotalQ.Name = "spLabelTotalQ"
        Me.spLabelTotalQ.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.38188979029655457R), Telerik.Reporting.Drawing.Unit.Inch(0.24000000953674316R))
        Me.spLabelTotalQ.Style.Color = System.Drawing.Color.FromArgb(CType(CType(105, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(40, Byte), Integer))
        Me.spLabelTotalQ.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.spLabelTotalQ.Value = "='Q'+ Quarter(Fields.OrderDate)"
        '
        'spLabelTotal
        '
        Me.spLabelTotal.Name = "spLabelTotal"
        Me.spLabelTotal.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.38188979029655457R), Telerik.Reporting.Drawing.Unit.Inch(0.24000000953674316R))
        Me.spLabelTotal.Style.Color = System.Drawing.Color.FromArgb(CType(CType(105, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(40, Byte), Integer))
        Me.spLabelTotal.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.spLabelTotal.Value = "Total"
        '
        'spLabelSalesPerson
        '
        Me.spLabelSalesPerson.Name = "spLabelSalesPerson"
        Me.spLabelSalesPerson.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.5R), Telerik.Reporting.Drawing.Unit.Inch(0.24015748500823975R))
        Me.spLabelSalesPerson.Value = "=Fields.SalesPersonFullName"
        '
        'sLabelTotalQ
        '
        Me.sLabelTotalQ.Name = "sLabelTotalQ"
        Me.sLabelTotalQ.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.38188979029655457R), Telerik.Reporting.Drawing.Unit.Inch(0.24000000953674316R))
        Me.sLabelTotalQ.Style.Color = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(105, Byte), Integer), CType(CType(104, Byte), Integer))
        Me.sLabelTotalQ.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.sLabelTotalQ.Value = "='Q'+Quarter(Fields.OrderDate)"
        '
        'sLabelTotal
        '
        Me.sLabelTotal.Name = "sLabelTotal"
        Me.sLabelTotal.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.38188979029655457R), Telerik.Reporting.Drawing.Unit.Inch(0.24000000953674316R))
        Me.sLabelTotal.Style.Color = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(105, Byte), Integer), CType(CType(104, Byte), Integer))
        Me.sLabelTotal.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.sLabelTotal.Value = "Total"
        '
        'sLabelStore
        '
        Me.sLabelStore.Name = "sLabelStore"
        Me.sLabelStore.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.5R), Telerik.Reporting.Drawing.Unit.Inch(0.24015748500823975R))
        Me.sLabelStore.Value = "=Fields.StoreName"
        '
        'pLabelTotalQ
        '
        Me.pLabelTotalQ.Name = "pLabelTotalQ"
        Me.pLabelTotalQ.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.38188979029655457R), Telerik.Reporting.Drawing.Unit.Inch(0.24000000953674316R))
        Me.pLabelTotalQ.Style.Color = System.Drawing.Color.FromArgb(CType(CType(236, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(27, Byte), Integer))
        Me.pLabelTotalQ.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.pLabelTotalQ.Value = "='Q'+Quarter(Fields.OrderDate)"
        '
        'pLabelTotal
        '
        Me.pLabelTotal.Name = "pLabelTotal"
        Me.pLabelTotal.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.38188979029655457R), Telerik.Reporting.Drawing.Unit.Inch(0.24000000953674316R))
        Me.pLabelTotal.Style.Color = System.Drawing.Color.FromArgb(CType(CType(236, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(27, Byte), Integer))
        Me.pLabelTotal.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.pLabelTotal.Value = "Total"
        '
        'pProductName
        '
        Me.pProductName.Name = "pProductName"
        Me.pProductName.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.5R), Telerik.Reporting.Drawing.Unit.Inch(0.24015748500823975R))
        Me.pProductName.Value = "=Fields.ProductName"
        '
        'yearDataSource
        '
        Me.yearDataSource.ConnectionString = "My.MySettings.TelerikConnectionString"
        Me.yearDataSource.Name = "yearDataSource"
        Me.yearDataSource.SelectCommand = "SELECT DISTINCT YEAR(OrderDate) AS Year" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "FROM         Sales.SalesOrderHeader" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "ORD" &
    "ER BY Year"
        '
        'detail
        '
        Me.detail.Height = Telerik.Reporting.Drawing.Unit.Inch(7.5407319068908691R)
        Me.detail.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.panel2, Me.panel1, Me.panelSalesByProduct})
        Me.detail.Name = "detail"
        Me.detail.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(5.0R)
        '
        'panel2
        '
        Me.panel2.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.panel3, Me.textBox11, Me.textBox12, Me.textBox3, Me.SalesPersonQuarterBar, Me.SalesPersonQuarterPie})
        Me.panel2.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.000078837074397597462R))
        Me.panel2.Name = "panel2"
        Me.panel2.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(10.40000057220459R), Telerik.Reporting.Drawing.Unit.Inch(2.5R))
        '
        'panel3
        '
        Me.panel3.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.SalesPersonQuarter, Me.textBox2})
        Me.panel3.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.47999998927116394R))
        Me.panel3.Name = "panel3"
        Me.panel3.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.5999999046325684R), Telerik.Reporting.Drawing.Unit.Inch(2.0199999809265137R))
        '
        'SalesPersonQuarter
        '
        Me.SalesPersonQuarter.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.38188984990119934R)))
        Me.SalesPersonQuarter.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.38188984990119934R)))
        Me.SalesPersonQuarter.Body.Rows.Add(New Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Inch(0.24015745520591736R)))
        Me.SalesPersonQuarter.Body.SetCellContent(0, 0, Me.spTotalQ)
        Me.SalesPersonQuarter.Body.SetCellContent(0, 1, Me.spTotal)
        TableGroup1.Groupings.Add(New Telerik.Reporting.Grouping("=Quarter(Fields.OrderDate)"))
        TableGroup1.Name = "Quater"
        TableGroup1.ReportItem = Me.spLabelTotalQ
        TableGroup1.Sortings.Add(New Telerik.Reporting.Sorting("=Quarter(Fields.OrderDate)", Telerik.Reporting.SortDirection.Asc))
        TableGroup2.ReportItem = Me.spLabelTotal
        Me.SalesPersonQuarter.ColumnGroups.Add(TableGroup1)
        Me.SalesPersonQuarter.ColumnGroups.Add(TableGroup2)
        Me.SalesPersonQuarter.Corner.SetCellContent(0, 0, Me.spTextCorner)
        Me.SalesPersonQuarter.DataSource = Me.mainDataSource
        Me.SalesPersonQuarter.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.spTextCorner, Me.spTotalQ, Me.spTotal, Me.spLabelTotalQ, Me.spLabelTotal, Me.spLabelSalesPerson})
        Me.SalesPersonQuarter.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.31999999284744263R))
        Me.SalesPersonQuarter.Name = "SalesPersonQuarter"
        TableGroup3.Filters.Add(New Telerik.Reporting.Filter("=Sum(Fields.LineTotal)", Telerik.Reporting.FilterOperator.TopN, "=5"))
        TableGroup3.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.SalesPersonFullName"))
        TableGroup3.Name = "SalesPersonFullName"
        TableGroup3.ReportItem = Me.spLabelSalesPerson
        TableGroup3.Sortings.Add(New Telerik.Reporting.Sorting("=Sum(Fields.LineTotal)", Telerik.Reporting.SortDirection.Desc))
        Me.SalesPersonQuarter.RowGroups.Add(TableGroup3)
        Me.SalesPersonQuarter.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.2637796401977539R), Telerik.Reporting.Drawing.Unit.Inch(0.48015749454498291R))
        Me.SalesPersonQuarter.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None
        Me.SalesPersonQuarter.Style.Font.Name = "Segoe UI"
        Me.SalesPersonQuarter.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8.0R)
        Me.SalesPersonQuarter.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        '
        'spTotalQ
        '
        Me.spTotalQ.Format = "{0:#.}"
        Me.spTotalQ.Name = "spTotalQ"
        Me.spTotalQ.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.38188979029655457R), Telerik.Reporting.Drawing.Unit.Inch(0.24015748500823975R))
        Me.spTotalQ.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.spTotalQ.Value = "=Sum(Fields.LineTotal)"
        '
        'spTotal
        '
        Me.spTotal.Format = "{0:#.}"
        Me.spTotal.Name = "spTotal"
        Me.spTotal.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.38188979029655457R), Telerik.Reporting.Drawing.Unit.Inch(0.24015748500823975R))
        Me.spTotal.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.spTotal.Value = "=Sum(Fields.LineTotal)"
        '
        'spTextCorner
        '
        Me.spTextCorner.Name = "spTextCorner"
        Me.spTextCorner.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.5R), Telerik.Reporting.Drawing.Unit.Inch(0.23999999463558197R))
        Me.spTextCorner.Style.Color = System.Drawing.Color.FromArgb(CType(CType(105, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(40, Byte), Integer))
        Me.spTextCorner.Value = "Sales Person"
        '
        'mainDataSource
        '
        Me.mainDataSource.ConnectionString = "My.MySettings.TelerikConnectionString"
        Me.mainDataSource.Name = "mainDataSource"
        Me.mainDataSource.Parameters.AddRange(New Telerik.Reporting.SqlDataSourceParameter() {New Telerik.Reporting.SqlDataSourceParameter("@Year", System.Data.DbType.Int32, "=Parameters.ReportYear.Value")})
        Me.mainDataSource.SelectCommand = resources.GetString("mainDataSource.SelectCommand")
        '
        'textBox2
        '
        Me.textBox2.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.0R))
        Me.textBox2.Name = "textBox2"
        Me.textBox2.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.5999999046325684R), Telerik.Reporting.Drawing.Unit.Inch(0.31999999284744263R))
        Me.textBox2.StyleName = "legend"
        Me.textBox2.Value = "SALES AMOUNT IN USD (THOUSANDS)"
        '
        'textBox11
        '
        Me.textBox11.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.5999999046325684R), Telerik.Reporting.Drawing.Unit.Inch(0.47999998927116394R))
        Me.textBox11.Name = "textBox11"
        Me.textBox11.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.0799999237060547R), Telerik.Reporting.Drawing.Unit.Inch(0.31999999284744263R))
        Me.textBox11.StyleName = "legend"
        Me.textBox11.Value = "YEARLY SALES DISTRIBUTION"
        '
        'textBox12
        '
        Me.textBox12.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.679999828338623R), Telerik.Reporting.Drawing.Unit.Inch(0.47999998927116394R))
        Me.textBox12.Name = "textBox12"
        Me.textBox12.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.7100000381469727R), Telerik.Reporting.Drawing.Unit.Inch(0.31999999284744263R))
        Me.textBox12.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.textBox12.StyleName = "legend"
        Me.textBox12.Value = "QUARTERLY SALES DISTRIBUTION"
        '
        'textBox3
        '
        Me.textBox3.Anchoring = CType((Telerik.Reporting.AnchoringStyles.Left Or Telerik.Reporting.AnchoringStyles.Right), Telerik.Reporting.AnchoringStyles)
        Me.textBox3.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.0R))
        Me.textBox3.Name = "textBox3"
        Me.textBox3.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(10.40000057220459R), Telerik.Reporting.Drawing.Unit.Inch(0.36000001430511475R))
        Me.textBox3.Style.BorderColor.Bottom = System.Drawing.Color.FromArgb(CType(CType(202, Byte), Integer), CType(CType(137, Byte), Integer), CType(CType(104, Byte), Integer))
        Me.textBox3.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBox3.Style.Color = System.Drawing.Color.FromArgb(CType(CType(236, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(27, Byte), Integer))
        Me.textBox3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(16.0R)
        Me.textBox3.TocText = "Top 5 Performing Agents"
        Me.textBox3.Value = "Top 5 performing agents"
        '
        'SalesPersonQuarterBar
        '
        GraphGroup1.Filters.Add(New Telerik.Reporting.Filter("=Sum(Fields.LineTotal)", Telerik.Reporting.FilterOperator.TopN, "=5"))
        GraphGroup1.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.SalesPersonFullName"))
        GraphGroup1.Label = "= Fields.SalesPersonFullName"
        GraphGroup1.Name = "SalesPersonGroup"
        GraphGroup1.Sortings.Add(New Telerik.Reporting.Sorting("=Sum(Fields.LineTotal)", Telerik.Reporting.SortDirection.Asc))
        Me.SalesPersonQuarterBar.CategoryGroups.Add(GraphGroup1)
        ColorPalette1.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(105, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(40, Byte), Integer)))
        ColorPalette1.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(196, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(0, Byte), Integer)))
        ColorPalette1.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(147, Byte), Integer), CType(CType(30, Byte), Integer)))
        ColorPalette1.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(177, Byte), Integer), CType(CType(161, Byte), Integer), CType(CType(82, Byte), Integer)))
        ColorPalette1.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(211, Byte), Integer), CType(CType(191, Byte), Integer), CType(CType(105, Byte), Integer)))
        Me.SalesPersonQuarterBar.ColorPalette = ColorPalette1
        Me.SalesPersonQuarterBar.CoordinateSystems.Add(Me.cartesianCoordinateSystem1)
        Me.SalesPersonQuarterBar.DataSource = Me.mainDataSource
        Me.SalesPersonQuarterBar.Legend.Position = Telerik.Reporting.GraphItemPosition.TopCenter
        Me.SalesPersonQuarterBar.Legend.Style.LineColor = System.Drawing.Color.LightGray
        Me.SalesPersonQuarterBar.Legend.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.SalesPersonQuarterBar.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(169.66999816894531R), Telerik.Reporting.Drawing.Unit.Mm(20.319999694824219R))
        Me.SalesPersonQuarterBar.Name = "SalesPersonQuarterBar"
        Me.SalesPersonQuarterBar.PlotAreaStyle.LineColor = System.Drawing.Color.LightGray
        Me.SalesPersonQuarterBar.PlotAreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.SalesPersonQuarterBar.Series.Add(Me.barSeries1)
        Me.SalesPersonQuarterBar.SeriesGroups.Add(GraphGroup2)
        Me.SalesPersonQuarterBar.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(94.489997863769531R), Telerik.Reporting.Drawing.Unit.Mm(43.180000305175781R))
        Me.SalesPersonQuarterBar.Style.Font.Name = "Arial"
        Me.SalesPersonQuarterBar.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8.0R)
        Me.SalesPersonQuarterBar.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(5.0R)
        Me.SalesPersonQuarterBar.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        Me.SalesPersonQuarterBar.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        Me.SalesPersonQuarterBar.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        '
        'cartesianCoordinateSystem1
        '
        Me.cartesianCoordinateSystem1.Name = "cartesianCoordinateSystem1"
        Me.cartesianCoordinateSystem1.XAxis = Me.graphAxis3
        Me.cartesianCoordinateSystem1.YAxis = Me.graphAxis4
        '
        'graphAxis3
        '
        Me.graphAxis3.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis3.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis3.MajorGridLineStyle.Visible = False
        Me.graphAxis3.MajorTickMarkDisplayType = Telerik.Reporting.GraphAxisTickMarkDisplayType.Outside
        Me.graphAxis3.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis3.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis3.MinorGridLineStyle.Visible = False
        Me.graphAxis3.Name = "graphAxis3"
        Me.graphAxis3.Scale = NumericalScale1
        Me.graphAxis3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8.0R)
        '
        'graphAxis4
        '
        Me.graphAxis4.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis4.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis4.MajorGridLineStyle.Visible = False
        Me.graphAxis4.MajorTickMarkDisplayType = Telerik.Reporting.GraphAxisTickMarkDisplayType.None
        Me.graphAxis4.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis4.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis4.MinorGridLineStyle.Visible = False
        Me.graphAxis4.Name = "graphAxis4"
        CategoryScale1.SpacingSlotCount = 0.4R
        Me.graphAxis4.Scale = CategoryScale1
        Me.graphAxis4.Size = Telerik.Reporting.Drawing.Unit.Inch(1.2999999523162842R)
        Me.graphAxis4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7.0R)
        Me.graphAxis4.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.graphAxis4.Style.Visible = True
        '
        'barSeries1
        '
        Me.barSeries1.ArrangeMode = Telerik.Reporting.GraphSeriesArrangeMode.Stacked
        Me.barSeries1.CategoryGroup = GraphGroup1
        Me.barSeries1.CoordinateSystem = Me.cartesianCoordinateSystem1
        Me.barSeries1.DataPointLabel = "=Sum(Fields.LineTotal)"
        Me.barSeries1.DataPointLabelStyle.Visible = False
        Me.barSeries1.DataPointStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.barSeries1.DataPointStyle.Visible = True
        Me.barSeries1.LegendItem.Value = "='Q' + Quarter(Fields.OrderDate)"
        GraphGroup2.Groupings.Add(New Telerik.Reporting.Grouping("= Quarter(Fields.OrderDate)"))
        GraphGroup2.Label = "='Q' + Quarter(Fields.OrderDate)"
        GraphGroup2.Name = "OrderDate.QuarterGroup"
        GraphGroup2.Sortings.Add(New Telerik.Reporting.Sorting("= Quarter(Fields.OrderDate)", Telerik.Reporting.SortDirection.Asc))
        Me.barSeries1.SeriesGroup = GraphGroup2
        Me.barSeries1.X = "=Sum(Fields.LineTotal)"
        Me.barSeries1.ToolTip.Title = "=Format('Q{0} results for {1}', Quarter(Fields.OrderDate), Fields.SalesPersonFullName)"
        Me.barSeries1.ToolTip.Text = "=Format('${0:N2} K', Sum(Fields.LineTotal))"
        '
        'panel1
        '
        Me.panel1.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.panel4, Me.textBox13, Me.textBox14, Me.textBox7, Me.StoreQuarterBar, Me.Graph1})
        Me.panel1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(2.5001578330993652R))
        Me.panel1.Name = "panel1"
        Me.panel1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(10.399999618530273R), Telerik.Reporting.Drawing.Unit.Inch(2.5199999809265137R))
        '
        'panel4
        '
        Me.panel4.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.StoreQuarter, Me.textBox9})
        Me.panel4.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.5R))
        Me.panel4.Name = "panel4"
        Me.panel4.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.5999999046325684R), Telerik.Reporting.Drawing.Unit.Inch(2.0199999809265137R))
        '
        'StoreQuarter
        '
        Me.StoreQuarter.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.38188984990119934R)))
        Me.StoreQuarter.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.38188984990119934R)))
        Me.StoreQuarter.Body.Rows.Add(New Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Inch(0.24015745520591736R)))
        Me.StoreQuarter.Body.SetCellContent(0, 0, Me.sTotalQ)
        Me.StoreQuarter.Body.SetCellContent(0, 1, Me.sTotal)
        TableGroup4.Groupings.Add(New Telerik.Reporting.Grouping("=Quarter(Fields.OrderDate)"))
        TableGroup4.Name = "Quater"
        TableGroup4.ReportItem = Me.sLabelTotalQ
        TableGroup4.Sortings.Add(New Telerik.Reporting.Sorting("=Quarter(Fields.OrderDate)", Telerik.Reporting.SortDirection.Asc))
        TableGroup5.ReportItem = Me.sLabelTotal
        Me.StoreQuarter.ColumnGroups.Add(TableGroup4)
        Me.StoreQuarter.ColumnGroups.Add(TableGroup5)
        Me.StoreQuarter.Corner.SetCellContent(0, 0, Me.sTextCorner)
        Me.StoreQuarter.DataSource = Me.mainDataSource
        Me.StoreQuarter.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.sTextCorner, Me.sTotalQ, Me.sTotal, Me.sLabelTotalQ, Me.sLabelTotal, Me.sLabelStore})
        Me.StoreQuarter.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.31999999284744263R))
        Me.StoreQuarter.Name = "StoreQuarter"
        TableGroup6.Filters.Add(New Telerik.Reporting.Filter("=Sum(Fields.LineTotal)", Telerik.Reporting.FilterOperator.TopN, "=5"))
        TableGroup6.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.StoreName"))
        TableGroup6.Name = "StoreName"
        TableGroup6.ReportItem = Me.sLabelStore
        TableGroup6.Sortings.Add(New Telerik.Reporting.Sorting("=Sum(Fields.LineTotal)", Telerik.Reporting.SortDirection.Desc))
        Me.StoreQuarter.RowGroups.Add(TableGroup6)
        Me.StoreQuarter.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.2637796401977539R), Telerik.Reporting.Drawing.Unit.Inch(0.48015749454498291R))
        Me.StoreQuarter.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8.0R)
        '
        'sTotalQ
        '
        Me.sTotalQ.Format = "{0:#.}"
        Me.sTotalQ.Name = "sTotalQ"
        Me.sTotalQ.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.38188979029655457R), Telerik.Reporting.Drawing.Unit.Inch(0.24015748500823975R))
        Me.sTotalQ.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.sTotalQ.Value = "=Sum(Fields.LineTotal)"
        '
        'sTotal
        '
        Me.sTotal.Format = "{0:#.}"
        Me.sTotal.Name = "sTotal"
        Me.sTotal.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.38188979029655457R), Telerik.Reporting.Drawing.Unit.Inch(0.24015748500823975R))
        Me.sTotal.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.sTotal.Value = "=Sum(Fields.LineTotal)"
        '
        'sTextCorner
        '
        Me.sTextCorner.Name = "sTextCorner"
        Me.sTextCorner.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.5R), Telerik.Reporting.Drawing.Unit.Inch(0.23999999463558197R))
        Me.sTextCorner.Style.Color = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(105, Byte), Integer), CType(CType(104, Byte), Integer))
        Me.sTextCorner.Value = "Store"
        '
        'textBox9
        '
        Me.textBox9.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.0R))
        Me.textBox9.Name = "textBox9"
        Me.textBox9.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.7000000476837158R), Telerik.Reporting.Drawing.Unit.Inch(0.31999999284744263R))
        Me.textBox9.StyleName = "legend"
        Me.textBox9.Value = "SALES AMOUNT IN USD (THOUSANDS)"
        '
        'textBox13
        '
        Me.textBox13.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.5999999046325684R), Telerik.Reporting.Drawing.Unit.Inch(0.5R))
        Me.textBox13.Name = "textBox13"
        Me.textBox13.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.0799999237060547R), Telerik.Reporting.Drawing.Unit.Inch(0.31999999284744263R))
        Me.textBox13.StyleName = "legend"
        Me.textBox13.Value = "YEARLY SALES DISTRIBUTION"
        '
        'textBox14
        '
        Me.textBox14.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.679999828338623R), Telerik.Reporting.Drawing.Unit.Inch(0.5R))
        Me.textBox14.Name = "textBox14"
        Me.textBox14.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.7200000286102295R), Telerik.Reporting.Drawing.Unit.Inch(0.31999999284744263R))
        Me.textBox14.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.textBox14.StyleName = "legend"
        Me.textBox14.Value = "QUARTERLY SALES DISTRIBUTION"
        '
        'textBox7
        '
        Me.textBox7.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.0R))
        Me.textBox7.Name = "textBox7"
        Me.textBox7.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(10.399999618530273R), Telerik.Reporting.Drawing.Unit.Inch(0.36000001430511475R))
        Me.textBox7.Style.BorderColor.Bottom = System.Drawing.Color.FromArgb(CType(CType(202, Byte), Integer), CType(CType(137, Byte), Integer), CType(CType(104, Byte), Integer))
        Me.textBox7.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBox7.Style.Color = System.Drawing.Color.FromArgb(CType(CType(236, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(27, Byte), Integer))
        Me.textBox7.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(16.0R)
        Me.textBox7.TocText = "Top 5 Performing Stores"
        Me.textBox7.Value = "Top 5 performing stores"
        '
        'StoreQuarterBar
        '
        GraphGroup5.Filters.Add(New Telerik.Reporting.Filter("=Sum(Fields.LineTotal)", Telerik.Reporting.FilterOperator.TopN, "=5"))
        GraphGroup5.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.StoreName"))
        GraphGroup5.Name = "ProductNameGroup"
        GraphGroup5.Sortings.Add(New Telerik.Reporting.Sorting("=Sum(Fields.LineTotal)", Telerik.Reporting.SortDirection.Asc))
        Me.StoreQuarterBar.CategoryGroups.Add(GraphGroup5)
        ColorPalette3.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(105, Byte), Integer), CType(CType(104, Byte), Integer)))
        ColorPalette3.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(88, Byte), Integer), CType(CType(168, Byte), Integer), CType(CType(35, Byte), Integer)))
        ColorPalette3.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(137, Byte), Integer), CType(CType(203, Byte), Integer), CType(CType(42, Byte), Integer)))
        ColorPalette3.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(34, Byte), Integer), CType(CType(181, Byte), Integer), CType(CType(115, Byte), Integer)))
        ColorPalette3.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(176, Byte), Integer), CType(CType(212, Byte), Integer)))
        Me.StoreQuarterBar.ColorPalette = ColorPalette3
        Me.StoreQuarterBar.CoordinateSystems.Add(Me.cartesianCoordinateSystem2)
        Me.StoreQuarterBar.DataSource = Me.mainDataSource
        Me.StoreQuarterBar.Legend.Position = Telerik.Reporting.GraphItemPosition.TopCenter
        Me.StoreQuarterBar.Legend.Style.LineColor = System.Drawing.Color.LightGray
        Me.StoreQuarterBar.Legend.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.StoreQuarterBar.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(169.66999816894531R), Telerik.Reporting.Drawing.Unit.Mm(20.829999923706055R))
        Me.StoreQuarterBar.Name = "StoreQuarterBar"
        Me.StoreQuarterBar.PlotAreaStyle.LineColor = System.Drawing.Color.LightGray
        Me.StoreQuarterBar.PlotAreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.StoreQuarterBar.Series.Add(Me.barSeries3)
        Me.StoreQuarterBar.SeriesGroups.Add(GraphGroup6)
        Me.StoreQuarterBar.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(94.489997863769531R), Telerik.Reporting.Drawing.Unit.Mm(43.180000305175781R))
        Me.StoreQuarterBar.Style.Font.Name = "Segoe UI"
        Me.StoreQuarterBar.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8.0R)
        Me.StoreQuarterBar.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(5.0R)
        Me.StoreQuarterBar.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        Me.StoreQuarterBar.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        Me.StoreQuarterBar.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        '
        'cartesianCoordinateSystem2
        '
        Me.cartesianCoordinateSystem2.Name = "cartesianCoordinateSystem1"
        Me.cartesianCoordinateSystem2.XAxis = Me.graphAxis5
        Me.cartesianCoordinateSystem2.YAxis = Me.graphAxis6
        '
        'graphAxis5
        '
        Me.graphAxis5.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis5.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis5.MajorGridLineStyle.Visible = False
        Me.graphAxis5.MajorTickMarkDisplayType = Telerik.Reporting.GraphAxisTickMarkDisplayType.Outside
        Me.graphAxis5.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis5.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis5.MinorGridLineStyle.Visible = False
        Me.graphAxis5.Name = "graphAxis3"
        Me.graphAxis5.Scale = NumericalScale3
        Me.graphAxis5.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8.0R)
        '
        'graphAxis6
        '
        Me.graphAxis6.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis6.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis6.MajorGridLineStyle.Visible = False
        Me.graphAxis6.MajorTickMarkDisplayType = Telerik.Reporting.GraphAxisTickMarkDisplayType.None
        Me.graphAxis6.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis6.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis6.MinorGridLineStyle.Visible = False
        Me.graphAxis6.Name = "graphAxis4"
        CategoryScale3.SpacingSlotCount = 0.4R
        Me.graphAxis6.Scale = CategoryScale3
        Me.graphAxis6.Size = Telerik.Reporting.Drawing.Unit.Inch(1.2999999523162842R)
        Me.graphAxis6.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7.0R)
        Me.graphAxis6.Style.Visible = True
        '
        'barSeries3
        '
        Me.barSeries3.ArrangeMode = Telerik.Reporting.GraphSeriesArrangeMode.Stacked
        Me.barSeries3.CategoryGroup = GraphGroup5
        Me.barSeries3.CoordinateSystem = Me.cartesianCoordinateSystem2
        Me.barSeries3.DataPointLabel = "=Sum(Fields.LineTotal)"
        Me.barSeries3.DataPointLabelStyle.Visible = False
        Me.barSeries3.DataPointStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.barSeries3.DataPointStyle.Visible = True
        Me.barSeries3.LegendItem.Value = "='Q' + Quarter(Fields.OrderDate)"
        GraphGroup6.Groupings.Add(New Telerik.Reporting.Grouping("= Quarter(Fields.OrderDate)"))
        GraphGroup6.Label = "='Q' + Quarter(Fields.OrderDate)"
        GraphGroup6.Name = "OrderDate.QuarterGroup"
        GraphGroup6.Sortings.Add(New Telerik.Reporting.Sorting("= Quarter(Fields.OrderDate)", Telerik.Reporting.SortDirection.Asc))
        Me.barSeries3.SeriesGroup = GraphGroup6
        Me.barSeries3.X = "=Sum(Fields.LineTotal)"
        Me.barSeries3.ToolTip.Title = "=Format('Q{0} results for {1}', Quarter(Fields.OrderDate), Fields.StoreName)"
        Me.barSeries3.ToolTip.Text = "=Format('${0:N2} K', Sum(Fields.LineTotal))"
        '
        'panelSalesByProduct
        '
        Me.panelSalesByProduct.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.panel6, Me.textBox15, Me.textBox16, Me.textBox8, Me.ProductQuarterBar, Me.ProductQuarterPie})
        Me.panelSalesByProduct.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(5.0203156471252441R))
        Me.panelSalesByProduct.Name = "panelSalesByProduct"
        Me.panelSalesByProduct.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(10.399999618530273R), Telerik.Reporting.Drawing.Unit.Inch(2.520416259765625R))
        '
        'panel6
        '
        Me.panel6.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.ProductQuarter, Me.textBox10})
        Me.panel6.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.5R))
        Me.panel6.Name = "panel6"
        Me.panel6.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.5999999046325684R), Telerik.Reporting.Drawing.Unit.Inch(2.0199999809265137R))
        '
        'ProductQuarter
        '
        Me.ProductQuarter.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.38188984990119934R)))
        Me.ProductQuarter.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.38188984990119934R)))
        Me.ProductQuarter.Body.Rows.Add(New Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Inch(0.24015745520591736R)))
        Me.ProductQuarter.Body.SetCellContent(0, 0, Me.pTotalQ)
        Me.ProductQuarter.Body.SetCellContent(0, 1, Me.pTotal)
        TableGroup7.Groupings.Add(New Telerik.Reporting.Grouping("=Quarter(Fields.OrderDate)"))
        TableGroup7.Name = "Quater"
        TableGroup7.ReportItem = Me.pLabelTotalQ
        TableGroup7.Sortings.Add(New Telerik.Reporting.Sorting("=Quarter(Fields.OrderDate)", Telerik.Reporting.SortDirection.Asc))
        TableGroup8.ReportItem = Me.pLabelTotal
        Me.ProductQuarter.ColumnGroups.Add(TableGroup7)
        Me.ProductQuarter.ColumnGroups.Add(TableGroup8)
        Me.ProductQuarter.Corner.SetCellContent(0, 0, Me.pTextCorner)
        Me.ProductQuarter.DataSource = Me.mainDataSource
        Me.ProductQuarter.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.pTextCorner, Me.pTotalQ, Me.pTotal, Me.pLabelTotalQ, Me.pLabelTotal, Me.pProductName})
        Me.ProductQuarter.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.31999999284744263R))
        Me.ProductQuarter.Name = "ProductQuarter"
        TableGroup9.Filters.Add(New Telerik.Reporting.Filter("=Sum(Fields.LineTotal)", Telerik.Reporting.FilterOperator.TopN, "=5"))
        TableGroup9.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.ProductName"))
        TableGroup9.Name = "ProductNameGroup"
        TableGroup9.ReportItem = Me.pProductName
        TableGroup9.Sortings.Add(New Telerik.Reporting.Sorting("=Sum(Fields.LineTotal)", Telerik.Reporting.SortDirection.Desc))
        Me.ProductQuarter.RowGroups.Add(TableGroup9)
        Me.ProductQuarter.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.2637796401977539R), Telerik.Reporting.Drawing.Unit.Inch(0.48015749454498291R))
        Me.ProductQuarter.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8.0R)
        '
        'pTotalQ
        '
        Me.pTotalQ.Format = "{0:#.}"
        Me.pTotalQ.Name = "pTotalQ"
        Me.pTotalQ.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.38188979029655457R), Telerik.Reporting.Drawing.Unit.Inch(0.24015748500823975R))
        Me.pTotalQ.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.pTotalQ.Value = "=Sum(Fields.LineTotal)"
        '
        'pTotal
        '
        Me.pTotal.Format = "{0:#.}"
        Me.pTotal.Name = "pTotal"
        Me.pTotal.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.38188979029655457R), Telerik.Reporting.Drawing.Unit.Inch(0.24015748500823975R))
        Me.pTotal.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.pTotal.Value = "=Sum(Fields.LineTotal)"
        '
        'pTextCorner
        '
        Me.pTextCorner.Name = "pTextCorner"
        Me.pTextCorner.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.5R), Telerik.Reporting.Drawing.Unit.Inch(0.23999999463558197R))
        Me.pTextCorner.Style.Color = System.Drawing.Color.FromArgb(CType(CType(236, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(27, Byte), Integer))
        Me.pTextCorner.Value = "Product"
        '
        'textBox10
        '
        Me.textBox10.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.0R))
        Me.textBox10.Name = "textBox10"
        Me.textBox10.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.7000000476837158R), Telerik.Reporting.Drawing.Unit.Inch(0.31999999284744263R))
        Me.textBox10.StyleName = "legend"
        Me.textBox10.Value = "SALES AMOUNT IN USD (THOUSANDS)"
        '
        'textBox15
        '
        Me.textBox15.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.5999999046325684R), Telerik.Reporting.Drawing.Unit.Inch(0.5R))
        Me.textBox15.Name = "textBox15"
        Me.textBox15.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.0799999237060547R), Telerik.Reporting.Drawing.Unit.Inch(0.31999999284744263R))
        Me.textBox15.StyleName = "legend"
        Me.textBox15.Value = "YEARLY SALES DISTRIBUTION"
        '
        'textBox16
        '
        Me.textBox16.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.679999828338623R), Telerik.Reporting.Drawing.Unit.Inch(0.5R))
        Me.textBox16.Name = "textBox16"
        Me.textBox16.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.7200000286102295R), Telerik.Reporting.Drawing.Unit.Inch(0.31999999284744263R))
        Me.textBox16.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.textBox16.StyleName = "legend"
        Me.textBox16.Value = "QUARTERLY SALES DISTRIBUTION"
        '
        'textBox8
        '
        Me.textBox8.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0R), Telerik.Reporting.Drawing.Unit.Inch(0.0R))
        Me.textBox8.Name = "textBox8"
        Me.textBox8.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(10.380000114440918R), Telerik.Reporting.Drawing.Unit.Inch(0.36000001430511475R))
        Me.textBox8.Style.BorderColor.Bottom = System.Drawing.Color.FromArgb(CType(CType(202, Byte), Integer), CType(CType(137, Byte), Integer), CType(CType(104, Byte), Integer))
        Me.textBox8.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBox8.Style.Color = System.Drawing.Color.FromArgb(CType(CType(236, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(27, Byte), Integer))
        Me.textBox8.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(16.0R)
        Me.textBox8.TocText = "Top 5 Performing Products"
        Me.textBox8.Value = "Top 5 performing products"
        '
        'ProductQuarterBar
        '
        GraphGroup9.Filters.Add(New Telerik.Reporting.Filter("=Sum(Fields.LineTotal)", Telerik.Reporting.FilterOperator.TopN, "=5"))
        GraphGroup9.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.ProductName"))
        GraphGroup9.Name = "ProductNameGroup"
        GraphGroup9.Sortings.Add(New Telerik.Reporting.Sorting("=Sum(Fields.LineTotal)", Telerik.Reporting.SortDirection.Asc))
        Me.ProductQuarterBar.CategoryGroups.Add(GraphGroup9)
        ColorPalette5.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(7, Byte), Integer), CType(CType(5, Byte), Integer)))
        ColorPalette5.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(87, Byte), Integer), CType(CType(35, Byte), Integer), CType(CType(17, Byte), Integer)))
        ColorPalette5.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(11, Byte), Integer)))
        ColorPalette5.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(236, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(27, Byte), Integer)))
        ColorPalette5.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(89, Byte), Integer), CType(CType(56, Byte), Integer)))
        Me.ProductQuarterBar.ColorPalette = ColorPalette5
        Me.ProductQuarterBar.CoordinateSystems.Add(Me.cartesianCoordinateSystem3)
        Me.ProductQuarterBar.DataSource = Me.mainDataSource
        Me.ProductQuarterBar.Legend.Position = Telerik.Reporting.GraphItemPosition.TopCenter
        Me.ProductQuarterBar.Legend.Style.LineColor = System.Drawing.Color.LightGray
        Me.ProductQuarterBar.Legend.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.ProductQuarterBar.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(169.66999816894531R), Telerik.Reporting.Drawing.Unit.Mm(20.829999923706055R))
        Me.ProductQuarterBar.Name = "ProductQuarterBar"
        Me.ProductQuarterBar.PlotAreaStyle.LineColor = System.Drawing.Color.LightGray
        Me.ProductQuarterBar.PlotAreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.ProductQuarterBar.Series.Add(Me.barSeries6)
        Me.ProductQuarterBar.SeriesGroups.Add(GraphGroup10)
        Me.ProductQuarterBar.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(94.489997863769531R), Telerik.Reporting.Drawing.Unit.Mm(43.180000305175781R))
        Me.ProductQuarterBar.Style.Font.Name = "Segoe UI"
        Me.ProductQuarterBar.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8.0R)
        Me.ProductQuarterBar.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(5.0R)
        Me.ProductQuarterBar.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        Me.ProductQuarterBar.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        Me.ProductQuarterBar.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        '
        'cartesianCoordinateSystem3
        '
        Me.cartesianCoordinateSystem3.Name = "cartesianCoordinateSystem1"
        Me.cartesianCoordinateSystem3.XAxis = Me.graphAxis11
        Me.cartesianCoordinateSystem3.YAxis = Me.graphAxis12
        '
        'graphAxis11
        '
        Me.graphAxis11.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis11.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis11.MajorGridLineStyle.Visible = False
        Me.graphAxis11.MajorTickMarkDisplayType = Telerik.Reporting.GraphAxisTickMarkDisplayType.Outside
        Me.graphAxis11.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis11.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis11.MinorGridLineStyle.Visible = False
        Me.graphAxis11.Name = "graphAxis3"
        Me.graphAxis11.Scale = NumericalScale5
        Me.graphAxis11.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8.0R)
        Me.graphAxis11.TitlePlacement = Telerik.Reporting.GraphAxisTitlePlacement.[Auto]
        '
        'graphAxis12
        '
        Me.graphAxis12.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis12.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis12.MajorGridLineStyle.Visible = False
        Me.graphAxis12.MajorTickMarkDisplayType = Telerik.Reporting.GraphAxisTickMarkDisplayType.None
        Me.graphAxis12.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis12.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis12.MinorGridLineStyle.Visible = False
        Me.graphAxis12.Name = "graphAxis4"
        CategoryScale5.SpacingSlotCount = 0.4R
        Me.graphAxis12.Scale = CategoryScale5
        Me.graphAxis12.Size = Telerik.Reporting.Drawing.Unit.Inch(1.2999999523162842R)
        Me.graphAxis12.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7.0R)
        Me.graphAxis12.Style.Visible = True
        '
        'barSeries6
        '
        Me.barSeries6.ArrangeMode = Telerik.Reporting.GraphSeriesArrangeMode.Stacked
        Me.barSeries6.CategoryGroup = GraphGroup9
        Me.barSeries6.CoordinateSystem = Me.cartesianCoordinateSystem3
        Me.barSeries6.DataPointLabel = "=Sum(Fields.LineTotal)"
        Me.barSeries6.DataPointLabelStyle.Visible = False
        Me.barSeries6.DataPointStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.barSeries6.DataPointStyle.Visible = True
        Me.barSeries6.LegendItem.Value = "='Q' + Quarter(Fields.OrderDate)"
        GraphGroup10.Groupings.Add(New Telerik.Reporting.Grouping("= Quarter(Fields.OrderDate)"))
        GraphGroup10.Label = "='Q' + Quarter(Fields.OrderDate)"
        GraphGroup10.Name = "OrderDate.QuarterGroup"
        GraphGroup10.Sortings.Add(New Telerik.Reporting.Sorting("= Quarter(Fields.OrderDate)", Telerik.Reporting.SortDirection.Asc))
        Me.barSeries6.SeriesGroup = GraphGroup10
        Me.barSeries6.X = "=Sum(Fields.LineTotal)"
        Me.barSeries6.ToolTip.Title = "=Format('Q{0} results for {1}', Quarter(Fields.OrderDate), Fields.ProductName)"
        Me.barSeries6.ToolTip.Text = "=Format('${0:N2} K', Sum(Fields.LineTotal))"
        '
        'textBox5
        '
        Me.textBox5.Name = "textBox5"
        Me.textBox5.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.4895833432674408R), Telerik.Reporting.Drawing.Unit.Inch(0.17708373069763184R))
        '
        'reportHeaderSection1
        '
        Me.reportHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.70003944635391235R)
        Me.reportHeaderSection1.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.panel5})
        Me.reportHeaderSection1.Name = "reportHeaderSection1"
        Me.reportHeaderSection1.Style.Visible = True
        '
        'panel5
        '
        Me.panel5.Anchoring = CType((Telerik.Reporting.AnchoringStyles.Left Or Telerik.Reporting.AnchoringStyles.Right), Telerik.Reporting.AnchoringStyles)
        Me.panel5.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.textBox1, Me.shape5, Me.textBox6})
        Me.panel5.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(0.068051397800445557R), Telerik.Reporting.Drawing.Unit.Mm(0.0010012307902798057R))
        Me.panel5.Name = "panel5"
        Me.panel5.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(10.407281875610352R), Telerik.Reporting.Drawing.Unit.Mm(17.780000686645508R))
        '
        'textBox1
        '
        Me.textBox1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0026791889686137438R), Telerik.Reporting.Drawing.Unit.Inch(0.000039339065551757812R))
        Me.textBox1.Name = "textBox1"
        Me.textBox1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(4.2124595642089844R), Telerik.Reporting.Drawing.Unit.Inch(0.59992140531539917R))
        Me.textBox1.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(34.0R)
        Me.textBox1.StyleName = "Header"
        Me.textBox1.Value = "Quarterly Sales"
        '
        'shape5
        '
        Me.shape5.Anchoring = CType((Telerik.Reporting.AnchoringStyles.Left Or Telerik.Reporting.AnchoringStyles.Right), Telerik.Reporting.AnchoringStyles)
        Me.shape5.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0026791889686137438R), Telerik.Reporting.Drawing.Unit.Inch(0.60003942251205444R))
        Me.shape5.Name = "shape5"
        Me.shape5.ShapeType = New Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW)
        Me.shape5.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(10.399999618530273R), Telerik.Reporting.Drawing.Unit.Point(3.75R))
        Me.shape5.Stretch = True
        Me.shape5.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.shape5.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid
        Me.shape5.Style.BorderWidth.Top = Telerik.Reporting.Drawing.Unit.Point(3.0R)
        Me.shape5.Style.Color = System.Drawing.Color.Transparent
        '
        'textBox6
        '
        Me.textBox6.Anchoring = Telerik.Reporting.AnchoringStyles.Right
        Me.textBox6.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(8.25732135772705R), Telerik.Reporting.Drawing.Unit.Inch(0.22485215961933136R))
        Me.textBox6.Name = "textBox6"
        Me.textBox6.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1199996471405029R), Telerik.Reporting.Drawing.Unit.Inch(0.22011041641235352R))
        Me.textBox6.Style.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(11, Byte), Integer))
        Me.textBox6.Style.Font.Name = "Segoe UI"
        Me.textBox6.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.textBox6.Value = "[INTERNAL PURPOSES ONLY]"
        '
        'SalesPersonQuarterPie
        '
        GraphGroup3.Name = "categoryGroup"
        Me.SalesPersonQuarterPie.CategoryGroups.Add(GraphGroup3)
        ColorPalette2.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(105, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(40, Byte), Integer)))
        ColorPalette2.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(196, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(0, Byte), Integer)))
        ColorPalette2.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(147, Byte), Integer), CType(CType(30, Byte), Integer)))
        ColorPalette2.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(177, Byte), Integer), CType(CType(161, Byte), Integer), CType(CType(82, Byte), Integer)))
        ColorPalette2.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(211, Byte), Integer), CType(CType(191, Byte), Integer), CType(CType(105, Byte), Integer)))
        Me.SalesPersonQuarterPie.ColorPalette = ColorPalette2
        Me.SalesPersonQuarterPie.CoordinateSystems.Add(Me.polarCoordinateSystem1)
        Me.SalesPersonQuarterPie.Culture = New System.Globalization.CultureInfo("")
        Me.SalesPersonQuarterPie.DataSource = Me.mainDataSource
        Me.SalesPersonQuarterPie.Legend.Height = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.SalesPersonQuarterPie.Legend.Position = Telerik.Reporting.GraphItemPosition.LeftCenter
        Me.SalesPersonQuarterPie.Legend.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7.0R)
        Me.SalesPersonQuarterPie.Legend.Style.LineColor = System.Drawing.Color.LightGray
        Me.SalesPersonQuarterPie.Legend.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.SalesPersonQuarterPie.Legend.Width = Telerik.Reporting.Drawing.Unit.Pixel(100.0R)
        Me.SalesPersonQuarterPie.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(91.4419937133789R), Telerik.Reporting.Drawing.Unit.Mm(20.322000503540039R))
        Me.SalesPersonQuarterPie.Name = "SalesPersonQuarterPie"
        Me.SalesPersonQuarterPie.PlotAreaStyle.LineColor = System.Drawing.Color.LightGray
        Me.SalesPersonQuarterPie.PlotAreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.SalesPersonQuarterPie.Series.Add(Me.barSeries2)
        Me.SalesPersonQuarterPie.SeriesGroups.Add(GraphGroup4)
        Me.SalesPersonQuarterPie.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(78.2300033569336R), Telerik.Reporting.Drawing.Unit.Mm(43.180000305175781R))
        Me.SalesPersonQuarterPie.Style.Font.Name = "Arial"
        Me.SalesPersonQuarterPie.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(5.0R)
        Me.SalesPersonQuarterPie.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(0.0R)
        Me.SalesPersonQuarterPie.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        Me.SalesPersonQuarterPie.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(5.0R)
        '
        'polarCoordinateSystem1
        '
        Me.polarCoordinateSystem1.AngularAxis = Me.graphAxis1
        Me.polarCoordinateSystem1.Name = "polarCoordinateSystem1"
        Me.polarCoordinateSystem1.RadialAxis = Me.graphAxis2
        '
        'graphAxis1
        '
        Me.graphAxis1.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis1.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis1.MajorGridLineStyle.Visible = False
        Me.graphAxis1.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis1.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis1.MinorGridLineStyle.Visible = False
        Me.graphAxis1.Name = "graphAxis1"
        Me.graphAxis1.Scale = NumericalScale2
        Me.graphAxis1.Style.Visible = False
        '
        'graphAxis2
        '
        Me.graphAxis2.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis2.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis2.MajorGridLineStyle.Visible = False
        Me.graphAxis2.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis2.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis2.MinorGridLineStyle.Visible = False
        Me.graphAxis2.Name = "graphAxis2"
        CategoryScale2.PositionMode = Telerik.Reporting.AxisPositionMode.OnTicks
        CategoryScale2.SpacingSlotCount = 0.0R
        Me.graphAxis2.Scale = CategoryScale2
        Me.graphAxis2.Style.Visible = False
        '
        'barSeries2
        '
        Me.barSeries2.ArrangeMode = Telerik.Reporting.GraphSeriesArrangeMode.Stacked100
        Me.barSeries2.CategoryGroup = GraphGroup3
        Me.barSeries2.CoordinateSystem = Me.polarCoordinateSystem1
        Me.barSeries2.DataPointLabel = "=Sum(Fields.LineTotal)"
        Me.barSeries2.DataPointLabelStyle.Visible = False
        Me.barSeries2.DataPointStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.barSeries2.DataPointStyle.Visible = True
        Me.barSeries2.LegendItem.Value = "= Fields.SalesPersonFullName"
        GraphGroup4.Filters.Add(New Telerik.Reporting.Filter("=Sum(Fields.LineTotal)", Telerik.Reporting.FilterOperator.TopN, "=5"))
        GraphGroup4.Groupings.Add(New Telerik.Reporting.Grouping("= Fields.SalesPersonFullName"))
        GraphGroup4.Name = "SalesPersonGroup"
        GraphGroup4.Sortings.Add(New Telerik.Reporting.Sorting("= Sum(Fields.LineTotal)", Telerik.Reporting.SortDirection.Desc))
        Me.barSeries2.SeriesGroup = GraphGroup4
        Me.barSeries2.X = "=Sum(Fields.LineTotal)"
        Me.barSeries2.ToolTip.Title = "= Fields.SalesPersonFullName"
        Me.barSeries2.ToolTip.Text = "=Format('Total: ${0:N2} K', Sum(Fields.LineTotal))"
        '
        'Graph1
        '
        GraphGroup7.Name = "categoryGroup"
        Me.Graph1.CategoryGroups.Add(GraphGroup7)
        ColorPalette4.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(105, Byte), Integer), CType(CType(104, Byte), Integer)))
        ColorPalette4.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(88, Byte), Integer), CType(CType(168, Byte), Integer), CType(CType(35, Byte), Integer)))
        ColorPalette4.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(137, Byte), Integer), CType(CType(203, Byte), Integer), CType(CType(42, Byte), Integer)))
        ColorPalette4.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(34, Byte), Integer), CType(CType(181, Byte), Integer), CType(CType(115, Byte), Integer)))
        ColorPalette4.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(176, Byte), Integer), CType(CType(212, Byte), Integer)))
        Me.Graph1.ColorPalette = ColorPalette4
        Me.Graph1.CoordinateSystems.Add(Me.polarCoordinateSystem2)
        Me.Graph1.Culture = New System.Globalization.CultureInfo("")
        Me.Graph1.DataSource = Me.mainDataSource
        Me.Graph1.Legend.Height = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.Graph1.Legend.Position = Telerik.Reporting.GraphItemPosition.LeftCenter
        Me.Graph1.Legend.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7.0R)
        Me.Graph1.Legend.Style.LineColor = System.Drawing.Color.LightGray
        Me.Graph1.Legend.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.Graph1.Legend.Width = Telerik.Reporting.Drawing.Unit.Pixel(100.0R)
        Me.Graph1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(91.43798828125R), Telerik.Reporting.Drawing.Unit.Mm(20.830009460449219R))
        Me.Graph1.Name = "Graph1"
        Me.Graph1.PlotAreaStyle.LineColor = System.Drawing.Color.LightGray
        Me.Graph1.PlotAreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.Graph1.Series.Add(Me.barSeries4)
        Me.Graph1.SeriesGroups.Add(GraphGroup8)
        Me.Graph1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(78.2300033569336R), Telerik.Reporting.Drawing.Unit.Mm(43.180000305175781R))
        Me.Graph1.Style.Font.Name = "Segoe UI"
        Me.Graph1.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(5.0R)
        Me.Graph1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(0.0R)
        Me.Graph1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        Me.Graph1.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(5.0R)
        '
        'polarCoordinateSystem2
        '
        Me.polarCoordinateSystem2.AngularAxis = Me.graphAxis7
        Me.polarCoordinateSystem2.Name = "polarCoordinateSystem1"
        Me.polarCoordinateSystem2.RadialAxis = Me.graphAxis8
        '
        'graphAxis7
        '
        Me.graphAxis7.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis7.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis7.MajorGridLineStyle.Visible = False
        Me.graphAxis7.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis7.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis7.MinorGridLineStyle.Visible = False
        Me.graphAxis7.Name = "graphAxis1"
        Me.graphAxis7.Scale = NumericalScale4
        Me.graphAxis7.Style.Visible = False
        '
        'graphAxis8
        '
        Me.graphAxis8.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis8.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis8.MajorGridLineStyle.Visible = False
        Me.graphAxis8.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis8.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis8.MinorGridLineStyle.Visible = False
        Me.graphAxis8.Name = "graphAxis2"
        CategoryScale4.PositionMode = Telerik.Reporting.AxisPositionMode.OnTicks
        CategoryScale4.SpacingSlotCount = 0.0R
        Me.graphAxis8.Scale = CategoryScale4
        Me.graphAxis8.Style.Visible = False
        '
        'barSeries4
        '
        Me.barSeries4.ArrangeMode = Telerik.Reporting.GraphSeriesArrangeMode.Stacked100
        Me.barSeries4.CategoryGroup = GraphGroup7
        Me.barSeries4.CoordinateSystem = Me.polarCoordinateSystem2
        Me.barSeries4.DataPointLabel = "=Sum(Fields.LineTotal)"
        Me.barSeries4.DataPointLabelStyle.Visible = False
        Me.barSeries4.DataPointStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.barSeries4.DataPointStyle.Visible = True
        Me.barSeries4.LegendItem.Value = "= Fields.StoreName"
        GraphGroup8.Filters.Add(New Telerik.Reporting.Filter("=Sum(Fields.LineTotal)", Telerik.Reporting.FilterOperator.TopN, "=5"))
        GraphGroup8.Groupings.Add(New Telerik.Reporting.Grouping("= Fields.StoreName"))
        GraphGroup8.Name = "ProductStoreGroup"
        GraphGroup8.Sortings.Add(New Telerik.Reporting.Sorting("=Sum(Fields.LineTotal)", Telerik.Reporting.SortDirection.Desc))
        Me.barSeries4.SeriesGroup = GraphGroup8
        Me.barSeries4.X = "=Sum(Fields.LineTotal)"
        Me.barSeries4.ToolTip.Title = "=Fields.StoreName"
        Me.barSeries4.ToolTip.Text = "=Format('Total: ${0:N2} K', Sum(Fields.LineTotal))"
        '
        'ProductQuarterPie
        '
        GraphGroup11.Name = "categoryGroup"
        Me.ProductQuarterPie.CategoryGroups.Add(GraphGroup11)
        ColorPalette6.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(7, Byte), Integer), CType(CType(5, Byte), Integer)))
        ColorPalette6.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(87, Byte), Integer), CType(CType(35, Byte), Integer), CType(CType(17, Byte), Integer)))
        ColorPalette6.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(11, Byte), Integer)))
        ColorPalette6.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(236, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(27, Byte), Integer)))
        ColorPalette6.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(89, Byte), Integer), CType(CType(56, Byte), Integer)))
        Me.ProductQuarterPie.ColorPalette = ColorPalette6
        Me.ProductQuarterPie.CoordinateSystems.Add(Me.polarCoordinateSystem3)
        Me.ProductQuarterPie.Culture = New System.Globalization.CultureInfo("")
        Me.ProductQuarterPie.DataSource = Me.mainDataSource
        Me.ProductQuarterPie.Legend.Height = Telerik.Reporting.Drawing.Unit.Pixel(300.0R)
        Me.ProductQuarterPie.Legend.Position = Telerik.Reporting.GraphItemPosition.LeftCenter
        Me.ProductQuarterPie.Legend.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7.0R)
        Me.ProductQuarterPie.Legend.Style.LineColor = System.Drawing.Color.LightGray
        Me.ProductQuarterPie.Legend.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.ProductQuarterPie.Legend.Width = Telerik.Reporting.Drawing.Unit.Pixel(100.0R)
        Me.ProductQuarterPie.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(91.4419937133789R), Telerik.Reporting.Drawing.Unit.Mm(20.838569641113281R))
        Me.ProductQuarterPie.Name = "ProductQuarterPie"
        Me.ProductQuarterPie.PlotAreaStyle.LineColor = System.Drawing.Color.LightGray
        Me.ProductQuarterPie.PlotAreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.ProductQuarterPie.Series.Add(Me.barSeries5)
        Me.ProductQuarterPie.SeriesGroups.Add(GraphGroup12)
        Me.ProductQuarterPie.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(78.2300033569336R), Telerik.Reporting.Drawing.Unit.Mm(43.180000305175781R))
        Me.ProductQuarterPie.Style.Font.Name = "Segoe UI"
        Me.ProductQuarterPie.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8.0R)
        Me.ProductQuarterPie.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(5.0R)
        Me.ProductQuarterPie.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(0.0R)
        Me.ProductQuarterPie.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(10.0R)
        Me.ProductQuarterPie.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(5.0R)
        '
        'polarCoordinateSystem3
        '
        Me.polarCoordinateSystem3.AngularAxis = Me.graphAxis9
        Me.polarCoordinateSystem3.Name = "polarCoordinateSystem1"
        Me.polarCoordinateSystem3.RadialAxis = Me.graphAxis10
        '
        'graphAxis9
        '
        Me.graphAxis9.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis9.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis9.MajorGridLineStyle.Visible = False
        Me.graphAxis9.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis9.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis9.MinorGridLineStyle.Visible = False
        Me.graphAxis9.Name = "graphAxis1"
        Me.graphAxis9.Scale = NumericalScale6
        Me.graphAxis9.Style.Visible = False
        '
        'graphAxis10
        '
        Me.graphAxis10.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis10.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis10.MajorGridLineStyle.Visible = False
        Me.graphAxis10.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis10.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis10.MinorGridLineStyle.Visible = False
        Me.graphAxis10.Name = "graphAxis2"
        CategoryScale6.PositionMode = Telerik.Reporting.AxisPositionMode.OnTicks
        CategoryScale6.SpacingSlotCount = 0.0R
        Me.graphAxis10.Scale = CategoryScale6
        Me.graphAxis10.Style.Visible = False
        '
        'barSeries5
        '
        Me.barSeries5.ArrangeMode = Telerik.Reporting.GraphSeriesArrangeMode.Stacked100
        Me.barSeries5.CategoryGroup = GraphGroup11
        Me.barSeries5.CoordinateSystem = Me.polarCoordinateSystem3
        Me.barSeries5.DataPointLabel = "=Sum(Fields.LineTotal)"
        Me.barSeries5.DataPointLabelStyle.Visible = False
        Me.barSeries5.DataPointStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0.0R)
        Me.barSeries5.DataPointStyle.Visible = True
        Me.barSeries5.LegendItem.Value = "=Fields.ProductName"
        GraphGroup12.Filters.Add(New Telerik.Reporting.Filter("=Sum(Fields.LineTotal)", Telerik.Reporting.FilterOperator.TopN, "=5"))
        GraphGroup12.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.ProductName"))
        GraphGroup12.Name = "ProductNameGroup"
        GraphGroup12.Sortings.Add(New Telerik.Reporting.Sorting("=Fields.ProductName", Telerik.Reporting.SortDirection.Asc))
        Me.barSeries5.SeriesGroup = GraphGroup12
        Me.barSeries5.X = "=Sum(Fields.LineTotal)"
        Me.barSeries5.ToolTip.Title = "=Fields.ProductName"
        Me.barSeries5.ToolTip.Text = "=Format('Total: ${0:N2} K', Sum(Fields.LineTotal))"
        '
        'Dashboard
        '
        Me.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.detail, Me.reportHeaderSection1})
        Me.Name = "Dashboard"
        Me.PageSettings.Landscape = True
        Me.PageSettings.Margins = New Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224R), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224R), Telerik.Reporting.Drawing.Unit.Inch(0.10000000149011612R), Telerik.Reporting.Drawing.Unit.Inch(0.0R))
        Me.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Letter
        ReportParameter1.AutoRefresh = True
        ReportParameter1.AvailableValues.DataSource = Me.yearDataSource
        ReportParameter1.AvailableValues.DisplayMember = "Year"
        ReportParameter1.AvailableValues.ValueMember = "Year"
        ReportParameter1.Name = "ReportYear"
        ReportParameter1.Text = "Sales for Year"
        ReportParameter1.Type = Telerik.Reporting.ReportParameterType.[Integer]
        ReportParameter1.Value = "=2001"
        ReportParameter1.Visible = True
        Me.ReportParameters.Add(ReportParameter1)
        StyleRule1.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.TextItemBase)), New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.HtmlTextBox))})
        StyleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        StyleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        StyleRule2.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.StyleSelector("Header")})
        StyleRule2.Style.Font.Bold = True
        StyleRule2.Style.Font.Name = "Segoe UI Light"
        StyleRule2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(25.0R)
        StyleRule3.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.TextBox))})
        StyleRule3.Style.Font.Name = "Segoe UI Light"
        StyleRule3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10.0R)
        StyleRule3.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(21.0R)
        StyleRule4.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.StyleSelector("legend")})
        StyleRule4.Style.Color = System.Drawing.Color.FromArgb(CType(CType(152, Byte), Integer), CType(CType(152, Byte), Integer), CType(CType(152, Byte), Integer))
        StyleRule4.Style.Font.Name = "Segoe UI Light"
        StyleRule4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        Me.StyleSheet.AddRange(New Telerik.Reporting.Drawing.StyleRule() {StyleRule1, StyleRule2, StyleRule3, StyleRule4})
        Me.TocText = "Dashboard"
        Me.UnitOfMeasure = Telerik.Reporting.Drawing.UnitType.Mm
        Me.Width = Telerik.Reporting.Drawing.Unit.Inch(10.409999847412109R)
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
#End Region

    Private detail As Telerik.Reporting.DetailSection
    Private textBox5 As Telerik.Reporting.TextBox
    Private reportHeaderSection1 As ReportHeaderSection
    Private textBox1 As Telerik.Reporting.TextBox
    Private textBox6 As Telerik.Reporting.TextBox
    Private shape5 As Shape
    Private panel5 As Telerik.Reporting.Panel
    Private yearDataSource As SqlDataSource
    Private panel2 As Telerik.Reporting.Panel
    Private panel3 As Telerik.Reporting.Panel
    Private SalesPersonQuarter As Crosstab
    Private spTotalQ As Telerik.Reporting.TextBox
    Private spTotal As Telerik.Reporting.TextBox
    Private spLabelTotalQ As Telerik.Reporting.TextBox
    Private spLabelTotal As Telerik.Reporting.TextBox
    Private spTextCorner As Telerik.Reporting.TextBox
    Private mainDataSource As SqlDataSource
    Private spLabelSalesPerson As Telerik.Reporting.TextBox
    Private textBox2 As Telerik.Reporting.TextBox
    Private textBox11 As Telerik.Reporting.TextBox
    Private textBox12 As Telerik.Reporting.TextBox
    Private textBox3 As Telerik.Reporting.TextBox
    Private SalesPersonQuarterBar As Telerik.Reporting.Graph
    Private cartesianCoordinateSystem1 As CartesianCoordinateSystem
    Private graphAxis3 As GraphAxis
    Private graphAxis4 As GraphAxis
    Private barSeries1 As BarSeries
    Private panel1 As Telerik.Reporting.Panel
    Private panel4 As Telerik.Reporting.Panel
    Private StoreQuarter As Crosstab
    Private sTotalQ As Telerik.Reporting.TextBox
    Private sTotal As Telerik.Reporting.TextBox
    Private sLabelTotalQ As Telerik.Reporting.TextBox
    Private sLabelTotal As Telerik.Reporting.TextBox
    Private sTextCorner As Telerik.Reporting.TextBox
    Private sLabelStore As Telerik.Reporting.TextBox
    Private textBox9 As Telerik.Reporting.TextBox
    Private textBox13 As Telerik.Reporting.TextBox
    Private textBox14 As Telerik.Reporting.TextBox
    Private textBox7 As Telerik.Reporting.TextBox
    Private StoreQuarterBar As Telerik.Reporting.Graph
    Private cartesianCoordinateSystem2 As CartesianCoordinateSystem
    Private graphAxis5 As GraphAxis
    Private graphAxis6 As GraphAxis
    Private barSeries3 As BarSeries
    Private panelSalesByProduct As Telerik.Reporting.Panel
    Private panel6 As Telerik.Reporting.Panel
    Private ProductQuarter As Crosstab
    Private pTotalQ As Telerik.Reporting.TextBox
    Private pTotal As Telerik.Reporting.TextBox
    Private pLabelTotalQ As Telerik.Reporting.TextBox
    Private pLabelTotal As Telerik.Reporting.TextBox
    Private pTextCorner As Telerik.Reporting.TextBox
    Private pProductName As Telerik.Reporting.TextBox
    Private textBox10 As Telerik.Reporting.TextBox
    Private textBox15 As Telerik.Reporting.TextBox
    Private textBox16 As Telerik.Reporting.TextBox
    Private textBox8 As Telerik.Reporting.TextBox
    Private ProductQuarterBar As Telerik.Reporting.Graph
    Private cartesianCoordinateSystem3 As CartesianCoordinateSystem
    Private graphAxis11 As GraphAxis
    Private graphAxis12 As GraphAxis
    Private barSeries6 As BarSeries
    Private WithEvents SalesPersonQuarterPie As Telerik.Reporting.Graph
    Private WithEvents polarCoordinateSystem1 As Telerik.Reporting.PolarCoordinateSystem
    Private WithEvents graphAxis1 As Telerik.Reporting.GraphAxis
    Private WithEvents graphAxis2 As Telerik.Reporting.GraphAxis
    Private WithEvents barSeries2 As Telerik.Reporting.BarSeries
    Private WithEvents Graph1 As Telerik.Reporting.Graph
    Private WithEvents polarCoordinateSystem2 As Telerik.Reporting.PolarCoordinateSystem
    Private WithEvents graphAxis7 As Telerik.Reporting.GraphAxis
    Private WithEvents graphAxis8 As Telerik.Reporting.GraphAxis
    Private WithEvents barSeries4 As Telerik.Reporting.BarSeries
    Private WithEvents ProductQuarterPie As Telerik.Reporting.Graph
    Private WithEvents polarCoordinateSystem3 As Telerik.Reporting.PolarCoordinateSystem
    Private WithEvents graphAxis9 As Telerik.Reporting.GraphAxis
    Private WithEvents graphAxis10 As Telerik.Reporting.GraphAxis
    Private WithEvents barSeries5 As Telerik.Reporting.BarSeries
End Class