using System;
using Base.UI.ViewModal;

namespace Base.UI
{
    public class AjaxFormBuilder<T> where T : class
    {
        private AjaxAction ajaxFormAction;
        public AjaxFormBuilder(AjaxAction ajaxFormAction)
        {
            this.ajaxFormAction = ajaxFormAction;
        }
        public AjaxFormBuilder<T> Params(Action<AjaxFormParamsBuilder<T>> action)
        {
            action(new AjaxFormParamsBuilder<T>(ajaxFormAction.Params));
            return this;
        }
    }
}