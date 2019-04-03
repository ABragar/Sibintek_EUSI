using Base.Attributes;
using Base.Translations;
using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Представляет справочник классов объектов имущества.
    /// </summary>
    /// <remarks>
    /// Внутренний справочник. На этапе первоначального импорта данных 
    /// заполняется на основании имеющихся в учётных системах классификаторов 
    /// (классОС, ОКОФ и пр.), в дальнейшем может быть уточнён пользователем.
    /// </remarks>
    [EnableFullTextSearch]
    public class EstateType : DictObject
    {

        private static readonly CompiledExpression<EstateType, string> _Title =
          DefaultTranslationOf<EstateType>.Property(x => x.Title).Is(x => x.Code + " " + x.Name);

        /// <summary>
        /// Инициализирует новый экземпляр класса EstateType.
        /// </summary>
        public EstateType()
        {

        }

        // <summary>
        /// Получает код + наименование.
        /// </summary>
        [ListView(Visible = false)]
        [DetailView(Name = "Наименование", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [SystemProperty]
        public string Title => _Title.Evaluate(this);

    }
}
