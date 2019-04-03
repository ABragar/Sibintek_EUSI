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
using CorpProp.Helpers;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Представляет справочник ОКАТО.
    /// </summary>
    [EnableFullTextSearch]
    public class OKATO : DictObject
    {
        private static readonly CompiledExpression<OKATO, string> _Title =
         DefaultTranslationOf<OKATO>.Property(x => x.Title).Is(x => x.Code + " " + x.Name);

        /// <summary>
        /// Инициализирует новый экземпляр класса OKATO.
        /// </summary>
        public OKATO()
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
