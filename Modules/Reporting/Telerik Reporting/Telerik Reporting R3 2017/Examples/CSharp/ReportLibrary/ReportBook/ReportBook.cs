namespace Telerik.Reporting.Examples.CSharp
{
    using System.ComponentModel;

    [Description("A collection of Product-related reports")]
    public class ReportBook : Telerik.Reporting.ReportBook
    {
        public ReportBook()
        {
            AddTocTemplate();

            AddReports();
        }

        void AddTocTemplate()
        {
            var tocReportSource = new TypeReportSource();
            tocReportSource.TypeName = typeof(ReportBookToc).AssemblyQualifiedName;
            this.TocReportSource = tocReportSource;
        }

        void AddReports()
        {
            this.ReportSources.Add(new TypeReportSource
            {
                TypeName = typeof(SalesByRegionDashboardPart).AssemblyQualifiedName
            });

            this.ReportSources.Add(new TypeReportSource
            {
                TypeName = typeof(DashboardPart).AssemblyQualifiedName
            });

            this.ReportSources.Add(new TypeReportSource
            {
                TypeName = typeof(ProductSalesPart).AssemblyQualifiedName
            });

            this.ReportSources.Add(new TypeReportSource
            {
                TypeName = typeof(ProductCatalog).AssemblyQualifiedName
            });

            this.ReportSources.Add(new TypeReportSource
            {
                TypeName = typeof(ProductLineSalesPart).AssemblyQualifiedName
            });
        }
    }

    [Browsable(false)]
    class SalesByRegionDashboardPart : SalesByRegionDashboard
    {
        public SalesByRegionDashboardPart()
        {
            this.DocumentMapText = "Sales By Region";
        }
    }

    [Browsable(false)]
    class DashboardPart : Dashboard
    {
        public DashboardPart()
        {
            this.DocumentMapText = "Dashboard";
        }
    }

    [Browsable(false)]
    class ProductSalesPart : ProductSales
    {
        public ProductSalesPart()
        {
            this.DocumentMapText = "Product Sales";
        }
    }

    [Browsable(false)]
    class ProductLineSalesPart : ProductLineSales
    {
        public ProductLineSalesPart()
        {
            this.DocumentMapText = "Product Line Sales";
        }
    }
}
