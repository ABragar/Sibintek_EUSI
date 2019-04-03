using Base.DAL;
using EUSI.Entities.Estate;

namespace EUSI.Services.Estate.FiasHandlers
{
    public abstract class FiasChainHandler
    {
        protected IUnitOfWork Uofw { get; set; }

        public FiasChainHandler(IUnitOfWork uofw)
        {
            this.Uofw = uofw;
        }

        private FiasChainHandler NextHandler { get; set; }

        public void SetNextChecker(FiasChainHandler nextChecker)
        {
            NextHandler = nextChecker;
        }

        public abstract void Handle(EstateRegistrationRow obj);

        protected void NextHandle(EstateRegistrationRow obj)
        {
            if (NextHandler != null)
            {
                NextHandler.Handle(obj);
            }
        }
    }
}
