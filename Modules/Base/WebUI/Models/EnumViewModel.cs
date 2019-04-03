using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Base.Extensions;
using Base.UI;

namespace WebUI.Models
{
    public class EnumViewModel<T> where T : struct, IConvertible
    {
        public EnumViewModel(UiEnumValue val)
        {
            if (val == null)
                throw new ArgumentNullException(nameof(val));

            if (!typeof(T).IsEnum)            
                throw new ArgumentException("T must be an enumerated type");
            
            T value;
            Enum.TryParse(val.Value, out value);

            Value = value;
            Title = val.Title;
            Color = val.Icon.Color;
            Icon = val.Icon.Value;
        }

        public EnumViewModel(T val)
        {
            var enumVal = (val as Enum);
            if (enumVal == null)
                throw new Exception("Value is not Enum");

            Value = val;
            Title = enumVal.GetTitle();
            Color = enumVal.GetColor();
            Icon = enumVal.GetIcon();
        }

        public string Title { get;  }
        public string Color { get;  }
        public string Icon { get;  }
        public T Value { get;  }

    }
}