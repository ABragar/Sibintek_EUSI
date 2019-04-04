using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Base.Conference.Models;
using Base.Conference.Service;
using WebUI.Models;
using Base.Conference.Entities;
using System.Text;
using Base.Extensions;
using Base.Security;
using Base.Service;
using Base.Service.Crud;
using Base.UI.Extensions;
using WebUI.Extensions;
using WebUI.Helpers;
using WebUI.Models.Communication;
using AppContext = Base.Ambient.AppContext;

namespace WebUI.Controllers
{
    public class CommunicationController : BaseController
    {
        private readonly IUserService<User> _userService;
        private readonly IPrivateMessageService _privateMessageService;
        private readonly IConferenceService _conferenceService;

        private const int MAX_MESSAGES = 50;
        private const int MIN_MESSAGES = 20;

        public CommunicationController(IBaseControllerServiceFacade serviceFacade, IUserService<User> userService, IPrivateMessageService privateMessageService, IConferenceService conferenceService) : base(serviceFacade)
        {
            _userService = userService;
            _privateMessageService = privateMessageService;
            _conferenceService = conferenceService;
        }

        [ChildActionOnly]
        public ActionResult Index()
        {
            return PartialView(new BaseViewModel(this));
        }

        public async Task<JsonNetResult> GetDialog(string dialogType, int dialogId)
        {
            if (string.IsNullOrEmpty(dialogType))
                return null;

            using (var uofw = CreateSystemUnitOfWork())
            {
                if (string.Equals(dialogType, "PublicMessage", StringComparison.CurrentCultureIgnoreCase))
                {
                    //TODO: async
                    var dialog = _conferenceService.Get(uofw, dialogId);

                    return dialog != null && dialog.Members.Any(x => x.ObjectID == AppContext.SecurityUser.ID)
                        ? new JsonNetResult(new ChatDialogViewModel(dialog))
                        : null;
                }
                else if (string.Equals(dialogType, "PrivateMessage", StringComparison.CurrentCultureIgnoreCase))
                {
                    var user = await _userService.GetAsync(uofw, dialogId);

                    return user != null
                        ? new JsonNetResult(new ChatDialogViewModel(user))
                        : null;
                }
            }

            return null;
        }

        public async Task<JsonNetResult> GetDialogs(string dialogType = "")
        {
            var userID = AppContext.SecurityUser.ID;

            IQueryable<ChatDialogViewModel> dialogs = null;

            var result = new List<ChatDialogViewModel>();

            using (var uofw = CreateSystemUnitOfWork())
            {
                if (string.IsNullOrEmpty(dialogType) || string.Equals(dialogType, "PublicMessage", StringComparison.CurrentCultureIgnoreCase))
                {
                    dialogs = _conferenceService.GetAll(uofw)
                        .Where(x => /*x.CreatorId == userID || */x.Members.Any(m => m.ObjectID == userID))
                        .Select(x => new ChatDialogViewModel
                        {
                            ID = x.ID,
                            ImageID = Guid.Empty,
                            Title = x.Title,
                            DialogType = ChatDialogType.Conference,
                        });

                    result.AddRange(await dialogs.ToListAsync());
                }

                if (string.IsNullOrEmpty(dialogType) || string.Equals(dialogType, "PrivateMessage", StringComparison.CurrentCultureIgnoreCase))
                {
                    var incomingPrivateDialogs = _privateMessageService.GetAll(uofw)
                        .Where(x => x.ToUserId == userID)
                        .GroupBy(x => x.From)
                        .Select(x => new
                        {
                            ID = x.Key.ID,
                            ImageID = x.Key.Image != null ? x.Key.Image.FileID : Guid.Empty,
                            Title = x.Key.FullName
                        });

                    var outcomingPrivateDialogs = _privateMessageService.GetAll(uofw)
                        .Where(x => x.FromId == userID)
                        .GroupBy(x => x.ToUser)
                        .Select(x => new
                        {
                            ID = x.Key.ID,
                            ImageID = x.Key.Image != null ? x.Key.Image.FileID : Guid.Empty,
                            Title = x.Key.FullName
                        });

                    var privateDialogs = incomingPrivateDialogs.Union(outcomingPrivateDialogs)
                        .Select(x => new ChatDialogViewModel
                        {
                            ID = x.ID,
                            ImageID = x.ImageID,
                            Title = x.Title,
                            DialogType = ChatDialogType.Private,
                        });

                    result.AddRange(await privateDialogs.ToListAsync());
                }

                return new JsonNetResult(result);
            }
        }

        public async Task<JsonNetResult> GetMessages(string dialogType, int dialogId)
        {
            var messages = new List<SimpleMessage>();

            using (var uofw = CreateSystemUnitOfWork())
            {
                var serv = (ISimpleMessageService)GetService<IBaseObjectCrudService>(dialogType);

                if (serv != null)
                {
                    messages = await serv.GetMessages(uofw, AppContext.SecurityUser.ID, dialogId, MAX_MESSAGES, MIN_MESSAGES);
                }
            }

            return new JsonNetResult(messages);
        }

        [HttpPost]
        public async Task<JsonNetResult> ReadMessages(string dialogType, int dialogId)
        {
            bool success = false;

            using (var uofw = CreateSystemUnitOfWork())
            {
                var serv = (ISimpleMessageService)GetService<IBaseObjectCrudService>(dialogType);

                if (serv != null)
                {
                    await serv.ReadMessages(uofw, AppContext.SecurityUser.ID, dialogId);

                    success = true;
                }
            }

            return new JsonNetResult(new { success });
        }

        private string CreateConferenceTitle(IEnumerable<User> members)
        {
            var names = members.Select(x => x.FullName);

            if (!names.Any())
            {
                return "Новая конференция";
            }

            var builder = new StringBuilder();

            foreach (string name in names)
            {
                builder.AppendFormat(", {0}", name);
            }

            return builder.Remove(0, 2).ToString();
        }

        [HttpPost]
        public async Task<JsonNetResult> CreateConference(int[] userIds)
        {
            if (userIds == null)
            {
                return null;
            }

            var ids = userIds
                .Where(x => x != AppContext.SecurityUser.ID)
                .Distinct()
                .ToList();

            using (var uofw = CreateSystemUnitOfWork())
            {
                // IF NO USERS EXCEPT CURRENT
                if (ids.Count == 0)
                {
                    return null;
                }

                // IF SELECTED ONLY ONE USER
                if (ids.Count == 1)
                {
                    var user = await _userService.GetAsync(uofw, ids[0]);

                    return user == null ? null : new JsonNetResult(new ChatDialogViewModel(user));
                }

                var conference = await _conferenceService.GetAll(uofw)
                    .Where(x => x.Members.All(mem => mem.ObjectID == AppContext.SecurityUser.ID || ids.Contains(mem.ObjectID ?? -1)) && x.Members.Count == ids.Count + 1)
                    .FirstOrDefaultAsync();

                if (conference != null)
                {
                    return new JsonNetResult(new ChatDialogViewModel(conference));
                }

                ids.Add(AppContext.SecurityUser.ID);

                var users = (await _userService.GetAllAsync(uofw, ids));

                var title = CreateConferenceTitle(users);

                conference = new Conference
                {
                    CreateDate = DateTime.Now,
                    Creator = users.FirstOrDefault(x => x.ID == AppContext.SecurityUser.ID),
                    Members = users.Select(x => new ConferenceMember
                    {
                        Object = x
                    }).ToList(),
                    Messages = new List<PublicMessage>(),
                    Title = title,
                };

                conference = _conferenceService.Create(uofw, conference);

                return new JsonNetResult(new ChatDialogViewModel(conference));
            }
        }

        [HttpPost]
        public async Task<JsonNetResult> InviteToConference(int conferenceId, int[] userIds)
        {
            // IF THERE ARE NO USERS EXCEPT CURRENT
            if (userIds == null || userIds.All(x => x == AppContext.SecurityUser.ID))
            {
                return null;
            }

            using (var uofw = CreateSystemUnitOfWork())
            {
                var conference = _conferenceService.Get(uofw, conferenceId);

                // CURRENT USER IS NOT A MEMBER OF THIS CONFERENCE OR NOT A CREATOR
                if (conference == null || conference.Members.All(x => x.ObjectID != AppContext.SecurityUser.ID) || conference.CreatorId != AppContext.SecurityUser.ID)
                {
                    return null;
                }

                var ids = userIds.Union(conference.Members.Select(x => (int)x.ObjectID));

                var users = await _userService.GetAllAsync(uofw, ids);

                conference.Members = users.Select(x => new ConferenceMember
                {
                    Object = x
                }).ToList();

                conference = _conferenceService.Update(uofw, conference);

                await uofw.SaveChangesAsync();

                return new JsonNetResult(new ChatDialogViewModel(conference));
            }
        }

        //[HttpPost]
        //public JsonNetResult RenameConference(int conferenceId, string name)
        //{
        //    using (var uofw = CreateSystemUnitOfWork())
        //    {
        //        return new JsonNetResult(new
        //        {
        //            success = _conferenceService.RenameConference(uofw, SecurityUser, conferenceId, name)
        //        });
        //    }
        //}

        //[HttpPost]
        //public JsonNetResult RemoveConferenceMember(int conferenceId, int userId)
        //{
        //    using (var uofw = CreateSystemUnitOfWork())
        //    {
        //        return new JsonNetResult(new
        //        {
        //            success = _conferenceService.RemoveMember(uofw, SecurityUser.ID, conferenceId, userId)
        //        });
        //    }
        //}

        public async Task<JsonNetResult> GetMembers(int conferenceId)
        {
            var members = new List<ChatDialogViewModel>();

            using (var uofw = CreateSystemUnitOfWork())
            {
                members = await _conferenceService.GetAll(uofw)
                    .Where(conf => conf.ID == conferenceId && conf.Members.Any(member => member.ObjectID == AppContext.SecurityUser.ID))
                    .SelectMany(conf => conf.Members)
                    .Select(member => new ChatDialogViewModel
                    {
                        ID = member.ObjectID.HasValue ? member.ObjectID.Value : -1,
                        ImageID = member.Object.Image != null ? member.Object.Image.FileID : Guid.Empty,
                        Title = member.Object.FullName,
                        DialogType = ChatDialogType.Private
                    })
                    .ToListAsync();
            }

            return new JsonNetResult(members);
        }

        public async Task<JsonNetResult> GetMnemonicCounters(string[] mnemonics)
        {
            if (mnemonics == null || mnemonics.Length == 0)
            {
                return null;
            }

            var result = new List<MnemonicCounter>();

            using (var uofw = CreateSystemUnitOfWork())
            {
                foreach (var mnemonic in mnemonics)
                {
                    var config = GetViewModelConfig(mnemonic);
                    var serv = GetService<IQueryService<object>>(mnemonic);
                    string filter = "";

                    if (config.TypeEntity == typeof(Base.Notification.Entities.Notification))
                        filter = "it.Hidden = false and it.Status = \"New\"";

                    if (config == null || serv == null)
                        continue;

                    result.Add(new MnemonicCounter
                    {
                        Mnemonic = mnemonic,
                        Type = config.TypeEntity.FullName,
                        Count = await serv.GetAll(uofw).Filter(this, uofw, config, filter).CountAsync()
                    });
                }
            }

            return new JsonNetResult(result);
        }
    }
}