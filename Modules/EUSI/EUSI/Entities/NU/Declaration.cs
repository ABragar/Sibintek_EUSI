using Base.Attributes;
using Base.ComplexKeyObjects.Superb;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Entities.Document;
using System;

namespace EUSI.Entities.NU
{
    /// <summary>
    /// Налоговая декларация
    /// </summary>
    [EnableFullTextSearch]
    public class Declaration : TypeObject, ISuperObject<Declaration>
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса Declaration.
        /// </summary>
        public Declaration() : base()
        {

        }

        [PropertyDataType(PropertyDataType.ExtraId)]
        public string ExtraID { get; } = null;

        /// <summary>
        /// Идентификатор файла
        /// </summary>
        /// <remarks>
        /// Содержит (повторяет) имя сформированного файла (без расширения)
        /// </remarks>  
        [ListView("Наименование файла")]
        [DetailView("Наименование файла")]
        [PropertyDataType(PropertyDataType.Text)]
        public string FileName { get; set; }

        /// <summary>
        /// Получает или задает Ид файла декларации.
        /// </summary>
        [DetailView(Visible = false)]
        public int? FileCardID { get; set; }

        /// <summary>
        /// Файл декларации
        /// </summary>
        
        [DetailView("Файл"), ListView(Visible = false)]
        public virtual FileCard FileCard { get; set; }
    

        /// <summary>
        /// Версия программы, с помощью которой сформирован файл
        /// </summary>
        [ListView("Версия программы")]
        [DetailView("Версия программы")]
        [PropertyDataType(PropertyDataType.Text)]
        public string SysVersion { get; set; }


        /// <summary>
        /// Версия формата
        /// </summary>
        [ListView("Версия формата")]
        [DetailView("Версия формата")]
        [PropertyDataType(PropertyDataType.Text)]
        public string FormatVersion { get; set; }


        /// <summary>
        /// Код формы отчетности по КНД
        /// </summary>
        [ListView("Код формы отчетности по КНД")]
        [DetailView("Форма КНД (код)", Description ="Код формы отчетности по КНД")]
        [PropertyDataType(PropertyDataType.Text)]
        public string KND { get; set; }

        /// <summary>
        /// Дата формирования документа
        /// </summary>
        [ListView("Дата формирования документа")]
        [DetailView("Дата формирования документа")]
        [PropertyDataType(PropertyDataType.Text)]
        public DateTime? FileDate { get; set; }

        /// <summary>
        /// Налоговый период (код)
        /// </summary>
        [ListView("Налоговый период (код)")]
        [DetailView("Налоговый период (код)")]
        [PropertyDataType(PropertyDataType.Text)]
        public string PeriodCode { get; set; }

        /// <summary>
        /// Отчетный год
        /// </summary>
        [ListView("Отчетный год")]
        [DetailView("Отчетный год")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Year { get; set; }

        /// <summary>
        /// Код налогового органа, в который представляется документ
        /// </summary>
        [ListView("Налоговый орган")]
        [DetailView("Налоговый орган", Description ="Код налогового органа, в который представляется документ")]
        [PropertyDataType(PropertyDataType.Text)]
        public string AuthorityCode { get; set; }

        /// <summary>
        /// Номер корректировки
        /// </summary>
        [ListView("Номер корректировки")]
        [DetailView("Номер корректировки")]        
        public int? CorrectionNumb { get; set; }

        /// <summary>
        /// Код места нахождения (учета), по которому представляется документ
        /// </summary>
        [ListView("Место нахождение НО (код)")]
        [DetailView("Место нахождение НО (код)", Description = "Код места нахождения (учета) налогового органа, по которому представляется документ")]
        [PropertyDataType(PropertyDataType.Text)]
        public string LocationCode { get; set; }



        /// <summary>
        /// Наименование организации
        /// </summary>
        [ListView("Наименование организации")]
        [DetailView("Наименование организации")]
        [PropertyDataType(PropertyDataType.Text)]
        public string SubjectName { get; set; }

        /// <summary>
        /// ИНН организации
        /// </summary>
        [ListView("ИНН организации")]
        [DetailView("ИНН организации")]
        [PropertyDataType(PropertyDataType.Text)]
        public string INN { get; set; }
        
        /// <summary>
        /// КПП
        /// </summary>
        [ListView("КПП")]
        [DetailView("КПП")]
        [PropertyDataType(PropertyDataType.Text)]
        public string KPP { get; set; }

        /// <summary>
        /// Номер контактного телефона
        /// </summary>
        [ListView("Номер контактного телефона")]
        [DetailView("Номер контактного телефона")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Phone { get; set; }


        /// <summary>
        /// Код формы реорганизации (ликвидация)
        /// </summary>
        /// <summary>
        /// Принимает значение:
        /// 0 – ликвидация   |
        /// 1 – преобразование   |
        /// 2 – слияние   |
        /// 3 – разделение   |
        /// 5 – присоединение   |
        /// 6 – разделение с одновременным присоединением
        /// </summary>
        [ListView("Форма реорганизации (ликвидация) код")]
        [DetailView("Код формы реорганизации (ликвидация)")]
        [PropertyDataType(PropertyDataType.Text)]
        public string ReorgFormCode { get; set; }

        /// <summary>
        /// ИНН реорганизованной организации
        /// </summary>
        [ListView("ИНН реорганизованной организации")]
        [DetailView("ИНН реорганизованной организации")]
        [PropertyDataType(PropertyDataType.Text)]
        public string ReorgINN { get; set; }

        /// <summary>
        /// КПП реорганизованной организации.
        /// </summary>
        [ListView("КПП реорганизованной организаци")]
        [DetailView("КПП реорганизованной организаци")]
        [PropertyDataType(PropertyDataType.Text)]
        public string ReorgKPP { get; set; }


        /// <summary>
        /// Подписант - Фамилия
        /// </summary>
        [ListView("Фамилия")]
        [DetailView("Фамилия")]
        [PropertyDataType(PropertyDataType.Text)]
        public string LastName { get; set; }

        /// <summary>
        /// Подписант - Имя
        /// </summary>
        [ListView("Имя")]
        [DetailView("Имя")]
        [PropertyDataType(PropertyDataType.Text)]
        public string FirstName { get; set; }

        /// <summary>
        /// Подписант - Отчество
        /// </summary>
        [ListView("Отчество")]
        [DetailView("Отчество")]
        [PropertyDataType(PropertyDataType.Text)]
        public string MiddleName { get; set; }

        /// <summary>
        /// Наименование и реквизиты документа, подтверждающего полномочия представителя
        /// </summary>
        [ListView("Наименование и реквизиты документа, подтверждающего полномочия представителя")]
        [DetailView("Наименование и реквизиты документа, подтверждающего полномочия представителя")]
        [PropertyDataType(PropertyDataType.Text)]
        public string RepresentDoc { get; set; }

        /// <summary>
        /// Наименование организации представителя налогоплательщика
        /// </summary>
        [ListView("Наименование организации представителя налогоплательщика")]
        [DetailView("Организация представителя", Description ="Наименование организации представителя налогоплательщика")]
        [PropertyDataType(PropertyDataType.Text)]
        public string RepresentOrg { get; set; }

        /// <summary>
        /// Признак лица, подписавшего документ
        /// </summary>
        /// <summary>
        /// Принимает значение:
        /// 1 – налогоплательщик   |
        /// 2 – представитель налогоплательщика
        /// </summary>
        [ListView("Признак лица, подписавшего документ")]
        [DetailView("Признак лица, подписавшего документ")]
        [PropertyDataType(PropertyDataType.Text)]
        public string RepresentType { get; set; }
    }
}
