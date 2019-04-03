using System;

namespace Base.WebApi.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException()
            : base("Ошибка при выполнении запроса")
        {

        }

        public BadRequestException(string message)
            : base(message)
        {

        }

        public virtual object Extended => new { Type = this.GetType().Name };

    }
}