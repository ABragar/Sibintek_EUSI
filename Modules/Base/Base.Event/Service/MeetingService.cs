using System.Linq;
using Base.Contact.Service.Abstract;
using Base.DAL;
using Base.Event.Entities;
using Base.Notification.Service.Abstract;
using Base.Security;
using Base.Service;
using Base.UI;

namespace Base.Event.Service
{
    public class MeetingService : EventService<Meeting>
    {
        protected readonly IEmployeeUserService _employeeUserService;
        public MeetingService(IBaseObjectServiceFacade facade,
            IUserService<User> userService,
            IEmployeeUserService employeeUserService,
            IUnitOfWorkFactory factory,
            IViewModelConfigService configService,
            INotificationService notificationService) :
            base(facade, userService, factory, configService, notificationService)
        {
            _employeeUserService = employeeUserService;
        }

        protected override IObjectSaver<Meeting> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<Meeting> objectSaver)
        {
            if (objectSaver.Dest.Responsible != null)
            {
                var employee =
                    _employeeUserService.GetAll(unitOfWork)
                        .FirstOrDefault(x => x.UserID == objectSaver.Dest.Responsible.ID);
                var department = employee?.Department;
                if (department != null)
                {
                    if (objectSaver.Dest.Location?.Address == null)
                    {
                        objectSaver.Dest.Location = department.DepartmentLocation;
                    }
                }
            }

            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneToMany(x => x.Participants, x => x.SaveOneObject(y => y.Object))
                .SaveOneObject(x => x.Responsible);
        }
    }
}