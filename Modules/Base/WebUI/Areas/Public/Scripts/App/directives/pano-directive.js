(function (angular) {
    function pano($timeout, $log) {
        return {
            restrict: 'A',
            scope: {
                //scene: '=ngScene',
                scenes: '=ngScenes',
                height: '@ngHeight',
                width: '@ngWidth'
            },
            replace: false,
            //template: '<div class="road-pano"></div>',
            link: function ($scope, $element, $attrs) {
                var reInit = function () {
                    console.log('Scope panoram reinit');
                    //debugger;
                    var panorama = $element.data('ipanorama');

                    if (panorama && panorama.destroy) panorama.destroy();

                    var keys = Object.keys($scope.scenes);

                    if (keys.length) {
                        var key = keys[0];

                        $element.ipanorama({
                            "theme": "ipnrm-theme-default",
                            "autoLoad": true,
                            "autoRotate": false,
                            "autoRotateInactivityDelay": 3000,
                            "mouseWheelRotate": false,
                            "mouseWheelRotateCoef": 0.2,
                            "mouseWheelZoom": false,
                            "mouseWheelZoomCoef": 0.05,
                            "hoverGrab": false,
                            "hoverGrabYawCoef": 20,
                            "hoverGrabPitchCoef": 20,
                            "grab": true,
                            "grabCoef": 0.1,
                            "showControlsOnHover": false,
                            "showSceneThumbsCtrl": false,
                            "showSceneMenuCtrl": false,
                            "showSceneNextPrevCtrl": false,
                            "showShareCtrl": false,
                            "showZoomCtrl": true,
                            "showFullscreenCtrl": true,
                            "sceneThumbsVertical": true,
                            "title": true,
                            "compass": false,
                            "keyboardNav": true,
                            "keyboardZoom": true,
                            "sceneNextPrevLoop": false,
                            "popover": true,
                            "popoverPlacement": "top",
                            "hotSpotBelowPopover": true,
                            "popoverShowTrigger": "hover",
                            "popoverHideTrigger": "leave",
                            "pitchLimits": true,
                            "mobile": false,
                            "sceneId": key,
                            "sceneFadeDuration": 3000,
                            "scenes": $scope.scenes
                        });
                    }
                };

                $scope.$watch('scenes', function (newValue, oldValue) {
                    $log.info('Scope panoram new:', newValue, 'Scope panoram old:', oldValue);
                    if (newValue && !angular.equals(newValue, {})) {
                        reInit();
                    }
                }, true);

                
            }
        };
    };

    angular.module('ngPano', [])
        .directive('ngPano', pano);

    pano.$inject = ['$timeout', '$log'];

})(window.angular);