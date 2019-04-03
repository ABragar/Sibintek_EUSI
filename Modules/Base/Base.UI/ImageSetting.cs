using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Attributes;
using Base.Settings;

namespace Base.UI
{
    [Serializable]
    public class ImageSetting : SettingItem
    {
        [DetailView]
        public int XXS { get; set; }

        [DetailView]
        public int XS { get; set; }

        [DetailView]
        public int S { get; set; }

        [DetailView]
        public int M { get; set; }

        [DetailView]
        public int L { get; set; }

        [DetailView]
        public int XL { get; set; }

        [DetailView]
        public int XXL { get; set; }
    }
}
