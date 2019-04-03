using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Web.Mvc;
using Base.DAL;
using Base.Service;
using Base.UI.Filter;
using Base.UI.Service.Abstract;
using Kendo.Mvc.Extensions;
using WebUI.Concrete;
using WebUI.Helpers;

namespace WebUI.Controllers
{
    public class QueryBuilderFilterController : BaseController
    {
        private readonly QueryBuilderFilterService _queryBuilderFilterService;
        private readonly IMnemonicFilterService<MnemonicFilter> _mnemonicFilterService;
        public QueryBuilderFilterController(IBaseControllerServiceFacade serviceFacade, QueryBuilderFilterService queryBuilderFilterService, IMnemonicFilterService<MnemonicFilter> mnemonicFilterService) : base(serviceFacade)
        {
            _queryBuilderFilterService = queryBuilderFilterService;
            _mnemonicFilterService = mnemonicFilterService;
        }


        public JsonNetResult GetMnemonicFilters(string mnemonic, int? selectedInPresetID)
        {
            IUnitOfWork uofw = CreateSystemUnitOfWork();
            var selectedFilter = _mnemonicFilterService.GetAll(uofw).SingleOrDefault(x => x.ID == selectedInPresetID);

            var selectedElem = selectedFilter != null ? new
            {
                ID = selectedFilter.ID,
                Title = selectedFilter.Title,
            } : null;

            var mnemonicFilters = _mnemonicFilterService.GetAll(uofw)
                .Where(x => x.Mnemonic == mnemonic && x.ID != selectedInPresetID)
                .Select(x => new
                {
                    ID = x.ID,
                    Title = x.Title,
                }).Take(20).ToList();
            if (selectedElem != null)
            {
                mnemonicFilters.Add(selectedElem);
            }
                            
            return new JsonNetResult(mnemonicFilters);
        }

        //возвращает коллекцию правил для querybuilder
        public JsonNetResult GetFilters(string mnemonic)
        {
            return new JsonNetResult(new
            {
                Data = _queryBuilderFilterService.GetFilters(mnemonic)
            });
        }

        public JsonNetResult VerifyQuery(string mnemonic, string query)
        {
            var serv = this.GetService<IQueryService<object>>(mnemonic);
            bool wasVerified = true;
            if (!string.IsNullOrEmpty(query))
            {
                using (var uofw = this.CreateUnitOfWork())
                {
                    try
                    {
                        IQueryable q = serv.GetAll(uofw).Where(query);
                    }
                    catch (ParseException e)
                    {
                        wasVerified = false;
                    }
                }
            }
            return new JsonNetResult(new
            {
                wasVerified
            });
        }

        public ActionResult BuilderForm()
        {
            return PartialView();
        }
    }
}