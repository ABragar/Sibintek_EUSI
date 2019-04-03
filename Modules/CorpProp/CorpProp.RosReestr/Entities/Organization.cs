using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Entities
{
    /// <summary>
    /// Юридическое лицо
    /// </summary>
    public class Organization : SubjectRecord
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса Organization.
        /// </summary>
        public Organization() : base()
        {

        }

        #region TOrganization

        /// <summary>
        /// Дата модификации
        /// </summary>
        public string MdfDate { get; set; }

        /// <summary>
        /// Код ОПФ
        /// </summary>
        public string Code_OPF { get; set; }
        public string Code_OPFName { get; set; }

        /// <summary>
        /// Код ОГРН (Регистрационный номер в стране регистрации (инкорпорации))
        /// </summary>
        public string Code_OGRN { get; set; }


        /// <summary>
        /// орган регистрации (наименование регистрирующего органа)
        /// </summary>
        public string AgencyRegistration { get; set; }

        /// <summary>
        /// Код КПП
        /// </summary>
        public string Code_CPP { get; set; }


        #endregion //TOrganization
    }
}
