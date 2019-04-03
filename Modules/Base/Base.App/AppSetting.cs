using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Attributes;
using Base.Entities.Complex;
using Base.EntityFrameworkTypes.Complex;
using Base.Settings;

namespace Base.Entities
{
    [Serializable]
    public class AppSetting : SettingItem
    {
        [DetailView("Название системы")]
        public string AppName { get; set; }

        public int? LogoLogInID { get; set; }
        [DetailView("Логотип Вход")]
        [Image(Width = 500, Height = 500)]
        public virtual FileData LogoLogIn { get; set; }

        public int? LogoID { get; set; }
        [DetailView("Логотип")]
        [Image(Circle = true)]
        public virtual FileData Logo { get; set; }
        
        public int? DashboardImageID { get; set; }
        [Image(Crop = false)]
        [DetailView("Фон рабочего стола")]
        public virtual FileData DashboardImage { get; set; }

        [DetailView("Приветствионное сообщение")]
        public string WelcomeMessage { get; set; }

        [PropertyDataType("MapPosition")]
        [DetailView(TabName = "[1]Центрирование карты")]
        public MapPosition MapPosition { get; set; } = new MapPosition();
    }
}
