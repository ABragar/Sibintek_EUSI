using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.ProjectActivity
{
    /// <summary>
    /// Представляет шаблон задачи.
    /// </summary>
    public class SibTaskTemplate : SibTask
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса SibTaskTemplate.
        /// </summary>
        public SibTaskTemplate()
        {
            //IsTemplate = true;
        }
    }
}
