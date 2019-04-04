using System.Linq;

namespace WebUI.Helpers
{
    public static class EstateArchiveHelper
    {
        private static readonly string[] ArchivableEstateMnemonics = {
            "InventoryObjectMenuList",
            "RealEstateMenuList",
            "CadastralMenuList",
            "LandMenuList",
            "BuildingStructureMenuList",
            "RoomMenuList",
            "CarParkingSpaceMenuList",
            "UnfinishedConstructionMenuList",
            "RealEstateComplexMenuList",
            "ShipMenuList",
            "AircraftMenuList",
            "MovableEstateMenuList",
            "VehicleMenuList",
            "IntangibleAsset"
        };
        public static bool IsArchivableEstate(string mnemonic)
        {
            return ArchivableEstateMnemonics.Contains(mnemonic);
        }
    }
}