using Base.Attributes;
using Base.ComplexKeyObjects.Superb;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Entities.Document;
using CorpProp.Entities.Estate;
using CorpProp.Entities.Law;
using CorpProp.Entities.Security;
using CorpProp.Entities.Subject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorpProp.Entities.Law
{
    /// <summary>
    /// Представляет Выписку ЕГРН/ЕГРП.
    /// </summary>
    [EnableFullTextSearch]
    public class Extract : TypeObject, ISuperObject<Extract>
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса Extract.
        /// </summary>
        public Extract()
        {

            CountDocRights = 0;
            CountEncumbrances = 0;
            CountObjects = 0;
            CountRights = 0;
        }

        [ListView(Hidden = true)]
        [DetailView("Акцептовано ДС", Visible = false)]
        [DefaultValue(false)]
        public bool isAccept { get; set; }


        [PropertyDataType(PropertyDataType.ExtraId)]
        public string ExtraID { get; }
        
        [SystemProperty]
        public int? SibUserID { get; set; }

        [ListView(Hidden = true)]
        [DetailView("Пользователь", Description ="Пользователь, загрузивший выписку", Visible = false)]
        public virtual SibUser SibUser { get; set; }


        /// <summary>
        /// Уникальный идентификатор записи КУВИ
        /// </summary>
        public decimal ID_KUVI { get; set; }

        /// <summary>
        /// Получает или задает наименование выписки.
        /// </summary>        
        [FullTextSearchProperty]       
        [PropertyDataType(PropertyDataType.Text)]
        [ListView("Наименование")]
        public string Name { get; set; }

        /// <summary>
        /// Получает или задает ИД типа выписки.
        /// </summary>
        public int? ExtractTypeID { get; set; }

        /// <summary>
        /// Получает или задает тип выписки.
        /// </summary>            
        public virtual ExtractType ExtractType { get; set; }

        /// <summary>
        /// Получает или задает ИД формата выписки.
        /// </summary>
        public int? ExtractFormatID { get; set; }

        /// <summary>
        /// Получает или задает формат выписки.
        /// </summary>             
        public virtual ExtractFormat ExtractFormat { get; set; }


        /// <summary>
        /// Получает или задает дату запроса.
        /// </summary>       
        public DateTime? DateRequest { get; set; }

        /// <summary>
        /// Получает или задает номер запроса.
        /// </summary>       
        [PropertyDataType(PropertyDataType.Text)]
        public string NumberRequest { get; set; }

        /// <summary>
        /// Дата выписки.
        /// </summary>
        public DateTime? ExtractDate { get; set; }

        
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// Дата начала периода
        /// </summary>             
        public string ExtractPeriodSDate { get; set; }

        public DateTime? EndDate { get; set; }
        /// <summary>
        /// Дата окончания периода
        /// </summary>      

        public string ExtractPeriodEDate { get; set; }
       

        /// <summary>
        /// номер выписки/справки/сообщения
        /// </summary>
        public string ExtractNumber { get; set; }

        #region Сервисная информация tServisInf
        /// <summary>
        /// Тип передаваемой информации
        /// </summary>   
        public string CodeType { get; set; }

        /// <summary>
        /// Версия схемы
        /// </summary>      
        public string Version { get; set; }

        /// <summary>
        /// Тип учетной системы
        /// </summary>        
        public string Scope { get; set; }

        #region Отправитель Sender
        /// <summary>
        /// Код
        /// </summary>      
        public string SenderKod { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>       
        public string SenderName { get; set; }

        /// <summary>
        /// Регион (код)
        /// </summary>      
        public string RegionCode { get; set; }

        /// <summary>
        /// Регион - наименование.
        /// </summary>
        public string RegionName { get; set; }

        /// <summary>
        /// Дата выгрузки
        /// </summary>       
        public System.DateTime? DateUpload { get; set; }

        /// <summary>
        /// Должность
        /// </summary>       
        public string Appointment { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>        
        public string FIO { get; set; }

        /// <summary>
        /// Адрес электронной почты отправителя
        /// </summary>     
        public string EMail { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>       
        public string Telephone { get; set; }

        #endregion

        #region Получатель Recipient

        /// <summary>
        /// Код организации получателя
        /// </summary>        
        public string RecipientKod { get; set; }

        /// <summary>
        /// Наименование организации получателя
        /// </summary>       
        public string RecipientName { get; set; }

        #endregion

        #endregion      

        #region Информация о запросе DeclarAttribute
        /// <summary>
        /// Адресат
        /// </summary>       
        public string ReceivName { get; set; }

        /// <summary>
        /// Субъект, от имени которого действует адресат
        /// </summary>       
        public string Representativ { get; set; }
        /// <summary>
        /// Адрес
        /// </summary>
        public string ReceivAdress { get; set; }

        /// <summary>
        /// код запрошенной информации
        /// </summary>
        public string ExtractTypeCode { get; set; }
        /// <summary>
        /// наименование запрошенной информации
        /// </summary>
        public string ExtractTypeName { get; set; }
        /// <summary>
        /// вид запрошенной информации текст
        /// </summary>
        public string ExtractTypeText { get; set; }

        
        /// <summary>
        /// дата документа запроса
        /// </summary>
        public string RequeryDate { get; set; }
        /// <summary>
        /// исходящий номер учреждения
        /// </summary>
        public string OfficeNumber { get; set; }
        /// <summary>
        /// исходящая дата учреждения
        /// </summary>
        public string OfficeDate { get; set; }
        /// <summary>
        /// количество сформированных выписок
        /// </summary>
        public int? ExtractCount { get; set; }
        /// <summary>
        /// количество сформированных уведомлений
        /// </summary>
        public int? NoticeCount { get; set; }
        /// <summary>
        /// количество сформированных отказов
        /// </summary>
        public int? RefuseCount { get; set; }
        /// <summary>
        /// регистратор, подписавший выписку(справку)
        /// </summary>
        public string Registrator { get; set; }
        #endregion

        #region Заголовок HeadContent
        /// <summary>
        /// Уникальный идентификатор документа в записи КУВИ
        /// </summary>      
        public object ID_REC_KUVI { get; set; }
        /// <summary>
        /// Наименование службы
        /// </summary>            
        public string Title { get; set; }

        /// <summary>
        /// Наименование территориального управления
        /// </summary>
        public string DeptName { get; set; }
        /// <summary>
        /// Наименование документа
        /// </summary>       
        public string ExtractTitle { get; set; }
        /// <summary>
        /// Суммарное  описание запроса
        /// </summary>       
        public string Content { get; set; }
        #endregion //HeadContent

        #region Завершающий текст FootContent
        /// <summary>
        /// описание получателя информации ( "выписка выдана ...")
        /// </summary>       
        public string FootRecipient { get; set; }

        /// <summary>
        /// регионы/коды управлений Росреестра
        /// </summary>       
        public string FootExtractRegion { get; set; }

        /// <summary>
        /// дата выписки/справки/сообщения
        /// </summary> 
        public string FootExtractDate { get; set; }
        /// <summary>
        /// Суммарное  описание завершающего текста (ссылка на закон и т.п.)
        /// </summary>      
        public string FootContent { get; set; }
        #endregion //FootContent


        public string SubjectINN { get; set; }

        public string SubjectName { get; set; }

        /// <summary>
        /// Получает или задает ИД ОГ
        /// </summary>
        public int? SocietyID { get; set; }

        /// <summary>
        /// Получает или задает общество группы.
        /// </summary>              
        public virtual Society Society { get; set; }

        /// <summary>
        /// Получает или задает документ.
        /// </summary>       
        public virtual FileCard FileCard { get; set; }


        /// <summary>
        /// Дата получения запроса органом регистрации прав.
        /// </summary>         
        public System.DateTime? DateReceipt { get; set; }       


        /// <summary>
        /// Глобальный уникальный идентификатор документа
        /// </summary> 
        [PropertyDataType(PropertyDataType.Text)]
        public string Guid { get; set; }


        /// <summary>
        /// Получает или задает кол-во прав.
        /// </summary>
        public int? CountRights { get; set; }

        /// <summary>
        /// Получает или задает кол-во ОНИ.
        /// </summary>
        public int? CountObjects { get; set; }

        /// <summary>
        /// Получает или задает кол-во документов основания прав.
        /// </summary>
        public int? CountDocRights { get; set; }

        /// <summary>
        /// Получает или задает кол-во ограничений/обременений.
        /// </summary>
        public int? CountEncumbrances { get; set; }
    }
}
