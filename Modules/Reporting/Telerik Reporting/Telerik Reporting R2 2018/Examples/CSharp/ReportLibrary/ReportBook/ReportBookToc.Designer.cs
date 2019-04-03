namespace Telerik.Reporting.Examples.CSharp
{
    using Telerik.Reporting;

    partial class ReportBookToc
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.Reporting.TocLevel tocLevel1 = new Telerik.Reporting.TocLevel();
            Telerik.Reporting.TocLevel tocLevel2 = new Telerik.Reporting.TocLevel();
            Telerik.Reporting.TocLevel tocLevel3 = new Telerik.Reporting.TocLevel();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            this.detailSection1 = new Telerik.Reporting.DetailSection();
            this.tocSection1 = new Telerik.Reporting.TocSection();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // detailSection1
            // 
            this.detailSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.30000004172325134D);
            this.detailSection1.Name = "detailSection1";
            this.detailSection1.Style.Visible = false;
            // 
            // tocSection1
            // 
            this.tocSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(1.8000000715255737D);
            this.tocSection1.KeepTogether = false;
            tocLevel1.LeaderSymbol = '\0';
            tocLevel1.Name = "Level1";
            tocLevel1.Style.Font.Bold = true;
            tocLevel1.Style.Font.Name = "Segoe UI Light";
            tocLevel1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(16D);
            tocLevel1.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Point(5D);
            tocLevel1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            tocLevel1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            tocLevel1.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Point(5D);
            tocLevel2.Name = "Level2";
            tocLevel2.Style.Font.Name = "Segoe UI Light";
            tocLevel2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(13D);
            tocLevel2.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Point(1D);
            tocLevel2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(32D);
            tocLevel2.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            tocLevel2.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Point(1D);
            tocLevel3.Name = "Level3";
            tocLevel3.Style.Font.Name = "Segoe UI Light";
            tocLevel3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
            tocLevel3.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Point(1D);
            tocLevel3.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(52D);
            tocLevel3.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            tocLevel3.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Point(1D);
            this.tocSection1.Levels.Add(tocLevel1);
            this.tocSection1.Levels.Add(tocLevel2);
            this.tocSection1.Levels.Add(tocLevel3);
            this.tocSection1.Name = "tocSection1";
            this.tocSection1.Title = "Report Book Table of Contents";
            this.tocSection1.TitleStyle.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.tocSection1.TitleStyle.BorderWidth.Bottom = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.tocSection1.TitleStyle.Font.Bold = true;
            this.tocSection1.TitleStyle.Font.Name = "Segoe UI Light";
            this.tocSection1.TitleStyle.Font.Size = Telerik.Reporting.Drawing.Unit.Point(25D);
            this.tocSection1.TitleStyle.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Point(10D);
            // 
            // ReportBookToc
            // 
            this.DocumentMapText = "Table of Contents";
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.detailSection1,
            this.tocSection1});
            this.Name = "ReportBookToc";
            this.PageSettings.ColumnCount = 1;
            this.PageSettings.ColumnSpacing = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.PageSettings.Landscape = false;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Inch(0.30000001192092896D), Telerik.Reporting.Drawing.Unit.Inch(0.40000000596046448D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Letter;
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule2.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1,
            styleRule2});
            this.Width = Telerik.Reporting.Drawing.Unit.Inch(8D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.DetailSection detailSection1;
        private Telerik.Reporting.TocSection tocSection1;
    }
}