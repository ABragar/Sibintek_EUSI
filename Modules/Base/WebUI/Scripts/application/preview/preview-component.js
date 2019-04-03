/* globals $, pbaAPI, application */
/* jshint strict: false */

pbaAPI.ensurePath('application.preview');

(function() {
    'use strict';

    var Placement = {
    	TOP: 1,
    	BOTTOM: 2
    };

    var ANIMATION_DURATION = 300;

    /**
     * Emits:
     * event: 'error', data: Error
     * event: 'destroy'
     * event: 'deactivate'
     * @constructor
     * @param {DOMElement|jquery}	element			Element, preview shall be binded for.
     * @param {PreviewDataProvider} dataProvider
     */
    var PreviewComponent = application.preview.PreviewComponent = function(element, dataProvider) {

    	// INITIALIZE PRIVATES
    	this._uid = pbaAPI.uid('preview_');
    	this._provider = dataProvider;
    	this._listeners = new Map();
    	this._childPreviews = [];
    	this._viewModel = null;

    	this._destroyed = false;

    	// EXPORT PREVIEW API
    	window[this._uid] = { preview: this };

    	// EXTRACT REQUIRED JQUERY ELEMENTS
    	this.$element = createElement();
    	this.$content = _getContent.call(this);
    	this.$bound = element.jquery ? element : $(element);

    	// APPEND PREVIEW TO JQUERY DATA OBJECT
    	this.$element.data('preview-tooltip', this);

    	// REQUIRED DOM EVENTS DELEGATION THROW PREVIEW EVENT EMITTING
    	_bindEvents.call(this);

    	// INITIALIZE PRIVATE STATE
    	this._state = getInitialState();

    	// APPLY STATE TO DOM ELEMENT
    	_applyState.call(this);

    	// APPEND PREVIEW ELEMENT TO DOM
    	document.body.appendChild(this.$element[0]);
    };

    PreviewComponent.prototype = {

    	/**
    	 * Is preview is visible.
    	 * @return {boolean}
    	 */
    	get isActive() { return this._state.active; },

    	/**
    	 * ...
    	 * @return {boolean}
    	 */
    	get isHovered() { return this._state.hovered; },

    	/**
    	 * ...
    	 * @return {boolean} [description]
    	 */
    	get isDestroyed() { return this._destroyed; },

    	/**
    	 * ...
    	 * @return {boolean}
    	 */
    	get hasHoveredChild() {
    		return this._childPreviews.some(function(preview) {
				return preview.isHovered;
			});
    	},

    	/**
    	 * Call this method to open a preview.
    	 *
    	 * @param  {number}				(xpos)		Css 'left' property in number format, where preview must be positionated (optional).
    	 * @param  {DOMElement|jquery}	(element)	DOM element, preview shall be rebinded for (optional).
    	 */
    	activate: function(xpos, element) {

    		// IF CONSTRUCTOR AND THIS METHOD WAS CALLED WITH NO GIVEN element PARAMETER
    		if (!this.$bound && !element) {
    			this.emit('error', Error('No element was provided for preview'));
    			this.deactivate();
    			return;
    		}

    		// RESOLVE TARGET ELEMENT (IT MAY BE A NEW GIVEN ELEMENT OR ALREADY BOUNDED ONE)
    		var $element = !element ? this.$bound
    			: element.jquery ? element
    			: $(element);

    		// IF PREVIEW'S ALREADY HAVE AN ACTIVE STATE (IT'S VISIBLE NOW)
    		if (this._state.active) {
    			if (this.$bound[0] !== $element[0]) {
    				this.$bound = $element;
    				this.update(null, xpos);
    			}
    			return;
    		}

    		this.$bound = $element;

    		this._state.placement = _resolvePlacement.call(this);
    		this._state.active = true;

    		_applyState.call(this);

    		this.update(null, xpos);

    		this.fetch();
    	},

    	/**
    	 * Call this method to hide preview.
    	 * If
    	 * @return {[type]} [description]
    	 */
    	deactivate: function() {
    		if (!this._state.active) {
    			return;
    		}

    		this._state.active = false;
    		this._state.ready = false;

    		this._childPreviews.forEach(function(preview) {
    			preview.deactivate();
    		});

    		_applyState.call(this);

    		this.emit('deactivation');
    	},

    	registerChild: function(preview) {

    		// IF THIS PREVIEW ALREADY IN CHILDREN LIST
    		if (this._childPreviews.some(function(p) {
    			return p === preview;
    		})) { return; }

			this._childPreviews.push(preview);

			// ON CHILD PREVIEW DESTROY
			preview.on('destroy', function() {

				// REMOVE THIS PREVIEW FROM CHILDREN LIST
				this._childPreviews = this._childPreviews.filter(function(p) {
					return p !== preview;
				});

			}.bind(this));
    	},

    	fetch: function(callback) {

    		// COLLAPSE PREVIEW TO SHOW A LOADING STATE
    		this.$element.stop().animate({
    			width: 0,
    			height: 0
    		}, ANIMATION_DURATION);

    		this._childPreviews.forEach(function(preview) {
    			preview.destroy();
    		});

    		this._state.ready = false;
	    	this._state.loading = true;
	    	_applyState.call(this);

	    	var component = this;

	    	Promise.all([
	    		this._provider.loadTemplate(),
	    		this._provider.loadViewModel()
			]).then(function(results) {

				// EXTRACT USEFUL DATA
				var template = results[0];
				var viewModel = results[1];

				// IF MODEL NOT CHANGED
				if (component._viewModel !== viewModel) {

					// CACHING MODEL
					component._viewModel = viewModel;

					// RENDER TEMPLATE AND REPLACE CURRENT CONTENT HTML
					component.$content.html(template({
						uid: component._uid,
						model: viewModel
                    }));

                    kendo.bind(component.$content, viewModel);
				}

				// CHANGE LOADING STATE
				component._state.loading = false;
				_applyState.call(component);

				// IF PREVIEW ALREADY DEACTIVATED
				if (!component._state.active) {
					return;
				}

				// APPLY CONTENT SIZE TO TOOLTIP
				component.update(function() {

					// CHANGE READY STATE
					component._state.ready = true;
					_applyState.call(component);

					if (callback) {
						callback(null);
					}
				});

			}).catch(function(err) {
				component.deactivate();
				component.emit('error', err);

				if (callback) {
					callback(err);
				}
			});
	    },

	    update: function(callback, xpos) {

	    	// CANCEL UPDATE IF IT'S NOT ACTIVE NOW
	    	if (!this._state.active) {
	    		if (callback) callback();
	    		return;
	    	}

			var targetWidth = this._state.loading ? this.$element.width() : this.$content.outerWidth(true);
			var targetHeight = this._state.loading ? this.$element.height() : this.$content.outerHeight(true);

			var windowWidth = window.innerWidth;

			var boundOffset = this.$bound.offset();
			var boundHeight = this.$bound.outerHeight(true);

			// RESOLVE NEW PREVIEW POSITION
			var left = xpos ? xpos
				: this.$element.css('left') === 'auto' ? (boundOffset.left + targetWidth / 2)
				: parseInt(this.$element.css('left'));
			var top = this._state.placement === Placement.TOP ? (boundOffset.top - targetHeight)
				: (boundOffset.top + boundHeight);

			// PREVENT PREVIEW TO GO OUTSIDE FROM WINDOW BOUNDS
			if (left + targetWidth > windowWidth) {
				if (targetWidth <= windowWidth) {
					left = windowWidth - targetWidth;
				} else {
					left = 0;
				}
			}

			// APPLY NEW POSITION TO PREVIEW
			this.$element[0].style.left = left ? (left + 'px') : 0;
			this.$element[0].style.top = top + 'px';

			// EXPAND PREVIEW SIZE TO FIT IT'S CONTENT WIDTH
			this.$element.stop().animate({
				width: targetWidth,
				height: targetHeight
			}, ANIMATION_DURATION, callback || function() {});
	    },

	   	/**
		 * [destroy description]
		 */
		destroy: function() {

			if (this._destroyed) {
				return;
			}

			this._destroyed = true;

			// HIDE PREVIEW
			this.deactivate();

			// TIMEOUT IS FOR ALLOW TRANSITION TO END,
			// BEFORE DOM ELEMENT WILL REMOVED
			setTimeout(function() {

				// REMOVE PREVIEW FROM DOM
				this.$element.remove();
				this.$element.data('preview-tooltip', null);

				// REMOVE ALL SAVED JQUERIES
				this.$element = null;
				this.$content = null;
				this.$bound = null;

			}.bind(this), ANIMATION_DURATION);

			this._childPreviews.forEach(function(preview) { preview.destroy(); });
			//this._childPreviews = [];

			// THIS EMIT ALSO UNBIND mouseenter AND mouseleave EVENTS
			// (see: _bindEvents() below this prototype definition)
			this.emit('destroy');

			// REMOVES ALL OBJECTS PREVENT 'GHOST' CLOSURES
			this._state = null;
			this._provider = null;
			this._listeners = null;
			this._viewModel = null;

			// REMOVE PUBLIC API
			// TODO: DECIDE, WHEN PUBLIC API MUST BE REMOVED, AFTER OR BEFORE 'destroy' EVENT EMITTED
			delete window[this._uid];
		},

		// ############
		// EventEmitter
		// ############

		/**
		 * Emit event (data is optionally).
		 * @param  {string} event
		 * @param  {any}	(data)
		 */
		emit: function(event, data) {

			// PREVIEW IS DESTROYED
			if (!this._listeners) {
				return;
			}

			if (this._listeners[event]) {
				this._listeners[event].forEach(function(handler) {
					handler(data);
				});
			}
		},

		/**
		 * Add event listener.
		 * @param  {string}		event
		 * @param  {function}	handler
		 */
		on: function(event, handler) {

			// PREVIEW IS DESTROYED
			if (!this._listeners) {
				return;
			}

			this._listeners[event] = this._listeners[event] || [];
			this._listeners[event].push(handler);
		},

		/**
		 * Remove event listener.
		 * @param  {string}		event
		 * @param  {function}	handler
		 */
		off: function(event, handler) {

			// PREVIEW IS DESTROYED
			if (!this._listeners) {
				return;
			}

			if (this._listeners[event]) {
				this._listeners[event] = this._listeners[event].filter(function(h) {
					return h !== handler;
				});
			}
		}
    };

    function createElement() {
    	var element = document.createElement('DIV');
    	var content = document.createElement('DIV');
    	var loader = document.createElement('DIV');

    	element.className = 'preview-tooltip';
    	content.className = 'preview-tooltip__content';
    	loader.className = 'preview-tooltip__loader';

    	element.appendChild(content);
    	element.appendChild(loader);

    	return $(element);
    }

    function getInitialState() {
    	return {
    		active: false,
    		hovered: false,
    		loading: true,
    		ready: false,
    		placement: null
    	};
    }

    function _bindEvents() {
    	/* jshint validthis: true */
    	var component = this;
    	var element = this.$element[0];

		var onMouseEnter = function() {
			component._state.hovered = true;
			component.emit('mouseenter');
		};
		var onMouseLeave = function() {
			component._state.hovered = false;
			component.emit('mouseleave');
		};

		element.addEventListener('mouseenter', onMouseEnter);
		element.addEventListener('mouseleave', onMouseLeave);

		this.on('destroy', function() {
			element.removeEventListener('mouseenter', onMouseEnter);
			element.removeEventListener('mouseleave', onMouseLeave);
		});
    }

    function _getContent() {
    	/* jshint validthis: true */
    	return this.$element.children().eq(0);
    }

    function _applyState() {
    	/* jshint validthis: true */
    	var state = this._state;

    	this.$element
    		.toggleClass('preview-tooltip--active', state.active)
    		.toggleClass('preview-tooltip--loading', state.loading)
    		.toggleClass('preview-tooltip--ready', state.ready)
    		.toggleClass('preview-tooltip--above', state.placement === Placement.TOP)
    		.toggleClass('preview-tooltip--under', state.placement === Placement.BOTTOM);
    }

    function _resolvePlacement() {
    	/* jshint validthis: true */
    	var windowHeight = window.innerHeight;
		var boundOffset = this.$bound.offset();
		var boundHeight = this.$bound.outerHeight(true);

		var placeOnTop = boundOffset.top;
		var placeOnBottom = windowHeight - placeOnTop - boundHeight;
		var placement = placeOnTop >= placeOnBottom ? Placement.TOP : Placement.BOTTOM;

		return placement;
    }
}());
