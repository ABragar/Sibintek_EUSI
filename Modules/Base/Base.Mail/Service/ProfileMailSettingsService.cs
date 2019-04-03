using Base.DAL;
using Base.Mail.Entities;
using Base.Service;

namespace Base.Mail.Service
{
    public class ProfileMailSettingsService : BaseObjectService<ProfileMailSettings>, IProfileMailSettingsService
    {

        public override ProfileMailSettings Create(IUnitOfWork unitOfWork, ProfileMailSettings obj)
        {
            //obj.AccountPassword = PasswordCryptographer.Encrypt(obj.AccountPassword);
            return base.Create(unitOfWork, obj);
        }

        public override ProfileMailSettings Update(IUnitOfWork unitOfWork, ProfileMailSettings obj)
        {
            //obj.AccountPassword = PasswordCryptographer.Encrypt(obj.AccountPassword);
            return base.Update(unitOfWork, obj);
        }

        public override ProfileMailSettings Get(IUnitOfWork unitOfWork, int id)
        {
            var obj = base.Get(unitOfWork, id);
            //obj.AccountPassword = PasswordCryptographer.Decrypt(obj.AccountPassword);
            return obj;
        }

        public ProfileMailSettingsService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }
    }
}
