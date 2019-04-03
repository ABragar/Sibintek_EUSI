using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Base.Service;
using Base.UI;
using Base.UI.RegisterMnemonics.Entities;
using Base.UI.RegisterMnemonics.Services;

namespace CorpProp.Services.Estate
{
    public class AdditionalPropertyService: MnemonicExCrudService<DeatilViewEx>
    {
        public AdditionalPropertyService(IBaseObjectServiceFacade facade, IViewModelConfigService viewModelConfig, IMnemonicExtensionService mnemonicExtensionService, IMnemonicErrorDescriber mnemonicErrorDescriber) : base(facade, viewModelConfig, mnemonicExtensionService, mnemonicErrorDescriber)
        {
        }

    }
}
