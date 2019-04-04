/* globals $, pbaAPI */
(function() {
	'use strict';

	/**
	 * Перевод строки (YANDEX API).
	 * @param  {string}		text		Исходных текст для перевода.
	 * @param  {string}		sl 			Язык исходного текста (например, en).
	 * @param  {string}		tl 			Язык, на который нужно перевести (например, ru).
	 * @param  {function}	callback	Колбэк, в который упадет перевод исходного текста или null в случае ошибки.
	 */
	pbaAPI.translate = function(text, sl, tl, callback) {
		$.ajax({
			url: 'https://translate.yandex.net/api/v1.5/tr.json/translate',
			data: {
				key: 'trnsl.1.1.20141221T090334Z.1adf7703a7c35a22.d50641aab4d4417719f2be7f7b578abc8ce3ebdd',
				text: text,
				lang: sl + '-' + tl,
			}
		}).done(function(res) {

			var result = res && res.text && res.text[0] || null;
			callback(result);

		}).fail(function(xhr, message) {

			pbaAPI.errorMsg(message);
			callback(null);

		});
	};

}());
