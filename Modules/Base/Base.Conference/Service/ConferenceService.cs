using System;
using System.Linq;
using Base.DAL;
using Base.Security;
using Base.Service;

namespace Base.Conference.Service
{
    public class ConferenceService : BaseObjectService<Entities.Conference>, IConferenceService
    {
        public ConferenceService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        protected override IObjectSaver<Entities.Conference> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<Entities.Conference> objectSaver)
        {
            objectSaver.SaveOneToMany(x => x.Members, saver => saver.SaveOneObject(x => x.Object)).SaveOneObject(x => x.Creator);

            return base.GetForSave(unitOfWork, objectSaver);
        }

        public override Entities.Conference Create(IUnitOfWork unitOfWork, Entities.Conference obj)
        {
            obj.ConferenceKey = Guid.NewGuid();
            return base.Create(unitOfWork, obj);
        }

        public bool RenameConference(IUnitOfWork uofw, ISecurityUser currentUser, int conferenceId, string name)
        {
            if (uofw == null || currentUser == null || string.IsNullOrWhiteSpace(name))
                return false;

            var conference = Get(uofw, conferenceId);

            if (conference == null || conference.CreatorId != currentUser.ID)
                return false;

            if (conference.Title != name)
            {
                conference.Title = name;

                Update(uofw, conference);

                uofw.SaveChanges();
            }

            return true;
        }

        public bool RemoveMember(IUnitOfWork uofw, int currentUserId, int conferenceId, int userToRemoveId)
        {
            if (uofw == null)
                return false;

            var conference = Get(uofw, conferenceId);

            // УДАЛЯТЬ ДРУГИХ ПОЛЬЗОВАТЕЛЕЙ МОЖЕТ ТОЛЬКО СОЗДАТЕЛЬ
            if (conference == null || conference.CreatorId != currentUserId && currentUserId != userToRemoveId || conference.Members.All(x => x.ObjectID != userToRemoveId))
                return false;

            conference.Members = conference.Members.Where(x => x.ObjectID != userToRemoveId).ToList();

            Update(uofw, conference);

            uofw.SaveChanges();

            return true;
        }
    }
}