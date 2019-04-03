using Data.Enums;

namespace Data.Entities
{
    public class CadastralObject : InventoryObject
    {
        public int CadastralNumber { get; set; }

        public int? CadastralValueID { get; set; }

        public virtual CadastralValue CadastralValue { get; set; }

        public string Address { get; set; }

        public bool IsNumberNominal { get; set; }

        public RealtyType RealtyType { get; set; }

        public string RegistrationNumber { get; set; }

        public MainCharacteristicType MainCharacteristicType { get; set; }

        public double MainCharacteristicValue { get; set; }

        public MainCharacteristicMeasurementUnit MainCharacteristicMeasurementUnit { get; set; }

        public string SpecialNotes { get; set; }
    }
}
