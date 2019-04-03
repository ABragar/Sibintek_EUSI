using Base.DAL;
using CorpProp.Entities.Import;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.RosReestr.Entities;
using CorpProp.RosReestr.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Extentions.EGRP.V06
{
    /// <summary>
    /// Предоставляет методы расширения дл импорта ОНИ из xml-выписки.
    /// </summary>
    public static class ObjectExtentions
    {
        #region V06
        public static void Import(
             this ObjectRecord oni
           , SibRosReestr.EGRP.V06.ExtractSubj.TObject obj
           , ref ImportHolder holder
           )
        {
            try
            {
                if (obj == null) return;

                oni.ID_Object = obj.ID_Object;
                oni.MdfDate = obj.MdfDate;
                oni.SetCadastralNumb(obj);
                oni.TypeCode = ImportHelper.GetCodeEnum(obj.ObjectType);
                oni.TypeValue = obj.ObjectTypeText;
                oni.TypeStr = $"{oni.TypeCode} {oni.TypeValue}";
                oni.Name = obj.Name;
                oni.PurposeCode = ImportHelper.GetCodeEnum(obj.Assignation_Code);
                oni.PurposeName = obj.Assignation_Code_Text;
                oni.GroundCategory = ImportHelper.GetCodeEnum(obj.GroundCategory);
                oni.GroundCategoryText = obj.GroundCategoryText;
                oni.SetArea(obj.Area);
                oni.Inv_No = obj.Inv_No;
                oni.Floor = obj.Floor;
                oni.SetFloorPlan(obj.FloorPlan_No);
                oni.SetAddress(obj.Address);
                oni.CreateParts(obj.Complex, holder.UnitOfWork);
                oni.ReEndDate = obj.ReEndDate;
                oni.CancelDate = ImportHelper.GetDate(obj.ReEndDate);
            }
            catch (Exception ex)
            {
                holder.ImportHistory.ImportErrorLogs.AddError(ex);
            }
        }

        /// <summary>
        /// Устанавливает кадастровый/условный номер и его признак.
        /// </summary>
        /// <param name="oni"></param>
        /// <param name="obj"></param>
        private static void SetCadastralNumb(
             this ObjectRecord oni
           , SibRosReestr.EGRP.V06.ExtractSubj.TObject obj
            )
        {
            if (obj == null || String.IsNullOrEmpty(obj.Item)) return;
            oni.CadastralNumber = obj.Item;
            if (obj.ItemElementName == SibRosReestr.EGRP.V06.ExtractSubj.ItemChoiceType1.ConditionalNumber)
                oni.IsConditional = true;

        }

        /// <summary>
        /// Устанавливает значение площади.
        /// </summary>
        /// <param name="oni"></param>
        /// <param name="obj"></param>
        private static void SetArea(
            this ObjectRecord oni
          , SibRosReestr.EGRP.V06.ExtractSubj.TArea obj
           )
        {
            if (obj == null) return;
            oni.Area = obj.Area;
            oni.AreaText = obj.AreaText;
            oni.UnitCode = ImportHelper.GetCodeEnum(obj.Unit);
            oni.Unit = ImportHelper.GetNameEnum(obj.Unit);
        }



        /// <summary>
        /// Устанавливает адрес ОНИ.
        /// </summary>
        /// <param name="oni"></param>
        /// <param name="obj"></param>
        private static void SetAddress(
          this ObjectRecord oni
        , SibRosReestr.EGRP.V06.ExtractSubj.TAddress obj
         )
        {
            if (obj == null) return;
            oni.Address = obj.Content;
            oni.ID_Address = obj.ID_Address;
            if (obj.Item != null && obj.Item is SibRosReestr.EGRP.V06.ExtractSubj.TAddressRegion)
            {
                oni.RegionCode = ImportHelper.GetCodeEnum(((SibRosReestr.EGRP.V06.ExtractSubj.TAddressRegion)(obj.Item)).Code);
                oni.RegionName = ((SibRosReestr.EGRP.V06.ExtractSubj.TAddressRegion)(obj.Item)).Name;
            }
            oni.AddressOkato = obj.Code_OKATO;
            oni.AddressKladr = obj.Code_KLADR;
            oni.AddressPostal_code = obj.Postal_Code;
            oni.AddressDistrictType = obj.District?.Type;
            oni.AddressDistrictName = obj.District?.Name;
            oni.AddressDistrictStr = $"{ oni.AddressDistrictType} {oni.AddressDistrictName}";
            oni.AddressCityType = obj.City?.Type;
            oni.AddressCityName = obj.City?.Name;
            oni.AddressCityStr = $"{oni.AddressCityType} {oni.AddressCityName}";
            oni.AddressUrban_districtType = obj.Urban_District?.Type;
            oni.AddressUrban_districtName = obj.Urban_District?.Name;
            oni.AddressUrban_districtStr = $"{oni.AddressUrban_districtType} {oni.AddressUrban_districtName}";
            oni.AddressSoviet_villageType = obj.Soviet_Village?.Type;
            oni.AddressSoviet_villageName = obj.Soviet_Village?.Name;
            oni.AddressSoviet_villageStr = $"{oni.AddressSoviet_villageType} {oni.AddressSoviet_villageName}";
            oni.AddressLocalityType = obj.Locality?.Type;
            oni.AddressLocalityName = obj.Locality?.Name;
            oni.AddressLocalityStr = $"{oni.AddressLocalityType} {oni.AddressLocalityName}";
            oni.AddressStreetType = obj.Street?.Type;
            oni.AddressStreetName = obj.Street?.Name;
            oni.AddressStreetStr = $"{oni.AddressStreetType} {oni.AddressStreetName}";
            oni.AddressLevel1Type = obj.Level1?.Type;
            oni.AddressLevel1Name = obj.Level1?.Name;
            oni.AddressLevel1Str = $"{oni.AddressLevel1Type} {oni.AddressLevel1Name}";
            oni.AddressLevel2Type = obj.Level2?.Type;
            oni.AddressLevel2Name = obj.Level2?.Name;
            oni.AddressLevel2Str = $"{oni.AddressLevel2Type} {oni.AddressLevel2Name}";
            oni.AddressLevel3Type = obj.Level3?.Type;
            oni.AddressLevel3Name = obj.Level3?.Name;
            oni.AddressLevel3Str = $"{oni.AddressLevel3Type} {oni.AddressLevel3Name}";
            oni.AddressApartmentType = obj.Apartment?.Type;
            oni.AddressApartmentName = obj.Apartment?.Name;
            oni.AddressApartmentStr = $"{oni.AddressApartmentType} {oni.AddressApartmentStr}";
            oni.AddressOther = obj.Other;
        }
        #endregion//V06
    }
}
