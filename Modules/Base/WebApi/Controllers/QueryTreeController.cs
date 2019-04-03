using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Base.DAL;
using Base.Extensions;
using Base.UI;
using Base.UI.Filter;
using Base.UI.QueryFilter;
using Base.UI.Service.Abstract;
using WebApi.Attributes;


namespace WebApi.Controllers
{
    [CheckSecurityUser]
    [RoutePrefix("querytree")]
    internal class QueryTreeController : ApiController
    {
        private readonly IQueryTreeFilter _queryTreeFilterService;
        private readonly IQueryTreeService _queryTreeService;
        private readonly IViewModelConfigService _modelConfigService;
        private readonly IMnemonicFilterService<MnemonicFilter> _mnemonicFilterService;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public QueryTreeController(IQueryTreeFilter queryTreeFilterService,
            IMnemonicFilterService<MnemonicFilter> mnemonicFilterService, IUnitOfWorkFactory unitOfWorkFactory,
            IViewModelConfigService modelConfigService, IQueryTreeService queryTreeService)
        {
            _queryTreeFilterService = queryTreeFilterService;
            _mnemonicFilterService = mnemonicFilterService;
            _unitOfWorkFactory = unitOfWorkFactory;
            _modelConfigService = modelConfigService;
            _queryTreeService = queryTreeService;
        }

        [Route("{mnemonic}")]
        public QueryTreeViewModel Get(string mnemonic)
        {
            return _queryTreeFilterService.GetFilter(mnemonic);
        }

        [Route("getAggregatableProperties/{mnemonic}")]
        public IEnumerable<QueryTreeItemViewModel> GetAggregatableProperties(string mnemonic)
        {
            return _queryTreeService.GetAggregatableProperties(mnemonic);
        }

        [HttpGet]
        [Route("filtersByMnemonic/{mnemonic}")]
        public async Task<IHttpActionResult> FiltersByMnemonic(string mnemonic, int? selectedId = null)
        {

            using (var uofw = _unitOfWorkFactory.CreateSystem())
            {
                try
                {
                    var res = await _mnemonicFilterService.GetAll(uofw)
                        .Where(x => x.Mnemonic == mnemonic)
                        .Select(x => new
                    {
                        x.ID,
                        x.Title,
                        SysOrder = x.ID == selectedId ? 0 : 1
                    }).OrderBy(x => x.SysOrder).ThenBy(x => x.Title).Take(20).ToListAsync();

                    return Ok(res);

                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
        }

    }
}