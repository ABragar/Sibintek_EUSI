using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base;
using Base.Attributes;
using Base.UI.ViewModal;

namespace CorpProp.Entities.Document
{
    public enum AccessModifier
    {
        ///<summary>Только для Автора документа</summary>
        AuthorOnly = 1,
        /// <summary>
        /// Для всех пользователей ОГ, к которому привязан Автор
        /// </summary>
        AuthorSociety = 2,
        /// <summary>
        /// Для всех пользователей ОГ с равным набором ролей(структур подразд. ОГ ???), к которому привязан Автор (Не уточнено..)
        /// </summary>
        AuthorSocietyWithEqualRoles = 3,
        /// <summary>
        /// Для всех пользователей АИС КС
        /// </summary>
        Everyone = 4
    }
    
    public class FileCardPermission: BaseObject
    {
        [ListView("Имя разграничения прав")]
        [DetailView("Имя разграничения права")]
        public string Name { get; set; }

        public AccessModifier AccessModifier { get; set; }

    }
}
