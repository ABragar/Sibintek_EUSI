using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Base;
using Base.DAL;
using Base.Extensions;
using Base.Service;
using Base.Service.Log;
using Base.UI;
using WebApi.Attributes;

namespace WebApi.Controllers
{
    [CheckSecurityUser]
    [RoutePrefix("hierarchy/{mnemonic}")]
    public class HierarchyController: BaseApiController
    {
        private readonly ILogService _logger;

        public HierarchyController(IViewModelConfigService viewModelConfigService, IUnitOfWorkFactory unitOfWorkFactory, ILogService logger) : base(viewModelConfigService, unitOfWorkFactory, logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("parents/{id}")]
        [GenericAction("mnemonic")]
        public async Task<IHttpActionResult> Parents<T>(string mnemonic, int id)
            where T : HCategory
        {
            try
            {
                using (var uofw = CreateUnitOfWork())
                {
                    var config = GetConfig();

                    var serv = config.GetService<IBaseCategoryService<T>>();

                    var ids = await serv.GetAll(uofw).Where(x => x.ID == id).Select(x => x.sys_all_parents).SingleOrDefaultAsync();

                    return Ok(new
                    {
                        ids = ids?.Split(HCategory.Seperator).Select(HCategory.IdToInt)
                    });
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }


        }

        [HttpPost]
        [Route("changePosition/{id}")]
        [GenericAction("mnemonic")]
        public IHttpActionResult ChangePosition<T>(string mnemonic, int id, int? parentId = null, int? posChangeId = null, string typePosChange = null)
            where T : HCategory
        {
            try
            {
                using (var uofw = this.CreateTransactionUnitOfWork())
                {
                    var config = GetConfig();
                    var serv = config.GetService<IBaseCategoryService<T>>();
                    var obj = serv.Get(uofw, id);

                    serv.ChangePosition(uofw, obj, posChangeId, typePosChange);

                    uofw.Commit();

                    return Ok();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
    }
}
