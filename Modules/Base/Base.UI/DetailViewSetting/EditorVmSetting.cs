using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Base.Attributes;
using Base.Entities;
using Base.Macros;
using Base.Macros.Entities;
using Base.Macros.Entities.Rules;
using Base.Rule;
using Base.Security;
using AppContext = Base.Ambient.AppContext;

namespace Base.UI.DetailViewSetting
{
    public class EditorVmSetting : BaseEditorVmSetting , ITransform
    {
        public EditorVmSetting(){ }


        public EditorVmSetting(EditorViewModel editorViewModel)
            : base(editorViewModel)
        {
            Title = editorViewModel.Title;
            Description = editorViewModel.Description;
            TabName = editorViewModel.TabName;
            Required = editorViewModel.IsRequired;
            Visible = editorViewModel.Visible;
            Enable = !editorViewModel.IsReadOnly;
            HideLabel = !editorViewModel.IsLabelVisible;
            SortOrder = editorViewModel.SortOrder;
        }

        [MaxLength(255)]
        [DetailView(Name = "Наименование", Required = true), ListView]
        public string Title { get; set; }

        [DetailView(Name = "Скрыть наименование")]
        public bool HideLabel { get; set; }

        [DetailView(Name = "Описание"), ListView]
        public string Description { get; set; }

        [MaxLength(255)]
        [DetailView(Name = "Наименование вкладки"), ListView]
        public string TabName { get; set; }

        [DetailView(Name = "Обязательно"), ListView]
        public bool Required { get; set; }

        [DetailView(Name = "Видимо"), ListView]
        public bool Visible { get; set; }

        [DetailView(Name = "Доступно"), ListView]
        public bool Enable { get; set; }

        [DetailView(Name = "Порядковый номер")]
        public int? FieldOrder { get; set; }


        [PropertyDataType(PropertyDataType.ConditionalBranch)]
        [DetailView(Name = "Видимо", TabName = "[1]Правила")]
        public virtual ICollection<RuleVisible> VisibleRules { get; set; }

        [PropertyDataType(PropertyDataType.ConditionalBranch)]

        [DetailView(Name = "Скрыто", TabName = "[1]Правила")]
        public virtual ICollection<RuleHidden> HiddenRules { get; set; }

        [PropertyDataType(PropertyDataType.ConditionalBranch)]

        [DetailView(Name = "Доступно", TabName = "[1]Правила")]
        public virtual ICollection<RuleEnable> EnableRules { get; set; }

        [PropertyDataType(PropertyDataType.ConditionalBranch)]

        [DetailView(Name = "Только чтение", TabName = "[1]Правила")]
        public virtual ICollection<ReadOnlyEnable> ReadOnlyRules { get; set; }

        [PropertyDataType(PropertyDataType.Rules)]
        [DetailView(Name = "Видимо", TabName = "[1]Правила")]
        public virtual ICollection<BRuleVisible> BVisibleRules { get; set; }
        [PropertyDataType(PropertyDataType.Rules)]
        [DetailView(Name = "Скрыто", TabName = "[1]Правила")]
        public virtual ICollection<BRuleHidden> BHiddenRules { get; set; }
        [PropertyDataType(PropertyDataType.Rules)]
        [DetailView(Name = "Доступно", TabName = "[1]Правила")]
        public virtual ICollection<BRuleEnable> BEnableRules { get; set; }
        [PropertyDataType(PropertyDataType.Rules)]
        [DetailView(Name = "Только чтение", TabName = "[1]Правила")]
        public virtual ICollection<BRuleReadOnly> BReadOnlyRules { get; set; }

        //[DetailView(Name = "Видимо", TabName = "[2]Роли")]
        //public virtual ICollection<FieldRoleVisible> VisibleRoles { get; set; }

        //[DetailView(Name = "Скрыто", TabName = "[2]Роли")]
        //public virtual ICollection<FieldRoleHidden> HiddenRoles { get; set; }

        //[DetailView(Name = "Доступно", TabName = "[2]Роли")]
        //public virtual ICollection<FieldRoleEnable> EnableRoles { get; set; }

        //[DetailView(Name = "Только чтение", TabName = "[2]Роли")]
        //public virtual ICollection<FieldRoleReadOnly> ReadOnlyRoles { get; set; }



        public override void Apply(EditorViewModel editor, Func<IEnumerable<ConditionItem>, bool> check)
        {
            editor.Title = this.Title;
            editor.Description = this.Description;

            if(!string.IsNullOrEmpty(TabName))
                editor.TabName = TabName;

            editor.IsRequired = this.Required;
            editor.Visible = this.Visible;
            editor.IsReadOnly = !this.Enable;
            editor.IsLabelVisible = !this.HideLabel;
            editor.SortOrder = this.FieldOrder ?? this.SortOrder;

            //
            if (check != null)
            {
                if (this.VisibleRules.Any())
                    editor.Visible = check(VisibleRules);

                if (this.BVisibleRules.Any())
                    editor.Visible = BVisibleRules.Any(x => check(x.Object.Rules));

                if (this.HiddenRules.Any())
                    editor.Visible = !check(HiddenRules);

                if (this.BHiddenRules.Any())
                    editor.Visible = !BHiddenRules.Any(x => check(x.Object.Rules));

                if (this.EnableRules.Any())
                    editor.IsReadOnly = !check(EnableRules);

                if (this.BEnableRules.Any())
                    editor.IsReadOnly = !BEnableRules.Any(x => check(x.Object.Rules));

                if (this.ReadOnlyRules.Any())
                    editor.IsReadOnly = check(ReadOnlyRules);

                if (this.BReadOnlyRules.Any())
                    editor.IsReadOnly = BReadOnlyRules.Any(x => check(x.Object.Rules));
            }

            //if (this.VisibleRoles.Any())
            //    editor.Visible = AppContext.SecurityUser.IsAdmin || this.VisibleRoles.Any(x => AppContext.SecurityUser.IsRole(x.ID));

            //if (this.HiddenRoles.Any() && !AppContext.SecurityUser.IsAdmin)
            //    editor.Visible = !this.HiddenRoles.Any(x => AppContext.SecurityUser.IsRole(x.ID));

            //if (this.EnableRoles.Any())
            //    editor.IsReadOnly = !(AppContext.SecurityUser.IsAdmin || this.EnableRoles.Any(x => AppContext.SecurityUser.IsRole(x.ID)));

            //if (this.ReadOnlyRoles.Any() && !AppContext.SecurityUser.IsAdmin)
            //    editor.IsReadOnly = this.ReadOnlyRoles.Any(x => AppContext.SecurityUser.IsRole(x.ID));
        }
    }

    public class RuleVisible : ConditionItem
    {
        public int? EditorVmSettingID { get; set; }
        public virtual EditorVmSetting EditorVmSetting { get; set; }
    }

    public class RuleHidden : ConditionItem
    {
        public int? EditorVmSettingID { get; set; }
        public virtual EditorVmSetting EditorVmSetting { get; set; }
    }

    public class RuleEnable : ConditionItem
    {
        public int? EditorVmSettingID { get; set; }
        public virtual EditorVmSetting EditorVmSetting { get; set; }
    }

    public class ReadOnlyEnable : ConditionItem
    {
        public int? EditorVmSettingID { get; set; }
        public virtual EditorVmSetting EditorVmSetting { get; set; }
    }

    public class BRuleVisible : EasyCollectionEntry<RuleForType>
    {
        public int? EditorVmSettingID { get; set; }
        public virtual EditorVmSetting EditorVmSetting { get; set; }
    }

    public class BRuleHidden : EasyCollectionEntry<RuleForType>
    {
        public int? EditorVmSettingID { get; set; }
        public virtual EditorVmSetting EditorVmSetting { get; set; }
    }

    public class BRuleEnable : EasyCollectionEntry<RuleForType>
    {
        public int? EditorVmSettingID { get; set; }
        public virtual EditorVmSetting EditorVmSetting { get; set; }
    }

    public class BRuleReadOnly : EasyCollectionEntry<RuleForType>
    {
        public int? EditorVmSettingID { get; set; }
        public virtual EditorVmSetting EditorVmSetting { get; set; }
    }


    //public class FieldRoleVisible : EasyCollectionEntry<Role>
    //{

    //}

    //public class FieldRoleHidden : EasyCollectionEntry<Role>
    //{

    //}

    //public class FieldRoleEnable : EasyCollectionEntry<Role>
    //{

    //}

    //public class FieldRoleReadOnly : EasyCollectionEntry<Role>
    //{

    //}
}
