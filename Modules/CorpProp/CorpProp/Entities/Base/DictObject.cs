using Base;
using Base.Attributes;
using CorpProp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.NSI;
using Base.DAL;
using System.Xml.Serialization;

namespace CorpProp.Entities.Base
{
    /// <summary>
    /// Представляет базовый класс справочника системы.
    /// </summary>
    [EnableFullTextSearch]
    public class DictObject : TypeObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса DictObject.
        /// </summary>
        public DictObject() : base()
        {

        }



        /// <summary>
        /// Инициализирует новый экземпляр класса DictObject.
        /// </summary>
        /// <param name="name">Наименование.</param>
        public DictObject(string name)
        {
            Name = name;
        }





        /// <summary>
        /// Получает или задает наименование.
        /// </summary>
        [ListView(Order = 2, Width = 100, Visible = false)]
        [FullTextSearchProperty]
        [DetailView(Name = "Наименование", TabName = CaptionHelper.DefaultTabName, Order = 1, Required = true, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [SystemProperty]
        public string Name { get; set; }

        /// <summary>
        /// Получает или задает системный код.
        /// </summary>
        [ListView(Order = 1, Width = 100, Visible = false)]
        [DetailView(Name = "ИД мастер системы", TabName = CaptionHelper.DefaultTabName, Order = 2, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        [SystemProperty]
        public string ExternalID { get; set; }

        /// <summary>
        /// Получает или задает системный код.
        /// </summary>
        [ListView(Order = 1, Width = 100, Visible = false)]
        [DetailView(Name = "ИД вышестоящего элемента мастер системы", TabName = CaptionHelper.DefaultTabName, Order = 2, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        [SystemProperty]
        public string ExternalParentID { get; set; }


        /// <summary>
        /// Получает или задает системный код.
        /// </summary>
        [ListView(Order = 1, Width = 100, Visible = false)]
        [DetailView(Name = "Системный код", TabName = CaptionHelper.DefaultTabName, Order = 2, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        [SystemProperty]
        public string Code { get; set; }

        /// <summary>
        /// Получает или задает публичный код.
        /// </summary>
        [ListView(Order = 1, Width = 100, Visible = false)]
        [DetailView(Name = "Код", TabName = CaptionHelper.DefaultTabName, Order = 2, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        [SystemProperty]
        public string PublishCode { get; set; }

        /// <summary>
        /// Получает или задает признак по умолчанию.
        /// </summary>
        [DetailView(Name = "По умолчанию", TabName = CaptionHelper.DefaultTabName, Order = 3, Visible = false)]
        [DefaultValue(false)]
        public bool IsDefault { get; set; }

        /// <summary>
        /// Получает или задает дату начала.
        /// </summary>
        [ListView(Order = 5, Hidden = true)]
        [DetailView(Name = "Дата начала", TabName = CaptionHelper.DefaultTabName, Order = 4, Required = true, Visible = false)]
        public DateTime? DateFrom { get; set; }

        /// <summary>
        /// Получает или задает дату окончания участия.
        /// </summary>
        [ListView(Order = 6, Hidden = true)]
        [DetailView(Name = "Дата окончания", TabName = CaptionHelper.DefaultTabName, Order = 5, Visible = false)]
        public DateTime? DateTo { get; set; }

        [SystemProperty]
        public int? DictObjectStateID { get; set; }

        /// <summary>
        /// Получает или задает состояние актуальности
        /// </summary>
        [ListView(Order = 3, Hidden = true)]
        [DetailView(Name = "Признак", TabName = CaptionHelper.DefaultTabName, Order = 6, ReadOnly = true, Visible = false)]
        public virtual DictObjectState DictObjectState { get; set; }

        [SystemProperty]
        public int? DictObjectStatusID { get; set; }

        /// <summary>
        /// Получает или задает статус элемента справочника
        /// </summary>
        [ListView(Order = 4, Hidden = true)]
        [DetailView(Name = "Статус", TabName = CaptionHelper.DefaultTabName, Order = 7, Visible = false)]
        public virtual DictObjectStatus DictObjectStatus { get; set; }


        public void TrimCode()
        {
            if (!String.IsNullOrEmpty(PublishCode))
                PublishCode = PublishCode.ToUpper().Trim(' ');

            if (String.IsNullOrEmpty(Code))
                Code = PublishCode;

        }

        public void ChangeState(IUnitOfWork unitOfWork, string code)
        {

            code = (string.IsNullOrEmpty(code)) ? "" : code;

            if (DictObjectStatus != null && code != null)
            {
                DictObjectState newState = null;

                //Если статус изменён на "Согласовано добавление" значение поля "Признак" устанавливается "Актуальный";
                //Если статус изменён с "Запрос на удаление" на "Запрос отклонён" значение поля "Признак" устанавливается "Актуальный";
                if (DictObjectStatus.Code == "AddConfirm" || (code == "RequestDeny" && !String.IsNullOrEmpty(code) && code.ToLower() == "delrequest"))
                {
                    newState = unitOfWork.GetRepository<DictObjectState>().Find(f => f.Code == "NotOld");
                    if(DateFrom ==null || DateFrom.Value == DateTime.MinValue)
                        DateFrom = DateTime.Now;
                }

                //Если статус изменён на "Согласовано удаление" значение поля "Признак" устанавливается "Устаревший";
                if (DictObjectStatus.Code == "DelConfirm")
                {
                    newState = unitOfWork.GetRepository<DictObjectState>().Find(f => f.Code == "Old");
                    if (DateTo == null || DateTo.Value == DateTime.MinValue)
                        DateTo = DateTime.Now;
                }

                //Если статус изменён с "Запрос на добавление" на "Запрос отклонён" значение поля "Признак" устанавливается "Временный";
                if (DictObjectStatus.Code == "RequestDeny" && !String.IsNullOrEmpty(code) && code.ToLower() == "addrequest")
                {
                    newState = unitOfWork.GetRepository<DictObjectState>().Find(f => f.Code == "Temporary");
                    if (DateFrom == null || DateFrom.Value == DateTime.MinValue)
                        DateFrom = DateTime.Now;
                }

                DictObjectState = newState;
                DictObjectStateID = newState?.ID;
            }
        }
    }
}
