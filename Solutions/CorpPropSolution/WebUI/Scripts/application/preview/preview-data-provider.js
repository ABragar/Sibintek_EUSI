/* globals kendo, pbaAPI, application */
/* jshint strict: false */
pbaAPI.ensurePath('application.preview');

(function() {
    'use strict';

    var TemplateCache = new Map();
    var ViewModelCache = new Map();

    // ###########
    // CONSTRUCTOR
    // ###########

    var PreviewDataProvider = application.preview.PreviewDataProvider = function(mnemonic, id) {
        this._mnemonic = mnemonic;
        this._id = id;
    };

    // #######
    // STATICS
    // #######

    PreviewDataProvider.CACHE_MAX_AGE = 10000;

    // ################
    // INSTANCE METHODS
    // ################

    PreviewDataProvider.prototype = {
        loadTemplate: function() {
            var mnemonic = this._mnemonic;

            return new Promise(function(resolve, reject) {

                // GET FROM CACHE IF POSSIBLE
                if (TemplateCache.has(mnemonic)) {
                    return resolve( TemplateCache.get(mnemonic) );
                }

                var url = application.url.GetView('GetPreviewTemplate');
                var data = { mnemonic: mnemonic, _: Math.random() };

                pbaAPI.ajax.get(url, data).then(function(res) {
                    if (!res || res.error) {
                        return reject(
                            Error(res && res.error || 'Ошибка сервера при получении шаблона превью'));
                    }

                    var template = kendo.template(res);

                    // UPDATE CACHE
                    TemplateCache.set(mnemonic, template);

                    resolve(template);

                }).catch(function(err) {
                    reject( Error(err.message) );
                });
            });
        },
        loadViewModel: function() {
            var mnemonic = this._mnemonic;
            var id = this._id;

            return new Promise(function(resolve, reject) {
                var key = mnemonic + ':' + id;

                // GET FROM CACHE IF POSSIBLE
                if (ViewModelCache.has(key)) {
                    var cacheItem = ViewModelCache.get(key);

                    if (Date.now() - cacheItem.stamp <= PreviewDataProvider.CACHE_MAX_AGE) {
                        return resolve(cacheItem.model);
                    }
                }

                pbaAPI.proxyclient.crud.getPreview({
                    mnemonic: mnemonic,
                    id: id
                }).done(function (res) {
                    if (!res || res.error) {
                        return reject(
                            Error(res && res.error || 'Ошибка сервера при получении превью'));
                    }

                    // UPDATE CACHE
                    ViewModelCache.set(key, { model: res, stamp: Date.now() });

                    resolve(res);

                }).fail(function (err) {
                    reject(Error(err.message));
                });
            });
        }
    };
}());
