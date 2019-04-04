/* globals pbaAPI */
(function() {
	'use strict';

	/**
	 * Проверка на пустой объект.
	 * @param  {any}		obj Проверяемый объект.
	 * @return {Boolean}		Вернет true, если переданный объект не имеет ни одного св-ва.
	 */
	pbaAPI.isEmpty = function(obj) {
		if (obj === null || obj === void 0) {
			return true;
		}

        // ReSharper disable once MissingHasOwnPropertyInForeach
        /* jshint unused: false */
		for (var key in obj) {
			return false;
		}

		return true;
	};

	/**
	 * Проверка на функцию.
	 * @param  {any}		obj Проверяемый объект.
	 * @return {boolean}		Вернет true, если переданный объект - функция.
	 */
	pbaAPI.isFunction = function(obj) {
		return !!(obj && obj.constructor && obj.call && obj.apply);
	};

	/**
	 * Проверка на массив.
	 * @param  {any} 		obj Проверяемый объект.
	 * @return {boolean}		Вернет true, если переданный объект - массив.
	 */
	pbaAPI.isArray = function(obj) {
		return obj && Array.isArray(obj);
	};

	/**
	 * Проверка на то, что объект является массивом ИЛИ похож на массив
	 * (имеет свойство length числового типа).
	 *
	 * @param  {any} 		obj Проверяемый объект.
	 * @return {boolean}		Вернет true, если переданный объект - массив или имеет числовое свойство 'length'.
	 */
	pbaAPI.isArrayLike = function(obj) {
		return obj && (Array.isArray(obj) || typeof obj === 'object' && obj.hasOwnProperty('length') && typeof obj.length === 'number');
	};


    pbaAPI.boolParse = function(val) {
        var falsy = /^(?:f(?:alse)?|no?|0+)$/i;
        return !falsy.test(val) && !!val;
    };
}());
