using System;
using System.Collections.Generic;
using System.Linq;
using Base.Extensions;

namespace Base.UI.Helpers
{
    public static class ViewModelConfigHelper
    {
        public static Dictionary<string, string> GetEditorParams(string parameters)
        {
            var _parameters = new Dictionary<string, string>();

            if (parameters == null) return _parameters;

            parameters.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ForEach(param =>
            {
                var values = param.Split('=');

                _parameters.Add(values[0], values.Length > 1 ? values[1] : "true");
            });

            return _parameters;
        }

        public static T GetAttribute<T>(this IEnumerable<Attribute> attributes) where T: Attribute
        {
            return (T)attributes.FirstOrDefault(x => x.GetType() == typeof (T));
        }
    }
}