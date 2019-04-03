<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Telerik.Reporting.Examples.CSharp.WebFormsDemo.Default" %>

<%@ Register TagPrefix="telerik" Assembly="Telerik.ReportViewer.Html5.WebForms" Namespace="Telerik.ReportViewer.Html5.WebForms" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Telerik HTML5 Web Forms Report Viewer Demo</title>

    <script src="http://code.jquery.com/jquery-1.9.1.min.js"></script>
	<script src="http://kendo.cdn.telerik.com/2015.3.930/js/kendo.all.min.js"></script>
	
    <script src="Scripts/themeSwitcher.js"></script>

    <link href="http://kendo.cdn.telerik.com/2015.3.930/styles/kendo.common.min.css" rel="stylesheet" id="commonCss" />
    <link href="http://kendo.cdn.telerik.com/2015.3.930/styles/kendo.blueopal.min.css" rel="stylesheet" id="skinCss" />

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

    <script type="text/javascript">
        $(document).ready(function () {
            //Theme switcher
            themeSwitcher(
                '#theme-switcher',
                '#commonCss',
                '#skinCss');
        });
    </script>
</head>
<body>
    <form runat="server">
		<select id="theme-switcher"></select>

        <telerik:ReportViewer Width="" Height="" EnableAccessibility="false"
            ID="reportViewer1"
            runat="server">
            <ReportSource 
                IdentifierType="TypeReportSource" 
                Identifier="Telerik.Reporting.Examples.CSharp.ReportCatalog, CSharp.ReportLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null">
            </ReportSource>
        </telerik:ReportViewer>


    </form>
</body>
</html>
