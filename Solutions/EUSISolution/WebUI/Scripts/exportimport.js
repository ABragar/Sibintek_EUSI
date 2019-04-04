window.exportimport = window.exportimport || {};

(function () {

    exportimport.exportXML = function (grid, readurl) {
        var callback = function (e) {

            var item = e.ID;

            var dsparams = grid.widget().dataSource._params();
            var prepared = grid.widget().dataSource.transport.parameterMap(dsparams);

            var ajaxParams = {
                sort: prepared.sort,
                page: prepared.page,
                pageSize: prepared.PageSize,
                group: prepared.group,
                filter: prepared.filter
            };

            var url = grid.widget().dataSource.transport.options.read.url;
            if (!readurl)
                url = url.replace('Standart/KendoUI_CollectionRead', 'ExportImport/ExportXML');
            else {
                url = url.replace('Standart/KendoUI_CollectionRead', 'ExportImport/'+readurl.method);
            }
            url = pbaAPI.replaceUrlParametr(url, 'packageID', item);

            $.ajax({
                url: url,
                type: "POST",
                data: ajaxParams,
                success: function (result) {
                    var data = pbaAPI.base64ToBlob(result.data, result.mimeType);
                    pbaAPI.download(data, result.filename, result.mimetype);
                }
            });

        };

        $.ajax({
            url: '/ExportImport/GetPackages',
            type: "GET",
            data: { mnemonic: grid.mnemonic },
            success: function (testResult) {
                pbaAPI.selectSimple(testResult, { callback: callback });
            },
            dataType: "json",
            contentType: 'application/json'
        });
    }
})()