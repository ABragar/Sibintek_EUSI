using System;
using Base.Attributes;
using Base.Security;

namespace Base.Identity.Entities
{
    public class UserToken : BaseObject
    {
        public int UserID { get; set; }

        [DetailView("Пользователь", ReadOnly = true, Required = true), ListView]
        public virtual User User { get; set; }

        [DetailView("Дата и время выдачи токена", ReadOnly = true)]
        public DateTime StartDate { get; set; }

        [DetailView("Дата и время окончания токена", Required = true, ReadOnly = true)]
        public DateTime EndDate { get; set; }

        [DetailView("Причина выдачи токена", ReadOnly = true, Required = true)]
        public string Reason { get; set; }

        [DetailView("Токен", ReadOnly = true)]
        public string Token { get; set; }
    }
}