using System;
using CorpProp.Entities.Import;
using CorpProp.Entities.Settings;

namespace CorpProp.Services.Settings
{
    public class EstateRegistrationNotificationStrategy : DefaultNotificationStrategy
    {
        protected override string GetSuccessCode()
        {
            return string.Empty;
            //throw new Exception("EstateRegistration не поддерживает шаблон нотификации для успешного импорта");
        }

        protected override string GetFailCode()
        {
            return $"EstateRegistration{base.GetFailCode()}";
        }
    }
}