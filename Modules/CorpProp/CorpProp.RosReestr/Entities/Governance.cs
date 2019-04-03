using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Entities
{
    /// <summary>
    /// Субъект правоотношений.
    /// </summary>
    public class Governance : SubjectRecord
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса Governance.
        /// </summary>
        public Governance(): base() { }


        #region TGovernance

        /// <summary>
        /// Код ОКАТО
        /// </summary>
        public string OKATO_Code { get; set; }
        /// <summary>
        /// Номер регистрационной записи
        /// </summary>
        public string RegNumber { get; set; }
        /// <summary>
        /// Дата государственной регистрации
        /// </summary>
        public string RegDate { get; set; }
        /// <summary>
        /// Полное наименование страны регистрации
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// Адрес в стране регистрации
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Зарегистрированный адрес субъекта
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Фактический адрес субъекта
        /// </summary>
        public string FactLocation { get; set; }
        #endregion//TGovernance
    }
}
