using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Entities.Subject;
using CorpProp.Helpers;

namespace CorpProp.Analyze.Entities.Subject
{
    public class AnalyzeSociety : TypeObject
    {

        public AnalyzeSociety() : base()
        {

        }


        public int? OwnerID { get; set; }

        /// <summary>
        /// Получает и задаёт общество группы которое расширяет класс
        /// </summary>
        [ListView]
        [DetailView]
        public Society Owner { get; set; }

        /// <summary>
        /// Получает или задаёт Статус общества 
        /// </summary>
        [ListView(Name = "Статус общества ")]
        [DetailView(Name = "Статус общества ")]
        [PropertyDataType(PropertyDataType.Text)]
        public string SocietyStatus { get; set; }

        /// <summary>
        /// Получает или задаёт Наличие лицензий 
        /// </summary>
        [ListView(Name = "Наличие лицензий ")]
        [DetailView(Name = "Наличие лицензий ")]
        [PropertyDataType(PropertyDataType.Text)]
        public string AvailabilityOfLicenses { get; set; }

        /// <summary>
        /// Получает или задаёт Чистые активы 
        /// </summary>
        [ListView(Name = "Чистые активы ")]
        [DetailView(Name = "Чистые активы ")]
        public decimal? NetAssets { get; set; }

        /// <summary>
        /// Получает или задаёт Численность работников по штатному расписанию 
        /// </summary>
        [ListView(Name = "Численность работников по штатному расписанию ")]
        [DetailView(Name = "Численность работников по штатному расписанию ")]
        public int? NumberOfEmployeesByStaffingSchedule { get; set; }

        /// <summary>
        /// Получает или задаёт Информация о наличии сохранности официальных документов в Обществе (архивное производство) 
        /// </summary>
        [ListView(Name = "Информация о наличии сохранности официальных документов в Обществе (архивное производство) ")]
        [DetailView(Name = "Информация о наличии сохранности официальных документов в Обществе (архивное производство) ")]
        [PropertyDataType(PropertyDataType.Text)]
        public string PresenceOfSafetyOfOfficialDocuments { get; set; }

        /// <summary>
        /// Получает или задаёт Сведения о лице, осуществляющем ведение реестра акционеров Общества 
        /// </summary>
        [ListView(Name = "Сведения о лице, осуществляющем ведение реестра акционеров Общества ")]
        [DetailView(Name = "Сведения о лице, осуществляющем ведение реестра акционеров Общества ")]
        [PropertyDataType(PropertyDataType.Text)]
        public string LeadingShareholderRegister { get; set; }

        /// <summary>
        /// Получает или задаёт Ведение учетной функции в ОЦО 
        /// </summary>
        [ListView(Name = "Ведение учетной функции в ОЦО ")]
        [DetailView(Name = "Ведение учетной функции в ОЦО ")]
        [PropertyDataType(PropertyDataType.Text)]
        public string MaintainingTheAccountingFunctionInTheSSC { get; set; }

        /// <summary>
        /// Получает или задаёт Аудитор Общества 
        /// </summary>
        [ListView(Name = "Аудитор Общества ")]
        [DetailView(Name = "Аудитор Общества ")]
        [PropertyDataType(PropertyDataType.Text)]
        public string AuditorOfCompany { get; set; }

        /// <summary>
        /// Получает или задаёт Совет директоров
        /// </summary>
        [ListView(Name = "Совет директоров")]
        [DetailView(Name = "Совет директоров")]
        public string BoardOfDirectors { get; set; }

        /// <summary>
        /// Получает или задаёт Состав правления
        /// </summary>
        [ListView(Name = "Состав правления")]
        [DetailView(Name = "Состав правления")]
        public string CompositionOfManagementBoard { get; set; }

        /// <summary>
        /// Получает или задаёт совместное/не совместное
        /// </summary>
        [ListView(Name = "Совместное")]
        [DetailView(Name = "Совместное")]
        public bool? IsJoint { get; set; }

    }
}