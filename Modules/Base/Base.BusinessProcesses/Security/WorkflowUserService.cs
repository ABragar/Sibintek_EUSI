using Base.DAL;
using Base.Security;
using Base.Security.Entities.Concrete;
using System;
using System.Linq;

namespace Base.BusinessProcesses.Security
{
    public class WorkflowUserService : IWorkflowUserService
    {
        private static ISecurityUser _manager;

        public WorkflowUserService(
            IUnitOfWork unitOfWork)
        {

            if (_manager == null)
            {
                var repository = unitOfWork.GetRepository<User>();

                var user = repository.Find(x => x.SysName == "WorkflowManager");

                if (user == null)
                {
                    //var profileTypes = unitOfWork.GetRepository<ProfileType>();
                    var usersCat = unitOfWork.GetRepository<UserCategory>().All().FirstOrDefault(x => x.ID == 1);

                    if (usersCat == null)
                        throw new Exception("Wf manager without user category");

                    user = new User
                    {
                        SysName = "WorkflowManager",
                        CategoryID = usersCat.ID,
                        Category_ = usersCat,
                    };

                    var profile = new SimpleProfile();

                    profile.LastName = profile.FirstName = profile.MiddleName = "WorkflowManager";
                    // profile.BoType = profileTypes.All().FirstOrDefault();

                    user.Profile = profile;

                    repository.Create(user);

                    unitOfWork.SaveChanges();
                }

                _manager = new SecurityUser(unitOfWork, user,null);
            }
        }

        public ISecurityUser WorkflowManager => _manager;
    }
}

