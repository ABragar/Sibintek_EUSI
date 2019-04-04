using System.IO;
using System.Web.Optimization;
using WebUI.Concrete;

namespace WebUI
{
    public class BundleConfig
    {
        private const string StyleNamespace = "CSS";
        private const string ScriptNamespace = "JS";

        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = false;

            RegisterStyleBundle(bundles, "Common", new[]
            {
                // KENDO
                "~/Content/vendor/kendo/styles/kendo.common.min.css",
                "~/Content/vendor/kendo/styles/kendo.common-material.min.css",
                "~/Content/vendor/kendo/styles/kendo.material.min.css",
                "~/Content/vendor/kendo/styles/kendo.mobile.switch.css",
                "~/Content/vendor/kendo/styles/telerikReportViewer.css",
                "~/Content/less/kendo.overrides.css",
                // BOOTSTRAP
                //"~/Content/vendor/bootstrap/css/bootstrap.css",
                //"~/Content/vendor/bootstrap-switch/css/bootstrap-switch.css",
                // TOOLTIPSTER
                "~/Content/vendor/tooltipster/css/tooltipster.bundle.css",
                "~/Content/vendor/tooltipster/css/plugins/tooltipster/sideTip/tooltipster-sideTip.min.css",
                "~/Content/vendor/tooltipster/css/plugins/tooltipster/sideTip/themes/tooltipster-sideTip-noir.min.css",
                // FONTS
                "~/Content/fonts/fira/fira.css",
                "~/Content/fonts/roboto/roboto.css",
                "~/Content/fonts/pt-sans/pt-sans.css",
                "~/Content/fonts/glyphicons/glyphicons-regular.css",
                "~/Content/fonts/glyphicons/glyphicons-halflings-regular.css",
                "~/Content/fonts/glyphicons/glyphicons-filetypes-regular.css",
                "~/Content/fonts/glyphicons/glyphicons-social-regular.css",
                "~/Content/fonts/fontello/fontello.css",
                "~/Content/fonts/mdi/materialdesignicons.css",
                "~/Content/fonts/font-awesome/font-awesome.css",
                "~/Content/fonts/custom/custom-fonts.css",
                // LEAFLET
                "~/Content/vendor/leaflet/Leaflet.core/leaflet.css",
                "~/Content/vendor/leaflet/Leaflet.draw/leaflet.draw.css",
                "~/Content/vendor/leaflet/Leaflet.markercluster/MarkerCluster.css",
                "~/Content/vendor/leaflet/Leaflet.markercluster/MarkerCluster.Default.css",
                // ANIMATE_CSS
                "~/Content/vendor/animate-css/animate.css",
                "~/Content/css/timeline.css",
                // PERFECT-SCROLLBAR
                "~/Content/vendor/perfect-scrollbar/css/perfect-scrollbar.css",
                // JQVMAP
                "~/Content/vendor/jqvmap/jqvmap.css",
                //FIXEDSTICKY
                "~/Content/vendor/fixedsticky-fixedfixed/fixedsticky.css",
                //CROPPIE
                "~/Content/vendor/croppie/croppie.css",


                // MAIN_CSS
//                "~/Content/less/bootstrap.animations.css",

//                "~/Content/less/normalize.css",
//                "~/Content/less/common.css",
//                "~/Content/less/helpers.css",

//                "~/Content/css/icons.css",
                //"~/Content/css/site.css",
//                "~/Content/less/lite-grid.css",
//                "~/Content/less/migration.css",
//                "~/Content/less/layout.css",
                //"~/Content/css/application.spa.css",
//                "~/Content/less/hamburger.css",
//                "~/Content/less/plugins.css",
//                "~/Content/less/dialog.css",
//                "~/Content/less/dashboard.css",
//                "~/Content/less/preview.css",
//                "~/Content/less/chat.css",
//                "~/Content/less/workflow.css",
//                "~/Content/less/components.css",
//                "~/Content/less/realty/graph.css",
                //"~/Content/css/data-style.css",
//                "~/Content/less/listview.css",
//                "~/Content/less/content-editor.css",
//                "~/Content/css/wizard.css",

                // COMMUNICATION
//                "~/Content/less/videoconference.css",

                // UI
//                "~/Content/ui/geobox/geobox.css",
//                "~/Content/ui/mapcache/vendor/termlib/term_styles.css",
//                "~/Content/ui/mapcache/styles/css/mapcache.css",

                //VIS
//                "~/Content/vendor/vis/vis.min.css",

                //Concatinating all styles
                "~/Content/less/styles.min.css",

                //query-builder
                "~/Content/vendor/querybuilder/query-builder.default.css"
            });

            RegisterScriptBundle(bundles, "BaseVendor", new[]
            {
                // MODERNIZR
                "~/Content/vendor/modernizr/modernizr-custom.js",
                // JQUERY
                "~/Content/vendor/jquery/jquery-2.2.0.min.js",
                "~/Content/vendor/jquery-migrate/jquery-migrate-1.3.0.js",
                // JQUERY_PLUGINS
                "~/Content/vendor/jquery-mousewheel/jquery.mousewheel.min.js",
                "~/Content/vendor/jquery.scrollTo/jquery.scrollTo.js",
                // BOOTSTRAP
                //"~/Content/vendor/bootstrap/js/bootstrap.js",
                //"~/Content/vendor/bootstrap-switch/js/bootstrap-switch.js",
                // TOOLTIPSTER
                "~/Content/vendor/tooltipster/js/tooltipster.bundle.js",
                // COOKIES
                "~/Content/vendor/js-cookie/js.cookie.js",
                // PERFECT-SCROLLBAR
                "~/Content/vendor/perfect-scrollbar/js/perfect-scrollbar.jquery.js",
                //FIXEDSTICKY+FIXEDFIXED
                "~/Content/vendor/fixedsticky-fixedfixed/fixedfixed.js",
                "~/Content/vendor/fixedsticky-fixedfixed/fixedsticky.js",
                // ES6
                "~/Content/vendor/core-js/shim.js",
                // KENDO
                "~/Content/vendor/jszip/jszip.js",
                "~/Content/vendor/kendo/js/kendo.all.min.js",
                "~/Content/vendor/kendo/js/telerikReportViewer-11.1.17.503.min.js",
                "~/Content/vendor/kendo/js/kendo.aspnetmvc.min.js",
                "~/Content/vendor/kendo/js/kendo.culture.ru.custom.js",
                "~/Content/vendor/kendo/js/kendo.override.js",
                // LEAFLET
                "~/Content/vendor/leaflet/Leaflet.core/leaflet-src.js",
                "~/Content/vendor/leaflet/Leaflet.draw/leaflet.draw-src.js",
                "~/Content/vendor/leaflet/Leaflet.markercluster/leaflet.markercluster-src.js",
                "~/Content/vendor/leaflet/Wicket/wicket.js",
                "~/Content/vendor/leaflet/Wicket/wicket-leaflet.js",
                "~/Content/vendor/leaflet/ext/leaflet.draw.ru.js",
                "~/Content/vendor/leaflet/ext/leaflet.extensions.js",
                // ADDONS
                "~/Content/vendor/jsPlumb/jquery.jsPlumb-1.5.5.js",
                "~/Scripts/soundNotifier/sound.notifier.js",
                "~/Content/vendor/favico/favico.js",
                "~/Content/vendor/jqvmap/jquery.vmap.js",
                //CROPPIE

                "~/Content/vendor/croppie/croppie.js",
            });

            RegisterScriptBundle(bundles, "Querybuilder", new[]
            {
                "~/Content/vendor/querybuilder/jQuery.extendext.js",
                "~/Content/vendor/querybuilder/doT.js",
                "~/Content/vendor/querybuilder/sql-parser.js",
                "~/Content/vendor/querybuilder/query-builder.js",
                "~/Content/vendor/querybuilder/query-builder.ru.js",
            });

            RegisterScriptBundle(bundles, "Common", new[]
            {
                // ...
                "~/Scripts/common.js",
                "~/Scripts/exportimport.js",
                // API
                "~/Scripts/api/util.js",
                "~/Scripts/api/util.is.js",
                "~/Scripts/api/util.decorators.js",
                "~/Scripts/api/components/mnemonic-counter.js",
                "~/Scripts/api/components/user-state.js",
                "~/Scripts/api/kendo.reporting.js",
                "~/Scripts/api/api.ajax.js",
                "~/Scripts/api/api.external.js",
                "~/Scripts/api/api.notification.js",
                "~/Scripts/api/api.editor.js",
                "~/Scripts/api/api.min.js",
                "~/Scripts/api/api.migrate.js",
                //GRID
                "~/Scripts/grid/grid.filters.js",
                "~/Scripts/grid/grid.templates.js",
                // APPLICATION
                "~/Scripts/application/application.js",
                "~/Scripts/application/spa/adapter.js",
                "~/Scripts/application/spa.js",
                "~/Scripts/application/preview/preview-component.js",
                "~/Scripts/application/preview/preview-pool.js",
                "~/Scripts/application/preview/preview-data-provider.js",
                "~/Scripts/application/preview/preview-dom-binder.js",
                // LAYOUT
                "~/Scripts/layout/index.js",
                "~/Scripts/layout/sidebar.js",
                "~/Scripts/layout/toolbar.js",
                // OTHER
                "~/Scripts/kendo.binders.js",
                "~/Scripts/kendo.widgets.js",
                "~/Scripts/kendo.inject.js",
                "~/Scripts/wraps.js",
                "~/Scripts/plugins/jquery.collapse.js",
                "~/Scripts/plugins/jquery.maskedinput.js",
                "~/Scripts/plugins/jquery.popup.js",
                "~/Scripts/content-editor.js",
                // UI
                "~/Scripts/ui/geobox/geobox.js",
                "~/Content/ui/mapcache/vendor/termlib/termlib.js",
                "~/Content/ui/mapcache/vendor/termlib/termlib_parser.js",
                "~/Content/ui/mapcache/mapcache.js",
                "~/Content/ui/mail/js/kendo.mailclient.js",
                // OTHER
                "~/Scripts/hotkeys/hotkeys.js",

                //Typescript

                //Telephony
                //"~/Scripts/app/application.js",

                //end of Typescript


                //VIS
                "~/Scripts/vis/vis.min.js",
                //LINKS
                "~/Scripts/links/links.js",

                "~/Areas/Account/Content/vendor/Materialize/js/materialize.js",

                //uri template
                "~/Scripts/URI.js",
                "~/Scripts/api/api.proxyclient.js",


                "~/Scripts/api/api.multiedit.js",

                //QUERY-FILTER
                "~/Scripts/querytree/query-builder.overrides.js",
                "~/Scripts/querytree/querytree.js",
                "~/Scripts/querytree/querytree.baseObjectId.js",
                "~/Scripts/querytree/querytree.enum.js",
                "~/Scripts/querytree/querytree.datetime.js",
                "~/Scripts/querytree/querytree.bool.js",
                "~/Scripts/querytree/querytree.userId.js",
                "~/Scripts/querytree/querytree.inAndNotIn.js",
                "~/Scripts/querytree/querytree.aggregateFuncs.js",

                //CorpProp
                "~/Scripts/corpProp/editors.js",
                "~/Scripts/corpProp/reporting.js",
                
                //EUSI
                "~/Scripts/EUSI/api.proxyclient.eusi.js",
                "~/Scripts/EUSI/eusi.editors.js",
            });


            RegisterDirectoryScriptBundle(bundles, "Editors", new[]
            {
                "~/Views/Standart/DetailView/Display",
                "~/Views/Standart/DetailView/Display/Common",
               // "~/Views/Data/DetailView/Display",
                "~/Views/Standart/DetailView/Editor",
                "~/Views/Standart/DetailView/Editor/Common",
                "~/Views/Data/DetailView/Editor",
                "~/Views/Links/DetailView/Editor",
                "~/Views/UI/DetailView/Editor"
            });

            RegisterScriptBundle(bundles, "SignalR", new[]
            {
                "~/Content/vendor/signalr/jquery.signalR.js"
            });

            RegisterScriptBundle(bundles, "Globalapp", new[]
            {
                // WEBRTC
                //"~/Scripts/conference/DetectRTC.js",
                //"~/Scripts/conference/RecordRTC.js",
                //"~/Scripts/conference/RTCMultiConnection-v2.2.5.js",
                // GLOBALAPP
                "~/Scripts/globalapp/socket-events.js",
                "~/Scripts/globalapp/socket-service.js",
                "~/Scripts/globalapp/message-service.js",
                "~/Scripts/globalapp/rtc-service.js",
                "~/Scripts/globalapp/user-state-service.js",
                "~/Scripts/globalapp/user-state-dom-binder.js",
                "~/Scripts/globalapp/mnemonic-counter-service.js",
                "~/Scripts/globalapp/system-service.js"
            });

            RegisterScriptBundle(bundles, "DeferredVendor", new[]
            {
                // ANGULAR
                //"~/Scripts/conference/angular.min.js",
                //"~/Scripts/conference/angular-roundProgress.js",
                //"~/Scripts/conference/angular-animate.min.js",
                //"~/Scripts/conference/angular-file-upload.min.js",
                //"~/Scripts/conference/angular-perfect-scrollbar.js",
                // JQUERY_UI
                "~/Content/vendor/jquery-ui/jquery.ui.core.js",
                "~/Content/vendor/jquery-ui/jquery.ui.widget.js",
                "~/Content/vendor/jquery-ui/jquery.ui.mouse.js",
                "~/Content/vendor/jquery-ui/jquery.ui.draggable.js",
                "~/Content/vendor/jquery-ui/jquery.ui.droppable.js",
                "~/Content/vendor/jquery-ui/jquery.ui.resizable.js",
                "~/Content/vendor/jquery-ui/jquery.ui.sortable.js",
                //"~/Areas/Account/Content/vendor/Materialize/js/materialize.js",
            });

            //RegisterScriptBundle(bundles, "DeferredCommon", new[]
            //{
            //    // CHATAPP
            //    "~/Scripts/conference/app/controllers/main-controller.js",
            //    "~/Scripts/conference/app/controllers/error-controller.js",
            //    "~/Scripts/conference/app/controllers/dialog-controller.js",
            //    "~/Scripts/conference/app/controllers/dialog-actions-controller.js",
            //    "~/Scripts/conference/app/controllers/message-controller.js",
            //    "~/Scripts/conference/app/directives/on-enter-press.js",
            //    "~/Scripts/conference/app/services/app-service.js",
            //    "~/Scripts/conference/app/services/dialog-service.js",
            //    "~/Scripts/conference/app/services/message-service.js",
            //    "~/Scripts/conference/app/services/type-service.js",
            //    "~/Scripts/conference/app/services/user-service.js",
            //    "~/Scripts/conference/app/services/url-service.js",
            //    "~/Scripts/conference/app/services/route-service.js",
            //    "~/Scripts/conference/app/app.js"
            //});

            RegisterStyleBundle(bundles, "Tagsinput", new[]
            {
                "~/Content/vendor/bootstrap-tagsinput/bootstrap-tagsinput.css",
                "~/Content/vendor/bootstrap-tagsinput/bootstrap-tagsinput-typeahead.css"
            });

            RegisterScriptBundle(bundles, "Tagsinput", new[]
            {
                "~/Content/vendor/typeahead/typeahead.jquery.js",
                "~/Content/vendor/bootstrap-tagsinput/bootstrap-tagsinput.js"
            });

            RegisterScriptBundle(bundles, "ForumBundle", new[]
            {
                "~/Content/vendor/jquery.hotkeys/jquery.hotkeys.js",
                "~/Content/vendor/bootstrap-wysiwyg/bootstrap-wysiwyg.js"
            });

            RegisterStyleBundle(bundles, "Account", new[]
            {
                // "~/Areas/Account/Content/vendor/Materialize/css/materialize.css",
                "~/Areas/Account/Content/vendor/font-awesome/css/font-awesome.css",
                "~/Content/fonts/roboto/roboto.css",
                "~/Content/fonts/pt-sans/pt-sans.css",
                // "~/Areas/Account/Content/vendor/mdi/css/materialdesignicons.css",
                //"~/Areas/Account/Content/style.css"
                "~/Areas/Account/Content/style.min.css"
            });

            RegisterScriptBundle(bundles, "Account", new[]
            {
                "~/Areas/Account/Content/vendor/jquery/jquery.js",
                "~/Areas/Account/Content/vendor/jquery-migrate/jquery-migrate.js",
                "~/Areas/Account/Content/vendor/Materialize/js/materialize.js",
                "~/Areas/Account/Content/script.js"
            });

            RegisterStyleBundle(bundles, "Example", new[]
            {
                "~/Content/vendor/highlight/styles/default.css",
                "~/Content/vendor/highlight/styles/monokai-sublime.css",
                "~/Content/less/example.css"
            });

            RegisterScriptBundle(bundles, "Example", new[]
            {
                "~/Content/vendor/highlight/highlight.pack.js"
            });

            RegisterStyleBundle(bundles, "Test", new[]
            {
                "~/Content/vendor/mocha/mocha.css"
            });

            RegisterScriptBundle(bundles, "Test", new[]
            {
                "~/Content/vendor/mocha/mocha.js",
                "~/Content/vendor/chai/chai.js"
            });

            BundleTable.EnableOptimizations = true;
        }

        private static void RegisterStyleBundle(BundleCollection bundleCollection, string bundleName, string[] files)
        {
            var bundle = new StyleBundle($"~/{StyleNamespace}/{bundleName}");

            foreach (var file in files)
            {
                bundle.Include(file, new CssRewriteUrlTransformFixed());
            }

            bundleCollection.Add(bundle);
        }

        private static void RegisterScriptBundle(BundleCollection bundleCollection, string bundleName, string[] files)
        {
            var bundle = new ScriptBundle($"~/{ScriptNamespace}/{bundleName}");

            bundle.Include(files);

            bundleCollection.Add(bundle);
        }

        private static void RegisterScriptBundle(BundleCollection bundleCollection, string bundleName, string directory, bool searchSubDirectories = false)
        {
            var bundle = new ScriptBundle($"~/{ScriptNamespace}/{bundleName}");

            bundle.IncludeDirectory(directory, "*.js", searchSubDirectories);

            bundleCollection.Add(bundle);
        }

        private static void RegisterDirectoryScriptBundle(BundleCollection bundleCollection, string bundleName, string[] directories, bool searchSubDirectories = false)
        {
            var bundle = new ScriptBundle($"~/{ScriptNamespace}/{bundleName}");

            foreach (string directory in directories)
            {
                bundle.IncludeDirectory(directory, "*.js", searchSubDirectories);
            }

            bundleCollection.Add(bundle);
        }
    }

}