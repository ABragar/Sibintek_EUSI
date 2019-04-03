using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Base;
using Base.Attributes;
using CorpProp.Entities.Base;
using Kendo.Mvc.UI;

namespace CorpProp.Entities.Document
{
    public class ViewSettingsByMnemonic: BaseObject
    {
        [SystemProperty]
        [ListView("Мнемоника"), DetailView("Мнемоника")]
        public string Mnemonic { get; set; }

        [SystemProperty]
        [ListView("Мнемоника связанного объекта"), DetailView("Мнемоника связанного объекта")]
        public string MnemonicFor { get; set; }

        [SystemProperty]
        [ListView("Идентификатор выбранного элемента"), DetailView("Идентификатор выбранного элемента")]
        public Guid DefaultSelectedOid { get; set; }
    }
}
