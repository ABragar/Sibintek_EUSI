using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;
using Base.UI.Service;

namespace Base.UI
{
    public static class ImageSettingInitializer
    {
        public static void Seed(IUnitOfWork unitOfWork, IImageSettingService imageSettingService)
        {
            imageSettingService.Create(unitOfWork, new ImageSetting()
            {
                Title = "Настройки изображений",
                XXS = 45,
                XS = 90,
                S = 250,
                M = 450,
                L = 960,
                XL = 1200,
                XXL = 1920
            });
        }
    }

}
