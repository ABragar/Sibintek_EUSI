(function ($, api) {
    "use strict";

    var createUrl = "";
    var getUrl = "";
    var updateUrl = "";
    var deleteUrl = "";
    var downloadUrl = "";
    var listUrl = "";
    var reportUrl = "";
    var _token = "";

    $.extend(api, {
        _refreshToken: function () {
            return $.get("/Authorize/token");
        },
        _createRequest: function (options, handleUnauthorized, defer) {
            var self = this;
            defer = defer || $.Deferred();

            handleUnauthorized = handleUnauthorized == null ? true : handleUnauthorized;

            options = $.extend(options, {
                headers: {
                    "Authorization": "Bearer " + _token
                },
                success: function (res) {
                    defer.resolve(res);
                },
                error: function (err) {
                    if (err.status === 401 && handleUnauthorized) {
                        self._refreshToken().done(function (token) {
                            _token = token.Token;
                            self._createRequest($.extend(options, {
                                success: function (r) {
                                    defer.resolve(r);
                                },
                                error: function (e) {
                                    defer.reject(e);
                                }
                            }), false, defer);
                        });
                    } else {
                        pbaAPI.errorMsg("Произошла ошибка. Обратитесь к администратору.");
                        console.log("Необработанная ошибка.", err);
                        defer.reject(err);
                    }
                }
            });

            $.ajax(options);

            return defer;
        },
        setService: function (link) {
            createUrl = link + "/manager/create/";
            getUrl = link + "/manager/get/";
            updateUrl = link + "/manager/update/";
            deleteUrl = link + "/manager/delete/";
            downloadUrl = link + "/manager/download/";
            listUrl = link + "/manager/list/";
            reportUrl = link + "/api/reports";
        },
        create: function (obj, callback) {
            this._createRequest({
                url: createUrl,
                type: 'POST',
                dataType: "json",
                contentType: 'application/json',
                data: JSON.stringify(obj)
            }).done(callback);
        },
        get: function (id, callback) {
            this._createRequest({
                url: getUrl + id,
                type: 'GET'
            }).done(callback);
        },
        update: function (obj, callback) {
            this._createRequest({
                url: updateUrl,
                type: 'POST',
                dataType: "json",
                contentType: 'application/json',
                data: JSON.stringify(obj)
            }).done(callback);
        },
        delete: function (id, callback) {
            this._createRequest({
                url: deleteUrl + id,
                type: 'DELETE'
            }).done(callback);
        },
        download: function (id) {
            window.location.href = downloadUrl + id + "?bearer_token=" + _token;
        },
        onFileUpload: function (e) {
            var xhr = e.XMLHttpRequest;

            if (xhr) {
                xhr.addEventListener("readystatechange", function onReady(e) {
                    if (xhr.readyState === 1 /* OPENED */) {
                        xhr.setRequestHeader("Authorization", "Bearer " + _token);

                        xhr.removeEventListener("readystatechange", onReady);
                    }
                });
            }
        },
        getGridData: function () {
            return this._createRequest({
                url: listUrl,
                type: 'GET'
            });

        },
        preview: function (report, options) {
            var windowOptions = $.extend({
                height: "80%",
                width: "80%",
                title: "",
                isModal: true
            }, options);

            var params = '';
            if (options.Params)
                params = JSON.parse("{" + options.Params + "}");
            var win = $("<div/>").kendoWindow(windowOptions).data("kendoWindow");
            debugger;
            win.content($("<div style='height: 100%;'/>").telerik_ReportViewer({
                serviceUrl: reportUrl,
                templateUrl: reportUrl + "/resources/templates/telerikReportViewerTemplate.html",
                reportSource: {
                    report: report,
                    parameters: params
                },
                viewMode: "PRINT_PREVIEW"
            }));

            win.center().open();
        }
    });
})(jQuery, window.kendoReporting || (window.kendoReporting = {}));