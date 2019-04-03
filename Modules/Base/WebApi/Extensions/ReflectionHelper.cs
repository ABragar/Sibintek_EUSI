using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base;

namespace WebApi.Extensions
{
    public static class ReflectionHelper
    {
        public static object InvokeMethodByName<T>(this T obj, string methodName, System.Reflection.BindingFlags bindingFlags, object[] invokeParams, params Type[] genericParams) where T : class
        {
            invokeParams = invokeParams ?? Array.Empty<object>();
            Type[] types = invokeParams.Select(o => o.GetType()).ToArray();
            var getMethod = typeof(T).GetMethod(nameof(methodName),
                bindingFlags,
                Type.DefaultBinder,
                types,
                null);
            if (getMethod == null)
            {
                throw new Exception($"Не найден метод {methodName} в классе {typeof(T).GetTypeName()} c прараметрами '{string.Join(",", types.Select(type => type.GetTypeName()))}'");
            }

            var genericGetMethod = getMethod.MakeGenericMethod(genericParams);
            var resultInvoke = genericGetMethod.Invoke(obj, invokeParams);
            return resultInvoke;
        }
    }
}
