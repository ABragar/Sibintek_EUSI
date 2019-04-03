using Base;
using Base.Attributes;
using Base.Translations;
using CorpProp.Entities.Base;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Base.Utils.Common.Attributes;
using CorpProp.Helpers;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Представляет  единицу консолидации.
    /// </summary>
    [EnableFullTextSearch]
    public class Consolidation : DictObject
    {
        private static readonly CompiledExpression<Consolidation, string> _Title =
          DefaultTranslationOf<Consolidation>.Property(x => x.Title).Is(x => x.Code + " " + x.Name);

        /// <summary>
        /// Инициализирует новый экземпляр класса Consolidation.
        /// </summary>
        public Consolidation()
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

        [SystemProperty]
        public int? TypeAccountingID { get; set; }


        [ListView("Ведение учета", Visible = false)]
        [DetailView("Ведение учета", Visible = false, TabName = CaptionHelper.DefaultTabName)]
        public virtual TypeAccounting TypeAccounting { get; set; }


        /// <summary>
        /// Получает или задает ИНН.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "ИНН", Visible = false, TabName = CaptionHelper.DefaultTabName)]
        [PropertyDataType(PropertyDataType.Text)]
        [SystemProperty]
        public string INN { get; set; }


        /// <summary>
        /// Учет в мониторе закрытия
        /// </summary>
        [DetailView(Name = "Подключение к ЕУСИ", Visible = true)]
        [ListView(Name = "Подключение к ЕУСИ", Visible = true)]
        public bool ConnectToEUSI { get; set; }

    }
}
