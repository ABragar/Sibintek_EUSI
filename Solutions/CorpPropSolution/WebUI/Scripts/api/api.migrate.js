/* globals console, $, kendo, pbaAPI */
(function() {
	'use strict';

	// #####################
	// DEPRECATED PROPERTIES
	// #####################

	deprecatedProperty(pbaAPI, 'objectstatus', {
		nochanges: 0,
		created: 1,
		modified: 2,
		deleted: 3,
	});

	// ##################
	// DEPRECATED METHODS
	// ##################

	$.extend(pbaAPI, {

		// не используется
		console: deprecate('console', function() {
			var t = arguments;
			try { console.log(kendo.format.apply(this, t)); }
			catch (e) { console.log(kendo.format('Some error was detected: {0}', e)); }
		}),

		// переименован в guid
		guidGenerator: deprecate('guidGenerator', function() {
	        var S4 = function () {
	            return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
	        };
	        return (S4() + S4() + '-' + S4() + '-' + S4() + '-' + S4() + '-' + S4() + S4() + S4());
	    }, 'guid (util.js)'),

		// не используется
		toClientTemplate: deprecate('toClientTemplate', function(html) {
			return html.replace('"#', '"\\#').replace('\'#', '\'\\#');
		}),

		// функционал реализован более общим способом (см. метод pbaAPI.extract)
		getIDs: deprecate('getIDs', function(arr) {
	        var _arr = arr || [];

	        var ids = new Array(_arr.length);

	        for (var i = _arr.length; i < _arr.length; i++) {
	            if (_arr[i]) {
	                ids[i] = _arr[i].ID;
	            }
	        }

	        return ids;
	    }, 'extract (util.js)'),

		// переименован в truncate
	    truncateStr: deprecate('truncateStr', function(str, max_len) {
	        if (str) {
	            if (str.length > max_len) {
	                str = str.substr(0, max_len - 3);

	                var idx = str.lastIndexOf(' ');

	                if (max_len - idx <= 10) {
	                    str = str.substr(0, idx);
	                }

	                str += '...';
	            }
	        } else {
	            str = '';
	        }

	        return str;
	    }, 'truncate (util.js)'),

	    // нигде не используется и его нужно дорабатывать
	    downloadImage: deprecate('downloadImage', function(id, callback) {
            if (!id) return;

            var loc = document.location;
            var a = document.createElement('a');
            var img = new Image();

            a.href = loc.protocol + '//' + loc.host + pbaAPI.getHrefFile(id);
            a.target = '_self';

            img.onload = function () {
                a.click();
                if (callback) {
                	callback();
                }
            };

            img.src = a.href;
	    }),

        // не используется
	    getValueByLang: deprecate('getValueByLang', function(xml, lang) {
	        return $(xml).find(lang).text();
	    }),

        // не используется
	    replaceFormPlaceholders: deprecate('replaceFormPlacholders', function(form, params) {
	        var initializedParams = {};
	        $.each(params, function(i) {
	            initializedParams[i] = params[i].replace(/\[(.*?)\]/g, function(g1, g2) {
	                return form.getPr(g2);
	            });
	        });

	        return initializedParams;
	    }),

	    // более не актуален
	    getUserStr: deprecate('getUserStr', function(user, onlyImage, w, h, showOnlineState, statusSize) {
            if (!user) {
                return '';
            }

            w = w || 32;
            h = h || 32;

            var fileID = user.Image ? user.Image.FileID : null;

            var imageUrl = pbaAPI.imageHelpers.getsrc(fileID, w, h, 'NoPhoto');

            var html = '<img data-user-image="' + user.ID + '" class="img-circle" src="' + imageUrl + '">';

            if (!onlyImage) {
                html += '<span>&nbsp;&nbsp;' + pbaAPI.htmlEncode(user.FullName || user.Title) + '</span>';
            }

            if (showOnlineState !== false) {
                html = '<div class="user-image">' + html + pbaAPI.getUserState(user.ID, { size: statusSize || 'small' }) + '</div>';
            } else {
                html = '<div class="user-image">' + html + '</div>';
            }

            return html;
        }),

		// более не актуален
        getContactStr: deprecate('getContactStr', function (contact, onlyImage, w, h) {
            if (!contact) {
                return "";
            }

            w = w || 32;
            h = h || 32;

            var fileID = contact.Image ? contact.Image.FileID : null;

            var imageUrl = pbaAPI.imageHelpers.getsrc(fileID, w, h, "NoPhoto");

            var html = "<img data-mnemonic=" + (contact.BoType || { Mnemonic: "BaseContact" }).Mnemonic + " data-contact-image=\"" + contact.ID + "\" class=\"img-circle\" src=\"" + imageUrl + "\">";

            if (!onlyImage) {
                html += "&nbsp;&nbsp;" + pbaAPI.htmlEncode(contact.Title);
            }

            return "<div class=\"contact-image\">" + html + "</div>";
        }),

	    // не актуален
        extend: deprecate('extend', function ( /*arguments*/) {
            return $.extend.apply($, arguments);
        }, '$.extend'),
	});

	// ###############
	// PRIVATE HELPERS
	// ###############

	/**
	 * Обрамляет неактуальный метод, выводя при его вызове соответствующее сообщение в консоль.
	 *
	 * @param  {string}		oldAPI			Старое название метода в pbaAPI.
	 * @param  {function}	func 			Старая реализация метода.
	 * @param  {string}		[relevantAPI]	Новое название актуального метода в pbaAPI (необязательно).
	 * @return {function}					Функция-обертка над старым методом (полностью функциональна).
	 */
	function deprecate (oldAPI, func, relevantAPI) {
		var msg = 'pbaAPI.' + oldAPI + ' is deprecated.';

		if (pbaAPI.hasOwnProperty(oldAPI)) {
			func = pbaAPI[oldAPI];
			warning(msg + ' You should remove it from the main pbaAPI module.');
		}

		if (relevantAPI) {
			msg += ' Use ' + relevantAPI + ' instead.';
		}

		return function deprecated() {
			warning(msg);
			return func.apply(pbaAPI, arguments);
		};
	}

	/**
	 * Создает в объекте геттер на заданное неактуальное св-во (без сеттера!), выводя при его вызове
	 * соответствующее сообщение в консоль.
	 *
	 * @param  {object}	destObj     	Объект, в котором св-во нужно сделать геттером.
	 * @param  {string}	oldAPI      	Старое название св-ва.
	 * @param  {any}	val				Значение старого св-ва.
	 * @param  {string} [relevantAPI]	Новое название актуального св-ва (необязательно).
	 */
	function deprecatedProperty (destObj, oldAPI, val, relevantAPI) {
		var msg = 'pbaAPI.' + oldAPI + ' is deprecated.';

		if (relevantAPI) {
			msg += ' Use ' + relevantAPI + ' instead.';
		}

		Object.defineProperty(destObj, oldAPI, {
			configurable: false,
			enumerable: false,
			get: function() {
				warning(msg);
				return val;
			},
			set: function() {
				console.error(msg + ' Value of this property cannot be changed.');
			},
		});
	}

	function warning(message) {
		console.warn(message);
		if (console.trace) {
			console.trace('stack trace');
		}
	}

}());
