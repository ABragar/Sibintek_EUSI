using Base.DAL;
using Base.Entities.Complex;
using Base.Notification.Entities;
using Base.Notification.Service.Abstract;
using Base.Security;
using Base.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Base.Events;

namespace Base.Notification.Service.Concrete
{
    public class NotificationService : BaseObjectService<Entities.Notification>, INotificationService
    {
        private readonly IMailQueueService _mailQueueService;

        public NotificationService(IBaseObjectServiceFacade facade, IMailQueueService mailQueueService) : base(facade)
        {
            _mailQueueService = mailQueueService;
        }

        public override Entities.Notification Create(IUnitOfWork unitOfWork, Entities.Notification obj)
        {
            //NOTE: use CreateNotification
            throw new NotSupportedException();
        }

        public override IReadOnlyCollection<Entities.Notification> CreateCollection(IUnitOfWork unitOfWork,
            IReadOnlyCollection<Entities.Notification> collection)
        {
            //NOTE: use CreateNotification
            throw new NotSupportedException();
        }

        public override Entities.Notification Update(IUnitOfWork unitOfWork, Entities.Notification obj)
        {
            throw new AccessDeniedException();
        }

        public override IReadOnlyCollection<Entities.Notification> UpdateCollection(IUnitOfWork unitOfWork,
            IReadOnlyCollection<Entities.Notification> collection)
        {
            throw new AccessDeniedException();
        }

        public override void Delete(IUnitOfWork unitOfWork, Entities.Notification obj)
        {
            base.Delete(unitOfWork, obj);
            //throw new AccessDeniedException();
        }

        public override void DeleteCollection(IUnitOfWork unitOfWork,
            IReadOnlyCollection<Entities.Notification> collection)
        {
            throw new AccessDeniedException();
        }

        private string GetKey(int id)
        {
            return $"notification:{id}";
        }

        public async Task<int> MarkAsRead(IEnumerable<int> ids)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids));

            using (var uofw = UnitOfWorkFactory.CreateSystem())
            {
                var notifications = GetAll(uofw).Where(x =>
                    x.UserID == Ambient.AppContext.SecurityUser.ID &&
                    x.Status == NotificationStatus.New &&
                    ids.Contains(x.ID)).ToList();

                var repository = uofw.GetRepository<Entities.Notification>();

                foreach (var notification in notifications)
                {
                    notification.Status = NotificationStatus.Viewed;
                    repository.Update(notification);
                }

                await uofw.SaveChangesAsync();

                foreach (var notification in notifications)
                {
                    OnUpdate.Raise(() => new OnUpdate<Entities.Notification>(notification, notification, uofw));
                }

                await _mailQueueService.RemoveFromQueue(uofw, notifications.Select(x => GetKey(x.ID)));

                return notifications.Count;
            }
        }

        public void CreateNotification(IUnitOfWork uow, int[] userIds, LinkBaseObject obj, string title, string descr)
        {
            var notifications = new List<Entities.Notification>();

            var notificationRepository = uow.GetRepository<Entities.Notification>();

            foreach (int userId in userIds.Distinct())
            {
                var notification = new Entities.Notification()
                {
                    Title = title,
                    Description = descr,
                    Date = DateTime.Now,
                    Entity = obj,
                    UserID = userId
                };

                notificationRepository.Create(notification);
                notifications.Add(notification);
            }

            uow.SaveChanges();

            ////TODO: Отправка электропочты по списку уведоблений. На данном этапе это не нужно.
            //var profiles = uow.GetRepository<User>()
            //    .All()
            //    .Where(x => userIds.Contains(x.ID))
            //    .Select(x => x.Profile);

            //var emails = profiles
            //    .SelectMany(x => x.Emails)
            //    .Where(x => x.IsPrimary)
            //    .ToDictionary(x => x.BaseProfileID, x => x.Email);


            //foreach (var notification in notifications)
            //{
            //    OnCreate.Raise(() => new OnCreate<Entities.Notification>(notification, uow));

            //    if (notification.User.ProfileId != null && emails.ContainsKey(notification.User.ProfileId.Value))
            //        _mailQueueService.AddToQueue(uow, GetKey(notification.ID), notification.Title, notification.Entity,
            //            emails[notification.User.ProfileId.Value], save: false);
            //}

            //uow.SaveChanges();
        }
    }
}