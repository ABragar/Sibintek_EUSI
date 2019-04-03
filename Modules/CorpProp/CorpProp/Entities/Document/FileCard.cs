using Base;
using Base.Attributes;
using Base.Utils.Common.Attributes;
using SubjectObject = CorpProp.Entities.Subject;
using EstateFolder = CorpProp.Entities.Estate;
using CorpProp.Helpers;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Base.ComplexKeyObjects.Superb;
using CorpProp.Entities.Base;
using CorpProp.Entities.Subject;
using CorpProp.Entities.Security;
using Base.DAL;
using System.Linq;
using CorpProp.Common;
using System.Text;
using System.Security.Cryptography;

namespace CorpProp.Entities.Document
{
    /// <summary>
    /// Представляет сведения о карточке документа.
    /// </summary>
    //[AccessPolicy(typeof(EditCreatorOnlyAccessPolicy))]
    [EnableFullTextSearch]
    public class FileCard : TypeObject, ICategorizedItem, ISuperObject<FileCard>, ISibAccessableObject
    {
        public const string PermissionTabName = "Разграничение прав доступа";

        /// <summary>
        /// Инициализирует новый экземпляр класса FileCard.
        /// </summary>
        public FileCard() : base()
        {
            
        }

        /// <summary>
        /// Получает или задает ИД профиля пользователя, создавшего документ.
        /// </summary>
        [SystemProperty]
        public int? CreateUserID { get; set; }

        /// <summary>
        /// Получает или задает профиль пользователя, создавшего документ.
        /// </summary>        
        public virtual SibUser CreateUser { get; set; }

       
        /// <summary>
        /// Получает или задает наименование.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Наименование", Order = 0, TabName = CaptionHelper.DefaultTabName, Required = true)]
        public string Name { get; set; }

        /// <summary>
        /// Получает или задает дату документа.
        /// </summary>
        //[FullTextSearchProperty]
        //[ListView]
        //[DetailView(Name = "Дата", Order = 1, TabName = CaptionHelper.DefaultTabName)]
        public DateTime? DateCard { get; set; }

        /// <summary>
        /// Получает или задает номер.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Номер", Order = 2, TabName = CaptionHelper.DefaultTabName)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Number { get; set; }

        /// <summary>
        /// Получает или задает ФИО исполнителя.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Ф.И.О. исполнителя", Order = 3, TabName = CaptionHelper.DefaultTabName)]
        public virtual SibUser PersonFullName { get; set; }

        /// <summary>
        /// Получает или задает примечание.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Примечание", Order = 5, TabName = CaptionHelper.DefaultTabName)]
        public string Description { get; set; }

        /// <summary>
        /// Получает или задает ИД папки.
        /// </summary>
        [SystemProperty]
        public int CategoryID { get; set; }

        /// <summary>
        /// Получает или задает папку.
        /// </summary>        
        [ListView(Hidden = true)]
        [ForeignKey("CategoryID")]
        [DetailView(Name = "Папка документов", Order = 6, TabName = CaptionHelper.DefaultTabName)]
        public virtual CardFolder Category_ { get; set; }



        #region Документы-основание регистрации
        // Набор атрибутов для документов Росреестра.
        [PropertyDataType(PropertyDataType.Text)]
        public string DocumentID { get; set; }

        [PropertyDataType(PropertyDataType.Text)]
        public string DocTypeCode { get; set; }

        [PropertyDataType(PropertyDataType.Text)]
        public string DocTypeName { get; set; }
       
        [PropertyDataType(PropertyDataType.Text)]
        public string SerialNumber { get; set; }
        [PropertyDataType(PropertyDataType.Text)]
        public string Issuer { get; set; }

        [PropertyDataType(PropertyDataType.Text)]
        public string ExecutorName { get; set; }


        #endregion


        #region Связи
        /// <summary>
        /// Получает или задает ИД делового партнера.
        /// </summary>
        //[ListView(Hidden = true)]
        //[DetailView(Visible = false)]
        [SystemProperty]
        public int? SubjectID { get; set; }
                
        //TODO: реализовать связь через таблицу связей
        /// <summary>
        /// Получает или задает связь с деловым партнером.
        /// </summary>       
        [ListView(Hidden = true, Visible = false)]
        [DetailView(Name = "Деловой партнер", Order = 7, TabName = CaptionHelper.DefaultTabName, Visible = false)]
        public virtual SubjectObject.Subject Subject { get; set; }


        /// <summary>
        /// Получает или задает ИД общества группы.
        /// </summary>
        //[ListView(Hidden = true)]
        //[DetailView(Visible = false)]
        [SystemProperty]
        public int? SocietyID { get; set; }

        /// <summary>
        /// Получает или задает связь с деловым партнером.
        /// </summary>       
        [ListView(Hidden = true)]
        [DetailView(Name = "Общество группы", ReadOnly = true, Order = 7, TabName = CaptionHelper.DefaultTabName)]
        public virtual Society Society { get; set; }

        /// <summary>
        /// Получает или задает права доступа к карточкам документов.
        /// </summary>       
        [ListView(Hidden = true)]
        [DetailView(Name = "Права доступа", TabName = CaptionHelper.DefaultTabName)]
        public virtual FileCardPermission FileCardPermission { get; set; }

        public int? FileCardPermissionID { get; set; }

        /// <summary>
        /// Получает или задает дату документа.
        /// </summary>
        [ListView]
        [DetailView(Name = "Дата документа", TabName = CaptionHelper.DefaultTabName)]
        public DateTime? FileCardDate { get; set; }

        /// <summary>
        /// Получает или задает тип документа.
        /// </summary>
        [ListView]
        [DetailView(Name = "Тип документа", TabName = CaptionHelper.DefaultTabName)]
        public FileCardType FileCardType { get; set; }
        public int? FileCardTypeID { get; set; }
        


        //[ListView(Hidden = true)]
        //[DetailView(Name = "Видно всем", TabName = CaptionHelper.DefaultTabName)]
        //public bool IsAccessAll { get; set; }

        #endregion

        #region ICategorizedItem

        public HCategory Category
        {
            get { return Category_; }
        }

        #endregion

        /// <summary>
        /// Получает или задает кол-во листов.
        /// </summary>
        [DetailView("Количество листов", Visible = false)]
        [ListView("Количество листов", Visible = false)]
        public int? ListCount { get; set; }

        /// <summary>
        /// Получает или задает номер ЕУСИ.
        /// </summary>
        [DetailView("Номер ЕУСИ", Visible = false)]
        [ListView("Номер ЕУСИ", Visible = false)]
        public int? EUSINumber { get; set; }

        public string ExtraID { get; set; }

        /// <summary>
        /// Получает или задает значение хэш.
        /// </summary>
        [SystemProperty]
        public string Hash { get; set; }


        /// <summary>
        /// Ищет профиль пользователя и устанавливает значение для поля "ФИО исполнителя".
        /// </summary>
        /// <param name="uow">Сессия.</param>
        /// <param name="userID">ИД пользователя.</param>
        public void SetCreateUser(IUnitOfWork uow, int? userID)
        {
            if (userID == null) return;
            if (CreateUser == null)
            {
                CreateUser = uow.GetRepository<SibUser>()
                .Filter(f => !f.Hidden && f.UserID == userID).FirstOrDefault();
                if (CreateUser != null && Society == null && CreateUser.Society != null )
                    Society = CreateUser.Society;
            }
                
        }


        /// <summary>
        /// Возвращает вычисленный хэш в виде шестнадцатеричной строки. 
        /// </summary>
        /// <param name="data">Входной масив байтов данных.</param>
        /// <returns>Шестнадцатеричная строка вычисленного хэш-а.</returns>
        public static string GetHash(byte[] input)
        {
            if (input == null)
                return null;
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(input);
                StringBuilder sBuilder = new StringBuilder();                
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }            
        }

        /// <summary>
        /// Проверяет хэш.
        /// </summary>
        /// <param name="input">Входной массив байтов.</param>
        /// <param name="hash">Сравниваемая строка вычисленнго хэш.</param>
        /// <returns></returns>
        public static bool VerifyHash(byte[] input, string hash)
        {            
            string hashOfInput = GetHash(input);            
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
    }
}
