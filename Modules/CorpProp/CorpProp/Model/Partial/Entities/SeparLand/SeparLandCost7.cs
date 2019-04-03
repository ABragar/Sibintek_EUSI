using Base;
using Base.Attributes;
using System;

namespace CorpProp.Model.Partial.Entities.SeparLand
{
    public class SeparLandCost7 : BaseObject, IChildrenModel
    {
        [SystemProperty]
        public int? ParentID { get; set; }

        /// <summary>
        /// Получает или задает дату обновления информации о стоимости.
        /// </summary>      
        [DetailView(Visible = false)]
        public DateTime? UpdateDate { get; set; }

        #region Вычесляемые поля из Изменение AccountingObjectMoving
        /// <summary>
        /// Получает или задает первоначальную стоимость в руб.
        /// </summary>      
        [DetailView(Visible = false)]
        public decimal? InitialCost { get; set; }

        /// <summary>
        /// Получает или задает остаточную стоимость в руб.
        /// </summary>       
        [DetailView(Visible = false)]
        public decimal? ResidualCost { get; set; }

        /// <summary>
        /// Получает или задает начисленную амортизацию в руб.
        /// </summary>        
        [DetailView(Visible = false)]
        public decimal? DepreciationCost { get; set; }

        /// <summary>
        /// Получает или задает срок полезного использования.
        /// </summary>       

        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string Useful { get; set; }

        /// <summary>
        /// Получает или задает оставшийся срок полезного использования.
        /// </summary>
        /// <remarks> Вычисляемое поле	Изменение</remarks>       
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string UsefulEnd { get; set; }


        /// <summary>
        /// Получает или задает стоимость выбытия в руб.
        /// </summary>       
        [DetailView(Visible = false)]
        public decimal? LeavingCost { get; set; }

        /// <summary>
        /// Получает или задает дату окончания срока полезного использования.
        /// </summary>       
        [DetailView(Visible = false)]
        public DateTime? UsefulEndDate { get; set; }

        /// <summary>
        /// Получает или задает рыночную стоимость.
        /// </summary>
        /// <remarks>Последняя актуальная рыночная стоимость объекта оценки.	ОИ.Оценка.Объект оценки</remarks>        
        [DetailView(Visible = false)]
        public decimal? MarketCost { get; set; }


        /// <summary>
        /// Получает или задает дату рыночной оценки.
        /// </summary>
        /// <remarks>Последняя актуальная дата оценки (ближайшей к текущей дате)	ОИ.Оценка</remarks>       
        [DetailView(Visible = false)]
        public DateTime? MarketDate { get; set; }


        ///// <summary>
        ///// Получает или задает реквизиты отчета об оценке.
        ///// </summary>
        ///// <remarks>Коллекция документов оценки (ближайшей к текущей дате)	ОИ.Оценка</remarks>
        ////TODO: Добавить в сервис наполнение атрибута       
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string AppraisalFileCard { get; set; }

        #endregion
    }
}
