using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Base.DAL;

namespace Base.UI
{
    public class OneToManyAssociationEditor : EditorViewModel
    {
        public Func<IUnitOfWork, IQueryable, int, Guid, IQueryable> Filter { get; set; }
        public LambdaExpression FilterExpression { get; set; }
        public Action<IUnitOfWork, object, int> Create { get; set; }
        public Action<IUnitOfWork, object, int> Delete { get; set; }
        private EditorAssociationType _type;
        public EditorAssociationType Type
        {
            get { return _type; }
            set
            {
                PropertyDataTypeName = $"OneToManyAssociation_{value}";
                _type = value;
            }
        }

        public string Controller { get; set; }

        public override T Copy<T>(T propertyView = null)
        {
            var editor = propertyView as OneToManyAssociationEditor ?? new OneToManyAssociationEditor();
            editor.Filter = Filter;
            editor.FilterExpression = FilterExpression;
            editor.Create = this.Create;
            editor.Delete = this.Delete;
            editor.Type = this.Type;
            editor.Controller = this.Controller;
            return base.Copy(editor as T);
        }
    }

    public enum EditorAssociationType
    {
        InLine,
        InModal
    }

    public static class OneToManyAssociationParamsExtensions
    {

        public static T Create<T>(this OneToManyAssociationParams parameters,T entity, IUnitOfWork unit_of_work, IViewModelConfigService view_model_config_service)
        {

            var config = view_model_config_service.Get(parameters.Mnemonic);

            var editor = (OneToManyAssociationEditor)config.DetailView.Editors.Single(x => x.SysName == parameters.SysName);

            editor.Create?.Invoke(unit_of_work, entity, parameters.Id);

            return entity;
        }
    }


    public class OneToManyAssociationParams
    {

        public OneToManyAssociationParams()
        {

        }

        [Obsolete()]
        public OneToManyAssociationParams(string @params)
        {
            if (string.IsNullOrEmpty(@params)) return;

            Mnemonic = GetParam(@params, "parent_mnemonic");
            Id = int.Parse(GetParam(@params, "parent_id") ?? "0");
            SysName = GetParam(@params, "property_sysname");
        }

        public string Mnemonic { get; set; }
        public int Id { get; set; }
        public string SysName { get; set; }
        public bool Success => !string.IsNullOrEmpty(Mnemonic) && !string.IsNullOrEmpty(SysName) && Id != 0;


        private static string GetParam(string @params, string param)
        {
            var regex = new Regex($@"{param}\(.*?\)", RegexOptions.IgnoreCase);
            var match = regex.Match(@params);
            return match.Success ? match.Groups[0].Value.Replace($"{param}(", "").Replace(")", "") : null;
        }
    }
}