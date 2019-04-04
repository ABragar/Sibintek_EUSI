(function () {
    "use strict";

    corpProp.reporting = corpProp.reporting || {};

    corpProp.reporting.getReportByCode = function (code) {
        var reportUrl = application.reportService + "/manager/getByCode/" + code;

        if (application.IsWindowAuth === "False") {
            kendoReporting._refreshToken().done(
                function (res) {
                    var token = res.Token;
                    $.ajax({
                        url: reportUrl,
                        method: "GET",
                        headers: {
                            Authorization: "Bearer " + token
                        },
                        success: function (res) {
                            //                        console.log(res);
                            corpProp.reporting.showReport(res);
                        },
                        error: function (res) {
                            pbaAPI.errorMsg(res.statusText);
                        }
                    });
                });
        } else {
            $.ajax({
                url: reportUrl,
                method: "GET",
                success: function (res) {
                    //                        console.log(res);
                    corpProp.reporting.showReport(res);
                },
                error: function (res) {
                    pbaAPI.errorMsg(res.statusText);
                }
            });
        }
    };

    corpProp.reporting.showReport = function (report) {
        var reportParams = report.Params;
        var params = null;
        if (!reportParams) 
            corpProp.reporting.showWindow(report, params);                    
        else
        pbaAPI.proxyclient.corpProp.getUserProfile({ id: application.currentUser.id }).done(function(res) {
            if (reportParams) {
                debugger;
                params = reportParams.replace("@currentUserId", application.currentUser.id)
                    .replace("@currentSocietyId", res.SocietyIDEUP).replace("@currentYear", (new Date()).getFullYear());

                corpProp.reporting.showWindow(report, params);
            }
        });
    };

    corpProp.reporting.showWindow = function (report, params) {
        var reportUrl = application.reportService + "/api/reports";
        var obj = report;
        var reportParams = (params) ? JSON.parse("{" + params + "}") : null;

        var windowOptions = {
            height: "80%",
            width: "80%",
            title: obj.Name || "TestReport",
            isModal: true
        };

        var win = $("<div/>").kendoWindow(windowOptions).data("kendoWindow");

        win.content($("<div style='height: 100%;'/>").telerik_ReportViewer({
            serviceUrl: reportUrl,
            templateUrl: reportUrl + "/resources/templates/corpPropReportViewerTemplate.html",
            reportSource: {
                report: obj.GuidId + obj.Extension,
                parameters: reportParams
            },
            viewMode: "PRINT_PREVIEW"
        }));

        win.center();
        win.open();
    };
})();