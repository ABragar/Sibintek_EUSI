using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Base.UI.RegisterMnemonics.Entities;
using Base.UI.RegisterMnemonics.Services;
using CorpProp.Services.Document;
using WebUI.Helpers;

namespace WebUI.Controllers
{
    public class AdditionalPropertyController : BaseController
    {
        private readonly IMnemonicItemService<SystemMnemonicItem> _mnemonicItemService;
        private readonly IMnemonicExCrudService<MnemonicEx> _mnemonicExCrudService;

        public AdditionalPropertyController(IBaseControllerServiceFacade serviceFacade,
            IMnemonicItemService<SystemMnemonicItem> mnemonicItemService,
            IMnemonicExCrudService<MnemonicEx> mnemonicExCrudService) : base(serviceFacade)
        {
            _mnemonicItemService = mnemonicItemService;
            _mnemonicExCrudService = mnemonicExCrudService;
        }

        public ActionResult GetToolBar()
        {
            return PartialView("AdditionalPropertyToolbar");
        }

        public ActionResult GetEstateToolBar()
        {
            return PartialView("AdditionalPropertyEstateToolbar");
        }

        public JsonNetResult GetDetailExIdByMnemonic(string mnemonic)
        {
            using (var uow = CreateUnitOfWork())
            {
                var mnenonicItems = _mnemonicItemService.GetAll(uow).Where(item => item.Mnemonic == mnemonic);

                var mnemonicItem = mnenonicItems.FirstOrDefault();


                if (mnemonicItem == null)
                {
                    mnemonicItem = _mnemonicItemService.Create(uow, new SystemMnemonicItem()
                    {
                        Mnemonic = mnemonic,
                        Description = mnemonic
                    });
                    _mnemonicExCrudService.Create(uow, new DeatilViewEx()
                    {
                        MnemonicItemID = mnemonicItem.ID
                    });

                    uow.SaveChanges();
                }

                return new JsonNetResult(new
                {
                    id = mnemonicItem.ID,
                    error = 0,
                });
            }
        }
    }
}