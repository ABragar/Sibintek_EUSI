using Base.Attributes;
using Base.Security;
using CorpProp.Entities.ProjectActivity;
using CorpProp.Entities.Subject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base;
using Base.Translations;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.NSI;
using CorpProp.Helpers;
using Base.DAL;

namespace CorpProp.Entities.Security
{
    /// <summary>
    /// Представляет профиль пользователя.
    /// </summary>
    public class SibUser : BaseProfile
    {
        private static readonly CompiledExpression<SibUser, string> _societyName =
         DefaultTranslationOf<SibUser>.Property(x => x.SocietyName).Is(x => (x.Society != null ) ? x.Society.ShortName : "");

        private static readonly CompiledExpression<SibUser, string> _societyDeptName =
         DefaultTranslationOf<SibUser>.Property(x => x.SocietyDeptName).Is(x => (x.SocietyDept != null) ? x.SocietyDept.Name : "");

        private static readonly CompiledExpression<SibUser, string> _fullname =
           DefaultTranslationOf<SibUser>.Property(x => x.FullName).Is(x => x.LastName + " " + (x.FirstName ?? "").Trim() + " " + (x.MiddleName ?? "").Trim());

        private static readonly CompiledExpression<SibUser, string> _fullnameAndOrganisation =
           DefaultTranslationOf<SibUser>.Property(x => x.FullNameAndOrg).Is(x => x.LastName + " " + (x.FirstName ?? "").Trim() + " " + (x.MiddleName ?? "").Trim() + " " + (x.SocietyName ?? "").Trim());

        private static readonly CompiledExpression<SibUser, string> _login =
        DefaultTranslationOf<SibUser>.Property(x => x.Login).Is(x => (x.User != null) ? x.User.SysName : "");

        /// <summary>
        /// Получает логин пользователя.
        /// </summary>
        [DetailView("Логин", Visible = false)]
        [FullTextSearchProperty]
        public string Login => _login.Evaluate(this);


        /// <summary>
        /// Получает полное ФИО пользователя.
        /// </summary>
        [ListView("Ф.И.О.")]
        [DetailView(Visible = false)]
        [FullTextSearchProperty]
        public new string FullName => _fullname.Evaluate(this);

        /// <summary>
        /// Получает полное ФИО пользователя и краткое наименование организации.
        /// </summary>
        [ListView("Ф.И.О. в ОГ")]
        [DetailView(Visible = false)]
        [FullTextSearchProperty]
        public string FullNameAndOrg => _fullnameAndOrganisation.Evaluate(this);

        /// <summary>
        /// Получает или задает ИД пользователя.
        /// </summary>
        public int? UserID { get; set; }

        /// <summary>
        /// Получает или задает пользователя данного профиля.
        /// </summary>       
        [ForeignKey("UserID")]
        [DetailView(Visible = false)]
        public virtual User User { get; set; }

        /// <summary>
        /// Получает или задает ИД руководителя.
        /// </summary>       
        public int? BossID { get; set; }

        /// <summary>
        /// Получает или задает руководителя.
        /// </summary>       
        [DetailView(Name = "Руководитель")]
        [ForeignKey("BossID")]
        public virtual SibUser Boss { get; set; }

       

        /// <summary>
        /// Получает или задает ИД замещающего пользователя.
        /// </summary>
        public int? ViceID { get; set; }

        /// <summary>
        /// Получает или задает замещающего пользователя.
        /// </summary>       
        [DetailView(Name = "Замещающий")]
        [ForeignKey("ViceID")]
        public virtual SibUser Vice { get; set; }

        /// <summary>
        /// Получает краткое наименование ОГ.
        /// </summary>
        [DetailView(Name = "Наименование ОГ", Visible = false)]
        public string SocietyName => _societyName.Evaluate(this);

        /// <summary>
        /// Получает или задает наименование структурного подразделения.
        /// </summary>
        //[DetailView(Name = "Подразделение")]
        //[PropertyDataType(PropertyDataType.Text)]
        public string DeptName { get; set; }

        /// <summary>
        /// Получает краткое наименование ОГ.
        /// </summary>
        [DetailView(Name = "Подразделение", Visible = false)]
        public string SocietyDeptName => _societyDeptName.Evaluate(this);

        /// <summary>
        /// Получает или задает ИД Структурного подразделения
        /// </summary>
        
        public int? SocietyDeptID { get; set; }

        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Структурное подразделение")]
        public  SocietyDept SocietyDept { get; set; }

        /// <summary>
        /// Получает или задает наименование должности.
        /// </summary>
        [DetailView(Name = "Должность")]
        [PropertyDataType(PropertyDataType.Text)]
        public string PostName { get; set; }

        /// <summary>
        /// Получает или задает рабочий телефон.
        /// </summary>
        [DetailView(Name = "Телефон раб.")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Phone { get; set; }

        /// <summary>
        /// Получает или задает мобильный телефон.
        /// </summary>
        [DetailView(Name = "Телефон моб.")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Mobile { get; set; }

        /// <summary>
        /// Получает или задает E-mail.
        /// </summary>
        [DetailView(Name = "E-mail")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Email { get; set; }

        /// <summary>
        /// Получает или задает примечание.
        /// </summary>
        [DetailView(Name = "Примечание")]
        public string Description { get; set; }

        /// <summary>
        /// Получает или задает ИД ОГ
        /// </summary>
        [SystemProperty]
        public int? SocietyID { get; set; }

        /// <summary>
        /// Получает или задает общество группы.
        /// </summary>       
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Общество группы" /*, ReadOnly = !SibiAssemblyInfo.FullAccess*/)]
        public virtual Society Society { get; set; }

        /// <summary>
        /// Получает или задает Имя ключа ПКЗИ.
        /// </summary>
        [DetailView(Name = "Имя ключа ПКЗИ")]
        public string PKZI { get; set; }

        /// <summary>
        /// Получает или задает признак Активный.
        /// </summary>
        [DetailView(Name = "Активный", Visible = false)]
        public bool IsActiv { get; set; }

        /// <summary>
        /// Получает или задает признак Административная.
        /// </summary>
        [DetailView(Name = "Административная")]
        public bool IsAdministrative { get; set; }

        /// <summary>
        /// Получает или задает  Дату начала полномочий.
        /// </summary>
        [DetailView(Name = "Дата начала полномочий")]
        public DateTime? DateStartAuthorization { get; set; }

        /// <summary>
        /// Получает или задает Дату окончания полномочий.
        /// </summary>
        [DetailView(Name = "Дата окончания полномочий")]
        public DateTime? DateEndAuthorization { get; set; }

        /// <summary>
        /// Получает или задает признак Наличия удаленного доступа.
        /// </summary>
        [DetailView(Name = "Наличие удаленного доступа")]
        public bool IsAvailabilityRemoteAccess { get; set; }
        
       

        ///// <summary>
        ///// Получает или задает запросы пользователя, где он соисполнитель.
        ///// </summary>
        //[InverseProperty("Executors")]
        //public virtual ICollection<CorpProp.Entities.Request.Request> ExecutorsRequests { get; set; }

        ///// <summary>
        ///// Получает или задает ответы пользователя, где он соисполнитель.
        ///// </summary>
        //[InverseProperty("CoexecutorsSibUser")]
        //public virtual ICollection<CorpProp.Entities.Request.Response> CoexecutorsSibUserResponses { get; set; }

        /// <summary>
        /// Коллекция задач, в которых пользователь соисполнитель.
        /// </summary>  

        [InverseProperty("Boss")]
        public virtual ICollection<SibUser> Workers { get; set; }

        [InverseProperty("Vice")]
        public virtual ICollection<SibUser> Vices { get; set; }

        /// <summary>
        /// Коллекция ОГ в которых пользователь является ответственным.
        /// </summary>       
        [InverseProperty("ResponsableForResponse")]
        public virtual ICollection<Society> ResponsableForResponseInSocietys { get; set; }

        /// <summary>
        /// Ответственный за ответы на запросы ЦАУК
        /// </summary>
        [ListView(Name = "Ответственный за ответы")]
        [DetailView(Name = "Ответственный за ответы")]
        public bool ResponsibleOnRequest { get; set; }

        /// <summary>
        /// Исполнитель по запросам
        /// </summary>
        [ListView(Name = "Исполнитель по запросам")]
        [DetailView(Name = "Исполнитель по запросам")]
        public bool ExecutorOnRequest { get; set; }

        /// <summary>
        /// Инициализирует новый экземпляр класса SibUser.
        /// </summary>
        public SibUser()
        {           
            Workers = new List<SibUser>();
            Vices = new List<SibUser>();
            ResponsableForResponseInSocietys = new List<Society>();
        }


        public void SetSocietyDept(IUnitOfWork uow, string deptName)
        {
            if (String.IsNullOrEmpty(deptName))
                return;
            if (Society != null)
            {
                var societyID = Society.ID;
                var dept = uow.GetRepository<SocietyDept>()
                   .Filter(f => !f.Hidden && f.Name == deptName && f.Society != null
                    && f.Society.ID == societyID)
                   .FirstOrDefault();

                if (dept == null)
                {
                    dept = uow.GetRepository<SocietyDept>()
                        .Create(new SocietyDept()
                        {
                            Society = this.Society,
                            Name = deptName
                        });
                }
            }
            else
                DeptName = deptName;
        }
    }
}
