using Base;
using Base.Audit.Services;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Services.Abstract;
using Base.Conference.Entities;
using Base.Contact.Service.Abstract;
using Base.Event.Entities;
using Base.Events;
using Base.Events.Auth;
using Base.Events.Registration;
using Base.Links.Service.Abstract;
using Base.Map;
using Base.Map.Clustering;
using Base.Map.Services;
using Base.Notification.Entities;
using Base.Security;
using Base.Security.Entities.Concrete;
using Base.Security.ObjectAccess;
using Base.Security.Service;
using Base.Security.Service.Abstract;
using Base.Service;
using Base.UI;
using Base.UI.Presets;
using Base.UI.Service;
using CorpProp.Entities.Base;
using CorpProp.Entities.Law;
using CorpProp.Services.Base;
using CorpProp.Services.Law;
using SimpleInjector;

namespace WebUI.Concrete
{
    public class EventBus : BaseEventBus
    {
        public EventBus(IServiceLocator locator) : base(locator)
        {
            RegisterHandler<IAuditItemService, IOnCreate<BaseObject>>();
            RegisterHandler<IAuditItemService, IOnUpdate<BaseObject>>();
            RegisterHandler<IAuditItemService, IOnDelete<BaseObject>>();

            #region Auth Events
            RegisterHandler<IAuditItemAuthService, IOnLogOn<IAuthResult>>();
            RegisterHandler<IAuditItemAuthService, IOnLogOff<IAuthResult>>();
            RegisterHandler<IAuditItemAuthService, IOnLogOnError<IAuthResult>>();
            RegisterHandler<IAuditItemAuthService, IOnAccountRegistered<IRegisterResult>>();
            #endregion

            RegisterHandler<IWorkflowService, IOnCreate<IBPObject>>();
            RegisterHandler<IWorkflowService, IOnDelete<IBPObject>>();
            //Register<IWorkflowService, IOnUpdate<IBPObject>>(); // сейчас там обработчик пустой

            RegisterHandler<ISecurityService, IOnCreate<IAccessibleObject>>(x => x.Modified is BaseObject);
            RegisterHandler<ISecurityService, IOnUpdate<IAccessibleObject>>(x => x.Modified is BaseObject);
            RegisterHandler<ISecurityService, IOnDelete<IAccessibleObject>>(x => x.Modified is BaseObject);

            RegisterHandler<ISecurityUserService, IChangeObjectEvent<SimpleProfile>>();
            RegisterHandler<ISecurityUserService, IChangeObjectEvent<User>>();
            RegisterHandler<ISecurityUserService, IChangeObjectEvent<UserCategory>>();

            RegisterHandler<ISecurityUserService, IChangeObjectEvent<Role>>();

            RegisterHandler<IEventNotificationService, ChangeObjectEvent<Event>>();

            RegisterHandler<IEmployeeUserService, IChangeObjectEvent<SimpleProfile>>();
            RegisterHandler<IEmployeeUserService, IChangeObjectEvent<User>>();

            RegisterHandler<ILinkItemService, IOnUpdate<BaseObject>>();
            RegisterHandler<ILinkItemService, IOnDelete<BaseObject>>();

            RegisterHandler<IClusterCacheManager, IChangeObjectEvent<IGeoObject>>();

            RegisterHandler<IPresetService<MenuPreset>, IChangeObjectEvent<IUserCategory>>();
            RegisterHandler<IPresetService<DashboardPreset>, IChangeObjectEvent<IUserCategory>>();
            RegisterHandler<IPresetService<QuickAccessBarPreset>, IChangeObjectEvent<IUserCategory>>();

            RegisterHandler<IPresetService<MenuPreset>, IChangeObjectEvent<PresetRegistor>>();
            RegisterHandler<IPresetService<DashboardPreset>, IChangeObjectEvent<PresetRegistor>>();
            RegisterHandler<IPresetService<QuickAccessBarPreset>, IChangeObjectEvent<PresetRegistor>>();

            #region BROADCASTER COUNTERS

            RegisterHandler<BroadcasterHandler, OnCreate<PrivateMessage>>();
            RegisterHandler<BroadcasterHandler, OnUpdate<PrivateMessage>>();
            RegisterHandler<BroadcasterHandler, OnCreate<PublicMessage>>();
            RegisterHandler<BroadcasterHandler, OnUpdate<PublicMessage>>();
            RegisterHandler<BroadcasterHandler, ChangeObjectEvent<Notification>>();

            #endregion BROADCASTER COUNTERS

            #region CorpProp
          
            RegisterHandler<ITypeObjectService<TypeObject>, IOnCreate<TypeObject>>();
            RegisterHandler<ITypeObjectService<TypeObject>, IOnUpdate<TypeObject>>();
            RegisterHandler<ITypeObjectService<TypeObject>, IOnDelete<TypeObject>>();

            RegisterHandler<IScheduleStateRegistrationRecordService, IOnCreate<ScheduleStateRegistrationRecord>>();
            RegisterHandler<IScheduleStateRegistrationRecordService, IOnUpdate<ScheduleStateRegistrationRecord>>();
            RegisterHandler<IScheduleStateRegistrationRecordService, IOnDelete<ScheduleStateRegistrationRecord>>();

            RegisterHandler<IScheduleStateTerminateRecordService, IOnCreate<ScheduleStateTerminateRecord>>();
            RegisterHandler<IScheduleStateTerminateRecordService, IOnUpdate<ScheduleStateTerminateRecord>>();
            RegisterHandler<IScheduleStateTerminateRecordService, IOnDelete<ScheduleStateTerminateRecord>>();

            #endregion

        }
    }
}