using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Entities
{
    /// <summary>
    /// Физическое лицо.
    /// </summary>
    public class TPerson : SubjectRecord
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса TPerson.
        /// </summary>
        public TPerson(): base() { }



        #region TPerson

        /// <summary>
        /// Гражданство
        /// </summary>
        public string Citizen { get; set; }

        /// <summary>
        /// Пол
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// СНИЛС
        /// </summary>
        public string SNILS { get; set; }
        #endregion//TPerson
    }
}
