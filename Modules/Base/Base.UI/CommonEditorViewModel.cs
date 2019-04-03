using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using Base.UI.ViewModal;
using Base.Utils.Common;
using Base.Ambient;

namespace Base.UI
{
    public class CommonEditorViewModel
    {
        private List<TabVm> _tabs;

        private List<GroupVm> _groups;

        public CommonEditorViewModel()
        {

        }

        public CommonEditorViewModel(ViewModelConfig viewModelConfig, IEnumerable<EditorViewModel> editors, bool isAjax, bool copy = true)
        {
            IsAjax = isAjax;

            ViewModelConfig = viewModelConfig;

            Editors = new List<EditorViewModel>();

            if (editors == null) return;

            foreach (
                var editor in editors.Select(x => x.Copy<EditorViewModel>()))
            {
                if (!editor.IsReadOnly)
                editor.IsReadOnly = 
                    !Ambient.AppContext.SecurityUser.PropertyCanWrite(viewModelConfig.TypeEntity, editor.PropertyName);

                editor.CanRead =
                    Ambient.AppContext.SecurityUser.PropertyCanRead(viewModelConfig.TypeEntity, editor.PropertyName);

                Editors.Add(editor);
            }
        }

        public bool IsAjax { get; private set; }

        public ViewModelConfig ViewModelConfig { get; private set; }

        public List<EditorViewModel> Editors { get; private set; }

        public List<TabVm> Tabs
        {
            get { return _tabs ?? (_tabs = TabVm.GetTabs(Editors.OrderBy(s=>s.SortOrder).Where(e => e.Visible))); }
        }

        public List<GroupVm> Groups
        {
            get { return _groups ?? (_groups = GroupVm.GetGroups(Editors.OrderBy(s => s.SortOrder).Where(e => e.Visible))); }
        }


        public CommonEditorVmSett<T> GetVmSett<T>() where T : class
        {
            return new CommonEditorVmSett<T>(this);
        }
    }

    public class TabVm
    {
        private static int _increment;


        public static List<TabVm> GetTabs(IEnumerable<EditorViewModel> editors)
        {
            return editors
                .GroupBy(x => x.TabName)
                .OrderBy(x => x.Key)
                .Select(x => new TabVm(x.Key, editors: x))
                .ToList();

        }

        public TabVm(string tabName, IGrouping<string, EditorViewModel> editors)
        {
            TabID = $"tab_{unchecked((uint)Interlocked.Increment(ref _increment))}"; ;
            TabName = tabName;
            Editors = editors;
        }

        public string TabID { get; }
        public string TabName { get; private set; }
        public IGrouping<string, EditorViewModel> Editors { get; private set; }
    }




    public class GroupVm
    {
        private static int _increment;

        public static List<GroupVm> GetGroups(IEnumerable<EditorViewModel> editors)
        {          
            var editorGroupInTabs = editors.OrderBy(x=>x.SortOrder).GroupBy(x => x.TabName);
            Dictionary<string, int> orderHorizontalGroup = new Dictionary<string, int>();
            int orderGroup = 0;

            foreach (var groupInTab in editorGroupInTabs)
            {
                foreach (var editorHorizont in groupInTab)
                {
                    if (editorHorizont.Group != null && !orderHorizontalGroup.ContainsKey(editorHorizont.Group))
                    {
                        editorHorizont.Group = editorHorizont.Group;
                        editorHorizont.GroupOrder = orderGroup;
                        orderHorizontalGroup.Add(editorHorizont.Group, orderGroup);
                    }
                    if (editorHorizont.Group != null && orderHorizontalGroup.ContainsKey(editorHorizont.Group))
                    {
                        editorHorizont.Group = editorHorizont.Group;
                        editorHorizont.GroupOrder = orderHorizontalGroup[editorHorizont.Group];
                    }
                    if (editorHorizont.Group == null)
                    {
                        orderHorizontalGroup.Add(Guid.NewGuid().ToString(), (int)editorHorizont.SortOrder);
                    }
                    orderGroup++;
                }
                int newOrder = orderHorizontalGroup.Values.Min();

                foreach (var editor in groupInTab)
                {
                    while (orderHorizontalGroup.ContainsValue(newOrder))
                    {
                        newOrder++;
                    }
                    if (editor.Group == null && editor.SortOrder < -1)
                    {
                        editor.GroupOrder = (int)editor.SortOrder;
                        continue;
                    }
                    if (editor.Group == null && editor.SortOrder > 0)
                    {
                        editor.GroupOrder = (int)editor.SortOrder;
                        continue;
                    }
                    if (editor.Group == null)
                    {
                        editor.GroupOrder = newOrder;
                        newOrder++;
                    }
                }
            }
            return editors
                .GroupBy(x => x.GroupOrder)
                .OrderBy(x => x.Key)
                .Select(x => new GroupVm(x.Key, editors: x)).ToList();
        }

        public GroupVm(int groupOrder, IGrouping<int, EditorViewModel> editors)
        {
            GroupID = $"group_{unchecked((uint)Interlocked.Increment(ref _increment))}"; ;
            GroupName = groupOrder;
            Editors = editors;

        }

        public string GroupID { get; }

        public int GroupName { get; private set; }


        public IGrouping<int, EditorViewModel> Editors { get; private set; }
    }



    public class CommonEditorVmSett<T>
    {
        private readonly CommonEditorViewModel _commonEditorViewModel;

        public CommonEditorVmSett(CommonEditorViewModel commonEditorViewModel)
        {
            _commonEditorViewModel = commonEditorViewModel;
        }

        public CommonEditorVmSett<T> Visible<TValue>(Expression<Func<T, TValue>> property, bool val)
        {
            var propertyExpression = property.Body as MemberExpression;

            if (propertyExpression == null)
                throw new Exception("propertyExpression");

            var propertyInfo = propertyExpression.Member as PropertyInfo;

            var editor = _commonEditorViewModel.Editors.FirstOrDefault(x => x.PropertyName == propertyInfo.Name);
            if (editor != null)
                editor.Visible = val;

            return this;
        }

        public CommonEditorVmSett<T> Visible(string property, bool val)
        {
            var editor = _commonEditorViewModel.Editors.SingleOrDefault(x => x.PropertyName == property) ??
                _commonEditorViewModel.Editors.SingleOrDefault(x => x.SysName == property);

            if (editor == null)
                throw new Exception($"property {property} not found");

            editor.Visible = val;

            return this;
        }

        public CommonEditorVmSett<T> Title<TValue>(Expression<Func<T, TValue>> property, string val)
        {
            var propertyExpression = property.Body as MemberExpression;

            if (propertyExpression == null)
                throw new Exception("propertyExpression");

            var propertyInfo = propertyExpression.Member as PropertyInfo;

            var editor = _commonEditorViewModel.Editors.FirstOrDefault(x => x.PropertyName == propertyInfo.Name);

            editor.Title = val;

            return this;
        }

        /// <summary>
        /// Устанавливает для свойства возможность только чтения
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="property"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public CommonEditorVmSett<T> ReadOnly<TValue>(Expression<Func<T, TValue>> property, bool val = true)
        {
            var propertyExpression = property.Body as MemberExpression;

            if (propertyExpression == null)
                throw new Exception("propertyExpression");

            var propertyInfo = propertyExpression.Member as PropertyInfo;

            var editor = _commonEditorViewModel.Editors.FirstOrDefault(x => x.PropertyName == propertyInfo.Name);
            if (editor != null)
                editor.IsReadOnly = val;

            return this;
        }

        public CommonEditorVmSett<T> ReadOnlyByMnemonic(string mnemonic, bool val = true)
        {
            var editor = _commonEditorViewModel.Editors.FirstOrDefault(x => x.Mnemonic == mnemonic);

            editor.IsReadOnly = val;

            return this;
        }

        public CommonEditorVmSett<T> Required<TValue>(Expression<Func<T, TValue>> property, bool val = true)
        {
            var propertyExpression = property.Body as MemberExpression;

            if (propertyExpression == null)
                throw new Exception("propertyExpression");

            var propertyInfo = propertyExpression.Member as PropertyInfo;

            var editor = _commonEditorViewModel.Editors.FirstOrDefault(x => x.PropertyName == propertyInfo.Name);
            if (editor != null)
            editor.IsRequired = val;

            return this;
        }

        public CommonEditorVmSett<T> SetReadOnlyAll(bool val = true)
        {
            _commonEditorViewModel.Editors.ForEach(x => x.IsReadOnly = val);

            return this;
        }

        #region sib 
        //sib

        public CommonEditorVmSett<T> ChangeEditor(
           string propertyName
           , bool required
           , bool readOnly
           , bool visible 
           , string paramsKey = null
           , string paramsValue = null
           , Func<bool> condition = null
           )
        {
            if (String.IsNullOrEmpty(propertyName))
                return this;

            var editor = _commonEditorViewModel.Editors.FirstOrDefault(x => x.PropertyName == propertyName || x.SysName == propertyName);
            if (condition == null && editor != null)
            {
                editor.Visible = visible;
                editor.IsRequired = required;
                editor.IsReadOnly = readOnly;
            }
            else
            {
                if (editor == null && condition != null && condition())
                {
                    var prop = typeof(T).GetProperties().Where(p => p.Name == propertyName).FirstOrDefault();
                    editor = Service.ViewModelConfigFactory.CreateEditor(prop.DeclaringType, prop);
                    _commonEditorViewModel.Editors.Add(editor);                   
                }
                if (editor != null)
                {
                    if (editor.ParentViewModelConfig == null)
                        editor.ParentViewModelConfig = this._commonEditorViewModel.ViewModelConfig;
                    editor.IsRequired = required;
                    editor.IsReadOnly = readOnly;
                    editor.Visible = (condition != null && !condition()) ? false: visible;
                    if (!String.IsNullOrEmpty(paramsKey))
                    {
                        if (!editor.Params.ContainsKey(paramsKey))
                            editor.Params.Add(paramsKey, paramsValue);
                        else
                            editor.Params[paramsKey] = paramsValue;
                    }
                }

            }

            return this;
        }

     
        #endregion//end sib
    }
}