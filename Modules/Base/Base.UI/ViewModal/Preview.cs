using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using Base.DAL;
using Base.Service;
using Base.UI.Service;

namespace Base.UI.ViewModal
{
    public class Preview : View
    {
        private Type _typeEntity;
        public bool Enable { get; set; }
        public LambdaExpression Selector { get; set; }
        public Func<IUnitOfWork, object, object> CustomSelect { get; set; }

        public void SetType(Type type)
        {
            _typeEntity = type;
        }

        public override Type TypeEntity => _typeEntity ?? Config.TypeEntity;

        public IQueryable GetPreviewExtendedData(IUnitOfWork uofw, IQueryService<BaseObject> serv, int id, string extended)
        {
            var data = Extended.Single(x => x.Name == extended);

            var q = data.GetPreviewExtendedData(uofw, serv, id);

            return q;
        }


        public object GetData(IUnitOfWork uofw, IQueryService<BaseObject> serv, int id)
        {
            IQueryable q = serv.GetAll(uofw, hidden: null).Where(x => x.ID == id);

            if (Selector != null)
            {
                q = q.Provider.CreateQuery(
                    Expression.Call(
                        typeof(Queryable), "Select",
                        new Type[] {q.ElementType, Selector.Body.Type},
                        q.Expression, Expression.Quote(Selector)));
            }
            else if (CustomSelect != null)
            {
                return CustomSelect(uofw, q.Single());
            }
            else
            {
                q = Select(q);
            }

            return q.Single();
        }

        public List<PreviewField> Fields { get; set; } = new List<PreviewField>();
        public List<ExtendedData> Extended { get; set; } = new List<ExtendedData>();
        public override IEnumerable<PropertyViewModel> Props => Fields;

        public override T Copy<T>(T view = null)
        {
            var preview = view as Preview ?? new Preview();

            preview.Enable = this.Enable;
            preview.Selector = this.Selector;
            preview.CustomSelect = this.CustomSelect;
            preview.SetType(this._typeEntity);

            foreach (var field in this.Fields)
            {
                preview.Fields.Add(field.Copy<PreviewField>());
            }

            foreach (var extendedData in Extended)
            {
                preview.Extended.Add(extendedData.Copy());
            }

            return base.Copy(preview as T);
        }
    }

    //что это?
    public class ExtendedData
    {
        private static int temp = 0;
        public string Name { get; set; }
        public string Title { get; set; }
        public string TabName { get; set; }
        protected LambdaExpression Selector;
        public LambdaExpression Order { get; set; }
        public List<PreviewField> Fields => Config.Preview.Fields;
        protected ViewModelConfig Config;
        protected Type Type;

        protected ExtendedData() { }

        public ExtendedData(LambdaExpression selector, Type type, IInitializerContext context)
        {
            Selector = selector;

            var exp = selector.Body as MemberExpression;

            if (exp == null)
            {
                Name = "extended" + temp++;
            }
            else
            {
                Name = exp.Member.Name + temp++;


                var prop = exp.Member as PropertyInfo;

                if (prop != null)
                {
                    var editor = ViewModelConfigFactory.CreateEditor(prop.DeclaringType, prop);

                    TabName = editor.TabName;
                    Title = editor.Title;
                }
            }

            Config = ViewModelConfigFactory.CreateDefault(type, context.GetChildContext<IServiceLocator>());
            Config.Preview.Config = Config;
            Type = type;
        }

        public IQueryable GetPreviewExtendedData(IUnitOfWork uofw, IQueryService<BaseObject> serv, int id)
        {
            IQueryable q = serv.GetAll(uofw, hidden: null).Where(x => x.ID == id);


            q = q.Provider.CreateQuery(
                Expression.Call(typeof(Queryable), "SelectMany",
                    new Type[] {q.ElementType, Type},
                    q.Expression, Expression.Quote(Selector)));

            if (Order != null)
            {
                q =
                    q.Provider.CreateQuery(Expression.Call(typeof(Queryable), "OrderBy",
                        new Type[] {q.ElementType, Order.Type}, q.Expression, Expression.Quote(Order)));
            }
            else
            {
                q = q.OrderBy("1");
            }

            return Config.Preview.Select(q);
        }

        public ExtendedData Copy()
        {
            var extendedData = new ExtendedData()
            {
                Name = this.Name,
                Title = this.Title,
                TabName = this.TabName,
                Selector = this.Selector,
                Order = this.Order,
                Config = this.Config,
                Type = this.Type
            };

            return extendedData;
        }
    }
}