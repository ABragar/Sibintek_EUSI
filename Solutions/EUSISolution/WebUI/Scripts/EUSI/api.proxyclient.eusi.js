(function ($, pba_api) {
    'use strict';

    pbaAPI.proxyclient.EUSI = {
        estateRegistrExport: pbaAPI.proxyclient.create_func("{+base_uri}/eusi/estateRegistrationExport/Export/{elementsIds}/{isAccountingObject}{?_}", { method: "GET" }, null, "&"),
        getWorkflowActionInfo: pbaAPI.proxyclient.create_func("{+base_uri}/eusi/estateRegistrationExport/getWfAction/{actionId}{?_}", { method: "GET" }, null, "&"),
        getGroupPosition: pbaAPI.proxyclient.create_func("{+base_uri}/eusi/estateRegistration/getGroupPosition/{id}{?_}", { method: "GET" }, null, "&"),
        exportMovings: pbaAPI.proxyclient.create_func("{+base_uri}/eusi/exportMoving/{mnemonic}", { method: "POST" }),
        calculateAccountingObject: pbaAPI.proxyclient.create_func("{+base_uri}/eusi/estateRegistration/eusi/calculateAccountingObject/{year}/{consolidationId}/{taxRateTypeCode}/{periodCalculatedNU}", { method: "POST" }),
        erOnDirected: pbaAPI.proxyclient.create_func("{+base_uri}/eusi/estateRegistration/erOnDirected/{ids}{?_}", { method: "POST" }, null, "&"),
        notifyOfERComplited: pbaAPI.proxyclient.create_func("{+base_uri}/eusi/estateRegistration/notifyOfERComplited/{ids}{?_}", { method: "POST" }, null, "&"),
        exportZip: pbaAPI.proxyclient.create_func("{+base_uri}/eusi/exportZip/{mnemonic}", { method: "POST" }),
        exportZipOS: pbaAPI.proxyclient.create_func("{+base_uri}/eusi/exportZipOS/{mnemonic}", { method: "POST" }),
        importER: pbaAPI.proxyclient.create_func("{+base_uri}/eusi/importER/{mnemonic}", { method: "POST" }),
        checkEntityBlock: pbaAPI.proxyclient.create_func("{+base_uri}/eusi/recordBlocker/checkRecordBlock/{entityId}/{entityMnemonic}", { method: "GET" }),
        removeEntityBlock: pbaAPI.proxyclient.create_func("{+base_uri}/eusi/recordBlocker/removeRecordBlock/{entityId}/{entityMnemonic}", { method: "POST" }),
        rentalOSExportZip: pbaAPI.proxyclient.create_func("{+base_uri}/eusi/rentalOSExportZip/{mnemonic}", { method: "POST" }),
        createMonitor: pbaAPI.proxyclient.create_func("{+base_uri}/eusi/createMonitor/{mnemonic}", { method: "POST" }),
        checkFileImportVersion: pbaAPI.proxyclient.create_func("{+base_uri}/eusi/import/checkFileVersions/{fileCardIds}{?_}", { method: "POST" }),
        sendEstateToArchive: pbaAPI.proxyclient.create_func("{+base_uri}/eusi/archive/sendEstateToArchive/{mnemonic}", { method: "POST" }),
        returnEstateFromArchive: pbaAPI.proxyclient.create_func("{+base_uri}/eusi/archive/returnEstateFromArchive/{mnemonic}", { method: "POST" }),
        sendObuToArchive: pbaAPI.proxyclient.create_func("{+base_uri}/eusi/archive/sendObuToArchive/{mnemonic}", { method: "POST" }),
        returnObuFromArchive: pbaAPI.proxyclient.create_func("{+base_uri}/eusi/archive/returnObuFromArchive/{mnemonic}", { method: "POST" }),
        notifyOfNotGettingData: pbaAPI.proxyclient.create_func("{+base_uri}/eusi/estateRegistration/notifyOfNotGettingData/{ids}{?_}", { method: "POST" }, null, "&"),
        updateTaxBaseCadastralObjects: pbaAPI.proxyclient.create_func("{+base_uri}/eusi/updateTaxBaseCadastralObjects/{mnemonic}", { method: "POST" }),
        logUndefinedError: pbaAPI.proxyclient.create_func("{+base_uri}/eusi/error/logUndefinedError", { method: "POST" })
    };
}(window.jQuery, window.pbaAPI));