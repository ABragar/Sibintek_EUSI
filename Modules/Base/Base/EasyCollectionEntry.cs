using System.ComponentModel.DataAnnotations.Schema;

namespace Base
{
    public interface IEasyCollectionEntry
    {
        int? ObjectID { get; set; }
    }

    public abstract class EasyCollectionEntry<TEntity> : BaseObject, IEasyCollectionEntry where TEntity : BaseObject
    {
        [ForeignKey("ObjectID")]
        public virtual TEntity Object { get; set; }
        public int? ObjectID { get; set; }
    }
}