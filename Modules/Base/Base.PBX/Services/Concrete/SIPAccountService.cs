using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using Base.DAL;
using Base.PBX.Entities;
using Base.PBX.Models;
using Base.PBX.Services.Abstract;
using Base.Security;
using Base.Service;

namespace Base.PBX.Services.Concrete
{
    public class SIPAccountService: BaseObjectService<SIPAccount>, ISIPAccountService
    {
        private readonly IAutoMapperCloner _autoMapperCloner;
        private readonly IPBXUserService _pbxUserService;
        private readonly IBaseObjectService<PBXServer> _pbxServerService;
        private readonly IUserService<User> _userService;

        private readonly Regex PWDExp = new Regex(@"^(?=.*[A-Z])(?=.*\d)(?!.*(.)\1\1)[a-zA-Z0-9@]{6,12}$");
        private readonly Regex VoicePWDExp = new Regex(@"^\d{5,}$");

        public SIPAccountService(IBaseObjectServiceFacade facade, IAutoMapperCloner autoMapperCloner, IPBXUserService pbxUserService, IBaseObjectService<PBXServer> pbxServerService, IUserService<User> userService) : base(facade)
        {
            _autoMapperCloner = autoMapperCloner;
            _pbxUserService = pbxUserService;
            _pbxServerService = pbxServerService;
            _userService = userService;
        }

        protected override IObjectSaver<SIPAccount> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<SIPAccount> objectSaver)
        {
            var oldObj = objectSaver.Original;
            var newObj = objectSaver.Src;

            if (!PWDExp.Match(newObj.user_password).Success)
                throw new ValidationException("Недопустимый пароль пользователя");

            if (!PWDExp.Match(newObj.secret).Success)
                throw new ValidationException("Недопустимый пароль SIP/IAX");

            if (!VoicePWDExp.Match(newObj.vmsecret).Success)
                throw new ValidationException("Недопустимый пароль голосовой почты");

            //if (GetAll(unitOfWork).Any(x => x.UserID == newObj.UserID))
            //    throw new ValidationException("К данному пользователю уже привязан номер телефона");

            if (objectSaver.IsNew)
                newObj.User = _userService.Get(unitOfWork, newObj.User.ID);

            var pbxUser = _autoMapperCloner.Copy<SIPAccount, PBXUser>(newObj);

            var pbxServer = _pbxServerService.Get(unitOfWork, newObj.PBXServer.ID);

            if (objectSaver.IsNew)
            {
                if (GetAll(unitOfWork).Any(x => x.UserID == newObj.User.ID))
                    throw new ValidationException("Данный пользователь уже привязан к почте.");

                var createdUser = _pbxUserService.CreateUser(pbxServer, pbxUser);
                objectSaver.Dest.user_id = createdUser.user_id;
                objectSaver.Dest.extension = createdUser.extension;
            }
            else
            {
                if (oldObj.UserID != newObj.User.ID)
                {
                    if (GetAll(unitOfWork).Any(x => x.UserID == newObj.User.ID))
                        throw new ValidationException("Данный пользователь уже привязан к почте.");
                }

                if (oldObj.extension != newObj.extension)
                {
                    var number = Int32.Parse(newObj.extension);

                    if (number < pbxServer.MinNumber)
                        throw new ValidationException("Указанный добавочный номер не является допустимым для данного сервера.");

                    if (_pbxUserService.IsNumberExist(pbxServer, number))
                        throw new ValidationException("Данный номер уже зарегестрирован в системе.");

                    DeletePBXUser(oldObj);

                    var createdUser = _pbxUserService.CreateUser(pbxServer, pbxUser);
                    objectSaver.Dest.user_id = createdUser.user_id;
                    objectSaver.Dest.extension = createdUser.extension;
                }
                else
                {
                    _pbxUserService.UpdateUser(pbxServer, pbxUser);
                }
            }

            return base.GetForSave(unitOfWork, objectSaver).SaveOneObject(x => x.User).SaveOneObject(x => x.PBXServer);
        }

        public override void Delete(IUnitOfWork unitOfWork, SIPAccount obj)
        {
            DeletePBXUser(obj);

            base.Delete(unitOfWork, obj);
        }

        public override SIPAccount CreateDefault(IUnitOfWork unitOfWork)
        {
            return base.CreateDefault(unitOfWork);
        }

        private void UpdatePBXUser(SIPAccount obj)
        {
            if (obj.User == null || obj.PBXServer == null) return;

            try
            {
                var number = Int32.Parse(obj.extension);

                if (number < obj.PBXServer.MinNumber)
                    throw new ValidationException("Указанный добавочный номер не является допустимым для данного сервера.");

                if (_pbxUserService.IsNumberExist(obj.PBXServer, number))
                    throw new ValidationException("Данный номер уже зарегестрирован в системе.");
            }
            catch (FormatException)
            {
                throw new ValidationException("Неправильный формат добавочного номера!");
            }

            var pbxUser = _autoMapperCloner.Copy<SIPAccount, PBXUser>(obj);

            if (pbxUser.user_id == 0)
            {
                var createdUser = _pbxUserService.CreateUser(obj.PBXServer, pbxUser);
                obj.user_id = createdUser.user_id;
                obj.extension = createdUser.extension;
            }
            else
            {
                _pbxUserService.UpdateUser(obj.PBXServer, pbxUser);
            }
        }

        private void DeletePBXUser(SIPAccount obj)
        {
            if (obj.User == null || obj.PBXServer == null) return;

            if (!string.IsNullOrWhiteSpace(obj.extension))
            {
                _pbxUserService.DeleteUser(obj.PBXServer, obj.extension);
            }
        }
    }
}