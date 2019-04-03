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
    public class PublicMessageService : BaseObjectService<Entities.PublicMessage>, IPublicMessageService
    {
        public PublicMessageService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        public async Task<MessageResult> CreateMessage(IUnitOfWork uofw, ConferenceMessage message, int targetId)
        {
            var result = new List<int>();

            var publicMessage = new PublicMessage(message)
            {
                ToConferenceId = targetId
            };

            Create(uofw, publicMessage);

            await uofw.SaveChangesAsync();

            var conferenceRepository = uofw.GetRepository<Entities.Conference>();
            var userRepository = uofw.GetRepository<User>();

            publicMessage.From = await userRepository.All().Where(x => x.ID == publicMessage.FromId).FirstOrDefaultAsync();
            publicMessage.ToConference = await conferenceRepository.All().Where(x => x.ID == publicMessage.ToConferenceId).FirstOrDefaultAsync();


            if (publicMessage.ToConference != null && publicMessage.ToConference.Members.Any())
            {
                result.AddRange(publicMessage.ToConference.Members.Where(x => x.ObjectID.HasValue).Select(conferenceMember => conferenceMember.ObjectID.Value));
            }

            return new MessageResult()
            {
                Message = new SimpleMessage(publicMessage),
                Targets = result
            };
        }

        public async Task<List<MissedDialog>> GetMissed(IUnitOfWork uofw, int userId)
        {
            var missed = await GetAll(uofw)
                .Where(x => x.FromId != userId && x.ToConferenceId.HasValue && x.ToConference.Members.Select(y => y.ObjectID).Contains(userId) && x.IsNew)
                .GroupBy(x => x.ToConferenceId).Select(x => new MissedDialog()
                {
                    Id = x.Key.Value,
                    DialogName = x.FirstOrDefault().ToConference.Title,
                    MissedCount = x.Count(),
                    DialogType = "PublicMessage"
                }).ToListAsync();

            return missed;
        }

        public async Task<List<SimpleMessage>> GetMessages(IUnitOfWork uofw, int userId, int targetId, int maxCount = 50, int minCount = 20)
        {
            var conferenceRepository = uofw.GetRepository<Entities.Conference>();

            var conference = conferenceRepository.Find(targetId);

            if (conference == null) return null;

            // IF THAT USER IS NOT IN CONFERENCE MEMBERS LIST
            if (conference.Members.All(x => x.ObjectID != userId))
            {
                return new List<SimpleMessage>();
            }

            var data = GetAll(uofw).Where(x => x.ToConferenceId == targetId)
                .OrderByDescending(x => x.ID)
                .Take(maxCount)
                .OrderBy(x => x.ID);

            var count = await data.CountAsync();

            if (count > minCount)
            {
                if (data.Take(count - minCount).Any(x => x.FromId != userId && x.IsNew))
                {
                    var firstNew = await data.Where(x => x.FromId != userId && x.IsNew).FirstOrDefaultAsync();

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
                ToId = x.ToConferenceId,
                ToImageId = Guid.Empty
            }).ToListAsync();

            return messages;
        }

        public async Task ReadMessages(IUnitOfWork uofw, int userId, int targetId)
        {
            var now = DateTime.Now;

            var data = await GetAll(uofw)
                .Where(x => x.FromId != userId && x.ToConferenceId == targetId && x.IsNew)
                .ToListAsync();

            data.ForEach(x =>
            {
                x.IsNew = false;
                x.ReadDate = now;
                Update(uofw, x);
            });

            await uofw.SaveChangesAsync();
        }

        protected override IObjectSaver<PublicMessage> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<PublicMessage> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver).SaveOneObject(x => x.File).SaveOneObject(m => m.Multimedia);
        }
    }
}