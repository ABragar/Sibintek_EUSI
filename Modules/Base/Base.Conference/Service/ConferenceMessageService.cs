using Base.Conference.Entities;
using Base.DAL;
using Base.Service;

namespace Base.Conference.Service
{
    public class ConferenceMessageService : BaseObjectService<ConferenceMessage>, IConferenceMessageService
    {
        public ConferenceMessageService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        protected override IObjectSaver<ConferenceMessage> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<ConferenceMessage> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver).SaveOneObject(x => x.File).SaveOneObject(m => m.Multimedia);
        }
    }
}