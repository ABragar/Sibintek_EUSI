/* globals $, pbaAPI */
/* jshint strict: false */
(function() {
	'use strict';

    /**
     * Оборачивает функцию 'method', предотвращая ее вызовы с маленькими промеутками.
     * @param   {function}  method                  Исходная функция.
     * @param   {number}    [options.time=250]      Ожидание перед выполнением, в случае вызовов с меньшим промежутком.
     * @param   {number}    [options.timeout=2000]  Максимальное время ожидания в сумме, после которого 'method' будет выполнен.
     * @param   {any}       [options.context=null]  Значение для this внутри 'method'.
     * @returns {function}                          Обертка над функцией 'method'
     * @example
     *
     *      var func = function(str) { alert(str) };
     *      var debounced = pbaAPI.debounce(func);
     *
     *      debounced('a');
     *      debounced('b');
     *      debounced('c');
     *
     *      функция 'func' будет вызвана один раз (т.к. промежуток между вызовами меньше 250мс),
     *      с последним набором параметров; в данном случае, спустя ~250мс выведется "c".
     */
    pbaAPI.debounce = function(method, options) {
        if (typeof method !== 'function') {
            throw new Error('first argument must be a function!');
        }

        var opts = $.extend({
            time: 250,
            timeout: 2000,
            context: null,
        }, options || {});

        var timeoutId = null;
        var start = null;

        return function() {
            var args = arguments;
            var now = Date.now();
            var work = function() {
                method.apply(opts.context, args);
                start = null;
            };

            start = start || now;

            clearTimeout(timeoutId);

            if (start && now - start > opts.timeout) {
                work();
                return;
            }

            timeoutId = setTimeout(work, opts.time);
        };
    };

}());
