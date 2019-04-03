/* globals console, $, pbaAPI */
/* jshint strict: false */
window.pbaAPI = window.pbaAPI || {};
(function() {
    'use strict';

    /**
     * Выполняет функцию 'fn' асинхронно.
     * @param  {function} fn Исходная функция.
     */
    pbaAPI.async = function(fn) {
        if (typeof fn !== 'function') {
            throw Error('fn is not a functions');
        }

        setTimeout(fn, 0);
    };

    /**
     * Создает псевдо-случайную строку длиной 8-32 цифр/букв (длина строки зависит от конкретного браузера).
     * @param {string} [prefix] Вернет псевдо-случайную строку с переданным префиксом формата "{prefix}_{RANDOM-STRING}"
     * @returns {string}
     */
    pbaAPI.uid = function(prefix) {
        var uid = Math.random().toString(36).slice(2);

        if (prefix) {
            return prefix + '_' + uid;
        }

        return uid;
    };

    /**
     * Создает псевдо-случайную строку формата GUID.
     * @param {string} [prefix] Вернет GUID с переданным префиксом формата "{prefix}_{GUID}".
     * @returns {string}
     */
    pbaAPI.guid = function(prefix) {
        var s4 = function() {
            return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
        };

        var guid = (s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4());

        if (prefix) {
            return prefix + '_' + guid;
        }

        return guid;
    };

    pbaAPI.ensurePath = function(path) {
        var endpoint = window;

        path.split('.').map(function(segment) {

            return segment.trim();

        }).forEach(function(segment) {

            if (!segment || segment === 'this') {
                console.error('bad segment: name is noop', {path: path, segment: segment});
                throw Error('bad segment', {path: path, segment: segment});
            }

            if (segment === 'window') {
                return;
            }

            if (endpoint[segment] === void 0) {
                endpoint[segment] = {};
            } else if (endpoint[segment] === null || typeof endpoint[segment] !== 'object') {
                console.error('bad segment: is null or not an object', {
                    path: path,
                    segment: segment,
                    endpoint: typeof endpoint[segment]
                });
                throw Error('bad segment');
            }

            endpoint = endpoint[segment];
        });

        return endpoint;
    };

    /**
     * Сокращает строку.
     * @param  {string} text      Исходная строка.
     * @param  {number} maxLength Максимальная длина результирующей строки.
     * @return {string}           Результат сокращения.
     */
    pbaAPI.truncate = function(text, maxLength) {
        if (!text) {
            return '';
        }

        if (!maxLength || text.length <= maxLength) {
            return text;
        }

        text = text.substr(0, maxLength - 3);

        var idx = text.lastIndexOf(' ');

        // Средняя длина русского слова составляет 5-8 символа
        if (maxLength - idx <= 10) {
            text = text.substr(0, idx);
        }

        return text + '...';
    };

    pbaAPI.splitTextIntoLines = function(text, lineCount, lettersInLine) {

        if (lineCount < 2) {
            return pbaAPI.truncate(text, lettersInLine);
        }

        var words = (text || "").trim().split(/\s+/g);

        if (!words[0]) {
            return [];
        }

        var lines = [];

        for (var i = 0, line = "", prependSpace = false; words.length && i < lineCount; i++, line = "", prependSpace = false) {
            do {

                // текущая линия + пробел + следующее слово - все помещается
                if (line.length + words[0].length + (prependSpace ? 1 : 0) <= lettersInLine) {
                    line += (prependSpace ? " " : "") + words.shift();
                    prependSpace = true;
                    continue;

                    // следующее слово меньше заданного предела (поместится в пустой линии)
                } else if (words[0].length <= lettersInLine) {

                    // текущая линия - последняя, поэтому конкатим это слово и заполняем линию
                    if (i === lineCount - 1) {
                        line += (prependSpace ? " " : "") + words[0].substr(0, lettersInLine - line.length - (prependSpace ? 1 : 0));
                        line = line.substr(0, line.length - 3) + "...";
                    }

                    // переходим на следующую линию (или выходим из обрамляющего цикла, если она последняя)
                    break;

                    // следующее слово длиннее даже пустой линии, поэтому дробим это слово, добавляя часть в текущую линию
                } else {
                    var temp = lettersInLine - line.length - (prependSpace ? 1 : 0);

                    line += (prependSpace ? " " : "") + words[0].substr(0, temp);

                    // убираем добавленную в линию часть слова из этого слова
                    words[0] = words[0].substr(temp);
                }

                break;

            } while (words.length);

            lines[i] = line;
        }

        return lines;
    };

    /**
     * Убирает дубликаты из массива (array-like, исходный массив не изменяется).
     * @param  {any[]} objArray Исходный массив.
     * @return {any[]}          Результат (уникальные значения исходного массива).
     */
    pbaAPI.distinct = function(objArray) {
        if (!pbaAPI.isArrayLike(objArray)) {
            return objArray;
        }

        return Array.prototype.reduce.call(objArray, function(resultArray, item) {
            var isUnique = resultArray.every(function(i) {
                return i !== item;
            });

            if (isUnique) {
                resultArray.push(item);
            }

            return resultArray;
        }, []);
    };

    /**
     * Убирает дубликаты из массива (array-like) объектов по значению заданного св-ва (исходный массив не изменяется).
     * @param  {any[]}  objArray    Исходный массив объектов.
     * @param  {string} [propName]  Название свойства, по которому отсеиваются повторяющиеся элементы массива
     *                              (если не указано, вызывается pbaAPI.distinct).
     * @return {[type]}             Результирующий массив.
     */
    pbaAPI.distinctBy = function(objArray, propName) {
        if (!pbaAPI.isArrayLike(objArray)) {
            return objArray;
        }

        if (!propName) {
            return pbaAPI.distinct(objArray);
        }

        return Array.prototype.reduce.call(objArray, function(resultArray, item) {
            var isUnique = resultArray.every(function(i) {
                return i[propName] !== item[propName];
            });

            if (isUnique) {
                resultArray.push(item);
            }

            return resultArray;
        }, []);
    };

    /**
     * Аналог Array#forEach. При возврате значения false функцией 'fn' - выходит из цикла.
     * Умеет работать с array-like объектами.
     * @param  {object[]}   arrayLike (Псевдо)массив, по которому нужно бежать.
     * @param  {function}   fn        Функция, которой передаются {элемент итерации}, {индекс итерации}, {исходный (псевдо)массив}
     */
    pbaAPI.each = function(arrayLike, fn) {
        if (!pbaAPI.isArrayLike(arrayLike) || !pbaAPI.isFunction(fn)) {
            console.error('wrong arguments', {arrayLike: arrayLike, fn: fn});
            return;
        }

        for (var i = 0; i < arrayLike.length; i++) {
            if (fn(arrayLike[i], i, arrayLike) === false) {
                break;
            }
        }
    };

    /**
     * Безопасный (не кидает исключений при неверных параметрах) аналог Array#filter.
     * Умеет работать с array-like объектами.
     * Не изменяет исходный объект/массив.
     * @param  {object[]}   arrayLike
     * @param  {function}   fn
     * @return {Array}
     */
    pbaAPI.filter = function(arrayLike, fn) {
        if (!pbaAPI.isArrayLike(arrayLike) || !pbaAPI.isFunction(fn)) {
            console.error('wrong arguments', {arrayLike: arrayLike, fn: fn});
            return [];
        }

        return Array.prototype.filter.call(arrayLike, fn);
    };

    /**
     * Безопасный (не кидает исключений при неверных параметрах) аналог Array#map.
     * Умеет работать с array-like объектами.
     * Не изменяет исходный объект/массив.
     * @param  {object[]}   arrayLike
     * @param  {function}   fn
     * @return {Array}      Новый массив.
     */
    pbaAPI.map = function(arrayLike, fn) {
        if (!pbaAPI.isArrayLike(arrayLike) || !pbaAPI.isFunction(fn)) {
            console.error('wrong arguments', {arrayLike: arrayLike, fn: fn});
            return [];
        }

        return Array.prototype.map.call(arrayLike, fn);
    };

    /**
     * Безопасный (не кидает исключений при неверных параметрах) аналог Array#reduce.
     * Умеет работать с array-like объектами.
     * Не изменяет исходный объект/массив.
     * @param {object[]}    arrayLike
     * @param {function}    fn
     * @param {any}         [initialValue]
     * @returns {any}     Результат.
     */
    pbaAPI.reduce = function(arrayLike, fn, initialValue) {
        if (!pbaAPI.isArrayLike(arrayLike) || !pbaAPI.isFunction(fn)) {
            console.error('wrong arguments', { arrayLike: arrayLike, fn: fn });
            return [];
        }

        return Array.prototype.reduce.call(arrayLike, fn, initialValue);
    };

    /**
     * Мапит массив значений выбранного св-ва каждого объекта исходного массива.
     * @param  {object[]}   objArrayLike            Исходный (псевдо)массив.
     * @param  {string}     propName                Название свойства.
     * @param  {boolean}    [skipWrong = false]     Пропускать элементы не имеющие свойства с заданным названием (необяз.).
     * @return {any[]}
     * @example
     *
     *      var array = [
     *          { a: 5,         b: 'some' },
     *          { a: 6,         b: 'some2' },
     *          { a: 'abc',     b: 'some3' },
     *          { a: undefined, b: 'some4' },
     *          true,
     *          {},
     *      ];
     *      var extracted           = pbaAPI.extract(array, 'a');      // [5, 6, 'abc', undefined, undefined, undefined]
     *      var extractedWithSkip   = pbaAPI.extract(array, 'a', true);// [5, 6, 'abc', undefined]
     */
    pbaAPI.extract = function(arrayLike, propName, skipWrong) {
        if (!pbaAPI.isArrayLike(arrayLike)) {
            return [];
        }

        var filter = Array.prototype.filter;
        var map = Array.prototype.map;

        skipWrong = skipWrong === true;

        if (!skipWrong) {
            return map.call(arrayLike, function(obj) {
                return obj && obj[propName];
            });
        }

        return filter.call(arrayLike, function(obj) {
            return obj && obj.hasOwnProperty(propName);
        }).map(function(obj) {
            return obj[propName];
        });
    };

    pbaAPI.json = {

        /**
         * "Безопасный" парсинг JSON.
         * @param  {string}         str Исходная строка для парсинга.
         * @return {object|null}        Результирующий объект или null в случае ошибки.
         */
        parse: function (str) {
            var obj = null;

            try {
                obj = JSON.parse(str);
            } catch (e) { /*EMPTY*/ }

            return obj;
        },

        /**
         * "Безопасная" сериализация объекта в JSON-строку.
         * @param  {object}         obj Исходный объект для сериализации.
         * @return {string|null}        Результирующая JSON-строка или null в случае ошибки.
         */
        stringify: function(obj) {
            var str = null;

            try {
                str = JSON.stringify(obj);
            } catch (e) { /*EMPTY*/ }

            return str;
        }
    };

    pbaAPI.emitterMixin = function() {
        var self = {
            _emitter: {
                _listeners: {},
                on: function(evt, handler) {
                    var listeners = self._emitter._listeners;

                    if (!listeners[evt]) {
                        listeners[evt] = [];
                    }

                    listeners[evt].push(handler);
                },
                once: function(evt, handler) {
                    var emitter = self._emitter;

                    emitter.on(evt, function callOnce(data) {
                        emitter.off(evt, callOnce);
                        handler(data);
                    });
                },
                off: function(evt, handler) {
                    var listeners = self._emitter._listeners;

                    if (listeners[evt]) {
                        listeners[evt] = listeners[evt].filter(function(listener) {
                            return listener !== handler;
                        });
                    }
                },
                emit: function(evt, data) {
                    var listeners = self._emitter._listeners;

                    if (listeners[evt]) {
                        listeners[evt].forEach(function(listener) {
                            listener(data);
                        });
                    }
                }
            },
            on: function() {
                self._emitter.on.apply(self._emitter, arguments);
            },
            once: function() {
                self._emitter.once.apply(self._emitter, arguments);
            },
            off: function() {
                self._emitter.off.apply(self._emitter, arguments);
            },
            emit: function() {
                self._emitter.emit.apply(self._emitter, arguments);
            }
        };

        return self;
    };

    /**
     * @param {string} html
     * @returns {string}
     */
    pbaAPI.htmlEncode = function(html) {
        if (!html) return html;

        return ('' + html)
            .replace(/\&/g, '&amp;')
            .replace(/\</g, '&lt;')
            .replace(/\>/g, '&gt;')
            .replace(/\"/g, '&quot;')
            .replace(/\'/g, '&apos;');
    };

}());
