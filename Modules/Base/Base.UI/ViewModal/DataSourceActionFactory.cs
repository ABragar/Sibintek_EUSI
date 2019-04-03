using System.Collections.Generic;
using Base.UI.ViewModal;

namespace Base.UI
{
    public class DataSourceActionFactory<T> where T : class
    {
        private readonly AjaxAction _action;

        public DataSourceActionFactory(AjaxAction action)
        {
            _action = action;
        }

        public DataSourceActionFactory<T> Action(string name)
        {
            _action.Name = name;
            return this;
        }

        public DataSourceActionFactory<T> Controller(string name)
        {
            _action.Controller = name;
            return this;
        }

        public DataSourceActionFactory<T> Params(Dictionary<string, string> name)
        {
            _action.Params = name;
            return this;
        }
    }
}