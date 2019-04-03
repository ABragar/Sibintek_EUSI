using System.Collections.Generic;
using System.Linq;

namespace Base.UI.ViewModal
{
    public class ListView : View
    {
        public ListView()
        {
            this.AutoRefreshInterval = -1;
            Sortable = true;
            Scrollable = true;
            HiddenActions = new List<ListViewAction>();
            Columns = new List<ColumnViewModel>();
            Toolbars = new List<Toolbar>();
            DataSource = new DataSource();
            ConditionalAppearance = new List<ConditionalAppearance>();
        }

        public ListViewType Type { get; set; }
        public string Title { get; set; }
        public bool Sortable { get; set; }
        public bool Scrollable { get; set; }
        public DataSource DataSource { get; set; }
        public bool HideToolbar { get; set; }
        public int AutoRefreshInterval { get; set; }
        public bool AllowEmptyAction { get; set; }
        public bool MultiSelect { get; set; }
        public List<ColumnViewModel> Columns { get; set; }
        public List<Toolbar> Toolbars { get; set; }
        public List<ConditionalAppearance> ConditionalAppearance { get; set; }
        public List<ListViewAction> HiddenActions { get; set; }

        public bool VisibleAction(LvAction action)
        {
            if (HiddenActions != null && HiddenActions.Any())
            {
                return HiddenActions.All(m => m.Value != action);
            }

            return true;
        }

        public override IEnumerable<PropertyViewModel> Props => Columns;
        public bool MultiEdit { get; set; }

        /// <summary>
        /// Текст скрипта при открытии строки ListView на редактирование - grid.editRow
        /// </summary>
        public string OnClientEditRow { get; set; }
        

        public override T Copy<T>(T view = null)
        {
            var lv = view as ListView ?? new ListView();

            lv.Type = Type;
            lv.Title = Title;
            lv.Sortable = Sortable;
            lv.Scrollable = Scrollable;
            lv.DataSource = DataSource?.Copy();
            lv.HideToolbar = HideToolbar;
            lv.AutoRefreshInterval = AutoRefreshInterval;
            lv.AllowEmptyAction = AllowEmptyAction;
            lv.MultiSelect = MultiSelect;
            lv.MultiEdit = MultiEdit;
            lv.OnClientEditRow = OnClientEditRow;            

            foreach (var column in this.Columns)
            {
                lv.Columns.Add(column.Copy<ColumnViewModel>());
            }

            foreach (var toolbar in this.Toolbars)
            {
                lv.Toolbars.Add(toolbar.Copy());
            }

            foreach (var conditionalAppearance in ConditionalAppearance)
            {
                lv.ConditionalAppearance.Add(conditionalAppearance.Copy());
            }

            foreach (var hiddenAction in HiddenActions)
            {
                lv.HiddenActions.Add(hiddenAction.Copy());
            }

            return base.Copy(lv as T);
        }
                
    }
}