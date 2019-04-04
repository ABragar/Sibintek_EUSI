using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Base;
using Base.Conference.Entities;
using Base.Conference.Models;
using Base.Conference.Service;
using Base.DAL;
using Base.Multimedia.Entities;
using Base.Multimedia.Models;

namespace WebUI.Concrete
{
    public class ChatService
    {
        //internal const int MAX_MESSAGE_LENGTH = 255;

        public string NormalizeMessage(string message)
        {
            message = message
                .Replace("<", "\x3C")
                .Replace(">", "\x3E")
                .Trim();

            //if (message.Length > MAX_MESSAGE_LENGTH)
            //{
            //    message = message.Substring(0, MAX_MESSAGE_LENGTH);
            //}

            return message;
        }

        public bool CheckMessage(string message)
        {
            var isNormal = !string.IsNullOrWhiteSpace(message);

            return isNormal;
        }

        public async Task<MessageResult> CreateTextMessage(IUnitOfWork uofw, ISimpleMessageService srv, string message, int fromId, int dialogId)
        {
            var baseMessage = new ConferenceMessage()
            {
                Date = DateTime.Now,
                IsNew = true,
                TextMessage = message,
                FromId = fromId
            };

            var messageResult = await srv.CreateMessage(uofw, baseMessage, dialogId);

            return messageResult;
        }

        public async Task<MessageResult> CreateFileMessage(IUnitOfWork uofw, ISimpleMessageService srv, FileData file, int fromId, int dialogId)
        {
            string ext = System.IO.Path.GetExtension(file.FileName);

            var baseMessage = new ConferenceMessage()
            {
                Date = DateTime.Now,
                IsNew = true,
                File = file,
                MessageType = ext != null && ext.EndsWith("pptx") ? MessageContentType.Presentation : MessageContentType.File,
                FromId = fromId
            };

            var messageResult = await srv.CreateMessage(uofw, baseMessage, dialogId);

            return messageResult;
        }

        public async Task<MessageResult> CreateMultimediaMessage(IUnitOfWork uofw, ISimpleMessageService srv, List<FileData> files, MultimediaType type, int fromId, int dialogId)
        {
            var sourceFiles = new List<SourceFile>();

            files.ForEach(file =>
            {
                sourceFiles.Add(new SourceFile()
                {
                    Object = file
                });
            });

            var baseMessage = new ConferenceMessage()
            {
                Date = DateTime.Now,
                IsNew = true,
                Multimedia = new MultimediaObject()
                {
                  SourceFiles = sourceFiles,
                  Type = type,
                },
                MessageType = MessageContentType.Multimedia,
                FromId = fromId
            };

            var messageResult = await srv.CreateMessage(uofw, baseMessage, dialogId);

            return messageResult;
        }

        public async Task<MessageResult> CreateSystemMessage(IUnitOfWork uofw, ISimpleMessageService srv, string message, int fromId, int dialogId, bool readed = false)
        {
            var baseMessage = new ConferenceMessage()
            {
                Date = DateTime.Now,
                IsNew = true,
                TextMessage = message,
                MessageType = MessageContentType.System,
                FromId = fromId,
            };

            if (readed)
            {
                baseMessage.ReadDate = DateTime.Now;
                baseMessage.IsNew = false;
            }

            var messageResult = await srv.CreateMessage(uofw, baseMessage, dialogId);

            return messageResult;
        }


    }

}