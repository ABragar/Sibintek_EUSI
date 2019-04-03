using System.Threading.Tasks;
using System.Web.Mvc;
using Base.Extensions;
using Base.PBX.Entities;
using Base.PBX.Models;
using Base.PBX.Services.Abstract;
using Base.Service.Crud;
using Kendo.Mvc.UI;
using WebUI.Helpers;
using System.Linq;

namespace WebUI.Controllers
{
    public class PBXController : BaseController
    {
        private readonly IPBXUserService _pbxService;
        private readonly ISIPAccountService _sipAccountService;

        public PBXController(IBaseControllerServiceFacade serviceFacade, IPBXUserService pbxService, ISIPAccountService sipAccountService) : base(serviceFacade)
        {
            _pbxService = pbxService;
            _sipAccountService = sipAccountService;
        }

        public ActionResult GetPBXServerToolbar(int? serverID)
        {
            return PartialView("ServerToolbar", serverID);
        }

        public ActionResult GetUsersListBuilder(int serverId)
        {
            return PartialView("UsersListBuilder", serverId);
        }

        public ActionResult GetSIPAccountToolbar(int? serverID, string number)
        {
            ViewBag.number = number;
            return PartialView("SIPAccountToolbar", serverID);
        }

        public async Task<JsonNetResult> GetUsers([DataSourceRequest] DataSourceRequest request, int serverId)
        {
            var service = GetService<IBaseObjectCrudService>("PBXServer");

            using (var uofw = CreateSystemUnitOfWork())
            {
                var server = service.Get(uofw, serverId);

                if (server != null)
                {
                    var result = await _pbxService.GetPagedUsersAsync((IPBXServer)server, request.Page, request.PageSize);

                    if (result != null)
                    {
                        return new JsonNetResult(new DataSourceResult()
                        {
                            Data = result.Result,
                            Total = result.Total,
                        });
                    }
                }
            }

            return new JsonNetResult(new DataSourceResult()
            {
                Errors = new[] { "No data" }
            });
        }

        public async Task<JsonNetResult> GetUser(int serverId, string number)
        {
            var service = GetService<IBaseObjectCrudService>("PBXServer");

            using (var uofw = CreateSystemUnitOfWork())
            {
                var server = service.Get(uofw, serverId);

                if (server != null)
                {
                    var result = await _pbxService.GetUserAsync((IPBXServer)server, number);
                    return new JsonNetResult(result);
                }
            }

            return new JsonNetResult(null);
        }

        [HttpPost]
        public async Task<JsonNetResult> SaveUser(int serverId, PBXUser user)
        {
            var service = GetService<IBaseObjectCrudService>("PBXServer");

            using (var uofw = CreateSystemUnitOfWork())
            {
                var server = service.Get(uofw, serverId);

                if (server != null)
                {
                    if (user.user_id != 0)
                    {
                        await _pbxService.UpdateUserAsync((IPBXServer)server, user);
                    }
                    else
                    {
                        await _pbxService.CreateUserAsync((IPBXServer)server, user);
                    }

                    return new JsonNetResult(new { user, status = 200 });
                }
            }

            return new JsonNetResult(null);
        }

        [HttpPost]
        public async Task<JsonNetResult> DeleteUser(int serverId, string number)
        {
            var service = GetService<IBaseObjectCrudService>("PBXServer");

            using (var uofw = CreateSystemUnitOfWork())
            {
                var server = service.Get(uofw, serverId);

                if (server != null)
                {
                    await _pbxService.DeleteUserAsync((IPBXServer)server, number);
                    return new JsonNetResult(new { status = 200 });
                }
            }

            return new JsonNetResult(null);
        }

        public async Task<JsonNetResult> GetServerStatus(int serverId)
        {
            PBXServerStatus status = null;

            var service = GetService<IBaseObjectCrudService>("PBXServer");

            using (var uofw = CreateSystemUnitOfWork())
            {
                var server = service.Get(uofw, serverId);

                if (server != null)
                {
                    status = await _pbxService.GetServerStatusAsync((IPBXServer)server);
                }
            }

            return new JsonNetResult(status);
        }

        public async Task<JsonNetResult> ApplyChanges(int serverId)
        {
            var service = GetService<IBaseObjectCrudService>("PBXServer");

            using (var uofw = CreateSystemUnitOfWork())
            {
                var server = service.Get(uofw, serverId);

                if (server != null)
                {
                    await _pbxService.ApplyChangesAsync((IPBXServer)server);

                    return new JsonNetResult(new { status = 200 });
                }
            }

            return new JsonNetResult(null);
        }

        public async Task<JsonNetResult> ApplyAllChanges()
        {
            var service = GetService<IBaseObjectCrudService>("PBXServer");

            using (var uofw = CreateSystemUnitOfWork())
            {
                var query = service.GetAll(uofw);

                if (await query.AnyAsync())
                {
                    var servers = await query.ToListAsync();

                    foreach (var server in servers)
                    {
                        await _pbxService.ApplyChangesAsync((IPBXServer)server);
                    }
                }

                return new JsonNetResult(new { status = 200 });
            }
        }

        public async Task<JsonNetResult> Reboot(int serverId)
        {
            var service = GetService<IBaseObjectCrudService>("PBXServer");

            using (var uofw = CreateSystemUnitOfWork())
            {
                var server = service.Get(uofw, serverId);

                if (server != null)
                {
                    await _pbxService.RebootAsync((IPBXServer)server);

                    return new JsonNetResult(new { status = 200 });
                }
            }

            return new JsonNetResult(null);
        }

        public async Task<JsonNetResult> GetSipAccountByUserId(int userId)
        {
            using (var uofw = CreateUnitOfWork())
            {
                int sipAccountId = await _sipAccountService.GetAll(uofw)
                    .Where(x => x.UserID == userId)
                    .Select(x => x.UserID)
                    .FirstOrDefaultAsync();
                
                if(sipAccountId == 0)
                    return new JsonNetResult(null);

                var config = GetViewModelConfig(typeof(SIPAccount));

                var model = config.DetailView.GetData(uofw, _sipAccountService, sipAccountId);

                return new JsonNetResult(model);

            }
        }
    }
}