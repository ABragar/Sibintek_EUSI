@Imports Telerik.Reporting
@Imports Telerik.ReportViewer.Mvc
@Imports Telerik.Reporting.Examples.CSharp
@Code
    ViewBag.Title = "Telerik HTML5 Report Viewer MVC Demo"
End Code

@Section styles
    <link href="http://kendo.cdn.telerik.com/2015.3.930/styles/kendo.common.min.css" rel="stylesheet" id="common-css" />
    <link href="http://kendo.cdn.telerik.com/2015.3.930/styles/kendo.blueopal.min.css" rel="stylesheet" id="skin-css" />

    <style>
        body {
            margin: 5px;
            font-family: Verdana, Arial;
        }

        #reportViewer1 {
            position: absolute;
            left: 5px;
            right: 5px;
            top: 40px;
            bottom: 5px;
            overflow: hidden;
            clear: both;
        }

        #theme-switcher {
            float: right;
            width: 12em;
            height: 30px;
        }
    </style>
End Section

<select id="theme-switcher"></select>

@Code
    Dim reportSource As Telerik.Reporting.TypeReportSource = New Telerik.Reporting.TypeReportSource()
    reportSource.TypeName = GetType(ReportCatalog).AssemblyQualifiedName

    ' Each report viewer must have an id - it will be used by the initialization script
    ' to find the element and initialize the report viewer.
    ' .Id("reportViewer1") _
    '
    ' The URL of the service which will serve reports.
    ' The URL corresponds to the name of the controller class (ReportsController).
    ' For more information on how to configure the service please check http://www.telerik.com/help/reporting/telerik-reporting-rest-conception.html.
    ' .ServiceUrl("/api/reports/") _
    '
    ' The url for the report viewer template. The template can be edited -
    ' new functionalities can be added and unneeded ones can be removed.
    ' For more information please check http://www.telerik.com/help/reporting/html5-report-viewer-templates.html.
    ' .TemplateUrl("/ReportViewer/templates/telerikReportViewerTemplate.html") _
    '
    ' Strongly typed ReportSource - TypeReportSource or UriReportSource.
    ' .ReportSource(reportSource) _
    '
    ' Specifies whether the viewer is in interactive or print preview mode.
    ' PrintPreview - Displays the paginated report as if it is printed on paper. Interactivity is not enabled.
    ' Interactive - Displays the report in its original width and height witn no paging. Additionally interactivity is enabled.
    ' .ViewMode(ViewMode.Interactive) _
    '
    ' Sets the scale mode of the viewer.
    ' Three modes exist currently:
    ' FitPage - The whole report will fit on the page (will zoom in or out), regardless of its width and height.
    ' FitPageWidth - The report will be zoomed in or out so that the width of the screen and the width of the report match.
    ' Specific - Uses the scale to zoom in and out the report.
    ' .ScaleMode(ScaleMode.Specific) _
    '
    ' Zoom in and out the report using the scale
    ' 1.0 is equal to 100%, i.e. the original size of the report
    ' .Scale(1.0) _
    '
    ' Enables or disables the accessibility features of the report viewer and its contents.
    ' .EnableAccessibility(False) _
    '
    ' Sets whether the viewer’s client session to be persisted between the page’s refreshes(ex. postback).
    ' The session is stored in the browser’s sessionStorage and is available for the duration of the page session.
    ' .PersistSession(False)

    Dim viewer = Html.TelerikReporting().ReportViewer() _
.Id("reportViewer1") _
.ServiceUrl(Url.Content("~/api/reports/")) _
.ReportSource(reportSource) _
.ViewMode(ViewMode.Interactive) _
.ScaleMode(ScaleMode.Specific) _
.Scale(1.0) _
.PersistSession(False) _
.PrintMode(PrintMode.AutoSelect) _
.EnableAccessibility(False) _
.Deferred()
End Code

@viewer

@Section scripts
    <!--kendo.all.min.js can be used as well kendo.web.min.js and kendo.mobile.min.js-->
    <script src="@Url.Content("~/ReportViewer/js/telerikReportViewer.kendo.min.js")"></script>
    <script src="@Url.Content("~/Scripts/themeSwitcher.js")"></script>

    <!-- replace start -->
    <script src="@Url.Content("~/ReportViewer/js/utils.js")"></script>
    <script src="@Url.Content("~/ReportViewer/js/sr.js")"></script>
    <script src="@Url.Content("~/ReportViewer/js/print.js")"></script>
    <script src="@Url.Content("~/ReportViewer/js/serviceClient.js")"></script>
    <script src="@Url.Content("~/ReportViewer/js/controller.js")"></script>
    <script src="@Url.Content("~/ReportViewer/js/touch.js")"></script>
    <script src="@Url.Content("~/ReportViewer/js/pagesArea.js")"></script>
    <script src="@Url.Content("~/ReportViewer/js/documentMapArea.js")"></script>
    <script src="@Url.Content("~/ReportViewer/js/parameters.js")"></script>
    <script src="@Url.Content("~/ReportViewer/js/parameterValidators.js")"></script>
    <script src="@Url.Content("~/ReportViewer/js/parametersArea.js")"></script>
    <script src="@Url.Content("~/ReportViewer/js/commandSet.js")"></script>
    <script src="@Url.Content("~/ReportViewer/js/history.js")"></script>
    <script src="@Url.Content("~/ReportViewer/js/mainMenu.js")"></script>
    <script src="@Url.Content("~/ReportViewer/js/sideMenu.js")"></script>
    <script src="@Url.Content("~/ReportViewer/js/binder.js")"></script>
    <script src="@Url.Content("~/ReportViewer/js/uiController.js")"></script>
    <script src="@Url.Content("~/ReportViewer/js/perspectives.js")"></script>
    <script src="@Url.Content("~/ReportViewer/js/reportViewer.js")"></script>
    <script src="@Url.Content("~/ReportViewer/js/accessibility.js")"></script>
    <script src="@Url.Content("~/ReportViewer/js/search.js")"></script>
    <!-- replace end -->
    @Code
'All deferred initialization statements will be rendered here
    End Code
    @Html.TelerikReporting().DeferredScripts()
End Section

<script type="text/javascript">
    $(document).ready(function () {
        //Theme switcher
        themeSwitcher(
            '#theme-switcher',
            '#common-css',
            '#skin-css');
    });
</script>

