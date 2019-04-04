var WrapWidget = kendo.Class.extend({
    composite: null,
    hotkeysObj: null,

    init: function (id, desc, type) {
        this.id = id;
        this.desc = desc;
        this.type = type;
    },
    widget: function () {
        if (this.id && this.type) {
            if (this.type == 'kendoTreeList' && this.id.id)
                return $("#" + this.id.id).data(this.type)
            else
                return $("#" + this.id).data(this.type);
        } else {
            return null;
        }
    },
    element: function () {
        if (this.widget())
            return this.widget().element;
        else
            return null;
    },
    onNeighbourWidgetChanged: function (e) { },
    destroy: function () {
        if (this.hotkeysObj && this.hotkeysObj != null) {
            this.hotkeysObj.destroy(this.id);
            this.hotkeysObj = null;

            if (this.widget())
                this.widget().destroy();
        }
    },
    getKeyCookie: function (mnemonic, key) {
        return mnemonic + "." + key;
    },
    getCookie: function (mnemonic, key, def) {
        return Cookies.get(this.getKeyCookie(mnemonic, key)) || def;
    },
    setCookie: function (mnemonic, key, val) {
        Cookies.set(this.getKeyCookie(mnemonic, key), val);
    },
    inithotkeys: function (array) {
        this.hotkeysObj = Object.create(hotkeysObject);
        this.hotkeysObj.init(this.id, array);
    }
});

var CompositeControl = WrapWidget.extend({
    init: function (id, desc) {
        this.id = id;
        this.widgets = {};
        WrapWidget.fn.init.call(this, id, desc || "Composite");
    },
    _element: null,
    element: function () {
        var self = this;

        if (self._element === null) {
            var $el = $("#" + self.id);

            if ($el.length >= 0) {
                $el.on("remove",
                    function () {
                        self.destroy();
                    });

                self._element = $el;
            }
        }

        return this._element;
    },
    destroy: function () {
        var widgets = this.widgets;
        for (var id in widgets) {
            if (widgets.hasOwnProperty(id)) {
                widgets[id].destroy();
            }
        }
    },
    // obj /WrapWidget/ - виджет
    registerWidget: function (obj) {
        if (obj === this) {
            throw new Error("Циклическая ссылка");
        }
        this.widgets[obj.id] = obj;
        obj.composite = this;
    },
    // obj /WrapWidget/ - виджет
    removeWidget: function (obj) {
        delete this.widgets[obj.id];
    },
    // -> obj /WrapWidget/ - виджет
    // name /string/ - имя виджета
    getWidget: function (name) {
        var widgets = this.widgets;
        for (var id in widgets) {
            if (widgets.hasOwnProperty(id)) {
                if (widgets[id].desc === name) {
                    return widgets[id];
                }
            }
        }

        return null;
    },
    // Вызывается при изменении любого зарегистрированного в композите виджета
    // e.sender /WrapWidget/ - источник
    // e.event /string/ - событие
    // e.params /object/ - параметры
    onWidgetChanged: function (e) {
        // Оповестим все виджиты
        for (var id in this.widgets) {
            if (this.widgets[id] !== e.sender) {
                this.widgets[id].onNeighbourWidgetChanged(e);
            }
        }
        if (e.sender !== this) {
            this.onChildWidgetChanged(e);
        }
    },
    onChildWidgetChanged: function (e) { },
});

var WrapViewModel = WrapWidget.extend({
    init: function (id, desc, typeDialog) {
        this.typeDialog = typeDialog;
        WrapWidget.fn.init.call(this, id, desc, "pbaForm");
    },
    widget: function () {
        return $("#" + this.id).find("form").data(this.type);
    },
    isModal: function () {
        return this.typeDialog === "Modal";
    },
    element: function () {
        return $("#" + this.id).find("form");
    }
});

var ListView = WrapWidget.extend({
    baseType: 'ListView',
    mnemonic: null,
    preset: null,
    transport: null,
    isCategorizedItem: false,
    _categoryID: null,
    _add: null,
    _edit: null,
    _transportReadUrl: null,
    _transportCreateUrl: null,
    init: function (options) {
        this.mnemonic = options.mnemonic;

        this.isCategorizedItem = options.isCategorizedItem || false;

        this.transport = {
            read: {
                controller: 'listview',
                data: {}
            },
            createDefault: {
                controller: 'standard',
                data: {}
            }
        }

        WrapWidget.fn.init.call(this, options.id, options.desc, options.type);
    },
    onNeighbourWidgetChanged: function (e) {
        var listview = this;

        if (e.sender.desc === 'Composite') {
            if (e.event === 'init') {
                if (e.params && e.sender.listView() === listview) {
                    listview.setTransport(e.params.transport);
                    listview._add = e.params.add;
                    listview.customParams(e.params.customParams);

                }

                if (!listview.isCategorizedItem) {
                    listview.bind();
                }
            }
        }
    },
    bind: function () {
        this.dataSourceRead();
    },
    getTransportReadUrl: function () {
        if (!this._transportReadUrl) {
            this._transportReadUrl = URI.expand('/api/{controller}/{mnemonic}/{type}/{categorized}',
                {
                    controller: this.transport.read.controller,
                    type: this.type,
                    mnemonic: this.mnemonic,
                    categorized: this.isCategorizedItem ? 'categorized' : null
                });
        }

        return this._transportReadUrl;
    },
    getTransportCreateUrl: function (mnemonic) {
        if (!this._transportCreateUrl) {
            this._transportCreateUrl = URI.expand('/api/{controller}/{mnemonic}/create_default',
                {
                    controller: this.transport.createDefault.controller,
                    type: this.type,
                    mnemonic: mnemonic || this.mnemonic,
                    categorized: this.isCategorizedItem ? 'categorized' : null
                }) + '?' + $.param(this.transport.createDefault.data);
        }

        return this._transportCreateUrl;
    },
    setTransport: function (transport) {
        if (!transport) return;

        if (transport.read) {
            if (transport.read.controller)
                this.transport.read.controller = transport.read.controller;

            if (transport.read.data)
                $.extend(this.transport.read.data, transport.read.data);

            this._transportReadUrl = null;
        }

        if (transport.createDefault) {
            if (transport.createDefault.controller)
                this.transport.createDefault.controller = transport.createDefault.controller;

            if (transport.createDefault.data)
                $.extend(this.transport.createDefault.data, transport.createDefault.data);

            this._transportCreateUrl = null;
        }
    },
    setCategoryID: function (id) {
        if (this.isCategorizedItem) {
            this._categoryID = id;
        }
    },
    getCategoryID: function () {
        return this._categoryID;
    },
    addEntity: function (params) {
        var listview = this;

        var initProps = $.extend(this.composite.params.initProps || {}, params.initProps || {});

        if (this.isCategorizedItem && this._categoryID)
            initProps.CategoryID = this._categoryID;

        var createDefault = function (mnemonic) {
            return $.ajax({
                url: listview.getTransportCreateUrl(mnemonic),
                method: 'GET',
                contentType: 'application/json',
                dataType: 'json'
            });
        }

        var mnemonic = this.mnemonic;
        if (('SibProjectMenuList' == mnemonic) || ('Appraisal' == mnemonic) || ('PropertyComplexIO' == mnemonic) || ('PropertyComplexIO_InventoryObject' == mnemonic) || ('InventoryObject' == mnemonic)) {
            // TODO проблем плейс
            // NOTE хот фикс для TFS 6960
            createDefault = null;
        }
        pbaAPI.openDetailView(this.mnemonic,
            {
                wid: this.id,
                initProps: initProps,
                createDefault: createDefault,
                callback: function (e) {
                    if (e.type === 'save' || e.type === 'apply') {
                        if (listview._add) {
                            listview._add(e).done(function (res) {
                                if (res.error)
                                    pbaAPI.errorMsg(res.error);

                                params.callback(e);
                            });
                        } else {
                            params.callback(e);
                        }
                        if (params && params._initProp && params._initProp.On && e.model && e.model.ID)
                            params._initProp.On(e.type, mnemonic, e.model.ID);
                    }
                }
            });
    },
    getUrlParametr: function (key) {
        var url = this.widget().dataSource.transport.options.read.url;
        return pbaAPI.getUrlParametr(url, key);
    },
    setInitProp: function (pr, value) {
        if (!this._initProp)
            this._initProp = {};
        this._initProp[pr] = value;
    },
    setUrlParametr: function (key, val) {
        this.transport.read.data[key] = val;
    },
    resize: function (height) {
        var $element = this.widget().element;
        $element.height(height);
    },
    getByUid: function (uid) {
        return this.widget().dataSource.getByUid(uid);
    },
    __getById: function (data, id) {
        for (var i = 0; i < data.length; i++) {
            if ('items' in data[i]) {
                var item = this.__getById(data[i].items, id);

                if (item)
                    return item;

                continue;
            }

            if (data[i].ID === id) {
                return data[i];
            }
        }

        return null;
    },
    getById: function (id) {
        return this.__getById(this.getDataSource().data(), id);
    },
    dataItem: function (item) {
        return this.widget().dataItem(item);
    },
    getDataSource: function () {
        return this.widget().dataSource;
    },
    setDataSource: function (ds) {
        this.widget().setDataSource(ds);
    },
    dataSourceRead: function () {

        this.transport.read.cache = false;


        var readData = this.transport.read.data;

        if (this.isCategorizedItem) {
            this.transport.read.data.categoryId = this._categoryID;
            this.transport.read.data.allItems = this.getCookie(this.mnemonic, 'allItems', false);
        }

        if (this.widget().dataSource.transport.options) {
            if (!this.widget().dataSource.transport.options.read.url)
                this.widget().dataSource.transport.options.read.url = this.getTransportReadUrl();

            this.widget().dataSource.transport.options.read.data = function () {
                return readData;
            };
        }

        this.widget().dataSource.transport.options.read.data = function () {
            return readData;
        };

        this.widget().dataSource.read();
    },
    search: function (str) {
        this.transport.read.data.searchStr = str || '';
        this.dataSourceRead();
    },
    customParams: function (objs) {
        this.transport.read.data.customParams = objs || '';
        this.dataSourceRead();
    },
    date: function (dt) {
        this.transport.read.data.date = dt || '';
        this.dataSourceRead();
    },
    filter: function (str) {
        this.transport.read.data.extrafilter = str || '';
        this.dataSourceRead();
    },
    setMnemonicFilterId: function (val) {
        this.transport.read.data.mnemonicFilterId = val;
    },
    getFilter: function () {
        if (!this.widget().dataSource.filter()) {
            this.widget().dataSource.filter(this.getEmptyAndFilter());
        }
        return this.widget().dataSource.filter();
    },
    addFilterAndUpdate: function (newFilter, colName) {
        var curFilter = this.getFilter();
        var cleanedFilter = this._removeFilterForColumnAndGet(curFilter, colName);
        var finalFilter = this._concatFilters(cleanedFilter, newFilter);
        this.widget().dataSource.filter(finalFilter);
    },
    removeFilterForColumnAndUpdate: function (colName) {
        var curFilter = this.getFilter();
        var cleanedFilter = this._removeFilterForColumnAndGet(curFilter, colName);
        this.widget().dataSource.filter(cleanedFilter);
    },
    clearFilter: function () {
        this.widget().dataSource.filter(this.getEmptyAndFilter());
    },
    exportExcel: function () {
        var widget = this.widget();
        var oldDs = widget.dataSource.options.transport.read;
        widget.dataSource.options.transport.read = widget.dataSource.transport.options.read;
        widget.saveAsExcel();
        widget.dataSource.options.transport.read = oldDs;

        //var uri = URI.expand('/api/excel/{mnemonic}/{categorized}',
        //    {
        //        mnemonic: this.mnemonic,
        //        categorized: this.isCategorizedItem ? 'categorized' : null
        //    });

        //uri = uri + '?' + $.param(this.transport.read.data);

        //window.location = uri.toString();
    },
    exportExcelTemplate: function () {
        var objs = this.getSelectItems();
        var isAo = this.mnemonic === "AccountingObjectAndEstateRegistrationObject";
        if (!objs || objs.length === 0)
            return pbaAPI.errorMsg("Выберите заявку.");

        var elIds = [];
        objs.forEach(function (item, i, arr) {
            elIds.push(item.ID);
        });       
        pbaAPI.proxyclient.EUSI.estateRegistrExport({
            elementsIds: elIds.join(),
            isAccountingObject: isAo
        }).then(function (res) {
            if (res.error === 1)
                return pbaAPI.errorMsg(res.message);
            else {
                var data = pbaAPI.base64ToBlob(res.data, res.mimeType);
                pbaAPI.download(data, res.filename, res.mimetype);
                this.dataSourceRead();
            }
        })
    },
    multiEdit: function () {
        var uri = URI.expand('/api/multi_edit/{mnemonic}/{categorized}',
            {
                mnemonic: this.mnemonic,
                categorized: this.isCategorizedItem ? 'categorized' : null
            });
        var filter = "";
        var si = this.getSelectItems();

        for (var i = 0; i < si.length; i++) {
            filter += filter === "" ? "ID~eq~" + si[i].ID : "~or~ID~eq~" + si[i].ID;
        }
        uri = uri + '?' + $.param(this.transport.read.data) + "&filter=" + filter;
        //uri = uri + '?' + $.param(this.transport.read.data);
        //var objectsFunc = $.get(uri).then(function (e) {
        //    if (e.Data) {
        //        return e.Data.map(function (e) {
        //            return { id: e.id || e.ID }
        //        });
        //    } else {
        //        return [];
        //    }
        //});
        var grid = this;
        var objectsFunc = $.ajax({
            url: uri,
            type: "GET",
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        }).then(function (e) {
            if (e.Data) {
                return e.Data.map(function (e) {
                    return { id: e.id || e.ID }
                });
            } else {
                return [];
            }
        })

        pbaAPI.openMultiEdit({
            mnemonic: this.mnemonic,
            objects_func: objectsFunc
        }).then(function () {
            grid.dataSourceRead();
        });
    },
    getSelectItems: function () {
        //--override
    },
    getEmptyAndFilter: function () {
        return { filters: [], logic: 'and' };
    },
    _concatFilters: function (oldFilter, newFilter) {
        if (newFilter && newFilter.filters && newFilter.filters.length > 0) {
            if (oldFilter) {
                if (oldFilter.logic !== 'and') {
                    oldFilter = { filters: [oldFilter, newFilter], logic: 'and' }
                } else {
                    oldFilter.filters.push(newFilter);
                }

            } else {
                oldFilter = { filters: [newFilter], logic: 'and' };
            }
        }
        return oldFilter;
    },
    getFiltersByColName: function (curFilter, colName) {
        var foundedFilters = [];
        if (curFilter.filters && curFilter.filters.length > 0) {
            for (var i = 0; i < curFilter.filters.length; i++) {
                if (curFilter.filters[i].field === colName) {
                    foundedFilters.push(curFilter.filters[i]);
                }
                if (curFilter.filters[i].filters && curFilter.filters[i].filters.length > 0) {
                    var foundedOnStep = this.getFiltersByColName(curFilter.filters[i], colName);
                    if (foundedOnStep && foundedOnStep.length > 0) {
                        foundedFilters = foundedFilters.concat(foundedOnStep);
                    }
                }
            }
        }
        return foundedFilters;
    },
    _removeFilterForColumnAndGet: function (curFilter, columnName) {
        if (!curFilter) {
            curFilter = this.getEmptyAndFilter();
        }

        var foundInRoot = this._removeFilterGroup(curFilter, columnName);
        if (foundInRoot) {
            curFilter = this.getEmptyAndFilter();
        }
        return curFilter;
    },
    _removeFilterGroup: function (filter, fieldName) {
        var foundElementOnLevel = false;
        if (filter.filters) {
            for (var i = 0; i < filter.filters.length; i++) {
                if (filter.filters[i].field === fieldName) {
                    foundElementOnLevel = true;
                    break;
                }
                if (filter.filters[i].filters) {
                    var foundElem = this._removeFilterGroup(filter.filters[i], fieldName);
                    if (foundElem) {
                        filter.filters.splice(i, 1);
                        i--;
                    }
                }
            }
        }
        return foundElementOnLevel;
    },
    //events
    onErrorHandler: function (e) {
        pbaAPI.errorMsg(e.errors);
    },

    onExcelExport: function (e) {
        var rows = e.workbook.sheets[0].rows;

        var nonFooterRows = rows.filter(function (obj) {
            return obj.type !== "group-footer" || obj.type !== "footer";
        });

        for (var i = 0; i < nonFooterRows.length; i++) {
            var row = nonFooterRows[i];

            for (var y = 0; y < row.cells.length; y++) {
                var cell = row.cells[y];

                if (cell.value && typeof cell.value === "object" && !(cell.value instanceof Date)) {
                    cell.value = cell.value.Name || cell.value.ShortName || cell.value.FullName;
                }
            }
        };

        footerRows = rows.filter(function (obj) {
            return obj.type === "group-footer" || obj.type === "footer";
        });

        for (var j = 0; j < footerRows.length; j++) {
            var row = footerRows[j];

            for (var k = 0; k < row.cells.length; k++) {
                var cell = row.cells[k];
                if (cell.value) {
                    //cell.value = cell.value.replace(/\./g, "\\.");
                    cell.value = $(cell.value.replace(/<br\/>/g, "\r\n")).text();
                    //cell.hAlign = "right";
                }
            }
        }

        //set columns format
        var cellsFormats = [];
        if (e.sender.columns) {
            for (i = 0; i < e.sender.columns.length; i++) {
                var column = e.sender.columns[i];
                if (column.template) {
                    if (column.template.includes('dd.MM.yyyy HH:mm:ss')) {
                        cellsFormats.push(i);
                    }
                }
            }
            if (cellsFormats.length > 0) {
                for (var rowIndex = 1; rowIndex < rows.length; rowIndex++) {
                    var row = rows[rowIndex];
                    for (var cellIndex = 0; cellIndex < cellsFormats.length; cellIndex++) {
                        row.cells[cellIndex].format = "dd.MM.yyyy hh:mm:ss";
                    }
                }
            }
        }
        
    },
});

var WrapTreeView = ListView.extend({
    init: function (options) {
        this.lock = false;
        options.type = "kendoTreeView";
        ListView.fn.init.call(this, options);
    },
    root: function () {
        return this.widget().root;
    },
    //select: function (item) { 
    //    if (item) {
    //        item = $(item);

    //        this.widget().select(item);

    //        this.onselect();
    //    }

    //    return $(this.widget().select());
    //},
    select: function (item) {
        if (item) {
            var $node;

            if ($.isNumeric(item)) {
                $node = this.getNodeById(item);
                return this.select($node || 'first');
            } else if (item === 'first') {
                $node = this.root().children(':first');
                return this.select($node);
            }

            this.widget().select(item);

            this.onselect();
        }

        return $(this.widget().select());
    },
    onselect: function (source) {
        if (this.composite != null) {

            var $source;

            if (source)
                $source = $(source);
            else
                $source = this.select();

            this.composite.onWidgetChanged(
                {
                    sender: this,
                    event: "select",
                    params: { dataItem: this.widget().dataItem($source), select: $source }
                });
        }
    },
    expand: function (selectNode) {
        this.widget().expand(selectNode);
    },
    append: function (item, node) {
        this.widget().append(item, node);
    },
    remove: function (item) {
        this.widget().remove(item);
    },
    refresh: function () {
        this.widget().dataSource.read();
    },
    getSelectID: function () {
        var $selectNode = this.select();

        if ($selectNode.length > 0) {
            var id = this.getId($selectNode);
            return parseInt(id);
        }

        return null;
    },
    getId: function (node) {
        return $(node).find('.tree-node').attr('id');
    },
    getNodeById: function (id) {
        var $node = this.root().find('.tree-node[id=' + id + ']');
        if ($node.length === 0) return null;
        return $node;
    },
    //override
    onErrorHandler: function (e) {
        pbaAPI.errorMsg(e.errorThrown.message);
    },
    checkedNodeIds: function (nodes, res) {
        for (var i = 0; i < nodes.length; i++) {
            if (nodes[i].checked) {
                res.push({ ID: nodes[i].id, Name: nodes[i].text });
            }
            if (nodes[i].hasChildren) {
                this.checkedNodeIds(nodes[i].children.view(), res);
            }
        }
    },
    getSelectItems: function () {
        var res = [];
        var multiSelect = this.widget().options.checkboxes;

        if (multiSelect) {
            this.checkedNodeIds(this.widget().dataSource.view(), res);
        } else {
            var node = this.widget().dataItem(this.select());

            if (node)
                res.push({ ID: node.id, Name: node.Title });
        }

        return res;
    },

    expandPath: function (ids) {
        if (ids) {
            this.widget().expandPath(ids);
        }
    }
});

var WrapGrid = ListView.extend({
    init: function (options) {
        this.currentRow = null;
        this.scrollTop = 0;
        this.isInit = false;
        options.type = "kendoGrid";
        ListView.fn.init.call(this, options);
        //this.SelectedItems = [];
    },
    content: function () {
        return this.widget().content;
    },
    table: function () {
        return this.widget().table;
    },
    thead: function () {
        return this.widget().thead;
    },
    tbody: function () {
        return this.widget().tbody;
    },
    resize: function (height) {
        var _fault = 2; //Погрешность, возможно связанняа с border-width
        var $grid = this.widget().element;

        var $content = $grid.find(".k-grid-content");

        $content.height(height - ($grid.outerHeight() - $content.outerHeight(true)) - _fault);
    },
    select: function (select) {
        if (select) {
            this.widget().select(select);
            this.onselect();
        }

        var $select = $(this.widget().select());

        return $select;
    },
    onselect: function () {
        if (this.composite != null) {

            var $select = $(this.widget().select());

            this.composite.onWidgetChanged(
                {
                    sender: this,
                    event: "select",
                    params: { dataItem: this.widget().dataItem($select), select: $select }
                });
        }
    },
    clearSelection: function () {
        this.widget().clearSelection();
    },
    selectUID: function () {
        var $select = this.select();

        if ($select.length > 0) {
            return this.select().attr("data-uid");
        }

        return null;
    },
    selectID: function () {
        var uid = this.selectUID();

        if (uid) {
            return this.getByUid(uid).ID;
        }

        return null;
    },
    activeRow: function () {
        var g = this.widget();

        var boolSelect = false;

        if (this.currentRow != null) {
            var item = this.getById(this.currentRow);

            if (item) {
                var $row = g.tbody.find('tr[data-uid="' + item.uid + '"]');

                if ($row.length > 0) {
                    g.select($row);

                    if (g.content)
                        g.content.scrollTop(this.scrollTop);

                    if (g.tbody.find('tr.k-grouping-row').length > 0) {
                        g.expandRow($row.prev('tr.k-grouping-row'));
                    }

                    boolSelect = true;
                }
            }
        }

        if (!boolSelect) {
            this.selectDefault();
        }
    },
    selectDefault: function () {
        var g = this.widget();
        g.select(g.tbody.find("tr:eq(0)"));
        if (g.content)
            g.content.scrollTop(0);
    },
    initCurrentRow: function () {
        var g = this.widget();
        var $select = g.select();

        if ($select.length > 0) {
            var item = g.dataItem($select);

            this.currentRow = item.ID;

            if (g.content)
                this.scrollTop = g.content.scrollTop();
        } else {
            this.currentRow = null;
            this.scrollTop = 0;
        }
    },
    setContentType: function (contentType) {
        var g = this.widget();
        g.dataSource.transport.options.update.contentType = contentType;
        g.dataSource.transport.options.create.contentType = contentType;
        g.dataSource.transport.options.destroy.contentType = contentType;

        g.dataSource.transport.parameterMap = function (options, type) {
            if (type !== "read") {
                return kendo.stringify(options);
            }

            return g.dataSource.transport.options.parameterMap.call(g, options, type);
        };
    },
    getTextByValue: function (data, column) {
        var key = "_values_collection_col_" + column;
        var grid = this.widget();
        var numcol = null;

        if (typeof column == 'number') {
            numcol = column;
            column = grid.columns[i].field;

        } else {
            for (var i = 0; i < grid.columns.length; i++) {
                if (grid.columns[i].field == column) {
                    numcol = i;
                    break;
                }
            }
        }

        if (!data[column]) {
            return "";
        }

        if (numcol) {
            if (!this[key]) {
                var collection = {};
                var valuesCollection = grid.options.columns[numcol].values;

                for (var value in valuesCollection) {
                    collection[valuesCollection[value].value] = valuesCollection[value].text;
                }

                this[key] = collection;
            }

            return this[key][data[column]];
        }
    },
    removeRow: function (item) {
        this.widget().removeRow(item);
    },
    setMultiSelect: function (selectable) {
        this.widget().selectable.options.multiple = selectable;
    },
    allItems: function (val) {

    },
    //override
    dataSourceRead: function () {
        var columns = $.map(this.widget().columns,
            function (n, i) {
                if (!n.hidden)
                    return n.field;

                return null;
            });

        this.transport.read.data["columns"] = columns;

        ListView.fn.dataSourceRead.call(this);
    },
    getSelectItems: function () {
        var $select = this.select();

        if ($select.length > 0) {
        //if ($select.length > 0 || this.SelectedItems.length > 0) {
            var res = new Array($select.length);

            for (var i = 0; i < $select.length; i++) {
                res[i] = this.dataItem($select[i]);
            }

            return res;
            //this.SelectedItems = [...new Set(this.SelectedItems.concat(res))]
            //    .filter((item) => {
            //    return this.widget()._selectedIds[item.ID];
            //});

            //return this.SelectedItems;
        }

        return null;
    }
});

var WrapScheduler = ListView.extend({
    init: function (options) {
        this.selectEvent = null;
        this.selectStart = null;
        this.selectEnd = null;
        options.type = "kendoScheduler";
        ListView.fn.init.call(this, options);
    },
    content: function () {
        return this.widget().content;
    },
    resize: function (height) {
        console.log("resize h: " + height);
        this.widget().element.height(height);
        this.widget().resize(true);
    },
    occurrenceByUid: function (uid) {
        return this.widget().occurrenceByUid(uid);
    },
    openRecurringDialog: function (params) {
        var wid = "recurring_dialog_" + this.id;

        var $w = $("#" + wid);

        if ($w.length == 0) {
            $("body").append(
                '<div id="' +
                wid +
                '" class="k-popup-edit-form k-window-content k-content">' +
                '<div class="common-form">' +
                '<p class="k-popup-message">Изменить повтор. событие?</p>' +
                '<div class="k-edit-buttons k-state-default">' +
                '<a class="k-button k-scheduler-current" href="#">Только текущую запись</a>' +
                '<a class="k-button k-scheduler-series" href="#">Все записи данной серии</a>' +
                '</div>' +
                '</div>' +
                '</div>');

            $w = $("#" + wid);

            $w.find("a.k-scheduler-current").on("click",
                function () {
                    $w.data("kendoWindow").close();
                    params.current();
                });
            $w.find("a.k-scheduler-series").on("click",
                function () {
                    $w.data("kendoWindow").close();
                    params.series();
                });

            $w.kendoWindow({
                title: "Редактировать",
                modal: true,
                visible: false
            });
        }

        var wnd = $w.data("kendoWindow");

        wnd.center();
        wnd.open();
    },
    view: function () {
        return this.widget().view();
    },
    slotByElement: function (el) {
        return this.widget().slotByElement(el);
    },
    dataSourceRead: function () {
        var scheduler = this;

        //todo: ???
        var schedulerView = scheduler.widget().view();
        var startPeriod = new Date(schedulerView.startDate());
        var endPeriod = new Date(schedulerView.endDate());

        if (startPeriod.getTime() === endPeriod.getTime()) {
            endPeriod.setDate(endPeriod.getDate() + 1);
        }

        scheduler.transport.read.data["start"] = startPeriod;
        scheduler.transport.read.data["end"] = endPeriod;

        ListView.fn.dataSourceRead.call(scheduler);
    },
    select: function () {
        return this.widget().select();
    },
    removeEvent: function (uid) {
        this.widget().removeEvent(uid);
    },
    //override
    getSelectItems: function () {
        return [this.selectEvent];
    }
});

var WrapGantt = ListView.extend({
    init: function (options) {
        this.selectedTask = null;
        this.selectStart = options.start;
        this.selectEnd = options.end;
        options.type = "kendoGantt";
        ListView.fn.init.call(this, options);
    },
    content: function () {
        return this.widget().content;
    },
    resize: function (height) {
        var $element = this.widget().element;

        $element.height(height - 22);

        this.widget().resize();
    },
    getByID: function (id) {
        var data = this.data();

        for (var i = 0; i < data.length; i++) {
            if (id === data[i].ID) {
                return data[i];
            }
        }
    },
    data: function () {
        return this.widget().dataSource.data();
    },
    dataSourceRead: function () {
        var gantt = this.widget();
        var dependencies = gantt.dependencies;

        this.transport.read.data['start'] = this.selectStart.toJSON();
        this.transport.read.data['end'] = this.selectEnd.toJSON();

        ListView.fn.dataSourceRead.call(this);

        if (dependencies.options.transport && dependencies.options.transport.read) {
            dependencies.read();
        }
    },
    addEntity: function (params) {
        if (this.isCategorizedItem && this._categoryID)
            initProps.CategoryID = this._categoryID;

        pbaAPI.openDetailView(this.mnemonic,
            {
                wid: this.id,
                initProps: params.initProps,
                callback: function (e) {
                    if (e.type === "save" || e.type === "apply") {
                        params.callback(e);
                    }
                }
            });
    }
});

var WrapSplitter = WrapWidget.extend({
    init: function (id, desc) {
        WrapWidget.fn.init.call(this, id, desc, "kendoSplitter");
    },
    resize: function (height) {
        var widget = this.widget();
        if (!widget) return;

        widget.element
            .height(height)
            .trigger("resize");

        //hack
        widget.element.find(".k-pane:first").height(height + 5);

        if (this.composite != null) {

            this.composite.onWidgetChanged(
                {
                    sender: this,
                    event: "resize",
                    params: { height: height }
                });
        }
    },
    toggle: function () {
        var widget = this.widget();
        if (!widget) return;

        widget.toggle('.k-pane:first');
    },
    collapse: function () {
        var widget = this.widget();
        if (!widget) return;

        widget.collapse('.k-pane:first');
    },
    expand: function () {
        var widget = this.widget();
        if (!widget) return;

        widget.expand('.k-pane:first');
    }
});

var WrapWindow = WrapWidget.extend({
    init: function (id, desc) {
        WrapWidget.fn.init.call(this, id, desc, "kendoWindow");
    },
    open: function () {
        this.widget().open();
    },
    center: function () {
        this.widget().center();
    },
    close: function () {
        this.widget().close();
    }
});

var WrapToolbar = WrapWidget.extend({
    init: function (id, desc) {
        this._customCommands = [];

        WrapWidget.fn.init.call(this, id, desc, "kendoToolBar");
    },
    enable: function (idbtn, enable) {
        if (this.element().find(idbtn).length === 0) return;
        this.widget().enable(idbtn, enable);
    },
    popupEl: function () {
        return this.widget().popup.element;
    },
    visible: function (idbtn, val) {
        if (val)
            this.widget().show(idbtn);
        else
            this.widget().hide(idbtn);
    },
    initCustomCommands: function (paramCommands) {
        if (!paramCommands || paramCommands.length === 0) return;
        if (this._customCommands.length !== 0) return;
        var i;
        var command;
        var $el;

        var commands = paramCommands.slice();

        for (i = commands.length; i--;) {
            command = commands[i];

            var actionid = '#' + command.id;

            $el = this.element().find(actionid);

            if ($el.length === 0) {
                continue;
            }

            $el.data('command', command);

            $el.click(function () {
                if (!$(this).hasClass('k-state-disabled')) {
                    $(this).data('command').execute();
                }
            });

            this._customCommands.push($el);

            this.visible(actionid, true);

            commands.splice(i, 1);
        }

        if (commands.length === 0) return;

        var id = '#custom_commands';

        var $commands = this.element().find('#custom_commands');

        if ($commands.length === 0) return;

        var $ul = $commands.find('ul');

        $commands.find('a').show();

        for (i = 0; i < commands.length; i++) {
            command = commands[i];
            $el = $('<li><a class="k-button">' + command.text + '</a></li>');
            $el.data('command', command);

            $el.click(function () {
                if (!$(this).hasClass('k-state-disabled')) {
                    $(this).data('command').execute();
                }
            });

            this._customCommands.push($el);

            $ul.append($el);
        }

        this.visible(id, true);
    },
    onNeighbourWidgetChanged: function (e) {
        if (e.sender.desc === 'Composite') {
            if (e.event === 'init') {
                if (e.params)
                    this.initCustomCommands(e.params.customCommands);
            }
        } else if (e.sender.baseType === 'ListView') {
            if (e.event === 'select' || e.event === 'onDataBound') {
                //var selectItems = e.sender.getSelectItems();
                var selectItems = [];

                for (var i = 0; i < this._customCommands.length; i++) {
                    var $action = this._customCommands[i];
                    var command = $action.data('command');

                    command.listView = e.sender;
                    command.selectItems = selectItems || [];

                    var enable = true;

                    if (command.canExecute) {
                        enable = command.canExecute({
                            sender: this,
                            listView: e.sender,
                            selectItems: selectItems || []
                        });
                    }

                    if (enable)
                        $action.removeClass('k-state-disabled');
                    else
                        $action.addClass('k-state-disabled');
                }
            }
        }
    }
});

var WrapContextMenu = WrapWidget.extend({
    init: function (id, desc) {
        WrapWidget.fn.init.call(this, id, desc, "kendoContextMenu");
    },
    enable: function (idbtn, enable) {
        this.widget().enable(idbtn, enable);
    },
    open: function () {
        this.widget().open();
    },
    close: function () {
        this.widget().close();
    }
});

var WrapTreeListView = WrapWidget.extend({
    init: function (id, desc) {
        this.currentRow = null;
        this.scrollTop = 0;
        WrapWidget.fn.init.call(this, id, desc, "kendoTreeList");
    },
    content: function () {
        return this.widget().content;
    },
    table: function () {
        return this.widget().table;
    },
    tbody: function () {
        return this.widget().tbody;
    },
    resize: function (height) {
        var _fault = 2; //Погрешность, возможно связанняа с border-width
        var $grid = this.widget().element;

        var $content = $grid.find(".k-grid-content");

        $content.height(height - ($grid.outerHeight() - $content.outerHeight(true)) - _fault);
    },
    select: function (select) {
        if (select) {
            this.widget().select(select);
            this.onselect();
        }

        var $select = $(this.widget().select());

        return $select;
    },
    onselect: function () {
        if (this.composite != null) {

            var $select = $(this.widget().select());

            this.composite.onWidgetChanged(
                {
                    sender: this,
                    event: "select",
                    params: { dataItem: this.widget().dataItem($select), select: $select }
                });
        }
    },
    clearSelection: function () {
        this.widget().clearSelection();
    },
    selectUID: function () {
        var $select = this.select();

        if ($select.length > 0) {
            return this.select().attr("data-uid");
        }

        return null;
    },
    selectID: function () {
        var uid = this.selectUID();

        if (uid) {
            return this.getByUid(uid).ID;
        }

        return null;
    },
    activeRow: function () {
        var g = this.widget();

        var boolSelect = false;

        if (this.currentRow != null) {
            var data = g.dataSource.data();

            for (var i = 0; i < data.length; i++) {
                if (this.currentRow == data[i].ID) {
                    var row = g.tbody.find("tr[data-uid='" + data[i].uid + "']");

                    if (row.length > 0) {
                        g.select(row);

                        if (g.content)
                            g.content.scrollTop(this.scrollTop);

                        boolSelect = true;
                        break;
                    }
                }
            }
        }

        if (!boolSelect) {
            this.selectDefault();
        }
    },
    selectDefault: function () {
        var g = this.widget();

        g.select(g.tbody.find("tr:eq(0)"));

        if (g.content)
            g.content.scrollTop(0);
    },
    initCurrentRow: function () {
        var g = this.widget();
        var $select = g.select();

        if ($select.length > 0) {
            var item = g.dataItem($select);

            this.currentRow = item.ID;

            if (g.content)
                this.scrollTop = g.content.scrollTop();
        } else {
            this.currentRow = null;
            this.scrollTop = 0;
        }
    },
    setContentType: function (contentType) {
        var g = this.widget();
        g.dataSource.transport.options.update.contentType = contentType;
        g.dataSource.transport.options.create.contentType = contentType;
        g.dataSource.transport.options.destroy.contentType = contentType;

        g.dataSource.transport.parameterMap = function (options, type) {
            if (type !== "read") {
                return kendo.stringify(options);
            }

            return g.dataSource.transport.options.parameterMap.call(g, options, type);
        };
    },
    getTextByValue: function (data, column) {
        var key = "_values_collection_col_" + column;
        var grid = this.widget();
        var numcol = null;

        if (typeof column == 'number') {
            numcol = column;
            column = grid.columns[i].field;

        } else {
            for (var i = 0; i < grid.columns.length; i++) {
                if (grid.columns[i].field == column) {
                    numcol = i;
                    break;
                }
            }
        }

        if (!data[column]) {
            return "";
        }

        if (numcol) {
            if (!this[key]) {
                var collection = {};
                var valuesCollection = grid.options.columns[numcol].values;

                for (var value in valuesCollection) {
                    collection[valuesCollection[value].value] = valuesCollection[value].text;
                }

                this[key] = collection;
            }

            return this[key][data[column]];
        }
    },
    getByUid: function (uid) {
        return this.widget().dataSource.getByUid(uid);
    },
    dataSourceRead: function () {
        this.widget().dataSource.read();
    },
    dataItem: function (item) {
        return this.widget().dataItem(item);
    },
    getFilter: function () {
        if (!this.widget().dataSource.filter()) {
            this.widget().dataSource.filter({ filters: [], logic: "and" });
        }

        return this.widget().dataSource.filter();
    },
    clearFilter: function () {
        this.widget().dataSource.filter({});
    },
    removeRow: function (item) {
        this.widget().removeRow(item);
    },
    //override
    getSelectItems: function () {
        var $select = this.select();

        if ($select.length > 0) {
            var res = new Array($select.length);

            for (var i = 0; i < $select.length; i++) {
                res[i] = this.dataItem($select[i]);
            }

            return res;
        }

        return null;
    }
});

var WrapMap = WrapWidget.extend({
    init: function (id, desc) {
        WrapWidget.fn.init.call(this, id, desc, "GeoBox");
    },
    element: function () {
        return $("#" + this.id);
    },
    resize: function (width, height) {
        //this.widget().clientSize = new GeoBox.Size('100%', '100%');
    },
    destroy: function () {
        if (this.widget()) {
            this.widget().dispose();
        }
    },
    createWidget: function (element,
        centerOrBounds,
        zoom,
        mapLayersUrl,
        geoObjectReadUrlOrCrudUrls,
        geoObjectCountsUrl) {
        element = $(element);

        // Init Widget
        var geobox = new GeoBox.GeoBoxView(centerOrBounds, zoom);
        geobox.layerDataSource = new GeoBox.DataSources.GeoLayerHierarchicalDataSource(mapLayersUrl);
        geobox.geoObjectDataSource = new GeoBox.DataSources.GeoObjectDataSource(geoObjectReadUrlOrCrudUrls, "mnemonic");
        geobox.geoObjectCountDataSource =
            new GeoBox.DataSources.GeoObjectCountDataSource(geoObjectCountsUrl, "mnemonics");

        // Render Widget
        geobox.renderTo(element[0]);

        // Subscribe Widget Events
        geobox.geoObjectCreated.add(this.handleAdd.bind(this));
        geobox.geoObjectEdited.add(this.handleEdited.bind(this));
        geobox.geoObjectDeleted.add(this.handleDeleted.bind(this));
        geobox.geoObjectDeleteButtonClick.add(this.handleDeleteButtonClick.bind(this));
        geobox.geoObjectEditButtonClick.add(this.handleEditButtonClick.bind(this));

        geobox.stateManager.requestError.add(this.handleError.bind(this));


        element.data("GeoBox", geobox);
        return geobox;
    },
    mnemonic: function () {
        throw new GeoBox.Common.NotImplementedException();
    },
    handleAdd: function (widget, event) {
        throw new GeoBox.Common.NotImplementedException();
    },
    handleEdited: function (widget, event) {
        throw new GeoBox.Common.NotImplementedException();
    },
    handleDeleted: function (widget, event) {
        throw new GeoBox.Common.NotImplementedException();
    },
    handleEditButtonClick: function (widget, event) {
        throw new GeoBox.Common.NotImplementedException();
    },
    handleDeleteButtonClick: function (widget, event) {
        throw new GeoBox.Common.NotImplementedException();
    },
    handleError: function (widget, event) {
        if (event.origin.errors !== undefined) {
            pbaAPI.errorMsg(event.origin.errors);
        } else {
            pbaAPI.errorMsg("Error request: " + event.origin.errorThrown);
        }
    }
});


var WrapTreeList = ListView.extend({
    init: function (options) {
        this.lock = false;
        options.type = "kendoTreeList";
        ListView.fn.init.call(this, options);
    },
    root: function () {
        return this.widget().root;
    },
    select: function (item) {
        if (item) {
            item = $(item);

            this.widget().select(item);

            this.onselect();
        }

        return $(this.widget().select());
    },
    onselect: function (source) {
        if (this.composite != null) {

            var $source;

            if (source)
                $source = $(source);
            else
                $source = this.select();

            this.composite.onWidgetChanged(
                {
                    sender: this,
                    event: "select",
                    params: { dataItem: this.widget().dataItem($source), select: $source }
                });
        }
    },
    expand: function (selectNode) {
        this.widget().expand(selectNode);
    },
    append: function (item, node) {
        this.widget().append(item, node);
    },
    remove: function (item) {
        this.widget().remove(item);
    },
    removeRow: function (item) {
        this.widget().removeRow(item);
    },
    refresh: function () {
        this.widget().dataSource.read();
    },
    getSelectID: function () {
        var $selectNode = this.select();

        if ($selectNode.length > 0) {
            var id = $selectNode.find(".tree-node").attr('id');
            return parseInt(id);
        }

        return null;
    },
    initCurrentRow: function () {
        var g = this.widget();
        var $select = g.select();

        if ($select.length > 0) {
            var item = g.dataItem($select);

            this.currentRow = item.ID;

            if (g.content)
                this.scrollTop = g.content.scrollTop();
        } else {
            this.currentRow = null;
            this.scrollTop = 0;
        }
    },
    selectUID: function () {
        var $select = this.select();

        if ($select.length > 0) {
            return this.select().attr("data-uid");
        }

        return null;
    },
    selectID: function () {
        var uid = this.selectUID();

        if (uid) {
            return this.getByUid(uid).ID;
        }

        return null;
    },
    //override
    getSelectItems: function () {
        var id = this.getSelectID();
        var res = [];

        if (id) {


            pbaAPI.proxyclient.crud.get({ mnemonic: this.mnemonic, id: id }, null, { async: false })
                .done(function (result) {
                    if (result.error) {
                        pbaAPI.errorMsg(result.error);
                    } else {
                        res.push(result.model);
                    }
                });
        }

        return res;
    }
});