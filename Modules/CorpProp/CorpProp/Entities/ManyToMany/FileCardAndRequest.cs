using Base;
using CorpProp.Entities.Document;

namespace CorpProp.Entities.ManyToMany
{

    /// <summary>
    /// Представляет связь запросов с документами.
    /// </summary>   
    public class FileCardAndRequest : ManyToManyAssociation<FileCard, CorpProp.Entities.Request.Request>
    {

        public FileCardAndRequest() : base()
        {

        }
    }
}
