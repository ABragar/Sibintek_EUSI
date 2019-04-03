using Base.DAL;
using Base.Security;
using Base.Service;


namespace Base.Conference.Service
{
    public interface IConferenceService : IBaseObjectService<Entities.Conference>
    {
        bool RenameConference(IUnitOfWork uofw, ISecurityUser currentUser, int conferenceId, string name);
        bool RemoveMember(IUnitOfWork uofw, int currentUserId, int conferenceId, int userToRemoveId);
    }
}