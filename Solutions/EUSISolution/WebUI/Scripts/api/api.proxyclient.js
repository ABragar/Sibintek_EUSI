(function ($, pba_api) {

    'use strict';

    var create_template = function (template_string) {


        var uri_template = new URITemplate(template_string);

        return function (params, defaults, global_defaults, other_prefix) {

            var other_params = $.extend({}, params);

            params = $.extend({}, global_defaults, defaults, params);

            var uri = uri_template.expand(function (key) {

                delete other_params[key];

                return params[key];

            },
            { strict: true });

            var other_params_query = $.param(other_params);

            if (other_params_query !== "")
                uri = uri + other_prefix + other_params_query;

            return uri;
        }
    }


    var get_default_other_params_prefix = function (proxy_client) {
        return proxy_client.default_other_params_prefix;
    }


    var ProxyClient = function (default_params, default_options, default_other_params_prefix) {


        this.default_params = default_params;
        this.default_options = default_options;
        this.default_other_params_prefix = default_other_params_prefix;


        var that = this;

        this.create_func = function (query_template, default_options, default_params, other_params_prefix) {

            var template = create_template(query_template);


            var other_params_prefix_func = get_default_other_params_prefix;


            if (other_params_prefix) {
                other_params_prefix_func = function () { return other_params_prefix; }
            }

            return function (params, body, options) {

                var url = template(params, default_params, that.default_params, other_params_prefix_func(that));

                var opt = $.extend({}, that.default_options, default_options, options);

                if (body)
                    opt.data = JSON.stringify(body);

                return $.ajax(url, opt);

            }
        }
    }


    var proxyclient = new ProxyClient(
            {
                base_uri: "/api",
                _: Date.now
            },
            {
                method: "GET",
                contentType: "application/json",
                dataType: "json",
                cache: false
            },
            "?");


    proxyclient.crud = {
        get: proxyclient.create_func("{+base_uri}/crud/{mnemonic}/{id}{?_}", { method: "GET" }, null, "&"),
        getCustom: proxyclient.create_func("{+base_uri}/crud/{mnemonic}/child/{id}{?_}", { method: "GET" }, null, "&"),
        getByCode: proxyclient.create_func("{+base_uri}/crud/{mnemonic}/code/{code}", { method: "GET" }),
        destroy: proxyclient.create_func("{+base_uri}/crud/{mnemonic}/{id}", { method: "DELETE" }),
        save: proxyclient.create_func("{+base_uri}/crud/{mnemonic}/", { method: "POST" }),
        patch: proxyclient.create_func("{+base_uri}/crud/{mnemonic}/{id}", { method: "PATCH" }),
        getPreview: proxyclient.create_func("{+base_uri}/crud/{mnemonic}/preview/{id}{?_}", { method: "GET" }, null, "&")
    };

    proxyclient.wizard = {
        get_instance: proxyclient.create_func("{+base_uri}/wizard/{wizard}{?_}", { method: "GET" }, null, "&"),
        start: proxyclient.create_func("{+base_uri}/wizard/{wizard}", { method: "PUT" }),
        next: proxyclient.create_func("{+base_uri}/wizard/{wizard}/next", { method: "PUT" }),
        prev: proxyclient.create_func("{+base_uri}/wizard/{wizard}/prev", { method: "PUT" }),
        complete: proxyclient.create_func("{+base_uri}/wizard/{wizard}", { method: "POST" })
    };

    proxyclient.standard = {
        create_default: proxyclient.create_func("{+base_uri}/standard/{mnemonic}/create_default{?_}", { method: "GET" }, null, "&"),
        get_uiEnum: proxyclient.create_func("{+base_uri}/standard/getUiEnum/{type}{?_}", { method: "GET" }, null, "&"),
        clone: proxyclient.create_func("{+base_uri}/standard/clone/{mnemonic}/{id}{?_}", { method: "POST" }),
        getDvSettings: proxyclient.create_func("{+base_uri}/standard/dvSettings/{mnemonic}/{id}{?_}", { method: "GET" }, null, "&"),
        lookupPropertyValue: proxyclient.create_func("{+base_uri}/standard/lookupPropertyValue/{mnemonic}/{id}{?_}", { method: "GET" }, null, "&")
    };

    proxyclient.listview = {
        grid: {
            change_sortorder: proxyclient.create_func("{+base_uri}/listview/{mnemonic}/kendoGrid/changeSortOrder/{id}/{posId}", { method: "POST" })
        },
        change_category: proxyclient.create_func("{+base_uri}/listview/{mnemonic}/{id}/{categoryId}", { method: "POST" })
    };


    proxyclient.preset = {
        get: proxyclient.create_func("{+base_uri}/preset/{preset}/{ownerName}{?_}", { method: "GET" }, null, "&"),
        destroy: proxyclient.create_func("{+base_uri}/preset/{preset}", { method: "DELETE" }),
        save: proxyclient.create_func("{+base_uri}/preset/{preset}", { method: "POST" })
    };

    proxyclient.viewConfig = {
        getEditor: proxyclient.create_func("{+base_uri}/viewConfig/getEditor/{objectType}/{propertyName}{?_}", { method: "GET" }, null, "&"),
        getColumn: proxyclient.create_func("{+base_uri}/viewConfig/getColumn/{objectType}/{propertyName}{?_}", { method: "GET" }, null, "&"),
        getLookupPropertyValue: proxyclient.create_func("{+base_uri}/viewConfig/lookupProperty/value/{mnemonic}/{id}{?_}", { method: "GET" }, null, "&"),
        getLookupPropertyValuesForCollection: proxyclient.create_func("{+base_uri}/viewConfig/lookupProperty/collection/value/{mnemonic}?{ids}&{_}", { method: "GET" }, null, "&"),
        getExtraId: proxyclient.create_func("{+base_uri}/viewConfig/extraid/{mnemonic}/{id}{?_}", { method: "GET" }, null, "&"),
        getRelations: proxyclient.create_func("{+base_uri}/viewConfig/getRelations/{mnemonic}{?_}", { method: "GET" }, null, "&"),
        getConfig: proxyclient.create_func("{+base_uri}/viewConfig/{mnemonic}{?_}", { method: "GET" }, null, "&")
    };

    proxyclient.corpProp = {
        getActiveSociety: proxyclient.create_func("{+base_uri}/corpProp/getActiveSociety/{id}{?_}", { method: "GET" }, null, "&"),
        addInComplex: proxyclient.create_func("{+base_uri}/corpProp/addInComplex/{complexID}/{objectIds}{?_}", { method: "POST" }, null, "&"),
        addInComplexIO: proxyclient.create_func("{+base_uri}/corpProp/addInComplexIO/{complexID}/{objectIds}{?_}", { method: "POST" }, null, "&"),
        getUserProfile: proxyclient.create_func("{+base_uri}/corpProp/getUserProfile/{id}{?_}", { method: "GET" }, null, "&"),
        createScheduleStateRecord: proxyclient.create_func("{+base_uri}/corpProp/createScheduleStateRecord/{complexID}/{stateID}{?_}", { method: "POST" }, null, "&"),
        checkEstateInNCA: proxyclient.create_func("{+base_uri}/corpProp/checkEstateInNCA/{Ids}/{typeCode}{?_}", { method: "POST" }, null, "&"),
        createNonCoreAsset: proxyclient.create_func("{+base_uri}/corpProp/createNonCoreAsset/{Ids}/{typeCode}{?_}", { method: "POST" }, null, "&"),
        createNonCoreAssetFromPC: proxyclient.create_func("{+base_uri}/corpProp/createNonCoreAssetFromPC/{pcId}/{typeCode}{?_}", { method: "POST" }, null, "&"),
        addNonCoreAsset: proxyclient.create_func("{+base_uri}/corpProp/addNonCoreAsset/{ncaIds}/{listId}{?_}", { method: "POST" }, null, "&"),
        changeNCAStatus: proxyclient.create_func("{+base_uri}/corpProp/changeNCAStatus/{itemsIds}/{statusCode}{?_}", { method: "POST" }, null, "&"),
        duplicateNCAItem: proxyclient.create_func("{+base_uri}/corpProp/duplicateNCAItem/{ncaItemIds}/{ncaListId}{?_}", { method: "POST" }, null, "&"),
        createScheduleStateRegistrationRecords: proxyclient.create_func("{+base_uri}/corpProp/createScheduleStateRegistrationRecords/{itemsIds}/{ssrId}{?_}", { method: "POST" }, null, "&"),
        createScheduleStateTerminateRecords: proxyclient.create_func("{+base_uri}/corpProp/createScheduleStateTerminateRecords/{itemsIds}/{sstId}{?_}", { method: "POST" }, null, "&"),
        checkUpdRightInCorpProp: proxyclient.create_func("{+base_uri}/corpProp/checkUpdRightInCorpProp/{ids}{?_}", { method: "POST" }, null, "&"),
        checkUpdInCorpProp: proxyclient.create_func("{+base_uri}/corpProp/checkUpdInCorpProp/{id}{?_}", { method: "POST" }, null, "&"),
        updRightInCorpProp: proxyclient.create_func("{+base_uri}/corpProp/updRightInCorpProp/{ids}{?_}", { method: "POST" }, null, "&"),
        updInCorpProp: proxyclient.create_func("{+base_uri}/corpProp/updInCorpProp/{id}{?_}", { method: "POST" }, null, "&"),
        createProjectTemplate: proxyclient.create_func("{+base_uri}/corpProp/createProjectTemplate/{projectId}{?_}", { method: "POST" }, null, "&"),
        createTaskTemplate: proxyclient.create_func("{+base_uri}/corpProp/createTaskTemplate/{taskId}{?_}", { method: "POST" }, null, "&"),
        sendTaskNotification: proxyclient.create_func("{+base_uri}/corpProp/sendTaskNotification/{taskId}/{userId}{?_}", { method: "POST" }, null, "&"),
        fileImport: proxyclient.create_func("{+base_uri}/corpProp/fileImport/{fileCardIds}{?_}", { method: "POST" }, null, "&"),
        getHolidays: proxyclient.create_func("{+base_uri}/corpProp/getHolidays/{startDate}/{endDate}{?_}", { method: "GET" }, null, "&"),
        getMenuPreset: proxyclient.create_func("{+base_uri}/corpProp/getMenuPreset/{presetFor}{?_}", { method: "GET" }, null, "&"),
        removeNonCoreAsset: proxyclient.create_func("{+base_uri}/corpProp/removeNonCoreAsset/{id}{?_}", { method: "DELETE" }),
        removeItems: proxyclient.create_func("{+base_uri}/corpProp/removeItems/{mnemonic}/{ids}{?_}", { method: "DELETE" }),
        getAuditInfo: proxyclient.create_func("{+base_uri}/corpProp/getAuditInfo/{mnemonic}/{id}{?_}", { method: "GET" }, null, "&"),
        getIdByUid: proxyclient.create_func("{+base_uri}/corpProp/getIdByOid/{mnemonic}/{oid}{?_}", { method: "GET" }, null, "&"),
        createEstateAppraisal: proxyclient.create_func("{+base_uri}/corpProp/createEstateAppraisal/{appraisalId}/{ids}{?_}", { method: "POST" }, null, "&"),
        cancelImport: proxyclient.create_func("{+base_uri}/corpProp/cancelImport/{id}{?_}", { method: "POST" }, null, "&"),
        addNCAPreviousPeriod: proxyclient.create_func("{+base_uri}/corpProp/addNCAPreviousPeriod/{currentID}/{id}{?_}", { method: "POST" }, null, "&"),
        getByDate: proxyclient.create_func("{+base_uri}/corpProp/getByDate/{mnemonic}/{id}/{date}{?_}", { method: "GET" }, null, "&"),
        ncaChangeOG: proxyclient.create_func("{+base_uri}/corpProp/ncaChangeOG/{currentID}/{id}{?_}", { method: "POST" }, null, "&"),
        notifyOfImport: proxyclient.create_func("{+base_uri}/corpProp/notifyOfImport/{?_}", { method: "POST" }, null, "&"),
        getNNARowStates: proxyclient.create_func("{+base_uri}/corpProp/getNNARowStates/{?_}", { method: "GET" }, null, "&"),
        sendToArhive: proxyclient.create_func("{+base_uri}/corpProp/sendToArhive/{mnemonic}/{ids}{?_}", { method: "POST" }),
        
    };


    proxyclient.querytree = {
        get: proxyclient.create_func("{+base_uri}/querytree/{mnemonic}{?_}", { method: "GET" }, null, "&"),
        getAggregatableProperties: proxyclient.create_func("{+base_uri}/querytree/getAggregatableProperties/{mnemonic}", {method: "GET"}, null, "&")
    };

    pba_api.proxyclient = proxyclient;


}(window.jQuery, window.pbaAPI));