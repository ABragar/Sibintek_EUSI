using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;
using Base.UI;
using Base.UI.Presets;
using Base.Utils.Common.Caching;
using CorpProp.Services.Preset;
using EUSI.DefaultData;

namespace EUSI.Services
{
    public class EUSIPresetService: SibMenuPresetService
    {
        public EUSIPresetService(IExtendedCacheWrapper cacheWrapper, IPresetFactory<MenuPreset> presetFactory, IUnitOfWorkFactory unitOfWorkFactory) : base(cacheWrapper, presetFactory, unitOfWorkFactory)
        {

        }

        public override async Task<MenuPreset> GetAsync(string ownerName)
        {
            var menuPreset = await base.GetAsync(ownerName);
            return menuPreset;
        }
    }
}
