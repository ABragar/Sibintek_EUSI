using System.Collections.Generic;
using System.Linq;
using Base.DAL;
using Base.Service;
using Base.UI.DetailViewSetting;
using Base.UI.ViewModal;

namespace Base.UI.Service
{
    public interface IDvSettingService<T> : IBaseObjectService<T> where T : DvSetting
    {
        IQueryable<T> GetDvSettings(IUnitOfWork uow, string objectType);

        ICollection<DvSettingValidationResult> ValidateSetting(DvSettingForType setting);
    }
}
