using System.Web.Optimization;
using WebUI.Concrete;

namespace WebUI.Areas.Public
{
    public class BundleConfig
    {
        private const string StyleNamespace = "CSS";
        private const string ScriptNamespace = "JS";
        private const string AreaName = "Public";

        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterStyleBundle(bundles, "Vendor", new[]
            {
                 $"~/Areas/{AreaName}/vendor/leafletjs/leaflet.css",
                 $"~/Areas/{AreaName}/vendor/leafletjs/leaflet-openweathermap.css",
                 $"~/Areas/{AreaName}/vendor/marker-cluster/MarkerCluster.Default.css",

                 $"~/Areas/{AreaName}/vendor/fancybox/jquery.fancybox.css",

                 $"~/Areas/{AreaName}/vendor/ipanorama/effect.css",

                 $"~/Areas/{AreaName}/vendor/perfectscrollbar/perfect-scrollbar.min.css",
                 $"~/Areas/{AreaName}/vendor/icheck-1.x/skins/all.css",
                 $"~/Areas/{AreaName}/vendor/angular/rzslider.css",

                 $"~/Areas/{AreaName}/vendor/ipanorama/ipanorama.css",
            });

            RegisterStyleBundle(bundles, "Common", new[]
            {
                 $"~/Areas/{AreaName}/Content/Fonts/style.css",
                 $"~/Areas/{AreaName}/Content/styles.css"
            });

            RegisterScriptBundle(bundles, "Common", new[]
            {
                 $"~/Areas/{AreaName}/vendor/signalr/jquery.signalR.js",
                 $"~/Areas/{AreaName}/vendor/fancybox/jquery.fancybox.js",
                 $"~/Areas/{AreaName}/vendor/momentjs/moment-with-locales.js",
                 $"~/Areas/{AreaName}/vendor/perfectscrollbar/perfect-scrollbar.jquery.js",
                 $"~/Areas/{AreaName}/vendor/icheck-1.x/icheck.js",
                 $"~/Areas/{AreaName}/vendor/weatherjs/jquery.simpleWeather.js",
                 $"~/Areas/{AreaName}/vendor/ipanorama/three.min.js",
                 $"~/Areas/{AreaName}/vendor/ipanorama/jquery.ipanorama.min.js",
                 $"~/Areas/{AreaName}/vendor/string-format/string-format.js",
            });

            RegisterScriptBundle(bundles, "AngularVendor", new[]
            {
                 $"~/Areas/{AreaName}/vendor/angular/angular.js",
                 $"~/Areas/{AreaName}/vendor/angular/angular-animate.js",
                 $"~/Areas/{AreaName}/vendor/angular/angular-route.js",
                 $"~/Areas/{AreaName}/vendor/angular/angular-cookies.js",
                 $"~/Areas/{AreaName}/vendor/angular/angular-clipboard.js",
                 $"~/Areas/{AreaName}/vendor/angular/angular-tiny-eventemitter.min.js",
                 $"~/Areas/{AreaName}/vendor/angular/angular-perfect-scrollbar.js",
                 $"~/Areas/{AreaName}/vendor/angular/ui-bootstrap-tpls-1.0.3.min.js",
                 $"~/Areas/{AreaName}/vendor/angular/rzslider.js",
                 $"~/Areas/{AreaName}/vendor/angular/re-tree.js",
                 $"~/Areas/{AreaName}/vendor/angular/ng-device-detector.js",
                 $"~/Areas/{AreaName}/vendor/angular/angular-dropdownMultiselect.js",
            });

            RegisterScriptBundle(bundles, "AngularApp", new[]
            {
                 //Directives
                 $"~/Areas/{AreaName}/Scripts/App/directives/angular-moment.js",
                 $"~/Areas/{AreaName}/Scripts/App/directives/uib-accordion-tree.js",
                 $"~/Areas/{AreaName}/Scripts/App/directives/angular-pageslide-directive.js",
                 $"~/Areas/{AreaName}/Scripts/App/directives/property-viewer-directive.js",
                 $"~/Areas/{AreaName}/Scripts/App/directives/icheck-directive.js",
                 $"~/Areas/{AreaName}/Scripts/App/directives/leaflet-directive.js",
                 $"~/Areas/{AreaName}/Scripts/App/directives/iframe-directive.js",
                 $"~/Areas/{AreaName}/Scripts/App/directives/pano-directive.js",
                 $"~/Areas/{AreaName}/Scripts/App/directives/content-widget-directive.js",
                 $"~/Areas/{AreaName}/Scripts/App/directives/kladr-directive.js",
                 $"~/Areas/{AreaName}/Scripts/App/directives/fancybox-directive.js",
                 //Filters
                 $"~/Areas/{AreaName}/Scripts/App/filters/map-filter.js",
                 //Application
                 $"~/Areas/{AreaName}/Scripts/App/app.js",
                 //Factories
                 $"~/Areas/{AreaName}/Scripts/App/services/map-tiles-factory.js",
                 $"~/Areas/{AreaName}/Scripts/App/services/map-controls-factory.js",
                 $"~/Areas/{AreaName}/Scripts/App/services/url-factory.js",
                 //Constants
                 $"~/Areas/{AreaName}/Scripts/App/constants/layers-constants.js",
                 $"~/Areas/{AreaName}/Scripts/App/constants/global-constants.js",
                 $"~/Areas/{AreaName}/Scripts/App/constants/kadastr-constants.js",
                 //Models
                 $"~/Areas/{AreaName}/Scripts/App/data/view-config-model.js",
                 $"~/Areas/{AreaName}/Scripts/App/data/geo-item-model.js",
                 $"~/Areas/{AreaName}/Scripts/App/data/search-result-model.js",
                 $"~/Areas/{AreaName}/Scripts/App/data/content-widget-model.js",
                 //Services
                 $"~/Areas/{AreaName}/Scripts/App/services/data-service.js",
                 $"~/Areas/{AreaName}/Scripts/App/services/signal-service.js",
                 $"~/Areas/{AreaName}/Scripts/App/services/crud-service.js",
                 $"~/Areas/{AreaName}/Scripts/App/services/map-provider-service.js",
                 $"~/Areas/{AreaName}/Scripts/App/services/utils-service.js",
                 $"~/Areas/{AreaName}/Scripts/App/services/map-service.js",
                 $"~/Areas/{AreaName}/Scripts/App/services/widget-service.js",
                 $"~/Areas/{AreaName}/Scripts/App/services/global-search-service.js",
                 $"~/Areas/{AreaName}/Scripts/App/services/navigation-service.js",
                 $"~/Areas/{AreaName}/Scripts/App/services/pano-service.js",
                 $"~/Areas/{AreaName}/Scripts/App/services/map-cadastre-service.js",
                 //Controllers
                 $"~/Areas/{AreaName}/Scripts/App/controllers/main-controller.js",
                 $"~/Areas/{AreaName}/Scripts/App/controllers/menu-controller.js",
                 $"~/Areas/{AreaName}/Scripts/App/controllers/map-controller.js",
                 $"~/Areas/{AreaName}/Scripts/App/controllers/detail-controller.js",
                 $"~/Areas/{AreaName}/Scripts/App/controllers/cluster-list-controller.js",
                 $"~/Areas/{AreaName}/Scripts/App/controllers/mnemonic-list-controller.js",
                 $"~/Areas/{AreaName}/Scripts/App/controllers/search-controller.js",
                 $"~/Areas/{AreaName}/Scripts/App/controllers/filter-controller.js",
                 $"~/Areas/{AreaName}/Scripts/App/controllers/cadastre-controller.js",
                 $"~/Areas/{AreaName}/Scripts/App/controllers/content-controller.js",
                 $"~/Areas/{AreaName}/Scripts/App/controllers/panoram-controller.js",
            });

            RegisterScriptBundle(bundles, "Leaflet", new []
            {
                 $"~/Areas/{AreaName}/vendor/leafletjs/leaflet-src.js",
                 $"~/Areas/{AreaName}/vendor/leafletjs/leaflet.geometryutil.js",
                 $"~/Areas/{AreaName}/vendor/leafletjs/leaflet.pattern-src.js",
                 $"~/Areas/{AreaName}/vendor/leafletjs/leaflet.extensions.js",
                 $"~/Areas/{AreaName}/vendor/leafletjs/leaflet-openweathermap.js",
                 $"~/Areas/{AreaName}/vendor/leafletjs/Yandex.js",
                 $"~/Areas/{AreaName}/vendor/leafletjs/Google.js",
                 $"~/Areas/{AreaName}/vendor/leafletjs/TileLayer.Rosreestr.js",
                 $"~/Areas/{AreaName}/vendor/leafletjs/leaflet.easyPrint.js",
                 $"~/Areas/{AreaName}/vendor/leafletjs/leaflet-weather-control.js",
                 $"~/Areas/{AreaName}/vendor/leafletjs/leaflet-focus-control.js",
                 $"~/Areas/{AreaName}/vendor/leafletjs/leaflet-pba-logo-control.js",
                 $"~/Areas/{AreaName}/vendor/marker-cluster/leaflet.markercluster-src.js",
                 $"~/Areas/{AreaName}/vendor/marker-cluster/MarkerClusterGroup.Refresh.js",
                 $"~/Areas/{AreaName}/vendor/marker-cluster/MarkerClusterGroup.AddLayers.js",
                 $"~/Areas/{AreaName}/vendor/marker-cluster/MarkerCluster.Extentions.js",
                 $"~/Areas/{AreaName}/vendor/marker-cluster/leaflet.markercluster.freezable-src.js",
                 $"~/Areas/{AreaName}/vendor/marker-cluster/leaflet.markercluster-src.js",
            });
        }

        private static void RegisterStyleBundle(BundleCollection bundleCollection, string bundleName, string[] files)
        {
            var bundle = new StyleBundle($"~/{AreaName}/{StyleNamespace}/{bundleName}");

            foreach (var file in files)
            {
                bundle.Include(file, new CssRewriteUrlTransformFixed());
            }

            bundleCollection.Add(bundle);
        }

        private static void RegisterScriptBundle(BundleCollection bundleCollection, string bundleName, string[] files)
        {
            var bundle = new ScriptBundle($"~/{AreaName}/{ScriptNamespace}/{bundleName}");
            
            bundle.Include(files);

            //TODO: Angular injector propblems
            bundle.Transforms.Clear();

            bundleCollection.Add(bundle);
        }

    }
}