using Base;
using Base.Attributes;
using Base.Translations;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Law;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Entities
{
    /// <summary>
    /// Сведения о праве и правообладателях.
    /// </summary>
    [EnableFullTextSearch]
    public class RightRecord : BaseObject
    {
        //protected const string TabName1 = "[000]Основные данные";         
        protected const string TabName2 = "[002]Доля";
        protected const string TabName3 = "[003]Характеристики недвижимости";
        protected const string TabName4 = "[004]Реквизиты выписки";


        private static readonly CompiledExpression<RightRecord, string> _CadastralNumber =
          DefaultTranslationOf<RightRecord>.Property(x => x.CadastralNumber).Is(x => (x.ObjectRecord  == null) ? "" : x.ObjectRecord.CadastralNumber);

        private static readonly CompiledExpression<RightRecord, string> _ObjectTypeText =
         DefaultTranslationOf<RightRecord>.Property(x => x.ObjectTypeText).Is(x => (x.ObjectRecord == null) ? "" : x.ObjectRecord.TypeValue);


        private static readonly CompiledExpression<RightRecord, string> _Address =
         DefaultTranslationOf<RightRecord>.Property(x => x.Address).Is(x => (x.ObjectRecord == null) ? "" : x.ObjectRecord.Address);


        public RightRecord()
        {
            //DocumentRecords = new List<DocumentRecord>();
            //DealRecords = new List<DealRecord>();
            ////RightHolders = new List<RightHolder>();
            Numerator = 1;
            Denominator = 1;
        }

        [ListView(Hidden = true)]
        [DetailView("Акцептовано ДС", Visible = false)]
        [DefaultValue(false)]
        public bool isAccept { get; set; }

        /// <summary>
        /// Получает или задает статус обновления записи в АИС КС.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView("Статус обновления в АИС КС", Visible = false)]
        public StatusCorpProp UpdateCPStatus { get; set; }

        /// <summary>
        /// Получает или задает статус обновления записи в АИС КС.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView("Дата обновления в АИС КС", Visible = false)]
        public DateTime? UpdateCPDateTime { get; set; }


        [ListView(Hidden =true)]
        [FullTextSearchProperty]
        [DetailView(Name = "Кадастровый номер", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string CadastralNumber => _CadastralNumber.Evaluate(this);

        [ListView(Hidden = true)]
        [DetailView(Name = "Тип ОНИ", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string ObjectTypeText => _ObjectTypeText.Evaluate(this);

        [ListView(Hidden = true)]
        [DetailView(Name = "Адрес", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Address => _Address.Evaluate(this);

        /// <summary>
        /// Уникальный идентификатор записи о праве (ограничении)
        /// </summary>        
        [DetailView(Visible = false)]
        public string ID_Record { get; set; }
        /// <summary>
        /// Дата модификации
        /// </summary>       
        [DetailView(Visible = false)]
        public string MdfDate { get; set; }
        /// <summary>
        /// Номер государствеенной регистрации
        /// </summary>
        [DetailView(Visible = false)]
        public string RegNumber { get; set; }
        /// <summary>
        /// Код  права
        /// </summary>
        [DetailView(Visible = false)]
        public string RightTypeCode { get; set; }

        [DetailView(Visible = false)]
        public string RightTypeName { get; set; }

        /// <summary>
        /// Информация о восстановлении права по решению суда
        /// </summary>      
        public string RestorCourt { get; set; }

        /// <summary>
        /// Номер прекращения (перехода) права
        /// </summary>        
        public string EndNumber { get; set; }

        /// <summary>
        /// Дата государственной регистрации
        /// </summary>
        [DetailView(Visible = false)]
        public string RegDateStr { get; set; }

        [DetailView(Visible = false)]
        public DateTime? RegDate { get; set; }

        /// <summary>
        /// Дата прекращения права
        /// </summary>
        [DetailView(Visible = false)]
        public string EndDateStr { get; set; }

        [DetailView(Visible = false)]
        public DateTime? EndDate { get; set; }

        [DetailView(Visible = false)]
        public int? Numerator { get; set;}

        [DetailView(Visible = false)]
        public int? Denominator { get; set; }

        [DetailView(Visible = false)]
        public string ShareText { get; set; }      


        #region Доля Shares

        /// <summary>
        /// Доля в праве общей долевой собственности пропорциональна размеру общей площади помещений, машино-мест, не переданных участникам долевого строительства
        /// </summary>        
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Доля пропорциональна площади, не переданных участникам ДС", TabName = TabName2,
         Description = "Доля в праве общей долевой собственности пропорциональна размеру общей площади помещений, машино-мест, не переданных участникам долевого строительства")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Builder_share_info { get; set; }

        /// <summary>
        /// Доля в праве общей долевой собственности пропорциональна размеру общей площади помещений, машино-мест, не переданных участникам долевого строительства, а также помещений, машино-мест
        /// </summary>       
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Доля пропорциональна площади, не переданных участникам ДС,а также помещений, машино-мест", TabName = TabName2,
            Description = "Доля в праве общей долевой собственности пропорциональна размеру общей площади помещений, машино-мест, не переданных участникам долевого строительства, а также помещений, машино-мест")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Builder_share_with_object_info { get; set; }

        /// <summary>
        /// Сведения о помещениях, машино-местах
        /// </summary>       
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Сведения о помещениях, машино-местах", TabName = TabName2)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Info_objectsCadNumbers { get; set; }


        /// <summary>
        /// Размер доли в праве общей долевой собственности на общее имущество собственников комнат в жилом помещении
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Размер доли в праве ДС собственников комнат", TabName = TabName2,
            Description = "Размер доли в праве общей долевой собственности на общее имущество собственников комнат в жилом помещении")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Room_owners_share_info { get; set; }



        /// <summary>
        /// Балло-гектары
        /// </summary>    
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Балло-гектары", TabName = TabName2)]
        [DefaultValue(0)]
        public decimal? Bal_hectare { get; set; }

        /// <summary>
        /// Гектары
        /// </summary>      
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Гектары", TabName = TabName2)]
        [DefaultValue(0)]
        public decimal? Hectare { get; set; }

        /// <summary>
        /// Доля в праве общей долевой собственности пропорциональна размеру общей площади
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Доля в праве общей ДС пропорциональна размеру общей площади", TabName = TabName2)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Share_Share_description { get; set; }

        /// <summary>
        /// Кадастровый номер для расчета пропорций
        /// </summary>
        [ListView(Hidden = true)]
        [FullTextSearchProperty]
        [DetailView(ReadOnly = true, Name = "Кадастровый номер для расчета пропорций", TabName = TabName2)]

        [PropertyDataType(PropertyDataType.Text)]
        public string Proportion_cad_number { get; set; }

        #endregion //Доля Shares


        /// <summary>
        /// Восстановление права на основании судебного акта
        /// Дата ранее произведенной государственной регистрации права
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Восстановление права на основании судебного акта",
            Description = "Дата ранее произведенной государственной регистрации права")]
        public System.DateTime? ReinstatementDate { get; set; }
                

        

        ///// <summary>
        ///// Сведения о правообладателях
        ///// </summary>               
        //public virtual ICollection<RightHolder> RightHolders { get; set; }

        /// <summary>
        /// Сведения о правообладателях строкой
        /// </summary>     
        [ListView]
        [DetailView(ReadOnly = true, Name="Правообладатели")]
        [PropertyDataType(PropertyDataType.Text)]
        public string RightHoldersStr { get; set; }

        /// <summary>
        /// Имеются содольщики ФЛ.
        /// </summary>
        [DefaultValue(false)]
        [DetailView("Имеются содольщики ФЛ.")]
        public bool PersonHolder { get; set; }

        public int? SubjectRecordID { get; set; }
        [DetailView(Visible = false)]
        public virtual SubjectRecord SubjectRecord { get; set; }

       
        [DetailView(Visible = false)]
        public int? DocumentsCount { get; set; }

        ///// <summary>
        ///// Документы-основания
        ///// </summary>      
        //public virtual ICollection<DocumentRecord> DocumentRecords { get; set; }

        [DetailView(Visible = false)]
        public int? EncumbrancesCount { get; set; }

        ///// <summary>
        ///// Ограничения/Орбеменения.
        ///// </summary>
        //public virtual ICollection<RestrictRecord> Encumbrances { get; set; }

        ///// <summary>
        ///// Сведения об осуществлении государственной регистрации сделки, права, ограничения права, 
        ///// совершенных без необходимого в силу закона согласия третьего лица, органа
        ///// </summary>
        //public virtual ICollection<DealRecord> DealRecords { get; set; }


        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name="ИД ОНИ", Visible = false)]
        public int? ObjectRecordID { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name="ОНИ", Visible =false)]
        public virtual ObjectRecord ObjectRecord { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name="ИД Выписки", Visible = false)]
        public int? ExtractID { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name="Выписка", Visible =false)]
        public virtual Extract Extract { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "ИД объекта имущества", Visible = false)]
        [SystemProperty]
        public int? EstateID { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Объект имущества", Visible = false)]
        public virtual CorpProp.Entities.Estate.Estate Estate { get; set; }

    }
}
