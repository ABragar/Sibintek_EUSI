using Base.UI.ViewModal;
using System;
using System.Linq;
using System.Reflection;

namespace Base.UI
{
    public class ListViewBuilder<T> where T : class
    {
        private readonly ListView _listView;
        private readonly IInitializerContext _context;

        public ListViewBuilder(ListView listView, IInitializerContext context)
        {
            _listView = listView;
            _context = context;
        }

        public ListViewBuilder<T> Title(string title)
        {
            _listView.Title = title;
            return this;
        }

        public ListViewBuilder<T> Type(ListViewType type)
        {
            _listView.Type = type;
            return this;
        }

        public ListViewBuilder<T> HiddenActions(LvAction[] actions)
        {
            if (actions == null) return this;

            _listView.HiddenActions = actions.Select(x => new ListViewAction() {Value = x}).ToList();

            return this;
        }

        public ListViewBuilder<T> Toolbar(Action<ToolbarBuilderFactory<T>> factory)
        {
            factory(new ToolbarBuilderFactory<T>(_listView.Toolbars));
            return this;
        }

        public ListViewBuilder<T> DataSource(Action<DataSourceBuilder<T>> configurator)
        {
            configurator(new DataSourceBuilder<T>(_listView.DataSource));
            return this;
        }

        public ListViewBuilder<T> TreeView(Action<TreeViewBuilder<T>> configurator)
        {
            configurator(new TreeViewBuilder<T>(_listView as TreeView, _context));

            return this;
        }

        public ListViewBuilder<T> ListViewCategorizedItem(Action<ListViewCategorizedItemBuilder<T>> configurator)
        {
            configurator(new ListViewCategorizedItemBuilder<T>(_listView as ListViewCategorizedItem, _context));
            return this;
        }

        public ListViewBuilder<T> Columns(Action<ListViewColumnFactory<T>> configurator)
        {
            configurator(new ListViewColumnFactory<T>(_listView.Columns));
            return this;
        }

        public ListViewBuilder<T> ConditionAppearence(Action<ListViewCondApprearanceFactory<T>> action)
        {
            action(new ListViewCondApprearanceFactory<T>(_listView.ConditionalAppearance));
            return this;
        }

        public ListViewBuilder<T> AutoRefreshInterval(int interval)
        {
            _listView.AutoRefreshInterval = interval;
            return this;
        }

        public ListViewBuilder<T> Sortable(bool sortable)
        {
            _listView.Sortable = sortable;
            return this;
        }

        public ListViewBuilder<T> IsMultiSelect(bool multiSelect)
        {
            _listView.MultiSelect = multiSelect;
            return this;
        }

        public ListViewBuilder<T> IsMultiEdit(bool multiEdit)
        {
            _listView.MultiEdit = multiEdit;
            return this;
        }

        /// <summary>
        /// Текст скрипта при открытии строки ListView на редактирование - grid.editRow
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        public ListViewBuilder<T> OnClientEditRow(string script)
        {
            _listView.OnClientEditRow = script;
            return this;
        }

        /// <summary>
        /// Добавление колонок в реестр из конфигурации мнемоники <see cref="childMnemonic" />.
        /// </summary>
        /// <remarks>
        /// Должна быть обеспечена ссылочная целостность связи
        /// </remarks>
        /// <typeparam name="T">Тип интегрируемой модели</typeparam>
        /// <param name="childMnemonic">Мнемоника конфигурации интегрируемой модели</param>
        /// <param name="parentProperty">Свойство основной модели для связи с интегрируемой
        /// моделью</param>
        /// <param name="childProperty">Свойство интегрируемой модели для связи с основной
        /// моделью</param>
        /// <param name="linkType"></param>
        public ListViewBuilder<T> ColumnsFrom<T1>(string childMnemonic, string parentProperty, string childProperty,
            string linkType = "=") //, Action<ListViewColumnFactory<T1>> configurator = null) 
            where T1 : class
        {
            if (string.IsNullOrEmpty(parentProperty))
                throw new ArgumentException("Argument must not be the empty string.", nameof(parentProperty));
            if (string.IsNullOrEmpty(childProperty))
                throw new ArgumentException("Argument must not be the empty string.", nameof(childProperty));

            var config = _context.GetVmConfig<T1>(childMnemonic);
            childMnemonic = config.Mnemonic;

            var extraColumns = config.ListView.Columns
                .Where(x => x.PropertyName != "ID")
                .Select(x =>
                {
                    var columnCopy = x.Copy<ColumnViewModel>();
                    columnCopy.ChildMnemonic = childMnemonic;
                    columnCopy.ChildMnemonicType = config.TypeEntity;
                    columnCopy.ParentProperty = parentProperty;
                    columnCopy.ChildProperty = childProperty;
                    columnCopy.PropertyName = $"{config.TypeEntity.Name}_{x.PropertyName}";
                    return columnCopy;
                }).ToList();
            _listView.Columns.AddRange(extraColumns);

            return this;
        }
    }
}