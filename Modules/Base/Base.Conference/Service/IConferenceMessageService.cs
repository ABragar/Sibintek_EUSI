using System.Collections.Generic;
using System.Threading.Tasks;
using Base.Conference.Entities;
using Base.Conference.Models;
using Base.DAL;
using Base.Service;

namespace Base.Conference.Service
{
    public interface IConferenceMessageService : IBaseObjectService<ConferenceMessage>
    {
    }

    public interface ISimpleMessageService : IService
    {
        Task<MessageResult> CreateMessage(IUnitOfWork uofw, ConferenceMessage message, int targetId);
        Task<List<MissedDialog>> GetMissed(IUnitOfWork uofw, int userId);

        Task<List<SimpleMessage>> GetMessages(IUnitOfWork uofw, int userId, int targetId, int maxCount = 50,
            int minCount = 20);

        Task ReadMessages(IUnitOfWork uofw, int userId, int targetId);
    }
}