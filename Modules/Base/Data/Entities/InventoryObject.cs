using Base.ComplexKeyObjects.Superb;
using Data.Enums;

namespace Data.Entities
{
    public class InventoryObject : PropertyObject, ISuperObject<InventoryObject>
    {
        public string ExtraID { get; }

        //public int? PropertyComplexID { get; set; }
        //public virtual PropertyComplex PropertyComplex { get; set; }

        //public ObjectReadinessStage ObjectReadinessStage { get; set; }

        //public int? ClassObjectID { get; set; }
        //public virtual ClassObject ClassObject { get; set; }
    }
}
