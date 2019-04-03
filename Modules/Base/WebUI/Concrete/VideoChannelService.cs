using System;
using System.Collections.Generic;
using System.Linq;
using Base.Conference.Entities;
using Base.Security;
using WebUI.Models;

namespace WebUI.Concrete
{
    public class VideoChannelService
    {
        public VideoChannelService()
        {
            Channels = new List<VideoChannel>();
        }

        public List<VideoChannel> Channels { get; set; }

        public VideoChannel Get(int dialogId, string dialogType)
        {
            return Channels.FirstOrDefault(x => x.DialogId == dialogId && x.DialogType == dialogType);
        }

        public VideoChannel Get(string key)
        {
            return Channels.FirstOrDefault(x => x.ChannelKey == key);
        }

        public VideoChannel AddChannel(int dialogId, string dialogType, string key)
        {
            var channel = Get(dialogId, dialogType);
            if (channel != null)
            {
                channel.ChannelKey = key;
                return channel;
            }

            var videoChannel = new VideoChannel()
            {
                DialogId = dialogId,
                DialogType = dialogType,
                ChannelKey = key
            };

            Channels.Add(videoChannel);
            return videoChannel;
        }

        public VideoChannel AddChannel(VideoChannel videoChannel)
        {
            var channel = Get(videoChannel.DialogId, videoChannel.DialogType);
            if (channel != null)
            {
                channel.ChannelKey = videoChannel.ChannelKey;
                return channel;
            }

            Channels.Add(videoChannel);
            return videoChannel;
        }

        public VideoChannel RemoveChannel(VideoChannel videoChannel)
        {
            Channels.Remove(videoChannel);
            return videoChannel;
        }

        public VideoChannel RemoveChannel(int dialogId, string dialogType)
        {
            var channel = Get(dialogId, dialogType);

            if (channel != null)
            {
                Channels.Remove(channel);
                return channel;
            }

            return null;
        }

        public VideoChannel RemoveChannel(string key)
        {
            var channel = Get(key);

            if (channel != null)
            {
                Channels.Remove(channel);
                return channel;
            }

            return null;
        }

        public VideoRequest CreateRequest(User fromUser, User toUser, string dialogType, string key = "", int conferenceId = 0, string conferenceTitle = "")
        {
            key = string.IsNullOrEmpty(key) ? Guid.NewGuid().ToString("N") : key;

            return new VideoRequest()
            {
                DialogType = dialogType,
                FromId = fromUser.ID,
                FromTitle = fromUser.FullName,
                FromImage = fromUser.Image != null ? fromUser.Image.FileID.ToString() : "",

                ToId = toUser.ID,
                ToTitle = toUser.FullName,
                ToImage = toUser.Image != null ? toUser.Image.FileID.ToString() : "",
                Key = key,
                ConferenceId = conferenceId,
                ConferenceTitle = conferenceTitle
            };
        }

        public List<VideoRequest> CreateRequests(User fromUser, Conference toConference, string dialogType)
        {
            var videoRequests = new List<VideoRequest>();

            if (toConference != null && toConference.Members.Any(x => x.ObjectID != null && x.ObjectID.Value != fromUser.ID))
                videoRequests.AddRange(toConference.Members.Select(conferenceMember => CreateRequest(fromUser, conferenceMember.Object, dialogType, toConference.ConferenceKey.ToString(), toConference.ID, toConference.Title)));

            return videoRequests;
        }

    }

}