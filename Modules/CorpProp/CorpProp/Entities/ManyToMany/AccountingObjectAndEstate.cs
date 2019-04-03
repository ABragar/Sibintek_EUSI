using Base;
using CorpProp.Entities.Accounting;

namespace CorpProp.Entities.ManyToMany
{
    /// <summary>
    /// Представляет связь ОБУ с ОИ в отношении М:М.
    /// </summary>
    /// <remarks>
    /// В обычной ситуации один ОБУ может быть связан только с одним ОИ. 
    /// Но, есть исключительные ситуации, где якобы один ОБУ может быть связан с несколькими ОИ.
    /// Пока методологи не проработали логику, требуется отобразить такие связи, для чего добавлен этот класс.
    /// </remarks>
    public class AccountingObjectAndEstate : ManyToManyAssociation<AccountingObject, Estate.Estate>
    {
        public AccountingObjectAndEstate() : base()
        {

        }

    }
}
