/* global $, pbaAPI, application */
/* jshint strict: false */
pbaAPI.ensurePath('application');
(function () {
    'use strict';

    // #########
    // EXTENSION
    // #########

    $.extend(application,
    {
        // INITIALIZATION IN ~/Views/Shared/Layout.chtml
        currentUser: null,
        isDebug: false,
        getDefaultModalWidth: function (type) {
            if (type === "listview")
                return $(window).width() - 100;
            else
                return Math.min(1300, $(window).width() - 100);
        },
        getDefaultModalHeight: function (type) {
            if (type === "listview")
                return $(window).height() - 100;
            else
                return Math.min(831, $(window).height() - 100);
        },
        url: {
            Icon: function (action, params) {
                return pbaAPI.addUrlParametrs('/Icon/' + action, params);
            },
            GetView: function (action, params) {
                return pbaAPI.addUrlParametrs('/View/' + action, params);
            },
            GetHCategory: function (action, params) {
                return pbaAPI.addUrlParametrs('/HCategory/' + action, params);
            },
            GetFiles: function (action, params) {
                return pbaAPI.addUrlParametrs('/Files/' + action, params);
            },
            GetFileData: function (action, params) {
                return pbaAPI.addUrlParametrs('/FileData/' + action, params);
            },
            GetWizard: function (action, params) {
                return pbaAPI.addUrlParametrs('/Wizard/' + action, params);
            }
        },
        getContent: function () {
            var $content = $('#content:first');

            if (!$content.length)
                $content = $(window);

            return $content;
        },
        getContentPosition: function () {
            return this.getContent().position();
        },
        viewModelConfigs: {
            _configs: [],
            _mnemonicIdx: {},
            _typeEntityIdx: {},
            _load: {},
            add: function (config) {
                if (config.Mnemonic in this._mnemonicIdx)
                    return;

                var i = this._configs.length;

                this._configs.push(config);

                this._mnemonicIdx[config.Mnemonic] = i;

                if (!(config.TypeEntity in this._typeEntityIdx))
                    this._typeEntityIdx[config.TypeEntity] = i;
            },
            get: function(key) {
                return this._get(key);
            },
            _get: function (key, d) {
                var self = this;

                var deff = d || $.Deferred();

                if (key in self._typeEntityIdx) {
                    deff.resolve(self._configs[self._typeEntityIdx[key]]);
                    return deff;
                }

                if (key in self._mnemonicIdx) {
                    deff.resolve(self._configs[self._mnemonicIdx[key]]);
                    return deff;
                }

                if (key in self._load) {
                    setTimeout(function () {
                        self._get(key, deff);
                    }, 50);

                    return deff;
                }

                self._load[key] = null;

                pbaAPI.proxyclient.viewConfig.getConfig({
                    mnemonic: key
                }).done(function(result) {
                    result = result || { TypeEntity: key, Mnemonic: key };

                    if (key.indexOf('.') > 0)
                        result.TypeEntity = key;

                    for (var pr in result) {
                        if (result.hasOwnProperty(pr)) {
                            self.add(result);
                        }
                    }

                    deff.resolve(result);
                    delete self._load[key];
                });

                return deff;
            },
          
        },
        UiEnums: {
            _enums: {},
            _load: {},
            get: function (type, callback) {
                var self = this;

                var enums = self._enums;

                if (type in enums) {
                    callback(enums[type]);
                    return;
                }

                if (type in self._load) {
                    setTimeout(function () {
                        self.get(type, callback);
                    }, 50);

                    return;
                }

                self._load[type] = null;

                pbaAPI.proxyclient.standard.get_uiEnum({
                    type: type
                }).done(function(result) {
                    if (result.error) {
                        if (application.isDebug)
                            pbaAPI.errorMsg(result.error);

                        result = {};
                    }

                    enums[type] = result;
                    callback(result);

                    delete self._load[type];
                });
            },
            getValue: function (type, val) {
                var enums = this._enums;

                if (type in enums)
                    return enums[type].Values[val];

                this.get(type, function (res) {
                    return res.Values[val];
                });
            },
            getValueCb: function (type, val, callback) {
                var enums = this._enums;

                if (type in enums) {
                    callback(enums[type].Values[val]);
                    return;
                }

                this.get(type, function (res) {
                    callback(res.Values[val]);
                });
            },
            getJqObject: function (type, val) {
                var $span = $('<span>');
                var $i = $('<i>');
                var $result = $();
                $result = $result.add($i);
                $result = $result.add($span);

                this.get(type,
                    function (res) {
                        if (val) {
                            var $enum = res.Values[val];
                            $span.text($enum.Title);
                            $i.addClass($enum.Icon).css('color', $enum.Color);
                        }                      
                        
                    });

                return $result;
            }
        },
        init: function () {
            application.spa.init();

            layout.sidebar.init();
            layout.toolbar.init();

            application.spa.on('route:changed', application.updateBackground.bind(application));
        },

        // TODO: вынести функционал с background'ом в ~/Scripts/layout/index.js (window.layout)
        _backgroundStyle: {},
        _backgroundSelector: '#layout .layout__content',
        setBackground: function (style) {
            style = $.extend({
                backgroundRepeat: 'no-repeat',
                backgroundPosition: 'center',
                backgroundSize: 'contain',
                backgroundImage: 'none'
            }, style);

            $(application._backgroundSelector).css(style);

            application.updateBackground();
        },
        enableBackground: function () {
            $(application._backgroundSelector).removeClass('layout__container--no-background');
        },
        disableBackground: function () {
            $(application._backgroundSelector).addClass('layout__container--no-background');
        },
        updateBackground: function () {
            application.enableBackground();
            //TODO: отображение фона главной страницы
            //if (application.spa.currentRouteHasBackground()) {
            //    application.enableBackground();
            //} else {
            //    application.disableBackground();
            //}
        }
    });

}());
