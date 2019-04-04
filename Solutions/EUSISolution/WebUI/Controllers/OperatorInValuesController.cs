using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Base.DAL;
using Base.Extensions;
using Base.Security;
using Base.Security.Service;
using Base.Service;
using Base.UI.Filter;
using CorpProp.Entities.Access;
using WebUI.Helpers;

namespace WebUI.Controllers
{
    public class OperatorInValuesController : BaseController
    {

        public OperatorInValuesController(IBaseControllerServiceFacade serviceFacade) : base(serviceFacade)
        {
        }

        public ActionResult GetMultipleValuesEditorButton()
        {
            return PartialView("OperatorInValuesToolbar");
        }

        [HttpPost]
        public ActionResult SetValues(Guid idForValue, Guid mnemonicFilterOid, ICollection<string> pavaluesram)
        {
            try
            {
                var counter = 0;
                using (var uow = CreateUnitOfWork())
                {                  
                    var repo = uow.GetRepository<OperatorInValues>();
                    repo.AutoDetectChangesEnabled = false;
                    repo.ValidateOnSaveEnabled = false;
                    
                    foreach (var value in pavaluesram)
                    {
                        var model = repo.Create(new OperatorInValues
                        {
                            IdForValue = idForValue,
                            MnemonicFilterOid = mnemonicFilterOid,
                            Value = value
                        });

                        counter++;
                        if (counter % 1000 == 0 && counter != 0)
                        {
                            uow.SaveChanges();
                        }
                    }
                    uow.SaveChanges();
                }
                return Json(new {NumberOfValues = counter});
            }
            catch (Exception e)
            {
                return Json(new {error = e.Message});
            }
        }
    }
}