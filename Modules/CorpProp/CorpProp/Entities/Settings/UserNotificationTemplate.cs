using Base;
using Base.Attributes;
using Base.Notification.Entities;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Document;
using CorpProp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Entity = Base.Entities;

namespace CorpProp.Entities.Settings
{
    /// <summary>
    /// Представляет шаблон уведомления.
    /// </summary>
    [Serializable]
    public class UserNotificationTemplate : BaseObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса UserNotificationTemplate.
        /// </summary>
        public UserNotificationTemplate() : base()
        {
            Oid = System.Guid.NewGuid();
        }

        /// <summary>
        /// Получает или задает Oid.
        /// </summary>
        [SystemProperty]
        public Guid Oid { get; private set; }

        /// <summary>
        /// Получает или задает наименование типа объектов уведомления.
        /// </summary>
        [ListView("Объект")]
        [DetailView("Объект", Required = true)]
        [PropertyDataType(PropertyDataType.ObjectType)]        
        public string ObjectType { get; set; }

        /// <summary>
        /// Получает или задает наименование шаблона.
        /// </summary>
        [FullTextSearchProperty]
        [DetailView("Наименование", Required = true), ListView]
        [PropertyDataType(PropertyDataType.Text)]
        public string Title { get; set; }

        /// <summary>
        /// Получает или задает код шаблона.
        /// </summary>
        [FullTextSearchProperty]
        [DetailView("Код", Required = true), ListView]
        [PropertyDataType(PropertyDataType.Text)]
        [SystemProperty]
        public string Code { get; set; }

        /// <summary>
        /// Получает или задает тему уведомления.
        /// </summary>
        [FullTextSearchProperty]
        [DetailView("Тема"), ListView]
        [PropertyDataType(PropertyDataType.Text)]
        public string Subject { get; set; }

        /// <summary>
        /// Получает или задает текст уведомления.
        /// </summary>
        [FullTextSearchProperty]
        [DetailView("Текст сообщения"), ListView]
        [PropertyDataType(PropertyDataType.Text)]
        public string Message { get; set; }

        /// <summary>
        /// Получает или задает описание шаблона.
        /// </summary>
        [FullTextSearchProperty]
        [DetailView("Описание"), ListView]
        [PropertyDataType(PropertyDataType.Text)]
        public string Description { get; set; }

        /// <summary>
        /// Получает или задает признак, что тело уведомления HTML.
        /// </summary>
        [DetailView("Это HTML"), ListView]
        [DefaultValue(false)]
        public bool IsHTML { get; set; }

        /// <summary>
        /// Получает или задает содержимое HTML уведомления.
        /// </summary>
        [DetailView("Содержимое HTML"), ListView]
        [PropertyDataType(PropertyDataType.MultilineText)]
        public string HtmlBody { get; set; }

        /// <summary>
        /// Получает или задает признак необходимости отправки уведомления в Системе.
        /// </summary>
        [DetailView("Отправить в Системе"), ListView]
        [DefaultValue(false)]
        public bool BySystem { get; set; }

        /// <summary>
        /// Получает или задает признак необходимости отправки уведомления по email.
        /// </summary>
        [DetailView("Отправить по E-mail"), ListView]
        [DefaultValue(false)]
        public bool ByEmail { get; set; }

        /// <summary>
        /// Получает или задает фиксированный email получателя.
        /// </summary>
        [FullTextSearchProperty]
        [DetailView("Фиксированный Получатель"), ListView]
        [PropertyDataType(PropertyDataType.Text)]
        [SystemProperty]
        public string Recipient { get; set; }

        /// <summary>
        /// Получает или задает файл отчета Report *.trdp.
        /// </summary>
        [FullTextSearchProperty]
        [DetailView("Файл отчета"), ListView]       
        public FileDB Report { get; set; }

        /// <summary>
        /// Получает или задает ИД файла отчета.
        /// </summary>
        [SystemProperty]
        public int? ReportID { get; set; }

        /// <summary>
        /// Получает или задает группу.
        /// </summary>
        [DetailView("Группа уведомлений"), ListView]
        public NotificationGroup NotificationGroup { get; set;}

        /// <summary>
        /// Получает или задает ИД группы.
        /// </summary>
        [SystemProperty]
        public int? NotificationGroupID { get; set; }

        /// <summary>
        /// Создает уведомление пользователя по шаблону.
        /// </summary>
        /// <returns></returns>
        public UserNotification CreateNotice(object obj, int? userID, BaseObject link = null)
        {
            UserNotification notice = new UserNotification()
            {
                Template = this,
                Date = DateTime.Now,
                Title = (String.IsNullOrEmpty(this.Subject))? this.Title : ReplaceValues(obj, this.Subject),
                Description = ReplaceValues(obj, this.Message),
                ByEmail = this.ByEmail,
                HtmlBody = ReplaceValues(obj, this.HtmlBody),
                IsHTML = this.IsHTML,
                Status = NotificationStatus.New,
                Entity = (link == null)? ((obj is BaseObject) ? new Entity.Complex.LinkBaseObject(obj as BaseObject): null) 
                : new Entity.Complex.LinkBaseObject(link),
                IsReadInSystem = this.BySystem,
                UserID = userID
            };
            return notice;
        }

        /// <summary>
        /// Заменяет объявленные перменные в тексте значениями свойств объекта.
        /// </summary>
        /// <param name="obj">Объект, значения которого будут заменены в переменных.</param>
        /// <param name="text">Исходных текст.</param>
        /// <returns>Текст уведомления.</returns>
        /// <remarks>
        /// Синтаксис объявления переменных {Fields.НаименованиеСвойства}.
        /// </remarks>
        public string ReplaceValues(object obj, string text)
        {
            if (obj == null || String.IsNullOrEmpty(text)) return text;
            string pattern = @"[{](Fields.)\w*?[}]";
            Regex regex = new Regex(pattern);
            MatchCollection matches = regex.Matches(text);            
            foreach (Match match in matches)
            {
                if (String.IsNullOrEmpty(match.Value))
                    continue;
                string source = match.Value.Replace("Fields.", "").Replace("{", "").Replace("}", "");
                string value = ObjectHelper.GetStrValue(obj, source);
                text = text.Replace(match.Value, value);
            }
            text = ReplaceList(obj,  text);
            return text;
        }

        private string ReplaceList(object obj, string text)
        {
            
            if (obj == null || String.IsNullOrEmpty(text)) return text;
            string pattern = @"{List.Fields}<tr[\s\S]*?<\/tr>{/List.Fields}";
            Regex regex = new Regex(pattern);
            MatchCollection matches = regex.Matches(text);
            foreach (Match match in matches)
            {
                if (String.IsNullOrEmpty(match.Value))
                    continue;
                string source = match.Value;
                string rowSource = source.Replace("{List.Fields}", "").Replace("{/List.Fields}", "");
                string rowResult = "";
                string property = GetListProperty(rowSource);
                IEnumerable<object> objs = ObjectHelper.GetListValue(obj, property);
                if (objs != null)
                    foreach (object item in objs)
                    {
                        var res = rowSource.Replace("{List." + property, "{Fields");
                        rowResult += ReplaceValues(item, res);
                    }

                text = text.Replace(source, rowResult);
            }
            
            return text;
        }

        private string GetListProperty(string source)
        {            
            string prop = "";
            string pattern = @"[{](List.)[\s\S]*?[}]";
            Regex regex = new Regex(pattern);
            MatchCollection matches = regex.Matches(source);
            foreach (Match match in matches)
            {
                if (String.IsNullOrEmpty(match.Value))
                    continue;
                var listProps = match.Value.Replace("List.", "").Replace("{", "").Replace("}", "").Split('.');
                prop = (listProps.Length > 0) ? listProps[0] : "";
                if (!string.IsNullOrEmpty(prop))
                    return prop;
            }
            return prop;
        }
    }
}
