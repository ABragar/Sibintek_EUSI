using Base.DAL;
using CorpProp.Entities.Accounting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUSI.Entities.Accounting
{

    /// <summary>
    /// Расширенное SQL-View ОС/НМА.
    /// </summary>
    [NotMapped]
    public class AccountingObjectExtView : AccountingObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса AccountingObjectExtView.
        /// </summary>
        public AccountingObjectExtView() : base()
        {
        }

        /// <summary>
        /// Получает или задает номер ЕУСИ связанного ОИ.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Получает или задает код БЕ.
        /// </summary>
        public new string ConsolidationCode { get; set; }

        /// <summary>
        /// Получает или задает код статуса РСБУ.
        /// </summary>
        public string StateObjectRSBUCode { get; set; }

        public override void OnSaving(IUnitOfWork uow, object entry)
        {
            return;
        }
    }
}
