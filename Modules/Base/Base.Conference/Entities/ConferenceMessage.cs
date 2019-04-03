using Base.Security;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Multimedia.Entities;

namespace Base.Conference.Entities
{
    public enum MessageContentType
    {
        Text = 0,
        File = 1,
        Multimedia = 2,
        Presentation = 3,
        System = 99
    }

    public class ConferenceMessage : BaseObject
    {
        public string TextMessage { get; set; }

        public int? FileID { get; set; }

        [ForeignKey("FileID")]
        public virtual FileData File { get; set; }

        public int? MultimediaID { get; set; }

        [ForeignKey("MultimediaID")]
        public virtual MultimediaObject Multimedia { get; set; }

        public int FromId { get; set; }

        [JsonIgnore]
        [ForeignKey("FromId")]
        public virtual User From { get; set; }

        public DateTime Date { get; set; }

        public DateTime? ReadDate { get; set; }

        public bool IsNew { get; set; }

        public MessageContentType MessageType { get; set; }
    }

    public class PrivateMessage : ConferenceMessage
    {
        public PrivateMessage() { }

        public PrivateMessage(ConferenceMessage message)
        {
            TextMessage = message.TextMessage;
            FromId = message.FromId;
            Date = message.Date;
            ReadDate = message.ReadDate;
            IsNew = message.IsNew;
            MessageType = message.MessageType;
            File = message.File;
            FileID = message.FileID;
            MultimediaID = message.MultimediaID;
            Multimedia = message.Multimedia;
        }

        public int? ToUserId { get; set; }

        [JsonIgnore]
        [ForeignKey("ToUserId")]
        public virtual User ToUser { get; set; }
        
    }

    public class PublicMessage : ConferenceMessage
    {
        public PublicMessage() { }

        public PublicMessage(ConferenceMessage message)
        {
            TextMessage = message.TextMessage;
            FromId = message.FromId;
            Date = message.Date;
            ReadDate = message.ReadDate;
            IsNew = message.IsNew;
            MessageType = message.MessageType;
            File = message.File;
            FileID = message.FileID;
            MultimediaID = message.MultimediaID;
            Multimedia = message.Multimedia;
        }

        public int? ToConferenceId { get; set; }

        [JsonIgnore]
        [InverseProperty("Messages")]
        [ForeignKey("ToConferenceId")]
        public virtual Conference ToConference { get; set; }
    }
}
