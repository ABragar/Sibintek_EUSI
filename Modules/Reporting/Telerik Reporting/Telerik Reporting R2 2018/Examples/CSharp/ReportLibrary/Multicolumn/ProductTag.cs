namespace Telerik.Reporting.Examples.CSharp
{
    using System.ComponentModel;

    /// <summary>
    /// A report for printing business cards.
    /// </summary>
    [Description("Printable product tags arranged in a newspaper-style columns")]
    public partial class ProductTagReport : Report
    {
        public ProductTagReport()
        {
            /// <summary>
            /// Required for Telerik Reporting designer support
            /// </summary>
            InitializeComponent();
        }
    }    
}

