using System;
using System.Data.SqlTypes;
using Base.Attributes;
using Base.EntityFrameworkTypes.Complex;
using CorpProp.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Subject;

namespace CorpProp.Analyze.Entities.Accounting
{
    /// <summary>
    /// Описывает банковский счёт
    /// </summary>
    public class BankAccount : TypeObject
    {
        public BankAccount() : base()
        {
        }

        public int? SocietyID { get; set; }

        /// <summary>
        /// Получает и задаёт общество группы которому принадлежит счёт
        /// </summary>
        public Society Society { get; set; }

        /// <summary>
        /// Получает или задает количество счетов.
        /// </summary>
        public int? AccountCount { get; set; }

        /// <summary>
        /// Получает или задает наименование банка.
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        public string BankName { get; set; }

        /// <summary>
        /// Получает или задает БИК банка.
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        public string BIK { get; set; }

        /// <summary>
        /// Получает или задает адрес банка.
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        public string Addres { get; set; }

        /// <summary>
        /// Получает или задает вид счёта.
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        public string AccountType { get; set; }

        /// <summary>
        /// Получает или задает ИД валюты счёта.
        /// </summary>
        public int? CurrencyID { get; set; }

        /// <summary>
        /// Получает или задает валюту счёта.
        /// </summary>
        public Currency Currency { get; set; }

        /// <summary>
        /// Получает или задаёт средний дневной оборот, руб.
        /// </summary>
        public decimal? AvgOfDay { get; set; }

        /// <summary>
        /// Получает или задает количество операций.
        /// </summary>
        public int? OperationCount { get; set; }
    }
}