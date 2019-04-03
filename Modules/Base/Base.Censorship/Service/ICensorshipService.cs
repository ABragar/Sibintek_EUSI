using Base.Censorship.Entities;
using Base.Service;
using Base.Settings;

namespace Base.Censorship.Service
{
    public interface ICensorshipService: ISettingService<CensorshipSetting>
    {
        void CheckObsceneLexis(string message);
    }
}