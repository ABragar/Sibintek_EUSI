using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;
using Base.Service;
using Base.UI.Filter;
using Base.UI.Presets;
using Base.UI.QueryFilter;

namespace Base.UI.Service.Concrete
{
    public class UsersMnemonicFilterService : MnemonicFilterService<UsersMnemonicFilter>
    {
        public UsersMnemonicFilterService(IBaseObjectServiceFacade facade, IPresetService<GridPreset> presetService,
            IUnitOfWorkFactory unitOfWorkFactory, IQueryTreeFilter filter_service) : base(facade, presetService, unitOfWorkFactory, filter_service)
        {
        }

        protected override IObjectSaver<UsersMnemonicFilter> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<UsersMnemonicFilter> objectSaver)
        {
            objectSaver.Dest.UserID = Base.Ambient.AppContext.SecurityUser.ID;
            return base.GetForSave(unitOfWork, objectSaver);
        }
    }
}
