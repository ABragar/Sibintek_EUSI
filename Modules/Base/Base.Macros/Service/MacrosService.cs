using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using Base.DAL;
using Base.Macros.Entities;
using Base.Security;
using Base.Utils.Common;
using Base.Utils.Common.Maybe;
using IronPython.Hosting;
using Newtonsoft.Json;

namespace Base.Macros.Service
{

    public class MacrosService : IMacrosService
    {


        #region HelperClasses from anonymus
        private class ObjectRefItem
        {
            public string Type { get; set; }
            public int ID { get; set; }
        }

        private class RefObject
        {
            private readonly PropertyInfo _property;
            private readonly object _o;

            public PropertyInfo Property => _property;
            public object Object => _o;
            public RefObject(PropertyInfo property, object o)
            {
                _property = property;
                _o = o;
            }
        }

        private class MacroPropertyPair
        {
            private readonly IEnumerable<InitItem> _macroses;
            private readonly PropertyInfo _property;

            public IEnumerable<InitItem> Macroses
            {
                get { return _macroses; }
            }

            public PropertyInfo Property
            {
                get { return _property; }
            }

            public MacroPropertyPair(IEnumerable<InitItem> macroses, PropertyInfo property)
            {
                _macroses = macroses;
                _property = property;
            }
        }

        #endregion


        public bool CheckBranch(IUnitOfWork uow, BaseObject obj, IEnumerable<ConditionItem> inits)
        {
            var engine = Python.CreateEngine();

            engine.Runtime.LoadAssembly(Assembly.GetExecutingAssembly());

            var scope = engine.CreateScope(new Dictionary<string, object> { { "obj", obj } });

            StringBuilder sb = new StringBuilder("import clr\r");
            sb.Append("clr.AddReference(\"System.Core\")\r");
            sb.Append("import System\r");
            sb.Append("clr.ImportExtensions(System.Linq)\r");            
            sb.Append("def checkObj():\r    if(");

            bool isFirst = true;

            var conditions = PrepareConditions(inits.Where(x => x.Hidden == false));

            foreach (var inititem in conditions)
            {
                if (!isFirst) sb.Append(" and ");

                switch (inititem.MacroType)
                {
                    case MacroType.String:
                        sb.AppendFormat(
                            inititem.Source == ConditionItemSource.Value
                                ? "'{0}' {2} str(obj.{1})"
                                : "str(obj.{0}){2} str(obj.{1})", inititem.Value, inititem.Member, inititem.Operator);
                        break;

                    case MacroType.Number:
                        sb.AppendFormat(
                            inititem.Source == ConditionItemSource.Value
                                ? "int(obj.{1}){2}{0}"
                                : "int(obj.{1}){2}int(obj.{0})", inititem.Value, inititem.Member, inititem.Operator);
                        break;
                    case MacroType.Boolean:
                        sb.AppendFormat(
                            inititem.Source == ConditionItemSource.Value
                                ? "bool(obj.{1}){2}{0}"
                                : "bool(obj.{1}){2}bool(obj.{0})", inititem.Value, inititem.Member, inititem.Operator);
                        break;

                    case MacroType.BaseObject:
                        sb.AppendFormat("obj.{1} is not None and int(obj.{1}.ID){2}{0}", JsonConvert.DeserializeObject<ObjectRefItem>(inititem.Value).ID, inititem.Member, inititem.Operator);
                        break;

                    case MacroType.DateTime:
                        if (inititem.Value == "DateTime.Now()")
                        {
                            var locVar = this.GenerateVaribleName();
                            scope.SetVariable(locVar, DateTime.Now);
                            sb.AppendFormat("datetime(obj.{0}) {1} {2}", inititem.Member, inititem.Operator, locVar);
                        }
                        else
                        {                            
                            if (inititem.Source == ConditionItemSource.FromObj)
                            {
                                sb.AppendFormat("datetime(obj.{0}) {1} datetime(obj.{2})", inititem.Member, inititem.Operator, inititem.Value);
                            }
                            else
                            {
                                var locVar = this.GenerateVaribleName();
                                scope.SetVariable(locVar, DateTime.Parse(inititem.Value));
                                sb.AppendFormat("datetime(obj.{0}) {1} {2})",inititem.Member, inititem.Operator, locVar);
                            }
                        }

                        
                        break;

                    case MacroType.TimeSpan:

                        break;


                    case MacroType.CollectionItem:
                        sb.AppendFormat("{1} obj.{0}.Any(lambda x:", inititem.Member, inititem.Operator);
                        var objIds = inititem.Value.Split(';');
                        bool isf = true;
                        foreach (var id in objIds)
                        {
                            if (!isf) sb.Append(" or ");
                            sb.AppendFormat("x.ID == {0}", id);
                            isf = false;
                        }
                        sb.Append(")");
                        break;

                    case MacroType.EqualNull:
                        sb.AppendFormat("obj.{0} is None", inititem.Member);
                        break;

                    case MacroType.NotNull:
                        sb.AppendFormat("obj.{0} is not None", inititem.Member);
                        break;

                    case MacroType.InitObject:
                        sb.AppendFormat("obj.{0} {2} obj.{1}", inititem.Member, inititem.Value, inititem.Operator);
                        break;
                }

                isFirst = false;
            }




            sb.Append("):\r");
            sb.Append("        return bool(1)\r");
            sb.Append("    else:\r");
            sb.Append("        return bool(0)\r");
            sb.Append("checkObj()");
            string script = sb.ToString();

            var result = engine.Execute(script, scope);
            if (result != null && result is bool)
            {
                return (bool)result;
            }

            return false;
        }

        //http://vk.com/doc265879884_429284411?hash=3ce556ae871e1ecbc3&dl=aae0ee89cc95d58c7a

        public void InitializeObject(ISecurityUser securityUser, BaseObject src, BaseObject dest, IEnumerable<InitItem> inits)
        {
            var resultScript = String.Empty;

            try
            {
                var engine = Python.CreateEngine();

                engine.Runtime.LoadAssembly(Assembly.GetExecutingAssembly());

                var scope = engine.CreateScope(new Dictionary<string, object> { { "Dest", dest }, { "Src", src } });

                IEnumerable<RefObject> refObjects;

                var macroses = PrepareMacros(dest, inits, out refObjects);

                foreach (var refObj in refObjects)
                    scope.SetVariable(String.Format("_refobject_{0}", refObj.Property.Name), refObj.Object);

                foreach (var pair in macroses.AsEnumerable().Reverse())
                {
                    var script = new StringBuilder(String.Format("Dest.{0} =", pair.Property.Name));

                    foreach (var initItem in pair.Macroses)
                    {
                        switch (initItem.MacroType)
                        {
                            case MacroType.String:
                                script.AppendFormat(" '{0}'", initItem.Value);
                                break;
                            case MacroType.Number:
                                {
                                    if (pair.Property.PropertyType.IsEnum)
                                    {
                                        var tempName = String.Format("_enum{0}", pair.Property.Name);

                                        scope.SetVariable(tempName, Enum.Parse(pair.Property.PropertyType, initItem.Value));

                                        script.AppendFormat(tempName);

                                        break;
                                    }

                                    goto case MacroType.Operator;
                                }
                            case MacroType.Boolean:
                                {
                                    script.AppendFormat(" '{0}'", initItem.Value);

                                    break;
                                }
                            case MacroType.Operator:
                                script.AppendFormat(" {0}", initItem.Value);
                                break;
                            case MacroType.InitObject:
                                {
                                    script.AppendFormat(" Src.{0}", initItem.Value);
                                    break;
                                }
                            case MacroType.BaseObject:
                                if (dest is ICategorizedItem)
                                {
                                    var foreignKey = pair.Property.GetCustomAttribute<ForeignKeyAttribute>();

                                    if (foreignKey != null && foreignKey.Name == "CategoryID")
                                    {
                                        script.Append(" None");

                                        dest.GetType().GetProperty("CategoryID").SetValue(dest, JsonConvert.DeserializeObject<ObjectRefItem>(initItem.Value).ID);

                                        break;
                                    }
                                }

                                script.AppendFormat(" _refobject_{0}", pair.Property.Name);
                                break;
                            case MacroType.Function:
                                if (initItem.Value == "dtn()")
                                {
                                    var locVar = this.GenerateVaribleName();

                                    scope.SetVariable(locVar, DateTime.Now);

                                    script.AppendFormat(" {0}", locVar);
                                }
                                break;
                            case MacroType.DateTime:
                                {
                                    DateTime dt;
                                    if (DateTime.TryParse(initItem.Value, out dt))
                                    {
                                        var locVar = this.GenerateVaribleName();

                                        scope.SetVariable(locVar, dt);

                                        script.AppendFormat(" {0}", locVar);

                                        break;
                                    }

                                    throw new Exception(
                                        String.Format("Äàòà èìååò íåâåðíûé ôîðìàò {0} -> {1}", pair.Property.Name, initItem.Value));
                                }
                            case MacroType.TimeSpan:
                                {
                                    double minutes;
                                    if (double.TryParse(initItem.Value, out minutes))
                                    {
                                        var locVar = this.GenerateVaribleName();

                                        scope.SetVariable(locVar, TimeSpan.FromMinutes(minutes));

                                        script.AppendFormat(" {0}", locVar);

                                        break;
                                    }

                                    throw new Exception(
                                        String.Format("Âðåìåííîé èíòåðâàë èìåë íåâåðíûé ôîðìàò {0} -> {1}", pair.Property.Name, initItem.Value));
                                }

                            case MacroType.CollectionItem:
                                {
                                    throw new NotImplementedException("Добавь обработку коллекции");
                                }

                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }

                    script.Append(";");

                    resultScript = script.ToString();

                    engine.Execute(resultScript, scope);
                }
            }
            catch (Exception e)
            {
                throw new ScriptExecutionException("Îøèáêà âûïîëíåíèÿ ìàêðîñà" + Environment.NewLine + resultScript, e)
                    .IfNotNull(x => x.Data["Script"] = resultScript);
            }
        }

        private static IEnumerable<MacroPropertyPair> PrepareMacros(BaseObject dest, IEnumerable<InitItem> inits, out IEnumerable<RefObject> refObjects)
        {
            try
            {
                var macroses = inits.Join(dest.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance),
                    item => item.Member,
                    info => info.Name,
                    (item, info) => new MacroPropertyPair(JsonConvert.DeserializeObject<IEnumerable<InitItem>>(item.Value), info))
                    .ToList();

                var t = macroses.SelectMany(x => x.Macroses, (x, init) => new { init.MacroType, InitItem = init, x.Property })
                    .Where(x => x.MacroType == MacroType.BaseObject)
                    .Select(x => new { x.Property, ObjectRefItem = JsonConvert.DeserializeObject<ObjectRefItem>(x.InitItem.Value) });

                refObjects = t.Select(x => new RefObject(x.Property, Activator.CreateInstance(x.Property.PropertyType)
                       .IfNotNull(o => o.GetType().GetProperty("ID").SetValue(o, x.ObjectRefItem.ID))));

                return macroses;
            }
            catch (Exception e)
            {
                throw new BadMacroException("BadMacroException", e);
            }
        }


        private IEnumerable<ConditionItem> PrepareConditions(IEnumerable<ConditionItem> conditions)
        {
            return conditions.Select(x => JsonConvert.DeserializeObject<ConditionItem>(x.Value));
        }


        private string GenerateVaribleName()
        {
            return String.Format("lcv_{0}",
                Guid.NewGuid().ToString().Split('-')[0]);
        }
    }
}

