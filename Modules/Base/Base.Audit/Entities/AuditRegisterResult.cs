using Base.Attributes;
using Base.Enums;
using Base.Events.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Audit.Entities
{
    [ViewModelConfig(Title = "Cобытие регистрации", IsReadOnly = true)]
    public class AuditRegisterResult : BaseObject, IRegisterResult
    {
        public AuditRegisterResult(string login, RegisterStatus status, int? userId = null)
        {
            Login = login;
            Status = status;
            UserId = userId;
        }


        [DetailView("Логин пользователя", ReadOnly = true, Required = true, Order = 1)]
        public string Login { get; private set; }

        [DetailView("Результат регистрации в системе", ReadOnly = true, Required = true, Order = 2)]
        public RegisterStatus Status { get; private set; }

        public int? UserId { get; private set; }
    }
}
