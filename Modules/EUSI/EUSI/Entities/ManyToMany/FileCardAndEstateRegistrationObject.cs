using Base;
using CorpProp.Entities.Document;
using EUSI.Entities.Estate;

namespace EUSI.Entities.ManyToMany
{
    /// <summary>
    /// Первичные документы к заявке на регистрацию ОИ
    /// </summary>
    public class FileCardAndEstateRegistrationObject: ManyToManyAssociation<FileCard, EstateRegistration>
    {
        public FileCardAndEstateRegistrationObject(): base()
        {

        }
    }
}
