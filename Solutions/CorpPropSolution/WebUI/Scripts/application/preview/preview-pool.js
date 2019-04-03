/* globals pbaAPI, application */
/* jshint strict: false */

pbaAPI.ensurePath('application.preview');

(function() {
	'use strict';

	// ############
	// DEPENDENCIES
	// ############

	var SpaAdapter = application.spa.adapter;
	var PreviewComponent = application.preview.PreviewComponent;

	// #######
	// PRIVATE
	// #######

	var pool = new Set();

	// ###
	// API
	// ###

	application.preview.PreviewPool = {
		createPreview: function() {
			var preview = new construct(PreviewComponent, arguments);

			pool.add(preview);

			return preview;
		}
	};

	// DESTROY ALL PREVIEWS WHEN PAGE IS CHANGING
	SpaAdapter.onChange(function() {
		pool.forEach(function(preview) { preview.destroy(); });
		pool.clear();
	});

	// http://stackoverflow.com/questions/1606797/use-of-apply-with-new-operator-is-this-possible#answer-1608546
	function construct(constructor, args) {
		function F() { return constructor.apply(this, args); }
		F.prototype = constructor.prototype;
		return new F();
	}
}());
