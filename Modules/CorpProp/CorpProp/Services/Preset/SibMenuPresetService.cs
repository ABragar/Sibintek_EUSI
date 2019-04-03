using System;
using System.CodeDom;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Base;
using Base.App;
using Base.DAL;
using Base.Events;
using Base.Extensions;
using Base.Security;
using Base.UI;
using Base.UI.Presets;
using Base.UI.Service;
using Base.Utils.Common.Caching;
using CorpProp.Common;
using CorpProp.Extentions;
using Kendo.Mvc.UI;
using AppContext = Base.Ambient.AppContext;
using Tasks = System.Threading.Tasks;

namespace CorpProp.Services.Preset
{
    public class SibMenuPresetService : PresetService<MenuPreset>
    {
        private readonly IExtendedCacheWrapper _cacheWrapper;
        private readonly IPresetFactory<MenuPreset> _presetFactory;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        
        public SibMenuPresetService(IExtendedCacheWrapper cacheWrapper, IPresetFactory<MenuPreset> presetFactory, IUnitOfWorkFactory unitOfWorkFactory) : base(cacheWrapper,presetFactory, unitOfWorkFactory)
        {
            _cacheWrapper = cacheWrapper;
            _presetFactory = presetFactory;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        void RemoveByMnemonic(MenuElement menu, ICollection<MenuElement> menuContainer, List<string> mnemonicsList)
        {
            if (menu != null && mnemonicsList.Contains(menu.Mnemonic))
            {
                menuContainer.Remove(menu);
            }
            else
            {
                var subItems = menu == null ? menuContainer : menu.Children;
                if (subItems != null)
                {
                    var subItemsArray = subItems.ToArray();
                    foreach (var menuElement in subItemsArray)
                    {
                        RemoveByMnemonic(menuElement, subItems, mnemonicsList);
                    }
                }
            }
        }

        List<string> DisabledMnemonicOnMenu()
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var cauk = AppContext.SecurityUser.IsFromCauk(uow);
                switch (cauk)
                {
                    case true:
                        return new List<string>() { "Response" };
                    default:
                        return new List<string>() {"Request"};
                }
            }
        }

        public override async Task<MenuPreset> GetAsync(string ownerName)
        {
            var menuPreset = await base.GetAsync(ownerName);
            var menuPresetFiltred = menuPreset.DeepCopy();
            RemoveByMnemonic(null, menuPresetFiltred.MenuElements, DisabledMnemonicOnMenu());
            return menuPresetFiltred;
        }
    }


}
