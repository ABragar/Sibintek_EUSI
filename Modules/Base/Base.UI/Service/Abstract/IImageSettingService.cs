using Base.Settings;
using Base.UI.Enums;

namespace Base.UI.Service
{
    public interface IImageSettingService: ISettingService<ImageSetting>
    {
        ImageSize GetClosestSize(int? size);
        int GetImageSizeValue(ImageSize size);
    }
}
