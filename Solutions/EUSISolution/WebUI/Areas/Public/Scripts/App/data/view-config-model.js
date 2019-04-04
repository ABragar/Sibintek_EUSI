(function (angular) {
    var getChildrens = function(childs) {
        var childrens = [];

        childs.forEach(function(children) {
            childrens.push(children);

            if (children.Children.length) {
                childrens = childrens.concat(getChildrens(children.Children));
            }
        });

        return childrens;
    };

    angular.module('MapApp')
        .factory('ViewConfig', ['$log', function($log) {
            var ViewConfig = function(config, mnemonic) {
                this.LayerId = config.LayerId;
                this.Mnemonic = config.Mnemonic;

                this.Checked = config.Checked;
                this.Lazy = config.Lazy;
                this.ClientClustering = config.ClientClustering;
                this.Count = config.Count;
                this.DetailView = config.DetailView;
                this.Filterable = config.Filterable;
                this.Filters = config.Filters;
                this.Load = config.Load;
                this.MaxSearchZoom = config.MaxSearchZoom;
                this.MinSearchZoom = config.MinSearchZoom;
                this.Mode = config.Mode;
                this.SearchOnClick = config.SearchOnClick;
                this.ServerClustering = config.ServerClustering;
                this.ServerClusteringMaxZoom = config.ServerClusteringMaxZoom;
                this.Style = config.Style;
                this.Title = config.Title;
                this.ParentMnemonic = angular.isDefined(mnemonic) ? mnemonic : null;
                this.Children = [];

                this.FullTextSearch = (function () {
                    return config.EnableFullTextSearch && !(config.Children && config.Children.length);
                })(); 

                this.HasChildren = (function() {
                    return !!(config.Children && config.Children.length);
                })();

                this.HasParent = this.ParentMnemonic !== null;

                this.IsVisible = function() {
                    //Return all loadable and visible and not searchable configs
                    return this.Load && this.Checked && !this.SearchOnClick;
                };

                this.GetAllChildren = function () {
                    if (!this.HasChildren) return [];
                    return getChildrens(this.Children);
                };

                this.IsIndeterminated = function () {
                    if (this.Checked || !this.HasChildren) return false;

                    var visible = this.GetAllChildren().filter(function (child) {
                        return child.IsVisible();
                    });

                    return visible.length;
                };

                this.IsStatic = (function() {
                    return config.Mode === 1 && !config.SearchOnClick;
                })();

                this.IsSearchable = (function() {
                    return config.SearchOnClick;
                })();

                this.IsDynamic = (function() {
                    return config.Mode === 2 && !config.SearchOnClick;
                })();

                this.IsLoadable = (function() {
                    return config.Load;
                })();

                this.IsFilterable = (function() {
                    return config.Filterable;
                })();

                this.IsClientCluster = (function() {
                    return config.ClientClustering;
                })();

                this.IsServerCluster = (function() {
                    return config.ServerClustering;
                })();

                this.ShowIcon = (function () {
                    return angular.isDefined(config.Style) && config.Style.ShowIcon;
                })();

                this.GetCount = function () {
                    if (this.HasChildren) {
                        return this.Children.map(function (children) {
                            return children.GetCount ? children.GetCount() : 0;
                        }).reduce(function (a, b) {
                            return a + b;
                        }, 0);
                    } else {
                        return this.Count;
                    }
                };

                this.AddFilter = function(filter) {
                    if (!angular.isArray(this.Filters)) this.Filters = [];

                    this.Filters.push(filter);
                };

                this.ClearFilters = function() {
                    this.Filters = null;
                };
            };

            return ViewConfig;
        }]);
})(window.angular);