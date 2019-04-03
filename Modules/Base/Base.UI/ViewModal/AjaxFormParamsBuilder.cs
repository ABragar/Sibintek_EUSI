using System.Collections.Generic;

namespace Base.UI
{
    public class AjaxFormParamsBuilder<T> where T : class
    {
        private Dictionary<string, string> Params;
        public AjaxFormParamsBuilder(Dictionary<string, string> dictionary)
        {
            Params = dictionary;
        }

        public AjaxFormParamsBuilder<T> Add(string key, string value)
        {
            Params.Add(key, value);
            return this;
        }
    }
}