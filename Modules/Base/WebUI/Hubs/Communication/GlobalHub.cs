using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Base;
using Base.Conference.Models;
using Base.Conference.Service;
using Base.Extensions;
using Base.Multimedia.Models;
using Base.Security;
using Base.Service.Crud;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR.Hubs;
using WebUI.Concrete;
using WebUI.Models;

namespace WebUI.Hubs
{
    [HubName("globalHub")]
    public class GlobalHub : BaseHub
    {
        private readonly IBroadcaster _broadcaster;
        
        public GlobalHub(ConferenceHubFactory conferenceHubFactory, IBroadcaster broadcaster)
            : base(conferenceHubFactory)
        {
            _broadcaster = broadcaster;
            _broadcaster.OnUpdateCounters += BroadcasterOnUpdateCounters;
        }

        private void BroadcasterOnUpdateCounters(object sender, BroadcasterEventArgs args)
        {            
            string type = args.Entity.FullName;

            if (args.User == null)
            {
                Clients.All.UpdateCounters(type);
                return;
            }

            var member = HubFactory.UserStatusService.GetUserStatus(args.User.ID);
            if (member != null)
            {
                Clients.Client(member.ConnectionId).UpdateCounters(type);
            }
        }

        #region Helpers
        private async Task _sendMessage(IEnumerable<int> targets, SimpleMessage message, string dialogType)
        {
            var result = ToStringResult(message);

            foreach (var target in targets)
            {
                var member = HubFactory.UserStatusService.GetUserStatus(target);
                if (member != null)
                    await Clients.Client(member.ConnectionId).OnTextMessageSend(result, dialogType);
            }
        }

        private async Task _sendVideoRequest(int targetId, VideoRequest videoRequest)
        {
            var targetConnection = HubFactory.UserStatusService.GetUserStatus(targetId);
            if (targetConnection != null)
            {
                videoRequest.Self = targetConnection.ConnectionId == Context.ConnectionId;
                await Clients.Client(targetConnection.ConnectionId).OnSendVideoRequest(videoRequest);
            }
        }

        private async Task _sendCancelRequest(int targetId, VideoRequest videoRequest)
        {
            var targetConnection = HubFactory.UserStatusService.GetUserStatus(targetId);
            if (targetConnection != null)
                await Clients.Client(targetConnection.ConnectionId).OnCallCancel(videoRequest);
        }

        private async Task _sendSuccessRequest(int targetId, VideoRequest videoRequest)
        {
            var targetConnection = HubFactory.UserStatusService.GetUserStatus(targetId);
            if (targetConnection != null)
                await Clients.Client(targetConnection.ConnectionId).OnCallSuccess(videoRequest);
        }

        private void _sendNotification(string connectionId, string message, NotificationType notificationType = NotificationType.Success)
        {
            if (connectionId != null)
                Clients.Client(connectionId).OnServerNotification(notificationType, message);
        }

        private void _sendNotification(int userId, string message, NotificationType notificationType = NotificationType.Success)
        {
            var targetConnection = HubFactory.UserStatusService.GetUserStatus(userId);
            _sendNotification(targetConnection.ConnectionId, message, notificationType);
        }

        private void _sendNotification(IEnumerable<int> userIds, string message,
            NotificationType notificationType = NotificationType.Success)
        {
            if (userIds == null) return;

            foreach (var userId in userIds)
            {
                _sendNotification(userId, message, notificationType);
            }
        }

        #endregion

        #region SingIn\Statuses update
        public async Task SignIn()
        {
            using (var uow = CreateSystemUnitOfWork())
            {
                int id = Context.User.Identity.GetUserId<int>();
                var user = await HubFactory.UserService.GetAsync(uow, id);

                if (user == null)
                    return;

                var userStatus = HubFactory.UserStatusService.SetOnline(user.ID, Context.ConnectionId);

                await Clients.Caller.OnSignedIn();
                await Clients.Caller.OnSignIn(userStatus);

                if (userStatus.CustomStatus != CustomStatus.Disconnected)
                {
                    await Clients.Others.OnSignIn(userStatus.GetPublicVersion());
                }
            }
        }

        public async Task ChangeCustomStatus(string status)
        {
            using (var uow = CreateSystemUnitOfWork())
            {
                int id = Context.User.Identity.GetUserId<int>();
                var user = await HubFactory.UserService.GetAsync(uow, id);

                if (string.IsNullOrWhiteSpace(status) || user == null)
                    return;

                using (var uofw = CreateSystemUnitOfWork())
                {
                    CustomStatus customStatus;

                    if (!Enum.TryParse(status, true, out customStatus))
                        return;

                    var userStatus =
                        await HubFactory.UserStatusService.SetCustomStatusAsync(uofw, user.ID, customStatus);

                    await Clients.Caller.OnChangeCustomStatus(userStatus);

                    if (userStatus.CustomStatus == CustomStatus.Disconnected && userStatus.LastCustomStatus.HasValue &&
                        userStatus.LastCustomStatus != CustomStatus.Disconnected)
                    {
                        // LAST STATUS IS NOT "DISCONNECTED" AND NEW STATUS IS "DISCONNECTED"
                        await Clients.Others.OnSignOut(userStatus.GetPublicVersion());
                    }
                    else if (userStatus.CustomStatus != CustomStatus.Disconnected && userStatus.LastCustomStatus.HasValue &&
                             userStatus.LastCustomStatus == CustomStatus.Disconnected)
                    {
                        // LAST STATUS IS "DISCONNECTED" AND NEW STATUS IS NOT "DISCONNECTED"
                        await Clients.Others.OnSignIn(userStatus.GetPublicVersion());
                    }
                    else
                    {
                        await Clients.Others.OnChangeCustomStatus(userStatus.GetPublicVersion());
                    }
                }
            }
        }

        #endregion

        #region WebRTC broadcaster
        public async Task WebRtcSend(string message)
        {
            //await Clients.All.onMessageReceived(message);
            //TODO: Need test
            await Clients.Others.onMessageReceived(message);
        }
        #endregion

        #region Text chat logic

        public async Task SendTextMessage(string dialogType, int dialogId, string message)
        {
            if (string.IsNullOrEmpty(dialogType) || MyStatus == null) return;

            message = HubFactory.ChatService.NormalizeMessage(message);

            if (HubFactory.ChatService.CheckMessage(message))
            {
                using (var uofw = CreateSystemUnitOfWork())
                {
                    var serv = GetService<ISimpleMessageService>(dialogType);

                    if (serv != null)
                    {
                        var messageResult = await HubFactory.ChatService.CreateTextMessage(uofw, serv, message, MyStatus.UserId, dialogId);
                        await _sendMessage(messageResult.Targets, messageResult.Message, dialogType);
                    }
                }
            }
        }

        public async Task SendFileMessage(string dialogType, int dialogId, string filestring)
        {
            if (string.IsNullOrEmpty(dialogType) || MyStatus == null) return;

            var file = ToObjectResult<FileData>(filestring);

            if (file == null)
                return;

            using (var uofw = CreateSystemUnitOfWork())
            {
                var serv = GetService<ISimpleMessageService>(dialogType);

                if (serv != null)
                {
                    var messageResult = await HubFactory.ChatService.CreateFileMessage(uofw, serv, file, MyStatus.UserId, dialogId);

                    await _sendMessage(messageResult.Targets, messageResult.Message, dialogType);
                }
            }
        }

        public async Task SendMultimediaMessage(string dialogType, int dialogId, string stringfiles, MultimediaType type)
        {
            if (string.IsNullOrEmpty(dialogType) || MyStatus == null) return;

            var files = ToObjectResult<List<FileData>>(stringfiles);

            if (files == null || !files.Any())
                return;

            using (var uofw = CreateSystemUnitOfWork())
            {
                var serv = GetService<ISimpleMessageService>(dialogType);

                if (serv != null)
                {
                    var messageResult = await HubFactory.ChatService.CreateMultimediaMessage(uofw, serv, files, type, MyStatus.UserId, dialogId);

                    await _sendMessage(messageResult.Targets, messageResult.Message, dialogType);
                }
            }
        }

        public async Task UpdateMissedMessages()
        {
            if (MyStatus == null)
                return;

            using (var uofw = CreateSystemUnitOfWork())
            {
                var missedPrivate = await HubFactory.PrivateMessageService.GetMissed(uofw, MyStatus.UserId);
                var missedPublic = await HubFactory.PublicMessageService.GetMissed(uofw, MyStatus.UserId);

                await Clients.Caller.OnUpdateMissedMessages(
                    missedPrivate.Concat(missedPublic));
            }
        }

        public void ExcludeConferenceMember(int conferenceId, int userId)
        {
            if (MyStatus == null)
                return;

            using (var uofw = CreateSystemUnitOfWork())
            {
                var conference = HubFactory.ConferenceService.Get(uofw, conferenceId);

                if (conference == null)
                {
                    _sendNotification(MyStatus.ConnectionId, "Конференция была удалена", NotificationType.Error);
                    return;
                }

                var membersStatuses = conference.Members
                    .Select(x => HubFactory.UserStatusService.GetUserStatus(x.ObjectID.Value))
                    .Where(x => x != null);

                bool success = HubFactory.ConferenceService.RemoveMember(uofw, MyStatus.UserId, conferenceId, userId);

                if (success)
                {
                    foreach (var status in membersStatuses)
                    {
                        Clients.Client(status.ConnectionId).OnConferenceMemberExclude(conferenceId, userId);
                    }
                }
                else
                {
                    _sendNotification(MyStatus.ConnectionId, "Вы не являетесь создателем конференции", NotificationType.Error);
                }
            }
        }

        #endregion

        #region Video logic

        public async Task SendVideoRequest(int dialogId, string dialogType, string key = "")
        {
            if (MyStatus == null)
                return;

            using (var uofw = CreateSystemUnitOfWork())
            {
                var fromUser = await HubFactory.UserService.GetAsync(uofw, MyStatus.UserId);

                if (fromUser == null)
                    return;

                switch (dialogType)
                {
                    case "PrivateMessage":
                        var toUser = await HubFactory.UserService.GetAsync(uofw, dialogId);
                        //key = string.IsNullOrEmpty(key) ? Guid.NewGuid().ToString("N") : key;

                        var privateRequest = HubFactory.VideoChannelService.CreateRequest(fromUser, toUser, dialogType, key: key);

                        //Request for target
                        await _sendVideoRequest(privateRequest.ToId, privateRequest);
                        //Request for requestor
                        await _sendVideoRequest(privateRequest.FromId, privateRequest);
                        break;
                    case "PublicMessage":
                        var conference = HubFactory.ConferenceService.Get(uofw, dialogId);
                        var publicRequests = HubFactory.VideoChannelService.CreateRequests(fromUser, conference, dialogType);

                        if (publicRequests.Any())
                        {
                            foreach (var request in publicRequests)
                            {
                                await _sendVideoRequest(request.ToId, request);
                            }
                        }

                        break;
                }
            }
        }

        public async Task CancelCall(VideoRequest videoRequest)
        {
            if (MyStatus != null)
            {
                switch (videoRequest.DialogType)
                {
                    case "PrivateMessage":
                        var targetId = MyStatus.UserId == videoRequest.FromId
                            ? videoRequest.ToId
                            : videoRequest.FromId;

                        await _sendCancelRequest(targetId, videoRequest);
                        break;

                    case "PublicMessage":
                        if (MyStatus.UserId == videoRequest.FromId)
                        {
                            using (var uofw = CreateSystemUnitOfWork())
                            {
                                var targets = await HubFactory.ConferenceService.GetAll(uofw)
                                    .Where(x => x.ID == videoRequest.ConferenceId)
                                    .SelectMany(x => x.Members)
                                    .Where(x => x.ObjectID.HasValue && x.ObjectID != MyStatus.UserId)
                                    .ToListAsync();

                                if (targets.Any())
                                {
                                    foreach (var target in targets)
                                    {
                                        await _sendCancelRequest(target.ObjectID.GetValueOrDefault(), videoRequest);
                                    }
                                }
                            }
                        }
                        break;
                }
            }
        }

        public async Task SuccessCall(VideoRequest videoRequest)
        {
            var callToConnection = HubFactory.UserStatusService.GetUserStatus(Context.ConnectionId);
            var callFromConnection = HubFactory.UserStatusService.GetUserStatus(videoRequest.FromId);

            if (callToConnection != null && callFromConnection != null)
                await _sendSuccessRequest(videoRequest.FromId, videoRequest);
        }

        public async Task MissedCall(VideoRequest videoRequest)
        {
            if (MyStatus != null)
            {
                using (var uofw = CreateSystemUnitOfWork())
                {
                    var dialogId = videoRequest.DialogType == "PrivateMessage"
                        ? videoRequest.ToId
                        : videoRequest.ConferenceId;

                    var serv = GetService<ISimpleMessageService>(videoRequest.DialogType);
                    if (serv != null)
                    {
                        var messageResult =
                            await
                                HubFactory.ChatService.CreateSystemMessage(uofw, serv, $"Вам звонил в: {DateTime.Now}", videoRequest.FromId, dialogId);

                        await _sendMessage(messageResult.Targets, messageResult.Message, videoRequest.DialogType);
                    }
                }
            }
        }

        public async Task StartVideoConference(VideoRequest videoRequest)
        {
            var targetConnection = HubFactory.UserStatusService.GetUserStatus(videoRequest.ToId);

            var dialogId = videoRequest.DialogType == "PrivateMessage" ? videoRequest.ToId : videoRequest.ConferenceId;

            HubFactory.VideoChannelService.AddChannel(new VideoChannel()
            {
                ChannelKey = videoRequest.Key,
                InitiatorId = videoRequest.FromId,
                DialogId = dialogId,
                DialogType = videoRequest.DialogType,
                StartDate = DateTime.Now
            });

            if (MyStatus != null && targetConnection != null)
                await Clients.Client(targetConnection.ConnectionId).OnStartVideoConference(videoRequest);
        }

        //Not working
        public async Task JoinVideoConference(string key, VideoRequest videoRequest)
        {
            var targetConnection = HubFactory.UserStatusService.GetUserStatus(videoRequest.FromId);

            if (MyStatus != null && targetConnection != null)
                await Clients.Client(targetConnection.ConnectionId).OnStartVideoConference(key, videoRequest);
        }

        public async Task EndVideoConference(string key)
        {
            var channel = HubFactory.VideoChannelService.Get(key);

            if (channel != null)
            {
                using (var uofw = CreateSystemUnitOfWork())
                {
                    var serv = GetService<ISimpleMessageService>(channel.DialogType);

                    if (serv != null)
                    {
                        var duration = DateTime.Now - channel.StartDate;
                        string minutes = ((int)duration.TotalMinutes).ToString("D2");
                        string seconds = duration.Seconds.ToString("D2");

                        var messageResult = await HubFactory.ChatService.CreateSystemMessage(uofw, serv, $"Аудио/Видео вызов был завершен. Длительность: {minutes}:{seconds}.", channel.InitiatorId, channel.DialogId, true);
                        await _sendMessage(messageResult.Targets, messageResult.Message, channel.DialogType);
                    }
                }

                HubFactory.VideoChannelService.RemoveChannel(channel);
            }
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        #endregion

        public override async Task ConfirmConnection()
        {
            using (var uow = CreateSystemUnitOfWork())
            {
                var status = HubFactory.UserStatusService.GetUserStatus(Context.ConnectionId);

                if (!status.Online && status.CustomStatus != CustomStatus.Disconnected)
                {
                    status = HubFactory.UserStatusService.SetOnline(status);

                    await Clients.Caller.OnSignedIn();
                    await Clients.Caller.OnSignIn(status);
                    await Clients.Others.OnSignIn(status.GetPublicVersion());
                }
            }

            await base.ConfirmConnection();
        }

        #region base override
        public override async Task OnDisconnected(bool stopCalled)
        {
            if (MyStatus != null && MyStatus.CustomStatus == CustomStatus.InConversation)
            {
                using (var uofw = CreateSystemUnitOfWork())
                {
                    await HubFactory.UserStatusService.SetCustomStatusAsync(uofw, MyStatus, MyStatus.LastCustomStatus ?? CustomStatus.Ready);
                }
            }

            var user = Base.Ambient.AppContext.SecurityUser;

            if (user != null)
            {
                int userID = user.ID;

                HubFactory.UserStatusService.SetOffline(userID);

                DisconnectTaskAsync(userID);

            }
            await base.OnDisconnected(stopCalled);

        }

        private async void DisconnectTaskAsync(int userID)
        {
            await Task.Delay(10000);
            try
            {
                var userStatus = HubFactory.UserStatusService.GetUserStatus(userID);

                if (userStatus != null && !userStatus.Online)
                {
                    Clients.Others.OnSignOut(userStatus.GetPublicVersion());
                }
            }
            catch (Exception)
            {
                //TODO ADD LOGGING
                
            }
            
        }

        #endregion
    }

    internal enum NotificationType
    {
        Error = 1,
        Info = 2,
        Success = 3,
        Chat = 4,
    }
}