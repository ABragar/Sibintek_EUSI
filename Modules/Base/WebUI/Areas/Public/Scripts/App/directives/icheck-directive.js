(function (angular) {
    function icheck($log, $timeout, $parse) {
        return {
            restrict: "A",
            require: "ngModel",
            scope: {
                disabled: "=ngDisabled",
                model: "=ngModel",
                value: "=ngValue",
                indeterminate: "=ngIndeterminate"
                //checked: "=ngChecked"
            },
            link: function ($scope, element, $attrs, ngModel) {
                return $timeout(function () {

                    //console.log('$scope.indeterminate', $scope.indeterminate);

                    var value = $scope.value; // $attrs["value"];

                    //$log.info('Tilelayer model|value:', ngModel, value, $scope.model);

                    $scope.$watch("model", function (newValue) {
                        $(element).iCheck("update");
                    });

                    $scope.$watch("disabled", function (newValue) {
                        if (newValue) {
                            //console.log("Disable change:", newValue, $(element));
                            $(element).iCheck("disable");
                        } else {
                            $(element).iCheck("enable");
                        }
                        $(element).iCheck("update");
                    });

                    $scope.$watch("indeterminate", function (indeterminate) {
                        //console.log('indeterminate', $scope.indeterminate, $(element));
                        $(element).iCheck(indeterminate ? "indeterminate" : "determinate");
                        //$(element).iCheck("update");
                    });

                    var theme = $attrs["ngIcheckTheme"] ? $attrs["ngIcheckTheme"] : "square";
                    var color = $attrs["ngIcheckColor"] ? $attrs["ngIcheckColor"] : "";

                    return $(element).iCheck({
                        checkboxClass: "icheckbox_" + theme + (color.length ? "-" + color : ""),
                        radioClass: "iradio_" + theme + (color.length ? "-" + color : ""),
                        increaseArea: '20%',
                        indeterminateClass: 'indcheck'
                    }).on("ifChanged", function (event) {
                        //debugger;

                        if ($(element).attr("type") === "checkbox" && $attrs["ngModel"]) {
                            $scope.$applyAsync(function () {
                                return ngModel.$setViewValue(event.target.checked);
                            });
                        }

                        if ($(element).attr("type") === "radio" && $attrs["ngModel"]) {
                            //$scope.$applyAsync(function () {
                            //    return ngModel.$setViewValue(event.target.checked);
                            //});

                            return $scope.$applyAsync(function () {
                                //$scope.model = value;
                                return ngModel.$setViewValue(value);
                            });
                        }
                    });
                });
            }
        };
    }

    angular.module("ngICheck", [])
        .directive("ngIcheck", icheck);

    icheck.$inject = ["$log", "$timeout", '$parse'];

})(window.angular);