using Base.Attributes;
using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.Estate
{
    /// <summary>
    /// Представляет часть кадастрового объекта.
    /// </summary>
    [EnableFullTextSearch]
    public class CadastralPart : TypeObject
    {
        public CadastralPart() : base() { }

        /// <summary>
        /// Порядковый номер части
        /// </summary>       
        [FullTextSearchProperty]
        [ListView]
        [DetailView(ReadOnly = true, Name = "Порядковый номер части")]
        [PropertyDataType(PropertyDataType.Text)]
        public string NumberPart { get; set; }

        /// <summary>
        /// Номер реестровой записи
        /// </summary>       
        [FullTextSearchProperty]
        [ListView]
        [DetailView(ReadOnly = true, Name = "Номер реестровой записи")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Number { get; set; }

        /// <summary>
        /// Номер регистрации ограничения права или обременения ОН
        /// </summary>       
        [FullTextSearchProperty]
        [ListView]
        [DetailView(ReadOnly = true, Name = "Номер регистрации ограничения права или обременения ОН")]
        [PropertyDataType(PropertyDataType.Text)]
        public string NumberEncumbrance { get; set; }

        /// <summary>
        /// Дата регистрации
        /// </summary>        
        [FullTextSearchProperty]
        [ListView]
        [DetailView(ReadOnly = true, Name = "Дата регистрации")]
        //[PropertyDataType(PropertyDataType.Text)]
        public System.DateTime? RegDate { get; set; }

        /// <summary>
        /// Реестровый номер границы 
        /// </summary>       
        [FullTextSearchProperty]
        [ListView]
        [DetailView(ReadOnly = true, Name = "Реестровый номер границы")]
        [PropertyDataType(PropertyDataType.Text)]
        public string NumberBorder { get; set; }


       
        public int? CadastralID { get; set; }
       
        public Cadastral Cadastral { get; set; }

        


    }
}
