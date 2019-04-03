using Base.Attributes;
using Base.DAL;
using CorpProp.Entities.Accounting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.Estate
{
    /// <summary>
    /// Представляет объект недвижимого имущества, не являющийся кадастровым.
    /// </summary>
    [Table("NonCadastral")]
    public class NonCadastral : RealEstate
    {
        public NonCadastral() : base()
        {
            
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса NonCadastral из объекта БУ.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="obj">Объект БУ.</param>
        public NonCadastral(IUnitOfWork uofw, AccountingObject obj) : base (uofw, obj)
        {

        }
    }
}
