using System.Collections.Generic;

namespace Base.UI
{
    public class ToolbarParamsBuilder<T> where T : class
    {
        private Dictionary<string, string> toolbarParams;

        public ToolbarParamsBuilder(Dictionary<string, string> toolbarDictionary)
        {
            toolbarParams = toolbarDictionary;
        }

        public ToolbarParamsBuilder<T> Add(string key, string value)
        {
            toolbarParams.Add(key, value);
            return this;
        }

    }
}