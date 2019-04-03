using Data.Enums;
using System;

namespace Data.Entities
{
    public class Vehicles : InventoryObject
    {
        public KindOfVessel KindOfVessel { get; set; }

        //public VehicleCategory VehiclesCategory { get; set; }

        public VehiclePowerMeasurementUnit VehiclePowerMeasurementUnit { get; set; }

        public int VehiclePower { get; set; }

        public string SerialNumber { get; set; }

        public bool DieselEngine { get; set; }

        public int EngineCapacity { get; set; }

        public string VehicleBrand { get; set; }

        public int YearOfManufacture { get; set; }

        public string NumberOfStateRegistration { get; set; }

        public DateTime DateOfStateRegistration { get; set; }

        public DateTime DateOfRemoval { get; set; }

        public bool AccountingInPLATON { get; set; }


    }
}
