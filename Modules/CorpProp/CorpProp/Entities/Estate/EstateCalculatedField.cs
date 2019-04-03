using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base;
using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Entities.Subject;

namespace CorpProp.Entities.Estate
{
    /// <summary>
    /// Представляет вычисмляемые данные по объекту имущетсва.
    /// </summary>
    [EnableFullTextSearch]
    public class EstateCalculatedField : BaseObject
    {
        public EstateCalculatedField() : base()
        {
        }

        /// <summary>
        /// Получает или задает кол-во дочерних объектов.
        /// </summary>
        public int? ChildObjectsCount { get; set; }

        /// <summary>
        /// Получает или задает первоначальную стоимость.
        /// </summary>
        public decimal? InitialCostSumOBU { get; set; }

        /// <summary>
        /// Получает или задает остаточную стоимость.
        /// </summary>
        public decimal? ResidualCostSumOBU { get; set; }

        /// <summary>
        /// Получает или задает первоначальную стоимость по НУ.
        /// </summary>
        public decimal? InitialCostSumNU { get; set; }

        /// <summary>
        /// Получает или задает остаточную стоимость по НУ.
        /// </summary>
        public decimal? ResidualCostSumNU { get; set; }


        /// <summary>
        /// Получает или задает ИД инвентарного объекта.
        /// </summary>
        public int? EstateID { get; set; }


        /// <summary>
        /// Получает или задает ИД балансодержателя.
        /// </summary>
        [SystemProperty]
        public int? OwnerID { get; set; }

        /// <summary>
        /// Получает или задает балансодержателя.
        /// </summary>
        [FullTextSearchProperty]
        public Society Owner { get; set; }


        /// <summary>
        /// Получает или задает ИД пользователя.
        /// </summary>
        [SystemProperty]
        public int? WhoUseID { get; set; }

        /// <summary>
        /// Получает или задает пользователя.
        /// </summary>
        [FullTextSearchProperty]
        public Society WhoUse { get; set; }

        /// <summary>
        /// Получает или задает Реквизыты договора
        /// </summary>
        [FullTextSearchProperty]
        public string DealProps { get; set; }

        /// <summary>
        /// Получает или задает идентификатор балансодержателя.
        /// </summary> 
        [SystemProperty]
        public int? MainOwnerID { get; set; }

        /// <summary>
        /// Получает или задает владельца/Собственник.
        /// </summary>
        [FullTextSearchProperty]
        public Society MainOwner { get; set; }
    }
}
