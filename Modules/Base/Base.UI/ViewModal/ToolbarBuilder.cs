using System;
using Base.UI.ViewModal;

namespace Base.UI
{
    public class ToolbarBuilder<T> where T : class
    {
        private Toolbar toolbar;

        public ToolbarBuilder(Toolbar toolbar)
        {
            this.toolbar = toolbar;
        }

        public ToolbarBuilder<T> Title(string title)
        {
            toolbar.Title = title;
            return this;
        }

        public ToolbarBuilder<T> Area(string area)
        {
            toolbar.AjaxAction.Area = area;
            return this;
        }

        public ToolbarBuilder<T> IsAjax(bool isAjax)
        {
            toolbar.IsAjax = isAjax;
            return this;
        }

        public ToolbarBuilder<T> Url(string url)
        {
            toolbar.Url = url;
            return this;
        }

        public ToolbarBuilder<T> IsMaximze(bool isMaximize)
        {
            toolbar.IsMaximize = isMaximize;
            return this;
        }

        public ToolbarBuilder<T> OnlyForSelect(bool onlyForSelect)
        {
            toolbar.OnlyForSelected = onlyForSelect;
            return this;
        }

        public ToolbarBuilder<T> ListParams(Action<ToolbarParamsBuilder<T>> action)
        {
            action(new ToolbarParamsBuilder<T>(toolbar.AjaxAction.Params));
            return this;
        }
    }
}