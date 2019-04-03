/* globals $, pbaAPI */
(function() {
    'use strict';

    /**
     * Promise-обертка над $.ajax, не принимает url первым строковым параметром, только объект 'opts'.
     * @param {Object}  opts            http://api.jquery.com/jquery.ajax/
     * @returns {Promise}
     */
    pbaAPI.ajax = function(opts) {
        return new Promise(function(resolve, reject) {
            $.ajax(opts).done(resolve).fail(reject);
        });
    };

    /**
     * Promise-обертка над $.get.
     * @param {string}  url             Ссылка для GET - запроса.
     * @param {Object}  (data)          Данные для GET - запроса.
     * @returns {Promise}
     */
    pbaAPI.ajax.get = function(url, data) {
        data = data || {};

        if (data.toJSON) {
            data = data.toJSON();
        }

        return new Promise(function(resolve, reject) {
            $.get(url, data).done(resolve).fail(reject);
        });
    };

    /**
     * Promise-обертка над $.post.
     * @param {string}  url             Ссылка для POST - запроса.
     * @param {Object}  data            Данные для POST - запроса.
     * @returns {Promise}
     */
    pbaAPI.ajax.post = function(url, data) {
        return new Promise(function(resolve, reject) {
            if (data && data.toJSON) {
                data = data.toJSON();
            }

            $.post(url, data).done(resolve).fail(reject);
        });
    };

}());
