using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Base.Attributes;
using Base.Utils.Common.Maybe;
using Base.Validation;
using WebUI.Models.Validation;

namespace WebUI.Controllers
{
    public class ValidationController : BaseController
    {
        private readonly IValidationConfigManager _validationConfigManager;

        public ValidationController(IBaseControllerServiceFacade facade, IValidationConfigManager configManager)
            : base(facade)
        {
            _validationConfigManager = configManager;
        }

    
        public ActionResult GetValidationRules(string objectType)
        {
            var type = Type.GetType(objectType) ?? GetType(objectType);
            if (type != null)
            {
                ValidationRulesVm vm = new ValidationRulesVm
                {
                    ValidationRules = _validationConfigManager.GetRules(type),
                };
                return PartialView("_ValidationRules", vm);
            }
            return null;
        }

        private IEnumerable<PropertyEditorVm> GetEditorsVms(Type type, Func<Type, bool> predicate = null)
        {
            var allProps = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.SetField).Where(x => x.CanWrite).ToList();

            if (predicate != null)
                allProps = allProps.Where(x => predicate(x.PropertyType)).ToList();
            return allProps.Select(x => new { Prop = x, Attr = x.GetCustomAttribute<DetailViewAttribute>() })
                .Where(x => x.Attr != null).Select(x => new PropertyEditorVm(x.Attr.Name, x.Prop));
        }
        private Type GetType(string objectType)
        {
            return !String.IsNullOrEmpty(objectType) ? GetViewModelConfig(objectType).With(x => x.TypeEntity) : null;
        }



    }
}




//public ActionResult CreateValidationRule(string objectType)
//{
//    var type = Type.GetType(objectType) ?? GetType(objectType);

//    if (type != null)
//    {
//        var props = this.GetEditorsVms(type, x =>
//        {
//            if (x != typeof(String) &&
//                x.GetInterfaces()
//                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
//            {
//                return false;
//            }

//            return true;
//        }).ToList();

//        CreateValidationVm vm = new CreateValidationVm
//        {
//            ObjectType = objectType,
//            Properties = props,
//        };
//        return PartialView("ValidationRuleCreator", vm);

//    }

//    throw new Exception("Тип не найден " + objectType);
//}

//public ActionResult GetEditorsVm(string objectType)
//{
//    var type = Type.GetType(objectType) ?? GetType(objectType);

//    if (type != null)
//    {
//        var props = this.GetEditorsVms(type, x =>
//        {
//            if (x != typeof(String) && x.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
//            {
//                return false;
//            }

//            return true;
//        }).ToList();

//        var rules = _validationConfigManager.GetRules(type).Select(x => new ValidationRuleVm() { Title = x.Title, Type = x.GetType().GetTypeName() }).ToList();

//        return PartialView("EditValidationProperties", new ValidationVm()
//        {
//            Editors = props,
//            ObjectType = type,
//            Mnemonic = this.GetViewModelConfig(type).Mnemonic,
//            ObjectValidationRules = rules,
//        });
//    }

//    return null;
//}
