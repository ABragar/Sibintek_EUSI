using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Base.CRM.Entities;
using Base.CRM.Services.Abstract;
using Base.CRM.UI.Presets;
using Base.Document.Services.Abstract;
using Base.Service;
using Base.UI;
using Base.UI.Service;
using WebUI.Helpers;

namespace WebUI.Controllers
{
    public class CrmController : BaseController
    {
        private readonly IBaseDocumentService<Deal> _dealsService;
        private readonly IBaseObjectService<DealStatus> _documentStatusService;

        private readonly IPresetService<SalesFunnelPreset> _salesFunnelPresetService;

        public CrmController(IBaseControllerServiceFacade serviceFacade, IBaseDocumentService<Deal> dealsService, BaseObjectService<DealStatus> documentStatusService, IPresetService<SalesFunnelPreset> salesFunnelPresetService) : base(serviceFacade)
        {
            _dealsService = dealsService;
            _documentStatusService = documentStatusService;
            _salesFunnelPresetService = salesFunnelPresetService;
            _salesFunnelPresetService = salesFunnelPresetService;
        }




        //get Mnemonics for IDeal mnemonics
        public ActionResult GetIDealMnemonics()
        {
            var configs = GetViewModelConfigs();

            return JsonNet(
                configs
                    .Select(o => new
                    {
                        ID = o.Mnemonic,
                        Text = o.Title,
                        IsShown = o.TypeEntity.GetInterfaces().Contains(typeof(IDeal))
                    }).Where(o => o.IsShown));
        }

      //  [HttpGet]
        public async Task<JsonNetResult> GetSalesFunnel()
        {
            using (var uofw = CreateUnitOfWork())
            {
                try
                {
                    var preset = await _salesFunnelPresetService.GetAsync("WidgetSalesFunnel");

                    var dealService = GetService<IDealService>(preset.EntityMnemonic);

                    List<SalesFunnel> data = null;

                    if (dealService != null)
                        data = dealService.GetSalesFunnel(preset);

                    return new JsonNetResult(data ?? new List<SalesFunnel>());
                }
                catch (Exception e)
                {
                    return new JsonNetResult(new { error = e.Message });
                }
            };
        }



    }
}