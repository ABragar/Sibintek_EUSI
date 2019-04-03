window.pbaLinks = window.pbaLinks || {};

(function () {
    $.extend(pbaLinks,
    {
        _diagramContainer: null,
        _network: null,
        _networkOptions: null,
        _contextMenu: null,
        _linksIcons: [],
        _cntxMenuId: "links_ctx_id",
        wid: "links_window_id",
        _params: {},
        openLinksWindow: function (params) {
            this._params = $.extend({ id: 0, mnemonic: null }, params);
            if (this._params.id === 0 || this._params.mnemonic === null) {
                return;
            }

            var $wnd = $("#" + this.wid);

            if (!$wnd.length) {
                $("body").append('<div id="' + this.wid + '">' +
                    '<div class="links-content col-md-10"></div>' +
                    '<div class="links-legend col-md-2"></div>' +
                    '</div>');

                $('<script id="template_links_window_item" type="text/x-kendo-template">' +
                    '<div>' +
                        '<span class="#:value#" style="font-size:#:size#px;color:#:color#"></span>' +
                        '<span>#:label#</span>' +
                    '</div></script>').appendTo(document.body);

                $wnd = $("#" + this.wid);
            }

            var wnd = $wnd.data('kendoWindow');

            if (!wnd) {
                wnd = $wnd.kendoWindow({
                    width: 900,
                    height: 600,
                    title: "Карта",
                    actions: ["Maximize", "Close"],
                    modal: true
                }).data("kendoWindow");
            }

            wnd.center().open();

            this._data = {
                nodes: new vis.DataSet([]),
                edges: new vis.DataSet([])
            };

            this._diagramContainer = $('#' + this.wid + ' .links-content')[0];
            this._requestData();
            this._initContextMenu();
        },
        _requestData: function () {
            var that = this;
            this._data.nodes.clear();
            this._data.edges.clear();

            var requestData = { mnemonic: this._params.mnemonic, objectID: this._params.id };
            $.when($.ajax({
                type: "GET",
                url: "/Links/GetMapItems",
                data: requestData,
                contentType: "application/json; charset=utf-8",
                success: function (result) {

                    var legendGroups = [];

                    for (var i = 0; i < result.length; i++) {
                        var node = {
                            id: result[i].ID,
                            label: result[i].Title,
                            group: result[i].Mnemonic,
                            Mnemonic: result[i].Mnemonic,
                            RealID: result[i].RealID
                        };

                        that._data.nodes.add(node);

                        if (legendGroups.indexOf(result[i].Mnemonic) === -1) {
                            legendGroups.push(result[i].Mnemonic);
                        }

                        if (legendGroups.indexOf(result[i].Mnemonic) === -1) {
                            legendGroups.push(result[i].Mnemonic);
                        }
                    }
                }
            }),
				$.ajax({
				    type: "GET",
				    url: "/Links/GetConnections",
				    data: requestData,
				    contentType: "application/json; charset=utf-8",
				    success: function (result) {

				        for (var i = 0; i < result.length; i++) {
				            var edge = {
				                from: result[i].FromObject,
				                to: result[i].ToObject,
				                id: result[i].ID
				            };
				            that._data.edges.add(edge);
				        }
				    }
				}),
                //TODO: пчм все (без параметров)
				$.ajax({
				    type: "GET",
				    url: "/Links/GetLinkGroups",
				    success: function (result) {

				        var g = { groups: {} };

				        for (var i = 0; i < result.length; i++) {
				            var res = result[i];

				            application.viewModelConfigs.get(res.name).done(
				                function (vmConfig) {
				                    if (!that.existElement(vmConfig.TypeEntity)) {
				                        that._linksIcons.push({ value: res.icon.value, size: res.icon.size, label: vmConfig.Title, color: res.icon.color, mnemonic: vmConfig.TypeEntity });
				                    }
				                });

				            g.groups[result[i].name] = {
				                shape: "icon",
				                size: result[i].size,
				                icon: result[i].icon,
				                font: result[i].font
				            };

				        }
				        that._networkOptions = $.extend(that._networkOptions, g);
				    }
				})
			).done(function () {
			    that._initNetwork();

			    var template = kendo.template($("#template_links_window_item").html());

			    var linksDataSorce = new kendo.data.DataSource({
			        data: that._linksIcons,
			        change: function () {
			            $(".links-legend").html(kendo.render(template, this.view()));
			        }
			    });

			    linksDataSorce.read();
			});
        },

        existElement: function (mnemonic) {
            for (var i = 0; i < this._linksIcons.length; i++) {
                if (this._linksIcons[i].mnemonic === mnemonic) {
                    return true;
                }
            }
            return false;
        },
        _drawLegend: function (mnemonic, index) {
            var x = -this._diagramContainer.clientWidth / 2;
            var y = -this._diagramContainer.clientHeight / 2;
            var step = 70;

            application.viewModelConfigs.get(mnemonic).done(function (vmConfig) {
                this._data.nodes.add({
                    id: 1000 + index,
                    x: x,
                    y: y + index * step,
                    label: vmConfig.Title,
                    group: mnemonic,
                    value: 1,
                    fixed: true,
                    physics: false
                });
            });
        },

        _removeLink: function () {

            var that = this;

            var selectedEdges = this._network.getSelectedEdges();

            if (selectedEdges.length === 0) {
                return;
            }

            var currentEdge = selectedEdges[0];

            $.ajax({
                type: "POST",
                url: "/Links/RemoveLink",
                data: JSON.stringify({ id: currentEdge }),
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    that._requestData();
                }
            });
        },

        _addLinkItem: function () {
            var typeName = null;
            var that = this;

            var selectedNodes = this._network.getSelectedNodes();

            if (selectedNodes.length === 0) {
                return;
            }

            var currentNode = this._data.nodes.get(selectedNodes[0]);

            var selectCallback = function (model) {
                if (model) {
                    that._saveLink(currentNode, model[0], typeName);
                }
            }

            var params = {
                callback: function (e) {
                    typeName = e.TypeName;
                    pbaAPI.openModalDialog(typeName, selectCallback);
                }
            };

            $.ajax({
                type: "GET",
                url: "/Links/GetLinkedTypes",
                data: { typeName: currentNode.Mnemonic },
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    pbaAPI.selectSimple(result, params);
                }
            });

        },

        _saveLink: function (sourceNode, destItem, destType) {
            var that = this;

            var postData = {
                sourceMnemonic: sourceNode.Mnemonic,
                sourceID: sourceNode.RealID,
                destMnemonic: destType,
                destID: destItem.ID
            }

            $.ajax({
                type: "POST",
                url: "/Links/SaveLink",
                data: JSON.stringify(postData),
                contentType: "application/json; charset=utf-8",
                success: function (result) {

                    that._requestData();
                }
            });
        },

        _onContextMenuSelect: function (e) {

            var action = $(e.item).attr('data-action');

            if (action === "add") {
                this._addLinkItem();
            } else if (action === "remove") {
                this._removeLink();
            }
        },

        _onNetworkContextMenu: function (params) {

            var point = { x: params.event.offsetX, y: params.event.offsetY };

            var node = this._network.getNodeAt(point);
            var edge = this._network.getEdgeAt(point);

            if (node) {
                params.event.stopPropagation();
                params.event.preventDefault();

                this._network.selectNodes([node]);
            } else if (edge) {

                params.event.stopPropagation();
                params.event.preventDefault();

                this._network.selectEdges([edge]);
            } else {
                params.preventDefault();
            }
        },

        _initNetwork: function () {
            this._networkOptions = $.extend(this._networkOptions,
				{
				    autoResize: true,
				    height: "100%",
				    width: '100%',
				    locale: 'ru',
				    clickToUse: true,
				    layout: {
				        randomSeed: 64
				    },
				    physics: {
				        maxVelocity: 50,
				        minVelocity: 0.1,
				        barnesHut: {
				            gravitationalConstant: 0,
				            centralGravity: 0,
				            springConstant: 0
				        },
				        stabilization: {
				            enabled: true,
				            iterations: 180, // maximum number of iteration to stabilize
				            updateInterval: 10,
				            onlyDynamicEdges: false,
				            fit: true
				        }
				    }
				});

            var that = this;
            this._network = new vis.Network(this._diagramContainer, this._data, this._networkOptions);


            this._network.on("stabilized ", function () {
                that._network.setOptions({
                    physics: true
                });
            });

            this._network.on("doubleClick",
				function (params) {
				    if (params.nodes[0]) {
				        var curnode = that._data.nodes.get(params.nodes[0]);
				        pbaAPI.openDetailView(curnode.Mnemonic,
						{
						    id: curnode.RealID,
						    toSave: true,
						    callback: function (e) {
						    }
						});
				    }
				});

            $(".vis-network").addClass("vis-active");
            $(".vis-overlay").css("display", "none");
        },

        _initContextMenu: function () {
            var that = this;

            $('#' + this.wid + ' .links-content').append('<ul id="' + this._cntxMenuId + '">' +
				' <li data-action="add">Добавить связь</li>' +
				' <li data-action="remove">Удалить связь</li>' +
				'</ul>');


            this._contextMenu = $('#' + this._cntxMenuId).kendoContextMenu({
                target: $('#' + this.wid + ' .links-content'),
                select: function (e) { that._onContextMenuSelect(e); },
                open: function (e) { that._onNetworkContextMenu(e); }
            }).data('kendoContextMenu');
        },

    });

}());