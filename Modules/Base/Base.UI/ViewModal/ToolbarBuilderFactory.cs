using System;
using System.Collections.Generic;
using Base.UI.ViewModal;

namespace Base.UI
{
    public class ToolbarBuilderFactory<T> where T : class
    {
        private List<Toolbar> toolbars;

        public ToolbarBuilderFactory(List<Toolbar> toolbars)
        {
            this.toolbars = toolbars;
        }

        public ToolbarBuilderFactory<T> Add(string action, string controller, Action<ToolbarBuilder<T>> tbuilderAction = null)
        {
            Toolbar t = new Toolbar();
            AjaxAction ajaxAction = new AjaxAction { Name = action, Controller = controller };
            t.AjaxAction = ajaxAction;
            toolbars.Add(t);
            if (tbuilderAction != null)
                tbuilderAction(new ToolbarBuilder<T>(t));
            return this;
        }

    }
}