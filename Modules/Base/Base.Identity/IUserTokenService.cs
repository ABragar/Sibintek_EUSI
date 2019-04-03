using System;
using System.Linq;
using Base.DAL;
using Base.Identity.Entities;
using Base.Identity.OAuth2;
using Base.Security;
using Base.Security.Service;
using Base.Service;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security;

namespace Base.Identity
{
    public interface IUserTokenService : IBaseObjectService<UserToken>
    {
        User GetUserByToken(IUnitOfWork unitOfWork, string token);
    }

    public class UserTokenService : BaseObjectService<UserToken>, IUserTokenService
    {
        private readonly ISecureDataFormat<AuthenticationTicket> _format;
        private readonly ISystemClock _clock;
        private readonly string _identity_type = "token";

        public UserTokenService(IBaseObjectServiceFacade facade, TicketFormatService ticket_format_service) : base(facade)
        {
            _format = ticket_format_service.GetAesTicketFormat("l;sdl;sdkl;sdl;asdfkl;asasdada");
            _clock = ticket_format_service.SystemClock;
        }

        public override UserToken CreateDefault(IUnitOfWork unitOfWork)
        {
            return new UserToken()
            {
                StartDate = DateTime.Now,
                EndDate = new DateTime(2099, 12, 31)
            };
        }

        public User GetUserByToken(IUnitOfWork unitOfWork, string token)
        {
            var userToken = this.GetAll(unitOfWork).SingleOrDefault(x => x.Token == token);

            if (userToken?.User == null)
                return null;

            if (unitOfWork.GetRepository<User>().Find(x => !x.Hidden && x.ID == userToken.User.ID) == null)
                return null;

            var unprotectToken = _format.Unprotect(token);

            if (unprotectToken.Properties.ExpiresUtc < _clock.UtcNow)
                return null;

            return userToken.User;
        }

        public override UserToken Create(IUnitOfWork unitOfWork, UserToken obj)
        {
            if (obj.User == null)
                throw new ArgumentException("Не указан пользователь");

            if (this.GetAll(unitOfWork).Any(x => x.UserID == obj.User.ID))
                throw new ArgumentException("Для данного пользователя уже создан токен");

            if (obj.EndDate <= obj.StartDate)
                throw new ArgumentException("Дата окончания действия токена не должны быть меньше или равна дате выдачи");

            if (string.IsNullOrEmpty(obj.Reason))
                throw new ArgumentException("Не указана причина выдачи токена");

            var difMinutes = (obj.EndDate - obj.StartDate).TotalMinutes;

            var identity = IdentityHelper.CreateIdentity(obj.User.ID, null, _identity_type);

            var ticket = new AuthenticationTicket(identity, new AuthenticationProperties());

            var date = _clock.UtcNow;

            ticket.Properties.IssuedUtc = date;
            ticket.Properties.ExpiresUtc = date.Add(TimeSpan.FromMinutes(difMinutes));

            obj.Token = _format.Protect(ticket);

            return base.Create(unitOfWork, obj);
        }

        public override UserToken Get(IUnitOfWork unitOfWork, int id)
        {
            var token = base.Get(unitOfWork, id);

            return token;
        }

        public override UserToken Update(IUnitOfWork unitOfWork, UserToken obj)
        {
            throw new NotImplementedException("Данный объект изменять нельзя");
        }
    }
}