using System;

namespace Base.Access.Service
{
    public class AccessErrorDescriber: IAccessErrorDescriber
    {
        public string GetAccessDenied(Type type)
        {
            return $"Отказано в доступе на тип [{type.Name}]";
        }
    }
}