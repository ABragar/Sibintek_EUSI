using Base;
using Base.DAL;
using Base.Entities.Complex;
using Base.Notification.Service.Abstract;
using Base.UI.Service;
using Base.Utils.Common;
using CorpProp.Common;
using CorpProp.Entities.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CorpProp.Helpers
{
    public class NotificationHelper
    {
        private readonly IUiFasade _uiFacade;
        private readonly INotificationService _notificationService;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>
        /// Инициализатор.
        /// </summary>
        /// <param name="uiFacade"></param>
        /// <param name="notificationService"></param>
        /// <param name="unitOfWorkFactory"></param>
        public NotificationHelper (IUiFasade uiFacade, INotificationService notificationService, IUnitOfWorkFactory unitOfWorkFactory)
        {
            _uiFacade = uiFacade;
            _notificationService = notificationService;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>
        /// Создает уведомление на основе условия.
        /// </summary>
        /// <param name="notification">Условия уведомления.</param>
        /// <returns>Количество отправленных уведомлений.</returns>
        public int PrepareNotification(SibNotification notification)
        {
            var count = 0;

            try
            {
                var config = _uiFacade.GetViewModelConfig(notification.Mnemonic);

                if (config.ServiceType.GetInterfaces().Contains(typeof(ISibNotification)))
                {
                    using (IUnitOfWork uofw = _unitOfWorkFactory.CreateSystem())
                    {
                        var service = config.GetService<ISibNotification>();
                        List<SibNotificationObject> nObjectsList = service.PrepareLinkedObject(uofw, notification);

                        foreach (SibNotificationObject nObject in nObjectsList)
                        {
                            if (nObject.Recipients.Count == 0)
                                continue;

                            CreateNotification(uofw, nObject);
                            notification.IsEnabled = false;
                            uofw.GetRepository<SibNotification>().Update(notification);

                            count++;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                ex.ToStringWithInner();
            }

            return count;
        }

        /// <summary>
        /// Вычисление даты уведомления.
        /// </summary>
        /// <param name="remindPeriod">Период уведомления.</param>
        /// <param name="endDateTime">Дата окончания действия объекта.</param>
        /// <returns>Время уведомления.</returns>
        public static DateTime? CalculateRemindDateTime(RemindPeriod remindPeriod, DateTime endDateTime)
        {
            DateTime dt;
            TimeSpan ts = new TimeSpan(0, 0, 0);
            switch (remindPeriod.RemindValueType)
            {
                case RemindValueType.Week:
                    dt = endDateTime.AddDays(-(int)remindPeriod.RemindValue * 7);
                    ts = ts.Add(new TimeSpan(dt.Hour, 0, 0));
                    return dt.Date + ts;
                case RemindValueType.Day:
                    dt = endDateTime.AddDays(-(int)remindPeriod.RemindValue);
                    ts = ts.Add(new TimeSpan(dt.Hour, 0, 0));
                    return dt.Date + ts;
                case RemindValueType.Hour:
                    dt = endDateTime.AddHours(-(int)remindPeriod.RemindValue);
                    ts = ts.Add(new TimeSpan(dt.Hour, 0, 0));
                    return dt.Date + ts;
            }

            return null;
        }

        /// <summary>
        /// Создание выражения для запроса динамического свойства.
        /// </summary>
        /// <typeparam name="TItem">Тип объекта.</typeparam>
        /// <typeparam name="TValue">Тип значения.</typeparam>
        /// <param name="property">Свойство.</param>
        /// <param name="value">Значение для выборки.</param>
        /// <returns></returns>
        public static Expression<Func<TItem, bool>> PropertyEquals<TItem, TValue>(PropertyInfo property, TValue value)
        {
            var param = Expression.Parameter(typeof(TItem));
            var expression = property.PropertyType == typeof(DateTime?) ? Expression.Constant(value, typeof(DateTime?)) : Expression.Constant(value, typeof(DateTime));
            var body = Expression.GreaterThanOrEqual(Expression.Property(param, property), expression);
            return Expression.Lambda<Func<TItem, bool>>(body, param);
        }

        /// <summary>
        /// Создает объект для прикрепления к уведомлению.
        /// </summary>
        /// <typeparam name="T">BaseObject</typeparam>
        /// <param name="obj">Объект.</param>
        /// <param name="mnemonic">Мнемоника.</param>
        /// <returns></returns>
        public static LinkBaseObject GetLinkedObj<T>(T obj, string mnemonic = null) where T : BaseObject
        {
            var lbo = new LinkBaseObject(obj)
            {
                Mnemonic = mnemonic
            };
            return lbo;
        }

        /// <summary>
        /// Создание уведомления.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="obj">Объект уведомления.</param>
        private void CreateNotification(IUnitOfWork uofw, SibNotificationObject obj)
        {
            _notificationService.CreateNotification(uofw, obj.Recipients.ToArray(), obj.LinkBaseObject, obj.Subject, obj.Message);
        }

        public enum ChangeType
        {
            Both = 0,
            Status = 1,
            DateEnd = 2
        }
    }
}
