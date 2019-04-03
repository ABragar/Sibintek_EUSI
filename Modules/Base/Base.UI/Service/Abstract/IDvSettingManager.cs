using System;
using System.Collections.Generic;
using Base.DAL;
using Base.UI.DetailViewSetting;

namespace Base.UI.Service
{
    public interface IDvSettingManager
    {
        DvSetting GetSettingForMnemonic(IUnitOfWork unitOfWork, string mnemonic);
        bool HasSettingsForType(IUnitOfWork unitOfWork, Type type);
        IEnumerable<DvSetting> GetSettingsForType(IUnitOfWork unitOfWork, Type type, BaseObject obj);
    }
}
