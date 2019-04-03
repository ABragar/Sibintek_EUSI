using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using Base.DAL;
using Base.Service.Crud;

namespace Base.UI.ViewModal
{
    public class DetailView: View
    {
        public DetailViewType Type { get; set; } = DetailViewType.DetailView;
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public bool IsMaximaze { get; set; }
        public bool HideToolbar { get; set; }
        public DataSource DataSource { get; set; }
        public List<EditorViewModel> Editors { get; set; } = new List<EditorViewModel>();
        public List<Toolbar> Toolbars { get; set; } = new List<Toolbar>();
        public string WizardName { get; set; }
        public bool HideTabs { get; set; }
        public override IEnumerable<PropertyViewModel> Props => Editors.Where(x => x.PropertyName != null);
        public Action<IUnitOfWork, object, CommonEditorViewModel> DefaultSetting { get; set; }
        public Func<IUnitOfWork, object, object> CustomSelect { get; set; }

        #region Sib
        /// <summary>
        /// DV всегда доступен для редактирования.
        /// </summary>
        /// <remarks>Пока используется только при открытии DV в Display редакторе навигационного св-ва.</remarks>
        public bool? AlwaysEdit { get; set; }
        #endregion


        public object GetData(IUnitOfWork uofw, IQueryable q)
        {
            if (CustomSelect != null)
            {
                var obj = CustomSelect(uofw, q.Single());
                //TODO: >> IsDynamicProxy()
                return obj.GetType().Namespace == "System.Data.Entity.DynamicProxies" ? SelectObj(obj) : obj;
            }
            else
            {
                q = Select(q);
            }

            return q.Single();
        }

        public object GetDataOrDefault(IUnitOfWork uofw, IQueryable q)
        {
            if (CustomSelect != null)
            {
                var obj = CustomSelect(uofw, q.SingleOrDefault());
                //TODO: >> IsDynamicProxy()
                return obj.GetType().Namespace == "System.Data.Entity.DynamicProxies" ? SelectObj(obj) : obj;
            }
            else
            {
                q = Select(q);
            }

            return q.SingleOrDefault();
        }


        public object GetData(IUnitOfWork uofw, IBaseObjectCrudService serv, int id)
        {
            IQueryable q = null;
            q = serv.GetById(uofw, id);
            return GetData(uofw, q);
        }

        public object GetData(IUnitOfWork uofw, IBaseObjectCrudService serv, string code = null)
        {
            IQueryable q = null;
            if (!string.IsNullOrEmpty(code))
                q = serv.GetAll(uofw, null).Where($"it.Code == @0", code);

            if (CustomSelect != null)
            {
                var obj = CustomSelect(uofw, q.Single());
                //TODO: >> IsDynamicProxy()
                return obj.GetType().Namespace == "System.Data.Entity.DynamicProxies" ? SelectObj(obj) : obj;
            }
            else
            {
                q = Select(q);
            }

            return q.Single();
        }

        public object GetData(IUnitOfWork uofw, IBaseObjectCrudService serv, string childKey, int childId)
        {
            IQueryable q = null;
            q = serv.GetAll(uofw).Where($"{childKey}=@0", new[] { childId });

            if (CustomSelect != null)
            {
                var obj = CustomSelect(uofw, q.Single());
                //TODO: >> IsDynamicProxy()
                return obj.GetType().Namespace == "System.Data.Entity.DynamicProxies" ? SelectObj(obj) : obj;
            }
            else
            {
                q = Select(q);
            }

            return q.Single();
        }

        public override T Copy<T>(T view = null)
        {
            var dv = view as DetailView ?? new DetailView();

            dv.Type = this.Type;
            dv.Title = this.Title;
            dv.Description = this.Description;
            dv.Width = this.Width;
            dv.Height = this.Height;
            dv.IsMaximaze = this.IsMaximaze;
            dv.HideToolbar = this.HideToolbar;
            dv.DataSource = this.DataSource;
            dv.WizardName = this.WizardName;
            dv.HideTabs = this.HideTabs;
            dv.CustomSelect = this.CustomSelect;
            dv.DefaultSetting = this.DefaultSetting;
            dv.AlwaysEdit = this.AlwaysEdit;

            foreach (var editor in this.Editors)
            {
                dv.Editors.Add(editor.Copy<EditorViewModel>());
            }

            foreach (var toolbar in this.Toolbars)
            {
                dv.Toolbars.Add(toolbar.Copy());
            }

            return base.Copy(dv as T);
        }
    }

    public enum DetailViewType
    {
        DetailView = 0,
        WizzardView = 1,
    }
}