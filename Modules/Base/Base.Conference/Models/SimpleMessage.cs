using System;
using Base.Conference.Entities;
using Base.Multimedia.Models;

namespace Base.Conference.Models
{
    public class SimpleMessage
    {

        public SimpleMessage() { }

        public SimpleMessage(PrivateMessage message)
        {
            BaseInit(message);
            ToId = message.ToUserId;
            ToImageId = message.ToUser != null && message.ToUser.Image != null
                ? message.ToUser.Image.FileID
                : Guid.Empty;
        }

        public SimpleMessage(PublicMessage message)
        {
            BaseInit(message);
            ToId = message.ToConferenceId;
            ToImageId = Guid.Empty;
        }

        internal void BaseInit(ConferenceMessage message)
        {
            ID = message.ID;
            Date = message.Date;
            MultimediaId = message.MultimediaID;
            MultimediaType = message.MultimediaID.HasValue ? message.Multimedia.Type : MultimediaType.Unknown;
            File = message.File;
            FromId = message.FromId;
            FromImageId = message.From != null && message.From.Image != null ? message.From.Image.FileID : Guid.Empty;
            IsNew = message.IsNew;
            ReadDate = message.ReadDate;
            TextMessage = message.TextMessage;
            MessageType = message.MessageType;
        }

        public int ID { get; set; }

        public string TextMessage { get; set; }

        public int? MultimediaId { get; set; }

        public MultimediaType MultimediaType { get; set; }

        public FileData File { get; set; }

        public int FromId { get; set; }

        public Guid? FromImageId { get; set; }

        public DateTime Date { get; set; }

        public DateTime? ReadDate { get; set; }

        public bool IsNew { get; set; }

        public MessageContentType MessageType { get; set; }

        public int? ToId { get; set; }

        public Guid? ToImageId { get; set; }

        //public string MessageDialogType { get; set; }

    }
}
