using Base;
using Base.DAL;
using Base.Security;
using Base.Security.Service;
using Base.Security.Service.Abstract;
using Base.Service;
using Base.Utils.Common;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebUI.Helpers;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUserService<User> _userService;
        private readonly IBaseProfileService _baseProfileService;
        private readonly IUserStatusService _userStatusService;
        private readonly IAccessUserService _accessUserService;
        private readonly IUserCategoryService _userCategoryService;

        public UsersController(
            IBaseControllerServiceFacade serviceFacade,
            IUserService<User> userService,
            IUserStatusService userStatusService,
            IAccessUserService accessUserService,
            IUserCategoryService userCategoryService, IBaseProfileService baseProfileService)
            : base(serviceFacade)
        {
            _userService = userService;

            _userStatusService = userStatusService;
            _accessUserService = accessUserService;
            _userCategoryService = userCategoryService;
            _baseProfileService = baseProfileService;
        }

        public async Task<ActionResult> Get(int id)
        {
            using (var uof = this.CreateUnitOfWork())
            {
                var user = await _userService.GetAsync(uof, id);

                if (user != null)
                {
                    return new JsonNetResult(new
                    {
                        model = new
                        {
                            user.ID,
                            user.FullName,
                            ImageID = user.Image?.FileID ?? Guid.Empty,
                            Email = user.Profile?.GetPrimaryEmail(),
                            Phones = user.Profile?.Phones.Select(x => new { x.Phone.Type, x.Phone.Code, x.Phone.Number }),
                        }
                    });
                }

                return new JsonNetResult(new
                {
                    error = "Пользователь не найден"
                });
            }
        }

        public JsonNetResult ResolveOnline(IEnumerable<int> userIds)
        {
            if (userIds == null)
            {
                return new JsonNetResult(new int[0]);
            }

            userIds = userIds.Distinct();

            var result = _userStatusService.GetUserStatuses()
                .Where(status => userIds.Contains(status.UserId))
                .Select(status => status.GetPublicVersion())
                .Select(status => new
                {
                    ID = status.UserId,
                    Status = status.CustomStatus
                });

            return new JsonNetResult(result);
        }

        public JsonNetResult KendoUI_CollectionRead([DataSourceRequest] DataSourceRequest request, string searchStr,
            string extrafilter)
        {
            try
            {
                using (var uofw = this.CreateUnitOfWork())
                {
                    var q =
                        _userService.GetAll(uofw);
                    q = q.FullTextSearch(searchStr, this.CacheWrapper);
                    var onlines = _userStatusService.GetOnlineIds();
                    var retval = q.Select(
                        x =>
                            new
                            {
                                x.ID,
                                x.FullName,
                                x.SortOrder,
                                x.Hidden,
                                x.Image,
                                CustomStatus = onlines.Contains(x.ID) ? x.CustomStatus : CustomStatus.Disconnected,
                            });

                    if (!string.IsNullOrWhiteSpace(extrafilter))
                        retval = retval.Where(extrafilter);
                    return new JsonNetResult(retval.ToDataSourceResult(request));
                }
            }
            catch (Exception e)
            {
                var res = new DataSourceResult()
                {
                    Errors = e.Message
                };

                return new JsonNetResult(res);
            }
        }

        public ActionResult ChooseProfile()
        {
            using (var uofw = CreateUnitOfWork())
            {
                int count = _userCategoryService.GetAccessibleCategories(uofw).Count();

                switch (count)
                {
                    case 0:
                        throw new Exception("Нет доступных категорий");
                    default:
                        return View("~/Views/Security/ChooseProfile.cshtml", new BaseViewModel(this));
                }
            }
        }

        [HttpPost]
        public ActionResult ChangeMyProfile(string mnemonic, int userCategoryID, BaseObject model)
        {
            try
            {
                using (var uofw = CreateTransactionUnitOfWork())
                {
                    _accessUserService.ChangeCategory(uofw, SecurityUser.ID, userCategoryID, (BaseProfile)model);
                    uofw.Commit();

                    return new JsonNetResult(new
                    {
                        success = "Профиль успешно сохранен"
                    });
                }
            }
            catch (Exception e)
            {
                return new JsonNetResult(new
                {
                    error = $"Ошибка сохранения профиля: {e.Message}"
                });
            }
        }

        public JsonNetResult GetAccessibleProfiles()
        {
            using (var uofw = CreateUnitOfWork())
            {
                var userCategories = _userCategoryService.GetAccessibleCategories(uofw).ToList();

                if (!userCategories.Any())
                    return new JsonNetResult(new { error = "" });

                var profileTypes = userCategories.Select(x => new
                {
                    x.ID,
                    Mnemonic = x.ProfileMnemonic,
                    x.Name //GetViewModelConfig(x)?.Title
                });

                return new JsonNetResult(new
                {
                    Data = profileTypes
                });
            }
        }
    }
}