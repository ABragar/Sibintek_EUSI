using Base.Attributes;
using System;
using System.Collections.Generic;

namespace Base.UI.ViewModal
{
    [Serializable]
    public class AjaxAction
    {
        public AjaxAction()
        {
            Params = new Dictionary<string, string>();
        }
        public string Name { get; set; }
        public string Controller { get; set; }
        public string Area { get; set; }
        public Dictionary<string,string> Params { get; set; }
    }
}