using System.Collections.Generic;
using Base.UI.ViewModal;

namespace Base.UI
{
    public class ListViewCondApprearanceFactory<T> where T : class
    {
        private List<ConditionalAppearance> _conds;

        public ListViewCondApprearanceFactory(List<ConditionalAppearance> conds)
        {
            _conds = conds;
        }


        public ListViewCondApprearanceFactory<T> Add(string condition, string backgound)
        {
            var appearance = new ConditionalAppearance { Backgound = backgound, Condition = condition };
            _conds.Add(appearance);
            return this;
        }
    }
}