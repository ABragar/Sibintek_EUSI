Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports Telerik.Reporting
Imports Telerik.Reporting.Drawing

Partial Class ProductSales
#Region "Component Designer generated code"
    ''' <summary>
    ''' Required method for telerik Reporting designer support - do not modify
    ''' the contents of this method with the code editor.
    ''' </summary>
    Private Sub InitializeComponent()
        Dim ToggleVisibilityAction1 As Telerik.Reporting.ToggleVisibilityAction = New Telerik.Reporting.ToggleVisibilityAction()
        Dim TableGroup1 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup2 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim ToggleVisibilityAction2 As Telerik.Reporting.ToggleVisibilityAction = New Telerik.Reporting.ToggleVisibilityAction()
        Dim TableGroup3 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup4 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup5 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim SortingAction1 As Telerik.Reporting.SortingAction = New Telerik.Reporting.SortingAction()
        Dim TableGroup6 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup7 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup8 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup9 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup10 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup11 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup12 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup13 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup14 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim NavigateToReportAction1 As Telerik.Reporting.NavigateToReportAction = New Telerik.Reporting.NavigateToReportAction()
        Dim TypeReportSource1 As Telerik.Reporting.TypeReportSource = New Telerik.Reporting.TypeReportSource()
        Dim FormattingRule1 As Telerik.Reporting.Drawing.FormattingRule = New Telerik.Reporting.Drawing.FormattingRule()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ProductSales))
        Dim FormattingRule2 As Telerik.Reporting.Drawing.FormattingRule = New Telerik.Reporting.Drawing.FormattingRule()
        Dim FormattingRule3 As Telerik.Reporting.Drawing.FormattingRule = New Telerik.Reporting.Drawing.FormattingRule()
        Dim GraphGroup1 As Telerik.Reporting.GraphGroup = New Telerik.Reporting.GraphGroup()
        Dim ColorPalette1 As Telerik.Reporting.Drawing.ColorPalette = New Telerik.Reporting.Drawing.ColorPalette()
        Dim GraphGroup3 As Telerik.Reporting.GraphGroup = New Telerik.Reporting.GraphGroup()
        Dim NumericalScale1 As Telerik.Reporting.NumericalScale = New Telerik.Reporting.NumericalScale()
        Dim NumericalScaleCrossAxisPosition1 As Telerik.Reporting.NumericalScaleCrossAxisPosition = New Telerik.Reporting.NumericalScaleCrossAxisPosition()
        Dim NumericalScale2 As Telerik.Reporting.NumericalScale = New Telerik.Reporting.NumericalScale()
        Dim NumericalScaleCrossAxisPosition2 As Telerik.Reporting.NumericalScaleCrossAxisPosition = New Telerik.Reporting.NumericalScaleCrossAxisPosition()
        Dim GraphGroup2 As Telerik.Reporting.GraphGroup = New Telerik.Reporting.GraphGroup()
        Dim GraphGroup4 As Telerik.Reporting.GraphGroup = New Telerik.Reporting.GraphGroup()
        Dim MonochromaticPalette1 As Telerik.Reporting.Drawing.MonochromaticPalette = New Telerik.Reporting.Drawing.MonochromaticPalette()
        Dim NumericalScale3 As Telerik.Reporting.NumericalScale = New Telerik.Reporting.NumericalScale()
        Dim NumericalScaleCrossAxisPosition3 As Telerik.Reporting.NumericalScaleCrossAxisPosition = New Telerik.Reporting.NumericalScaleCrossAxisPosition()
        Dim NumericalScale4 As Telerik.Reporting.NumericalScale = New Telerik.Reporting.NumericalScale()
        Dim NumericalScaleCrossAxisPosition4 As Telerik.Reporting.NumericalScaleCrossAxisPosition = New Telerik.Reporting.NumericalScaleCrossAxisPosition()
        Dim GraphGroup5 As Telerik.Reporting.GraphGroup = New Telerik.Reporting.GraphGroup()
        Dim GraphGroup6 As Telerik.Reporting.GraphGroup = New Telerik.Reporting.GraphGroup()
        Dim NumericalScale5 As Telerik.Reporting.NumericalScale = New Telerik.Reporting.NumericalScale()
        Dim NumericalScaleCrossAxisPosition5 As Telerik.Reporting.NumericalScaleCrossAxisPosition = New Telerik.Reporting.NumericalScaleCrossAxisPosition()
        Dim NumericalScale6 As Telerik.Reporting.NumericalScale = New Telerik.Reporting.NumericalScale()
        Dim NumericalScaleCrossAxisPosition6 As Telerik.Reporting.NumericalScaleCrossAxisPosition = New Telerik.Reporting.NumericalScaleCrossAxisPosition()
        Dim GraphGroup7 As Telerik.Reporting.GraphGroup = New Telerik.Reporting.GraphGroup()
        Dim GraphGroup8 As Telerik.Reporting.GraphGroup = New Telerik.Reporting.GraphGroup()
        Dim PictureWatermark1 As Telerik.Reporting.Drawing.PictureWatermark = New Telerik.Reporting.Drawing.PictureWatermark()
        Dim StyleRule1 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule2 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim DescendantSelector1 As Telerik.Reporting.Drawing.DescendantSelector = New Telerik.Reporting.Drawing.DescendantSelector()
        Dim StyleRule3 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule4 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule5 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule6 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Me.textBoxQtr = New Telerik.Reporting.TextBox()
        Me.textBoxTotalYearLabel = New Telerik.Reporting.TextBox()
        Me.textBox4 = New Telerik.Reporting.TextBox()
        Me.textBoxSubCategory = New Telerik.Reporting.TextBox()
        Me.textBox8 = New Telerik.Reporting.TextBox()
        Me.textBoxCategory = New Telerik.Reporting.TextBox()
        Me.textBoxYear = New Telerik.Reporting.TextBox()
        Me.textBoxGrandTotalYearLabel = New Telerik.Reporting.TextBox()
        Me.textBox2 = New Telerik.Reporting.TextBox()
        Me.textBox9 = New Telerik.Reporting.TextBox()
        Me.textBoxGrandTotalCategoryLabel = New Telerik.Reporting.TextBox()
        Me.detail = New Telerik.Reporting.DetailSection()
        Me.crosstab1 = New Telerik.Reporting.Crosstab()
        Me.textBoxFact = New Telerik.Reporting.TextBox()
        Me.textBoxTotalCategory = New Telerik.Reporting.TextBox()
        Me.textBoxTotalCategoryYear = New Telerik.Reporting.TextBox()
        Me.textBoxTotalYear = New Telerik.Reporting.TextBox()
        Me.textBoxGrandTotalYearCategory = New Telerik.Reporting.TextBox()
        Me.textBoxGrandTotalSubCategoryYear = New Telerik.Reporting.TextBox()
        Me.textBoxGrandTotal = New Telerik.Reporting.TextBox()
        Me.GrandTotalCategoryYear = New Telerik.Reporting.TextBox()
        Me.textBoxGrandTotalCategoryQtr = New Telerik.Reporting.TextBox()
        Me.graph1 = New Telerik.Reporting.Graph()
        Me.cartesianCoordinateSystem2 = New Telerik.Reporting.CartesianCoordinateSystem()
        Me.graphAxis4 = New Telerik.Reporting.GraphAxis()
        Me.graphAxis3 = New Telerik.Reporting.GraphAxis()
        Me.lineSeries1 = New Telerik.Reporting.LineSeries()
        Me.graph4 = New Telerik.Reporting.Graph()
        Me.cartesianCoordinateSystem3 = New Telerik.Reporting.CartesianCoordinateSystem()
        Me.graphAxis5 = New Telerik.Reporting.GraphAxis()
        Me.graphAxis6 = New Telerik.Reporting.GraphAxis()
        Me.barSeries2 = New Telerik.Reporting.BarSeries()
        Me.graph5 = New Telerik.Reporting.Graph()
        Me.cartesianCoordinateSystem1 = New Telerik.Reporting.CartesianCoordinateSystem()
        Me.graphAxis1 = New Telerik.Reporting.GraphAxis()
        Me.graphAxis2 = New Telerik.Reporting.GraphAxis()
        Me.barSeries1 = New Telerik.Reporting.BarSeries()
        Me.textBox3 = New Telerik.Reporting.TextBox()
        Me.textBox5 = New Telerik.Reporting.TextBox()
        Me.textBox6 = New Telerik.Reporting.TextBox()
        Me.sqlDataSource1 = New Telerik.Reporting.SqlDataSource()
        Me.textBox1 = New Telerik.Reporting.TextBox()
        Me.reportHeaderSection1 = New Telerik.Reporting.ReportHeaderSection()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'textBoxQtr
        '
        Me.textBoxQtr.Name = "textBoxQtr"
        Me.textBoxQtr.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.95669287443161011R), Telerik.Reporting.Drawing.Unit.Inch(0.24000000953674316R))
        Me.textBoxQtr.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None
        Me.textBoxQtr.StyleName = "RedTextBox"
        Me.textBoxQtr.Value = "Q{Fields.Qtr}"
        '
        'textBoxTotalYearLabel
        '
        Me.textBoxTotalYearLabel.Name = "textBoxTotalYearLabel"
        Me.textBoxTotalYearLabel.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.95275598764419556R), Telerik.Reporting.Drawing.Unit.Inch(0.24000000953674316R))
        Me.textBoxTotalYearLabel.StyleName = "RedTextBox"
        Me.textBoxTotalYearLabel.Value = "Total"
        '
        'textBox4
        '
        Me.textBox4.Name = "textBox4"
        Me.textBox4.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.8202083110809326R), Telerik.Reporting.Drawing.Unit.Inch(0.23999999463558197R))
        Me.textBox4.StyleName = "RedTextBox"
        Me.textBox4.Value = "Monthly Trend"
        '
        'textBoxSubCategory
        '
        Me.textBoxSubCategory.Name = "textBoxSubCategory"
        Me.textBoxSubCategory.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.8799998760223389R), Telerik.Reporting.Drawing.Unit.Inch(0.35039371252059937R))
        Me.textBoxSubCategory.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBoxSubCategory.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left
        Me.textBoxSubCategory.Style.Visible = True
        Me.textBoxSubCategory.StyleName = "ItemDetails"
        Me.textBoxSubCategory.Value = "=Fields.ProductSubCategory"
        '
        'textBox8
        '
        Me.textBox8.Name = "textBox8"
        Me.textBox8.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.8799998760223389R), Telerik.Reporting.Drawing.Unit.Inch(0.35039371252059937R))
        Me.textBox8.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left
        Me.textBox8.Value = "Total"
        '
        'textBoxCategory
        '
        ToggleVisibilityAction1.DisplayExpandedMark = False
        TableGroup1.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.ProductSubCategory"))
        TableGroup1.Name = "ProductSubCategory"
        TableGroup1.ReportItem = Me.textBoxSubCategory
        TableGroup1.Sortings.Add(New Telerik.Reporting.Sorting("=Fields.ProductSubCategory", Telerik.Reporting.SortDirection.Asc))
        TableGroup1.Visible = False
        TableGroup2.Name = "ProductSubCategoryTotal"
        TableGroup2.ReportItem = Me.textBox8
        TableGroup2.Visible = False
        ToggleVisibilityAction1.Targets.AddRange(New Telerik.Reporting.IActionTarget() {TableGroup1, TableGroup2})
        Me.textBoxCategory.Action = ToggleVisibilityAction1
        Me.textBoxCategory.KeepTogether = False
        Me.textBoxCategory.Name = "textBoxCategory"
        Me.textBoxCategory.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.5799999237060547R), Telerik.Reporting.Drawing.Unit.Inch(0.70078748464584351R))
        Me.textBoxCategory.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left
        Me.textBoxCategory.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.textBoxCategory.Value = "=Fields.ProductCategory"
        '
        'textBoxYear
        '
        ToggleVisibilityAction2.DisplayExpandedMark = False
        TableGroup3.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.Qtr"))
        TableGroup3.Name = "Quarter"
        TableGroup3.ReportItem = Me.textBoxQtr
        TableGroup3.Sortings.Add(New Telerik.Reporting.Sorting("=Fields.Qtr", Telerik.Reporting.SortDirection.Asc))
        TableGroup3.Visible = False
        TableGroup4.Name = "YearTotalGroup"
        TableGroup4.ReportItem = Me.textBoxTotalYearLabel
        TableGroup4.Visible = False
        TableGroup5.Name = "GroupMonthlyTrend"
        TableGroup5.ReportItem = Me.textBox4
        TableGroup5.Visible = False
        ToggleVisibilityAction2.Targets.AddRange(New Telerik.Reporting.IActionTarget() {TableGroup3, TableGroup4, TableGroup5})
        Me.textBoxYear.Action = ToggleVisibilityAction2
        Me.textBoxYear.KeepTogether = False
        Me.textBoxYear.Name = "textBoxYear"
        Me.textBoxYear.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.0197675228118896R), Telerik.Reporting.Drawing.Unit.Inch(0.27000001072883606R))
        Me.textBoxYear.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBoxYear.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None
        Me.textBoxYear.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left
        Me.textBoxYear.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.textBoxYear.Value = "=Fields.OrderDate.Year"
        '
        'textBoxGrandTotalYearLabel
        '
        SortingAction1.SortingExpression = "=Sum(Fields.LineTotal)"
        TableGroup6.ChildGroups.Add(TableGroup1)
        TableGroup6.ChildGroups.Add(TableGroup2)
        TableGroup6.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.ProductCategory"))
        TableGroup6.Name = "ProductCategory"
        TableGroup6.ReportItem = Me.textBoxCategory
        TableGroup6.Sortings.Add(New Telerik.Reporting.Sorting("=Fields.ProductCategory", Telerik.Reporting.SortDirection.Asc))
        SortingAction1.Targets.AddRange(New Telerik.Reporting.IActionTarget() {TableGroup6, TableGroup1})
        Me.textBoxGrandTotalYearLabel.Action = SortingAction1
        Me.textBoxGrandTotalYearLabel.Name = "textBoxGrandTotalYearLabel"
        Me.textBoxGrandTotalYearLabel.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2716536521911621R), Telerik.Reporting.Drawing.Unit.Inch(0.51000005006790161R))
        Me.textBoxGrandTotalYearLabel.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None
        Me.textBoxGrandTotalYearLabel.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBoxGrandTotalYearLabel.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None
        Me.textBoxGrandTotalYearLabel.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11.0R)
        Me.textBoxGrandTotalYearLabel.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.textBoxGrandTotalYearLabel.Value = "GRAND TOTAL"
        '
        'textBox2
        '
        Me.textBox2.CanGrow = False
        Me.textBox2.Name = "textBox2"
        Me.textBox2.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(4.2914209365844727R), Telerik.Reporting.Drawing.Unit.Inch(0.10000000894069672R))
        Me.textBox2.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None
        Me.textBox2.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None
        Me.textBox2.StyleName = ""
        '
        'textBox9
        '
        Me.textBox9.Name = "textBox9"
        Me.textBox9.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(4.2914209365844727R), Telerik.Reporting.Drawing.Unit.Inch(0.039999991655349731R))
        Me.textBox9.Style.BorderColor.Bottom = System.Drawing.Color.Black
        Me.textBox9.Style.BorderColor.Top = System.Drawing.Color.Black
        Me.textBox9.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBox9.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBox9.Style.BorderWidth.Top = Telerik.Reporting.Drawing.Unit.Point(3.0R)
        Me.textBox9.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.textBox9.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom
        Me.textBox9.StyleName = "RedTextBox"
        Me.textBox9.Value = ""
        '
        'textBoxGrandTotalCategoryLabel
        '
        Me.textBoxGrandTotalCategoryLabel.Name = "textBoxGrandTotalCategoryLabel"
        Me.textBoxGrandTotalCategoryLabel.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.4599997997283936R), Telerik.Reporting.Drawing.Unit.Inch(0.35039371252059937R))
        Me.textBoxGrandTotalCategoryLabel.Style.BorderColor.Bottom = System.Drawing.Color.Black
        Me.textBoxGrandTotalCategoryLabel.Style.BorderColor.Top = System.Drawing.Color.Black
        Me.textBoxGrandTotalCategoryLabel.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBoxGrandTotalCategoryLabel.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBoxGrandTotalCategoryLabel.Style.BorderWidth.Bottom = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        Me.textBoxGrandTotalCategoryLabel.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11.0R)
        Me.textBoxGrandTotalCategoryLabel.Style.LineStyle = Telerik.Reporting.Drawing.LineStyle.Solid
        Me.textBoxGrandTotalCategoryLabel.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left
        Me.textBoxGrandTotalCategoryLabel.Value = "GRAND TOTAL"
        '
        'detail
        '
        Me.detail.Height = Telerik.Reporting.Drawing.Unit.Inch(1.8000001907348633R)
        Me.detail.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.crosstab1})
        Me.detail.Name = "detail"
        '
        'crosstab1
        '
        Me.crosstab1.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(2.4300007820129395R)))
        Me.crosstab1.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(2.8202083110809326R)))
        Me.crosstab1.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(2.4200003147125244R)))
        Me.crosstab1.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(3.2300000190734863R)))
        Me.crosstab1.Body.Rows.Add(New Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.88999998569488525R)))
        Me.crosstab1.Body.Rows.Add(New Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.88999998569488525R)))
        Me.crosstab1.Body.Rows.Add(New Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.88999998569488525R)))
        Me.crosstab1.Body.SetCellContent(0, 0, Me.textBoxFact)
        Me.crosstab1.Body.SetCellContent(1, 0, Me.textBoxTotalCategory)
        Me.crosstab1.Body.SetCellContent(1, 2, Me.textBoxTotalCategoryYear)
        Me.crosstab1.Body.SetCellContent(0, 2, Me.textBoxTotalYear)
        Me.crosstab1.Body.SetCellContent(1, 3, Me.textBoxGrandTotalYearCategory)
        Me.crosstab1.Body.SetCellContent(0, 3, Me.textBoxGrandTotalSubCategoryYear)
        Me.crosstab1.Body.SetCellContent(2, 3, Me.textBoxGrandTotal)
        Me.crosstab1.Body.SetCellContent(2, 2, Me.GrandTotalCategoryYear)
        Me.crosstab1.Body.SetCellContent(2, 0, Me.textBoxGrandTotalCategoryQtr)
        Me.crosstab1.Body.SetCellContent(0, 1, Me.graph1)
        Me.crosstab1.Body.SetCellContent(2, 1, Me.graph4)
        Me.crosstab1.Body.SetCellContent(1, 1, Me.graph5)
        TableGroup10.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.Qtr"))
        TableGroup10.Name = "Quarter"
        TableGroup10.ReportItem = Me.textBoxQtr
        TableGroup10.Sortings.Add(New Telerik.Reporting.Sorting("=Fields.Qtr", Telerik.Reporting.SortDirection.Asc))
        TableGroup10.Visible = False
        TableGroup11.Name = "GroupMonthlyTrend"
        TableGroup11.ReportItem = Me.textBox4
        TableGroup11.Visible = False
        TableGroup12.Name = "YearTotalGroup"
        TableGroup12.ReportItem = Me.textBoxTotalYearLabel
        TableGroup12.Visible = False
        TableGroup9.ChildGroups.Add(TableGroup10)
        TableGroup9.ChildGroups.Add(TableGroup11)
        TableGroup9.ChildGroups.Add(TableGroup12)
        TableGroup9.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.OrderDate.Year"))
        TableGroup9.Name = "Year"
        TableGroup9.ReportItem = Me.textBoxYear
        TableGroup9.Sortings.Add(New Telerik.Reporting.Sorting("=Fields.OrderDate.Year", Telerik.Reporting.SortDirection.Asc))
        TableGroup13.ReportItem = Me.textBoxGrandTotalYearLabel
        TableGroup8.ChildGroups.Add(TableGroup9)
        TableGroup8.ChildGroups.Add(TableGroup13)
        TableGroup8.Name = "Group1"
        TableGroup8.ReportItem = Me.textBox2
        TableGroup7.ChildGroups.Add(TableGroup8)
        TableGroup7.Name = "Group4"
        TableGroup7.ReportItem = Me.textBox9
        Me.crosstab1.ColumnGroups.Add(TableGroup7)
        Me.crosstab1.ColumnHeadersPrintOnEveryPage = True
        Me.crosstab1.Corner.SetCellContent(2, 0, Me.textBox3, 2, 2)
        Me.crosstab1.Corner.SetCellContent(0, 0, Me.textBox5, 1, 2)
        Me.crosstab1.Corner.SetCellContent(1, 0, Me.textBox6, 1, 2)
        Me.crosstab1.DataSource = Me.sqlDataSource1
        Me.crosstab1.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.textBox5, Me.textBox6, Me.textBox3, Me.textBoxFact, Me.graph1, Me.textBoxTotalYear, Me.textBoxGrandTotalSubCategoryYear, Me.textBoxTotalCategory, Me.graph5, Me.textBoxTotalCategoryYear, Me.textBoxGrandTotalYearCategory, Me.textBoxGrandTotalCategoryQtr, Me.graph4, Me.GrandTotalCategoryYear, Me.textBoxGrandTotal, Me.textBox9, Me.textBox2, Me.textBoxYear, Me.textBoxQtr, Me.textBox4, Me.textBoxTotalYearLabel, Me.textBoxGrandTotalYearLabel, Me.textBoxSubCategory, Me.textBox8, Me.textBoxCategory, Me.textBoxGrandTotalCategoryLabel})
        Me.crosstab1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0R), Telerik.Reporting.Drawing.Unit.Inch(0R))
        Me.crosstab1.Name = "crosstab1"
        TableGroup14.ReportItem = Me.textBoxGrandTotalCategoryLabel
        Me.crosstab1.RowGroups.Add(TableGroup6)
        Me.crosstab1.RowGroups.Add(TableGroup14)
        Me.crosstab1.RowHeadersPrintOnEveryPage = True
        Me.crosstab1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(19.688608169555664R), Telerik.Reporting.Drawing.Unit.Cm(4.3210000991821289R))
        Me.crosstab1.Style.BorderColor.Default = System.Drawing.Color.Empty
        '
        'textBoxFact
        '
        TypeReportSource1.Parameters.Add(New Telerik.Reporting.Parameter("ProductCategory", "=Fields.ProductCategory"))
        TypeReportSource1.Parameters.Add(New Telerik.Reporting.Parameter("ProductSubcategory", "=Fields.ProductSubCategory"))
        TypeReportSource1.Parameters.Add(New Telerik.Reporting.Parameter("FromDate", "=Min(Fields.OrderDate)"))
        TypeReportSource1.Parameters.Add(New Telerik.Reporting.Parameter("ToDate", "=Max(Fields.OrderDate)"))
        TypeReportSource1.TypeName = "ProductLineSales, VB.ReportLibrary, Version" &
    "=1.0.0.0, Culture=neutral, PublicKeyToken=null"
        NavigateToReportAction1.ReportSource = TypeReportSource1
        Me.textBoxFact.Action = NavigateToReportAction1
        Me.textBoxFact.Bindings.Add(New Telerik.Reporting.Binding("Action", "=IIF(Fields.ProductCategory IS NULL, NULL, ReportItem.ItemDefinition.Action)"))
        Me.textBoxFact.Bindings.Add(New Telerik.Reporting.Binding("ToolTip.Text", "=IIF(Fields.ProductCategory IS NULL, NULL, ReportItem.ItemDefinition.ToolTip.Text" &
            ")"))
        Me.textBoxFact.Bindings.Add(New Telerik.Reporting.Binding("ToolTip.Title", "=IIF(Fields.ProductCategory IS NULL, NULL, ReportItem.ItemDefinition.ToolTip.Titl" &
            "e)"))
        FormattingRule1.Filters.Add(New Telerik.Reporting.Filter("Fields.ProductCategory IS NULL", Telerik.Reporting.FilterOperator.NotEqual, "=True"))
        FormattingRule1.StopIfTrue = True
        FormattingRule1.Style.Font.Underline = True
        Me.textBoxFact.ConditionalFormatting.AddRange(New Telerik.Reporting.Drawing.FormattingRule() {FormattingRule1})
        Me.textBoxFact.Format = "{0:N1}"
        Me.textBoxFact.Name = "textBoxFact"
        Me.textBoxFact.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.95669287443161011R), Telerik.Reporting.Drawing.Unit.Inch(0.35039371252059937R))
        Me.textBoxFact.StyleName = "ItemDetails"
        Me.textBoxFact.Value = "=Sum(Fields.LineTotal)/1000.0"
        Me.textBoxFact.ToolTip.Title = "Opens a detailed report with following parameters:"
        Me.textBoxFact.ToolTip.Text = "=Format('Category: {0}; " & vbCrLf & "Subcategory: {1}; " & vbCrLf & "Period: {2} - {3}', Fields.ProductCategory, Fields.ProductSubCategory, Min(Fields.OrderDate).ToString('MMM yyyy'), Max(Fields.OrderDate).ToString('MMM yyyy'))"
        '
        'textBoxTotalCategory
        '
        Me.textBoxTotalCategory.Format = "{0:N1}"
        Me.textBoxTotalCategory.Name = "textBoxTotalCategory"
        Me.textBoxTotalCategory.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.95669287443161011R), Telerik.Reporting.Drawing.Unit.Inch(0.35039371252059937R))
        Me.textBoxTotalCategory.Style.BackgroundColor = System.Drawing.Color.Ivory
        Me.textBoxTotalCategory.StyleName = ""
        Me.textBoxTotalCategory.Value = "=Sum(Fields.LineTotal)/1000.0"
        '
        'textBoxTotalCategoryYear
        '
        Me.textBoxTotalCategoryYear.Format = "{0:N1}"
        Me.textBoxTotalCategoryYear.Name = "textBoxTotalCategoryYear"
        Me.textBoxTotalCategoryYear.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.95275598764419556R), Telerik.Reporting.Drawing.Unit.Inch(0.35039371252059937R))
        Me.textBoxTotalCategoryYear.Style.BackgroundColor = System.Drawing.Color.Ivory
        Me.textBoxTotalCategoryYear.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBoxTotalCategoryYear.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.textBoxTotalCategoryYear.Value = "=Sum(Fields.LineTotal)/1000.0"
        '
        'textBoxTotalYear
        '
        Me.textBoxTotalYear.Format = "{0:N1}"
        Me.textBoxTotalYear.Name = "textBoxTotalYear"
        Me.textBoxTotalYear.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.95275598764419556R), Telerik.Reporting.Drawing.Unit.Inch(0.35039371252059937R))
        Me.textBoxTotalYear.Style.BackgroundColor = System.Drawing.Color.Ivory
        Me.textBoxTotalYear.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBoxTotalYear.StyleName = ""
        Me.textBoxTotalYear.ToolTip.Text = "=Format('{0:N1}', Sum(Fields.LineTotal)/1000.0)"
        Me.textBoxTotalYear.ToolTip.Title = "=Format('Total for year {0}', Fields.OrderDate.Year)"
        Me.textBoxTotalYear.Value = "=Sum(Fields.LineTotal)/1000.0"
        '
        'textBoxGrandTotalYearCategory
        '
        Me.textBoxGrandTotalYearCategory.Format = "{0:N1}"
        Me.textBoxGrandTotalYearCategory.Name = "textBoxGrandTotalYearCategory"
        Me.textBoxGrandTotalYearCategory.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2716536521911621R), Telerik.Reporting.Drawing.Unit.Inch(0.35039371252059937R))
        Me.textBoxGrandTotalYearCategory.StyleName = "ItemTotal"
        Me.textBoxGrandTotalYearCategory.Value = "=Sum(Fields.LineTotal)/1000.0"
        '
        'textBoxGrandTotalSubCategoryYear
        '
        Me.textBoxGrandTotalSubCategoryYear.Format = "{0:N1}"
        Me.textBoxGrandTotalSubCategoryYear.Name = "textBoxGrandTotalSubCategoryYear"
        Me.textBoxGrandTotalSubCategoryYear.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2716536521911621R), Telerik.Reporting.Drawing.Unit.Inch(0.35039371252059937R))
        Me.textBoxGrandTotalSubCategoryYear.StyleName = "ItemTotal"
        Me.textBoxGrandTotalSubCategoryYear.Value = "=Sum(Fields.LineTotal)/1000.0"
        '
        'textBoxGrandTotal
        '
        Me.textBoxGrandTotal.Format = "{0:N1}"
        Me.textBoxGrandTotal.Name = "textBoxGrandTotal"
        Me.textBoxGrandTotal.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2716536521911621R), Telerik.Reporting.Drawing.Unit.Inch(0.35039371252059937R))
        Me.textBoxGrandTotal.Style.BorderColor.Default = System.Drawing.Color.Black
        Me.textBoxGrandTotal.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBoxGrandTotal.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        Me.textBoxGrandTotal.Style.BorderWidth.Top = Telerik.Reporting.Drawing.Unit.Point(1.0R)
        Me.textBoxGrandTotal.Style.LineStyle = Telerik.Reporting.Drawing.LineStyle.Solid
        Me.textBoxGrandTotal.StyleName = "ItemTotal"
        Me.textBoxGrandTotal.Value = "=Sum(Fields.LineTotal)/1000.0"
        '
        'GrandTotalCategoryYear
        '
        Me.GrandTotalCategoryYear.Format = "{0:N1}"
        Me.GrandTotalCategoryYear.Name = "GrandTotalCategoryYear"
        Me.GrandTotalCategoryYear.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.95275598764419556R), Telerik.Reporting.Drawing.Unit.Inch(0.35039371252059937R))
        Me.GrandTotalCategoryYear.Style.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(219, Byte), Integer))
        Me.GrandTotalCategoryYear.Style.BorderColor.Bottom = System.Drawing.Color.Black
        Me.GrandTotalCategoryYear.Style.BorderColor.Top = System.Drawing.Color.Black
        Me.GrandTotalCategoryYear.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.GrandTotalCategoryYear.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid
        Me.GrandTotalCategoryYear.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        Me.GrandTotalCategoryYear.Style.BorderWidth.Right = Telerik.Reporting.Drawing.Unit.Point(1.0R)
        Me.GrandTotalCategoryYear.Style.BorderWidth.Top = Telerik.Reporting.Drawing.Unit.Point(1.0R)
        Me.GrandTotalCategoryYear.Style.LineStyle = Telerik.Reporting.Drawing.LineStyle.Solid
        Me.GrandTotalCategoryYear.Value = "=Sum(Fields.LineTotal)/1000.0"
        '
        'textBoxGrandTotalCategoryQtr
        '
        FormattingRule2.Filters.Add(New Telerik.Reporting.Filter("=Sum(Fields.LineTotal)", Telerik.Reporting.FilterOperator.LessThan, "=7000000"))
        FormattingRule2.StopIfTrue = True
        FormattingRule2.Style.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(193, Byte), Integer), CType(CType(193, Byte), Integer))
        FormattingRule3.Filters.Add(New Telerik.Reporting.Filter("=Sum(Fields.LineTotal)", Telerik.Reporting.FilterOperator.GreaterThan, "=13000000"))
        FormattingRule3.Style.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(193, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(193, Byte), Integer))
        Me.textBoxGrandTotalCategoryQtr.ConditionalFormatting.AddRange(New Telerik.Reporting.Drawing.FormattingRule() {FormattingRule2, FormattingRule3})
        Me.textBoxGrandTotalCategoryQtr.Format = "{0:N1}"
        Me.textBoxGrandTotalCategoryQtr.Name = "textBoxGrandTotalCategoryQtr"
        Me.textBoxGrandTotalCategoryQtr.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.95669287443161011R), Telerik.Reporting.Drawing.Unit.Inch(0.35039371252059937R))
        Me.textBoxGrandTotalCategoryQtr.Style.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(219, Byte), Integer))
        Me.textBoxGrandTotalCategoryQtr.Style.BorderColor.Default = System.Drawing.Color.Black
        Me.textBoxGrandTotalCategoryQtr.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBoxGrandTotalCategoryQtr.Style.BorderWidth.Bottom = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        Me.textBoxGrandTotalCategoryQtr.Style.LineStyle = Telerik.Reporting.Drawing.LineStyle.Solid
        Me.textBoxGrandTotalCategoryQtr.Value = "=Sum(Fields.LineTotal)/1000.0"
        '
        'graph1
        '
        Me.graph1.Bindings.Add(New Telerik.Reporting.Binding("DataSource", "= ReportItem.DataObject"))
        GraphGroup1.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.OrderDate.Month"))
        GraphGroup1.Name = "orderDate.MonthGroup2"
        GraphGroup1.Sortings.Add(New Telerik.Reporting.Sorting("=Fields.OrderDate.Month", Telerik.Reporting.SortDirection.Asc))
        Me.graph1.CategoryGroups.Add(GraphGroup1)
        ColorPalette1.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(77, Byte), Integer)))
        ColorPalette1.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(162, Byte), Integer)))
        ColorPalette1.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(150, Byte), Integer), CType(CType(70, Byte), Integer)))
        ColorPalette1.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(119, Byte), Integer), CType(CType(44, Byte), Integer), CType(CType(42, Byte), Integer)))
        ColorPalette1.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(59, Byte), Integer), CType(CType(98, Byte), Integer)))
        ColorPalette1.Colors.Add(System.Drawing.Color.FromArgb(CType(CType(182, Byte), Integer), CType(CType(87, Byte), Integer), CType(CType(8, Byte), Integer)))
        Me.graph1.ColorPalette = ColorPalette1
        Me.graph1.CoordinateSystems.Add(Me.cartesianCoordinateSystem2)
        Me.graph1.Legend.IsInsidePlotArea = True
        Me.graph1.Legend.Style.LineColor = System.Drawing.Color.LightGray
        Me.graph1.Legend.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0R)
        Me.graph1.Legend.Style.Visible = False
        Me.graph1.Name = "graph1"
        Me.graph1.PlotAreaStyle.LineColor = System.Drawing.Color.LightGray
        Me.graph1.PlotAreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0R)
        Me.graph1.Series.Add(Me.lineSeries1)
        GraphGroup3.ChildGroups.Add(GraphGroup2)
        GraphGroup3.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.ProductCategory"))
        GraphGroup3.Name = "productCategoryGroup2"
        GraphGroup3.Sortings.Add(New Telerik.Reporting.Sorting("=Fields.ProductCategory", Telerik.Reporting.SortDirection.Asc))
        Me.graph1.SeriesGroups.Add(GraphGroup3)
        Me.graph1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.8202083110809326R), Telerik.Reporting.Drawing.Unit.Cm(0.89000004529953R))
        Me.graph1.Style.BackgroundColor = System.Drawing.Color.Ivory
        Me.graph1.Style.BorderColor.Default = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.graph1.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.graph1.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graph1.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(6.0R)
        Me.graph1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(3.0R)
        Me.graph1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(3.0R)
        Me.graph1.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(6.0R)
        Me.graph1.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.graph1.StyleName = ""
        '
        'cartesianCoordinateSystem2
        '
        Me.cartesianCoordinateSystem2.Name = "cartesianCoordinateSystem2"
        Me.cartesianCoordinateSystem2.XAxis = Me.graphAxis4
        Me.cartesianCoordinateSystem2.YAxis = Me.graphAxis3
        '
        'graphAxis4
        '
        Me.graphAxis4.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis4.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis4.MajorGridLineStyle.Visible = False
        Me.graphAxis4.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis4.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis4.MinorGridLineStyle.Visible = False
        Me.graphAxis4.Name = "graphAxis4"
        NumericalScaleCrossAxisPosition1.Position = Telerik.Reporting.GraphScaleCrossAxisPosition.[Auto]
        NumericalScaleCrossAxisPosition1.Value = 0R
        NumericalScale1.CrossAxisPositions.Add(NumericalScaleCrossAxisPosition1)
        NumericalScale1.MajorStep = 1.0R
        NumericalScale1.Maximum = 12.0R
        NumericalScale1.Minimum = 0R
        NumericalScale1.SpacingSlotCount = 0.5R
        Me.graphAxis4.Scale = NumericalScale1
        Me.graphAxis4.Style.Visible = False
        '
        'graphAxis3
        '
        Me.graphAxis3.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis3.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis3.MajorGridLineStyle.Visible = False
        Me.graphAxis3.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis3.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis3.MinorGridLineStyle.Visible = False
        Me.graphAxis3.Name = "graphAxis3"
        NumericalScaleCrossAxisPosition2.Position = Telerik.Reporting.GraphScaleCrossAxisPosition.[Auto]
        NumericalScaleCrossAxisPosition2.Value = 0R
        NumericalScale2.CrossAxisPositions.Add(NumericalScaleCrossAxisPosition2)
        Me.graphAxis3.Scale = NumericalScale2
        Me.graphAxis3.Style.Visible = False
        '
        'lineSeries1
        '
        Me.lineSeries1.CategoryGroup = GraphGroup1
        Me.lineSeries1.CoordinateSystem = Me.cartesianCoordinateSystem2
        Me.lineSeries1.DataPointLabel = "=Sum(Fields.LineTotal)"
        Me.lineSeries1.DataPointLabelStyle.Visible = False
        Me.lineSeries1.DataPointStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0R)
        Me.lineSeries1.DataPointStyle.Visible = False
        Me.lineSeries1.LegendItem.Style.LineColor = System.Drawing.Color.LightGray
        Me.lineSeries1.LegendItem.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0R)
        Me.lineSeries1.LegendItem.Value = "=Fields.ProductCategory + '/' + Fields.ProductSubCategory"
        Me.lineSeries1.LineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.lineSeries1.MarkerMaxSize = Telerik.Reporting.Drawing.Unit.Pixel(50.0R)
        Me.lineSeries1.MarkerMinSize = Telerik.Reporting.Drawing.Unit.Pixel(5.0R)
        Me.lineSeries1.MarkerSize = Telerik.Reporting.Drawing.Unit.Pixel(5.0R)
        GraphGroup2.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.ProductSubCategory"))
        GraphGroup2.Name = "productSubCategoryGroup1"
        GraphGroup2.Sortings.Add(New Telerik.Reporting.Sorting("=Fields.ProductSubCategory", Telerik.Reporting.SortDirection.Asc))
        Me.lineSeries1.SeriesGroup = GraphGroup2
        Me.lineSeries1.Size = Nothing
        Me.lineSeries1.X = "= Fields.OrderDate.Month"
        Me.lineSeries1.Y = "=Sum(Fields.LineTotal)"
        '
        'graph4
        '
        Me.graph4.Bindings.Add(New Telerik.Reporting.Binding("DataSource", "= ReportItem.DataObject"))
        GraphGroup4.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.OrderDate.Month"))
        GraphGroup4.Name = "orderDate.MonthGroup2"
        GraphGroup4.Sortings.Add(New Telerik.Reporting.Sorting("=Fields.OrderDate.Month", Telerik.Reporting.SortDirection.Asc))
        Me.graph4.CategoryGroups.Add(GraphGroup4)
        MonochromaticPalette1.BaseColor = System.Drawing.Color.FromArgb(CType(CType(113, Byte), Integer), CType(CType(137, Byte), Integer), CType(CType(63, Byte), Integer))
        Me.graph4.ColorPalette = MonochromaticPalette1
        Me.graph4.CoordinateSystems.Add(Me.cartesianCoordinateSystem3)
        Me.graph4.Legend.IsInsidePlotArea = True
        Me.graph4.Legend.Style.LineColor = System.Drawing.Color.LightGray
        Me.graph4.Legend.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0R)
        Me.graph4.Legend.Style.Visible = False
        Me.graph4.Name = "graph4"
        Me.graph4.PlotAreaStyle.LineColor = System.Drawing.Color.LightGray
        Me.graph4.PlotAreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0R)
        Me.graph4.Series.Add(Me.barSeries2)
        Me.graph4.SeriesGroups.Add(GraphGroup5)
        Me.graph4.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.8202083110809326R), Telerik.Reporting.Drawing.Unit.Cm(0.89000004529953R))
        Me.graph4.Style.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(219, Byte), Integer))
        Me.graph4.Style.BorderColor.Bottom = System.Drawing.Color.Black
        Me.graph4.Style.BorderColor.Default = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.graph4.Style.BorderColor.Top = System.Drawing.Color.Black
        Me.graph4.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.graph4.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.graph4.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid
        Me.graph4.Style.BorderWidth.Bottom = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        Me.graph4.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graph4.Style.BorderWidth.Top = Telerik.Reporting.Drawing.Unit.Point(1.0R)
        Me.graph4.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(6.0R)
        Me.graph4.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(3.0R)
        Me.graph4.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(3.0R)
        Me.graph4.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(6.0R)
        Me.graph4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.graph4.StyleName = ""
        '
        'cartesianCoordinateSystem3
        '
        Me.cartesianCoordinateSystem3.Name = "cartesianCoordinateSystem2"
        Me.cartesianCoordinateSystem3.XAxis = Me.graphAxis5
        Me.cartesianCoordinateSystem3.YAxis = Me.graphAxis6
        '
        'graphAxis5
        '
        Me.graphAxis5.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis5.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis5.MajorGridLineStyle.Visible = False
        Me.graphAxis5.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis5.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis5.MinorGridLineStyle.Visible = False
        Me.graphAxis5.Name = "graphAxis4"
        NumericalScaleCrossAxisPosition3.Position = Telerik.Reporting.GraphScaleCrossAxisPosition.[Auto]
        NumericalScaleCrossAxisPosition3.Value = 0R
        NumericalScale3.CrossAxisPositions.Add(NumericalScaleCrossAxisPosition3)
        NumericalScale3.MajorStep = 1.0R
        NumericalScale3.Maximum = 13.0R
        NumericalScale3.Minimum = 0R
        NumericalScale3.SpacingSlotCount = 0.5R
        Me.graphAxis5.Scale = NumericalScale3
        Me.graphAxis5.Style.Visible = False
        '
        'graphAxis6
        '
        Me.graphAxis6.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis6.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis6.MajorGridLineStyle.Visible = False
        Me.graphAxis6.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis6.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis6.MinorGridLineStyle.Visible = False
        Me.graphAxis6.Name = "graphAxis3"
        NumericalScaleCrossAxisPosition4.Position = Telerik.Reporting.GraphScaleCrossAxisPosition.[Auto]
        NumericalScaleCrossAxisPosition4.Value = 0R
        NumericalScale4.CrossAxisPositions.Add(NumericalScaleCrossAxisPosition4)
        Me.graphAxis6.Scale = NumericalScale4
        Me.graphAxis6.Style.Visible = False
        '
        'barSeries2
        '
        Me.barSeries2.CategoryGroup = GraphGroup4
        Me.barSeries2.CoordinateSystem = Me.cartesianCoordinateSystem3
        Me.barSeries2.DataPointLabel = "=Sum(Fields.LineTotal)"
        Me.barSeries2.DataPointLabelStyle.Visible = False
        Me.barSeries2.DataPointStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0R)
        Me.barSeries2.DataPointStyle.Visible = True
        Me.barSeries2.LegendItem.Style.LineColor = System.Drawing.Color.LightGray
        Me.barSeries2.LegendItem.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0R)
        Me.barSeries2.LegendItem.Value = "=Fields.ProductCategory + '/' + Fields.ProductSubCategory"
        GraphGroup5.Name = "graphGroup1"
        Me.barSeries2.SeriesGroup = GraphGroup5
        Me.barSeries2.X = "= Fields.OrderDate.Month"
        Me.barSeries2.Y = "=Sum(Fields.LineTotal)"
        '
        'graph5
        '
        Me.graph5.Bindings.Add(New Telerik.Reporting.Binding("DataSource", "= ReportItem.DataObject"))
        GraphGroup6.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.OrderDate.Month"))
        GraphGroup6.Name = "orderDate.MonthGroup2"
        GraphGroup6.Sortings.Add(New Telerik.Reporting.Sorting("=Fields.OrderDate.Month", Telerik.Reporting.SortDirection.Asc))
        Me.graph5.CategoryGroups.Add(GraphGroup6)
        Me.graph5.CoordinateSystems.Add(Me.cartesianCoordinateSystem1)
        Me.graph5.Legend.IsInsidePlotArea = True
        Me.graph5.Legend.Style.LineColor = System.Drawing.Color.LightGray
        Me.graph5.Legend.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0R)
        Me.graph5.Legend.Style.Visible = False
        Me.graph5.Name = "graph5"
        Me.graph5.PlotAreaStyle.LineColor = System.Drawing.Color.LightGray
        Me.graph5.PlotAreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0R)
        Me.graph5.Series.Add(Me.barSeries1)
        Me.graph5.SeriesGroups.Add(GraphGroup7)
        Me.graph5.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.8202085494995117R), Telerik.Reporting.Drawing.Unit.Cm(0.89000004529953R))
        Me.graph5.Style.BackgroundColor = System.Drawing.Color.Ivory
        Me.graph5.Style.BorderColor.Default = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.graph5.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.graph5.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graph5.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(6.0R)
        Me.graph5.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(3.0R)
        Me.graph5.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(3.0R)
        Me.graph5.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(6.0R)
        Me.graph5.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.graph5.StyleName = ""
        '
        'cartesianCoordinateSystem1
        '
        Me.cartesianCoordinateSystem1.Name = "cartesianCoordinateSystem2"
        Me.cartesianCoordinateSystem1.XAxis = Me.graphAxis1
        Me.cartesianCoordinateSystem1.YAxis = Me.graphAxis2
        '
        'graphAxis1
        '
        Me.graphAxis1.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis1.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis1.MajorGridLineStyle.Visible = False
        Me.graphAxis1.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray
        Me.graphAxis1.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1.0R)
        Me.graphAxis1.MinorGridLineStyle.Visible = False
        Me.graphAxis1.Name = "graphAxis4"
        NumericalScaleCrossAxisPosition5.Position = Telerik.Reporting.GraphScaleCrossAxisPosition.[Auto]
        NumericalScaleCrossAxisPosition5.Value = 0R
        NumericalScale5.CrossAxisPositions.Add(NumericalScaleCrossAxisPosition5)
        NumericalScale5.MajorStep = 1.0R
        NumericalScale5.Maximum = 13.0R
        NumericalScale5.Minimum = 0R
        NumericalScale5.SpacingSlotCount = 0.5R
        Me.graphAxis1.Scale = NumericalScale5
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
        Me.graphAxis2.Name = "graphAxis3"
        NumericalScaleCrossAxisPosition6.Position = Telerik.Reporting.GraphScaleCrossAxisPosition.[Auto]
        NumericalScaleCrossAxisPosition6.Value = 0R
        NumericalScale6.CrossAxisPositions.Add(NumericalScaleCrossAxisPosition6)
        Me.graphAxis2.Scale = NumericalScale6
        Me.graphAxis2.Style.Visible = False
        '
        'barSeries1
        '
        Me.barSeries1.CategoryGroup = GraphGroup6
        Me.barSeries1.CoordinateSystem = Me.cartesianCoordinateSystem1
        Me.barSeries1.DataPointLabel = "=Sum(Fields.LineTotal)"
        Me.barSeries1.DataPointLabelStyle.Visible = False
        Me.barSeries1.DataPointStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0R)
        Me.barSeries1.DataPointStyle.Visible = True
        Me.barSeries1.LegendItem.Style.LineColor = System.Drawing.Color.LightGray
        Me.barSeries1.LegendItem.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0R)
        Me.barSeries1.LegendItem.Value = "=Fields.ProductCategory + '/' + Fields.ProductSubCategory"
        GraphGroup8.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.ProductSubCategory"))
        GraphGroup8.Name = "productSubCategoryGroup1"
        GraphGroup8.Sortings.Add(New Telerik.Reporting.Sorting("=Fields.ProductSubCategory", Telerik.Reporting.SortDirection.Asc))
        GraphGroup7.ChildGroups.Add(GraphGroup8)
        GraphGroup7.Groupings.Add(New Telerik.Reporting.Grouping("=Fields.ProductCategory"))
        GraphGroup7.Name = "productCategoryGroup3"
        GraphGroup7.Sortings.Add(New Telerik.Reporting.Sorting("=Fields.ProductCategory", Telerik.Reporting.SortDirection.Asc))
        Me.barSeries1.SeriesGroup = GraphGroup7
        Me.barSeries1.X = "= Fields.OrderDate.Month"
        Me.barSeries1.Y = "=Sum(Fields.LineTotal)"
        '
        'textBox3
        '
        Me.textBox3.Name = "textBox3"
        Me.textBox3.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.4600000381469727R), Telerik.Reporting.Drawing.Unit.Inch(0.50999999046325684R))
        Me.textBox3.Style.BorderColor.Bottom = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.textBox3.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBox3.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBox3.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None
        Me.textBox3.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left
        Me.textBox3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Top
        Me.textBox3.StyleName = "RedTextBox"
        Me.textBox3.Value = "(USD IN THOUSANDS)"
        '
        'textBox5
        '
        Me.textBox5.Name = "textBox5"
        Me.textBox5.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.4600000381469727R), Telerik.Reporting.Drawing.Unit.Inch(0.039999999105930328R))
        Me.textBox5.Style.BorderColor.Bottom = System.Drawing.Color.Black
        Me.textBox5.Style.BorderColor.Top = System.Drawing.Color.Black
        Me.textBox5.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBox5.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBox5.Style.BorderWidth.Top = Telerik.Reporting.Drawing.Unit.Point(3.0R)
        Me.textBox5.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom
        Me.textBox5.StyleName = ""
        '
        'textBox6
        '
        Me.textBox6.CanGrow = False
        Me.textBox6.Name = "textBox6"
        Me.textBox6.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.4600000381469727R), Telerik.Reporting.Drawing.Unit.Inch(0.10000000149011612R))
        Me.textBox6.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None
        Me.textBox6.StyleName = ""
        '
        'sqlDataSource1
        '
        Me.sqlDataSource1.CalculatedFields.AddRange(New Telerik.Reporting.CalculatedField() {New Telerik.Reporting.CalculatedField("Qtr", GetType(Integer), "= Quarter(Fields.OrderDate)")})
        Me.sqlDataSource1.ConnectionString = "My.MySettings.TelerikConnectionString"
        Me.sqlDataSource1.Name = "sqlDataSource1"
        Me.sqlDataSource1.SelectCommand = resources.GetString("sqlDataSource1.SelectCommand")
        '
        'textBox1
        '
        Me.textBox1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.000039259593904716894R), Telerik.Reporting.Drawing.Unit.Inch(0.000039418537198798731R))
        Me.textBox1.Name = "textBox1"
        Me.textBox1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(6.6439194679260254R), Telerik.Reporting.Drawing.Unit.Inch(0.49996057152748108R))
        Me.textBox1.StyleName = "Header"
        Me.textBox1.Value = "Sales by Product Line per Period"
        '
        'reportHeaderSection1
        '
        Me.reportHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.5R)
        Me.reportHeaderSection1.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.textBox1})
        Me.reportHeaderSection1.Name = "reportHeaderSection1"
        '
        'ProductSales
        '
        Me.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.detail, Me.reportHeaderSection1})
        Me.Name = "ProductSales"
        Me.PageSettings.Landscape = True
        Me.PageSettings.Margins = New Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224R), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224R), Telerik.Reporting.Drawing.Unit.Inch(0.30000001192092896R), Telerik.Reporting.Drawing.Unit.Inch(0.40000000596046448R))
        Me.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4
        PictureWatermark1.Image = CType(resources.GetObject("PictureWatermark1.Image"), Object)
        PictureWatermark1.Opacity = 0.17R
        PictureWatermark1.Position = Telerik.Reporting.Drawing.WatermarkPosition.Front
        PictureWatermark1.Sizing = Telerik.Reporting.Drawing.WatermarkSizeMode.ScaleProportional
        Me.PageSettings.Watermarks.Add(PictureWatermark1)
        StyleRule1.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.StyleSelector("Header")})
        StyleRule1.Style.Font.Bold = True
        StyleRule1.Style.Font.Name = "Segoe UI Light"
        StyleRule1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(25.0R)
        StyleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0R)
        DescendantSelector1.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.Crosstab)), New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.ReportItem))})
        StyleRule2.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {DescendantSelector1})
        StyleRule2.Style.BorderColor.Default = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(230, Byte), Integer))
        StyleRule2.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None
        StyleRule2.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid
        StyleRule2.Style.Font.Name = "Segoe UI"
        StyleRule2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10.0R)
        StyleRule2.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Cm(0R)
        StyleRule2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Cm(0R)
        StyleRule2.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Cm(0R)
        StyleRule2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        StyleRule2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom
        StyleRule3.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.StyleSelector("RedTextBox")})
        StyleRule3.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None
        StyleRule3.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None
        StyleRule3.Style.Color = System.Drawing.Color.FromArgb(CType(CType(195, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(11, Byte), Integer))
        StyleRule3.Style.Font.Name = "Segoe UI"
        StyleRule3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10.0R)
        StyleRule4.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.StyleSelector("ItemDetails")})
        StyleRule4.Style.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        StyleRule5.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.StyleSelector("ItemTotal")})
        StyleRule5.Style.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(219, Byte), Integer))
        StyleRule6.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.TextItemBase)), New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.HtmlTextBox))})
        StyleRule6.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        StyleRule6.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        Me.StyleSheet.AddRange(New Telerik.Reporting.Drawing.StyleRule() {StyleRule1, StyleRule2, StyleRule3, StyleRule4, StyleRule5, StyleRule6})
        Me.TocText = "Product Sales"
        Me.Width = Telerik.Reporting.Drawing.Unit.Inch(7.7514209747314453R)
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
#End Region

    Private detail As Telerik.Reporting.DetailSection
    Private sqlDataSource1 As SqlDataSource
    Private textBox1 As Telerik.Reporting.TextBox
    Private reportHeaderSection1 As ReportHeaderSection
    Private WithEvents crosstab1 As Crosstab
    Private WithEvents textBoxFact As Telerik.Reporting.TextBox
    Private WithEvents textBoxTotalCategory As Telerik.Reporting.TextBox
    Private WithEvents textBoxTotalCategoryYear As Telerik.Reporting.TextBox
    Private WithEvents textBoxTotalYear As Telerik.Reporting.TextBox
    Private WithEvents textBoxGrandTotalYearCategory As Telerik.Reporting.TextBox
    Private WithEvents textBoxGrandTotalSubCategoryYear As Telerik.Reporting.TextBox
    Private WithEvents textBoxGrandTotal As Telerik.Reporting.TextBox
    Private WithEvents GrandTotalCategoryYear As Telerik.Reporting.TextBox
    Private WithEvents textBoxGrandTotalCategoryQtr As Telerik.Reporting.TextBox
    Private WithEvents graph1 As Graph
    Private WithEvents cartesianCoordinateSystem2 As CartesianCoordinateSystem
    Private WithEvents graphAxis4 As GraphAxis
    Private WithEvents graphAxis3 As GraphAxis
    Private WithEvents lineSeries1 As LineSeries
    Private WithEvents graph4 As Graph
    Private WithEvents cartesianCoordinateSystem3 As CartesianCoordinateSystem
    Private WithEvents graphAxis5 As GraphAxis
    Private WithEvents graphAxis6 As GraphAxis
    Private WithEvents barSeries2 As BarSeries
    Private WithEvents graph5 As Graph
    Private WithEvents cartesianCoordinateSystem1 As CartesianCoordinateSystem
    Private WithEvents graphAxis1 As GraphAxis
    Private WithEvents graphAxis2 As GraphAxis
    Private WithEvents barSeries1 As BarSeries
    Private WithEvents textBoxQtr As Telerik.Reporting.TextBox
    Private WithEvents textBox4 As Telerik.Reporting.TextBox
    Private WithEvents textBoxTotalYearLabel As Telerik.Reporting.TextBox
    Private WithEvents textBoxYear As Telerik.Reporting.TextBox
    Private WithEvents textBoxGrandTotalYearLabel As Telerik.Reporting.TextBox
    Private WithEvents textBoxSubCategory As Telerik.Reporting.TextBox
    Private WithEvents textBox8 As Telerik.Reporting.TextBox
    Private WithEvents textBoxCategory As Telerik.Reporting.TextBox
    Private WithEvents textBox2 As Telerik.Reporting.TextBox
    Private WithEvents textBox9 As Telerik.Reporting.TextBox
    Private WithEvents textBox3 As Telerik.Reporting.TextBox
    Private WithEvents textBox5 As Telerik.Reporting.TextBox
    Private WithEvents textBox6 As Telerik.Reporting.TextBox
    Private WithEvents textBoxGrandTotalCategoryLabel As Telerik.Reporting.TextBox
End Class

