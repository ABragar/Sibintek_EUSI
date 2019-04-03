using System;

namespace Base.Exceptions
{
    public class ActivationException : Exception
    {
        public ActivationException(Type type, Exception inner) : base($"Ошибка активации {type.Name}", inner)
        {

        }
    }
}