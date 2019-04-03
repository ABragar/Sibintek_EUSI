Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports Telerik.Reporting
Imports Telerik.Reporting.Drawing

<Description("A choropleth map, showing the population density in the 100 most populated countries.")> _
Partial Public Class PopulationDensity
    Inherits Telerik.Reporting.Report
    Public Sub New()
        InitializeComponent()
    End Sub
End Class