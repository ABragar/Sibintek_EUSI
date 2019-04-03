using Base.UI;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Base.Utils.Common;

namespace WebUI.Helpers
{
    public static class DashboardHelper
    {
        public static HtmlDashboardWidgetBuilder DashboardWidget(this HtmlHelper htmlHelper)
        {
            return new HtmlDashboardWidgetBuilder(htmlHelper, htmlHelper.ViewData.Model as DashboardWidget);
        }

        public static HtmlDashboardWidgetBuilder DashboardWidget(this HtmlHelper htmlhelper, DashboardWidget model)
        {
            return new HtmlDashboardWidgetBuilder(htmlhelper, model);
        }
    }

    public class HtmlDashboardWidgetBuilder : IHtmlString
    {
        private readonly HtmlHelper _htmlHelper;
        private readonly HtmlDashboardWidget _model;

        public HtmlDashboardWidgetBuilder(HtmlHelper htmlHelper, DashboardWidget model)
        {
            _htmlHelper = htmlHelper;
            _model = ObjectHelper.CreateAndCopyObject<HtmlDashboardWidget>(model);
        }

        public HtmlDashboardWidgetBuilder Content(Func<object, object> value)
        {
            var result = value(null);

            if (result != null)
                _model.Html = result.ToString();

            return this;
        }

        public HtmlDashboardWidgetBuilder HtmlAttrs(object attrs)
        {
            _model.HtmlAttributes = attrs;

            return this;
        }
 
        public HtmlDashboardWidgetBuilder Content(string value)
        {
            _model.Html = value;
            return this;
        }

        public string ToHtmlString()
        {
            return _htmlHelper.Partial("~/Views/Dashboard/Widgets/Base.cshtml", _model).ToHtmlString();
        }
    }

    public class HtmlDashboardWidget : DashboardWidget
    {
        public string Html { get; set; }
        public object HtmlAttributes { get; set; }
    }
}