using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Helpers;
using Base.Service;
using Base.Settings;
using Base.UI.Enums;
using Base.Utils.Common.Caching;

namespace Base.UI.Service
{
    public class ImageSettingService : SettingService<ImageSetting>, IImageSettingService
    {
        public ImageSettingService(IBaseObjectServiceFacade facade, ISimpleCacheWrapper cache_wrapper, IHelperJsonConverter json_converter) : base(facade, cache_wrapper, json_converter)
        {
        }

        public ImageSize GetClosestSize(int? size)
        {
            var setting = this.Get();

            if (size == null)
            {
                return ImageSize.M;
            }

            if (size <= setting.XXS)
            {
                return ImageSize.XXS;
            }
            else if (setting.XXS < size && size <= setting.XS)
            {
                return ImageSize.XS;
            }
            else if (setting.XS < size && size <= setting.S)
            {
                return ImageSize.S;
            }
            else if (setting.S < size && size <= setting.M)
            {
                return ImageSize.M;
            }
            else if (setting.M < size && size <= setting.L)
            {
                return ImageSize.L;
            }
            else if (setting.L < size && size <= setting.XL)
            {
                return ImageSize.XL;
            }
            else
            {
                return ImageSize.XXL;
            }
        }

        public int GetImageSizeValue(ImageSize size)
        {
            var setting = this.Get();
            switch (size)
            {
                case ImageSize.XXS:
                    return setting.XXS;
                case ImageSize.XS:
                    return setting.XS;
                case ImageSize.S:
                    return setting.S;
                case ImageSize.M:
                default:
                    return setting.M;
                case ImageSize.L:
                    return setting.L;
                case ImageSize.XL:
                    return setting.XL;
                case ImageSize.XXL:
                    return setting.XXL;
            }
        }

    }
}
