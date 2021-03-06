"use strict";
var guid_1 = require("../utils/guid");
var Request = (function () {
    function Request(serviceIdentifier, parentContext, parentRequest, bindings, target) {
        if (target === void 0) { target = null; }
        this.guid = guid_1.guid();
        this.serviceIdentifier = serviceIdentifier;
        this.parentContext = parentContext;
        this.parentRequest = parentRequest;
        this.target = target;
        this.childRequests = [];
        this.bindings = (Array.isArray(bindings) ? bindings : ((bindings) ? [bindings] : []));
    }
    Request.prototype.addChildRequest = function (serviceIdentifier, bindings, target) {
        var child = new Request(serviceIdentifier, this.parentContext, this, bindings, target);
        this.childRequests.push(child);
        return child;
    };
    return Request;
}());
exports.Request = Request;
