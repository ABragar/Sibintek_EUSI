using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorpProp.Entities.Base;
using Base;
using Base.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Base.Translations;
using Base.Utils.Common.Attributes;
using CorpProp.Helpers;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Представляет справочник ОКОФ (версия 1994г.).
    /// </summary>
    [EnableFullTextSearch]
    public class OKOF94 : DictObject
    {
        private static readonly CompiledExpression<OKOF94, string> _Title =
           DefaultTranslationOf<OKOF94>.Property(x => x.Title).Is(x => x.Code + " " + x.Name);

        /// <summary>
        /// Инициализирует новый экземпляр класса OKOF94.
        /// </summary>
        public OKOF94()
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

        /// <summary>
        /// Получает или задает дополнительный код.
        /// </summary>
        [ListView(Order = 2, Width = 100, Visible = true)]
        [DetailView(Name = "Доп код ОКОФ", TabName = CaptionHelper.DefaultTabName, Order = 8)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string AdditionalCode { get; set; }
    }

    /// <summary>
    /// Представляет справочник ОКОФ (версия 2014г.).
    /// </summary>
    [EnableFullTextSearch]
    public class OKOF2014 : DictObject
    {
        private static readonly CompiledExpression<OKOF2014, string> _Title =
             DefaultTranslationOf<OKOF2014>.Property(x => x.Title).Is(x => x.Code + " " + x.Name);

        /// <summary>
        /// Инициализирует новый экземпляр класса OKOF2014.
        /// </summary>
        public OKOF2014()
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

        /// <summary>
        /// Получает или задает дополнительный код.
        /// </summary>
        [ListView(Order = 2, Width = 100, Visible = true)]
        [DetailView(Name = "Доп код ОКОФ", TabName = CaptionHelper.DefaultTabName, Order = 8)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string AdditionalCode { get; set; }
    }

    /// <summary>
    /// Представляет справочник дополнительных кодов ОКОФ.
    /// </summary>
    [EnableFullTextSearch]
    public class AddonOKOF : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса AddonOKOF.
        /// </summary>
        public AddonOKOF() : base()
        {
        }

        /// <summary>
        /// Получает или задает ИД ОКОФ-1994.
        /// </summary>   
        [SystemProperty]
        public int? OKOF94ID { get; set; }

        /// <summary>
        /// Получает или задает ОКОФ-1994.
        /// </summary>             
        [ListView(Name = "ОКОФ", Visible = true)]
        [DetailView(Name = "ОКОФ", Visible = true)]
        public virtual OKOF94 OKOF94 { get; set; }

    }

    /// <summary>
    /// Представляет справочник дополнительных кодов ОКОФ-2.
    /// </summary>
    public class AddonOKOF2014 : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса AddonOKOF2014.
        /// </summary>
        public AddonOKOF2014() : base()
        {
        }

        /// <summary>
        /// Получает или задает ИД ОКОФ-2014.
        /// </summary>   
        [SystemProperty]
        public int? OKOF2014ID { get; set; }

        /// <summary>
        /// Получает или задает ОКОФ-2014.
        /// </summary>             
        [ListView(Name = "ОКОФ2", Visible = true)]
        [DetailView(Name = "ОКОФ2", Visible = true)]
        public virtual OKOF2014 OKOF2014 { get; set; }
    }
}
