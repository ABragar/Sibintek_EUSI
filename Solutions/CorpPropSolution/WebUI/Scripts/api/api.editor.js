(function () {
    'use strict';

    var editors = {};

    /**
     * Базовый класс для всех едиторов.
     * @example
     *      var SomeEditor = pbaAPI.Editor.extend({
     *          init: function($wrap, propertyName) {
     *              pbaAPI.Editor.fn.init.call(this, $wrap, propertyName);
     *          }
     *      });
     */
    pbaAPI.Editor = kendo.Class.extend({
        form: $(),
        wrap: $(),

        pbaForm: null,

        init: function ($wrap, propertyName) {
            this.propertyName = propertyName;
            this.form = $wrap.closest('form');
            this.wrap = $wrap;
            this.wrapId = $wrap.attr('id');

            this.form.on('onResize', this.onResize.bind(this));
            this.form.on('onSave', this.onSave.bind(this));
            this.form.on('onScroll', this.onScroll.bind(this));

            this._onAfterBind();
            this._onBeforeBind();
            this._onShown();
        },

        onAfterBind: function () { },
        onBeforeBind: function () { },
        onResize: function () { },
        onSave: function () { },
        onScroll: function () { },
        onShown: function () { },
        readProperty: function () {
            return this.pbaForm.getPr(this.propertyName);
        },
        writeProperty: function (val) {
            this.pbaForm.setPr(this.propertyName, val);
        },

        /**
         * @private
         */
        _onAfterBind: function () {
            var editor = this;
            var pbaForm = this.form.data('pbaForm');

            if (pbaForm) {
                pbaAPI.async(function () {
                    editor.pbaForm = pbaForm;
                    editor.onAfterBind();
                });

                return;
            }

            this.form.on('onAfterBind',
                function (evt, form) {
                    editor.pbaForm = form;
                    editor.onAfterBind();
                });
        },
        _onBeforeBind: function () {
            var editor = this;
            var pbaForm = this.form.data('pbaForm');

            if (pbaForm) {
                pbaAPI.async(function () {
                    editor.pbaForm = pbaForm;
                    editor.onBeforeBind();
                });

                return;
            }

            this.form.on('onBeforeBind',
                function (evt, form) {
                    editor.pbaForm = form;
                    editor.onBeforeBind();
                });
        },
        _onShown: function () {
            var editor = this;

            editor.form.on('onTabShown',
                function (e, evt) {

                    if (editor.wrap.closest('[data-tab-content="' + evt.tabID + '"]').length > 0) {
                        editor.onShown();
                    }
                });
        }
    });

    /**
     * Эту функцию следует вызывать в каждом едитор-js-нике
     * @param {string} editorName
     * @param {pbaAPI.Editor} EditorConstructor 
     */
    pbaAPI.registerDisplay = function (editorName, EditorConstructor) {
        var name = "display." + editorName;

        if (editors[name]) {
            pbaAPI.errorMsg('Редактор ' + editorName + ' уже зарегестрирован!');
            return;
        }

        editors[name] = EditorConstructor;
    }

    /**
    * Эту функцию следует вызывать в каждом едитор-js-нике
    * @param {string} editorName
    * @param {pbaAPI.Editor} EditorConstructor 
    */
    pbaAPI.registerEditor = function (editorName, EditorConstructor) {
        var name = "editor." + editorName;

        if (editors[name]) {
            pbaAPI.errorMsg('Редактор ' + editorName + ' уже зарегестрирован!');
            return;
        }

        editors[name] = EditorConstructor;
    };

    /**
     * Эту функцию следует вызывать на каждой едитор-вьюхе (выполняется асинхронно!).
     * @param {string} editorName
     * @param {string} wrapId 
     * @param {string} propertyName 
     */
    pbaAPI.wrapDisplay = function (editorName, wrapId, propertyName) {
        var name = "display." + editorName;

        var EditorConstructor = editors[name];

        if (!EditorConstructor) {
            //pbaAPI.errorMsg('Не найден редактор ' + editorName);
            return;
        }

        pbaAPI.async(function () {
            var $wrap = $('#' + wrapId);

            if (application.isDebug && $wrap.length === 0) {
                pbaAPI.errorMsg('Неправильный wrapId');
                return;
            }

            var editor = new EditorConstructor($wrap, propertyName);
        });
    };

    /**
     * Эту функцию следует вызывать на каждой едитор-вьюхе (выполняется асинхронно!).
     * @param {string} editorName
     * @param {string} wrapId 
     * @param {string} propertyName 
     */
    pbaAPI.wrapEditor = function (editorName, wrapId, propertyName) {
        var name = "editor." + editorName;

        var EditorConstructor = editors[name];

        if (!EditorConstructor) {
            //pbaAPI.errorMsg('Не найден редактор ' + editorName);
            return;
        }

        var $wrap = $('#' + wrapId);

        if (application.isDebug && $wrap.length === 0) {
            pbaAPI.errorMsg('Неправильный wrapId');
            return;
        }

        var editor = new EditorConstructor($wrap, propertyName);
    };
}());