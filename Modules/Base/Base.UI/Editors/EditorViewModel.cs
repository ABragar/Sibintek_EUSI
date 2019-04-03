using System;
using System.Collections.Generic;
using System.Linq;

namespace Base.UI
{
    public class EditorViewModel : PropertyViewModel
    {
        public const string ParentKey = "ParentKey";
        public const string ChildKey = "ChildKey";
        private string _tabName;

        public EditorViewModel() : base()
        {
        }

        public EditorViewModel(string name) : base(name)
        {
        }

        public bool IsReadOnly { get; set; }
        public bool IsRequired { get; set; }
        public bool IsLabelVisible { get; set; } = true;

        public string TabName
        {
            get { return _tabName ?? "[0]Основные данные"; }
            set { _tabName = value; }
        }

        public string Group { get; set; }

        public int GroupOrder { get; set; }

        public bool IsHorizontal { get; set; }
        public string WizardName { get; set; }
        public CascadeFrom CascadeFrom { get; set; }
        public Type EditorType => ViewModelType;

        public string BgColor { get; set; }

        public string EditorTemplate
        {
            get { return PropertyDataTypeName; }
            set { PropertyDataTypeName = value; }
        }

        public Dictionary<string, string> EditorTemplateParams
        {
            get { return Params; }
            set { Params = value; }
        }

        public override T Copy<T>(T propertyView = null)
        {
            var editor = propertyView as EditorViewModel ?? new EditorViewModel();

            editor.IsReadOnly = this.IsReadOnly;
            editor.IsRequired = this.IsRequired;
            editor.IsLabelVisible = this.IsLabelVisible;
            editor.TabName = this.TabName;
            editor.WizardName = this.WizardName;

            if (this.CascadeFrom != null)
            {
                editor.CascadeFrom = new CascadeFrom()
                {
                    Field = this.CascadeFrom.Field,
                    IdField = this.CascadeFrom.IdField
                };
            }

            editor.EditorTemplate = this.EditorTemplate;
            editor.EditorTemplateParams = EditorTemplateParams?.ToDictionary(x => x.Key, x => x.Value);

            editor.Group = this.Group;
            editor.GroupOrder = this.GroupOrder;
            editor.SortOrder = this.SortOrder;
            editor.OnChangeClientScript = this.OnChangeClientScript;
            editor.OnClientEditorChange = this.OnClientEditorChange;
            editor.SetCustomParams = this.SetCustomParams;
            editor.BgColor = this.BgColor;
            editor.Mnemonic = this.Mnemonic;
            return base.Copy(editor as T);
        }

        #region Sib

        //TODO: добавить обработку onChange на все редакторы
        //пока работает установка onChange для редакторов Editor\Common\BaseObjectOne
        public string OnChangeClientScript { get; set; }

        //TODO: добавить обработку onChange едитора на все редакторы
        // нужен для попадания в событие onChange самого редактора, а не свойства формы
        //(во избежания рекурсии для задач, завязанных на логике изменения множества полей)
        //пока работает установка onChange для редакторf Editor\Decimal
        public string OnClientEditorChange { get; set; }

        #endregion Sib

        /// <summary>
        /// Используется для возмоности передать инициализацию castomParams
        /// </summary>
        public string SetCustomParams { get; set; }
    }

    public enum Relationship
    {
        None = 0,
        One = 1,
        OneToMany = 2,
        ManyToMany = 3
    }

    public class CascadeFrom
    {
        public string Field { get; set; }
        public string IdField { get; set; }

        /// <summary>
        /// Флаг, определяющий доступность элемента, при пустом значении вышестоящего.
        /// </summary>
        public bool LockDependentsIfParentEmpty { get; set; }
    }
}