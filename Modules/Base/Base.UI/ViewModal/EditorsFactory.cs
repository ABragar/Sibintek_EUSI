using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Base.Attributes;
using Base.UI.Editors;
using Base.UI.Service;

namespace Base.UI
{
    public class EditorsFactory<T> where T : class
    {
        private readonly List<EditorViewModel> _editors;

        public EditorsFactory(List<EditorViewModel> editors)
        {
            this._editors = editors;
        }

        public EditorsFactory<T> Clear()
        {
            _editors.Clear();
            return this;
        }

        public EditorsFactory<T> HideAll()
        {
            _editors.ForEach(x =>
            {
                x.Visible = false;
            });

            return this;
        }

        public EditorsFactory<T> Add<TValue>(Expression<Func<T, TValue>> expression, Action<EditorBuilder<T>> action = null)
        {
            var exp = expression.Body as MemberExpression;

            if (exp == null)
                throw new Exception("propertyExpression");

            var propInfo = exp.Member as PropertyInfo;

            var editor = _editors.SingleOrDefault(x => x.PropertyName == propInfo.Name);

            if (editor == null)
            {
                editor = ViewModelConfigFactory.CreateEditor<T>(propInfo);
                //editor.SortOrder = _editors.Max(x => x.SortOrder) + 1;
                _editors.Add(editor);
            }

            action?.Invoke(new EditorBuilder<T>(editor));

            return this;
        }

        public EditorsFactory<T> Add<TValue>(string id = null, Action<EditorBuilder<T>> action = null)
        {
            EditorViewModel editor = null;

            if (id != null)
                editor = _editors.SingleOrDefault(x => x.SysName == id);

            if (editor == null)
            {
                editor = new EditorViewModel
                {
                    SysName = id ?? Guid.NewGuid().ToString("N"),
                    PropertyName = null,
                    PropertyType = typeof(TValue),
                    Title = typeof(TValue).Name,
                    SortOrder = _editors.Max(x => x.SortOrder) + 1,
                };

                ViewModelConfigFactory.InitPropertyVm(typeof(T), editor, null);

                _editors.Add(editor);
            }

            action?.Invoke(new EditorBuilder<T>(editor));

            return this;
        }

        public EditorsFactory<T> AddOneToManyAssociation<TEntity>(string sysName, Action<OneToManyAssociationEditorBuilder<T, TEntity>> action = null) where TEntity : IBaseObject
        {
            if (string.IsNullOrWhiteSpace(sysName))
                throw new ArgumentNullException(nameof(sysName));

            var editor = (OneToManyAssociationEditor)_editors.SingleOrDefault(x => x.SysName == sysName);

            if (editor == null)
            {
                editor = new OneToManyAssociationEditor
                {
                    SysName = sysName,
                    PropertyName = null,
                    PropertyType = typeof(TEntity),
                    Title = "",
                    IsLabelVisible = false,
                    SortOrder = _editors.Max(x => x.SortOrder) + 1,
                    Type = EditorAssociationType.InLine
                };

                _editors.Add(editor);
            }

            action?.Invoke(new OneToManyAssociationEditorBuilder<T, TEntity>(editor));

            return this;
        }

        private void AddManyToManyAssociation(string sysName, Type manyToManyType, Type manyToManyLeftOrRightType, ManyToManyAssociationType associationType, Action<ManyToManyAssociationEditorBuilder> action)
        {
            ManyToManyAssociationEditor editor = null;

            if (sysName != null)
                editor = (ManyToManyAssociationEditor)_editors.SingleOrDefault(x => x.SysName == sysName);

            if (editor != null) return;

            editor = new ManyToManyAssociationEditor(manyToManyType, associationType)
            {
                SysName = sysName ?? Guid.NewGuid().ToString("N"),
                PropertyName = null,
                PropertyDataTypeName = "",
                PropertyType = manyToManyType.GetInterfaces()
                    .Single(x => x.IsGenericType && x.GetGenericTypeDefinition() == manyToManyLeftOrRightType),
                Title = "",
                IsLabelVisible = false,
                SortOrder = _editors.Max(x => x.SortOrder) + 1,
                Type = EditorAssociationType.InLine
            };

            action?.Invoke(new ManyToManyAssociationEditorBuilder(editor));

            _editors.Add(editor);
        }

        public EditorsFactory<T> AddManyToManyLeftAssociation<TManyToMany>(string sysName, Action<ManyToManyAssociationEditorBuilder> action = null) where TManyToMany : IManyToManyLeftAssociation<T>
        {
            AddManyToManyAssociation(sysName, typeof(TManyToMany), typeof(IManyToManyRightAssociation<>), ManyToManyAssociationType.Left, action);

            return this;
        }

        public EditorsFactory<T> AddManyToManyLeftAssociation<TLeft, TManyToMany>(string sysName, Action<ManyToManyAssociationEditorBuilder> action = null) where TManyToMany : IManyToManyLeftAssociation<TLeft>
        {
            AddManyToManyAssociation(sysName, typeof(TManyToMany), typeof(IManyToManyRightAssociation<>), ManyToManyAssociationType.Left, action);

            return this;
        }

        public EditorsFactory<T> AddManyToManyRigthAssociation<TManyToMany>(string sysName, Action<ManyToManyAssociationEditorBuilder> action = null) where TManyToMany : IManyToManyRightAssociation<T>
        {
            AddManyToManyAssociation(sysName, typeof(TManyToMany), typeof(IManyToManyLeftAssociation<>), ManyToManyAssociationType.Rigth, action);

            return this;
        }

        public EditorsFactory<T> AddManyToManyRigthAssociation<TRight, TManyToMany>(string sysName, Action<ManyToManyAssociationEditorBuilder> action = null) where TManyToMany : IManyToManyRightAssociation<TRight>
        {
            AddManyToManyAssociation(sysName, typeof(TManyToMany), typeof(IManyToManyLeftAssociation<>), ManyToManyAssociationType.Rigth, action);

            return this;
        }

        //sib
        /// <summary>
        /// Добавляет пустой текстовый эдитор.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public EditorsFactory<T> AddEmpty(Action<EditorBuilder<T>> action = null)
        {
            EditorViewModel editor = null;
            var oid = Guid.NewGuid().ToString("N");
            editor = new EditorViewModel
            {
                SysName = oid,
                PropertyName = null,
                PropertyType = typeof(string),
                PropertyDataType = PropertyDataType.Text,
                PropertyDataTypeName = "Text",
                Title = "EmptyItem_" + oid,
                SortOrder = _editors.Max(x => x.SortOrder) + 1,
            };

            ViewModelConfigFactory.InitPropertyVm(typeof(T), editor, null);

            _editors.Add(editor);

            action?.Invoke(new EditorBuilder<T>(editor));

            return this;
        }

        /// <summary>
        /// Сброс сортировки эдиторов.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public EditorsFactory<T> ResetOrder(Action<EditorBuilder<T>> action = null)
        {
            _editors.ForEach(item =>
           {
               item.SortOrder = -1;
           });
            return this;
        }

        /// <summary>
        /// Добавляет эдитор по наименованию свойства.
        /// </summary>
        /// <param name="propertyName">Наименование свойства.</param>
        /// <param name="action">Действие.</param>
        /// <param name="condition">Условие добавления. По результату выполнения условия: True - эдитор добавляется, False - нет.</param>
        /// <returns></returns>
        public EditorsFactory<T> AddProp(
            string propertyName
            , Action<EditorBuilder<T>> action = null
            , Func<bool> condition = null,
             bool allowDuplicate = false)
        {
            if ((condition != null && !condition()) || String.IsNullOrEmpty(propertyName))
                return this;

            EditorViewModel editor = null;
            var propInfo = typeof(T).GetProperty(propertyName);
            if (propInfo != null)
            {
                editor = ViewModelConfigFactory.CreateEditor<T>(propInfo);

                var existingEditorIndex = _editors.FindIndex(x => x.PropertyName == propertyName);

                if (!allowDuplicate && existingEditorIndex >= 0)
                {
                    _editors[existingEditorIndex] = editor;
                }
                else
                {
                    _editors.Add(editor);
                }
            }
            if (editor != null)
                action?.Invoke(new EditorBuilder<T>(editor));

            return this;
        }

        public EditorsFactory<T> ChangeProp(
            string propertyName
            , Action<EditorBuilder<T>> action = null
            )
        {
            if (String.IsNullOrEmpty(propertyName))
                return this;

            EditorViewModel editor = null;
            var propInfo = typeof(T).GetProperty(propertyName);
            if (propInfo != null)
            {
                editor = _editors.SingleOrDefault(x => x.PropertyName == propertyName);
            }
            if (editor != null)
                action?.Invoke(new EditorBuilder<T>(editor));

            return this;
        }

        ///// <remark>ссылочная целостность не контроллируется</remark>

        /// <summary>
        /// Добавление полей в карточку из конфигурации мнемоники<see cref="mnemonic"/>.
        /// </summary>
        /// <param name="mnemonic">Мнемоника с конфигурацией редакторов добавляемой модели</param>
        /// <param name="parentProperty">Свойство основной модели связанное один к одному с моделью интегрируемой в интерфейсе</param>
        /// <param name="childProperty">Свойство интегрируемой в интерфейсе модели используется для связи один к одному с основной моделью></param>
        /// <param name="action">Конфигурация добавляемого редактора</param>
        /// <returns></returns>
        /// <example>
        /// Пример добавления полей интегрируемой модели на карточку. В качестве интегрируемой в интерфейс модели выступает мнемоника Aircraft_SeparRealEstate_Appointments
        /// связи установлены образованы полем nameof(RealEstate.SeparRealEstateID) основной модели и полем nameof(IBaseObject.ID)
        /// с помощью конфигуратора редактора <see cref="action"> указан TabName добавляемых полей
        /// <code>
        /// .AddPartialEditor("Aircraft_SeparRealEstate_Appointments", nameof(RealEstate.SeparRealEstateID), nameof(IBaseObject.ID), peb => peb.TabName(EstateTabs.MainData))
        /// </code>
        /// </example>
        public EditorsFactory<T> AddPartialEditor(string mnemonic, string parentProperty, string childProperty, Action<EditorBuilder<T>> action = null, bool reverse = false)
        {
            if (string.IsNullOrEmpty(mnemonic))
                throw new ArgumentException("Argument must not be the empty string.", nameof(mnemonic));
            if (string.IsNullOrEmpty(parentProperty))
                throw new ArgumentException("Argument must not be the empty string.", nameof(parentProperty));
            if (string.IsNullOrEmpty(childProperty))
                throw new ArgumentException("Argument must not be the empty string.", nameof(childProperty));

            var linkValue = reverse
                ? $"child.{childProperty}=parent.{parentProperty}"
                : $"parent.{parentProperty}=child.{childProperty}";
            var editor = new EditorViewModel
            {
                SysName = mnemonic,
                PropertyName = null,
                PropertyType = typeof(string),
                SortOrder = _editors.Max(x => x.SortOrder) + 1
            };

            var editorBuilder = new EditorBuilder<T>(editor)
                .Mnemonic(mnemonic)
                .Title(mnemonic)
                .EditorTemplate(nameof(PropertyDataType.PartialEditor))
                .DataType(PropertyDataType.PartialEditor)
                .AddParam("link", linkValue);

            _editors.Add(editor);

            action?.Invoke(editorBuilder);

            return this;
        }

        //end sib
    }
}