using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Base.Conference.Entities;
using Base.Conference.Models;
using Base.DAL;
using Base.Extensions;
using Base.Multimedia.Models;
using Base.Security;
using Base.Service;

namespace Base.Conference.Service
{
    public class PrivateMessageService : BaseObjectService<Entities.PrivateMessage>, IPrivateMessageService
    {
        public PrivateMessageService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        public async Task<MessageResult> CreateMessage(IUnitOfWork uofw, ConferenceMessage message, int targetId)
        {
            var targets = new List<int>();

            var privateMessage = new PrivateMessage(message)
            {
                ToUserId = targetId
            };

            Create(uofw, privateMessage);

            await uofw.SaveChangesAsync();

            var userRepository = uofw.GetRepository<User>();

            privateMessage.From = await userRepository.All().Where(x => x.ID == privateMessage.FromId).FirstOrDefaultAsync();
            privateMessage.ToUser = await userRepository.All().Where(x => x.ID == privateMessage.ToUserId).FirstOrDefaultAsync();

            targets.Add(privateMessage.FromId);
            
            if(privateMessage.ToUserId.HasValue)
                targets.Add(privateMessage.ToUserId.Value);

            return new MessageResult()
            {
                Message = new SimpleMessage(privateMessage),
                Targets = targets
            };
        }

        public async Task<List<MissedDialog>> GetMissed(IUnitOfWork uofw, int userId)
        {
            var missed = await GetAll(uofw)
                .Where(x => x.ToUserId.HasValue && x.ToUserId == userId && x.IsNew)
                .GroupBy(x => x.FromId)
                .Select(x => new MissedDialog()
                {
                    Id = x.Key,
                    DialogName = x.FirstOrDefault().From.FullName,
                    MissedCount = x.Count(),
                    DialogType = "PrivateMessage"
                }).ToListAsync();

            return missed;
        }

        public async Task<List<SimpleMessage>> GetMessages(IUnitOfWork uofw, int userId, int targetId, int maxCount = 50, int minCount = 20)
        {
            var userRepository = uofw.GetRepository<User>();

            var targetUser = userRepository.Find(targetId);

            if (targetUser == null) return null;

            var data = GetAll(uofw)
                .Where(x => (x.FromId == targetId && x.ToUserId == userId) || (x.FromId == userId && x.ToUserId == targetId))
                .OrderByDescending(x => x.ID)
                .Take(maxCount)
                .OrderBy(x => x.ID);

            var count = await data.CountAsync();

            if (count > minCount)
            {
                if (data.Take(count - minCount).Any(x => x.ToUserId == userId && x.IsNew))
                {
                    var firstNew = await data.Where(x => x.ToUserId == userId && x.IsNew).FirstOrDefaultAsync();

                    data = data.Where(x => x.ID >= firstNew.ID).OrderBy(x => x.ID);
                }
                else
                {
                    data = data.Skip(count - minCount).OrderBy(x => x.ID);
                }
            }

            var messages = await data.Select(x => new SimpleMessage
            {
                ID = x.ID,
                Date = x.Date,
                File = x.File,
                FromId = x.FromId,
                FromImageId = x.From != null && x.From.Image != null ? x.From.Image.FileID : Guid.Empty,
                IsNew = x.IsNew,
                ReadDate = x.ReadDate,
                TextMessage = x.TextMessage,
                MessageType = x.MessageType,
                MultimediaId = x.MultimediaID,
                MultimediaType = x.MultimediaID.HasValue ? x.Multimedia.Type : MultimediaType.Unknown,
                ToId = x.ToUserId,
                ToImageId = x.ToUser != null && x.ToUser.Image != null ? x.ToUser.Image.FileID : Guid.Empty
            }).ToListAsync();

            return messages;
        }

        protected override IObjectSaver<PrivateMessage> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<PrivateMessage> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver).SaveOneObject(x => x.File).SaveOneObject(m => m.Multimedia);
        }

        public async Task ReadMessages(IUnitOfWork uofw, int userId, int targetId)
        {
            var now = DateTime.Now;

            var data = await GetAll(uofw)
                .Where(x => x.FromId == targetId && x.ToUserId == userId && x.IsNew)
                .ToListAsync();

            data.ForEach(x =>
            {
                x.IsNew = false;
                x.ReadDate = now;
                Update(uofw, x);
            });

            await uofw.SaveChangesAsync();
        }
    }
}