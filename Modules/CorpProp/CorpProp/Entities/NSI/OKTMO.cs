using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorpProp.Entities.Base;
using Base;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Base.Attributes;
using Base.Translations;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Представляет справочник ОКТМО.
    /// </summary>
    [EnableFullTextSearch]
    public class OKTMO : DictObject
    {
        private static readonly CompiledExpression<OKTMO, string> _Title =
           DefaultTranslationOf<OKTMO>.Property(x => x.Title).Is(x => x.Code + " " + x.Name);

        /// <summary>
        /// Инициализиреует новый экземпляр класса OKTMO.
        /// </summary>
        public OKTMO()
        {

        }

        /// <summary>
        /// Получает код + наименование.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Наименование", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [SystemProperty]
        public string Title => _Title.Evaluate(this);

    }
}
