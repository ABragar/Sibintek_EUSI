using System;
using Base.UI.ViewModal;
using System.Collections.Generic;
using System.Linq.Expressions;
using Base.DAL;

namespace Base.UI
{
    public class DetailViewBuilder<T> where T : class
    {
        private readonly DetailView _detailView;

        public DetailViewBuilder(DetailView detailView)
        {
            _detailView = detailView;
        }

        public DetailViewBuilder<T> Title(string title)
        {
            _detailView.Title = title;
            return this;
        }

        public DetailViewBuilder<T> Editors(Action<EditorsFactory<T>> configurator)
        {
            configurator(new EditorsFactory<T>(_detailView.Editors));
            return this;
        }

        public DetailViewBuilder<T> Toolbar(Action<ToolbarBuilderFactory<T>> factory)
        {
            factory(new ToolbarBuilderFactory<T>(_detailView.Toolbars));

            return this;
        }

        public DetailViewBuilder<T> Width(int width)
        {
            _detailView.Width = width;
            return this;
        }

        public DetailViewBuilder<T> Height(int height)
        {
            _detailView.Height = height;
            return this;
        }

        public DetailViewBuilder<T> IsMaximized(bool ismax)
        {
            _detailView.IsMaximaze = ismax;
            return this;
        }

        public DetailViewBuilder<T> HideToolbar(bool hideToolbar)
        {
            _detailView.HideToolbar = hideToolbar;
            return this;
        }

        public DetailViewBuilder<T> Description(string message)
        {
            _detailView.Description = message;
            return this;
        }



        public DetailViewBuilder<T> Wizard(string wizzardName)
        {
            _detailView.WizardName = wizzardName;
            return this;
        }

        public DetailViewBuilder<T> DefaultSettings(Action<IUnitOfWork, T, CommonEditorVmSett<T>> action)
        {
            _detailView.DefaultSetting = (uow, o, e) => action(uow, (T)o, e.GetVmSett<T>());
            return this;
        }

        public DetailViewBuilder<T> Select(Func<IUnitOfWork, T, T> func)
        {
            _detailView.CustomSelect = (x, y) => func(x, (T)y);
            return this;
        }

        #region Sib
        /// <summary>
        /// DV всегда доступен для редактирования.
        /// </summary>
        /// <remarks>Пока используется только при открытии DV в Display редакторе навигационного св-ва.</remarks>
        public DetailViewBuilder<T> AlwaysEdit(bool val = false)
        {
            _detailView.AlwaysEdit = val;
            return this;
        }
        #endregion
    }

}