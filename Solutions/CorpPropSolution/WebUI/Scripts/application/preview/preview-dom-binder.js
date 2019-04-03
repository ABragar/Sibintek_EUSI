/* globals $, pbaAPI, application */
/* jshint strict: false */

pbaAPI.ensurePath('application.preview');

(function () {
    'use strict';

    application.preview.PreviewDOMBinder = {
        SHOW_DELAY: 500,
        HIDE_DELAY: 400
    };

}());

$(function () {
    'use strict';

    var PreviewPool = application.preview.PreviewPool;
    var PreviewDataProvider = application.preview.PreviewDataProvider;
    var PreviewDOMBinder = application.preview.PreviewDOMBinder;

    var _attachedPreviews = new Map();

    $(document.body).on('mouseenter', '[data-mnemonic][data-id]', function (mouseEvent) {
        var mnemonic = this.getAttribute('data-mnemonic');
        var id = this.getAttribute('data-id');
        var $el = this;

        application.viewModelConfigs.get(mnemonic).done(
	        function (config) {
	            if (shouldPreview(config, id)) {
	                $el.setAttribute('data-preview', '');
	            } else {
	                $el.removeAttribute('data-preview');
	                return;
	            }

	            var clientX = mouseEvent.clientX;
	            var preview = _attachedPreviews.get(this);

	            // DON'T WAIT FOR "LONG HOVERED" IF PREVIEW IS ALREADY ACTIVE
	            if (preview && preview.isActive) {
	                return;
	            }

	            var onMouseMove = function (mouseMoveEvent) {
	                clientX = mouseMoveEvent.clientX;
	            };

	            $el.addEventListener('mousemove', onMouseMove);

	            onLongHovered($el, PreviewDOMBinder.SHOW_DELAY, function (element) {
	                element.removeEventListener('mousemove', onMouseMove);

	                var preview = _attachedPreviews.get(element);
	                var timeoutId = null;

	                if (!preview) {
	                    preview = PreviewPool.createPreview(element,
	                        new PreviewDataProvider(mnemonic, id));

	                    _attachedPreviews.set(element, preview);

	                    preview.on('destroy', function() { _attachedPreviews.delete(element); });
	                } else {
	                    preview._provider._id = id;
	                    preview._provider._mnemonic = mnemonic;
	                }

	                var parentPreviewElement = $(element).closest('.preview-tooltip');
	                var parentPreview = parentPreviewElement.data('preview-tooltip');
	                if (parentPreview) {
	                    parentPreview.registerChild(preview);
	                    preview.on('deactivation', function () {
	                        if (!parentPreview.isHovered) {
	                            parentPreview.deactivate();
	                        }
	                    });
	                }

	                preview.activate(clientX);

	                preview.on('error', function (err) {
	                    _attachedPreviews.delete(element);

	                    preview.destroy();

	                    preview = null;

	                    pbaAPI.errorMsg(err.message);
	                });

	                var cancelDeactivation = function () {
	                    clearTimeout(timeoutId);
	                };
	                var queueDeactivation = function () {
	                    clearTimeout(timeoutId);

	                    timeoutId = setTimeout(function () {

	                        // IF PREVIEW IS DESTROYED OR
	                        // IT OR ANY OF IT CHILDREN HAS
	                        // HOVERED STATE
	                        if (!preview || preview.isHovered || preview.hasHoveredChild) return;

	                        preview.off('mouseenter', cancelDeactivation);
	                        preview.off('mouseleave', queueDeactivation);

	                        element.removeEventListener('mouseenter', cancelDeactivation);
	                        element.removeEventListener('mouseleave', queueDeactivation);

	                        preview.deactivate();
	                        // if (parentPreview && !parentPreview.isHovered) {
	                        // 	parentPreview.deactivate();
	                        // }
	                    }, PreviewDOMBinder.HIDE_DELAY);
	                };

	                preview.on('mouseenter', cancelDeactivation);
	                preview.on('mouseleave', queueDeactivation);

	                element.addEventListener('mouseenter', cancelDeactivation);
	                element.addEventListener('mouseleave', queueDeactivation);
	            });

	        });
    });

    function shouldPreview(config, id) {

        // RECEIVE VIEW MODEL CONFIG IN SYNCHRONOUS FLOW.
        // IMPORTANT: PREVIEW SHOULD NOT SHOW UP, IF
        // <config.Preview> IS FALSE
        return (id > 0) && config && config.Preview ? true : false;
    }

    function onLongHovered(element, duration, callback) {
        var timeoutId = null;

        var onMouseLeave = function () {
            clearTimeout(timeoutId);
        };

        timeoutId = setTimeout(function () {
            element.removeEventListener('mouseleave', onMouseLeave);
            callback(element);
        }, duration);

        element.addEventListener('mouseleave', onMouseLeave);
    }
});
