using System.ComponentModel.DataAnnotations.Schema;
using Base;

namespace CorpProp.Entities.Estate
{
    public class EstateImage : FileCollectionItem
    {
        public int? EstateID { get; set; }

        [ForeignKey("EstateID")]
        public Estate Estate { get; set; }
    }
}
