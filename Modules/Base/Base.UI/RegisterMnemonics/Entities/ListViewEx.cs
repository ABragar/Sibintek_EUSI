using Base.Attributes;
using Base.UI.ViewModal;
using System.Collections.Generic;
using System.Linq;

namespace Base.UI.RegisterMnemonics.Entities
{
    public class ListViewEx : MnemonicEx
    {
        [DetailView("Групповое редактирование")]
        public bool? MultiEdit { get; set; }

        [DetailView("Колонки")]
        public virtual ICollection<ColumnEx> Columns { get; set; }

        private IDictionary<string, ColumnEx> _columns = null;

        public override void Visit(ConfigListView configListView)
        {
            if (MultiEdit.HasValue)
                configListView.MultiEdit = MultiEdit.Value;
        }

        public override void Visit(ConfigColumn configColumn)
        {
            if (configColumn.PropertyName == null) return;

            if (_columns == null)
                _columns = Columns?.ToDictionary(x => x.PropertyName, x => x);

            if (_columns == null || !_columns.ContainsKey(configColumn.PropertyName)) return;

            var columnEx = _columns[configColumn.PropertyName];

            configColumn.Title = columnEx.Title;
            configColumn.Visible = columnEx.Visible;
            configColumn.OneLine = columnEx.OneLine;

            if (columnEx.Order != null)
                configColumn.SortOrder = columnEx.Order.Value;
        }
    }
}