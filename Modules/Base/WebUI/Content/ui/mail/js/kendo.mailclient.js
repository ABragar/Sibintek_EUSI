(function(f, define) {
    "use strict";
    define("kendo.mailclient.ns", [], f);
})(function() {
    "use strict";
    kendo.ui.mailclient = {};
    return window.kendo;
}, typeof define == "function" && define.amd ? define : function(a1, a2, a3) {
    "use strict";
    (a3 || a2)();
});

(function(f, define) {
    "use strict";
    define("kendo.mailclient.utils", [ "kendo.mailclient.ns" ], f);
})(function() {
    "use strict";
    (function($, undefined) {
        var kendo = window.kendo, ui = kendo.ui, mailclientNS = ui.mailclient, extend = $.extend, localStorage = window.localStorage, ObservableObject = kendo.data.ObservableObject, CURRENT_DATE = new Date(), DEACTIVATE = "deactivate", CLOSE = "close", KENDO_WINDOW = "kendoMailClientWindow", KENDO_MODAL = "kendoMailClientModal";
        function areSameDate(date1, date2) {
            return date1.getFullYear() === date2.getFullYear() && date1.getMonth() === date2.getMonth() && date1.getDate() === date2.getDate();
        }
        function areSameYear(date1, date2) {
            return date1.getFullYear() === date2.getFullYear();
        }
        function isDate(value) {
            return value !== undefined && value !== null && value instanceof Date;
        }
        function defGetSetProp(proto, name, get, set) {
            if (!get && !set) {
                return;
            }
            var descr = {
                enumerable: true,
                configurable: true
            };
            if (get) {
                descr.get = get;
            }
            if (set) {
                descr.set = set;
            }
            Object.defineProperty(proto, name, descr);
        }
        function defGetSetProps(proto, def) {
            if (!def || $.isEmptyObject(def)) {
                return;
            }
            var prop;
            for (prop in def) {
                var descr = def[prop];
                var get = typeof descr === "function" ? descr : descr.get;
                var set = descr.set;
                defGetSetProp(proto, prop, get, set);
            }
        }
        function createIframe(iframeElement, scrolling) {
            var element = iframeElement, initMarker = "__init_iframe__", resizeDelay = 60, resizeTimer = timer(resizeDelay, resize);
            function resize() {
                var iframeWnd = element.contentWindow, body = iframeWnd ? iframeWnd.document.body : null;
                if (body) {
                    element.style.height = body.scrollHeight + "px";
                    body.onresize = _resizeWithDelay;
                }
            }
            function _resizeWithDelay() {
                resizeTimer.restart();
            }
            function setHtml(html) {
                var doc = element.contentWindow ? element.contentWindow.document : element.contentDocument;
                doc.open();
                doc.write(html);
                doc.close();
            }
            function _onload() {
                try {
                    if (element) {
                        resize();
                    }
                } catch (error) {
                    traceError(error.name + " " + error.message);
                }
            }
            function isInit() {
                return element[initMarker];
            }
            function init() {
                element.onload = _onload;
                element[initMarker] = true;
                element.marginWidth = "0";
                element.marginHeight = "0";
                element.frameBorder = "0";
                _initScrolling();
            }
            function _initScrolling() {
                if (scrolling) {
                    element.scrolling = "yes";
                    element.style.overflow = "auto";
                } else {
                    element.scrolling = "no";
                    element.style.overflow = "hidden";
                }
            }
            function destroy() {
                element.onload = null;
                delete element[initMarker];
                element = null;
            }
            init();
            return {
                init: init,
                resize: resize,
                element: element,
                setHtml: setHtml,
                isInit: isInit,
                destroy: destroy
            };
        }
        createIframe.isInit = function(element) {
            return element && element.__init_iframe__;
        };
        function createWndDialog(options, viewModel, isModal) {
            var template = options.template, windowId = options.windowId, isMaximize = options.isMaximize, onCloseFn = $.noop, container = window.document.body, dlgResult = "dialogResult", wnd = null, view = null;
            function _view() {
                if (!container) {
                    return null;
                }
                var view = new kendo.View(template, {
                    model: viewModel,
                    evalTemplate: true,
                    wrap: true
                });
                viewModel.bind(CLOSE, close);
                view.render(container);
                return view;
            }
            function _find() {
                var elementId = kendo.format("#" + windowId, viewModel.uid);
                return $(elementId).data(isModal ? KENDO_MODAL : KENDO_WINDOW);
            }
            function _setup(wnd) {
                if (isModal) {
                    wnd.bind(CLOSE, _handleClose);
                } else {
                    wnd.bind(DEACTIVATE, _handleClose);
                }
            }
            function _open(wnd) {
                wnd.center();
                wnd.open();
                if (isMaximize) {
                    wnd.maximize();
                }
            }
            function show() {
                view = _view();
                if (!view) {
                    return;
                }
                wnd = _find();
                if (wnd) {
                    _setup(wnd);
                    _open(wnd);
                }
            }
            function close() {
                if (wnd) {
                    wnd.close();
                }
            }
            function _destroy() {
                if (wnd) {
                    wnd.unbind();
                    wnd.destroy();
                    wnd = null;
                }
                if (view) {
                    view.destroy();
                    view = null;
                }
                onCloseFn = container = null;
            }
            function _handleClose() {
                onCloseFn.call(null, viewModel.get(dlgResult));
                _destroy();
            }
            function onClose(fn) {
                if (typeof fn === "function") {
                    onCloseFn = fn;
                }
            }
            function getWnd() {
                return wnd;
            }
            return {
                show: show,
                close: close,
                onClose: onClose,
                getWnd: getWnd
            };
        }
        function confirm(text, title, messages, descr, width) {
            var confirmOpt = {
                content: text
            };
            if (title !== undefined) {
                confirmOpt.title = title;
            }
            if (messages !== undefined) {
                confirmOpt.messages = messages;
            }
            if (width !== undefined) {
                confirmOpt.width = width;
            }
            if (descr) {
                var descrHtml = '<div class="k-confirm-descr">';
                descrHtml += '<i class="fa fa-exclamation-triangle"></i><span>' + descr + "</span>";
                descrHtml += "</div>";
                confirmOpt.content = confirmOpt.content + descrHtml;
            }
            var confirmDialog = $("<div />").kendoConfirm(confirmOpt).data("kendoConfirm").open();
            return confirmDialog.result;
        }
        function storage(namespace) {
            var store = load();
            function set(key, value) {
                store[key] = value;
            }
            function get(key) {
                return store[key];
            }
            function load() {
                var result;
                try {
                    result = JSON.parse(localStorage.getItem(namespace));
                } catch (e) {
                    result = null;
                }
                return result || {};
            }
            function reset() {
                store = load();
            }
            function save(isClose) {
                localStorage.setItem(namespace, JSON.stringify(store));
                if (isClose) {
                    close();
                }
            }
            function close() {
                store = null;
            }
            return {
                get: get,
                set: set,
                save: save,
                reset: reset,
                close: close,
                load: load,
                open: reset
            };
        }
        function timer(timeout, callback) {
            var handle = null;
            function start() {
                cancel();
                handle = window.setTimeout(function() {
                    if (typeof callback === "function") {
                        callback.call(null);
                    }
                }, timeout);
            }
            function cancel() {
                if (handle !== null) {
                    window.clearTimeout(handle);
                    handle = null;
                }
            }
            function restart() {
                cancel();
                start();
            }
            return {
                start: start,
                cancel: cancel,
                restart: restart
            };
        }
        function post(url, data, bearerToken, withCredentials, ret) {
            return request("POST", url, data, bearerToken, withCredentials, ret);
        }
        function put(url, data, bearerToken, withCredentials, ret) {
            return request("PUT", url, data, bearerToken, withCredentials, ret);
        }
        function get(url, data, bearerToken, withCredentials, ret) {
            return request("GET", url, data, bearerToken, withCredentials, ret);
        }
        function del(url, data, bearerToken, withCredentials, ret) {
            return request("DELETE", url, data, bearerToken, withCredentials, ret);
        }
        function request(method, url, data, bearerToken, withCredentials, ret) {
            var defer = $.Deferred();
            if (data !== null && data !== undefined) {
                data = JSON.stringify(data);
            }
            var options = {
                type: method,
                url: url,
                data: data,
                dataType: "json",
                cache: false,
                contentType: "application/json; charset=utf-8",
                xhrFields: {},
                beforeSend: function(xhr) {
                    if (bearerToken) {
                        xhr.setRequestHeader("Authorization", "Bearer " + bearerToken);
                    }
                },
                error: function(jqXHR, textStatus, errorThrown) {
                    defer.reject(makeReqError(jqXHR, textStatus, errorThrown));
                },
                success: function(response, textStatus, jqXHR) {
                    defer.resolve({
                        response: response,
                        textStatus: textStatus,
                        jqXHR: jqXHR
                    });
                }
            };
            if (typeof withCredentials === "boolean") {
                options.xhrFields.withCredentials = withCredentials;
            }
            var xhr = $.ajax(options);
            if (ret) {
                ret.xhr = xhr;
            }
            return defer.promise();
        }
        function makeReqError(jqXHR, textStatus, errorThrown) {
            var error = new Error("Request failed.");
            error.textStatus = textStatus;
            error.errorThrown = errorThrown;
            error.jqXHR = jqXHR;
            error.toString = function() {
                var string = Error.prototype.toString.call(this);
                string += " " + this.jqXHR.status;
                string += " " + this.jqXHR.statusText;
                string += " " + this.textStatus;
                string += " " + this.errorThrown;
                return string;
            };
            return error;
        }
        function fileExtension(fileName, dot) {
            var matches = fileName.match(/\.([^\.]+)$/);
            return matches ? matches[dot ? 0 : 1] : "";
        }
        function humanFileSize(bytes, si) {
            var thresh = si ? 1e3 : 1024;
            if (Math.abs(bytes) < thresh) {
                return bytes + " Б";
            }
            var units = si ? [ "КБ", "МБ", "ГБ", "ТБ", "PB", "EB", "ZB", "YB" ] : [ "КБ", "МБ", "ГБ", "ТБ", "PiB", "EiB", "ZiB", "YiB" ];
            var u = -1;
            do {
                bytes /= thresh;
                ++u;
            } while (Math.abs(bytes) >= thresh && u < units.length - 1);
            return bytes.toFixed(1) + " " + units[u];
        }
        function initials(name, separator, partCount) {
            separator = separator || " ";
            partCount = partCount || 2;
            var nameParts = (name || "").split(separator), result = "";
            for (var i = 0; i < partCount; i++) {
                if (nameParts[i]) {
                    result += nameParts[i].charAt(0).toUpperCase();
                }
            }
            return result;
        }
        function getNoun(number, one, two, five) {
            number = Math.abs(number);
            number %= 100;
            if (number >= 5 && number <= 20) {
                return five;
            }
            number %= 10;
            if (number == 1) {
                return one;
            }
            if (number >= 2 && number <= 4) {
                return two;
            }
            return five;
        }
        function bindingTargetSource(element) {
            return element && element.kendoBindingTarget && element.kendoBindingTarget.source instanceof ObservableObject ? element.kendoBindingTarget.source : null;
        }
        function traceError(msg) {
            if (console && console.error) {
                console.error(msg);
            } else if (console && console.log) {
                console.log(msg);
            }
        }
        function isFn(obj, fn) {
            return obj !== null && obj !== undefined && typeof obj[fn] === "function";
        }
        function isObject(obj) {
            return obj !== null && obj !== undefined && Object.prototype.toString.call(obj) === "[object Object]";
        }
        function compareObjects(o1, o2, left) {
            var p1, p2;
            for (p1 in o1) {
                if (o1.hasOwnProperty(p1)) {
                    if (o1[p1] !== o2[p1]) {
                        return false;
                    }
                }
            }
            if (!left) {
                for (p2 in o2) {
                    if (o2.hasOwnProperty(p2)) {
                        if (o1[p2] !== o2[p2]) {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        function notNullFilter(obj) {
            return obj !== null && obj !== undefined;
        }
        function filterByField(name, value) {
            return function(item) {
                return item[name] === value;
            };
        }
        function onlyUnique(value, index, self) {
            return self.indexOf(value) === index;
        }
        function findByRef(array, ref) {
            var result = array.filter(function(item) {
                return item === ref;
            });
            return result.length > 0 ? result[0] : null;
        }
        function selectField(name) {
            return function(item) {
                return item[name];
            };
        }
        function arrayCount(array, filter) {
            return array.filter(filter).length;
        }
        function arrayFlatten(arr) {
            return arr.reduce(function(flat, toFlatten) {
                return flat.concat(Array.isArray(toFlatten) ? arrayFlatten(toFlatten) : toFlatten);
            }, []);
        }
        function arrayIntersect(a, b) {
            var t;
            if (b.length > a.length) {
                t = b;
                b = a;
                a = t;
            }
            return a.filter(function(e) {
                if (b.indexOf(e) !== -1) {
                    return true;
                }
            });
        }
        function arrayFirst(array) {
            return array.length > 0 ? array[0] : null;
        }
        function arrayLast(array) {
            return array.length > 0 ? array[array.length - 1] : null;
        }
        extend(mailclientNS, {
            CURRENT_DATE: CURRENT_DATE,
            createIframe: createIframe,
            timer: timer,
            traceError: traceError,
            areSameDate: areSameDate,
            areSameYear: areSameYear,
            isDate: isDate,
            defGetSetProp: defGetSetProp,
            defGetSetProps: defGetSetProps,
            storage: storage,
            createWndDialog: createWndDialog,
            post: post,
            put: put,
            get: get,
            del: del,
            request: request,
            fileExtension: fileExtension,
            humanFileSize: humanFileSize,
            isFn: isFn,
            isObject: isObject,
            bindingTargetSource: bindingTargetSource,
            notNullFilter: notNullFilter,
            filterByField: filterByField,
            findByRef: findByRef,
            onlyUnique: onlyUnique,
            selectField: selectField,
            arrayCount: arrayCount,
            arrayFlatten: arrayFlatten,
            arrayIntersect: arrayIntersect,
            first: arrayFirst,
            last: arrayLast,
            initials: initials,
            confirm: confirm,
            compareObjects: compareObjects,
            getNoun: getNoun
        });
    })(window.kendo.jQuery);
    return window.kendo;
}, typeof define == "function" && define.amd ? define : function(a1, a2, a3) {
    "use strict";
    (a3 || a2)();
});

(function(f, define) {
    "use strict";
    define("kendo.mailclient.constants", [ "kendo.mailclient.ns" ], f);
})(function() {
    "use strict";
    (function($, undefined) {
        var kendo = window.kendo, ui = kendo.ui, mailclientNS = ui.mailclient, extend = $.extend;
        var FolderType = {
            None: "None",
            All: "All",
            Inbox: "Inbox",
            Sent: "Sent",
            Drafts: "Drafts",
            Junk: "Junk",
            Trash: "Trash",
            Archive: "Archive",
            Flagged: "Flagged"
        };
        var CommandState = {
            Init: 1,
            Pending: 2,
            Success: 3,
            Failed: 4
        };
        var Commands = {
            MoveTo: "MoveTo",
            MoveAll: "MoveAll",
            AddFlag: "AddFlag",
            RemoveFlag: "RemoveFlag",
            MarkAsRead: "MarkAsRead",
            MarkAsUnread: "MarkAsUnread",
            Delete: "Delete",
            DeleteAll: "DeleteAll",
            Compose: "Compose",
            Reply: "Reply",
            Forward: "Forward",
            Refresh: "Refresh",
            NewFolder: "NewFolder",
            UpdateFolder: "ChangeFolder",
            RemoveFolder: "RemoveFolder",
            RemoveFolderToTrash: "RemoveFolderToTrash",
            MarkAsReadFolder: "MarkAsReadFolder",
            ClearAllFolder: "ClearAllFolder",
            RefreshMessages: "RefreshMessages",
            EditDraft: "EditDraft",
            EditSettings: "EditSettings",
            EditAsNew: "EditAsNew"
        };
        var FolderIconMap = {};
        FolderIconMap[FolderType.Inbox] = "inbox";
        FolderIconMap[FolderType.Drafts] = "sticky-note-o";
        FolderIconMap[FolderType.Sent] = "paper-plane-o";
        FolderIconMap[FolderType.Trash] = "trash-o";
        FolderIconMap[FolderType.All] = "folder-o";
        FolderIconMap[FolderType.Flagged] = "folder-o";
        FolderIconMap[FolderType.Junk] = "exclamation-circle";
        FolderIconMap[FolderType.Archive] = "folder-o";
        FolderIconMap[FolderType.None] = "folder-o";
        var FolderTranslateMap = {};
        FolderTranslateMap[FolderType.Inbox] = "inbox";
        FolderTranslateMap[FolderType.Drafts] = "drafts";
        FolderTranslateMap[FolderType.Sent] = "sent";
        FolderTranslateMap[FolderType.Trash] = "trash";
        FolderTranslateMap[FolderType.Junk] = "junk";
        var BroadcastEvents = {
            CHANGE_FOLDER: "changeFolder",
            COMPOSE_WND_FOCUS: "composeWndFocus",
            MARK_AS_UNREAD: "markAsUnread",
            MARK_AS_READ: "markAsRead",
            SELECT_FOLDER: "selectFolder"
        };
        var HttpCodes = {
            UNAUTHORIZED: 401
        };
        extend(mailclientNS, {
            FolderType: FolderType,
            CommandState: CommandState,
            Commands: Commands,
            FolderIconMap: FolderIconMap,
            FolderTranslateMap: FolderTranslateMap,
            BroadcastEvents: BroadcastEvents,
            HttpCodes: HttpCodes
        });
    })(window.kendo.jQuery);
    return window.kendo;
}, typeof define == "function" && define.amd ? define : function(a1, a2, a3) {
    "use strict";
    (a3 || a2)();
});

(function(f, define) {
    "use strict";
    define("kendo.mailclient.models", [ "kendo.mailclient.ns", "kendo.mailclient.utils", "kendo.mailclient.constants" ], f);
})(function() {
    "use strict";
    (function($, undefined) {
        var kendo = window.kendo, ui = kendo.ui, extend = $.extend, Class = kendo.Class, Model = kendo.data.Model, mailclientNS = ui.mailclient, FolderIconMap = mailclientNS.FolderIconMap, FolderType = mailclientNS.FolderType, isDate = mailclientNS.isDate, isObject = mailclientNS.isObject, isArray = Array.isArray, areSameDate = mailclientNS.areSameDate, areSameYear = mailclientNS.areSameYear, defGetSetProps = mailclientNS.defGetSetProps, humanFileSize = mailclientNS.humanFileSize, fileExtension = mailclientNS.fileExtension, CURRENT_DATE = mailclientNS.CURRENT_DATE;
        var AccountModel = Model.define({
            id: "ClientId",
            fields: {
                name: {
                    type: "string",
                    field: "Name"
                },
                address: {
                    type: "string",
                    field: "Address"
                }
            }
        });
        var FolderModel = Model.define({
            id: "FolderId",
            fields: {
                displayName: {
                    type: "string",
                    field: "Title"
                },
                type: {
                    type: "string",
                    field: "Type"
                },
                unreadItemCount: {
                    type: "number",
                    field: "UnreadCount"
                },
                totalItemCount: {
                    type: "number",
                    field: "TotalCount"
                },
                recentItemCount: {
                    type: "number",
                    field: "RecentCount"
                },
                parentId: {
                    type: "string",
                    field: "ParentId",
                    nullable: true
                },
                sortOrder: {
                    type: "number",
                    field: "SortOrder"
                },
                hasChildren: {
                    type: "boolean",
                    field: "hasChildren",
                    defaultValue: false
                },
                expanded: {
                    type: "boolean",
                    field: "expanded",
                    defaultValue: false
                },
                children: {
                    type: "object",
                    field: "children"
                }
            }
        });
        var AttachmentModel = Model.define({
            id: "FileId",
            fields: {
                FileName: {
                    type: "string",
                    field: "FileName"
                },
                FileSize: {
                    type: "number",
                    field: "FileSize"
                }
            }
        });
        var ContactModel = Model.define({
            id: "ID",
            fields: {
                Name: {
                    type: "string",
                    field: "Title"
                },
                Address: {
                    type: "string",
                    field: "Email"
                }
            }
        });
        var BaseMessageModelFields = {
            uniqueId: {
                type: "number",
                field: "UniqueId"
            },
            folderId: {
                type: "string",
                field: "FolderId"
            },
            subject: {
                type: "string",
                field: "Subject",
                defaultValue: ""
            },
            from: {
                type: "object",
                field: "From"
            },
            cc: {
                type: "object",
                field: "Cc"
            },
            bcc: {
                type: "object",
                field: "Bcc"
            },
            to: {
                type: "object",
                field: "To"
            },
            sentDate: {
                type: "date",
                field: "Date"
            },
            isRead: {
                type: "boolean",
                field: "IsRead",
                defaultValue: false
            },
            isFlag: {
                type: "boolean",
                field: "IsFlag",
                defaultValue: false
            },
            isDraft: {
                type: "boolean",
                field: "IsDraft",
                defaultValue: false
            },
            isSend: {
                type: "boolean",
                field: "IsSend",
                defaultValue: false
            },
            isSent: {
                type: "boolean",
                field: "IsSent",
                defaultValue: false
            },
            highPriority: {
                type: "boolean",
                field: "HighPriority",
                defaultValue: false
            },
            confirmRead: {
                type: "boolean",
                field: "ConfirmRead",
                defaultValue: false
            },
            confirmDelivery: {
                type: "boolean",
                field: "ConfirmDelivery",
                defaultValue: false
            }
        };
        var MessageListItemModel = Model.define({
            id: "uniqueId",
            fields: extend({}, BaseMessageModelFields, {
                hasAttachments: {
                    type: "boolean",
                    field: "HasAttachments",
                    defaultValue: false
                },
                bodyPreview: {
                    type: "string",
                    field: "BodyPreview"
                }
            })
        });
        var MessageDetailsModel = Model.define({
            id: "uniqueId",
            fields: extend({}, BaseMessageModelFields, {
                body: {
                    type: "string",
                    field: "Body"
                },
                isHtml: {
                    type: "boolean",
                    field: "IsHtmlBody"
                },
                attachments: {
                    type: "object",
                    field: "Attachments",
                    parse: function(value) {
                        if (typeof value.map === "function") {
                            return value.map(function(item) {
                                return new AttachmentModel(item);
                            });
                        }
                        return [];
                    }
                }
            })
        });
        var BaseMessageModelProps = {
            fromName: function() {
                return this.from ? this.from.Name : null;
            },
            fromAddress: function() {
                return this.from ? this.from.Address : null;
            },
            toName: function() {
                return this.to && this.to[0] ? this.to[0].Name : null;
            },
            toAddress: function() {
                return this.to && this.to[0] ? this.to[0].Address : null;
            },
            sentDateFormatted: function() {
                if (isDate(this.sentDate)) {
                    if (areSameDate(this.sentDate, CURRENT_DATE)) {
                        return kendo.toString(this.sentDate, "t");
                    } else if (areSameYear(this.sentDate, CURRENT_DATE)) {
                        return kendo.toString(this.sentDate, "MMM d");
                    } else {
                        return kendo.toString(this.sentDate, "MMM d, yy");
                    }
                }
                return "";
            },
            isNotFlag: function() {
                return !this.get("isFlag");
            },
            isUnread: function() {
                return !this.get("isRead");
            },
            isFlagShow: function() {
                return this.get("isFlag") && this.get("isRead");
            }
        };
        var BaseMessageModelProto = {
            setRead: function(value) {
                this.set("isRead", value);
            },
            setFlag: function(value) {
                this.set("isFlag", value);
            },
            recipientsToString: function(recipients, def, sep) {
                recipients = recipients || [];
                sep = sep !== undefined ? sep : ", ";
                return recipients.map(function(item) {
                    return item.Name || item.Address || def;
                }).join(sep);
            }
        };
        MessageListItemModel.createFromDetails = function(messageDetailsModel) {
            if (messageDetailsModel instanceof Model) {
                var data = messageDetailsModel.toJSON();
                data.hasAttachments = data.attachments.length > 0;
                delete data.body;
                delete data.isHtml;
                delete data.attachments;
                return new MessageListItemModel(data);
            }
        };
        extend(MessageListItemModel.prototype, BaseMessageModelProto);
        extend(MessageDetailsModel.prototype, BaseMessageModelProto);
        defGetSetProps(MessageListItemModel.prototype, BaseMessageModelProps);
        defGetSetProps(MessageDetailsModel.prototype, BaseMessageModelProps);
        extend(MessageListItemModel.prototype, {
            setId: function(value) {
                this.set(MessageListItemModel.idField, value);
                this.id = this.get(MessageListItemModel.idField);
            }
        });
        extend(MessageDetailsModel.prototype, {
            setId: function(value) {
                this.set(MessageDetailsModel.idField, value);
                this.id = this.get(MessageDetailsModel.idField);
            }
        });
        defGetSetProps(AccountModel.prototype, {
            displayName: function() {
                return this.address;
            }
        });
        defGetSetProps(FolderModel.prototype, {
            icon: function() {
                return FolderIconMap[this.type];
            },
            itemCount: function() {
                return this.get("unreadItemCount") || 0;
            },
            notExpanded: function() {
                return !this.get("expanded");
            },
            isSystem: function() {
                return this.get("type") !== FolderType.None;
            },
            isCustom: function() {
                return this.get("type") === FolderType.None;
            }
        });
        var FolderModelProto = {
            subUnreadItemCount: function(value) {
                var val = this.unreadItemCount - value;
                this.set("unreadItemCount", val < 0 ? 0 : val);
            },
            addUnreadItemCount: function(value) {
                this.set("unreadItemCount", this.unreadItemCount + value);
            },
            addTotalItemCount: function(value) {
                this.set("totalItemCount", this.totalItemCount + value);
            },
            subTotalItemCount: function(value) {
                var val = this.totalItemCount - value;
                this.set("totalItemCount", val < 0 ? 0 : val);
            },
            setUnreadItemCount: function(value) {
                this.set("unreadItemCount", value);
            },
            resetCounters: function() {
                this.set("totalItemCount", 0);
                this.set("unreadItemCount", 0);
            },
            toggleExpanded: function() {
                return this.set("expanded", !this.get("expanded"));
            },
            setExpand: function(value) {
                this.set("expanded", value);
            },
            setEdit: function(value) {
                this.set("edit", value);
            },
            setHasChildren: function(value) {
                this.set("hasChildren", value);
            },
            setChildren: function(value) {
                this.set("children", value);
            },
            setSortOrder: function(value) {
                this.set("sortOrder", value);
            },
            setParentId: function(value) {
                this.set("parentId", value);
            },
            incrSortOrder: function(max) {
                var value = max || this.sortOrder || 0;
                this.set("sortOrder", value + 1);
            },
            setDisplayName: function(value) {
                this.set("displayName", value);
            },
            setId: function(value) {
                this.set(FolderModel.idField, value);
                this.id = this.get(FolderModel.idField);
            }
        };
        extend(FolderModel.prototype, FolderModelProto);
        FolderModel.create = function(id, parentId, title) {
            return new FolderModel({
                FolderId: id,
                parentId: parentId,
                sortOrder: 0,
                displayName: title,
                type: FolderType.None,
                totalItemCount: 0,
                recentItemCount: 0,
                unreadItemCount: 0,
                expanded: false,
                hasChildren: false,
                children: []
            });
        };
        FolderModel.from = function(rawData) {
            rawData = rawData || {};
            var fieldsSpec = FolderModel.fields;
            var idField = FolderModel.idField;
            var data = {};
            data[idField] = rawData[idField];
            Object.keys(fieldsSpec).forEach(function(field) {
                var fieldSpec = fieldsSpec[field];
                if (fieldSpec.field) {
                    data[field] = rawData[fieldSpec.field];
                }
            });
            return new FolderModel(data);
        };
        defGetSetProps(AttachmentModel.prototype, {
            HumanFileSize: function() {
                return humanFileSize(this.FileSize);
            },
            FileExt: function() {
                return fileExtension(this.FileName || "");
            }
        });
        var Recipient = Class.extend({
            init: function(name, address) {
                this.name = name || "";
                this.address = address || "";
            },
            toString: function() {
                if (!this.name || this.name === this.address && this.address) {
                    return "<" + this.address + ">";
                } else if (this.name && !this.address) {
                    return this.name;
                } else {
                    return this.name + " <" + this.address + ">";
                }
            }
        });
        Recipient.fromRaw = function(data) {
            if (isArray(data)) {
                return data.map(Recipient.fromRaw);
            } else if (isObject(data)) {
                return new Recipient(data.Name, data.Address);
            } else {
                throw new Error("Invalid raw data.");
            }
        };
        extend(mailclientNS, {
            AccountModel: AccountModel,
            FolderModel: FolderModel,
            AttachmentModel: AttachmentModel,
            MessageListItemModel: MessageListItemModel,
            MessageDetailsModel: MessageDetailsModel,
            ContactModel: ContactModel,
            Recipient: Recipient
        });
    })(window.kendo.jQuery);
    return window.kendo;
}, typeof define == "function" && define.amd ? define : function(a1, a2, a3) {
    "use strict";
    (a3 || a2)();
});

(function(f, define) {
    "use strict";
    define("kendo.mailclient.data", [ "kendo.mailclient.ns", "kendo.mailclient.utils", "kendo.mailclient.models" ], f);
})(function() {
    "use strict";
    (function($, undefined) {
        var kendo = window.kendo, ui = kendo.ui, mailclientNS = ui.mailclient, extend = $.extend, DataSource = kendo.data.DataSource, proxy = $.proxy, isArray = Array.isArray, slice = Array.prototype.slice, get = mailclientNS.get, last = mailclientNS.last, storage = mailclientNS.storage, selectField = mailclientNS.selectField, arrayIntersect = mailclientNS.arrayIntersect, traceError = mailclientNS.traceError, HttpCodes = mailclientNS.HttpCodes, AccountModel = mailclientNS.AccountModel, FolderModel = mailclientNS.FolderModel, MessageListItemModel = mailclientNS.MessageListItemModel, MessageDetailsModel = mailclientNS.MessageDetailsModel, ContactModel = mailclientNS.ContactModel, STORAGE_NS = "kendoMailClient";
        var BaseDataSource = DataSource.extend({
            init: function(auth, options) {
                DataSource.fn.init.call(this, options);
                this.transport.options.read.beforeSend = proxy(this._handleBeforeSend, this);
                this.auth = auth;
            },
            _handleBeforeSend: function(xhr) {
                if (this.auth && this.auth.enabled()) {
                    this.auth.addBearerHeader(xhr);
                }
            },
            read: function(data) {
                var that = this, deferred = $.Deferred();
                DataSource.fn.read.call(this, data).done(deferred.resolve).fail(function(xhr) {
                    if (that.auth && that.auth.enabled() && xhr && xhr.status === HttpCodes.UNAUTHORIZED) {
                        that._refreshAuthToken(deferred, data);
                    } else {
                        var args = slice.call(arguments);
                        deferred.reject.apply(deferred, args);
                    }
                });
                return deferred.promise();
            },
            _refreshAuthToken: function(targetDeferred, targetData) {
                var that = this;
                this.auth.refreshToken().done(function(token) {
                    try {
                        DataSource.fn.read.call(that, targetData).done(targetDeferred.resolve).fail(targetDeferred.reject);
                    } catch (err) {
                        traceError("Read data failed: " + err.toString());
                        targetDeferred.reject({}, "", "Read exception");
                    }
                    return token;
                }).fail(function(err) {
                    traceError("Refresh token failed: " + err.toString());
                    targetDeferred.reject(err.jqXHR, err.textStatus, err.errorThrown);
                    return err;
                });
            },
            inProgress: function() {
                return this._requestInProgress;
            }
        });
        var AccountDataSource = BaseDataSource.extend({
            init: function(auth, url) {
                BaseDataSource.fn.init.call(this, auth, {
                    transport: {
                        read: {
                            url: url,
                            dataType: "json"
                        }
                    },
                    schema: {
                        data: "Items",
                        total: "TotalCount",
                        model: AccountModel
                    }
                });
            }
        });
        var FolderListDataSource = BaseDataSource.extend({
            init: function(auth, url) {
                BaseDataSource.fn.init.call(this, auth, {
                    transport: {
                        read: {
                            url: proxy(this._makeUrl, this),
                            dataType: "json"
                        },
                        parameterMap: proxy(this._getParameterMap, this)
                    },
                    filter: {
                        field: "parentId",
                        operator: "eq",
                        value: null
                    },
                    sort: [ {
                        field: "sortOrder",
                        dir: "asc"
                    }, {
                        field: "displayName",
                        dir: "asc"
                    } ],
                    schema: {
                        data: proxy(this._processData, this),
                        model: FolderModel
                    }
                });
                this._urlTemplate = url;
            },
            _getParameterMap: function() {
                return null;
            },
            _makeUrl: function(parameters) {
                return kendo.format(this._urlTemplate, parameters.accountId);
            },
            _processData: function(response) {
                if (response && isArray(response)) {
                    var items = response.map(function(item) {
                        if (!item.ParentId) {
                            item.ParentId = null;
                        }
                        item.hasChildren = false;
                        item.expanded = false;
                        item.children = [];
                        return item;
                    });
                    items = this._createTree(items);
                    return items;
                }
                return [];
            },
            _createTree: function(nodes) {
                var that = this;
                return this._rootNodes(nodes).map(function(node) {
                    that._subtree(node, nodes);
                    return node;
                });
            },
            _rootNodes: function(nodes) {
                return nodes.filter(function(node) {
                    return !node.ParentId;
                });
            },
            _subtree: function(parent, nodeMap) {
                var result = [];
                for (var i = 0, len = nodeMap.length; i < len; i++) {
                    if (parent.FolderId === nodeMap[i].ParentId) {
                        this._subtree(nodeMap[i], nodeMap);
                        result.push(nodeMap[i]);
                    }
                }
                parent.children = this.createChildDataSource(result);
                parent.children.fetch();
                parent.hasChildren = result.length > 0;
            },
            flattenArray: function() {
                var subTreeArray = [];
                this._subTreeArray(subTreeArray, this.data());
                return subTreeArray;
            },
            flattenArrayBy: function(folder) {
                var subTreeArray = [];
                if (folder.hasChildren) {
                    this._subTreeArray(subTreeArray, folder.children.data());
                }
                return subTreeArray;
            },
            _subTreeArray: function(result, map) {
                for (var i = 0, len = map.length; i < len; i++) {
                    var node = map[i];
                    result.push(node);
                    if (node.hasChildren) {
                        this._subTreeArray(result, node.children.data());
                    }
                }
            },
            findParents: function(folderId, first) {
                var allItems = this.flattenArray();
                var result = [];
                var folder = allItems.filter(function(item) {
                    return item.id === folderId;
                });
                if (!folder.length) {
                    return [];
                }
                function getParents(parentId) {
                    for (var i = 0; i < allItems.length; i++) {
                        if (allItems[i].id === parentId) {
                            result.push(allItems[i]);
                            if (first) {
                                break;
                            }
                            if (allItems[i].parentId) {
                                getParents(allItems[i].parentId);
                            }
                        }
                    }
                }
                getParents(folder[0].parentId);
                return result;
            },
            createFolder: function(id, parentId, title) {
                var folder = FolderModel.create(id, parentId, title);
                folder.set("children", this.createChildDataSource());
                folder.children.fetch();
                return folder;
            },
            createChildDataSource: function(data) {
                return new DataSource({
                    data: data || [],
                    schema: {
                        model: FolderModel
                    },
                    sort: {
                        field: "displayName",
                        dir: "asc"
                    }
                });
            },
            maxSortOrder: function(ds) {
                ds = ds || this;
                var lastItem = last(ds.data());
                return lastItem ? lastItem.sortOrder || 0 : 0;
            },
            updateIdentifiers: function(model, rawData) {
                var idField = FolderModel.idField;
                model[idField] = rawData[idField];
                model.id = model.get(idField);
                model.parentId = rawData.ParentId || null;
            },
            updateSubtree: function(model, rawDataArray) {
                rawDataArray = rawDataArray || [];
                var node = {
                    FolderId: model.id
                };
                this._subtree(node, rawDataArray);
                model.setChildren(node.children);
                model.setHasChildren(node.hasChildren);
            }
        });
        var MessageListDataSource = BaseDataSource.extend({
            init: function(auth, url, pageSize) {
                BaseDataSource.fn.init.call(this, auth, {
                    transport: {
                        read: {
                            url: proxy(this._makeUrl, this),
                            dataType: "json"
                        },
                        parameterMap: proxy(this._getParameterMap, this)
                    },
                    schema: {
                        data: "Items",
                        total: "TotalCount",
                        errors: "Error",
                        model: MessageListItemModel
                    },
                    serverPaging: true,
                    serverFiltering: true,
                    pageSize: pageSize
                });
                this._urlTemplate = url;
                this.accountId = null;
                this.folderId = null;
            },
            _getParameterMap: function(opt, operation) {
                if (operation === "read") {
                    this._makeSearchOpt(opt);
                    this._makeIsFlagOpt(opt);
                    this._makeIsReadOpt(opt);
                    this._makeHasAttachmentsOpt(opt);
                    this._makeSortOpt(opt);
                    this._makeSortDirectionOpt(opt);
                    delete opt.pageSize;
                    delete opt.page;
                    delete opt.filter;
                }
                return opt;
            },
            _makeUrl: function() {
                return kendo.format(this._urlTemplate, this.accountId, this.folderId);
            },
            _makeSearchOpt: function(opt) {
                var filter = this._findFilter(opt.filter, "search");
                if (filter) {
                    opt.search = filter.value;
                }
            },
            _makeIsFlagOpt: function(opt) {
                var filter = this._findFilter(opt.filter, "is_flag");
                if (filter) {
                    opt.is_flag = filter.value;
                }
            },
            _makeIsReadOpt: function(opt) {
                var filter = this._findFilter(opt.filter, "is_read");
                if (filter) {
                    opt.is_read = filter.value;
                }
            },
            _makeHasAttachmentsOpt: function(opt) {
                var filter = this._findFilter(opt.filter, "has_attachments");
                if (filter) {
                    opt.has_attachments = filter.value;
                }
            },
            _makeSortOpt: function(opt) {
                var filter = this._findFilter(opt.filter, "order");
                if (filter) {
                    opt.order = filter.value;
                }
            },
            _makeSortDirectionOpt: function(opt) {
                var filter = this._findFilter(opt.filter, "desc");
                if (filter) {
                    opt.desc = filter.value;
                }
            },
            _findFilter: function(filter, field) {
                if (filter && isArray(filter.filters) && field) {
                    var result = filter.filters.filter(function(item) {
                        return item.field === field;
                    });
                    return result.length > 0 ? result[0] : null;
                }
                return null;
            }
        });
        var MessageDetailsDataSource = BaseDataSource.extend({
            init: function(auth, url) {
                BaseDataSource.fn.init.call(this, auth, {
                    transport: {
                        read: {
                            url: proxy(this._makeUrl, this),
                            dataType: "json"
                        },
                        parameterMap: proxy(this._getParameterMap, this)
                    },
                    schema: {
                        data: function(response) {
                            return isArray(response) ? [ response[0] ] : [ response ];
                        },
                        errors: "Error",
                        model: MessageDetailsModel
                    }
                });
                this._urlTemplate = url;
            },
            _getParameterMap: function() {
                return null;
            },
            _makeUrl: function(parameters) {
                return kendo.format(this._urlTemplate, parameters.accountId, parameters.folderId, parameters.messageId);
            }
        });
        var LocalContactListDataSource = DataSource.extend({
            init: function(data) {
                DataSource.fn.init.call(this, {
                    data: data,
                    schema: {
                        model: ContactModel
                    }
                });
            }
        });
        var ContactListDataSource = DataSource.extend({
            init: function(url, withCredentials) {
                var that = this;
                DataSource.fn.init.call(this, {
                    transport: {
                        read: function(e) {
                            var filterValue = that._getFilterValue(e.data);
                            e.success(that._concatWithLocalData([], filterValue));
                            that._fetchServerData(filterValue);
                        },
                        create: function(e) {
                            e.data[ContactModel.idField] = kendo.guid();
                            that.addToLocalStorage([ e.data ]);
                            e.success(e.data);
                        }
                    },
                    schema: {
                        model: ContactModel
                    },
                    serverFiltering: true
                });
                this._urlTemplate = url;
                this._storage = storage(STORAGE_NS);
                this._withCredentials = withCredentials;
                this._additionalDataXhr = {
                    xhr: null
                };
                this.recipientCache = "recipientCache";
                this.uniqModelField = "Email";
                this.titleModelField = "Title";
                this.maxCacheItems = 50;
            },
            _fetchServerData: function(filterValue) {
                var that = this;
                if (this._additionalDataXhr && this._additionalDataXhr.xhr) {
                    this._additionalDataXhr.xhr.abort();
                    this._additionalDataXhr.xhr = null;
                }
                that.trigger("dataFetching");
                get(that._makeUrl(filterValue), null, null, this._withCredentials, this._additionalDataXhr).done(function(reg) {
                    var localDs = new LocalContactListDataSource(reg.response || []);
                    localDs.fetch();
                    var items = localDs.data().map(function(item) {
                        return item;
                    });
                    var oldItems = that.data().map(function(item) {
                        return item;
                    });
                    that.data(oldItems.concat(items));
                    that.trigger("dataFetchSuccess");
                }).fail(function(error) {
                    that.trigger("dataFetchError");
                    if (error.textStatus !== "abort") {
                        traceError("Error request: " + error.textStatus + " " + error.errorThrown);
                    }
                });
            },
            _makeUrl: function(filterValue) {
                return kendo.format(this._urlTemplate, encodeURIComponent(filterValue || ""));
            },
            _concatWithLocalData: function(array, filterValue) {
                array = array || [];
                var savedItems = this._fetchLocalItems();
                if (filterValue) {
                    var filteredItems = this._filterRawData(savedItems, filterValue);
                    array = array.concat(this._removeDuplicates(filteredItems, array));
                } else {
                    array = array.concat(this._removeDuplicates(savedItems, array));
                }
                return array;
            },
            _removeDuplicates: function(sourceArray, compareArray) {
                var sourceKeys, compareKeys, key = this.uniqModelField;
                sourceKeys = sourceArray.map(selectField(key));
                compareKeys = compareArray.map(selectField(key));
                var duplicates = arrayIntersect(sourceKeys, compareKeys);
                if (duplicates.length > 0) {
                    return sourceArray.filter(function(item) {
                        return duplicates.indexOf(item[key]) === -1;
                    });
                }
                return sourceArray;
            },
            _fetchLocalItems: function() {
                this._storage.open();
                var savedItems = this._storage.get(this.recipientCache) || [];
                this._storage.close();
                return savedItems;
            },
            _filterRawData: function(data, value) {
                data = data || [];
                var query = kendo.data.Query.process(data, {
                    filter: {
                        logic: "or",
                        filters: [ {
                            field: this.uniqModelField,
                            value: value,
                            operator: "contains"
                        }, {
                            field: this.titleModelField,
                            value: value,
                            operator: "contains"
                        } ]
                    }
                });
                return query.data;
            },
            _getFilterValue: function(options) {
                var filter = options.filter, filters = filter ? filter.filters : null;
                return filters && filters[0] ? filters[0].value || "" : "";
            },
            addToLocalStorage: function(items) {
                this._storage.open();
                var that = this, savedItems = this._storage.get(this.recipientCache) || [], insertItems = savedItems.slice(), uniqField = that.uniqModelField;
                var findItem = function(value) {
                    var items = savedItems.filter(function(item) {
                        return item[uniqField] === value;
                    });
                    return items.length > 0 ? items[0] : null;
                };
                (items || []).forEach(function(item) {
                    var itemSaved = findItem(item[uniqField]);
                    if (!itemSaved) {
                        if (insertItems.length >= that.maxCacheItems) {
                            insertItems.pop();
                        }
                        insertItems.unshift(item);
                    }
                });
                this._storage.set(this.recipientCache, insertItems);
                this._storage.save(true);
            },
            createRawItems: function(data) {
                data = data || [];
                var model = this.reader.model, rawItems = [];
                if (!model) {
                    return rawItems;
                }
                var fields = model.fields;
                data.forEach(function(item) {
                    var rawItem = {};
                    Object.keys(item).forEach(function(prop) {
                        var fieldSpec = fields[prop];
                        if (fieldSpec && fieldSpec.field) {
                            rawItem[fieldSpec.field] = item[prop];
                        }
                    });
                    rawItems.push(rawItem);
                });
                return rawItems;
            }
        });
        extend(mailclientNS, {
            AccountDataSource: AccountDataSource,
            FolderListDataSource: FolderListDataSource,
            MessageListDataSource: MessageListDataSource,
            MessageDetailsDataSource: MessageDetailsDataSource,
            ContactListDataSource: ContactListDataSource
        });
    })(window.kendo.jQuery);
    return window.kendo;
}, typeof define == "function" && define.amd ? define : function(a1, a2, a3) {
    "use strict";
    (a3 || a2)();
});

(function(f, define) {
    "use strict";
    define("kendo.mailclient.auth", [ "kendo.mailclient.ns", "kendo.mailclient.utils" ], f);
})(function() {
    "use strict";
    (function($, undefined) {
        var kendo = window.kendo, ui = kendo.ui, Class = kendo.Class, mailclientNS = ui.mailclient, request = mailclientNS.request, HttpCodes = mailclientNS.HttpCodes, extend = $.extend;
        var Authenticator = Class.extend({
            init: function(mailclient) {
                this.mailclient = mailclient;
                this.options = mailclient.options;
                this.token = null;
                this._enabled = this._checkTokenOpt();
                this._refreshTokenDefer = null;
            },
            _checkTokenOpt: function() {
                var opt = this.options, tokenOpt = opt.auth ? opt.auth.token : null, optTokenValid = tokenOpt && tokenOpt.method && tokenOpt.field;
                return !!(tokenOpt.createUrl && optTokenValid);
            },
            enabled: function(value) {
                if (!arguments.length) {
                    return this._enabled;
                }
                this._enabled = !!value;
            },
            getToken: function() {
                var that = this, deferred = $.Deferred(), opt = this.options, tokenOpt = opt.auth.token, mailclient = this.mailclient, url = tokenOpt.createUrl, method = tokenOpt.method, withCredentials = tokenOpt.withCredentials;
                mailclient.showProgress();
                request(method, url, null, null, withCredentials).done(function(req) {
                    var response = req.response, token = response[tokenOpt.field];
                    mailclient.hideProgress();
                    if (token) {
                        that.token = token;
                        deferred.resolve(token);
                    } else {
                        var error = new Error(kendo.format("Invalid auth token: {0}", token));
                        mailclient.showError(error.toString());
                        deferred.reject(error);
                    }
                }).fail(function(error) {
                    mailclient.hideProgress();
                    mailclient.showError(error.toString());
                    deferred.reject(error);
                });
                return deferred;
            },
            refreshToken: function() {
                var that = this;
                if (!this._refreshTokenDefer) {
                    this._refreshTokenDefer = this.getToken().done(function(token) {
                        that._refreshTokenDefer = null;
                        return token;
                    }).fail(function(e) {
                        that._refreshTokenDefer = null;
                        return e;
                    });
                }
                return this._refreshTokenDefer;
            },
            validateOrRefreshToken: function(token) {
                var that = this, tokenValidationOpt = this.options.auth.tokenValidation, url = tokenValidationOpt.readUrl, method = tokenValidationOpt.method, withCredentials = tokenValidationOpt.withCredentials, deferred = $.Deferred();
                if (tokenValidationOpt.readUrl) {
                    request(method, url, null, token || this.token, withCredentials).done(deferred.resolve).fail(function(err) {
                        if (err.jqXHR && err.jqXHR.status === HttpCodes.UNAUTHORIZED) {
                            that.refreshToken().done(deferred.resolve).fail(deferred.reject);
                        } else {
                            deferred.reject();
                        }
                    });
                } else {
                    deferred.resolve();
                }
                return deferred.promise();
            },
            addBearerHeader: function(xhr) {
                if (this._enabled && this.token) {
                    xhr.setRequestHeader("Authorization", "Bearer " + this.token);
                }
            }
        });
        extend(mailclientNS, {
            Authenticator: Authenticator
        });
    })(window.kendo.jQuery);
    return window.kendo;
}, typeof define == "function" && define.amd ? define : function(a1, a2, a3) {
    "use strict";
    (a3 || a2)();
});

(function(f, define) {
    "use strict";
    define("kendo.mailclient.events", [ "kendo.mailclient.ns", "kendo.mailclient.utils" ], f);
})(function() {
    "use strict";
    (function($, undefined) {
        var kendo = window.kendo, ui = kendo.ui, extend = $.extend, isEmptyObject = $.isEmptyObject, proxy = $.proxy, isArray = Array.isArray, Class = kendo.Class, mailclientNS = ui.mailclient, timer = mailclientNS.timer, traceError = mailclientNS.traceError, BroadcastEvents = mailclientNS.BroadcastEvents, FolderModel = mailclientNS.FolderModel;
        var EventPoll = Class.extend({
            init: function(mailclient, options) {
                this.mailclient = mailclient;
                this.options = options;
                this._timer = timer(options.pollingInterval, proxy(this._sendRequest, this));
                this._requestDone = proxy(this._requestDone, this);
                this._requestFail = proxy(this._requestFail, this);
                this._requestDefer = null;
                this._running = false;
            },
            start: function() {
                if (!this._running) {
                    this._running = true;
                    this._timer.start();
                }
            },
            stop: function() {
                if (this._running) {
                    this._running = false;
                    this._timer.cancel();
                }
            },
            _sendRequest: function() {
                if (!this._requestDefer) {
                    var service = this.mailclient.service;
                    this._requestDefer = service.getFolders().done(this._requestDone).fail(this._requestFail);
                }
            },
            _requestDone: function(request) {
                this._requestDefer = null;
                var events = this._fetchEvents(request.response);
                if (events.length > 0) {
                    try {
                        this._broadcast(events);
                    } catch (err) {
                        traceError("EventPoll: Broadcast events error: " + err.toString());
                    }
                }
                if (this._running) {
                    this._timer.restart();
                }
            },
            _requestFail: function(error) {
                this._requestDefer = null;
                traceError("EventPoll: Request failed: " + error.toString());
                if (this._running) {
                    this._timer.restart();
                }
            },
            _broadcast: function(events) {
                var mailclient = this.mailclient;
                events.forEach(function(event) {
                    mailclient.broadcast(event.type, event);
                });
            },
            _fetchEvents: function(rawData) {
                if (!rawData || !isArray(rawData) || rawData.length === 0) {
                    return [];
                }
                var result = [], folders = this.mailclient.folderList.flattenAll(), fetchedFolders = this._getFoldersFromRawData(rawData), fetchedFoldersById = this._getModelsById(fetchedFolders);
                for (var i = 0, c = folders.length; i < c; i++) {
                    var folder = folders[i];
                    var fetchedFolder = fetchedFoldersById[folder.id];
                    if (fetchedFolder) {
                        var changedFields = this._getChangedFields(folder, fetchedFolder, [ "unreadItemCount", "totalItemCount" ]);
                        if (!isEmptyObject(changedFields)) {
                            result.push({
                                type: BroadcastEvents.CHANGE_FOLDER,
                                folderId: folder.id,
                                changedFields: changedFields
                            });
                        }
                    }
                }
                return result;
            },
            _getChangedFields: function(oldState, newState, checkFields) {
                var result = {}, field, i, c;
                for (i = 0, c = checkFields.length; i < c; i++) {
                    field = checkFields[i];
                    if (oldState[field] !== newState[field]) {
                        result[field] = newState[field];
                    }
                }
                return result;
            },
            _getModelsById: function(models) {
                var byId = {};
                models.forEach(function(model) {
                    byId[model.id] = model;
                });
                return byId;
            },
            _getFoldersFromRawData: function(rawData) {
                return rawData.map(function(item) {
                    return FolderModel.from(item);
                });
            }
        });
        var EventManager = Class.extend({
            init: function(mailclient) {
                this.mailclient = mailclient;
                this._initEventProvider();
            },
            _initEventProvider: function() {
                var mailclient = this.mailclient, eventsOpt = mailclient.options.events;
                if (eventsOpt.polling) {
                    this.eventProvider = new EventPoll(mailclient, eventsOpt);
                }
            },
            start: function() {
                if (this.eventProvider) {
                    this.eventProvider.start();
                }
            },
            stop: function() {
                if (this.eventProvider) {
                    this.eventProvider.stop();
                }
            }
        });
        extend(mailclientNS, {
            EventManager: EventManager,
            EventPoll: EventPoll
        });
    })(window.kendo.jQuery);
    return window.kendo;
}, typeof define == "function" && define.amd ? define : function(a1, a2, a3) {
    "use strict";
    (a3 || a2)();
});

(function(f, define) {
    "use strict";
    define("kendo.mailclient.commandutil", [ "kendo.mailclient.ns", "kendo.mailclient.utils" ], f);
})(function() {
    "use strict";
    (function($, undefined) {
        var kendo = window.kendo, ui = kendo.ui, Class = kendo.Class, mailclientNS = ui.mailclient, compareObjects = mailclientNS.compareObjects, first = mailclientNS.first, isFunction = kendo.isFunction, extend = $.extend;
        var RunningCommands = Class.extend({
            init: function() {
                this._commands = [];
            },
            add: function(command) {
                if (!this.exists(command)) {
                    this._commands.push(command);
                }
            },
            remove: function(command) {
                var idx = this._commands.indexOf(command);
                if (idx !== -1) {
                    this._commands.splice(idx, 1);
                }
            },
            exists: function(command) {
                return this._commands.indexOf(command) !== -1;
            },
            existsBy: function(type, args) {
                return this.findBy(type, args) !== null;
            },
            findBy: function(type, args) {
                var result = this._commands.filter(function(command) {
                    return command.type === type && compareObjects(args, command.options, true);
                });
                return first(result);
            },
            findByType: function(type, criteria) {
                var result = this._commands.filter(function(command) {
                    return !isFunction(criteria) && command.type === type || command.type === type && isFunction(criteria) && criteria(command);
                });
                return first(result);
            },
            find: function(command) {
                var idx = this._commands.indexOf(command);
                if (idx !== -1) {
                    return this._commands[idx];
                }
                return null;
            }
        });
        function registerCommand(name, constructor) {
            if (!registerCommand.registered) {
                registerCommand.registered = {};
            }
            registerCommand.registered[name] = constructor;
        }
        function makeCommandFactory(mailclient, runningCommands) {
            function createCommand(commandName, commandArgs) {
                if (registerCommand.registered[commandName] === undefined) {
                    throw new Error("Could not found command: " + commandName);
                }
                var commandConstructor = registerCommand.registered[commandName];
                if (typeof commandConstructor !== "function") {
                    throw new Error("Could not found command constructor: " + commandName);
                }
                var command = new commandConstructor($.extend({}, commandArgs, {
                    mailclient: mailclient,
                    widgetOpt: mailclient.options,
                    commandType: commandName,
                    runningCommands: runningCommands
                }));
                return command;
            }
            return createCommand;
        }
        extend(mailclientNS, {
            RunningCommands: RunningCommands,
            makeCommandFactory: makeCommandFactory,
            registerCommand: registerCommand
        });
    })(window.kendo.jQuery);
    return window.kendo;
}, typeof define == "function" && define.amd ? define : function(a1, a2, a3) {
    "use strict";
    (a3 || a2)();
});

(function(f, define) {
    "use strict";
    define("kendo.mailclient.view-models", [ "kendo.mailclient.ns", "kendo.mailclient.utils", "kendo.mailclient.constants", "kendo.mailclient.data", "kendo.mailclient.models", "kendo.mailclient.auth", "kendo.mailclient.events", "kendo.mailclient.commandutil" ], f);
})(function() {
    "use strict";
    (function($, undefined) {
        var kendo = window.kendo, ui = kendo.ui, mailclientNS = ui.mailclient, extend = $.extend, DataSource = kendo.data.DataSource, ObservableObject = kendo.data.ObservableObject, proxy = $.proxy, isEmptyObject = $.isEmptyObject, keys = kendo.keys, isFunction = kendo.isFunction, FolderType = mailclientNS.FolderType, Commands = mailclientNS.Commands, FolderTranslateMap = mailclientNS.FolderTranslateMap, BroadcastEvents = mailclientNS.BroadcastEvents, HttpCodes = mailclientNS.HttpCodes, isArray = Array.isArray, arraySlice = Array.prototype.slice, storage = mailclientNS.storage, arrayCount = mailclientNS.arrayCount, first = mailclientNS.first, timer = mailclientNS.timer, traceError = mailclientNS.traceError, initials = mailclientNS.initials, fileExtension = mailclientNS.fileExtension, confirm = mailclientNS.confirm, getNoun = mailclientNS.getNoun, get = mailclientNS.get, post = mailclientNS.post, put = mailclientNS.put, del = mailclientNS.del, notNullFilter = mailclientNS.notNullFilter, filterByField = mailclientNS.filterByField, selectField = mailclientNS.selectField, findByRef = mailclientNS.findByRef, bindingTargetSource = mailclientNS.bindingTargetSource, makeCommandFactory = mailclientNS.makeCommandFactory, AccountDataSource = mailclientNS.AccountDataSource, FolderListDataSource = mailclientNS.FolderListDataSource, MessageListDataSource = mailclientNS.MessageListDataSource, MessageDetailsDataSource = mailclientNS.MessageDetailsDataSource, ContactListDataSource = mailclientNS.ContactListDataSource, Authenticator = mailclientNS.Authenticator, EventManager = mailclientNS.EventManager, RunningCommands = mailclientNS.RunningCommands, STORAGE_NS = "kendoMailClient", CURRENT_ACCOUNT_KEY = "currentAccount", SETTINGS_KEY = "settings", LISTVIEW_SELECT_ITEM = "listViewSelectItem", LISTVIEW_SELECT_ELEMENT = "listViewSelectElement", LISTVIEW_CLEAR_SELECTION = "listViewClearSelection", FOLDER = "folder", MESSAGE = "message", CHANGE = "change", CLOSE = "close", SHOW_ERROR = "showError", HIDE_ERROR = "hideError", DOCUMENT_CLICK = "documentClick", EDIT_COMPLETE = "editComplete", EDITOR_WIDGET = "kendoEditor", EDITOR_SELECTOR = "[data-role='editor']", UPLOAD_WIDGET = "kendoMailClientUpload", UPLOAD_SELECTOR = "[data-role='mailclientupload']", OP_UPLOAD = "upload", OP_REMOVE = "remove", QUEUE_PROCESSING = "queueProcessing", QUEUE_COMPLETE = "queueComplete", REMOVE_MESSAGE = "removeMessage", UPDATE_MESSAGE = "updateMessage", REQUEST_START = "requestStart", REQUEST_END = "requestEnd", REQUEST_ERROR = "error";
        var BaseViewModel = ObservableObject.extend({
            init: function() {
                ObservableObject.fn.init.call(this);
            },
            _initDataSource: function(ds) {
                ds.bind(REQUEST_START, this._onRequestStart.bind(this));
                ds.bind(REQUEST_END, this._onRequestEnd.bind(this));
                ds.bind(REQUEST_ERROR, this._onRequestError.bind(this));
            },
            _onRequestStart: function() {
                this.parent().showProgress();
            },
            _onRequestEnd: function() {
                this.parent().hideProgress();
            },
            _onRequestError: function(e) {
                if (this._isUnauthorized(e.xhr)) {
                    traceError(this._makeErrorMessage(e));
                } else {
                    this.parent().showError(this._makeErrorMessage(e));
                }
            },
            _isUnauthorized: function(xhr) {
                return xhr && xhr.status === HttpCodes.UNAUTHORIZED;
            },
            _makeErrorMessage: function(e) {
                var message = "Request error: ";
                message += e.errorThrown + " ";
                message += e.status + " ";
                message += e.xhr.statusText;
                return message;
            },
            getFirst: function(ds) {
                return first(ds.view());
            }
        });
        var Broadcaster = ObservableObject.extend({
            init: function() {
                ObservableObject.fn.init.call(this);
            }
        });
        var AccountViewModel = BaseViewModel.extend({
            init: function(auth, options) {
                BaseViewModel.fn.init.call(this);
                var opt = options.account;
                this.onChange = proxy(this.onChange, this);
                this._storage = storage(STORAGE_NS);
                this.set("selectedItem", null);
                this.set("dataSource", new AccountDataSource(auth, opt.readUrl));
                this._initDataSource(this.dataSource);
            },
            fetch: function() {
                return this.dataSource.fetch();
            },
            getFirst: function() {
                return BaseViewModel.fn.getFirst.call(this, this.dataSource);
            },
            getCurrentId: function() {
                return this.selectedItem;
            },
            setCurrentId: function(accountId) {
                this.set("selectedItem", accountId);
                this._storage.set(CURRENT_ACCOUNT_KEY, accountId);
                this._storage.save();
            },
            getCurrent: function() {
                return this.selectedItem ? this.dataSource.get(this.selectedItem) : null;
            },
            switchAccount: function(accountId) {
                this.parent().folderList.reload(accountId);
            },
            switchToCurrent: function() {
                var current = this.getCurrent();
                if (!current) {
                    current = this.getCurrentFromStorage();
                    if (current) {
                        this.setCurrentId(current.id);
                    }
                }
                if (!current) {
                    current = this.getFirst();
                    if (current) {
                        this.setCurrentId(current.id);
                    }
                }
                if (current) {
                    this.switchAccount(current.id);
                }
            },
            getCurrentFromStorage: function() {
                var currentId = this._storage.get(CURRENT_ACCOUNT_KEY) || null;
                return currentId ? this.dataSource.get(currentId) : null;
            },
            onChange: function() {
                this.setCurrentId(this.selectedItem);
                this.switchAccount(this.selectedItem);
            }
        });
        var FolderListViewModel = BaseViewModel.extend({
            init: function(auth, broadcaster, options) {
                BaseViewModel.fn.init.call(this);
                var opt = options.folder;
                this._options = options;
                this.set("dataSource", new FolderListDataSource(auth, opt.readUrl));
                this._selectedItem = null;
                this._currentEdit = null;
                this._currentEditHandler = null;
                this._storage = storage(STORAGE_NS);
                this.EXPAND_STORAGE_KEY = "FolderExpand";
                this._dragExpanded = [];
                this._autoCollapseTimeout = 2e3;
                this._autoCollapseTimer = timer(this._autoCollapseTimeout, proxy(this._onAutoCollapse, this));
                this.onChange = proxy(this.onChange, this);
                this.toggleExpand = proxy(this.toggleExpand, this);
                this.onExpanderEnter = proxy(this.onExpanderEnter, this);
                this.onFolderBarLeave = proxy(this.onFolderBarLeave, this);
                this.onFolderBarEnter = proxy(this.onFolderBarEnter, this);
                this.onFolderBarDrop = proxy(this.onFolderBarDrop, this);
                this.onDragHint = proxy(this.onDragHint, this);
                this.onDrop = proxy(this.onDrop, this);
                this.onDragLeave = proxy(this.onDragLeave, this);
                this.onDragEnter = proxy(this.onDragEnter, this);
                this.onDragStart = proxy(this.onDragStart, this);
                this.onContextCommand = proxy(this.onContextCommand, this);
                this.onOpenContextMenu = proxy(this.onOpenContextMenu, this);
                this.onBroadcastChangeFolder = proxy(this.onBroadcastChangeFolder, this);
                broadcaster.bind(BroadcastEvents.CHANGE_FOLDER, this.onBroadcastChangeFolder);
                this._initDataSource(this.dataSource);
                this.dataSource.bind(CHANGE, proxy(this._updateExpandFromStorage, this));
            },
            reload: function(accountId) {
                var that = this;
                this.reset();
                this.parent().messageList.clearSelection();
                that.fetch(accountId).then(function() {
                    that.switchToCurrent(accountId);
                });
            },
            fetch: function(accountId) {
                return this.dataSource.read({
                    accountId: accountId
                });
            },
            find: function(folderId) {
                if (typeof this.dataSource.flattenArray === "function") {
                    var res = this.dataSource.flattenArray().filter(filterByField("id", folderId));
                    return first(res);
                }
                return this.dataSource.get(folderId);
            },
            findByType: function(folderType) {
                var data;
                if (typeof this.dataSource.flattenArray === "function") {
                    data = this.dataSource.flattenArray();
                } else {
                    data = this.dataSource.data();
                }
                var res = data.filter(filterByField("type", folderType));
                return first(res);
            },
            findTopFolder: function(folder) {
                var parents = this.getParents(folder);
                return parents.length > 0 ? parents[parents.length - 1] : folder;
            },
            flattenAll: function() {
                if (typeof this.dataSource.flattenArray === "function") {
                    return this.dataSource.flattenArray();
                }
                return this.dataSource.data();
            },
            getParents: function(folder, first) {
                return this.dataSource.findParents(folder.id, first);
            },
            getParent: function(folder) {
                var parents = this.getParents(folder, true);
                return first(parents);
            },
            getParentsAndSelf: function(folder) {
                return [ folder ].concat(this.getParents(folder));
            },
            getFirst: function() {
                return BaseViewModel.fn.getFirst.call(this, this.dataSource);
            },
            getCurrent: function() {
                return this._selectedItem;
            },
            isCurrent: function(folderId) {
                var current = this.getCurrent();
                return current ? current.id === folderId : false;
            },
            sort: function(ds) {
                if (ds instanceof DataSource) {
                    ds.sort(ds.options.sort);
                }
            },
            findDataSource: function(folder) {
                var parent = this.getParent(folder);
                return parent ? parent.children : this.dataSource;
            },
            isRootDataSource: function(ds) {
                return ds === this.dataSource;
            },
            isFolder: function(model) {
                var ctor = this.dataSource.reader.model;
                return model && ctor && model instanceof ctor;
            },
            switchFolder: function(accountId, folderId) {
                if (accountId && folderId) {
                    this.parent().messageList.reload(accountId, folderId);
                }
            },
            switchToCurrent: function(accountId) {
                var current = this.getCurrent();
                if (!current) {
                    current = this.getFirst();
                    if (current) {
                        this._updateSelection(current);
                    }
                }
                if (current) {
                    this.switchFolder(accountId, current.id);
                }
            },
            folderName: function(folder) {
                var translateName = FolderTranslateMap[folder.type], localization = this._options.localization, displayName = folder.get("displayName");
                return translateName && localization.folders[translateName] ? localization.folders[translateName] : displayName;
            },
            moveToList: function() {
                return this.dataSource.view().filter(function(item) {
                    return item.type !== FolderType.Trash;
                });
            },
            totalUnreadItemCount: function() {
                return this.flattenAll().reduce(function(prev, curr) {
                    return prev + curr.unreadItemCount;
                }, 0);
            },
            isTrashFolder: function() {
                var folder = this.parent().folderList.getCurrent();
                return folder && folder.type === FolderType.Trash;
            },
            isSpamFolder: function() {
                var folder = this.parent().folderList.getCurrent();
                return folder && folder.type === FolderType.Junk;
            },
            isSentFolder: function() {
                var folder = this.parent().folderList.getCurrent();
                return folder && folder.type === FolderType.Sent;
            },
            _updateSelection: function(folder) {
                this._selectedItem = folder;
                this.parent().trigger(LISTVIEW_SELECT_ITEM, {
                    type: FOLDER,
                    item: folder
                });
            },
            reselect: function() {
                if (this._selectedItem) {
                    var newSelectedItem = this.find(this._selectedItem.id);
                    if (newSelectedItem) {
                        this._updateSelection(newSelectedItem);
                    }
                    this._selectedItem = newSelectedItem;
                }
            },
            clearSelection: function() {
                this._selectedItem = null;
                this.parent().trigger(LISTVIEW_CLEAR_SELECTION, {
                    type: FOLDER
                });
            },
            isSubtreeOrSelfSelected: function(folder) {
                if (!this._selectedItem) {
                    return false;
                }
                if (this._selectedItem.id === folder.id) {
                    return true;
                }
                var parents = this.getParents(this._selectedItem);
                var result = parents.filter(filterByField("id", folder.id));
                return result.length > 0;
            },
            toggleExpand: function(e) {
                e.stopImmediatePropagation();
                e.data.toggleExpanded();
                this._saveExpand(e.data);
                this._removeFromDragExpanded(e.data);
            },
            reset: function() {
                this._selectedItem = null;
                this._currentEdit = null;
                this._currentEditHandler = null;
                this._dragExpanded = [];
                this._autoCollapseTimer.cancel();
            },
            onBroadcastChangeFolder: function(e) {
                var changedFields = e.changedFields, folder = this.find(e.folderId);
                if (folder) {
                    Object.keys(changedFields).forEach(function(key) {
                        folder.set(key, changedFields[key]);
                    });
                }
            },
            onChange: function(e) {
                var previousItem = this._selectedItem, selectedItem = e.sender.dataItem(e.sender.select()), accountId = this.parent().account.getCurrentId(), changed = selectedItem && !previousItem || selectedItem && previousItem && selectedItem.id !== previousItem.id;
                this._selectedItem = selectedItem || null;
                if (selectedItem && changed && accountId) {
                    this.parent().service.broadcast(BroadcastEvents.SELECT_FOLDER, {
                        prev: previousItem,
                        selected: selectedItem
                    });
                    this.switchFolder(accountId, selectedItem.id);
                    this._removeArrayFromDragExpanded(this.getParentsAndSelf(selectedItem));
                }
            },
            onFolderItemClick: function(e) {
                e.stopImmediatePropagation();
                this.parent().trigger(LISTVIEW_SELECT_ELEMENT, {
                    type: FOLDER,
                    item: e.currentTarget
                });
            },
            onExpanderEnter: function(e) {
                this.cancelAutoCollapse();
                var folder = bindingTargetSource(e.dropTarget[0]);
                if (folder && !folder.expanded) {
                    folder.setExpand(true);
                    this._addToDragExpanded(folder);
                }
            },
            onFolderBarLeave: function() {
                this.autoCollapse();
            },
            onFolderBarEnter: function() {
                this.cancelAutoCollapse();
            },
            onFolderBarDrop: function(e) {
                var dragTarget = e.draggable.currentTarget, folderModel = dragTarget ? this._contextModel(dragTarget[0]) : null;
                if (folderModel) {
                    this.onDrop(e);
                }
                this.autoCollapse();
            },
            onDragHint: function(e) {
                var target = e.target ? e.target[0] : null, folder = target ? bindingTargetSource(target) : null;
                e.hintData.title = folder ? this.folderName(folder) : "";
            },
            onDrop: function(e) {
                var that = this, ev = e.origin || e, destFolder = this._contextModel(ev.dropTarget[0]), dragElement = ev.draggable ? ev.draggable.currentTarget : null, srcFolder = dragElement && dragElement.length ? this._contextModel(dragElement[0]) : null, destFolderId = destFolder ? destFolder.id : null, srcFolderId = srcFolder ? srcFolder.id : null, oldParent = srcFolder ? this.getParent(srcFolder) : null;
                if (srcFolder) {
                    var allowChangeParent = srcFolder.parentId !== destFolderId && srcFolder.id !== destFolderId;
                    if (allowChangeParent) {
                        var command = this.parent().command(Commands.UpdateFolder, {
                            folderId: srcFolder.id,
                            parentId: destFolderId
                        });
                        command.exec().done(function() {
                            if (srcFolder && destFolder && oldParent) {
                                that.changeParent(srcFolder, destFolder, oldParent);
                            }
                            if (srcFolderId && srcFolder) {
                                that._updateExpandFolderId(srcFolderId, srcFolder.id);
                            }
                        });
                    }
                }
            },
            onDragLeave: function() {
                this.autoCollapse();
            },
            onDragEnter: function() {
                this.cancelAutoCollapse();
            },
            onDragStart: function() {
                this.completeEdit();
            },
            autoCollapse: function() {
                this._autoCollapseTimer.restart();
            },
            cancelAutoCollapse: function() {
                this._autoCollapseTimer.cancel();
            },
            _onAutoCollapse: function() {
                this._dragExpanded.forEach(function(folder) {
                    if (folder.expanded) {
                        folder.setExpand(false);
                    }
                });
                this._dragExpanded = [];
            },
            _addToDragExpanded: function(folder) {
                this._dragExpanded.push(folder);
            },
            _removeArrayFromDragExpanded: function(folders) {
                var that = this;
                folders.forEach(function(folder) {
                    that._removeFromDragExpanded(folder);
                });
            },
            _removeFromDragExpanded: function(folder) {
                var index = this._dragExpanded.indexOf(folder);
                if (index > -1) {
                    this._dragExpanded.splice(index, 1);
                }
            },
            _updateExpandFromStorage: function(e) {
                if (!e.action) {
                    this._storage.open();
                    var savedItems = this._storage.get(this.EXPAND_STORAGE_KEY);
                    if (savedItems) {
                        this.flattenAll().forEach(function(item) {
                            if (savedItems[item.id]) {
                                item.setExpand(true);
                            }
                        });
                    }
                    this._storage.close();
                }
            },
            _saveExpand: function(folder) {
                this._storage.open();
                var savedItems = this._storage.get(this.EXPAND_STORAGE_KEY) || {};
                if (folder.expanded) {
                    savedItems[folder.id] = folder.expanded;
                } else {
                    delete savedItems[folder.id];
                }
                this._storage.set(this.EXPAND_STORAGE_KEY, savedItems);
                this._storage.save(true);
            },
            _updateExpandFolderId: function(oldFolderId, newFolderId) {
                this._storage.open();
                var savedItems = this._storage.get(this.EXPAND_STORAGE_KEY) || {};
                if (savedItems[oldFolderId]) {
                    savedItems[newFolderId] = savedItems[oldFolderId];
                    delete savedItems[oldFolderId];
                }
                this._storage.set(this.EXPAND_STORAGE_KEY, savedItems);
                this._storage.save(true);
            },
            enableEdit: function(folder, onComplete) {
                if (this.isCurrentEdit(folder)) {
                    return;
                }
                if (this._currentEdit && this._currentEdit !== folder) {
                    this.completeEdit();
                }
                this._currentEdit = folder;
                this._currentEdit.setEdit(true);
                this._currentEditHandler = function(e) {
                    if (isFunction(onComplete)) {
                        onComplete.call(null, e.sender);
                    }
                };
                this._currentEdit.bind(EDIT_COMPLETE, this._currentEditHandler);
            },
            enableNewEdit: function(parentFolder, onComplete) {
                var rootds = this.dataSource, parentId = parentFolder ? parentFolder.id : null, newFolder = rootds.createFolder(null, parentId, ""), ds = parentFolder ? parentFolder.children : rootds;
                if (parentFolder) {
                    parentFolder.setHasChildren(true);
                    parentFolder.setExpand(true);
                }
                if (!parentFolder) {
                    newFolder.incrSortOrder(rootds.maxSortOrder());
                }
                ds.pushCreate(newFolder);
                this.enableEdit(newFolder, onComplete);
            },
            isCurrentEdit: function(folder) {
                return this._currentEdit === folder && folder.edit;
            },
            cancelEdit: function() {
                if (this._currentEdit && !this._currentEdit.id) {
                    this.remove(this._currentEdit);
                }
                this.stopEdit();
            },
            stopEdit: function(sort) {
                if (this._currentEdit) {
                    this._currentEdit.setEdit(false);
                    this._currentEdit.unbind(EDIT_COMPLETE, this._currentEditHandler);
                    if (sort) {
                        var ds = this.findDataSource(this._currentEdit);
                        this.sort(!this.isRootDataSource(ds) ? ds : null);
                    }
                    this._currentEdit = this._currentEditHandler = null;
                }
            },
            completeEdit: function() {
                if (this._currentEdit) {
                    this._currentEdit.trigger(EDIT_COMPLETE);
                }
                this.stopEdit(true);
            },
            remove: function(folder) {
                var parents = this.getParents(folder), parent = parents.length > 0 ? parents[0] : null, rootds = this.dataSource, ds = parent ? parent.children : rootds, isCurrent = this._selectedItem === folder;
                ds.pushDestroy(folder);
                if (parent && ds.total() === 0) {
                    parent.setHasChildren(false);
                    parent.setExpand(false);
                }
                if (isCurrent) {
                    this.parent().messageList.clearAll();
                }
            },
            changeParent: function(folder, newParent, oldParent) {
                var newParentDs = newParent ? newParent.children : this.dataSource, currentParent = oldParent, currentParentDs = currentParent ? currentParent.children : this.dataSource, newParentId = newParent ? newParent.id : null;
                folder.setParentId(newParentId);
                currentParentDs.pushDestroy(folder);
                this._updateHasChildren(currentParent);
                if (newParent) {
                    newParent.setHasChildren(true);
                }
                newParentDs.pushCreate(folder);
            },
            _updateHasChildren: function(folder) {
                if (folder) {
                    folder.setHasChildren(!!folder.children.total());
                    if (!folder.hasChildren && folder.expanded) {
                        folder.setExpand(false);
                    }
                }
            },
            onEditKeyUp: function(e) {
                if (e.keyCode === keys.ENTER) {
                    this.completeEdit();
                }
            },
            onEditFocusOut: function() {
                this.completeEdit();
            },
            onContextCommand: function(e) {
                var command = $(e.item).data("command"), contextModel = this._contextModel(e.target), commandHandler = command ? command + "Command" : null;
                if (commandHandler && typeof this[commandHandler] === "function") {
                    this[commandHandler].call(this, contextModel);
                }
            },
            onOpenContextMenu: function(e) {
                var sender = e.sender, target = e.target, contextModel = this._contextModel(target), isContextModel = contextModel !== null, isUserFolder = contextModel ? contextModel.type === FolderType.None : false, isTrashFolder = contextModel && contextModel.type === FolderType.Trash, isSpamFolder = contextModel && contextModel.type === FolderType.Junk, isNonTrashFolder = contextModel && contextModel.type !== FolderType.Trash || !contextModel;
                this._enableContextMenuItems(sender, {
                    menuItemCreateFolder: !isSpamFolder,
                    menuItemRenameFolder: isUserFolder,
                    menuItemDeleteFolder: isUserFolder,
                    menuItemMarkAsReadFolder: isContextModel,
                    menuItemClearFolder: isContextModel
                });
                this._visibleContextMenuItems(sender, {
                    menuItemCreateFolder: isNonTrashFolder,
                    menuItemRenameFolder: isNonTrashFolder,
                    menuItemDeleteFolder: isNonTrashFolder,
                    menuItemClearFolder: isNonTrashFolder,
                    menuItemClearTrashFolder: isTrashFolder
                });
            },
            _enableContextMenuItems: function(contextMenu, menuItemCommands) {
                menuItemCommands = menuItemCommands || [];
                contextMenu.element.find("> li").each(function(i, ele) {
                    var command = $(ele).data("command");
                    if (isEmptyObject(menuItemCommands) || menuItemCommands[command] !== undefined) {
                        contextMenu.enable(ele, menuItemCommands[command]);
                    }
                });
            },
            _visibleContextMenuItems: function(contextMenu, menuItemCommands) {
                menuItemCommands = menuItemCommands || [];
                contextMenu.element.find("> li").each(function(i, ele) {
                    var command = $(ele).data("command");
                    if (isEmptyObject(menuItemCommands) || menuItemCommands[command] !== undefined) {
                        if (menuItemCommands[command]) {
                            $(ele).show();
                        } else {
                            $(ele).hide();
                        }
                    }
                });
            },
            _contextModel: function(element) {
                var model = bindingTargetSource(element);
                return this.isFolder(model) ? model : null;
            },
            menuItemCreateFolderCommand: function(parentFolder) {
                var that = this;
                this.enableNewEdit(parentFolder, function(newFolder) {
                    var displayName = (newFolder.displayName || "").trim();
                    if (displayName) {
                        var command = that.parent().command(Commands.NewFolder, {
                            model: newFolder
                        });
                        command.exec().fail(function() {
                            that.remove(newFolder);
                        });
                    } else {
                        that.cancelEdit();
                    }
                });
            },
            menuItemRenameFolderCommand: function(folder) {
                var that = this;
                if (folder) {
                    var oldValue = folder.displayName;
                    this.enableEdit(folder, function(editFolder) {
                        var displayName = (editFolder.displayName || "").trim(), valueChanged = oldValue !== displayName;
                        editFolder.setDisplayName(displayName);
                        if (!displayName) {
                            editFolder.setDisplayName(oldValue);
                            return;
                        }
                        if (valueChanged) {
                            var command = that.parent().command(Commands.UpdateFolder, {
                                model: editFolder
                            });
                            command.exec().fail(function() {
                                editFolder.setDisplayName(oldValue);
                            });
                        } else {
                            that.cancelEdit();
                        }
                    });
                }
            },
            menuItemMarkAsReadFolderCommand: function(folder) {
                if (folder) {
                    var command = this.parent().command(Commands.MarkAsReadFolder, {
                        folderId: folder.id
                    });
                    command.exec();
                }
            },
            menuItemClearTrashFolderCommand: function(folder) {
                if (folder && folder.type === FolderType.Trash) {
                    var that = this;
                    var folderId = folder.id;
                    var localization = this._options.localization;
                    var confirmDescr = localization.clearTrashFolderConfirmDescr;
                    var confirmMessage = kendo.format(localization.clearTrashFolderConfirm, this.folderName(folder));
                    confirm(confirmMessage, "", localization.dialogs, confirmDescr).done(function() {
                        var command = that.parent().command(Commands.ClearAllFolder, {
                            folderId: folderId
                        });
                        command.exec();
                    });
                }
            },
            menuItemClearFolderCommand: function(folder) {
                if (folder) {
                    var parents = this.getParents(folder);
                    var topParent = parents[parents.length - 1];
                    if (topParent && topParent.type === FolderType.Trash) {
                        this.clearFolderCommand(folder);
                    } else {
                        this.moveAllToTrashCommand(folder);
                    }
                }
            },
            clearFolderCommand: function(folder) {
                var that = this;
                var folderId = folder.id;
                var localization = this._options.localization;
                var confirmDescr = localization.deleteAllMessagesConfirmDescr;
                var confirmMessage = kendo.format(localization.deleteAllMessagesConfirm, this.folderName(folder));
                confirm(confirmMessage, "", localization.dialogs, confirmDescr).done(function() {
                    var command = that.parent().command(Commands.DeleteAll, {
                        folderId: folderId
                    });
                    command.exec();
                });
            },
            moveAllToTrashCommand: function(folder) {
                var that = this;
                var folderId = folder.id;
                var localization = this._options.localization;
                var confirmMessage = kendo.format(localization.deleteAllToTrashConfirm, this.folderName(folder));
                confirm(confirmMessage, "", localization.dialogs).done(function() {
                    var trashFolder = that.findByType(FolderType.Trash);
                    var command = that.parent().command(Commands.MoveAll, {
                        srcFolderId: folderId,
                        destFolderId: trashFolder ? trashFolder.id : null
                    });
                    command.exec();
                });
            },
            menuItemDeleteFolderCommand: function(folder) {
                if (folder) {
                    var parents = this.getParents(folder);
                    var topParent = parents[parents.length - 1];
                    if (topParent && topParent.type === FolderType.Trash) {
                        this.deleteFolderCommand(folder);
                    } else {
                        this.deleteFolderToTrashCommand(folder);
                    }
                }
            },
            deleteFolderCommand: function(folder) {
                var that = this;
                var folderId = folder.id;
                var localization = this._options.localization;
                var confirmDescr = localization.deleteFolderConfirmDescr;
                var confirmMessage = kendo.format(localization.deleteFolderConfirm, this.folderName(folder));
                confirm(confirmMessage, "", localization.dialogs, confirmDescr).done(function() {
                    var command = that.parent().command(Commands.RemoveFolder, {
                        folderId: folderId
                    });
                    command.exec();
                });
            },
            deleteFolderToTrashCommand: function(folder) {
                var that = this;
                var folderId = folder.id;
                var localization = this._options.localization;
                var confirmMessage = kendo.format(localization.deleteFolderToTrashConfirm, this.folderName(folder));
                confirm(confirmMessage, "", localization.dialogs).done(function() {
                    var command = that.parent().command(Commands.RemoveFolderToTrash, {
                        folderId: folderId
                    });
                    command.exec();
                });
            }
        });
        var AvatarStatic = {
            initials: function(name, widgetOptions) {
                return widgetOptions.contact.imageUrl ? null : name ? initials(name) : null;
            },
            imageUrl: function(service, name, email) {
                var url = service.makeContactImageUrl(name, email);
                return url ? 'url("' + url + '")' : null;
            },
            isVisible: function(name, widgetOptions) {
                return !!(widgetOptions.contact.imageUrl || name);
            }
        };
        var MessageListViewModel = BaseViewModel.extend({
            init: function(auth, broadcaster, options) {
                BaseViewModel.fn.init.call(this);
                var opt = options.message;
                this._options = options;
                this.search = proxy(this.search, this);
                this.searchOnEnter = proxy(this.searchOnEnter, this);
                this.resetSearch = proxy(this.resetSearch, this);
                this.isSearching = proxy(this.isSearching, this);
                this.toggleFlag = proxy(this.toggleFlag, this);
                this.toggleRead = proxy(this.toggleRead, this);
                this.toggleFilter = proxy(this.toggleFilter, this);
                this.toggleSort = proxy(this.toggleSort, this);
                this.selectFilterItem = proxy(this.selectFilterItem, this);
                this.resetFilter = proxy(this.resetFilter, this);
                this._handleCloseFilter = proxy(this._handleCloseFilter, this);
                this._handleCloseSort = proxy(this._handleCloseSort, this);
                this.onChange = proxy(this.onChange, this);
                this.onDoubleClick = proxy(this.onDoubleClick, this);
                this.onDragHint = proxy(this.onDragHint, this);
                this.onDrop = proxy(this.onDrop, this);
                this.onDragLeave = proxy(this.onDragLeave, this);
                this.onDragEnter = proxy(this.onDragEnter, this);
                this.onContextCommand = proxy(this.onContextCommand, this);
                this.onOpenContextMenu = proxy(this.onOpenContextMenu, this);
                this.onBroadcastUpdateMessage = proxy(this.onBroadcastUpdateMessage, this);
                this.onBroadcastRemoveMessage = proxy(this.onBroadcastRemoveMessage, this);
                this.onBroadcastChangeFolder = proxy(this.onBroadcastChangeFolder, this);
                this.onBroadcastSelectFolder = proxy(this.onBroadcastSelectFolder, this);
                broadcaster.bind(UPDATE_MESSAGE, this.onBroadcastUpdateMessage);
                broadcaster.bind(REMOVE_MESSAGE, this.onBroadcastRemoveMessage);
                broadcaster.bind(BroadcastEvents.CHANGE_FOLDER, this.onBroadcastChangeFolder);
                broadcaster.bind(BroadcastEvents.SELECT_FOLDER, this.onBroadcastSelectFolder);
                this.set("dataSource", new MessageListDataSource(auth, opt.readUrl, opt.pageSize));
                this.set("searchQuery", "");
                this.set("activeSearch", false);
                this.set("moveToMenuItems", new DataSource({
                    data: []
                }));
                this.set("activeFilter", false);
                this.set("filterOpened", false);
                this.set("filterItems", {});
                this.set("resetFilterVisible", false);
                this.set("sortOpened", false);
                this.set("sortActivated", false);
                this.set("sortOrderDesc", true);
                this.set("sortItems", [ {
                    id: "date",
                    title: "Дата",
                    selected: true,
                    def: true
                }, {
                    id: "subject",
                    title: "Тема"
                }, {
                    id: "size",
                    title: "Размер"
                }, {
                    id: "from",
                    title: "От"
                }, {
                    id: "to",
                    title: "Кому"
                } ]);
                this._selectedItem = null;
                this._selectedList = null;
                this._initDataSource(this.dataSource);
                this.dataSource.bind(REQUEST_END, proxy(this._postFetch, this));
            },
            reload: function(accountId, folderId) {
                this.clearSelection();
                this.fetch(accountId, folderId);
            },
            fetch: function(accountId, folderId) {
                this.dataSource.accountId = accountId;
                this.dataSource.folderId = folderId;
                this.dataSource._skip = 0;
                this.dataSource._page = 1;
                this.search();
            },
            _postFetch: function(e) {
                var isSentFolder = this.parent().folderList.isSentFolder();
                if (isSentFolder) {
                    var resp = e.response || {};
                    (resp.Items || []).forEach(function(item) {
                        if (isSentFolder) {
                            item.IsSent = true;
                        }
                    });
                }
            },
            search: function() {
                this.set("activeSearch", !this.searchQueryEmpty());
                this.dataSource.filter(this._makeFilter());
            },
            searchOnEnter: function(e) {
                if (e.keyCode === keys.ENTER) {
                    this.search();
                }
            },
            resetSearch: function() {
                this.set("searchQuery", "");
                this.search();
            },
            searchQueryEmpty: function() {
                return !(this.get("searchQuery") || "").trim();
            },
            isSearching: function() {
                return this.get("activeSearch");
            },
            _makeFilter: function() {
                var filters = [], searchQuery = (this.searchQuery || "").trim(), orderBy = this.getSortOrder(), orderDirection = this.sortOrderDesc;
                if (searchQuery) {
                    filters.push({
                        field: "search",
                        operator: "eq",
                        value: searchQuery
                    });
                }
                if (this.flaggedFilterChecked()) {
                    filters.push({
                        field: "is_flag",
                        operator: "eq",
                        value: true
                    });
                }
                if (this.unreadFilterChecked()) {
                    filters.push({
                        field: "is_read",
                        operator: "eq",
                        value: false
                    });
                }
                if (this.hasAttachmentsFilterChecked()) {
                    filters.push({
                        field: "has_attachments",
                        operator: "eq",
                        value: true
                    });
                }
                if (orderBy) {
                    filters.push({
                        field: "order",
                        operator: "eq",
                        value: orderBy.id
                    });
                    filters.push({
                        field: "desc",
                        operator: "eq",
                        value: orderDirection
                    });
                }
                return filters;
            },
            find: function(messageId) {
                return this.dataSource.get(messageId);
            },
            remove: function(messageId) {
                var item = this.find(messageId);
                if (item) {
                    this.dataSource.pushDestroy(item);
                }
            },
            clearAll: function() {
                this.clearSelection();
                this.dataSource.data([]);
            },
            removeList: function(messages) {
                var that = this;
                if (isArray(messages)) {
                    messages.forEach(function(item) {
                        if (item) {
                            that.dataSource.pushDestroy(item);
                        }
                    });
                }
            },
            add: function(model) {
                this.dataSource.pushCreate(model);
            },
            addIfNotExists: function(model) {
                this.dataSource.pushUpdate(model);
            },
            itemSubject: function(message) {
                return message.subject || this._options.localization.noSubject;
            },
            itemInitials: function(message) {
                return AvatarStatic.initials(message.fromName, this._options);
            },
            itemImageUrl: function(message) {
                return AvatarStatic.imageUrl(this.parent().service, message.fromName, message.fromAddress);
            },
            isAvatar: function(message) {
                return AvatarStatic.isVisible(message.fromName, this._options);
            },
            itemToInitials: function(message) {
                return AvatarStatic.initials(this._firstToName(message), this._options);
            },
            itemToImageUrl: function(message) {
                return AvatarStatic.imageUrl(this.parent().service, this._firstToName(message), this._firstToAddress(message));
            },
            isToAvatar: function(message) {
                return AvatarStatic.isVisible(this._firstToName(message), this._options);
            },
            _firstToName: function(message) {
                return message.to && message.to[0] ? message.to[0].Name : null;
            },
            _firstToAddress: function(message) {
                return message.to && message.to[0] ? message.to[0].Address : null;
            },
            select: function(messageId) {
                var item = this.find(messageId);
                if (item) {
                    this._selectedItem = item;
                    this._updateSelection(item);
                }
            },
            clearSelection: function() {
                this._selectedItem = this._selectedList = null;
                this.parent().messageDetails.clear();
                this._updateClearSelection();
            },
            clearSelectedItem: function() {
                this._selectedItem = null;
                this._updateClearSelection();
            },
            selectedItem: function(value) {
                if (!arguments.length) {
                    return this._selectedItem;
                }
                this._selectedItem = value;
            },
            selectedList: function(value) {
                if (!arguments.length) {
                    return this._selectedList;
                }
                this._selectedList = value || [];
            },
            singleSelected: function() {
                return (!this._selectedList || this._selectedList.length === 0) && this._selectedItem;
            },
            reselect: function() {
                var that = this;
                if (this._selectedList) {
                    var newSelectedList = this._selectedList.map(function(item) {
                        return that.find(item.id);
                    }).filter(notNullFilter);
                    newSelectedList.forEach(function(item) {
                        that._updateSelection(item);
                    });
                    this._selectedList = newSelectedList;
                } else if (this._selectedItem) {
                    var newSelectedItem = this.find(this._selectedItem.id);
                    if (newSelectedItem) {
                        this._updateSelection(newSelectedItem);
                    }
                    this._selectedItem = newSelectedItem;
                }
            },
            _clearManySelection: function() {
                this._selectedList = null;
                this._updateClearSelection();
            },
            _updateSelection: function(messageItem) {
                this.parent().trigger(LISTVIEW_SELECT_ITEM, {
                    type: MESSAGE,
                    item: messageItem
                });
            },
            _updateClearSelection: function() {
                this.parent().trigger(LISTVIEW_CLEAR_SELECTION, {
                    type: MESSAGE
                });
            },
            folderName: function(folder) {
                return this.parent().folderList.folderName(folder);
            },
            toggleFlag: function(e) {
                var message = e.data;
                e.stopImmediatePropagation();
                if (message) {
                    if (message.isFlag) {
                        this.menuItemRemoveFlagCommand([ message ]);
                    } else {
                        this.menuItemAddFlagCommand([ message ]);
                    }
                }
            },
            toggleRead: function(e) {
                var message = e.data;
                e.stopImmediatePropagation();
                if (message) {
                    if (message.isRead) {
                        this.menuItemMarkAsUnreadCommand([ message ]);
                    } else {
                        this.menuItemMarkAsReadCommand([ message ]);
                    }
                }
            },
            toggleFilter: function(e) {
                if (this.filterOpened) {
                    this.set("filterOpened", false);
                    this.parent().unbind(DOCUMENT_CLICK, this._handleCloseFilter);
                } else {
                    e.stopImmediatePropagation();
                    this._handleCloseSort();
                    this.set("filterOpened", true);
                    this.parent().bind(DOCUMENT_CLICK, this._handleCloseFilter);
                }
            },
            _handleCloseFilter: function() {
                this.set("filterOpened", false);
                this.parent().unbind(DOCUMENT_CLICK, this._handleCloseFilter);
            },
            selectFilterItem: function(e) {
                var filterId = $(e.currentTarget).data("filter-id");
                this.set("filterItems." + filterId, !this.get("filterItems." + filterId));
                this.set("activeFilter", this.flaggedFilterChecked() || this.unreadFilterChecked() || this.hasAttachmentsFilterChecked());
                this.set("resetFilterVisible", this.activeFilter);
                this.search();
            },
            resetFilter: function() {
                this.set("filterItems", {});
                this.set("activeFilter", false);
                this.set("resetFilterVisible", false);
                this.search();
            },
            flaggedFilterChecked: function() {
                return !!this.get("filterItems.flagged");
            },
            unreadFilterChecked: function() {
                return !!this.get("filterItems.unread");
            },
            hasAttachmentsFilterChecked: function() {
                return !!this.get("filterItems.hasAttachments");
            },
            toggleSort: function(e) {
                if (this.sortOpened) {
                    this.set("sortOpened", false);
                    this.parent().unbind(DOCUMENT_CLICK, this._handleCloseSort);
                } else {
                    e.stopImmediatePropagation();
                    this._handleCloseFilter();
                    this.set("sortOpened", true);
                    this.parent().bind(DOCUMENT_CLICK, this._handleCloseSort);
                }
            },
            _handleCloseSort: function() {
                this.set("sortOpened", false);
                this.parent().unbind(DOCUMENT_CLICK, this._handleCloseSort);
            },
            selectSortItem: function(e) {
                var previousOrder = this.getSortOrder();
                this.resetSortItems();
                this.set("sortActivated", !e.data.get("def"));
                e.data.set("selected", true);
                if (e.data.id === previousOrder.id) {
                    this.set("sortOrderDesc", !this.get("sortOrderDesc"));
                }
                this.search();
            },
            resetSortItems: function() {
                var selectedItem = first(this.sortItems.filter(filterByField("selected", true)));
                if (selectedItem) {
                    selectedItem.set("selected", false);
                }
            },
            _resetSort: function() {
                this.resetSortItems();
                this.set("sortActivated", false);
                this.set("sortOrderDesc", true);
                var defaultItem = first(this.sortItems.filter(filterByField("def", true)));
                if (defaultItem) {
                    defaultItem.set("selected", true);
                }
            },
            getSortOrder: function() {
                return first(this.sortItems.filter(filterByField("selected", true)));
            },
            onChange: function(e) {
                var sender = e.sender, selectedItems = sender.select().map(function(i, ele) {
                    return sender.dataItem(ele);
                }).get(), selectedItemsEmpty = !isArray(selectedItems) || selectedItems.length === 0, selectedMoreOne = !selectedItemsEmpty && selectedItems.length > 1, selectedItem = !selectedItemsEmpty ? selectedItems[0] : null;
                if (selectedMoreOne) {
                    this._selectedList = selectedItems;
                } else if (selectedItem) {
                    this._showMessage(selectedItem);
                }
            },
            onDoubleClick: function(e) {
                var message = e.data;
                if (message && message.isDraft) {
                    this.menuItemEditCommand([ message ]);
                }
            },
            onDragHint: function(e) {
                e.hintData.count = 1;
                if (e.target && this._elementInSelectedList(e.target[0])) {
                    e.hintData.count = (this._selectedList || []).length;
                }
            },
            onDrop: function(e) {
                var origin = e.origin, targetFolder = bindingTargetSource(origin.dropTarget[0]), folderList = this.parent().folderList, currentFolder = this.parent().folderList.getCurrent(), dragElement = origin.draggable ? origin.draggable.currentTarget : null, dragModel = dragElement && dragElement.length ? bindingTargetSource(dragElement[0]) : null, allowMove = currentFolder && targetFolder && dragModel && currentFolder.id !== targetFolder.id, messages = [];
                if (allowMove) {
                    if (this._elementInSelectedList(dragElement[0])) {
                        messages = (this._selectedList || []).slice();
                    } else {
                        messages.push(dragModel);
                    }
                    if (messages.length > 0) {
                        this._moveToFolder(targetFolder, messages).done(function() {
                            folderList.autoCollapse();
                        }).fail(function() {
                            folderList.autoCollapse();
                        });
                        return;
                    }
                }
                folderList.autoCollapse();
            },
            onDragLeave: function() {
                this.parent().folderList.autoCollapse();
            },
            onDragEnter: function() {
                this.parent().folderList.cancelAutoCollapse();
            },
            _broadcastFilter: function(filter) {
                if (filter) {
                    var folderId = filter.folderId;
                    return this.parent().folderList.isCurrent(folderId);
                }
                return true;
            },
            onBroadcastUpdateMessage: function(e) {
                var newMessageId = e.newMessageId;
                var oldMessageId = e.oldMessageId;
                if (oldMessageId && newMessageId && this._broadcastFilter(e.filter)) {
                    var message = this.find(oldMessageId);
                    if (message) {
                        message.setId(newMessageId);
                    }
                }
            },
            onBroadcastRemoveMessage: function(e) {
                var messageId = e.messageId;
                if (messageId && this._broadcastFilter(e.filter)) {
                    this.remove(messageId);
                }
            },
            onBroadcastChangeFolder: function(e) {
                var parent = this.parent(), changedFields = e.changedFields, isCurrentFolder = parent.folderList.isCurrent(e.folderId);
                if (isCurrentFolder && (changedFields.unreadItemCount !== undefined || changedFields.totalItemCount !== undefined)) {
                    parent.command(Commands.Refresh, {
                        noRefreshFolders: true
                    }).exec();
                }
            },
            onBroadcastSelectFolder: function() {
                this.set("searchQuery", "");
                this.set("activeSearch", false);
                this.set("filterItems", {});
                this.set("activeFilter", false);
                this.set("resetFilterVisible", false);
                this._resetSort();
                var filter = this.dataSource.filter();
                if (filter && filter.filters) {
                    filter.filters = [];
                }
                this._handleCloseFilter();
                this._handleCloseSort();
            },
            _moveToFolder: function(folder, messages) {
                var command = this.parent().command(Commands.MoveTo, {
                    messageIds: messages.map(selectField("id")),
                    folderId: folder.id
                });
                return command.exec();
            },
            _elementInSelectedList: function(element) {
                if (this._selectedList) {
                    var model = bindingTargetSource(element);
                    if (model) {
                        var result = this._selectedList.filter(filterByField("id", model.id));
                        return result.length > 0;
                    }
                }
                return false;
            },
            _showMessage: function(message, updateSelection) {
                var parent = this.parent(), accountId = parent.account.getCurrentId(), folder = parent.folderList.getCurrent(), previousItem = this._selectedItem, changed = message && !previousItem || message && previousItem && previousItem.id !== message.id;
                if (message && changed && accountId && folder) {
                    this._selectedItem = message;
                    this._selectedList = null;
                    parent.messageDetails.show(accountId, folder.id, message.id);
                    if (updateSelection) {
                        this._updateClearSelection();
                        this._updateSelection(message);
                    }
                }
            },
            _findMessageIdsForCommand: function(messages, fieldName, fieldValue) {
                return messages.filter(filterByField(fieldName, fieldValue)).map(selectField("id"));
            },
            menuItemOpenCommand: function(messages) {
                var message = messages[0];
                if (message) {
                    this._showMessage(message, true);
                    return null;
                }
                return false;
            },
            menuItemReplyCommand: function(messages) {
                var message = messages[0];
                if (message) {
                    var command = this.parent().command(Commands.Reply, {
                        messageId: message.id
                    });
                    return command.exec();
                }
                return false;
            },
            menuItemReplyAllCommand: function(messages) {
                var message = messages[0];
                if (message) {
                    var command = this.parent().command(Commands.Reply, {
                        messageId: message.id,
                        isAll: true
                    });
                    return command.exec();
                }
                return false;
            },
            menuItemForwardCommand: function(messages) {
                var message = messages[0];
                if (message) {
                    var command = this.parent().command(Commands.Forward, {
                        messageId: message.id
                    });
                    return command.exec();
                }
                return false;
            },
            menuItemEditCommand: function(messages) {
                var message = messages[0];
                if (message) {
                    if (this.isRunningEditDraftCommand(message.id)) {
                        this.parent().broadcast(BroadcastEvents.COMPOSE_WND_FOCUS, {
                            messageId: message.id
                        });
                    } else {
                        var command = this.parent().command(Commands.EditDraft, {
                            messageId: message.id
                        });
                        return command.exec();
                    }
                }
                return false;
            },
            menuItemMarkAsReadCommand: function(messages) {
                var messageIds = this._findMessageIdsForCommand(messages, "isRead", false);
                if (messageIds.length > 0) {
                    var command = this.parent().command(Commands.MarkAsRead, {
                        messageIds: messageIds
                    });
                    return command.exec();
                }
                return false;
            },
            menuItemMarkAsUnreadCommand: function(messages) {
                var messageIds = this._findMessageIdsForCommand(messages, "isRead", true);
                if (messageIds.length > 0) {
                    var command = this.parent().command(Commands.MarkAsUnread, {
                        messageIds: messageIds
                    });
                    return command.exec();
                }
                return false;
            },
            menuItemAddFlagCommand: function(messages) {
                var messageIds = this._findMessageIdsForCommand(messages, "isFlag", false);
                if (messageIds.length > 0) {
                    var command = this.parent().command(Commands.AddFlag, {
                        messageIds: messageIds
                    });
                    return command.exec();
                }
                return false;
            },
            menuItemRemoveFlagCommand: function(messages) {
                var messageIds = this._findMessageIdsForCommand(messages, "isFlag", true);
                if (messageIds.length > 0) {
                    var command = this.parent().command(Commands.RemoveFlag, {
                        messageIds: messageIds
                    });
                    return command.exec();
                }
                return false;
            },
            menuItemSpamCommand: function(messages) {
                var command = this.parent().command(Commands.MoveTo, {
                    messageIds: messages.map(selectField("id")),
                    folderType: FolderType.Junk
                });
                return command.exec();
            },
            menuItemDeleteCommand: function(messages) {
                var command = this.deleteMessagesCommand(messages);
                return command.defer;
            },
            deleteMessagesCommand: function(messages) {
                var that = this, folderList = this.parent().folderList, currentFolder = folderList.getCurrent(), topFolder = currentFolder ? folderList.findTopFolder(currentFolder) : null, parentOrSelfTrashFolder = topFolder ? topFolder.type === FolderType.Trash : false, command, defer;
                if (parentOrSelfTrashFolder && currentFolder) {
                    var localization = this._options.localization;
                    var confirmDescr = messages.length > 1 ? localization.deleteMessagesConfirmDescr : localization.deleteMessageConfirmDescr;
                    var confirmMessage = messages.length > 1 ? kendo.format(localization.deleteMessagesConfirm, messages.length, getNoun.apply(null, [ messages.length ].concat(localization.deleteMessagesConfirmNoun))) : kendo.format(localization.deleteMessageConfirm, this.itemSubject(messages[0]));
                    var messageIds = messages.map(selectField("id"));
                    defer = $.Deferred();
                    command = that.parent().command(Commands.Delete, {
                        folderId: currentFolder.id,
                        messageIds: messageIds
                    });
                    confirm(confirmMessage, "", localization.dialogs, confirmDescr, 500).done(function() {
                        command.exec().done(defer.resolve).fail(defer.reject);
                    });
                } else {
                    command = this.parent().command(Commands.MoveTo, {
                        messageIds: messages.map(selectField("id")),
                        folderType: FolderType.Trash
                    });
                    defer = command.exec();
                }
                return {
                    command: command,
                    defer: defer
                };
            },
            menuItemMoveToCommand: function(messages, menuItem) {
                var currentFolder = this.parent().folderList.getCurrent();
                var folderId = menuItem.data("folder-id");
                if (folderId) {
                    if (!currentFolder || currentFolder.id !== folderId) {
                        var command = this.parent().command(Commands.MoveTo, {
                            messageIds: messages.map(selectField("id")),
                            folderId: folderId
                        });
                        return command.exec();
                    }
                }
                return false;
            },
            menuItemEditAsNewCommand: function(messages) {
                var message = messages[0];
                if (message) {
                    var command = this.parent().command(Commands.EditAsNew, {
                        messageId: message.id
                    });
                    return command.exec();
                }
                return false;
            },
            isRunningEditDraftCommand: function(messageId) {
                return this.parent().runningCommands.findByType(Commands.EditDraft, function(command) {
                    return command.viewModel.getDraft().id === messageId;
                });
            },
            onContextCommand: function(e) {
                var that = this, item = $(e.item), contextModel = bindingTargetSource(e.target), command = item.data("command"), commandHandler = command ? command + "Command" : null, hasManySelected = this._selectedList && this._selectedList.length > 0;
                if (commandHandler && typeof this[commandHandler] === "function") {
                    if (hasManySelected) {
                        var selectedListContainsContextModel = !!findByRef(this._selectedList, contextModel);
                        if (selectedListContainsContextModel) {
                            var commandResult = this[commandHandler].call(this, this._selectedList, item);
                            if (commandResult && typeof commandResult.done === "function") {
                                commandResult.done(function() {
                                    that._clearManySelection();
                                });
                            }
                            return;
                        }
                    }
                    if (contextModel) {
                        this[commandHandler].call(this, [ contextModel ], item);
                    }
                }
            },
            onOpenContextMenu: function(e) {
                var item = e.item, sender = e.sender, target = e.target, parent = this.parent(), isOpenSubMenu = item && $(item).hasClass("k-item");
                if (isOpenSubMenu) {
                    return;
                }
                this.moveToMenuItems.data(parent.folderList.moveToList());
                sender._updateClasses();
                var hasManySelected = this._selectedList && this._selectedList.length > 0, contextModel = bindingTargetSource(target);
                if (!hasManySelected && contextModel) {
                    this._processContextMenuItems(sender, [ contextModel ]);
                } else if (hasManySelected && contextModel) {
                    var selectedListContainsContextModel = !!findByRef(this._selectedList, contextModel);
                    if (selectedListContainsContextModel) {
                        this._processContextMenuItems(sender, this._selectedList);
                    } else {
                        this._processContextMenuItems(sender, [ contextModel ]);
                    }
                } else if (hasManySelected) {
                    this._processContextMenuItems(sender, this._selectedList);
                }
            },
            _processContextMenuItems: function(contextMenu, targetModels) {
                var isMany = targetModels.length > 1, targetModel = !isMany ? targetModels[0] : {}, hiddenMenuItems = {}, disabledMenuItems = {}, counters = {}, parent = this.parent(), isSpamFolder = parent.folderList.isSpamFolder(), currentFolder = parent.folderList.getCurrent();
                if (isMany) {
                    hiddenMenuItems.menuItemOpen = true;
                    hiddenMenuItems.menuItemReply = true;
                    hiddenMenuItems.menuItemReplyAll = true;
                    hiddenMenuItems.menuItemForward = true;
                    hiddenMenuItems.menuItemEdit = true;
                    hiddenMenuItems.menuItemEditAsNew = true;
                    var readedCount = arrayCount(targetModels, filterByField("isRead", true));
                    var flaggedCount = arrayCount(targetModels, filterByField("isFlag", true));
                    var unreadedCount = targetModels.length - readedCount;
                    var noFlaggedCount = targetModels.length - flaggedCount;
                    hiddenMenuItems.menuItemMarkAsRead = !unreadedCount;
                    hiddenMenuItems.menuItemMarkAsUnread = !readedCount;
                    hiddenMenuItems.menuItemAddFlag = !noFlaggedCount;
                    hiddenMenuItems.menuItemRemoveFlag = !flaggedCount;
                    counters.menuItemMarkAsRead = unreadedCount;
                    counters.menuItemMarkAsUnread = readedCount;
                    counters.menuItemDelete = targetModels.length;
                } else {
                    hiddenMenuItems.menuItemReply = targetModel.isDraft;
                    hiddenMenuItems.menuItemReplyAll = targetModel.isDraft;
                    hiddenMenuItems.menuItemForward = targetModel.isDraft;
                    hiddenMenuItems.menuItemEdit = !targetModel.isDraft;
                    hiddenMenuItems.menuItemMarkAsRead = targetModel.isRead;
                    hiddenMenuItems.menuItemMarkAsUnread = !targetModel.isRead;
                    hiddenMenuItems.menuItemAddFlag = targetModel.isFlag;
                    hiddenMenuItems.menuItemRemoveFlag = !targetModel.isFlag;
                }
                hiddenMenuItems.menuItemDelete = false;
                hiddenMenuItems.menuItemSpam = isSpamFolder || targetModel.isDraft;
                contextMenu.element.find("> li").show().each(function(i, ele) {
                    var jmenuItem = $(ele), menuItem = ele, titleChanged = menuItem._titleChanged_, originTitle = menuItem._originTitle_, manyTitle = jmenuItem.data("many-title"), commandName = jmenuItem.data("command"), menuItemHidden = hiddenMenuItems[commandName], menuItemDisabled = disabledMenuItems[commandName], count = counters[commandName] || 0, jcontent = jmenuItem.find("> .k-link");
                    if (menuItemHidden) {
                        jmenuItem.hide();
                    }
                    if (menuItemDisabled !== undefined) {
                        contextMenu.enable(ele, !menuItemDisabled);
                    }
                    if (count > 0 && manyTitle) {
                        if (!menuItem._titleChanged_) {
                            menuItem._titleChanged_ = true;
                            menuItem._originTitle_ = jcontent.html();
                        }
                        jcontent.html(kendo.format(manyTitle, count));
                    } else if (titleChanged) {
                        jcontent.html(originTitle);
                        menuItem._titleChanged_ = false;
                        menuItem._originTitle_ = null;
                    }
                });
                contextMenu.element.find("li[data-folder-id]").each(function(i, ele) {
                    var item = $(ele);
                    var folderId = item.data("folder-id"), span = item.find("> .k-link > span"), model = span.length > 0 ? bindingTargetSource(span[0]) : null;
                    if (currentFolder && currentFolder.id === folderId) {
                        if (model && model.children && model.children.total() > 0) {
                            item.addClass("context-menu__subitem--disabled");
                        } else {
                            contextMenu.enable(ele, false);
                        }
                    }
                });
            }
        });
        var MessageDetailsViewModel = BaseViewModel.extend({
            init: function(auth, broadcaster, options) {
                BaseViewModel.fn.init.call(this);
                var opt = options.message;
                this.set("dataSource", new MessageDetailsDataSource(auth, opt.readItemUrl));
                this.set("hasCurrent", false);
                this.set("infoVisible", false);
                this.set("folderSelectorOpened", false);
                this.set("additionalActionsOpened", false);
                this.set("moveToItems", new DataSource({
                    data: []
                }));
                this._options = options;
                this._markAsReadTimer = null;
                this._moveToCommand = null;
                this._setFlagCommand = null;
                this._handleCloseFolderSelector = proxy(this._handleCloseFolderSelector, this);
                this._handleCloseAdditionalActions = proxy(this._handleCloseAdditionalActions, this);
                this.onBroadcastUpdateMessage = proxy(this.onBroadcastUpdateMessage, this);
                this.onBroadcastRemoveMessage = proxy(this.onBroadcastRemoveMessage, this);
                broadcaster.bind(UPDATE_MESSAGE, this.onBroadcastUpdateMessage);
                broadcaster.bind(REMOVE_MESSAGE, this.onBroadcastRemoveMessage);
                this._initDataSource(this.dataSource);
            },
            fetch: function(accountId, folderId, messageId) {
                var that = this;
                return this.dataSource.read({
                    accountId: accountId,
                    folderId: folderId,
                    messageId: messageId
                }).then(function() {
                    that._resetState();
                    that.set("hasCurrent", true);
                    return that.getCurrent();
                });
            },
            show: function(accountId, folderId, messageId) {
                var that = this, opt = this._options.message;
                this._cancelMarkAsReadTimer();
                return this.fetch(accountId, folderId, messageId).done(function(message) {
                    if (!message.isRead && opt.markAsReadTimeout !== null) {
                        that._startMarkAsReadTimer(message.id, opt.markAsReadTimeout);
                    }
                });
            },
            getCurrent: function() {
                var item = this.dataSource.view();
                return item && item.length > 0 ? item[0] : null;
            },
            isCurrent: function(messageId) {
                var current = this.getCurrent();
                return current ? current.id === messageId : false;
            },
            find: function(messageId) {
                return this.dataSource.get(messageId);
            },
            clear: function() {
                this.set("hasCurrent", false);
                this.dataSource.data([]);
                this._resetState();
            },
            close: function() {
                var current = this.getCurrent(), messageList = this.parent().messageList, selectedItem = messageList.selectedItem(), selectedList = messageList.selectedList(), clearSelection = selectedItem && current && selectedItem.id === current.id;
                if (clearSelection) {
                    if (selectedList) {
                        messageList.selectedItem(null);
                    } else {
                        messageList.clearSelectedItem();
                    }
                }
                this.clear();
            },
            moveToTrash: function() {
                var current = this.getCurrent(), parent = this.parent();
                if (current) {
                    parent.messageList.deleteMessagesCommand([ current ]);
                }
            },
            moveToSpam: function() {
                this.moveToFolder(FolderType.Junk);
            },
            isTrashFolder: function() {
                return this.parent().folderList.isTrashFolder();
            },
            isSpamFolder: function() {
                return this.parent().folderList.isSpamFolder();
            },
            isSentFolder: function() {
                return this.parent().folderList.isSentFolder();
            },
            moveToFolder: function(folderType, folderId) {
                var current = this.getCurrent(), parent = this.parent(), currentFolder = parent.folderList.getCurrent(), canMoveTo = (!currentFolder || currentFolder.id !== folderId) && (current && !this._commandPending(this._moveToCommand)), command;
                if (canMoveTo) {
                    command = parent.command(Commands.MoveTo, {
                        messageIds: [ current.id ],
                        folderType: folderType,
                        folderId: folderId
                    });
                    command.exec();
                    this._moveToCommand = command;
                }
            },
            toggleFlag: function() {
                var current = this.getCurrent(), parent = this.parent(), command;
                if (current && !this._commandPending(this._setFlagCommand)) {
                    if (!current.isFlag) {
                        command = parent.command(Commands.AddFlag, {
                            messageIds: [ current.id ]
                        });
                    } else {
                        command = parent.command(Commands.RemoveFlag, {
                            messageIds: [ current.id ]
                        });
                    }
                    command.exec();
                    this._setFlagCommand = command;
                }
            },
            markAsRead: function(messageId) {
                var parent = this.parent(), command;
                if (messageId) {
                    command = parent.command(Commands.MarkAsRead, {
                        messageIds: [ messageId ],
                        noProgressBar: true
                    });
                    command.exec();
                }
            },
            reply: function() {
                this._reply(false);
            },
            replyAll: function() {
                this._reply(true);
            },
            _reply: function(isAll) {
                var message = this.getCurrent();
                if (message) {
                    this.parent().command(Commands.Reply, {
                        messageDetails: message,
                        isAll: isAll
                    }).exec();
                }
            },
            forward: function() {
                var message = this.getCurrent();
                if (message) {
                    this.parent().command(Commands.Forward, {
                        messageDetails: message
                    }).exec();
                }
            },
            editAsNew: function() {
                var message = this.getCurrent();
                if (message) {
                    this.parent().command(Commands.EditAsNew, {
                        messageDetails: message
                    }).exec();
                }
            },
            edit: function() {
                var message = this.getCurrent();
                if (message && message.isDraft) {
                    var command = this.parent().runningCommands.findByType(Commands.EditDraft, function(command) {
                        return command.viewModel.getDraft().id === message.id;
                    });
                    if (command) {
                        this.parent().broadcast(BroadcastEvents.COMPOSE_WND_FOCUS, {
                            messageId: message.id
                        });
                    } else {
                        this.parent().command(Commands.EditDraft, {
                            messageDetails: message
                        }).exec();
                    }
                }
            },
            toggleInfo: function() {
                this.set("infoVisible", !this.get("infoVisible"));
            },
            toggleAdditionalActions: function(e) {
                if (this.additionalActionsOpened) {
                    this.closeAdditionalActions();
                } else {
                    this.closeFolderSelector();
                    this.openAdditionalActions(e);
                }
            },
            openAdditionalActions: function(e) {
                e.stopImmediatePropagation();
                this.set("additionalActionsOpened", true);
                this.parent().bind(DOCUMENT_CLICK, this._handleCloseAdditionalActions);
            },
            closeAdditionalActions: function() {
                this.set("additionalActionsOpened", false);
                this.parent().unbind(DOCUMENT_CLICK, this._handleCloseAdditionalActions);
            },
            _handleCloseAdditionalActions: function() {
                this.closeAdditionalActions();
            },
            toggleFolderSelector: function(e) {
                if (this.folderSelectorOpened) {
                    this.closeFolderSelector();
                } else {
                    this.closeAdditionalActions();
                    this.openFolderSelector(e);
                }
            },
            openFolderSelector: function(e) {
                e.stopImmediatePropagation();
                this.updateMoveToItems();
                this.set("folderSelectorOpened", true);
                this.parent().bind(DOCUMENT_CLICK, this._handleCloseFolderSelector);
            },
            closeFolderSelector: function() {
                this.set("folderSelectorOpened", false);
                this.parent().unbind(DOCUMENT_CLICK, this._handleCloseFolderSelector);
            },
            _handleCloseFolderSelector: function() {
                this.closeFolderSelector();
            },
            selectFolder: function(e) {
                var folder = e.data;
                e.stopImmediatePropagation();
                this.closeFolderSelector();
                if (folder) {
                    this.moveToFolder(null, folder.id);
                }
            },
            folderName: function(folder) {
                return this.parent().folderList.folderName(folder);
            },
            folderList: function() {
                return [];
            },
            updateMoveToItems: function() {
                var result = [];
                var folderTree = this.parent().folderList.moveToList();
                var deepLevel = 0;
                this._flattenMoveToItems(result, deepLevel, folderTree);
                this.moveToItems.data(result);
            },
            _flattenMoveToItems: function(result, deepLevel, nodes) {
                for (var i = 0; i < nodes.length; i++) {
                    nodes[i].deepLevel = deepLevel;
                    result.push(nodes[i]);
                    if (nodes[i].children.total() > 0) {
                        this._flattenMoveToItems(result, deepLevel + 1, nodes[i].children.data());
                    }
                }
            },
            isMoveToItemDisabled: function(data) {
                var currentFolder = this.parent().folderList.getCurrent();
                return currentFolder && currentFolder.id === data.id;
            },
            onDownloadAttachment: function(e) {
                var that = this, parent = this.parent(), auth = parent.auth, authToken = auth.enabled() ? auth.token : null, attachment = e.data;
                if (authToken) {
                    attachment.set("progress", true);
                    auth.validateOrRefreshToken(authToken).done(function() {
                        attachment.set("progress", false);
                        var url = that.makeAttachmentUrl(attachment.FileId, auth.token);
                        if (url !== false) {
                            attachment.set("downloadUrl", [ {
                                url: url
                            } ]);
                        } else {
                            attachment.set("downloadUrl", []);
                        }
                    }).fail(function() {
                        attachment.set("progress", false);
                        attachment.set("downloadUrl", []);
                    });
                } else {
                    attachment.set("downloadUrl", []);
                }
                e.preventDefault();
            },
            makeAttachmentUrl: function(fileId, authToken) {
                var opt = this._options, parent = this.parent(), urlTemplate = opt.message.attachmentUrl, curr = this.getCurrent(), folderId = curr ? curr.folderId : null, messageId = curr ? curr.id : null, accountId = parent.account.getCurrentId(), isValid = accountId && folderId && messageId && urlTemplate && fileId;
                return isValid ? kendo.format(urlTemplate, accountId, folderId, messageId, fileId, authToken) : false;
            },
            initials: function(message) {
                return AvatarStatic.initials(message.fromName, this._options);
            },
            imageUrl: function(message) {
                return AvatarStatic.imageUrl(this.parent().service, message.fromName, message.fromAddress);
            },
            isAvatar: function(message) {
                return AvatarStatic.isVisible(message.fromName, this._options);
            },
            _commandPending: function(command) {
                return command && command.isPending();
            },
            _startMarkAsReadTimer: function(messageId, timeout) {
                var that = this;
                this._cancelMarkAsReadTimer();
                this._markAsReadTimer = timer(timeout, function() {
                    that.markAsRead(messageId);
                });
                this._markAsReadTimer.start();
            },
            _cancelMarkAsReadTimer: function() {
                if (this._markAsReadTimer) {
                    this._markAsReadTimer.cancel();
                    this._markAsReadTimer = null;
                }
            },
            _resetState: function() {
                this.set("infoVisible", false);
                this.set("folderSelectorOpened", false);
                this.moveToItems.data([]);
                this._cancelMarkAsReadTimer();
            },
            _broadcastFilter: function(filter) {
                if (filter) {
                    var folderId = filter.folderId;
                    return this.parent().folderList.isCurrent(folderId);
                }
                return true;
            },
            onBroadcastUpdateMessage: function(e) {
                var newMessageId = e.newMessageId, oldMessageId = e.oldMessageId;
                if (oldMessageId && newMessageId && this.isCurrent(oldMessageId) && this._broadcastFilter(e.filter)) {
                    this.getCurrent().setId(newMessageId);
                }
            },
            onBroadcastRemoveMessage: function(e) {
                var messageId = e.messageId;
                if (messageId && this.isCurrent(messageId) && this._broadcastFilter(e.filter)) {
                    this.close();
                }
            }
        });
        var ContactPreviewViewModel = ObservableObject.extend({
            init: function(options) {
                ObservableObject.fn.init.call(this);
                this._options = options;
                this.onPreviewNotFound = proxy(this.onPreviewNotFound, this);
                this.onBeforePreviewOpen = proxy(this.onBeforePreviewOpen, this);
            },
            onPreviewNotFound: function(e) {
                var sender = e.sender, data = {
                    Name: "",
                    Email: ""
                }, target = sender.target(), email = target ? target.data("preview") : null, title = target ? target.data("preview-title") : null;
                data.Title = title || "";
                data.Email = email || "";
                e.data = data;
            },
            onBeforePreviewOpen: function(e) {
                var sender = e.sender, contentOpt = sender.options.content, ajaxOptions = this._options.contact.ajaxOptions, email = e.target ? e.target.data("preview") : null, service = this.parent().service;
                contentOpt.url = email ? service.makeContactPreviewUrl(email) : null;
                contentOpt.xhrFields = {
                    withCredentials: ajaxOptions.withCredentials
                };
            }
        });
        var SendMessageDataViewModel = ObservableObject.extend({
            init: function(data) {
                var defaults = {
                    to: [],
                    cc: [],
                    bcc: [],
                    subject: "",
                    body: ""
                };
                data = $.extend(defaults, data || {});
                ObservableObject.fn.init.call(this, data);
            }
        });
        var AttachmentUploadViewModel = ObservableObject.extend({
            init: function(auth, options) {
                ObservableObject.fn.init.call(this);
                this.auth = auth;
                this._options = options;
                this._uploader = null;
                this.set("fileCount", 0);
                this.set("fileListOpened", false);
                this.set("fileListEnabled", false);
                this.set("actionPending", false);
                this.set("actionQueue", []);
                this.toggleFileList = proxy(this.toggleFileList, this);
                this.onFileSelect = proxy(this.onFileSelect, this);
                this.onFileUpload = proxy(this.onFileUpload, this);
                this.onFileSuccess = proxy(this.onFileSuccess, this);
                this.onFileRemove = proxy(this.onFileRemove, this);
                this.onFileCancel = proxy(this.onFileCancel, this);
                this.onFileError = proxy(this.onFileError, this);
                this.onUploadCompleted = proxy(this.onUploadCompleted, this);
                this.onFileListCreated = proxy(this.onFileListCreated, this);
                this.onFileListDestroyed = proxy(this.onFileListDestroyed, this);
            },
            initFiles: function(files) {
                this.updateFiles(files);
            },
            updateFiles: function(files) {
                if (files && files.length > 0 && this.getUploader()) {
                    var uploader = this.getUploader();
                    this._updateUploadedFiles(uploader, {
                        Attachments: files
                    });
                    this.set("fileCount", files.length);
                }
            },
            getFiles: function() {
                return this.getUploader() ? this.getUploader().getFiles().map(this.selectFile) : [];
            },
            selectFile: function(file) {
                return {
                    id: file.id,
                    name: file.name,
                    size: file.size,
                    ext: file.extension
                };
            },
            initUploader: function() {
                var uploader = this.getUploader();
                if (uploader) {
                    this.setFileListEnabled(uploader.isFileList());
                }
            },
            getUploader: function() {
                if (!this._uploader) {
                    this._uploader = this.findUploader();
                }
                return this._uploader;
            },
            findUploader: function() {
                var wnd = this.parent().getWnd();
                return wnd ? wnd.element.find(UPLOAD_SELECTOR).data(UPLOAD_WIDGET) : null;
            },
            dispose: function() {
                this.set("actionQueue", []);
                this._uploader = null;
            },
            addFileCount: function(value) {
                this.set("fileCount", this.fileCount + value);
            },
            subFileCount: function(value) {
                var val = this.fileCount - value;
                this.set("fileCount", val < 0 ? 0 : val);
            },
            toggleFileList: function() {
                if (this.fileListEnabled) {
                    this.set("fileListOpened", !this.fileListOpened);
                } else {
                    this.set("fileListOpened", false);
                }
            },
            setFileListOpened: function(flag) {
                this.set("fileListOpened", flag);
            },
            closeFileList: function() {
                this.setFileListOpened(false);
            },
            setFileListEnabled: function(flag) {
                this.set("fileListEnabled", flag);
            },
            isPending: function() {
                return this.get("actionPending");
            },
            pending: function(value) {
                this.set("actionPending", value);
            },
            actionQueueCount: function() {
                return this.get("actionQueue").length;
            },
            queueAction: function(files, operation) {
                var that = this;
                files = files || [];
                files.forEach(function(file) {
                    that.actionQueue.push({
                        op: operation,
                        fileUid: file.uid
                    });
                });
            },
            queueUpload: function(files) {
                this.queueAction(files, OP_UPLOAD);
            },
            queueRemove: function(files) {
                this.queueAction(files, OP_REMOVE);
            },
            removeQueueAction: function(files, operation) {
                var that = this;
                files = files || [];
                files.forEach(function(file) {
                    var entry = that.actionQueue.find(function(item) {
                        return item.fileUid === file.uid && item.op === operation;
                    });
                    if (entry) {
                        that.actionQueue.remove(entry);
                    }
                });
            },
            performNextAction: function(uploader) {
                if (!this.isPending()) {
                    var action = this.actionQueue.shift();
                    if (action) {
                        if (action.op === OP_UPLOAD) {
                            this.performUploadAction(uploader, action);
                        } else if (action.op === OP_REMOVE) {
                            this.performRemoveAction(uploader, action);
                        }
                        this.trigger(QUEUE_PROCESSING);
                    } else {
                        this.trigger(QUEUE_COMPLETE);
                    }
                }
            },
            performUploadAction: function(uploader, action) {
                var fileUid = action.fileUid;
                if (fileUid && uploader.hasFileByUid(fileUid)) {
                    uploader.uploadFileByUid(fileUid);
                } else {
                    this.performNextAction(uploader);
                }
            },
            performRemoveAction: function(uploader, action) {
                var fileUid = action.fileUid, fileData = fileUid ? uploader.findFileDataByUid(fileUid) : null, isRemove = fileUid && fileData && fileData.id && uploader.isFileUploadedByUid(fileUid);
                if (isRemove) {
                    this.prepareRemoveUrl(uploader, fileData.id);
                    this.pending(true);
                    this._fileListProgress(uploader, true);
                    uploader.removeFileByUid(fileUid);
                } else {
                    this.performNextAction(uploader);
                }
            },
            performNextActionAsync: function(uploader) {
                var that = this;
                this._runAsync(function() {
                    that.auth.validateOrRefreshToken().done(function() {
                        that.performNextAction(uploader);
                    });
                });
            },
            prepareUploadUrl: function(uploader) {
                var parent = this.parent(), service = parent._service, draft = parent.getDraft() || {}, asyncOpt = uploader.options.async;
                asyncOpt.saveUrl = service.makeUploadFileUrl(draft.id, draft.folderId);
            },
            prepareRemoveUrl: function(uploader, fileId) {
                var parent = this.parent(), service = parent._service, draft = parent.getDraft() || {}, asyncOpt = uploader.options.async;
                asyncOpt.removeUrl = service.makeRemoveFileUrl(draft.id, fileId, draft.folderId);
            },
            _runAsync: function(fn) {
                var that = this;
                var timeout = window.setTimeout(function() {
                    clearTimeout(timeout);
                    if (isFunction(fn)) {
                        fn.call(that);
                    }
                }, 0);
            },
            _updateFileCount: function(count, operation) {
                if (operation === OP_UPLOAD) {
                    this.addFileCount(count);
                } else if (operation === OP_REMOVE) {
                    this.subFileCount(count);
                }
            },
            _updateDraft: function(data) {
                var parent = this.parent(), newDraftId = data.UniqueId || null;
                if (newDraftId) {
                    parent.setDraft(newDraftId);
                }
            },
            _updateUploadedFiles: function(uploader, data) {
                var newFiles = data.Attachments || [], oldFileListOpened = this.fileListOpened;
                if (newFiles.length > 0) {
                    uploader.clearUploadedFiles();
                    uploader.addUploadedFiles(newFiles.map(function(file) {
                        return {
                            name: file.FileName,
                            size: file.FileSize,
                            extension: fileExtension(file.FileName, true),
                            id: file.FileId
                        };
                    }));
                    this.setFileListOpened(oldFileListOpened);
                }
            },
            _updateUploadedFilesAsync: function(uploader, data) {
                this._runAsync(function() {
                    this._updateUploadedFiles(uploader, data);
                });
            },
            _isFilesUploaded: function(uploader, files) {
                var uploaded = files.filter(function(file) {
                    return uploader.isFileUploadedByUid(file.uid);
                });
                return uploaded.length === files.length;
            },
            _fileListProgress: function(uploader, flag) {
                uploader.fileListProgress(flag);
            },
            onFileSelect: function(e) {
                var that = this, parent = this.parent(), sender = e.sender, files = e.files;
                this.queueUpload(files);
                this.setFileListOpened(true);
                parent.createDraft().done(function() {
                    that.performNextActionAsync(sender);
                });
            },
            onFileRemove: function(e) {
                var that = this, parent = this.parent(), sender = e.sender, files = e.files;
                this.removeQueueAction(e.files, OP_UPLOAD);
                if (this._isFilesUploaded(sender, files)) {
                    e.preventDefault();
                    if (!this.isPending()) {
                        this.removeQueueAction(files, OP_REMOVE);
                        this.queueRemove(files);
                        parent.createDraft().done(function() {
                            that.performNextActionAsync(sender);
                        });
                    }
                }
            },
            onFileUpload: function(e) {
                if (this.isPending()) {
                    e.preventDefault();
                    return;
                }
                this.pending(true);
                this._fileListProgress(e.sender, true);
                this.prepareUploadUrl(e.sender);
            },
            onFileSuccess: function(e) {
                var response = e.response || {}, operation = e.operation, files = e.files || [], filesCount = files.length, sender = e.sender;
                this.pending(false);
                this._fileListProgress(sender, false);
                this._updateFileCount(filesCount, operation);
                this._updateDraft(response);
                this._updateUploadedFilesAsync(sender, response);
                this.performNextActionAsync(sender);
            },
            onFileCancel: function(e) {
                this.pending(false);
                this._fileListProgress(e.sender, false);
                this.performNextActionAsync(e.sender);
            },
            onFileError: function(e) {
                this.pending(false);
                this._fileListProgress(e.sender, false);
                this.performNextActionAsync(e.sender);
            },
            onUploadCompleted: function() {},
            onFileListCreated: function() {
                this.setFileListEnabled(true);
            },
            onFileListDestroyed: function() {
                this.setFileListEnabled(false);
                this.setFileListOpened(false);
            }
        });
        var NewMessageDlgViewModel = ObservableObject.extend({
            init: function(options, service, messageData) {
                ObservableObject.fn.init.call(this);
                this._prepareMessageData(messageData);
                this.set("msg", new SendMessageDataViewModel(messageData));
                this.set("attachments", new AttachmentUploadViewModel(service.getAuth(), options));
                this.set("sending", false);
                this.set("sendError", false);
                this.set("draftSaveError", false);
                this.set("copyVisible", false);
                this.set("hasUnsavedData", false);
                this.set("attachmentSaved", true);
                this.set("dataSavedTs", null);
                this.set("isSettingsOpened", false);
                this.set("settingItems", [ {
                    id: "highPriority",
                    title: "Высокий приоритет",
                    checked: !!this.msg.highPriority,
                    icon: "flag"
                }, {
                    id: "confirmRead",
                    title: "Подтвердить прочтение",
                    checked: !!this.msg.confirmRead,
                    icon: "envelope-open-o"
                }, {
                    id: "confirmDelivery",
                    title: "Подтверждение о доставке",
                    checked: !!this.msg.confirmDelivery,
                    icon: "laptop"
                } ]);
                this.dialogResult = false;
                this._options = options;
                this._service = service;
                this._draft = {
                    id: null,
                    folderId: null
                };
                this._wnd = null;
                this._editor = null;
                this._titleSet = false;
                this._draftDeferred = null;
                this._saveDraftQueued = false;
                this._forceClose = false;
                this._needRefreshAfterClose = false;
                this._initDataSources(messageData);
                this._initDraft(messageData);
                this._saveDraftTimer = timer(options.draft.saveTimeout, proxy(this._queueSaveDraft, this));
                this._notifyDataChanged = kendo.throttle(proxy(this._dataChanged, this), options.draft.notifyChangedTimeout);
                this._resizeEditorToolbarHandler = kendo.throttle(proxy(this._resizeEditorToolbar, this), 500);
                this._saveDraftDone = proxy(this._saveDraftDone, this);
                this._saveDraftFail = proxy(this._saveDraftFail, this);
                this._sendDraftDone = proxy(this._sendDraftDone, this);
                this._sendDraftFail = proxy(this._sendDraftFail, this);
                this.msg.bind(CHANGE, this._notifyDataChanged);
                this.attachments.bind(QUEUE_PROCESSING, proxy(this.onAttachmentProcessing, this));
                this.attachments.bind(QUEUE_COMPLETE, proxy(this.onAttachmentCompleted, this));
                this.onBroadcastComposeWndFocus = proxy(this.onBroadcastComposeWndFocus, this);
                service.onBroadcast(BroadcastEvents.COMPOSE_WND_FOCUS, this.onBroadcastComposeWndFocus);
                this._onCloseSettings = proxy(this._onCloseSettings, this);
                this._onEditorFocus = proxy(this._onEditorFocus, this);
                this._waitSaveDaftAndClose = proxy(this._waitSaveDaftAndClose, this);
            },
            _initDataSources: function(messageData) {
                var options = this._options, to = [], cc = [], bcc = [];
                if (messageData) {
                    to = messageData.to || [];
                    cc = messageData.cc || [];
                    bcc = messageData.bcc || [];
                }
                this.set("toDataSource", this._createContactDataSource(to, options));
                this.set("ссDataSource", this._createContactDataSource(cc, options));
                this.set("bссDataSource", this._createContactDataSource(bcc, options));
                this.set("copyVisible", cc.length > 0 || bcc.length > 0);
            },
            _initDraft: function(messsageData) {
                if (messsageData) {
                    var draftId = messsageData.messageId || null, folderId = messsageData.folderId || null;
                    this.setDraft(draftId, folderId);
                }
            },
            setWnd: function(wnd) {
                this._oldTitle = wnd.title();
                this._wnd = wnd;
            },
            getWnd: function() {
                return this._wnd;
            },
            send: function() {
                if (this.sendEnabled()) {
                    this.attachments.closeFileList();
                    if (this.hasUnsavedData || !this.hasDraft()) {
                        this.saveDraftAndSend();
                    } else {
                        this._sendDraft();
                    }
                }
            },
            saveDraftAndSend: function() {
                var that = this;
                this.cancelSaveDraftTimer();
                this.pending(true);
                var saveDone = function() {
                    that._sendDraft();
                };
                var saveFail = function() {
                    that.pending(false);
                    that.showSendError();
                };
                if (this._draftDeferred) {
                    this._draftDeferred.done(saveDone).fail(saveFail);
                } else {
                    this._saveDraft().done(saveDone).fail(saveFail);
                }
            },
            sendEnabled: function() {
                var attachmentSaved = this.get("attachmentSaved"), hasTo = (this.get("msg.to") || []).length > 0, notSending = !this.get("sending");
                return attachmentSaved && hasTo && notSending;
            },
            pending: function(value) {
                this.set("sending", value);
            },
            showSendError: function() {
                this.set("sendError", true);
                this.set("draftSaveError", false);
            },
            showDraftError: function() {
                this.set("sendError", false);
                this.set("draftSaveError", true);
            },
            hideErrors: function() {
                this.set("sendError", false);
                this.set("draftSaveError", false);
            },
            showCopy: function() {
                this.set("copyVisible", true);
            },
            dataSavedTsFormatted: function() {
                var ts = this.get("dataSavedTs");
                return ts ? kendo.toString(ts, "t") : "";
            },
            saveStatusVisible: function() {
                return !this.get("hasUnsavedData") && !!this.get("dataSavedTs") && !this.get("sending") && this.get("attachmentSaved");
            },
            toggleSettings: function(e) {
                if (this.isSettingsOpened) {
                    this.set("isSettingsOpened", false);
                    this._service.parent().unbind(DOCUMENT_CLICK, this._onCloseSettings);
                } else {
                    e.stopImmediatePropagation();
                    this.set("isSettingsOpened", true);
                    this._service.parent().bind(DOCUMENT_CLICK, this._onCloseSettings);
                }
            },
            _onCloseSettings: function() {
                this.set("isSettingsOpened", false);
                this._service.parent().unbind(DOCUMENT_CLICK, this._onCloseSettings);
            },
            onSelectSettingItem: function(e) {
                e.stopImmediatePropagation();
                e.data.set("checked", !e.data.get("checked"));
                this._notifyDataChanged();
            },
            isSettingChecked: function(settingId) {
                var settingItem = first(this.settingItems.filter(filterByField("id", settingId)));
                return settingItem ? settingItem.checked : false;
            },
            onResizeWnd: function() {
                this._resizeEditorToolbarHandler(this._editor);
            },
            onAfterOpenWnd: function() {
                this._editor = this._findEditor();
                this._resizeEditorToolbarHandler(this._editor);
                this._initEditorEvents(this._editor);
                this.attachments.initUploader();
                this.attachments.initFiles(this.msg.attachments);
            },
            onAfterCloseWnd: function() {
                if (this._needRefreshAfterClose) {
                    this._refreshMessages();
                }
                this.destroy();
            },
            onBeforeCloseWnd: function(e) {
                if (!this._forceClose && (this.hasUnsavedData || !this.attachmentSaved)) {
                    this.closeConfirm();
                    e.preventDefault();
                }
                if (this._forceClose) {
                    this._removeDraft();
                    this._needRefreshAfterClose = false;
                }
            },
            closeConfirm: function() {
                var that = this;
                var localization = this._options.localization;
                var confirmMessage = localization.unsavedDataConfirm;
                confirm(confirmMessage, "", localization.usavedDataDialog, null, 480).done(function() {
                    that.unbind(CHANGE, that._waitSaveDaftAndClose);
                    that.bind(CHANGE, that._waitSaveDaftAndClose);
                }).fail(function() {
                    that._forceClose = true;
                    that._close();
                });
            },
            _waitSaveDaftAndClose: function(e) {
                if (e.field === "hasUnsavedData" || e.field === "attachmentSaved") {
                    if (!this.hasUnsavedData && this.attachmentSaved) {
                        this._close();
                    }
                }
            },
            onAttachmentProcessing: function() {
                this.set("attachmentSaved", false);
                this._updateTitle();
                this._needRefreshAfterClose = true;
            },
            onAttachmentCompleted: function() {
                this.set("attachmentSaved", true);
                this._updateDataSavedTs();
                this._updateTitle();
                this.onSaveDraftQueue();
            },
            onBroadcastComposeWndFocus: function(e) {
                var messageId = e.messageId;
                if (messageId === this.getDraft().id && this._wnd) {
                    this._wnd.toFront();
                    if (this._editor) {
                        this._editor.focus();
                    }
                }
            },
            _findEditor: function() {
                return this._wnd ? this._wnd.element.find(EDITOR_SELECTOR).data(EDITOR_WIDGET) : null;
            },
            _resizeEditorToolbar: function(editor) {
                if (editor) {
                    editor.toolbar.resize();
                }
            },
            _initEditorEvents: function(editor) {
                if (editor) {
                    if (editor.body) {
                        $(editor.body).on("focus", this._onEditorFocus);
                    }
                }
            },
            _remEditorEvents: function(editor) {
                if (editor) {
                    if (editor.body) {
                        $(editor.body).off("focus", this._onEditorFocus);
                    }
                }
            },
            _onEditorFocus: function() {
                this._onCloseSettings();
            },
            editorChanged: function() {
                this._notifyDataChanged();
            },
            _editorValue: function() {
                return this._editor ? this._editor.value() : null;
            },
            _syncEditorValue: function() {
                this.msg.body = this._editorValue();
            },
            _sendDraft: function() {
                this.pending(true);
                this._service.sendDraft(this._draft.id, this._draft.folderId).done(this._sendDraftDone).fail(this._sendDraftFail);
            },
            _sendDraftDone: function() {
                this.pending(false);
                this.onRemoveMessage(this._draft.id, {
                    folderId: this._draft.folderId
                });
                this._needRefreshAfterClose = false;
                this._close();
            },
            _sendDraftFail: function(error) {
                traceError(error.toString());
                this.pending(false);
                this.showSendError();
            },
            _saveDraft: function() {
                this._syncEditorValue();
                var draft = this._draft, service = this._service, withCopyAttachments = !draft.id, msgData = this._createMsg(withCopyAttachments);
                this.hideErrors();
                this._draftDeferred = service.saveDraft(draft.id, draft.folderId, msgData).done(this._saveDraftDone).fail(this._saveDraftFail);
                return this._draftDeferred;
            },
            _queueSaveDraft: function() {
                if (this._draftDeferred || !this.attachmentSaved) {
                    this._saveDraftQueued = true;
                } else {
                    this._saveDraft();
                }
            },
            _saveDraftDone: function(data) {
                this._draftDeferred = null;
                this.setDraft(data.id, data.folderId);
                if (this.attachments) {
                    this.attachments.updateFiles(data.files);
                }
                this._dataSaved();
                this.onSaveDraftQueue();
            },
            _saveDraftFail: function(error) {
                traceError(error.toString());
                this._draftDeferred = null;
                this.showDraftError();
                this.onSaveDraftQueue();
            },
            onSaveDraftQueue: function() {
                if (this._saveDraftQueued) {
                    this._saveDraftQueued = false;
                    this._queueSaveDraft();
                }
            },
            createDraft: function() {
                var that = this, deferred = $.Deferred();
                if (this._draftDeferred) {
                    if (!this._createDraftWait) {
                        this._createDraftWait = true;
                        this._draftDeferred.done(function() {
                            deferred.resolve(that.getDraft());
                            that._createDraftWait = false;
                        }).fail(function(error) {
                            that._createDraftWait = false;
                            deferred.reject(error);
                        });
                    }
                } else if (this.hasDraft()) {
                    if (this.hasUnsavedData) {
                        this.cancelSaveDraftTimer();
                        this._saveDraftQueued = true;
                    }
                    deferred.resolve(this.getDraft());
                } else {
                    this.cancelSaveDraftTimer();
                    this._markAsUnsaved();
                    this._saveDraft().done(function() {
                        deferred.resolve(that.getDraft());
                    }).fail(deferred.reject);
                }
                return deferred;
            },
            cancelSaveDraftTimer: function() {
                this._saveDraftTimer.cancel();
            },
            getDraft: function() {
                return this._draft;
            },
            setDraft: function(id, folderId) {
                var previousId = this._draft.id;
                this._draft.id = id;
                if (folderId) {
                    this._draft.folderId = folderId;
                }
                if (previousId && previousId !== id) {
                    this.onUpdateMessage(previousId, id, {
                        folderId: this._draft.folderId
                    });
                }
            },
            hasDraft: function() {
                return !!this._draft.id;
            },
            _removeDraft: function() {
                if (this._draftDeferred) {
                    var that = this, service = this._service, moveToTrash = function() {
                        that._moveDraftToTrash(service, that.getDraft().id);
                    };
                    this._draftDeferred.done(moveToTrash).fail(moveToTrash);
                } else if (this.hasDraft()) {
                    this._moveDraftToTrash(this._service, this.getDraft().id);
                }
            },
            _moveDraftToTrash: function(service, draftId) {
                if (draftId) {
                    var folderList = service.parent().folderList;
                    service.parent().command(Commands.MoveTo, {
                        messageIds: [ draftId ],
                        folderType: FolderType.Trash,
                        srcFolder: folderList.findByType(FolderType.Drafts)
                    }).exec();
                }
            },
            _dataSaved: function() {
                this.set("hasUnsavedData", false);
                this._updateDataSavedTs();
                this._updateTitle();
            },
            _updateDataSavedTs: function() {
                this.set("dataSavedTs", new Date());
            },
            _dataChanged: function() {
                if (!this.sending) {
                    this._saveDraftTimer.restart();
                    this.set("hasUnsavedData", true);
                    this._updateTitle();
                    this._needRefreshAfterClose = true;
                }
            },
            _updateTitle: function() {
                if (this.hasUnsavedData || !this.attachmentSaved) {
                    this._markAsUnsaved();
                } else {
                    this._markAsSaved();
                }
            },
            _markAsUnsaved: function() {
                if (!this._titleSet) {
                    this._title(this._oldTitle + " *");
                    this._titleSet = true;
                }
            },
            _markAsSaved: function() {
                this._title(this._oldTitle);
                this._titleSet = false;
            },
            _close: function() {
                this.trigger(CLOSE);
            },
            _title: function(value) {
                var wnd = this._wnd;
                if (!arguments.length) {
                    return wnd ? wnd.title() : null;
                }
                if (wnd) {
                    wnd.title(value);
                }
            },
            _createMsg: function(withCopyAttachments) {
                var msg = {
                    to: this._normalizeAddresses(this.msg.to),
                    cc: this._normalizeAddresses(this.msg.cc),
                    bcc: this._normalizeAddresses(this.msg.bcc),
                    subject: this.msg.subject,
                    body: this._prepareBody(this.msg.body),
                    isHtml: true,
                    highPriority: this.isSettingChecked("highPriority"),
                    confirmRead: this.isSettingChecked("confirmRead"),
                    confirmDelivery: this.isSettingChecked("confirmDelivery")
                };
                if (withCopyAttachments) {
                    this._addCopyAttachments(msg, this.msg.copyAttachments, this.msg.attachments);
                }
                return msg;
            },
            _addCopyAttachments: function(msg, copyInfo, attachments) {
                if (copyInfo && attachments && attachments.length > 0) {
                    var fileIds = attachments.map(function(attachment) {
                        return attachment.FileId;
                    }).filter(notNullFilter);
                    if (fileIds.length > 0) {
                        msg.copyAttachments = {
                            folderId: copyInfo.folderId,
                            messageId: copyInfo.messageId,
                            files: fileIds
                        };
                    }
                }
            },
            _prepareBody: function(body) {
                var draftOpt = this._options.draft;
                return body ? kendo.format(draftOpt.bodyWrap, body) : null;
            },
            _normalizeAddresses: function(addresses) {
                addresses = addresses || [];
                return addresses.map(function(item) {
                    return {
                        Name: item.Name === item.Address ? null : item.Name,
                        Address: item.Address
                    };
                });
            },
            _prepareAddresses: function(addresses) {
                addresses = addresses || [];
                return addresses.map(function(item) {
                    return {
                        Name: !item.Name ? item.Address : item.Name,
                        Address: item.Address
                    };
                });
            },
            _prepareMessageData: function(messageData) {
                if (messageData) {
                    messageData.to = this._prepareAddresses(messageData.to);
                    messageData.cc = this._prepareAddresses(messageData.cc);
                    messageData.bcc = this._prepareAddresses(messageData.bcc);
                }
            },
            _createContactDataSource: function(initData, options) {
                var contactOpt = options.contact;
                if (contactOpt.readListUrl) {
                    var ds = new ContactListDataSource(contactOpt.readListUrl, contactOpt.ajaxOptions.withCredentials);
                    if (initData.length > 0) {
                        ds.addToLocalStorage(ds.createRawItems(initData));
                    }
                    return ds;
                } else {
                    return new DataSource({
                        data: initData
                    });
                }
            },
            onUpdateMessage: function(oldId, newId, filter) {
                this._service.broadcast(UPDATE_MESSAGE, {
                    oldMessageId: oldId,
                    newMessageId: newId,
                    filter: filter
                });
            },
            onRemoveMessage: function(messageId, filter) {
                this._service.broadcast(REMOVE_MESSAGE, {
                    messageId: messageId,
                    filter: filter
                });
            },
            _refreshMessages: function() {
                var that = this;
                var runCommand = function() {
                    if (that.hasDraft()) {
                        var draft = that.getDraft();
                        that._service.command(Commands.RefreshMessages, {
                            folderId: draft.folderId,
                            messageId: draft.id
                        }).exec();
                    }
                };
                if (this._draftDeferred) {
                    this._draftDeferred.done(runCommand).fail(runCommand);
                } else {
                    runCommand();
                }
            },
            destroy: function() {
                this._remEditorEvents(this._editor);
                this.cancelSaveDraftTimer();
                this.attachments.dispose();
                this.msg.unbind();
                this._service.removeBroadcast(BroadcastEvents.COMPOSE_WND_FOCUS, this.onBroadcastComposeWndFocus);
                this.msg = this.attachments = null;
                this._wnd = null;
                this._editor = null;
                this._draftDeferred = null;
                this._saveDraftTimer = null;
            }
        });
        var SettingsViewModel = ObservableObject.extend({
            init: function(options) {
                ObservableObject.fn.init.call(this);
                this._options = options;
                this.set("signature", "");
            },
            fetch: function() {
                var service = this.parent().service, that = this;
                return service.getSettings().done(function(req) {
                    that._update(storage(STORAGE_NS).get(SETTINGS_KEY));
                    if (req.response && !that.signature) {
                        that.set("signature", req.response.MailSignature);
                    }
                });
            },
            _update: function(data) {
                var that = this;
                data = data || {};
                Object.keys(data).forEach(function(key) {
                    that.set(key, data[key]);
                });
            },
            shouldSerialize: function(field) {
                return field !== "_options" && ObservableObject.fn.shouldSerialize.call(this, field);
            }
        });
        var SettingsDlgViewModel = ObservableObject.extend({
            init: function(settings, service) {
                ObservableObject.fn.init.call(this);
                this._service = service;
                this._storage = storage(STORAGE_NS);
                this._wnd = null;
                this._editor = null;
                this.dialogResult = false;
                this.set("settings", settings);
                this._notifyEditorChangedHandler = kendo.throttle(proxy(this._notifyEditorChanged, this), 1e3);
            },
            initWnd: function(wnd) {
                this._wnd = wnd;
                this._editor = this._findEditor();
            },
            save: function() {
                this._storage.open();
                this._storage.set(SETTINGS_KEY, this.settings.toJSON());
                this._storage.save(true);
            },
            onEditorKeyup: function() {
                this._notifyEditorChangedHandler();
            },
            onOK: function() {
                this.save();
                this.dialogResult = true;
            },
            onCancel: function() {
                this.dialogResult = false;
            },
            onClose: function() {
                this.destroy();
            },
            close: function() {
                this.trigger(CLOSE);
            },
            _notifyEditorChanged: function() {
                if (this._editor) {
                    this._editor.trigger(CHANGE);
                }
            },
            _findEditor: function() {
                return this._wnd ? this._wnd.element.find(EDITOR_SELECTOR).data(EDITOR_WIDGET) : null;
            },
            destroy: function() {
                this._wnd = null;
                this._editor = null;
            }
        });
        var ToolbarViewModel = ObservableObject.extend({
            init: function(options) {
                ObservableObject.fn.init.call(this);
                this._options = options;
                this.compose = proxy(this.compose, this);
                this.refresh = proxy(this.refresh, this);
                this.settings = proxy(this.settings, this);
            },
            compose: function() {
                var command = this.parent().command(Commands.Compose);
                command.exec();
            },
            refresh: function() {
                var command = this.parent().command(Commands.Refresh);
                command.exec();
            },
            settings: function() {
                var parent = this.parent();
                var command = parent.command(Commands.EditSettings, {
                    settings: parent.settings
                });
                command.exec();
            }
        });
        var MailServiceViewModel = ObservableObject.extend({
            init: function(options) {
                ObservableObject.fn.init.call(this);
                this._options = options;
                this._saveDraftResult = proxy(this._saveDraftResult, this);
            },
            _getAccountId: function() {
                return this.parent().account.getCurrentId();
            },
            _getFolderId: function() {
                var folder = this.parent().folderList.getCurrent();
                return folder ? folder.id : null;
            },
            command: function(commandName, commandArgs) {
                return this.parent().command(commandName, commandArgs);
            },
            broadcast: function(eventName, args) {
                this.parent().broadcast(eventName, args);
            },
            onBroadcast: function(eventName, handler) {
                this.parent().broadcaster.bind(eventName, handler);
            },
            removeBroadcast: function(eventName, handler) {
                this.parent().broadcaster.unbind(eventName, handler);
            },
            getAuth: function() {
                return this.parent().auth;
            },
            getAuthToken: function() {
                var auth = this.getAuth();
                return auth.enabled() && auth.token ? auth.token : null;
            },
            getAccountId: function() {
                return this._getAccountId();
            },
            getFolderId: function() {
                return this._getFolderId();
            },
            addFlag: function(messageIds) {
                return this._setFlag(messageIds, true);
            },
            removeFlag: function(messageIds) {
                return this._setFlag(messageIds, false);
            },
            moveTo: function(folderId, messageIds, srcFolderId) {
                var opt = this._options.message, url = kendo.format(opt.moveToUrl, this._getAccountId(), srcFolderId || this._getFolderId());
                return this._resourcePost(url, {
                    Messages: messageIds,
                    Value: folderId
                }, this.getAuthToken());
            },
            moveAll: function(srcFolderId, destFolderId) {
                var opt = this._options.message, url = kendo.format(opt.moveAllUrl, this._getAccountId(), srcFolderId), data = {
                    Value: destFolderId
                };
                return this._resourcePost(url, data, this.getAuthToken());
            },
            markAsReadAll: function(folderId) {
                var opt = this._options.message, url = kendo.format(opt.setReadAllUrl, this._getAccountId(), folderId), data = {
                    Value: true
                };
                return this._resourcePost(url, data, this.getAuthToken());
            },
            markAsRead: function(messageIds) {
                return this._setRead(messageIds, true);
            },
            markAsUnread: function(messageIds) {
                return this._setRead(messageIds, false);
            },
            deleteAll: function(folderId) {
                var opt = this._options.message, url = kendo.format(opt.deleteAllUrl, this._getAccountId(), folderId);
                return this._resourceDelete(url, null, this.getAuthToken());
            },
            deleteItems: function(folderId, messageIds) {
                var opt = this._options.message, url = kendo.format(opt.deleteItemsUrl, this._getAccountId(), folderId), data = {
                    Messages: messageIds
                };
                return this._resourceDelete(url, data, this.getAuthToken());
            },
            saveDraft: function(messageId, folderId, messageData) {
                if (!messageId) {
                    return this.createDraft(messageData);
                } else {
                    return this.updateDraft(messageId, folderId, messageData);
                }
            },
            createDraft: function(messageData) {
                var opt = this._options.draft, url = kendo.format(opt.createUrl, this._getAccountId()), msg = this._createMsg(messageData);
                return this._resourcePost(url, msg, this.getAuthToken()).then(this._saveDraftResult);
            },
            updateDraft: function(messageId, folderId, messageData) {
                var opt = this._options.draft, url = kendo.format(opt.updateUrl, this._getAccountId(), folderId, messageId), msg = this._createMsg(messageData);
                return this._resourcePut(url, msg, this.getAuthToken()).then(this._saveDraftResult);
            },
            _saveDraftResult: function(request) {
                var resp = request.response || {};
                return {
                    id: resp.UniqueId || null,
                    changedDate: resp.Date || null,
                    folderId: resp.FolderId || null,
                    files: resp.Attachments || []
                };
            },
            sendDraft: function(draftId, folderId) {
                var opt = this._options.draft, url = kendo.format(opt.sendUrl, this._getAccountId(), folderId, draftId);
                return this._resourcePost(url, null, this.getAuthToken());
            },
            createFolder: function(name, parentId) {
                var opt = this._options.folder, url = kendo.format(opt.createUrl, this._getAccountId()), data = {
                    Name: name,
                    ParentId: parentId
                };
                return this._resourcePost(url, data, this.getAuthToken());
            },
            updateFolder: function(folderId, name, parentId) {
                var opt = this._options.folder, url = kendo.format(opt.updateUrl, this._getAccountId(), folderId), data = {
                    Name: name,
                    ParentId: parentId
                };
                return this._resourcePost(url, data, this.getAuthToken());
            },
            deleteFolder: function(folderId) {
                var opt = this._options.folder, url = kendo.format(opt.deleteUrl, this._getAccountId(), folderId);
                return this._resourceDelete(url, null, this.getAuthToken());
            },
            getFolders: function() {
                var opt = this._options.folder, url = kendo.format(opt.readUrl, this._getAccountId());
                return this._resourceGet(url, null, this.getAuthToken());
            },
            getSettings: function() {
                var opt = this._options.settings;
                return this._resourceGet(opt.readUrl, null, null, opt.ajaxOptions.withCredentials);
            },
            makeUploadFileUrl: function(draftId, folderId) {
                var opt = this._options.uploader, url = kendo.format(opt.saveUrl, this._getAccountId(), folderId, draftId, this.getAuthToken());
                return url;
            },
            makeRemoveFileUrl: function(draftId, fileId, folderId) {
                var opt = this._options.uploader, url = kendo.format(opt.removeUrl, this._getAccountId(), folderId, draftId, fileId, this.getAuthToken());
                return url;
            },
            makeContactImageUrl: function(name, email) {
                var opt = this._options.contact, url = opt.imageUrl ? kendo.format(opt.imageUrl, encodeURIComponent(name), encodeURIComponent(email)) : null;
                return url;
            },
            makeContactPreviewUrl: function(email) {
                var opt = this._options.contact, url = opt.previewUrl ? kendo.format(opt.previewUrl, encodeURIComponent(email)) : null;
                return url;
            },
            getMessageDetails: function(messageId) {
                var opt = this._options.message, readItemUrl = opt.readItemUrl, ds = new MessageDetailsDataSource(this.getAuth(), readItemUrl);
                return ds.read({
                    accountId: this.getAccountId(),
                    folderId: this.getFolderId(),
                    messageId: messageId
                }).then(function() {
                    return first(ds.data());
                });
            },
            _setRead: function(messageIds, value) {
                var opt = this._options.message, url = kendo.format(opt.setReadUrl, this._getAccountId(), this._getFolderId());
                return this._resourcePost(url, {
                    Messages: messageIds,
                    Value: value
                }, this.getAuthToken());
            },
            _setFlag: function(messageIds, value) {
                var opt = this._options.message, url = kendo.format(opt.setFlagUrl, this._getAccountId(), this._getFolderId());
                return this._resourcePost(url, {
                    Messages: messageIds,
                    Value: value
                }, this.getAuthToken());
            },
            _createMsg: function(rawData) {
                return {
                    To: rawData.to,
                    Cc: rawData.cc,
                    Bcc: rawData.bcc,
                    Subject: rawData.subject,
                    Body: rawData.body,
                    IsHtmlBody: rawData.isHtml,
                    HighPriority: !!rawData.highPriority,
                    ConfirmRead: !!rawData.confirmRead,
                    ConfirmDelivery: !!rawData.confirmDelivery,
                    MessageAttachments: rawData.copyAttachments ? {
                        FolderId: rawData.copyAttachments.folderId,
                        UniqueId: rawData.copyAttachments.messageId,
                        Files: rawData.copyAttachments.files
                    } : null
                };
            },
            _resourceGet: function(url, data, bearerToken, withCredentials) {
                return this._resourceRequest(get, url, data, bearerToken, withCredentials);
            },
            _resourcePost: function(url, data, bearerToken, withCredentials) {
                return this._resourceRequest(post, url, data, bearerToken, withCredentials);
            },
            _resourceDelete: function(url, data, bearerToken, withCredentials) {
                return this._resourceRequest(del, url, data, bearerToken, withCredentials);
            },
            _resourcePut: function(url, data, bearerToken, withCredentials) {
                return this._resourceRequest(put, url, data, bearerToken, withCredentials);
            },
            _resourceRequest: function(requestFunc, url, data, bearerToken, withCredentials) {
                var requestArgs = arraySlice.call(arguments, 1);
                if (!bearerToken) {
                    return requestFunc.apply(null, requestArgs);
                }
                var defer = $.Deferred();
                requestFunc.apply(null, requestArgs).done(defer.resolve).fail(this._makeUnauthorizedHandler(defer, requestFunc, [ url, data, null, withCredentials ]));
                return defer;
            },
            _makeUnauthorizedHandler: function(defer, requestFunc, requestArgs) {
                var that = this;
                return function(error) {
                    if (that.getAuth().enabled() && error.jqXHR && error.jqXHR.status === HttpCodes.UNAUTHORIZED) {
                        that.getAuth().refreshToken().done(function(token) {
                            try {
                                requestArgs[2] = token;
                                requestFunc.apply(null, requestArgs).done(defer.resolve).fail(defer.reject);
                            } catch (e) {
                                traceError(e.toString());
                                defer.reject(e);
                            }
                            return token;
                        }).fail(function(e) {
                            defer.reject(e);
                            return e;
                        });
                    } else {
                        defer.reject(error);
                    }
                };
            }
        });
        var MailClientViewModel = ObservableObject.extend({
            init: function(options) {
                ObservableObject.fn.init.call(this);
                this.options = options;
                this.auth = new Authenticator(this);
                this.broadcaster = new Broadcaster();
                this.eventManager = new EventManager(this);
                this.runningCommands = new RunningCommands();
                this.commandFactory = makeCommandFactory(this, this.runningCommands);
                this.set("inProgress", false);
                this.set("disabled", false);
                this._inProgressCount = 0;
                this._hideErrorTimer = timer(500, proxy(this.hideError, this));
                this._initViewModels(options);
                if (options.autoLoad) {
                    this._load();
                }
            },
            _initViewModels: function(options) {
                this.set("account", new AccountViewModel(this.auth, options));
                this.set("folderList", new FolderListViewModel(this.auth, this.broadcaster, options));
                this.set("messageList", new MessageListViewModel(this.auth, this.broadcaster, options));
                this.set("messageDetails", new MessageDetailsViewModel(this.auth, this.broadcaster, options));
                this.set("toolbar", new ToolbarViewModel(options));
                this.set("service", new MailServiceViewModel(options));
                this.set("contactPreview", new ContactPreviewViewModel(options));
                this.set("settings", new SettingsViewModel(options));
            },
            load: function() {
                this._load();
            },
            _load: function() {
                var that = this;
                if (this.auth.enabled()) {
                    this.set("disabled", true);
                    this.auth.getToken().done(function() {
                        that.set("disabled", false);
                        that._fetch();
                    });
                } else {
                    this._fetch();
                }
            },
            _fetch: function() {
                var that = this;
                this.set("disabled", true);
                this.settings.fetch().done(function() {
                    that.account.fetch().then(function() {
                        that.set("disabled", false);
                        that.account.switchToCurrent();
                        that.eventManager.start();
                    });
                });
            },
            showProgress: function() {
                this._inProgressCount++;
                this.set("inProgress", true);
            },
            hideProgress: function() {
                this._inProgressCount--;
                if (this._inProgressCount <= 0) {
                    this.set("inProgress", false);
                    this._inProgressCount = 0;
                }
            },
            showError: function(message) {
                traceError(message);
                this.trigger(SHOW_ERROR, {
                    message: message
                });
                this._hideErrorTimer.restart();
            },
            hideError: function() {
                this._hideErrorTimer.cancel();
                this.trigger(HIDE_ERROR);
            },
            broadcast: function(eventName, args) {
                this.broadcaster.trigger(eventName, args);
            },
            command: function(commandName, commandArgs) {
                return this.commandFactory(commandName, commandArgs);
            }
        });
        extend(mailclientNS, {
            AccountViewModel: AccountViewModel,
            FolderListViewModel: FolderListViewModel,
            MessageListViewModel: MessageListViewModel,
            MessageDetailsViewModel: MessageDetailsViewModel,
            SendMessageDataViewModel: SendMessageDataViewModel,
            AttachmentUploadViewModel: AttachmentUploadViewModel,
            NewMessageDlgViewModel: NewMessageDlgViewModel,
            ToolbarViewModel: ToolbarViewModel,
            MailServiceViewModel: MailServiceViewModel,
            MailClientViewModel: MailClientViewModel,
            ContactPreviewViewModel: ContactPreviewViewModel,
            SettingsDlgViewModel: SettingsDlgViewModel,
            SettingsViewModel: SettingsViewModel
        });
    })(window.kendo.jQuery);
    return window.kendo;
}, typeof define == "function" && define.amd ? define : function(a1, a2, a3) {
    "use strict";
    (a3 || a2)();
});

(function(f, define) {
    "use strict";
    define("kendo.mailclient", [ "kendo.mailclient.ns", "kendo.mailclient.view-models" ], f);
})(function() {
    "use strict";
    (function($, undefined) {
        var kendo = window.kendo, ui = kendo.ui, Widget = ui.Widget, mailclientNS = ui.mailclient, Model = kendo.data.Model, proxy = $.proxy, NS = ".kendoMailClient", MailClientViewModel = mailclientNS.MailClientViewModel, SCRIPT = "SCRIPT", LISTVIEW_SELECT_ITEM = "listViewSelectItem", LISTVIEW_CLEAR_SELECTION = "listViewClearSelection", LISTVIEW_SELECT_ELEMENT = "listViewSelectElement", FIND_MESSAGE_IFRAME = "findMessageIFrame", FOLDER = "folder", MESSAGE = "message", SHOW_ERROR = "showError", HIDE_ERROR = "hideError", DOCUMENT_CLICK = "documentClick", LISTVIEW_WIDGET = "kendoMailClientListView", DEFAULT_BODY_WRAP = "<html><body>{0}</body></html>";
        var MailClient = Widget.extend({
            init: function(element, options) {
                Widget.fn.init.call(this, element, options);
                this._element();
                this._viewModel();
                this._view();
                this._document();
                this._folderListView = null;
                this._messageListView = null;
                kendo.notify(this);
            },
            options: {
                name: "MailClient",
                template: "",
                templateId: "",
                autoLoad: true,
                account: {
                    readUrl: ""
                },
                folder: {
                    readUrl: "",
                    createUrl: "",
                    updateUrl: "",
                    deleteUrl: ""
                },
                message: {
                    readUrl: "",
                    readItemUrl: "",
                    attachmentUrl: "",
                    setReadUrl: "",
                    setFlagUrl: "",
                    moveToUrl: "",
                    moveAllUrl: "",
                    setReadAllUrl: "",
                    deleteAllUrl: "",
                    deleteItemsUrl: "",
                    markAsReadTimeout: 2e3,
                    pageSize: 20
                },
                draft: {
                    sendUrl: "",
                    createUrl: "",
                    updateUrl: "",
                    saveTimeout: 2e3,
                    notifyChangedTimeout: 500,
                    bodyWrap: DEFAULT_BODY_WRAP
                },
                uploader: {
                    saveUrl: "",
                    readUrl: "",
                    removeUrl: ""
                },
                dialogs: {
                    newMessage: {
                        template: "",
                        windowId: ""
                    },
                    editSettings: {
                        template: "",
                        windowId: ""
                    }
                },
                auth: {
                    token: {
                        createUrl: "",
                        field: "",
                        method: "POST",
                        withCredentials: false
                    },
                    tokenValidation: {
                        readUrl: "",
                        method: "GET",
                        withCredentials: false
                    }
                },
                contact: {
                    imageUrl: "",
                    readListUrl: "",
                    previewUrl: "",
                    ajaxOptions: {
                        withCredentials: false
                    }
                },
                events: {
                    polling: false,
                    pollingInterval: 2e4
                },
                settings: {
                    readUrl: "",
                    ajaxOptions: {
                        withCredentials: false
                    }
                },
                localization: {
                    unsavedCloseConfirm: "You have unsaved data. Close a window?",
                    unsavedDataConfirm: "You have unsaved data.",
                    deleteFolderConfirm: 'Are you sure you want to permanently delete the "{0}" folder?',
                    deleteFolderToTrashConfirm: 'Are you sure you want to permanently delete the "{0}" folder to trash?',
                    deleteAllToTrashConfirm: "Are you sure you want delete all messages to trash?",
                    deleteAllMessagesConfirm: 'Are you sure you want to permanently delete all messages in "{0}" folder?',
                    clearTrashFolderConfirm: "Clear trash folder?",
                    deleteMessageConfirm: 'Are you sure you want to permanently delete "{0}" message?',
                    deleteMessagesConfirm: 'Are you sure you want to permanently delete "{0}" messages?',
                    deleteFolderConfirmDescr: "",
                    deleteAllMessagesConfirmDescr: "",
                    clearTrashFolderConfirmDescr: "",
                    deleteMessageConfirmDescr: "",
                    deleteMessagesConfirmDescr: "",
                    noSubject: "No subject",
                    draftTitle: "Draft",
                    folders: {
                        drafts: "Drafts",
                        sent: "Sent",
                        junk: "Junk",
                        trash: "Trash",
                        inbox: "INBOX"
                    },
                    dialogs: {
                        okText: "Ok",
                        cancel: "Cancel"
                    },
                    usavedDataDialog: {
                        okText: "Save draft",
                        cancel: "Delete draft"
                    }
                }
            },
            events: [],
            _element: function() {
                this.element.addClass("mail-client").attr("role", "mailclient");
            },
            _viewModel: function() {
                this.viewModel = new MailClientViewModel(this.options);
                this.viewModel.bind(LISTVIEW_SELECT_ITEM, proxy(this._listViewSelectItem, this));
                this.viewModel.bind(LISTVIEW_SELECT_ELEMENT, proxy(this._listViewSelectElement, this));
                this.viewModel.bind(LISTVIEW_CLEAR_SELECTION, proxy(this._listViewClearSelection, this));
                this.viewModel.bind(FIND_MESSAGE_IFRAME, proxy(this._findMessageIFrame, this));
                this.viewModel.bind(SHOW_ERROR, proxy(this._showError, this));
                this.viewModel.bind(HIDE_ERROR, proxy(this._hideError, this));
            },
            _view: function() {
                var options = this.options, template = options.templateId;
                if (!template) {
                    var firstChild = this.element.children()[0];
                    if (firstChild && firstChild.tagName === SCRIPT) {
                        template = firstChild;
                    }
                }
                this.view = new kendo.View(template, {
                    model: this.viewModel,
                    evalTemplate: false,
                    wrap: false
                });
                this.view.render(this.element);
            },
            _document: function() {
                $(window.document).on("click" + NS, proxy(this._handleDocumentClick, this));
            },
            _handleDocumentClick: function() {
                this.viewModel.trigger(DOCUMENT_CLICK);
            },
            _listViewSelectItem: function(e) {
                if (e.type === FOLDER && this._getFolderListView()) {
                    if (e.item instanceof Model) {
                        this._getFolderListView().element.find('[data-uid="' + e.item.uid + '"]').addClass("k-state-selected");
                    }
                } else if (e.type === MESSAGE && this._getMessageListView()) {
                    if (e.item instanceof Model) {
                        this._getMessageListView().element.find('[data-uid="' + e.item.uid + '"]').addClass("k-state-selected");
                    }
                }
            },
            _listViewSelectElement: function(e) {
                if (e.type === FOLDER && e.item instanceof Element) {
                    var listView = $(e.item).closest(".k-listview").data(LISTVIEW_WIDGET);
                    if (listView) {
                        var rootListView = this._getFolderListView();
                        rootListView.element.find(".k-listview").each(function() {
                            var childListView = $(this).data(LISTVIEW_WIDGET);
                            if (childListView && childListView.select().length > 0) {
                                childListView.selectable.clear();
                            }
                        });
                        if (rootListView.select().length > 0) {
                            rootListView.selectable.clear();
                        }
                        listView.select(e.item);
                    }
                }
            },
            _listViewClearSelection: function(e) {
                if (e.type === MESSAGE && this._getMessageListView()) {
                    this._getMessageListView().clearSelection();
                } else if (e.type === FOLDER && this._getFolderListView()) {
                    this._getFolderListView().clearSelection();
                }
            },
            _getFolderListView: function() {
                if (!this._folderListView) {
                    this._folderListView = this.element.find(".folders-list.k-listview").first().data(LISTVIEW_WIDGET);
                }
                return this._folderListView;
            },
            _getMessageListView: function() {
                if (!this._messageListView) {
                    this._messageListView = this.element.find(".letters-list.k-listview").data(LISTVIEW_WIDGET);
                }
                return this._messageListView;
            },
            _findMessageIFrame: function(e) {
                var iframe = this.element.find(".letter-view .letter__body > iframe");
                e.result = iframe.length > 0 ? iframe[0] : null;
            },
            _showError: function() {
                this.element.find("> .mail-client__error").addClass("show");
            },
            _hideError: function() {
                this.element.find("> .mail-client__error").removeClass("show");
            },
            destroy: function() {
                Widget.fn.destroy.call(this);
                this.view.destroy();
                $(window.document).off(NS);
                this.element = null;
                this.viewModel = null;
                this.view = null;
            }
        });
        kendo.ui.plugin(MailClient);
    })(window.kendo.jQuery);
    return window.kendo;
}, typeof define == "function" && define.amd ? define : function(a1, a2, a3) {
    "use strict";
    (a3 || a2)();
});

(function(f, define) {
    "use strict";
    define("kendo.mailclient.iframe", [ "kendo.mailclient.ns", "kendo.mailclient.utils" ], f);
})(function() {
    "use strict";
    (function($, undefined) {
        var kendo = window.kendo, ui = kendo.ui, Widget = ui.Widget, proxy = $.proxy, mailclientNS = ui.mailclient, binders = kendo.data.binders, Binder = kendo.data.Binder, createIframe = mailclientNS.createIframe, NS = ".kendoMailClientIFrame", CONTENT_UPDATED = "contentUpdated", UPDATED = "updated";
        var MailClientIFrame = Widget.extend({
            init: function(element, options) {
                Widget.fn.init.call(this, element, options);
                this._element();
                this._content();
                this._iframeResizer();
                kendo.notify(this);
            },
            options: {
                name: "MailClientIFrame",
                autoResize: false,
                scrolling: true
            },
            events: [ UPDATED ],
            _element: function() {
                this.element.addClass("mail-client-iframe").attr("role", "mailclientiframe").attr("marginwidth", "0").attr("marginheight", "0").attr("frameborder", "0");
                if (this.options.scrolling) {
                    this.element.attr("scrolling", "yes").css("overflow", "auto");
                } else {
                    this.element.attr("scrolling", "no").css("overflow", "hidden");
                }
            },
            _content: function() {
                this.element.on(CONTENT_UPDATED + NS, proxy(this._contentUpdated, this));
            },
            _iframeResizer: function() {
                if (this.options.autoResize) {
                    this._defaultResizer();
                }
            },
            _defaultResizer: function() {
                this._resizer = createIframe(this.element[0], this.options.scrolling);
            },
            _contentUpdated: function() {
                if (this._resizer) {
                    this._resizer.resize();
                }
                this.trigger(UPDATED);
            },
            destroy: function() {
                Widget.fn.destroy.call(this);
                if (this._resizer) {
                    this._resizer.destroy();
                    this._resizer = null;
                }
                this.element.off(NS);
                this.element = null;
            }
        });
        binders.widget.mailclientiframe = {
            html: Binder.extend({
                init: function(widget, bindings, options) {
                    Binder.fn.init.call(this, widget.element[0], bindings, options);
                    this.widget = widget;
                },
                refresh: function() {
                    var element = this.widget.element[0], doc = element.contentWindow ? element.contentWindow.document : element.contentDocument, html = this.bindings.html.get();
                    doc.open();
                    doc.write(html);
                    doc.close();
                    this._notifyContentUpdated();
                },
                _notifyContentUpdated: function() {
                    this.widget.element.trigger(CONTENT_UPDATED + NS);
                }
            })
        };
        kendo.ui.plugin(MailClientIFrame);
    })(window.kendo.jQuery);
    return window.kendo;
}, typeof define == "function" && define.amd ? define : function(a1, a2, a3) {
    "use strict";
    (a3 || a2)();
});

(function(f, define) {
    "use strict";
    define("kendo.mailclient.multitags", [], f);
})(function() {
    "use strict";
    (function($, undefined) {
        var kendo = window.kendo, ui = kendo.ui, proxy = $.proxy, keys = kendo.keys, binders = kendo.data.binders, isArray = Array.isArray, KEYDOWN = "keydown", CHANGE = "change", OPEN = "open", REQUEST_END = "requestEnd", SYNC = "sync", CREATE = "create";
        var MailClientMultiTags = ui.MultiSelect.extend({
            init: function(element, options) {
                ui.MultiSelect.fn.init.call(this, element, options);
                this._enter();
                this._onDataFetchingHandler = proxy(this._onDataFetching, this);
                this._onDataFetchSuccessHandler = proxy(this._onDataFetchSuccess, this);
                this._onDataFetchErrorHandler = proxy(this._onDataFetchError, this);
                this._dataSourceEvents();
                this._progressDataTemplate();
            },
            options: {
                name: "MailClientMultiTags",
                progressDataTemplate: ""
            },
            _progressDataTemplate: function() {
                var template = this.options.progressDataTemplate;
                this.progressDataTemplate = typeof template !== "function" ? kendo.template(template) : template;
            },
            setDataSource: function(dataSource) {
                ui.MultiSelect.fn.setDataSource.call(this, dataSource);
                this._dataSourceEvents();
            },
            _dataSourceEvents: function() {
                this.dataSource.unbind("dataFetching", this._onDataFetchingHandler);
                this.dataSource.unbind("dataFetchSuccess", this._onDataFetchSuccessHandler);
                this.dataSource.unbind("dataFetchError", this._onDataFetchErrorHandler);
                this.dataSource.bind("dataFetching", this._onDataFetchingHandler);
                this.dataSource.bind("dataFetchSuccess", this._onDataFetchSuccessHandler);
                this.dataSource.bind("dataFetchError", this._onDataFetchErrorHandler);
            },
            _onDataFetching: function(e) {
                this._updateProgress(e, true);
            },
            _onDataFetchSuccess: function(e) {
                this._updateProgress(e, false);
            },
            _onDataFetchError: function(e) {
                this._updateProgress(e, false);
            },
            _updateProgress: function(e, isEnabled) {
                if (isEnabled) {
                    this._showBusy(e);
                } else {
                    this._hideBusy(e);
                }
                clearTimeout(this._renderProgressDataTimeout);
                clearTimeout(this._renderNoDataTimeout);
                if (this.listView._view && this.listView._view.length === 0) {
                    if (isEnabled) {
                        this._renderProgressDataTimeout = setTimeout(proxy(this._renderProgressData, this), 100);
                    } else {
                        this._renderNoDataTimeout = setTimeout(proxy(this._renderNoData, this), 100);
                    }
                }
            },
            _renderProgressData: function() {
                var noData = this.noData;
                if (!noData) {
                    return;
                }
                this._angularElement(noData, "cleanup");
                noData.children(":first").html(this.progressDataTemplate({
                    instance: this
                }));
                this._angularElement(noData, "compile");
            },
            _enter: function() {
                this.input.on(KEYDOWN + this.ns, proxy(this._inputEnter, this));
            },
            _inputEnter: function(e) {
                if (e.keyCode === keys.ENTER) {
                    this._addVal(this.input.val());
                }
            },
            _inputFocusout: function() {
                var val = this.input.val();
                ui.MultiSelect.fn._inputFocusout.call(this);
                this._addVal(val);
            },
            _addVal: function(val) {
                var that = this, ds = this.dataSource, opt = this.options, isAdd = val !== "" && ds && opt.dataTextField && opt.dataValueField;
                if (!isAdd) {
                    return;
                }
                var item = ds.data().filter(function(item) {
                    return item[opt.dataValueField] === val;
                });
                if (item.length > 0) {
                    this._appendValue(item[0][opt.dataValueField]);
                    return;
                }
                ds.add(this._createItem(val));
                ds.one(REQUEST_END, function(args) {
                    if (args.type !== CREATE) {
                        return;
                    }
                    var resp = args.response, modelRawField = that._modelRawField(opt.dataValueField), rawData = resp && isArray(resp) ? resp[0] : resp, newValue = rawData && rawData[modelRawField] ? rawData[modelRawField] : null;
                    if (!newValue) {
                        return;
                    }
                    ds.one(SYNC, function() {
                        that._appendValue(newValue);
                    });
                });
                ds.sync();
            },
            _appendValue: function(value) {
                this.value(this.value().concat([ value ]));
            },
            _listChange: function(e) {
                ui.MultiSelect.fn._listChange.call(this, e);
                if (e.added && e.added.length > 0 || e.removed && e.removed.length > 0) {
                    this.trigger(CHANGE);
                }
            },
            _modelRawField: function(field) {
                var model = this.dataSource.reader.model;
                if (model) {
                    var fieldSpec = model.fields[field];
                    if (fieldSpec) {
                        return fieldSpec.field;
                    }
                }
                return field;
            },
            _createItem: function(val) {
                var item = {}, opt = this.options;
                item[opt.dataTextField] = val;
                item[opt.dataValueField] = val;
                return item;
            },
            _onOpen: function(e) {
                var listView = e.sender.listView;
                if (listView._view && listView._view.length === 0) {
                    e.preventDefault();
                }
            }
        });
        binders.widget.mailclientmultitags = binders.widget.multiselect;
        kendo.ui.plugin(MailClientMultiTags);
    })(window.kendo.jQuery);
    return window.kendo;
}, typeof define == "function" && define.amd ? define : function(a1, a2, a3) {
    "use strict";
    (a3 || a2)();
});

(function(f, define) {
    "use strict";
    define("kendo.mailclient.tooltip", [], f);
})(function() {
    "use strict";
    (function($, undefined) {
        var kendo = window.kendo, ui = kendo.ui, proxy = $.proxy, NS = ".kendoMailClientTooltip", MOUSEENTER = "mouseenter", MOUSELEAVE = "mouseleave", TRUE = "true", OPEN = "open", INVERT_ATTR = "data-tooltip-invert", TITLE_ATTR = "data-tooltip-title", INVERT_TITLE_ATTR = "data-tooltip-invert-title";
        var MailClientTooltip = ui.Tooltip.extend({
            init: function(element, options) {
                ui.Tooltip.fn.init.call(this, element, options);
                this._targetContainer();
                this._wrapContent();
            },
            options: {
                name: "MailClientTooltip",
                target: ""
            },
            _targetContainer: function() {
                if (this.options.target) {
                    var target = $(this.options.target);
                    target.on(this.options.showOn + NS, this.options.filter, proxy(this._showOn, this)).on(MOUSEENTER + NS, this.options.filter, proxy(this._mouseenter, this));
                    if (this.options.autoHide) {
                        target.on(MOUSELEAVE + NS, this.options.filter, proxy(this._mouseleave, this));
                    }
                }
            },
            _wrapContent: function() {
                var that = this, contentFn = this.options.content;
                this.options.content = function(e) {
                    var target = $(e.target);
                    if (target.attr(INVERT_ATTR) !== undefined) {
                        if (target.attr(INVERT_ATTR) === TRUE) {
                            return target.attr(INVERT_TITLE_ATTR);
                        } else {
                            return target.attr(TITLE_ATTR);
                        }
                    } else if (typeof contentFn === "function") {
                        return contentFn.call(that, e);
                    }
                };
            },
            _initPopup: function() {
                ui.Tooltip.fn._initPopup.apply(this, arguments);
                this.popup.bind(OPEN, proxy(this._popupOpen, this));
            },
            _popupOpen: function() {
                var target = this.target();
                if (target && target.attr("data-tooltip-invert") !== undefined) {
                    this.refresh();
                }
            },
            destroy: function() {
                if (this.options.target) {
                    $(this.options.target).off(NS);
                }
                ui.Tooltip.fn.destroy.call(this);
            }
        });
        kendo.ui.plugin(MailClientTooltip);
    })(window.kendo.jQuery);
    return window.kendo;
}, typeof define == "function" && define.amd ? define : function(a1, a2, a3) {
    "use strict";
    (a3 || a2)();
});

(function(f, define) {
    "use strict";
    define("kendo.mailclient.upload", [], f);
})(function() {
    "use strict";
    (function($, undefined) {
        var kendo = window.kendo, ui = kendo.ui, FILELIST_CREATED = "fileListCreated", FILELIST_DESTROYED = "fileListDestroyed";
        var MailClientUpload = ui.Upload.extend({
            init: function(element, options) {
                ui.Upload.fn.init.call(this, element, options);
            },
            options: {
                name: "MailClientUpload",
                cancelOnDestroy: true
            },
            events: (ui.Upload.fn.events || []).concat([ FILELIST_CREATED, FILELIST_DESTROYED ]),
            uploadFileByUid: function(uid) {
                var fileEntry = this.findFileByUid(uid);
                if (fileEntry) {
                    var started = this.isFileUploadStarted(fileEntry);
                    var hasValidationErrors = this._filesContainValidationErrors(fileEntry.data("fileNames"));
                    if (!started && !hasValidationErrors) {
                        this._module.performUpload(fileEntry);
                    }
                }
            },
            hasFileByUid: function(uid) {
                return !!this.findFileByUid(uid);
            },
            findFileByUid: function(uid) {
                var fileEntry = $(".k-file[" + kendo.attr("uid") + '="' + uid + '"]', this.wrapper);
                return fileEntry.length > 0 ? fileEntry : null;
            },
            findFileDataByUid: function(uid) {
                var fileEntry = this.findFileByUid(uid);
                return fileEntry.length > 0 ? (fileEntry.data("fileNames") || [])[0] : null;
            },
            clearUploadedFiles: function() {
                var that = this;
                var files = that.wrapper.find(".k-file.k-file-success");
                files.each(function(index, file) {
                    that._removeFileByDomElement(file, false);
                });
            },
            addUploadedFiles: function(files) {
                this._renderInitialFiles(files);
            },
            isFileUploadedByUid: function(uid) {
                var fileEntry = this.findFileByUid(uid);
                return fileEntry ? this.isFileUploaded(fileEntry) : false;
            },
            isFileUploaded: function(fileEntry) {
                return fileEntry.hasClass("k-file-success");
            },
            isFileUploadStarted: function(fileEntry) {
                return fileEntry.is(".k-file-progress, .k-file-success, .k-file-error");
            },
            fileInProgress: function(fileEntry) {
                return fileEntry.hasClass("k-file-progress");
            },
            fileListProgress: function(flag) {
                var fileList = $(".k-upload-files", this.wrapper);
                if (flag) {
                    fileList.addClass("k-upload-progress");
                } else {
                    fileList.removeClass("k-upload-progress");
                }
            },
            isFileList: function() {
                var fileList = $(".k-upload-files", this.wrapper);
                return fileList.length > 0;
            },
            cancelAllUpload: function(force) {
                var that = this;
                var files = that.wrapper.find(".k-file");
                files.each(function(index, file) {
                    file = $(file);
                    if (that.fileInProgress(file)) {
                        that.cancelUpload(file, force);
                    }
                });
            },
            cancelUpload: function(fileEntry, force) {
                if (force) {
                    this._module.onCancel({
                        target: $(fileEntry, this.wrapper)
                    });
                } else {
                    var button = fileEntry.find(".k-upload-action");
                    if (button.length && button.find(".k-i-cancel").length) {
                        this._onFileAction({
                            target: button
                        });
                    }
                }
            },
            _enqueueFile: function(name, data) {
                var fileEntry = ui.Upload.fn._enqueueFile.call(this, name, data);
                if (this.isFileList()) {
                    this.trigger(FILELIST_CREATED);
                }
                return fileEntry;
            },
            _removeFileEntry: function(fileEntry) {
                ui.Upload.fn._removeFileEntry.call(this, fileEntry);
                if (!this.isFileList()) {
                    this.trigger(FILELIST_DESTROYED);
                }
            },
            _onParentFormReset: function() {
                ui.Upload.fn._onParentFormReset.call(this);
                this.trigger(FILELIST_DESTROYED);
            },
            _onFileProgress: function(e, percentComplete) {
                if (percentComplete >= 100) {
                    percentComplete = 99;
                }
                ui.Upload.fn._onFileProgress.call(this, e, percentComplete);
            },
            destroy: function() {
                if (this.options.cancelOnDestroy) {
                    this.cancelAllUpload(true);
                }
                ui.Upload.fn.destroy.call(this);
            }
        });
        kendo.ui.plugin(MailClientUpload);
    })(window.kendo.jQuery);
    return window.kendo;
}, typeof define == "function" && define.amd ? define : function(a1, a2, a3) {
    "use strict";
    (a3 || a2)();
});

(function(f, define) {
    "use strict";
    define("kendo.mailclient.window", [], f);
})(function() {
    "use strict";
    (function($, undefined) {
        var kendo = window.kendo, ui = kendo.ui, proxy = $.proxy, NS = ".MailClientWindow", KWINDOWTITLE = ".k-window-title", KWINDOWTITLEBAR = KWINDOWTITLE + "bar", OPEN = "open";
        var MailClientWindow = ui.Window.extend({
            init: function(element, options) {
                ui.Window.fn.init.call(this, element, options);
                if (!this.options.resizable) {
                    this._maximization();
                }
                this.bind(OPEN, proxy(this._handleSnap, this));
            },
            options: {
                name: "MailClientWindow",
                snapToRightBottom: false,
                maximizeVmargin: 0,
                maximizeHmargin: 0
            },
            _maximization: function() {
                this.wrapper.on("dblclick" + NS, KWINDOWTITLEBAR, proxy(function(e) {
                    if (!$(e.target).closest(".k-window-action").length) {
                        this.toggleMaximization();
                    }
                }, this));
            },
            _handleSnap: function() {
                var opt = this.options, snap = opt.snapToRightBottom, isMaximized = opt.isMaximized;
                if (!snap || isMaximized) {
                    return;
                }
                var position = opt.position, wrapper = this.wrapper, documentWindow = $(window), scrollTop = 0, scrollLeft = 0, newTop, newLeft, rightOffset = 6, bottomOffset = 6, paddingTop = parseInt(wrapper.css("paddingTop"), 10);
                if (!opt.pinned) {
                    scrollTop = documentWindow.scrollTop();
                    scrollLeft = documentWindow.scrollLeft();
                }
                newLeft = scrollLeft + Math.max(0, documentWindow.width() - wrapper.width() - rightOffset);
                newTop = scrollTop + Math.max(0, documentWindow.height() - wrapper.height() - bottomOffset - paddingTop);
                wrapper.css({
                    left: newLeft,
                    top: newTop
                });
                position.top = newTop;
                position.left = newLeft;
            },
            _onDocumentResize: function() {
                var opt = this.options, horizontalMargin = opt.maximizeHmargin || 0, verticalMargin = opt.maximizeVmargin || 0, wrapper = this.wrapper, wnd = $(window), zoomLevel = kendo.support.zoomLevel(), w, h;
                if (!this.options.isMaximized) {
                    return;
                }
                w = wnd.width() / zoomLevel - 2 * horizontalMargin;
                h = wnd.height() / zoomLevel - 2 * verticalMargin - parseInt(wrapper.css("padding-top"), 10);
                wrapper.css({
                    width: w,
                    height: h,
                    left: horizontalMargin,
                    top: verticalMargin
                });
                this.options.width = w;
                this.options.height = h;
                this.resize();
            },
            destroy: function() {
                this.wrapper.off(NS);
                ui.Window.fn.destroy.call(this);
            }
        });
        kendo.ui.plugin(MailClientWindow);
    })(window.kendo.jQuery);
    return window.kendo;
}, typeof define == "function" && define.amd ? define : function(a1, a2, a3) {
    "use strict";
    (a3 || a2)();
});

(function(f, define) {
    "use strict";
    define("kendo.mailclient.resize-handle", [], f);
})(function() {
    "use strict";
    (function($, undefined) {
        var kendo = window.kendo, ui = kendo.ui, Widget = ui.Widget, proxy = $.proxy, BODY = "body", CURSOR = "cursor", ESC_KEY = 27, UNIT_PERCENT = "percent";
        var MailClientResizeHandle = Widget.extend({
            init: function(element, options) {
                Widget.fn.init.call(this, element, options);
                this._element();
                this._init();
                this._position();
                kendo.notify(this);
            },
            options: {
                name: "MailClientResizeHandle",
                left: "",
                right: "",
                leftMin: "0rem",
                rightMin: "0rem",
                unit: UNIT_PERCENT
            },
            events: [],
            _element: function() {
                this.element.addClass("mail-client-resize-handle").attr("role", "mailclientresizehandle");
            },
            _init: function() {
                this.parentElement = this.element.parent();
                this.leftElement = this.parentElement.find(this.options.left);
                this.rightElement = this.parentElement.find(this.options.right);
                this.htmlElement = $(document.documentElement);
                this._draggable = new ui.Draggable(this.element, {
                    group: this.element.id + "-resizing",
                    dragstart: proxy(this._dragstart, this),
                    drag: proxy(this._drag, this),
                    dragend: proxy(this._dragend, this)
                });
            },
            _position: function() {
                var lw = this.leftElement.outerWidth(), lx = this.leftElement[0].offsetLeft, rx = this.rightElement[0].offsetLeft, w = this.element.outerWidth(), x = lx + lw, delta = Math.abs(x - rx), posX;
                if (w >= delta) {
                    posX = x - (w - delta) / 2;
                } else {
                    posX = x;
                }
                this.element.css("left", this._toCssValue(posX));
            },
            _minMax: function() {
                var opt = this.options, leftMin = parseInt(opt.leftMin, 10), rightMin = parseInt(opt.rightMin, 10), result = {
                    leftMin: 0,
                    rightMin: 0
                }, htmlFontSizeInPixels = this.htmlElement.css("font-size"), htmlFontSize = parseInt(htmlFontSizeInPixels, 10);
                if (isNaN(htmlFontSize) || htmlFontSize <= 0) {
                    return result;
                }
                if (!isNaN(leftMin)) {
                    result.leftMin = leftMin * htmlFontSize;
                }
                if (!isNaN(rightMin)) {
                    result.rightMin = rightMin * htmlFontSize;
                }
                return result;
            },
            _dragstart: function(e) {
                this._initialSize = {
                    leftElement: {
                        left: this.leftElement[0].offsetLeft,
                        width: this.leftElement.outerWidth()
                    },
                    rightElement: {
                        left: this.rightElement[0].offsetLeft,
                        width: this.rightElement.outerWidth()
                    },
                    handleElement: {
                        left: this.element[0].offsetLeft,
                        width: this.element.outerWidth()
                    },
                    minMax: this._minMax()
                };
                this._initialParentPos = {
                    left: this.parentElement.offset().left
                };
                $(BODY).css(CURSOR, e.currentTarget.css(CURSOR));
            },
            _drag: function(e) {
                var initialSize = this._initialSize, initialParentPos = this._initialParentPos, initialHandleX = initialSize.handleElement.left, initialHandleW = initialSize.handleElement.width, offsetMoveX = e.x.location - initialParentPos.left, leftEdge = initialSize.leftElement.left, rightEdge = initialSize.rightElement.left + initialSize.rightElement.width, maxWidth = initialSize.leftElement.width + initialSize.rightElement.width, nearLeftEdge = false, nearRightEdge = false, newLeftWidth, newRightWidth, newRightX, newHandleX;
                if (offsetMoveX < leftEdge) {
                    nearLeftEdge = true;
                }
                if (offsetMoveX + initialHandleW > rightEdge) {
                    nearRightEdge = true;
                }
                var dx = offsetMoveX - initialHandleX;
                newLeftWidth = initialSize.leftElement.width + dx;
                if (newLeftWidth < 0) {
                    newLeftWidth = 0;
                }
                if (newLeftWidth > maxWidth) {
                    newLeftWidth = maxWidth;
                }
                if (nearLeftEdge) {
                    newHandleX = leftEdge;
                } else if (nearRightEdge) {
                    newHandleX = rightEdge - initialHandleW;
                } else {
                    newHandleX = offsetMoveX;
                }
                var newLeftWidthDelta = newLeftWidth - initialSize.leftElement.width;
                newRightWidth = Math.max(initialSize.rightElement.width - newLeftWidthDelta, 0);
                var newRightWidthDelta = newRightWidth - initialSize.rightElement.width;
                newRightX = Math.max(initialSize.rightElement.left - newRightWidthDelta, 0);
                this.leftElement.css("width", this._toCssValue(newLeftWidth));
                this.rightElement.css("width", this._toCssValue(newRightWidth));
                this.rightElement.css("left", this._toCssValue(newRightX));
                this.element.css("left", this._toCssValue(newHandleX));
            },
            _dragend: function(e) {
                $(BODY).css(CURSOR, "");
                if (e.keyCode == ESC_KEY) {
                    this._resetDrag();
                }
                this._initialSize = this._initialParentPos = null;
                return false;
            },
            _resetDrag: function() {
                if (this._initialSize) {
                    var lw = this._initialSize.leftElement.width, rw = this._initialSize.rightElement.width, rx = this._initialSize.rightElement.left, hx = this._initialSize.handleElement.left;
                    this.leftElement.css("width", this._toCssValue(lw));
                    this.rightElement.css("width", this._toCssValue(rw));
                    this.rightElement.css("left", this._toCssValue(rx));
                    this.element.css("left", this._toCssValue(hx));
                }
            },
            _toPercent: function(val) {
                return 100 * parseInt(val, 10) / this.parentElement.width();
            },
            _toCssValue: function(val) {
                return this.options.unit === UNIT_PERCENT ? this._toPercent(val) + "%" : val + "px";
            },
            destroy: function() {
                Widget.fn.destroy.call(this);
                if (this._draggable) {
                    this._draggable.destroy();
                }
                this._draggable = null;
                this.leftElement = this.rightElement = this.parentElement = this.htmlElement = null;
                this.element = null;
            }
        });
        kendo.ui.plugin(MailClientResizeHandle);
    })(window.kendo.jQuery);
    return window.kendo;
}, typeof define == "function" && define.amd ? define : function(a1, a2, a3) {
    "use strict";
    (a3 || a2)();
});

(function(f, define) {
    "use strict";
    define("kendo.mailclient.preview", [], f);
})(function() {
    "use strict";
    (function($, undefined) {
        var kendo = window.kendo, ui = kendo.ui, proxy = $.proxy, isPlainObject = $.isPlainObject, isEmptyObject = $.isEmptyObject, NS = ".kendoMailClientPreview", CONFIG = "config", MOUSEENTER = "mouseenter", MOUSELEAVE = "mouseleave", REQUESTSTART = "requestStart", ERROR = "error", BEFORE_CONTENTLOAD = "beforeContentLoad", CONTENTLOAD = "contentLoad", BEFORE_CONTENTERROR = "beforeContentError", CONTENTERROR = "contentError", BEFORE_CONTENTAPPEND = "beforeContentAppend", POSITIONS = {
            top: {
                origin: "top left",
                position: "bottom left"
            }
        };
        var MailClientPreview = ui.Tooltip.extend({
            init: function(element, options) {
                ui.Tooltip.fn.init.call(this, element, options);
                this._filter();
                this._targetContainer();
                this._templates();
                this._setupEvents();
                this._cache = {};
                this._ajaxJsonErrorHandler = proxy(this._ajaxJsonErrorHandler, this);
                this._ajaxJsonSuccessHandler = proxy(this._ajaxJsonSuccessHandler, this);
                this._popupMouseEnter = proxy(this._popupMouseEnter, this);
                this._popupMouseLeave = proxy(this._popupMouseLeave, this);
            },
            options: {
                name: "MailClientPreview",
                target: "",
                template: "",
                errorTemplate: "",
                caching: false,
                iframe: false,
                autoHide: true,
                cssProgress: "",
                cssPopup: "",
                closeDelay: 50
            },
            events: (ui.Tooltip.fn.events || []).concat([ BEFORE_CONTENTLOAD, BEFORE_CONTENTERROR, BEFORE_CONTENTAPPEND, CONTENTERROR ]),
            _filter: function() {
                this.options.filter = (this.options.filter || "").replace(/\\/g, "");
            },
            _templates: function() {
                this.template = kendo.template(this.options.template);
                this.errorTemplate = kendo.template(this.options.errorTemplate);
            },
            _targetContainer: function() {
                if (this.options.target) {
                    var target = $(this.options.target);
                    target.on(this.options.showOn + NS, this.options.filter, proxy(this._showOn, this)).on(MOUSEENTER + NS, this.options.filter, proxy(this._mouseenter, this));
                    if (this.options.autoHide) {
                        target.on(MOUSELEAVE + NS, this.options.filter, proxy(this._mouseleave, this));
                    }
                }
            },
            _initPopup: function() {
                ui.Tooltip.fn._initPopup.apply(this, arguments);
                this.popup.element.addClass(this.options.cssPopup).on(MOUSEENTER + NS, this._popupMouseEnter).on(MOUSELEAVE + NS, this._popupMouseLeave);
                var popupPosition = POSITIONS[this.options.position];
                if (popupPosition) {
                    if (popupPosition.origin) {
                        this.popup.options.origin = popupPosition.origin;
                    }
                    if (popupPosition.position) {
                        this.popup.options.position = popupPosition.position;
                    }
                }
            },
            _popupMouseEnter: function() {
                clearTimeout(this._closeTimeout);
            },
            _popupMouseLeave: function() {
                if (this.popup) {
                    this.popup.close();
                }
                clearTimeout(this.timeout);
            },
            _mouseleave: function() {
                var that = this;
                if (!this.popup) {
                    ui.Tooltip.fn._mouseleave.apply(this, arguments);
                    return;
                }
                this._closeTimeout = setTimeout(function() {
                    that.popup.close();
                }, this.options.closeDelay);
                clearTimeout(this.timeout);
            },
            _appendContent: function(target) {
                var contentOptions = this.options.content, element = this.content;
                this.trigger(BEFORE_CONTENTAPPEND, {
                    target: target
                });
                if (isPlainObject(contentOptions) && contentOptions.url) {
                    if (this.options.caching && contentOptions.url !== CONFIG && this._cache[contentOptions.url] !== undefined) {
                        element.html(this._cache[contentOptions.url]);
                        return;
                    }
                }
                ui.Tooltip.fn._appendContent.apply(this, arguments);
            },
            _setupEvents: function() {
                this.bind(REQUESTSTART, proxy(this._requestStart, this));
                this.bind(CONTENTLOAD, proxy(this._requestEnd, this));
                this.bind(ERROR, proxy(this._requestFailed, this));
            },
            _requestStart: function() {
                var opt = this.options, contentOptions = opt.content, isUrl = isPlainObject(contentOptions) && contentOptions.url;
                if (isUrl) {
                    if (contentOptions.dataType === "json") {
                        contentOptions.success = this._ajaxJsonSuccessHandler;
                        contentOptions.error = this._ajaxJsonErrorHandler;
                    }
                }
                this._addProgressCss();
            },
            _requestEnd: function() {
                this._removeProgressCss();
            },
            _requestFailed: function() {
                this._removeProgressCss();
                this._contentError();
            },
            _contentError: function() {
                if (this.options.errorTemplate) {
                    var e = {
                        data: {},
                        template: null
                    };
                    if (!this.trigger(BEFORE_CONTENTERROR, e)) {
                        this.content.html((e.template || this.errorTemplate)(e.data || {}));
                        this.trigger(CONTENTERROR);
                    }
                }
            },
            _addProgressCss: function() {
                if (this.options.cssProgress && this.popup) {
                    this.popup.element.addClass(this.options.cssProgress);
                }
            },
            _removeProgressCss: function() {
                if (this.options.cssProgress && this.popup) {
                    this.popup.element.removeClass(this.options.cssProgress);
                }
            },
            _ajaxJsonErrorHandler: function(xhr, status) {
                kendo.ui.progress(this.content, false);
                this.trigger(ERROR, {
                    status: status,
                    xhr: xhr
                });
            },
            _ajaxJsonSuccessHandler: function(data) {
                data = data || {};
                kendo.ui.progress(this.content, false);
                var e = {
                    response: data
                };
                if (this.trigger(BEFORE_CONTENTLOAD, e)) {
                    return;
                }
                if (isEmptyObject(e.response)) {
                    this._contentError();
                    this._requestEnd();
                    return;
                }
                var contents = this.template(e.response || data);
                if (this.options.caching) {
                    this._cache[this.options.content.url] = contents;
                }
                this.content.html(contents);
                this.trigger(CONTENTLOAD);
            },
            destroy: function() {
                if (this.options.target) {
                    $(this.options.target).off(NS);
                }
                if (this.popup) {
                    this.popup.element.off(NS);
                }
                ui.Tooltip.fn.destroy.call(this);
            }
        });
        kendo.ui.plugin(MailClientPreview);
    })(window.kendo.jQuery);
    return window.kendo;
}, typeof define == "function" && define.amd ? define : function(a1, a2, a3) {
    "use strict";
    (a3 || a2)();
});

(function(f, define) {
    "use strict";
    define("kendo.mailclient.listview", [], f);
})(function() {
    "use strict";
    (function($, undefined) {
        var kendo = window.kendo, ui = kendo.ui, keys = kendo.keys, proxy = $.proxy, START = "start", MOVE = "move", END = "end", FOCUSSELECTOR = ">*:not(.k-loading-mask)", DRAGHINT = "draghint", DRAGSTART = "dragstart", DRAGEND = "dragend", DRAGENTER = "dragenter", DRAGLEAVE = "dragleave", DROP = "drop", NS = ".kendoMailClientListView";
        var MailClientListView = ui.ListView.extend({
            init: function(element, options) {
                ui.ListView.fn.init.call(this, element, options);
                this._draggable();
                this._hover();
            },
            options: {
                name: "MailClientListView",
                draggable: false,
                draggableFilter: FOCUSSELECTOR,
                draggableGroup: "default",
                hintTemplate: "",
                dropArea: null,
                dropGroup: "default",
                dropFilter: null,
                preventDragEnd: true,
                hintOnCenter: true,
                cssDropTargetActive: "",
                cssHintOnDropTarget: "",
                cssHover: "",
                hoverFilter: "",
                autoRefresh: false
            },
            events: (ui.ListView.fn.events || []).concat([ DRAGHINT, DROP ]),
            setDataSource: function(dataSource) {
                ui.ListView.fn.setDataSource.call(this, dataSource);
                if (this.options.autoRefresh) {
                    this.refresh();
                }
            },
            _templates: function() {
                ui.ListView.fn._templates.call(this);
                this.hintTemplate = kendo.template(this.options.hintTemplate || "");
            },
            _hover: function() {
                var opt = this.options;
                if (opt.hoverFilter) {
                    this.element.on("mouseenter" + NS, opt.hoverFilter, function(e) {
                        $(e.currentTarget).addClass(opt.cssHover);
                    }).on("mouseleave" + NS, opt.hoverFilter, function(e) {
                        $(e.currentTarget).removeClass(opt.cssHover);
                    });
                }
            },
            _selectable: function() {
                ui.ListView.fn._selectable.call(this);
                var that = this, options = this.options, selectable = this.selectable, disableMarquee = selectable && selectable.options.multiple && options.draggable, navigatable = this.options.navigatable;
                if (disableMarquee) {
                    selectable.userEvents.unbind(START).unbind(MOVE).unbind(END);
                }
                if (navigatable) {
                    that.element.on("keydown" + NS, function(e) {
                        var key = e.keyCode, current = that.current();
                        if (keys.UP === key || keys.LEFT === key) {
                            if (e.target == e.currentTarget) {
                                e.preventDefault();
                            }
                            that.selectable.clear();
                            that.selectable.value(!current || !current[0] ? that._item("last") : current);
                        } else if (keys.DOWN === key || keys.RIGHT === key) {
                            if (e.target == e.currentTarget) {
                                e.preventDefault();
                            }
                            that.selectable.clear();
                            that.selectable.value(!current || !current[0] ? that._item("first") : current);
                        }
                    });
                }
            },
            _draggable: function() {
                var opt = this.options;
                if (opt.draggable) {
                    this.draggable = new ui.Draggable(this.element, {
                        filter: opt.draggableFilter,
                        group: opt.draggableGroup,
                        hint: proxy(this._dragHint, this),
                        dragstart: proxy(this._dragStart, this),
                        dragend: proxy(this._dragEnd, this)
                    });
                    if (opt.dropArea) {
                        this.dropArea = new ui.DropTargetArea(opt.dropArea, {
                            group: opt.dropGroup,
                            filter: opt.dropFilter,
                            drop: proxy(this._drop, this),
                            dragenter: proxy(this._dragEnter, this),
                            dragleave: proxy(this._dragLeave, this)
                        });
                    }
                }
            },
            _dragHint: function(element) {
                var e = {
                    hintData: {},
                    target: element
                };
                this.trigger(DRAGHINT, e);
                if (this.options.hintTemplate) {
                    return $(this.hintTemplate(e.hintData || {}));
                } else {
                    return element.clone();
                }
            },
            _dragStart: function(e) {
                if (this.options.hintOnCenter) {
                    this._hintCenter(e);
                }
                this.trigger(DRAGSTART, {
                    origin: e
                });
            },
            _dragEnd: function(e) {
                this.trigger(DRAGEND, {
                    origin: e
                });
                if (this.options.preventDragEnd) {
                    e.preventDefault();
                }
            },
            _dragEnter: function(e) {
                this._dragEnterCss(e.dropTarget, e.draggable);
                this.trigger(DRAGENTER, {
                    origin: e
                });
            },
            _dragLeave: function(e) {
                this._dragLeaveCss(e.dropTarget, e.draggable);
                this.trigger(DRAGLEAVE, {
                    origin: e
                });
            },
            _drop: function(e) {
                this._dragLeaveCss(e.dropTarget, e.draggable);
                this.trigger(DROP, {
                    origin: e
                });
            },
            _dragEnterCss: function(dropTarget, draggable) {
                var activeCss = this.options.cssDropTargetActive, hintCss = this.options.cssHintOnDropTarget;
                if (activeCss && dropTarget) {
                    dropTarget.addClass(activeCss);
                }
                if (hintCss && draggable && draggable.hint) {
                    draggable.hint.addClass(hintCss);
                }
            },
            _dragLeaveCss: function(dropTarget, draggable) {
                var activeCss = this.options.cssDropTargetActive, hintCss = this.options.cssHintOnDropTarget;
                if (activeCss && dropTarget) {
                    dropTarget.removeClass(activeCss);
                }
                if (hintCss && draggable && draggable.hint) {
                    draggable.hint.removeClass(hintCss);
                }
            },
            _hintCenter: function(e) {
                var sender = e.sender, hintOffset = sender.hintOffset, hint = sender.hint, cursorX = e.x.location, cursorY = e.y.location;
                if (hint && hintOffset) {
                    var halfWidth = hint.width() / 2;
                    var halfHeight = hint.height() / 2;
                    var offsetX = cursorX - hintOffset.left - halfWidth;
                    var offsetY = cursorY - hintOffset.top - halfHeight;
                    hint.css({
                        marginLeft: offsetX,
                        marginTop: offsetY
                    });
                }
            },
            destroy: function() {
                if (this.draggable) {
                    this.draggable.destroy();
                    this.draggable = null;
                }
                if (this.dropArea) {
                    this.dropArea.destroy();
                    this.dropArea = null;
                }
                this.element.off("keydown" + NS).off("mouseenter" + NS).off("mouseleave" + NS);
                ui.ListView.fn.destroy.call(this);
            }
        });
        kendo.ui.plugin(MailClientListView);
    })(window.kendo.jQuery);
    return window.kendo;
}, typeof define == "function" && define.amd ? define : function(a1, a2, a3) {
    "use strict";
    (a3 || a2)();
});

(function(f, define) {
    "use strict";
    define("kendo.mailclient.contextmenu", [], f);
})(function() {
    "use strict";
    (function($, undefined) {
        var kendo = window.kendo, ui = kendo.ui, proxy = $.proxy, NS = ".kendoMenu", DEFAULT = "default", PREVENT = "prevent", OPEN = "open", CLOSE = "close";
        var MailClientContextMenu = ui.ContextMenu.extend({
            init: function(element, options) {
                ui.ContextMenu.fn.init.call(this, element, options);
                this.bind(OPEN, proxy(this._onOpen, this));
                this.bind(CLOSE, proxy(this._onClose, this));
            },
            options: {
                name: "MailClientContextMenu",
                withTarget: false,
                ignore: "",
                openIgnore: DEFAULT,
                cssOpenTarget: ""
            },
            _onOpen: function(e) {
                this._addCss(e.target);
            },
            _onClose: function(e) {
                this._removeCss(e.target);
            },
            _addCss: function(element) {
                if (element && this.options.cssOpenTarget) {
                    $(element).addClass(this.options.cssOpenTarget);
                }
            },
            _popupTarget: function() {
                return this.popup.options.anchor;
            },
            _removeCss: function(element) {
                if (element && this.options.cssOpenTarget) {
                    $(element).removeClass(this.options.cssOpenTarget);
                }
            },
            _wire: function() {
                ui.ContextMenu.fn._wire.call(this);
                var opt = this.options;
                if (opt.filter && this.target[0] && opt.withTarget) {
                    this.target.on(opt.showOn + NS + this._marker, this._showProxy);
                }
                if (this.target[0] && opt.ignore) {
                    this.target.on(opt.showOn + NS + this._marker, opt.ignore, proxy(this._ignoreHandler, this));
                }
            },
            _ignoreHandler: function(e) {
                if (this.options.openIgnore === PREVENT) {
                    e.preventDefault();
                }
                e.stopImmediatePropagation();
            },
            _showHandler: function(e) {
                var opt = this.options, filter = opt.filter, isBaseTarget = e.currentTarget === this.target[0], openOnTarget = opt.withTarget && filter && isBaseTarget, previosTarget = this._popupTarget(), targetChanged = e.currentTarget !== this._popupTarget();
                if (targetChanged) {
                    this._removeCss(previosTarget);
                }
                if (openOnTarget) {
                    opt.filter = null;
                }
                ui.ContextMenu.fn._showHandler.call(this, e);
                if (openOnTarget) {
                    opt.filter = filter;
                }
            }
        });
        kendo.ui.plugin(MailClientContextMenu);
    })(window.kendo.jQuery);
    return window.kendo;
}, typeof define == "function" && define.amd ? define : function(a1, a2, a3) {
    "use strict";
    (a3 || a2)();
});

(function(f, define) {
    "use strict";
    define("kendo.mailclient.droptargetarea", [], f);
})(function() {
    "use strict";
    (function($, undefined) {
        var kendo = window.kendo, ui = kendo.ui, DEFAULT = "default", DRAGENTER = "dragenter", DRAGLEAVE = "dragleave", DROP = "drop";
        var MailClientDropTargetArea = ui.Widget.extend({
            init: function(element, options) {
                ui.Widget.fn.init.call(this, element, options);
                this._dropTargetAreas();
            },
            options: {
                name: "MailClientDropTargetArea",
                target: "",
                filter: null,
                groups: [],
                group: DEFAULT
            },
            _dropTargetAreas: function() {
                var that = this, opt = this.options, groups = opt.groups.length ? opt.groups : [ opt.group ], filter = opt.filter, dropTargetElement = opt.target ? $(opt.target) : this.element, DropTargetWidget = filter ? ui.DropTargetArea : ui.DropTarget, dropTargetList = [];
                groups.forEach(function(group) {
                    dropTargetList.push(new DropTargetWidget(dropTargetElement, {
                        filter: filter,
                        group: group,
                        drop: that._eventProxy(DROP),
                        dragenter: that._eventProxy(DRAGENTER),
                        dragleave: that._eventProxy(DRAGLEAVE)
                    }));
                });
                this.dropTargetList = dropTargetList;
            },
            _eventProxy: function(eventName) {
                var that = this;
                return function(e) {
                    that.trigger(eventName, e);
                };
            },
            destroy: function() {
                if (this.dropTargetList) {
                    this.dropTargetList.forEach(function(dropTarget) {
                        dropTarget.destroy();
                        dropTarget.element = null;
                    });
                    this.dropTargetList = null;
                }
                ui.Widget.fn.destroy.call(this);
            }
        });
        kendo.ui.plugin(MailClientDropTargetArea);
    })(window.kendo.jQuery);
    return window.kendo;
}, typeof define == "function" && define.amd ? define : function(a1, a2, a3) {
    "use strict";
    (a3 || a2)();
});

(function(f, define) {
    "use strict";
    define("kendo.mailclient.modal", [], f);
})(function() {
    "use strict";
    (function($, undefined) {
        var kendo = window.kendo, ui = kendo.ui, Confirm = ui.Confirm, Widget = ui.Widget, KCONFIRM = "k-confirm", OK = "ok", CANCEL = "cancel";
        var MailClientModal = Confirm.extend({
            _init: function(element, options) {
                Confirm.fn._init.call(this, element, options);
                this.wrapper.removeClass(KCONFIRM).addClass("mail-client-modal");
            },
            events: (ui.Confirm.fn.events || []).concat([ OK, CANCEL ]),
            options: {
                name: "MailClientModal",
                modal: true,
                actions: [ {
                    text: "#= messages.okText #",
                    primary: true,
                    action: function(e) {
                        var preventClose = !e.sender.trigger(OK);
                        e.sender.result.resolve();
                        return preventClose;
                    }
                }, {
                    text: "#= messages.cancel #",
                    action: function(e) {
                        var preventClose = !e.sender.trigger(CANCEL);
                        e.sender.result.reject();
                        return preventClose;
                    }
                } ]
            },
            _destroy: function() {
                Confirm.fn._destroy.call(this);
                Widget.fn.destroy.call(this);
                kendo.destroy(this.wrapper);
            }
        });
        kendo.ui.plugin(MailClientModal);
    })(window.kendo.jQuery);
    return window.kendo;
}, typeof define == "function" && define.amd ? define : function(a1, a2, a3) {
    "use strict";
    (a3 || a2)();
});

(function(f, define) {
    "use strict";
    define("kendo.mailclient.commands", [ "kendo.mailclient.ns", "kendo.mailclient.utils", "kendo.mailclient.constants", "kendo.mailclient.view-models", "kendo.mailclient.data", "kendo.mailclient.models", "kendo.mailclient.commandutil" ], f);
})(function() {
    "use strict";
    (function($, undefined) {
        var kendo = window.kendo, ui = kendo.ui, mailclientNS = ui.mailclient, proxy = $.proxy, extend = $.extend, Class = kendo.Class, ObservableObject = kendo.data.ObservableObject, isArray = Array.isArray, CommandState = mailclientNS.CommandState, Commands = mailclientNS.Commands, BroadcastEvents = mailclientNS.BroadcastEvents, FolderType = mailclientNS.FolderType, arrayCount = mailclientNS.arrayCount, isFn = mailclientNS.isFn, isDate = mailclientNS.isDate, notNullFilter = mailclientNS.notNullFilter, filterByField = mailclientNS.filterByField, onlyUnique = mailclientNS.onlyUnique, registerCommand = mailclientNS.registerCommand, first = mailclientNS.first, NewMessageDlgViewModel = mailclientNS.NewMessageDlgViewModel, SettingsDlgViewModel = mailclientNS.SettingsDlgViewModel, Recipient = mailclientNS.Recipient, createWndDialog = mailclientNS.createWndDialog, ACTION_ADD = "add", ACTION_REMOVE = "remove";
        var Command = Class.extend({
            init: function(options) {
                this.options = options;
                this.mailclient = options.mailclient;
                this.service = this.mailclient.service;
                this.widgetOpt = options.widgetOpt;
                this.noProgressBar = !!options.noProgressBar;
                this.type = options.commandType;
                this.runningCommands = options.runningCommands;
                this.state = CommandState.Init;
                this.execDeferred = null;
                this.lastError = null;
                this._pending = proxy(this._pending, this);
                this._done = proxy(this._done, this);
                this._error = proxy(this._error, this);
            },
            isPending: function() {
                return this.state === CommandState.Pending;
            },
            isCompleted: function() {
                return this.state === CommandState.Failed || this.state === CommandState.Success;
            },
            _showProgress: function() {
                if (!this.noProgressBar) {
                    this.mailclient.showProgress();
                }
            },
            _hideProgress: function() {
                if (!this.noProgressBar) {
                    this.mailclient.hideProgress();
                }
            },
            progressBar: function(value) {
                this.noProgressBar = !value;
            },
            _pending: function() {
                this.state = CommandState.Pending;
                this._showProgress();
            },
            _done: function() {
                this._running(false);
                this.state = CommandState.Success;
                this._hideProgress();
                if (this.execDeferred) {
                    this.execDeferred.resolve();
                }
            },
            _error: function(error, hideProgress) {
                hideProgress = hideProgress === undefined ? true : hideProgress;
                this._running(false);
                this.state = CommandState.Failed;
                this.lastError = error;
                if (hideProgress) {
                    this._hideProgress();
                }
                this.mailclient.showError(error.toString());
                if (this.execDeferred) {
                    this.execDeferred.reject(error);
                }
            },
            _deferred: function(isNew) {
                if (!this.execDeferred || isNew) {
                    this.execDeferred = $.Deferred();
                }
                return this.execDeferred;
            },
            _running: function(isRun) {
                if (this.runningCommands) {
                    if (isRun) {
                        this.runningCommands.add(this);
                    } else {
                        this.runningCommands.remove(this);
                    }
                }
            },
            _baseExec: function() {
                this._running(true);
            },
            exec: function() {
                throw new Error("Not implemented.");
            }
        });
        var MoveToCommand = Command.extend({
            init: function(options) {
                Command.fn.init.call(this, options);
                this._handleMoveTo = proxy(this._handleMoveTo, this);
            },
            exec: function() {
                var opt = this.options, messageIds = opt.messageIds, folderType = opt.folderType, destFolderId = opt.folderId, folderList = this.mailclient.folderList, srcFolder = opt.srcFolder || folderList.getCurrent(), srcFolderId = srcFolder ? srcFolder.id : null;
                this._baseExec();
                this._deferred(true);
                if (!destFolderId && folderType) {
                    var folder = folderList.findByType(folderType);
                    destFolderId = folder ? folder.id : null;
                }
                if (!destFolderId) {
                    this._error(new Error("Unknown folder id."), false);
                    return this._deferred();
                }
                opt.srcFolderId = srcFolderId;
                opt.destFolderId = destFolderId;
                this._pending();
                this.service.moveTo(destFolderId, messageIds, srcFolderId).then(this._handleMoveTo).done(this._done).fail(this._error);
                return this._deferred();
            },
            _handleMoveTo: function() {
                var opt = this.options, folderList = this.mailclient.folderList, messageDetails = this.mailclient.messageDetails, messageList = this.mailclient.messageList, srcFolderId = opt.srcFolderId, destFolderId = opt.destFolderId, messageIds = opt.messageIds, srcFolder = folderList.find(srcFolderId), destFolder = folderList.find(destFolderId), currFolder = folderList.getCurrent(), currMessage = messageDetails.getCurrent(), srcIsCurrFolder = srcFolder && currFolder && srcFolder.id === currFolder.id, messages = messageIds.map(function(id) {
                    return messageList.find(id);
                }).filter(notNullFilter), messageCount = messages.length, unreadCount = arrayCount(messages, filterByField("isRead", false));
                if (srcIsCurrFolder) {
                    var isCurrMessageMoved = currMessage && !!arrayCount(messages, filterByField("id", currMessage.id));
                    if (isCurrMessageMoved) {
                        messageDetails.close();
                    }
                    messageList.removeList(messages);
                    if (!isCurrMessageMoved && currMessage) {
                        messageList.select(currMessage.id);
                    }
                }
                if (srcFolder) {
                    srcFolder.subTotalItemCount(messageCount);
                    srcFolder.subUnreadItemCount(unreadCount);
                }
                if (destFolder) {
                    destFolder.addTotalItemCount(messageCount);
                    destFolder.addUnreadItemCount(unreadCount);
                }
            }
        });
        var MoveAllCommand = Command.extend({
            init: function(options) {
                Command.fn.init.call(this, options);
                this._handleMoveAll = proxy(this._handleMoveAll, this);
            },
            exec: function() {
                var opt = this.options, srcFolderId = opt.srcFolderId, destFolderId = opt.destFolderId;
                this._baseExec();
                this._deferred(true);
                if (!srcFolderId || !destFolderId) {
                    this._error(new Error("srcFolderId and destFolderId parameters is required."), false);
                    return this._deferred();
                }
                this._pending();
                this.service.moveAll(srcFolderId, destFolderId).then(this._handleMoveAll).done(this._done).fail(this._error);
                return this._deferred();
            },
            _handleMoveAll: function() {
                var opt = this.options, folderList = this.mailclient.folderList, messageList = this.mailclient.messageList, srcFolderId = opt.srcFolderId, destFolderId = opt.destFolderId, currentFolder = folderList.getCurrent(), srcFolder = folderList.find(srcFolderId), destFolder = folderList.find(destFolderId);
                if (currentFolder) {
                    if (currentFolder.id === srcFolderId) {
                        messageList.clearAll();
                    } else if (currentFolder.id === destFolderId) {
                        var ds = messageList.dataSource;
                        if (!ds.inProgress()) {
                            this.mailclient.command(Commands.Refresh, {
                                noRefreshFolders: true
                            }).exec();
                        }
                    }
                }
                if (srcFolder && destFolder) {
                    destFolder.addUnreadItemCount(srcFolder.unreadItemCount);
                    srcFolder.resetCounters();
                }
            }
        });
        var DeleteAllCommand = Command.extend({
            init: function(options) {
                Command.fn.init.call(this, options);
                this._handleDeleteAll = proxy(this._handleDeleteAll, this);
            },
            exec: function() {
                var opt = this.options, folderId = opt.folderId;
                this._baseExec();
                this._deferred(true);
                if (!folderId) {
                    this._error(new Error("folderId parameter is required."), false);
                    return this._deferred();
                }
                this._pending();
                this.service.deleteAll(folderId).then(this._handleDeleteAll).done(this._done).fail(this._error);
                return this._deferred();
            },
            _handleDeleteAll: function() {
                var opt = this.options, folderList = this.mailclient.folderList, messageList = this.mailclient.messageList, folderId = opt.folderId, currentFolder = folderList.getCurrent(), folder = folderList.find(folderId);
                if (currentFolder && currentFolder.id === folderId) {
                    messageList.clearAll();
                }
                if (folder) {
                    folder.resetCounters();
                }
            }
        });
        var DeleteCommand = Command.extend({
            init: function(options) {
                Command.fn.init.call(this, options);
                this._handleDelete = proxy(this._handleDelete, this);
            },
            exec: function() {
                var opt = this.options, messageIds = opt.messageIds || [], folderId = opt.folderId;
                this._baseExec();
                this._deferred(true);
                if (!folderId || messageIds.length === 0) {
                    this._error(new Error("folderId and messageIds is required."), false);
                    return this._deferred();
                }
                this._pending();
                this.service.deleteItems(folderId, messageIds).then(this._handleDelete).done(this._done).fail(this._error);
                return this._deferred();
            },
            _handleDelete: function() {
                var opt = this.options, folderList = this.mailclient.folderList, messageDetails = this.mailclient.messageDetails, messageList = this.mailclient.messageList, folderId = opt.folderId, messageIds = opt.messageIds, folder = folderList.find(folderId), currFolder = folderList.getCurrent(), currMessage = messageDetails.getCurrent(), isCurrFolder = folder && currFolder && folder.id === currFolder.id, messages = messageIds.map(function(id) {
                    return messageList.find(id);
                }).filter(notNullFilter), messageCount = messages.length, unreadCount = arrayCount(messages, filterByField("isRead", false));
                if (isCurrFolder) {
                    var isCurrMessageMoved = currMessage && !!arrayCount(messages, filterByField("id", currMessage.id));
                    if (isCurrMessageMoved) {
                        messageDetails.close();
                    }
                    messageList.removeList(messages);
                    if (!isCurrMessageMoved && currMessage) {
                        messageList.select(currMessage.id);
                    }
                }
                if (folder) {
                    folder.subTotalItemCount(messageCount);
                    folder.subUnreadItemCount(unreadCount);
                }
            }
        });
        var SetFlagCommand = Command.extend({
            init: function(options) {
                Command.fn.init.call(this, options);
            },
            _exec: function(serviceFn, handler) {
                var opt = this.options, messageIds = opt.messageIds, action = opt.action;
                this._baseExec();
                this._deferred(true);
                if (!serviceFn[action]) {
                    this._error(new Error("Unknown service function."), false);
                    return this._deferred();
                }
                var messages = this._findMessages(messageIds);
                opt.folderIds = this._findFolderIds(messages);
                this._pending();
                serviceFn[action].call(null, messageIds).then(handler).done(this._done).fail(this._error);
                return this._deferred();
            },
            _handle: function(flagSetter, countersFn) {
                var opt = this.options, messageIds = opt.messageIds, folderIds = opt.folderIds, action = opt.action, flagVal = action === ACTION_ADD, messages = this._findMessages(messageIds), currMessage = this.mailclient.messageDetails.getCurrent();
                messages.forEach(function(item) {
                    if (isFn(item, flagSetter)) {
                        item[flagSetter](flagVal);
                    }
                });
                if (currMessage) {
                    var isCurrUpdate = messageIds.filter(function(id) {
                        return id === currMessage.id;
                    }).length > 0;
                    if (isCurrUpdate && isFn(currMessage, flagSetter)) {
                        currMessage[flagSetter](flagVal);
                    }
                }
                if (countersFn) {
                    this._updateCounters(folderIds || [], countersFn[action], messages.length);
                }
            },
            _updateCounters: function(folderIds, counterFn, value) {
                var folders = this._findFolders(folderIds);
                if (typeof counterFn === "string" && folders.length > 0) {
                    folders.forEach(function(item) {
                        if (isFn(item, counterFn)) {
                            item[counterFn](value);
                        }
                    });
                }
            },
            _findFolderIds: function(messages) {
                return messages.map(function(item) {
                    return item.folderId;
                });
            },
            _findFolders: function(folderIds) {
                var folderList = this.mailclient.folderList;
                return folderIds.filter(onlyUnique).map(function(id) {
                    return folderList.find(id);
                }).filter(notNullFilter);
            },
            _findMessages: function(messageIds) {
                var messageList = this.mailclient.messageList;
                return messageIds.map(function(id) {
                    return messageList.find(id);
                }).filter(notNullFilter);
            }
        });
        var AddFlagCommand = SetFlagCommand.extend({
            init: function(options) {
                options.action = ACTION_ADD;
                SetFlagCommand.fn.init.call(this, options);
            },
            exec: function() {
                var serviceFn = {}, service = this.service, that = this;
                serviceFn[ACTION_ADD] = service.addFlag.bind(service);
                return this._exec(serviceFn, function() {
                    that._handle("setFlag");
                });
            }
        });
        var RemoveFlagCommand = SetFlagCommand.extend({
            init: function(options) {
                options.action = ACTION_REMOVE;
                SetFlagCommand.fn.init.call(this, options);
            },
            exec: function() {
                var serviceFn = {}, service = this.service, that = this;
                serviceFn[ACTION_REMOVE] = service.removeFlag.bind(service);
                return this._exec(serviceFn, function() {
                    that._handle("setFlag");
                });
            }
        });
        var MarkAsReadCommand = SetFlagCommand.extend({
            init: function(options) {
                options.action = ACTION_ADD;
                SetFlagCommand.fn.init.call(this, options);
            },
            exec: function() {
                var serviceFn = {}, countersFn = {}, service = this.service, that = this;
                serviceFn[ACTION_ADD] = service.markAsRead.bind(service);
                countersFn[ACTION_ADD] = "subUnreadItemCount";
                return this._exec(serviceFn, function() {
                    that._handle("setRead", countersFn);
                    service.broadcast(BroadcastEvents.MARK_AS_READ);
                });
            }
        });
        var MarkAsUnreadCommand = SetFlagCommand.extend({
            init: function(options) {
                options.action = ACTION_REMOVE;
                SetFlagCommand.fn.init.call(this, options);
            },
            exec: function() {
                var serviceFn = {}, countersFn = {}, service = this.service, that = this;
                serviceFn[ACTION_REMOVE] = service.markAsUnread.bind(service);
                countersFn[ACTION_REMOVE] = "addUnreadItemCount";
                return this._exec(serviceFn, function() {
                    that._handle("setRead", countersFn);
                    service.broadcast(BroadcastEvents.MARK_AS_UNREAD);
                });
            }
        });
        var ComposeCommand = Command.extend({
            init: function(options) {
                Command.fn.init.call(this, options);
                this.noProgressBar = true;
                this.viewModel = null;
            },
            exec: function() {
                var that = this, opt = this.options, messageData = opt.messageData || {}, isSignature = opt.isSignature, title = opt.title, service = this.service, widgetOpt = this.widgetOpt, dialogsOpt = widgetOpt.dialogs, isMaximize = opt.isMaximize;
                this._baseExec();
                this._deferred(true);
                if (isSignature === undefined || isSignature) {
                    messageData.body = this._addBodySignature(messageData.body, true);
                }
                this.viewModel = new NewMessageDlgViewModel(widgetOpt, service, messageData);
                var dialog = createWndDialog(extend({
                    isMaximize: isMaximize
                }, dialogsOpt.newMessage), this.viewModel);
                dialog.onClose(function() {
                    that._done();
                    that.viewModel = null;
                });
                dialog.show();
                if (title) {
                    dialog.getWnd().title(title);
                }
                this.viewModel.setWnd(dialog.getWnd());
                return this._deferred();
            },
            _addBodySignature: function(html, isHtml) {
                var settings = this.mailclient.settings, signature = settings.signature;
                if (signature) {
                    var signatureText = this._makeBodySignature(signature, isHtml);
                    return this._appendToHtml(html, signatureText);
                }
                return html;
            },
            _makeBodySignature: function(signature, isHtml) {
                var signatureText;
                if (isHtml) {
                    signatureText = "<br/><br/>-- <div>" + signature + "</div>";
                } else {
                    signatureText = "\r\n\r\n-- \r\n" + kendo.htmlEncode(signature);
                }
                return signatureText;
            },
            _appendToHtml: function(html, appendText) {
                var closeBodyTagRExp = /<\/body>/i, closeHtmlTagRExp = /<\/html>/i;
                return this._replaceText(html, null, [ closeBodyTagRExp, closeHtmlTagRExp ], null, appendText);
            },
            _prependHtml: function(html, prependText) {
                var openHtmlTagRExp = /<html( [^>]+)?>/i, openBodyTagRExp = /<body( [^>]+)?>/i, closeHeadTagRExp = /<\/head>/i;
                return this._replaceText(html, [ openBodyTagRExp, closeHeadTagRExp, openHtmlTagRExp ], null, prependText, null);
            },
            _replaceText: function(text, startRegexp, endRegexp, startString, endString) {
                var result = text || "", startReplaced = false, endReplaced = false;
                if (startString) {
                    startRegexp = startRegexp || [];
                    for (var i = 0; i < startRegexp.length; i++) {
                        if (startRegexp[i].test(result)) {
                            result = result.replace(startRegexp[i], "$&" + startString);
                            startReplaced = true;
                            break;
                        }
                    }
                    if (!startReplaced) {
                        result = startString + result;
                    }
                }
                if (endString) {
                    endRegexp = endRegexp || [];
                    for (var j = 0; j < endRegexp.length; j++) {
                        if (endRegexp[j].test(result)) {
                            result = result.replace(endRegexp[j], endString + "$&");
                            endReplaced = true;
                            break;
                        }
                    }
                    if (!endReplaced) {
                        result += endString;
                    }
                }
                return result;
            }
        });
        var ReplyCommand = ComposeCommand.extend({
            init: function(options) {
                ComposeCommand.fn.init.call(this, this._prepareOptions(options));
                this._handleGetMessage = proxy(this._handleGetMessage, this);
            },
            exec: function() {
                var opt = this.options, service = this.service, messageId = opt.messageId, messageDetails = opt.messageDetails;
                if (messageId) {
                    this._deferred(true);
                    this.progressBar(true);
                    this._pending();
                    service.getMessageDetails(messageId).done(this._handleGetMessage).fail(this._error);
                    return this._deferred();
                } else if (messageDetails) {
                    this._extendMessageData(opt, this._convertToMessageData(messageDetails));
                    this._prepareOptions(opt);
                }
                return ComposeCommand.fn.exec.call(this);
            },
            _handleGetMessage: function(messageDetails) {
                var opt = this.options, deferred = this._deferred();
                this._hideProgress();
                this.progressBar(false);
                this._extendMessageData(opt, this._convertToMessageData(messageDetails));
                this._prepareOptions(opt);
                ComposeCommand.fn.exec.call(this).done(deferred.resolve).fail(deferred.reject);
            },
            _extendMessageData: function(opt, messageData) {
                extend(opt, {
                    messageData: messageData
                });
            },
            _convertToMessageData: function(messageDetails) {
                var message = messageDetails.toJSON();
                var messageData = {
                    from: message.from,
                    to: message.to,
                    cc: message.cc,
                    subject: message.subject,
                    body: message.body,
                    sentDate: message.sentDate,
                    isHtml: message.isHtml
                };
                return messageData;
            },
            _prepareOptions: function(opt) {
                var messageData = opt.messageData;
                if (messageData) {
                    var currentAccount = opt.mailclient.account.getCurrent();
                    var to = this._makeReplyTo(messageData.from);
                    var cc = this._makeReplyCopyTo(currentAccount, messageData.to, messageData.cc, opt.isAll);
                    var subject = this._makeSubject(messageData.subject);
                    var body = this._makeBody(messageData);
                    messageData.to = to;
                    messageData.cc = cc;
                    messageData.subject = subject;
                    messageData.body = body;
                    opt.title = subject;
                }
                return opt;
            },
            _makeSubject: function(subject) {
                return "Re: " + subject;
            },
            _makeReplyTo: function(from) {
                var to = isArray(from) ? from : from ? [ from ] : [];
                to = this._normalizeRecipientForEditor(to);
                return to;
            },
            _makeReplyCopyTo: function(account, to, cc, isAll) {
                if (!isAll) {
                    return [];
                }
                var replyCc = [], hasTo = isArray(to) && to.length > 0, hasCC = isArray(cc) && cc.length > 0;
                if (hasTo) {
                    var filterTo = to.filter(function(item) {
                        return !account || account.address !== item.Address;
                    });
                    replyCc = replyCc.concat(filterTo);
                }
                if (hasCC) {
                    replyCc = replyCc.concat(cc);
                }
                replyCc = this._normalizeRecipientForEditor(replyCc);
                return replyCc;
            },
            _makeBody: function(messageData, noQuote) {
                var body = messageData.body, info = this._makeInfo(messageData);
                return this._makeHtmlBody(body, info, noQuote);
            },
            _makeHtmlBody: function(html, info, noQuote) {
                var startString = "", endString = "", body = html || "";
                startString = noQuote ? "<br/>" : "<br/><br/>";
                if (info) {
                    startString += "<div>" + info + "</div>";
                }
                if (noQuote) {
                    startString += "<br/><br/>";
                } else {
                    var blockquoteStyle = "margin: 10px;";
                    blockquoteStyle += "border-left: 1px solid #00a7e1;";
                    blockquoteStyle += "padding: 0 0 0 10px;";
                    blockquoteStyle = 'style="' + blockquoteStyle + '"';
                    startString += "<blockquote " + blockquoteStyle + ">";
                    endString = "</blockquote>";
                }
                body = this._prependHtml(body, startString);
                body = this._appendToHtml(body, endString);
                return body;
            },
            _makeInfo: function(messageData) {
                var from = messageData.from ? [ messageData.from ] : [], date = messageData.sentDate, isHtml = messageData.isHtml, recipients = Recipient.fromRaw(from), info = this._formatDate(date);
                if (recipients.length > 0) {
                    info += " ";
                    info += recipients.map(function(item) {
                        return item.toString();
                    }).join(", ");
                }
                info = info ? info.trim() + ":" : "";
                if (isHtml) {
                    info = kendo.htmlEncode(info);
                }
                return info;
            },
            _normalizeRecipientForEditor: function(recipients) {
                recipients = recipients || [];
                recipients.forEach(function(item) {
                    if (item && !item.Name) {
                        item.Name = item.Address;
                    }
                });
                return recipients;
            },
            _formatDate: function(date) {
                return isDate(date) ? kendo.toString(date, "dd MMM yyyy г., HH:mm") : "";
            }
        });
        var ForwardCommand = ReplyCommand.extend({
            _prepareOptions: function(opt) {
                var messageData = opt.messageData;
                if (messageData) {
                    var subject = this._makeSubject(messageData.subject);
                    var body = this._makeBody(messageData, true, true);
                    messageData.to = [];
                    messageData.cc = [];
                    messageData.subject = subject;
                    messageData.body = body;
                    opt.title = subject;
                }
                return opt;
            },
            _convertToMessageData: function(messageDetails) {
                var messageData = ReplyCommand.fn._convertToMessageData.call(this, messageDetails);
                var attachments = messageDetails.attachments ? messageDetails.attachments.toJSON() : [];
                if (attachments && attachments.length > 0) {
                    messageData.attachments = attachments;
                    messageData.copyAttachments = {
                        folderId: messageDetails.folderId,
                        messageId: messageDetails.id
                    };
                }
                return messageData;
            },
            _makeSubject: function(subject) {
                return "Fwd: " + subject;
            },
            _makeInfo: function(messageData) {
                var from = messageData.from, to = messageData.to, cc = messageData.cc, date = messageData.sentDate, subject = messageData.subject, data = {
                    from: null,
                    date: null,
                    subject: null,
                    to: null,
                    cc: null
                };
                var forwardTemplate = "---------- Пересылаемое сообщение ----------<br/>" + "#if (from) {#От кого: ${from}<br/>#}#" + "#if (date) {#Дата: ${date}<br/>#}#" + "#if (subject) {#Тема: ${subject}<br/>#}#" + "#if (to) {#Кому: ${to}<br/>#}#" + "#if (cc) {#Копия: ${cc}<br/>#}#";
                var template = kendo.template(forwardTemplate);
                data.from = from ? Recipient.fromRaw(from).toString() : null;
                data.subject = subject;
                data.date = this._formatDate(date);
                if (isArray(to) && to.length > 0) {
                    data.to = to.map(function(item) {
                        return item.Address;
                    }).join(", ");
                }
                if (isArray(cc) && cc.length > 0) {
                    data.cc = to.map(function(item) {
                        return item.Address;
                    }).join(", ");
                }
                return template(data);
            }
        });
        var RefreshCommand = Command.extend({
            init: function(options) {
                Command.fn.init.call(this, options);
                this.noProgressBar = true;
            },
            exec: function() {
                var that = this, opt = this.options, mailclient = this.mailclient, accountId = mailclient.account.getCurrentId(), folderList = mailclient.folderList, messageList = mailclient.messageList, noRefreshFolders = opt.noRefreshFolders, noRefreshMessages = opt.noRefreshMessages, folderDefer, messageDefer;
                this._baseExec();
                this._deferred(true);
                if (!accountId) {
                    this._error(new Error("Could not found current account id."), false);
                    return this._deferred();
                }
                this._pending();
                var folderListInProgress = folderList.dataSource.inProgress();
                var messageListInProgress = messageList.dataSource.inProgress();
                if (!folderListInProgress && !noRefreshFolders) {
                    folderDefer = folderList.fetch(accountId).then(function() {
                        folderList.reselect();
                    });
                }
                if (!messageListInProgress && !noRefreshMessages) {
                    messageDefer = messageList.dataSource.read().then(function() {
                        messageList.reselect();
                        that._maybeCloseViewedMessage();
                    });
                }
                if (folderDefer || messageDefer) {
                    $.when(folderDefer, messageDefer).done(this._done).fail(this._error);
                } else {
                    this._done();
                }
                return this._deferred();
            },
            _maybeCloseViewedMessage: function() {
                var mailclient = this.mailclient, viewedMessage = mailclient.messageDetails.getCurrent();
                if (viewedMessage && !mailclient.messageList.find(viewedMessage.id)) {
                    mailclient.messageDetails.close();
                }
            }
        });
        var SaveFolderCommand = Command.extend({
            init: function(options) {
                Command.fn.init.call(this, options);
                this._handleSave = proxy(this._handleSave, this);
            },
            exec: function() {
                var opt = this.options, model = opt.model, id = model ? model.id : opt.folderId, parentId = model ? model.parentId : opt.parentId, displayName = model ? model.displayName : opt.displayName;
                this._baseExec();
                this._deferred(true);
                this._pending();
                displayName = displayName === undefined ? null : displayName || "";
                parentId = parentId === undefined ? null : parentId || "";
                if (id) {
                    this.service.updateFolder(id, displayName, parentId).then(this._handleSave).done(this._done).fail(this._error);
                } else {
                    this.service.createFolder(displayName, parentId).then(this._handleSave).done(this._done).fail(this._error);
                }
                return this._deferred();
            },
            _handleSave: function(req) {
                var opt = this.options, model = opt.model, id = model ? model.id : opt.folderId, resp = req ? req.response : null, rawData = (resp || {}).Current || resp, subItems = (resp || {}).SubFolders || [];
                if (rawData && model) {
                    if (model.id) {
                        this._clearMessages(model);
                    }
                    this._updateModel(model, rawData);
                } else if (rawData && id) {
                    model = this._findModel(id);
                    if (model) {
                        this._clearMessages(model);
                        this._updateSubtreeAndSelf(model, rawData, subItems);
                    }
                }
            },
            _findModel: function(id) {
                return this.mailclient.folderList.find(id);
            },
            _updateModel: function(model, rawData) {
                var folderList = this.mailclient.folderList, ds = folderList.dataSource;
                ds.updateIdentifiers(model, rawData);
            },
            _updateSubtreeAndSelf: function(model, rawData, subItems) {
                subItems = subItems || [];
                var folderList = this.mailclient.folderList, ds = folderList.dataSource;
                if (model && rawData) {
                    ds.updateIdentifiers(model, rawData);
                    if (subItems.length > 0) {
                        ds.updateSubtree(model, subItems);
                    }
                }
            },
            _clearMessages: function(model) {
                var folderList = this.mailclient.folderList, messageList = this.mailclient.messageList;
                if (folderList.isSubtreeOrSelfSelected(model)) {
                    folderList.clearSelection();
                    messageList.clearAll();
                }
            }
        });
        var NewFolderCommand = SaveFolderCommand.extend({
            init: function(options) {
                SaveFolderCommand.fn.init.call(this, options);
            }
        });
        var UpdateFolderCommand = SaveFolderCommand.extend({
            init: function(options) {
                SaveFolderCommand.fn.init.call(this, options);
            }
        });
        var RemoveFolderCommand = Command.extend({
            init: function(options) {
                Command.fn.init.call(this, options);
                this._handleRemove = proxy(this._handleRemove, this);
            },
            exec: function() {
                var that = this, opt = this.options, folderId = opt.folderId;
                this._baseExec();
                this._deferred(true);
                if (!folderId) {
                    this._error(new Error("Parameter folderId is required."), false);
                    return this._deferred();
                }
                this._pending();
                this._removeSubFolders(folderId).done(function() {
                    that.service.deleteFolder(folderId).then(that._handleRemove).done(that._done).fail(that._error);
                }).fail(this._error);
                return this._deferred();
            },
            _removeSubFolders: function(folderId) {
                var that = this, folderList = this.mailclient.folderList, folderToRemove = folderList.find(folderId), subTree = folderToRemove ? folderList.dataSource.flattenArrayBy(folderToRemove) : [], reverseSubTree = subTree.reverse(), deferArray = [];
                reverseSubTree.forEach(function(subFolder) {
                    deferArray.push(that.service.deleteFolder(subFolder.id).then(function() {
                        folderList.remove(subFolder);
                    }));
                });
                return $.when.apply(null, deferArray);
            },
            _handleRemove: function() {
                var opt = this.options, folderId = opt.folderId, folderList = this.mailclient.folderList, folderToRemove = folderList.find(folderId);
                if (folderToRemove) {
                    folderList.remove(folderToRemove);
                }
            }
        });
        var RemoveFolderToTrashCommand = Command.extend({
            init: function(options) {
                Command.fn.init.call(this, options);
                this._handleMoveFolder = proxy(this._handleMoveFolder, this);
                this._updateState = proxy(this._updateState, this);
            },
            exec: function() {
                var opt = this.options, folderId = opt.folderId;
                this._baseExec();
                this._deferred(true);
                if (!folderId) {
                    this._error(new Error("Parameter folderId is required."), false);
                    return this._deferred();
                }
                this._pending();
                this._fetchFolders().done(this._handleMoveFolder).fail(this._error);
                return this._deferred();
            },
            _handleMoveFolder: function(req) {
                var that = this, opt = this.options, folderId = opt.folderId, folders = req.response || [], srcFolder = that._findFolder(folderId, folders);
                if (srcFolder) {
                    var destFolder = that._findFolderByType(FolderType.Trash, folders);
                    if (!destFolder) {
                        this._error(new Error("Trash folder not found."));
                        return;
                    }
                    if (srcFolder.FolderId === destFolder.FolderId) {
                        this._error(new Error("Cannot move folder to Trash."));
                        return;
                    }
                    var folderName = that._createNewFolderName(srcFolder, destFolder, folders);
                    this._moveFolderToTrash(srcFolder.FolderId, destFolder.FolderId, folderName).done(this._done).fail(this._error);
                } else {
                    this._error(new Error("Folder not found with id: " + folderId));
                }
            },
            _findFolder: function(id, folders) {
                return first(folders.filter(filterByField("FolderId", id)));
            },
            _findFolderByType: function(type, folders) {
                return first(folders.filter(filterByField("Type", type)));
            },
            _fetchFolders: function() {
                return this.service.getFolders();
            },
            _createNewFolderName: function(srcFolder, destFolder, folders) {
                var newFolderName = srcFolder.Title || "";
                var children = folders.filter(filterByField("ParentId", destFolder.FolderId));
                if (!children.length) {
                    return newFolderName;
                }
                var nameMatches = children.filter(function(item) {
                    return (item.Title || "").toUpperCase() === newFolderName.toUpperCase();
                });
                if (!nameMatches.length) {
                    return newFolderName;
                }
                var nameExpression = new RegExp(newFolderName + "_(\\d+)", "i");
                var indexes = children.map(function(item) {
                    var matches = nameExpression.exec(item.Title || "");
                    return matches !== null && matches[1] ? +matches[1] : 0;
                });
                var maxIndex = Math.max.apply(null, indexes) || 1;
                newFolderName = newFolderName + "_" + (maxIndex + 1);
                return newFolderName;
            },
            _moveFolderToTrash: function(srcFolderId, destFolderId, newFolderName) {
                var folderList = this.mailclient.folderList, srcFolder = folderList.find(srcFolderId), destFolder = folderList.find(destFolderId), oldParent = srcFolder ? folderList.getParent(srcFolder) : null;
                var command = this.mailclient.command(Commands.UpdateFolder, {
                    folderId: srcFolderId,
                    parentId: destFolderId,
                    displayName: newFolderName
                });
                return command.exec().done(function() {
                    if (srcFolder && destFolder && oldParent) {
                        srcFolder.setDisplayName(newFolderName);
                        folderList.changeParent(srcFolder, destFolder, oldParent);
                    }
                });
            }
        });
        var ClearAllFolderCommand = Command.extend({
            init: function(options) {
                Command.fn.init.call(this, options);
                this._handleClearAll = proxy(this._handleClearAll, this);
            },
            exec: function() {
                var opt = this.options, folderId = opt.folderId;
                this._baseExec();
                this._deferred(true);
                if (!folderId) {
                    this._error(new Error("Parameter folderId is required."), false);
                    return this._deferred();
                }
                this._pending();
                this._handleClearAll();
                return this._deferred();
            },
            _handleClearAll: function() {
                var opt = this.options, folderId = opt.folderId, folderList = this.mailclient.folderList, folder = folderList.find(folderId), mailclient = this.mailclient;
                if (folder) {
                    var commandDefers = [];
                    var deleteAllCommand = mailclient.command(Commands.DeleteAll, {
                        folderId: folderId
                    });
                    commandDefers.push(deleteAllCommand.exec());
                    var ds = folder.children;
                    if (ds) {
                        ds.data().forEach(function(item) {
                            var removeFolderCommand = mailclient.command(Commands.RemoveFolder, {
                                folderId: item.id
                            });
                            commandDefers.push(removeFolderCommand.exec());
                        });
                    }
                    $.when.apply(null, commandDefers).done(this._done).fail(this._error);
                } else {
                    this._error(new Error("Error when clear all folder."), true);
                }
            }
        });
        var MarkAsReadFolderCommand = Command.extend({
            init: function(options) {
                Command.fn.init.call(this, options);
                this._updateState = proxy(this._updateState, this);
            },
            exec: function() {
                var opt = this.options, folderId = opt.folderId;
                this._baseExec();
                this._deferred(true);
                if (!folderId) {
                    this._error(new Error("Parameter folderId is required."), false);
                    return this._deferred();
                }
                this._pending();
                this.service.markAsReadAll(folderId).then(this._updateState).done(this._done).fail(this._error);
                return this._deferred();
            },
            _updateState: function() {
                var opt = this.options, folderId = opt.folderId, folderList = this.mailclient.folderList, messageList = this.mailclient.messageList, currentFolder = folderList.getCurrent();
                if (currentFolder && currentFolder.id === folderId) {
                    messageList.dataSource.data().forEach(function(message) {
                        message.setRead(true);
                    });
                    currentFolder.setUnreadItemCount(0);
                }
            }
        });
        var RefreshMessagesCommand = Command.extend({
            init: function(options) {
                Command.fn.init.call(this, options);
                this.noProgressBar = true;
            },
            exec: function() {
                var that = this, opt = this.options, folderId = opt.folderId, messageId = opt.messageId, mailclient = this.mailclient, messageList = mailclient.messageList, currentFolder = mailclient.folderList.getCurrent(), isRefresh = !folderId || currentFolder && currentFolder.id === folderId;
                this._baseExec();
                this._deferred(true);
                if (isRefresh) {
                    this._pending();
                    messageList.dataSource.read().then(function() {
                        messageList.reselect();
                        var defer = that._refreshMessage(messageId);
                        if (defer) {
                            defer.done(that._done).fail(that._error);
                        } else {
                            that._done();
                        }
                    }).fail(this._error);
                } else {
                    this._done();
                }
                return this._deferred();
            },
            _refreshMessage: function(messageId) {
                var mailclient = this.mailclient, isRefresh = mailclient.messageDetails.isCurrent(messageId);
                return isRefresh ? this._openMessage(messageId) : null;
            },
            _openMessage: function(messageId) {
                var mailclient = this.mailclient, service = this.service, accountId = service.getAccountId(), folderId = service.getFolderId();
                if (accountId && folderId && messageId) {
                    return mailclient.messageDetails.show(accountId, folderId, messageId);
                }
                return null;
            }
        });
        var EditDraftCommand = ComposeCommand.extend({
            init: function(options) {
                options = extend(options || {}, {
                    isSignature: false,
                    isMaximize: true
                });
                ComposeCommand.fn.init.call(this, options);
                this._handleEditDraft = proxy(this._handleEditDraft, this);
            },
            exec: function() {
                var opt = this.options, messageId = opt.messageId, messageDetails = opt.messageDetails, service = this.service;
                if (messageId) {
                    this._deferred(true);
                    this.progressBar(true);
                    this._pending();
                    service.getMessageDetails(messageId).done(this._handleEditDraft).fail(this._error);
                    return this._deferred();
                } else if (messageDetails) {
                    this._prepareOptions(messageDetails);
                }
                return ComposeCommand.fn.exec.call(this);
            },
            _handleEditDraft: function(messageDetails) {
                var deferred = this._deferred();
                this._hideProgress();
                this.progressBar(false);
                this._prepareOptions(messageDetails);
                ComposeCommand.fn.exec.call(this).done(deferred.resolve).fail(deferred.reject);
            },
            _prepareOptions: function(messageDetails) {
                var opt = this.options, localization = this.widgetOpt.localization;
                extend(opt, {
                    title: messageDetails.subject || localization.draftTitle,
                    messageData: this._convertToMessageData(messageDetails)
                });
            },
            _convertToMessageData: function(messageDetails) {
                var message = messageDetails.toJSON();
                var messageData = {
                    to: message.to,
                    cc: message.cc,
                    bcc: message.bcc,
                    subject: message.subject,
                    body: message.body,
                    sentDate: message.sentDate,
                    isHtml: message.isHtml,
                    messageId: messageDetails.id,
                    folderId: message.folderId,
                    attachments: message.attachments,
                    highPriority: message.highPriority,
                    confirmRead: message.confirmRead,
                    confirmDelivery: message.confirmDelivery
                };
                return messageData;
            }
        });
        var EditAsNewCommand = EditDraftCommand.extend({
            init: function(options) {
                EditDraftCommand.fn.init.call(this, options);
            },
            _prepareOptions: function(messageDetails) {
                var opt = this.options;
                extend(opt, {
                    messageData: this._convertToMessageData(messageDetails)
                });
            },
            _convertToMessageData: function(messageDetails) {
                var message = messageDetails.toJSON();
                var attachments = messageDetails.attachments ? messageDetails.attachments.toJSON() : [];
                var messageData = {
                    to: message.to,
                    cc: message.cc,
                    bcc: message.bcc,
                    subject: message.subject,
                    body: message.body,
                    sentDate: message.sentDate,
                    isHtml: message.isHtml,
                    highPriority: message.highPriority,
                    confirmRead: message.confirmRead,
                    confirmDelivery: message.confirmDelivery
                };
                if (attachments && attachments.length > 0) {
                    messageData.attachments = attachments;
                    messageData.copyAttachments = {
                        folderId: messageDetails.folderId,
                        messageId: messageDetails.id
                    };
                }
                return messageData;
            }
        });
        var EditSettingsCommand = Command.extend({
            init: function(options) {
                Command.fn.init.call(this, options);
                this.noProgressBar = true;
            },
            exec: function() {
                var that = this, opt = this.options, settings = opt.settings, service = this.service, widgetOpt = this.widgetOpt, dialogsOpt = widgetOpt.dialogs;
                this._baseExec();
                this._deferred(true);
                if (!settings) {
                    this._error(new Error("settings parameter is required."), false);
                    return this._deferred();
                }
                if (settings instanceof ObservableObject) {
                    settings = settings.toJSON();
                }
                var viewModel = new SettingsDlgViewModel(settings, service);
                var dialog = createWndDialog(dialogsOpt.editSettings, viewModel, true);
                dialog.onClose(function(dialogResult) {
                    if (dialogResult) {
                        var values = viewModel.settings.toJSON();
                        if (opt.settings instanceof ObservableObject) {
                            that._updateObservableObject(opt.settings, values);
                        } else {
                            that._updateObject(opt.settings, values);
                        }
                    }
                    that._done();
                });
                dialog.show();
                viewModel.initWnd(dialog.getWnd());
                return this._deferred();
            },
            _updateObservableObject: function(obj, values) {
                Object.keys(values).forEach(function(key) {
                    obj.set(key, values[key]);
                });
            },
            _updateObject: function(obj, values) {
                extend(obj, values);
            }
        });
        registerCommand(Commands.MoveTo, MoveToCommand);
        registerCommand(Commands.MoveAll, MoveAllCommand);
        registerCommand(Commands.DeleteAll, DeleteAllCommand);
        registerCommand(Commands.Delete, DeleteCommand);
        registerCommand(Commands.AddFlag, AddFlagCommand);
        registerCommand(Commands.RemoveFlag, RemoveFlagCommand);
        registerCommand(Commands.MarkAsRead, MarkAsReadCommand);
        registerCommand(Commands.MarkAsUnread, MarkAsUnreadCommand);
        registerCommand(Commands.Compose, ComposeCommand);
        registerCommand(Commands.Reply, ReplyCommand);
        registerCommand(Commands.Forward, ForwardCommand);
        registerCommand(Commands.Refresh, RefreshCommand);
        registerCommand(Commands.NewFolder, NewFolderCommand);
        registerCommand(Commands.UpdateFolder, UpdateFolderCommand);
        registerCommand(Commands.RemoveFolder, RemoveFolderCommand);
        registerCommand(Commands.RemoveFolderToTrash, RemoveFolderToTrashCommand);
        registerCommand(Commands.ClearAllFolder, ClearAllFolderCommand);
        registerCommand(Commands.MarkAsReadFolder, MarkAsReadFolderCommand);
        registerCommand(Commands.RefreshMessages, RefreshMessagesCommand);
        registerCommand(Commands.EditDraft, EditDraftCommand);
        registerCommand(Commands.EditSettings, EditSettingsCommand);
        registerCommand(Commands.EditAsNew, EditAsNewCommand);
    })(window.kendo.jQuery);
    return window.kendo;
}, typeof define == "function" && define.amd ? define : function(a1, a2, a3) {
    "use strict";
    (a3 || a2)();
});

(function(f, define) {
    "use strict";
    define("kendo.mailclient.binders", [], f);
})(function() {
    "use strict";
    (function($, undefined) {
        var kendo = window.kendo, Binder = kendo.data.Binder, binders = kendo.data.binders;
        binders.focus = Binder.extend({
            refresh: function() {
                var value = this.bindings.focus.get();
                if (value) {
                    $(this.element).focus();
                } else {
                    $(this.element).blur();
                }
            }
        });
        binders.focusIn = Binder.extend({
            refresh: function() {
                var value = this.bindings.focusIn.get();
                if (value) {
                    $(this.element).focus();
                }
            }
        });
    })(window.kendo.jQuery);
    return window.kendo;
}, typeof define == "function" && define.amd ? define : function(a1, a2, a3) {
    "use strict";
    (a3 || a2)();
});

(function(f, define) {
    "use strict";
    define("kendo.mailclient.core", [ "kendo.mailclient", "kendo.mailclient.ns", "kendo.mailclient.utils", "kendo.mailclient.constants", "kendo.mailclient.iframe", "kendo.mailclient.multitags", "kendo.mailclient.tooltip", "kendo.mailclient.upload", "kendo.mailclient.window", "kendo.mailclient.resize-handle", "kendo.mailclient.preview", "kendo.mailclient.listview", "kendo.mailclient.contextmenu", "kendo.mailclient.droptargetarea", "kendo.mailclient.modal", "kendo.mailclient.models", "kendo.mailclient.data", "kendo.mailclient.models", "kendo.mailclient.view-models", "kendo.mailclient.commands", "kendo.mailclient.commandutil", "kendo.mailclient.binders", "kendo.mailclient.events" ], f);
})(function() {
    "use strict";
    return window.kendo;
}, typeof define == "function" && define.amd ? define : function(a1, a2, a3) {
    "use strict";
    (a3 || a2)();
});