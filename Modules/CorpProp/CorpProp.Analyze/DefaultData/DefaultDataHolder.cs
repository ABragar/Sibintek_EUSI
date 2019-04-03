using System.Collections.Generic;
using System.Xml.Serialization;
using CorpProp.Analyze.Entities.NSI;
using CorpProp.DefaultData;
using CorpProp.Entities.NSI;
using CorpProp.Entities.ProjectActivity;

namespace CorpProp.Analyze.DefaultData
{
    [DataHolder(@"CorpProp.Analyze.DefaultData.XML.DefaultDataHolder.xml")]
    public class DefaultDataHolder
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса DefaultDataHolder.
        /// </summary>
        public DefaultDataHolder()
        {
        }

        /// <summary>
        /// Получает или задает дефолтные строки бюджета.
        /// </summary>
        [XmlArray("BudgetLines")]
        [XmlArrayItem("BudgetLine")]
        public List<BudgetLine> BudgetLines { get; set; }

        /// <summary>
        /// Получает или задает дефолтные финансовые показатели.
        /// </summary>
        [XmlArray("FinancialIndicators")]
        [XmlArrayItem("FinancialIndicator")]
        public List<FinancialIndicator> FinancialIndicators { get; set; }

        /// <summary>
        /// Получает или задает дефолтные справочники. 
        /// </summary>        
        [XmlArray("NSIs")]
        [XmlArrayItem("NSI")]
        public List<NSI> NSIs { get; set; }


    }
}