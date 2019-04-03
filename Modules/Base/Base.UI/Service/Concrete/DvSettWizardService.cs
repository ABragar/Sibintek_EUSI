using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;
using Base.Service;
using Base.UI.DetailViewSetting;
using Base.UI.ViewModal;
using Base.UI.Wizard;

namespace Base.UI.Service
{
    public class DvSettWizardService : BaseWizardService<DvSettWizard, DvSettingForType>, IDvSettWizardService
    {
        public DvSettWizardService(IDvSettingService<DvSettingForType> baseService, IAccessService accessService) : base(baseService, accessService)
        {

        }

    }
}
