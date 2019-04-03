using Base;
using Base.Attributes;
using CorpProp.Entities.Subject;

namespace CorpProp.Entities.Access
{
    public class SibPermission: BaseObject
    {
        
        public Society Society { get; set; }
        public int SocietyID { get; set; }
        [DetailView(Name = "Объект")]
        [ListView]
        [PropertyDataType(PropertyDataType.ListBaseObjects)]
        public string ObjectMnemonic { get; set; }
        public int ObjectId { get; set; }

    }
}
