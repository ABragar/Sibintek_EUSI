using Base;
using Base.DAL;
using Base.Notification.Service.Abstract;
using Base.Service;
using Base.UI;
using Base.UI.Service;
using CorpProp.Entities.Settings;
using CorpProp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Services.Settings
{
    /// <summary>
    /// Предоставляет данные и методы сервиса объекта - уведомления по условию.
    /// </summary>
    public interface ISibNotificationService : IBaseObjectService<SibNotification>
    {
        int GetNotificationToRemind();
    }
    public class SibNotificationService : BaseObjectService<SibNotification>, ISibNotificationService
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IUiFasade _uiFasade;
        private readonly INotificationService _notificationService;

        public SibNotificationService(IBaseObjectServiceFacade facade, IUnitOfWorkFactory unitOfWorkFactory, IUiFasade uiFasade, INotificationService notificationService) : base(facade)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _uiFasade = uiFasade;
            _notificationService = notificationService;
        }

        protected override IObjectSaver<SibNotification> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<SibNotification> objectSaver)
        {
            objectSaver.Dest.ItemID = objectSaver.Src.ItemID == 0 ? null : objectSaver.Src.ItemID;
            if (objectSaver.Src.Reciever != null)
            {
                if(unitOfWork.GetRepository<Entities.Security.SibUser>()
                    .Find(f => f.ID == objectSaver.Src.Reciever.ID)?.User == null)
                    throw new Exception("Данный пользователь не имеет прав для работы в системе.");
            }

            return base.GetForSave(unitOfWork, objectSaver)
                            .SaveOneObject(s => s.Reciever)
                ;
        }

        /// <summary>
        /// Создание уведомлений.
        /// </summary>
        /// <returns>Количество отправленных уведомлений.</returns>
        public int GetNotificationToRemind()
        {
            int result = 0;
            using (IUnitOfWork uofw = _unitOfWorkFactory.CreateSystem())
            {
                var notifications = this.GetAll(uofw).Where(w => w.IsEnabled && !w.Hidden).ToList();

                if(notifications.Count > 0)
                {
                    foreach (SibNotification notification in notifications)
                    {
                        NotificationHelper nh = new NotificationHelper(_uiFasade, _notificationService, _unitOfWorkFactory);
                        result =+ nh.PrepareNotification(notification);
                    }
                }
            }
            return result;
        }
    }
}
