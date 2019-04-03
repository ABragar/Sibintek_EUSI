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

namespace CorpProp.RosReestr.Extentions.EGRN.Unknown
{
    /// <summary>
    /// Предоставляет методы расширения дл импорта ОНИ из xml-выписки.
    /// </summary>
    public static class ObjectExtentions
    {
        public static void SetRecordInfo(
          this ObjectRecord oni
        , SibRosReestr.EGRN.Unknown.RecordInfo rr
        )
        {
            if (rr == null) return;
            oni.RegistrationDate = ImportHelper.GetDate(rr.Registration_date);
            oni.CancelDate = ImportHelper.GetDate(rr.Cancel_date);
        }

        public static void SetObjectCommonData(
             this ObjectRecord br
            , SibRosReestr.EGRN.Unknown.ObjectCommonData obj
        )
        {
            if (obj == null || obj.Common_data == null) return;

            br.CadastralNumber = obj.Common_data.Cad_number;
            br.Quarter_cad_number = obj.Common_data.Quarter_cad_number;
            br.TypeCode = obj.Common_data.Type?.Code;
            br.TypeValue = obj.Common_data.Type?.Value;
            br.TypeStr = $"{br.TypeCode} {br.TypeValue}";


        }


        public static void SetBuildParams(
            this ObjectRecord br
          , SibRosReestr.EGRN.Unknown.ParamsBuildWithoutMaterials obj
          , IUnitOfWork uow
        )
        {
            if (obj == null) return;
            br.Area = obj.Area;
            br.AreaText = obj.Area.ToString();
            br.Floors = obj.Floors;
            br.Underground_floors = obj.Underground_floors;
            br.PurposeCode = obj.Purpose?.Code;
            br.PurposeName = obj.Purpose?.Value;
            br.PurposeStr = $"{br.PurposeCode} {br.PurposeName}";
            br.Name = obj.Name;
            br.Year_built = obj.Year_built;
            br.Year_commisioning = obj.Year_commisioning;


            if (obj.Permitted_uses != null)
            {
                foreach (var item in obj.Permitted_uses)
                {
                    RosReestr.Entities.PermittedUse pm =
                        uow.GetRepository<RosReestr.Entities.PermittedUse>().Create(new RosReestr.Entities.PermittedUse());
                    pm.Name = item.Name;
                    pm.ObjectRecord = br;
                    pm.Extract = br.Extract;
                    br.Permitted_usesStr += (!String.IsNullOrEmpty(br.Permitted_usesStr)) ? $"; {item.Name}" : item.Name;

                }
            }

        }

        public static void SetAddress(
           this ObjectRecord br
         , SibRosReestr.EGRN.Unknown.AddressLocationBuild obj
       )
        {
            if (obj == null) return;
            br.Address_typeCode = obj.Address_type?.Code;
            br.Address_typeName = obj.Address_type?.Value;
            br.Address_typeStr = $"{br.Address_typeCode} {br.Address_typeName}";
            br.SetAddresMain(obj.Address);
            if (obj.Location != null)
            {
                br.LocationOksOkato = obj.Location.Okato;
                br.LocationOksOktmo = obj.Location.Oktmo;
                br.LocationOksRegionCode = obj.Location.Region?.Code;
                br.LocationOksRegionName = obj.Location.Region?.Value;
                br.LocationOksRegionStr = $"{br.LocationOksRegionCode} {br.LocationOksRegionName}";
                br.LocationOksPosition_description = obj.Location.Position_description;
            }

        }

        public static void SetAddress(
          this ObjectRecord br
        , SibRosReestr.EGRN.Unknown.AddressLocationLand obj
      )
        {
            if (obj == null) return;
            br.Address_typeCode = obj.Address_type?.Code;
            br.Address_typeName = obj.Address_type?.Value;
            br.Address_typeStr = $"{br.Address_typeCode} {br.Address_typeName}";

            br.SetAddresMain(obj.Address);
            if (obj.Rel_position != null)
            {
                br.In_boundaries_mark = obj.Rel_position.In_boundaries_mark;
            }

        }

        private static void SetAddresMain(
             this ObjectRecord br
           , SibRosReestr.EGRN.Unknown.AddressMain Address
            )
        {
            if (Address == null) return;
            if (Address.Address_fias != null)
            {
                if (Address.Address_fias.Level_settlement != null)
                {
                    br.AddressFias = Address.Address_fias.Level_settlement.Fias;
                    br.AddressOkato = Address.Address_fias.Level_settlement.Okato;
                    br.AddressKladr = Address.Address_fias.Level_settlement.Kladr;
                    br.AddressOktmo = Address.Address_fias.Level_settlement.Oktmo;
                    br.AddressPostal_code = Address.Address_fias.Level_settlement.Postal_code;
                    br.RegionCode = Address.Address_fias.Level_settlement.Region?.Code;
                    br.RegionName = Address.Address_fias.Level_settlement.Region?.Value;
                    br.RegionStr = $"{br.RegionCode} {br.RegionName}";
                    br.AddressDistrictType = Address.Address_fias.Level_settlement.District?.Type_district;
                    br.AddressDistrictName = Address.Address_fias.Level_settlement.District?.Name_district;
                    br.AddressDistrictStr = $"{br.AddressDistrictType} {br.AddressDistrictName}";
                    br.AddressCityType = Address.Address_fias.Level_settlement.City?.Type_city;
                    br.AddressCityName = Address.Address_fias.Level_settlement.City?.Name_city;
                    br.AddressCityStr = $"{br.AddressCityType} {br.AddressCityName}";
                    br.AddressUrban_districtType = Address.Address_fias.Level_settlement.Urban_district?.Type_urban_district;
                    br.AddressUrban_districtName = Address.Address_fias.Level_settlement.Urban_district?.Name_urban_district;
                    br.AddressSoviet_villageType = Address.Address_fias.Level_settlement.Soviet_village?.Type_soviet_village;
                    br.AddressSoviet_villageName = Address.Address_fias.Level_settlement.Soviet_village?.Name__soviet_village;
                    br.AddressSoviet_villageStr = $"{br.AddressSoviet_villageType} {br.AddressSoviet_villageName}";
                    br.AddressLocalityType = Address.Address_fias.Level_settlement.Locality?.Type_locality;
                    br.AddressLocalityName = Address.Address_fias.Level_settlement.Locality?.Name_locality;
                    br.AddressLocalityStr = $"{br.AddressLocalityType} {br.AddressLocalityName}";

                }
                if (Address.Address_fias.Detailed_level != null)
                {
                    br.AddressStreetType = Address.Address_fias.Detailed_level.Street?.Type_street;
                    br.AddressStreetName = Address.Address_fias.Detailed_level.Street?.Name_street;
                    br.AddressStreetStr = $"{br.AddressStreetType} {br.AddressStreetName}";
                    br.AddressLevel1Type = Address.Address_fias.Detailed_level.Level1?.Type_level1;
                    br.AddressLevel1Name = Address.Address_fias.Detailed_level.Level1?.Name_level1;
                    br.AddressLevel1Str = $"{br.AddressLevel1Type} {br.AddressLevel1Name}";
                    br.AddressLevel2Type = Address.Address_fias.Detailed_level.Level2?.Type_level2;
                    br.AddressLevel2Name = Address.Address_fias.Detailed_level.Level2?.Name_level2;
                    br.AddressLevel2Str = $"{br.AddressLevel2Type} {br.AddressLevel2Name}";
                    br.AddressLevel3Type = Address.Address_fias.Detailed_level.Level3?.Type_level3;
                    br.AddressLevel3Name = Address.Address_fias.Detailed_level.Level3?.Name_level3;
                    br.AddressLevel3Str = $"{br.AddressLevel3Type} {br.AddressLevel3Name}";
                    br.AddressApartmentType = Address.Address_fias.Detailed_level.Apartment?.Type_apartment;
                    br.AddressApartmentName = Address.Address_fias.Detailed_level.Apartment?.Name_apartment;
                    br.AddressApartmentStr = $"{br.AddressApartmentType} {br.AddressApartmentName}";
                    br.AddressOther = Address.Address_fias.Detailed_level.Other;
                }

            }
            br.Address = Address.Note;
            br.Note = Address.Note;
            br.Readable_address = Address.Readable_address;
        }

        /// <summary>
        /// Устанавливает состав сложной вещи/части ОНИ.
        /// </summary>
        /// <param name="br"></param>
        /// <param name="objs"></param>
        /// <param name="uow"></param>
        public static void SetParts(
               this ObjectRecord br
             , IUnitOfWork uow
             , List<SibRosReestr.EGRN.Unknown.ObjectPartNumberRestrictions> objs

      )
        {
            if (objs == null) return;
            foreach (var item in objs)
            {

                CorpProp.RosReestr.Entities.ObjectPartNumberRestrictions part =
                    uow.GetRepository<CorpProp.RosReestr.Entities.ObjectPartNumberRestrictions>()
                    .Create(new CorpProp.RosReestr.Entities.ObjectPartNumberRestrictions());

                part.Part_number = item.Part_number;
                part.ObjectRecord = br;
                part.Extract = br.Extract;
                if (item.Content_restrictions != null && item.Content_restrictions.Item != null)
                {
                    if (item.Content_restrictions.Item is SibRosReestr.EGRN.Unknown.Registration)
                    {
                        SibRosReestr.EGRN.Unknown.Registration reg = item.Content_restrictions.Item as SibRosReestr.EGRN.Unknown.Registration;
                        part.Number = reg.Restriction_encumbrance_number?.Number;
                        part.Right_number = reg.Restriction_encumbrance_number?.Right_number;
                        part.Registration_date = ImportHelper.GetDate(reg.Registration_date);
                    }
                    if (item.Content_restrictions.Item is SibRosReestr.EGRN.Unknown.RegNumberBorder)
                    {
                        SibRosReestr.EGRN.Unknown.RegNumberBorder bb = item.Content_restrictions.Item as SibRosReestr.EGRN.Unknown.RegNumberBorder;
                        part.Reg_number = bb.Reg_number;
                    }
                }


            }

        }

        /// <summary>
        /// Устанавливает кол-ию контура ОНИ.
        /// </summary>
        /// <param name="br"></param>
        /// <param name="Contours"></param>
        /// <param name="uow"></param>
        public static void SetContours(
              this ObjectRecord br
            , IUnitOfWork uow
            , List<SibRosReestr.EGRN.Unknown.ContourOKSOut> Contours
        )
        {
            if (Contours == null) return;
            foreach (var item in Contours)
            {
                CorpProp.RosReestr.Entities.ContourOKSOut cont =
                    uow.GetRepository<CorpProp.RosReestr.Entities.ContourOKSOut>().Create(
                        new CorpProp.RosReestr.Entities.ContourOKSOut());

                cont.Number_pp = item.Number_pp;
                cont.Sk_id = item.Entity_spatial?.Sk_id;
                if (item.Entity_spatial != null && item.Entity_spatial.Spatials_elements != null)
                {
                    for (int i = 0; i < item.Entity_spatial.Spatials_elements.Count; i++)
                    {
                        SibRosReestr.EGRN.Unknown.SpatialElementOKSOut sp = item.Entity_spatial.Spatials_elements[i];
                        if (i == 0)
                        {
                            cont.Level_contourCode = sp.Level_contour?.Code;
                            cont.Level_contourName = sp.Level_contour?.Value;
                            if (sp.Ordinates != null)
                                cont.SetOrdinates(uow, sp.Ordinates);
                        }
                        else
                        {
                            CorpProp.RosReestr.Entities.ContourOKSOut ks =
                                uow.GetRepository<CorpProp.RosReestr.Entities.ContourOKSOut>().Create(
                                    new CorpProp.RosReestr.Entities.ContourOKSOut());
                            ks.Level_contourCode = sp.Level_contour?.Code;
                            ks.Level_contourName = sp.Level_contour?.Value;
                            if (sp.Ordinates != null)
                                ks.SetOrdinates(uow, sp.Ordinates);

                        }
                    }
                }

            }
        }

        public static void SetContours(
            this ObjectRecord br
          , IUnitOfWork uow
          , List<SibRosReestr.EGRN.Unknown.ContourZUOut> Contours
      )
        {
            if (Contours == null) return;
            foreach (var item in Contours)
            {
                Entities.ContourOKSOut cont =
                    uow.GetRepository<Entities.ContourOKSOut>()
                    .Create(new Entities.ContourOKSOut());
                cont.Extract = br.Extract;
                cont.ObjectRecord = br;
                cont.Number_pp = item.Number_pp;
                cont.Sk_id = item.Entity_spatial?.Sk_id;
                cont.Cad_number = item.Cad_number;
                if (item.Entity_spatial != null && item.Entity_spatial.Spatials_elements != null)
                {
                    for (int i = 0; i < item.Entity_spatial.Spatials_elements.Count; i++)
                    {
                        SibRosReestr.EGRN.Unknown.SpatialsElementZUOut sp = item.Entity_spatial.Spatials_elements[i];
                        if (i == 0)
                        {
                            if (sp.Ordinates != null)
                                cont.SetOrdinates(uow, sp.Ordinates);
                        }
                        else
                        {
                            Entities.ContourOKSOut ks =
                                uow.GetRepository<Entities.ContourOKSOut>()
                                .Create(new Entities.ContourOKSOut());
                            ks.Number_pp = cont.Number_pp;
                            ks.Sk_id = cont.Sk_id;
                            ks.Cad_number = cont.Cad_number;
                            ks.Extract = cont.Extract;
                            ks.ObjectRecord = cont.ObjectRecord;
                            if (sp.Ordinates != null)
                                ks.SetOrdinates(uow, sp.Ordinates);
                        }
                    }
                }

            }
        }

        public static void SetOwnerless(
              this ObjectRecord br
            , SibRosReestr.EGRN.Unknown.OwnerlessRightRecordOut ownerless
            )
        {
            if (ownerless == null) return;
            br.OwnerlessRightRecordRegDate = ImportHelper.GetDate(ownerless.Record_info?.Registration_date);
            br.Ownerless_right_number = ownerless.Ownerless_right_data?.Ownerless_right_number;
            br.Authority_name = ownerless.Ownerless_right_data?.Authority_name;
        }


        public static void SetCadLinks(
             this ObjectRecord br
           , IUnitOfWork uofw
           , SibRosReestr.EGRN.Unknown.CadLinks obj
           )
        {
            if (obj == null) return;
            br.SetLandCadNumbers(uofw, obj.Land_cad_numbers);
            br.SetRoomCadNumbers(uofw, obj.Room_cad_numbers);
            br.SetCarParkingSpaceCadNumbers(uofw, obj.Car_parking_space_cad_numbers);
            br.SetOldNumbers(uofw, obj.Old_numbers);
        }


        private static void SetLandCadLinks(
             this ObjectRecord br
           , IUnitOfWork uofw
           , SibRosReestr.EGRN.Unknown.CadLinksLandIncludedOld obj
       )
        {
            if (obj == null) return;
            br.SetRoomCadNumbers(uofw, obj.Included_objects);
            br.SetOldNumbers(uofw, obj.Old_numbers);
        }


        private static void SetLandCadNumbers(
              this ObjectRecord br
            , IUnitOfWork uofw
            , List<SibRosReestr.EGRN.Unknown.CadNumber> objs
        )
        {
            if (objs == null) return;
            foreach (var item in objs)
            {
                Entities.CadNumber cad =
                    uofw.GetRepository<Entities.CadNumber>().Create(
                    new Entities.CadNumber());
                cad.Cad_number = item.Cad_number;
                cad.ExtractLand = br.Extract;
                cad.ObjectRecordLand = br;
                br.Land_cad_numbersStr += cad.Cad_number +";";
            }
        }

        public static void SetRoomCadNumbers(
              this ObjectRecord br
            , IUnitOfWork uofw
            , List<SibRosReestr.EGRN.Unknown.CadNumber> objs
       )
        {
            if (objs == null) return;
            foreach (var item in objs)
            {
                RosReestr.Entities.CadNumber cad =
                    uofw.GetRepository<Entities.CadNumber>().Create(
                        new RosReestr.Entities.CadNumber());
                cad.Cad_number = item.Cad_number;
                cad.ExtractRoom = br.Extract;
                cad.ObjectRecordRoom = br;
                br.Room_cad_numbersStr += cad.Cad_number + ";";
            }
        }

        private static void SetCarParkingSpaceCadNumbers(
              this ObjectRecord br
            , IUnitOfWork uofw
            , List<SibRosReestr.EGRN.Unknown.CadNumber> objs
      )
        {
            if (objs == null) return;
            foreach (var item in objs)
            {
                RosReestr.Entities.CadNumber cad =
                   uofw.GetRepository<Entities.CadNumber>().Create(
                       new RosReestr.Entities.CadNumber());
                cad.Cad_number = item.Cad_number;
                cad.ExtractCarParking = br.Extract;
                cad.ObjectRecordCarParking = br;
            }
        }

        public static void SetOldNumbers(
              this ObjectRecord br
            , IUnitOfWork uofw
            , List<SibRosReestr.EGRN.Unknown.OldNumber> objs
        )
        {
            if (objs == null) return;
            foreach (var item in objs)
            {
                Entities.OldNumber cad =
                    uofw.GetRepository<Entities.OldNumber>()
                    .Create(new Entities.OldNumber());
                cad.Number_typeCode = item.Number_type?.Code;
                cad.Number_typeName = item.Number_type?.Value;
                cad.Number = item.Number;
                cad.Assignment_date = ImportHelper.GetDate(item.Assignment_date);
                cad.Assigner = item.Assigner;
                cad.Extract = br.Extract;
                cad.ObjectRecord = br;
            }
        }

        public static void SetRoomRecords(
              this ObjectRecord br
            , IUnitOfWork uofw
           , List<SibRosReestr.EGRN.Unknown.RoomLocationInBuildPlans> objs
           )
        {
            if (objs == null) return;
            foreach (var item in objs)
            {
                Entities.RoomLocationInBuildPlans cad =
                    uofw.GetRepository<Entities.RoomLocationInBuildPlans>()
                    .Create(new Entities.RoomLocationInBuildPlans());
                cad.ObjectCadNumber = item.Object?.Common_data?.Cad_number;
                cad.Extract = br.Extract;
                cad.ObjectRecord = br;
                cad.SetLocationInBuild(uofw, item.Location_in_build);
            }
        }



        public static void SetParkingRecords(
             this ObjectRecord br
            , IUnitOfWork uow
            , List<SibRosReestr.EGRN.Unknown.CarParkingSpaceLocationInBuildPlans> objs
            )
        {
            if (objs == null) return;
            foreach (var item in objs)
            {
                Entities.CarParkingSpaceLocationInBuildPlans cad =
                    uow.GetRepository<Entities.CarParkingSpaceLocationInBuildPlans>()
                    .Create(new Entities.CarParkingSpaceLocationInBuildPlans());
                cad.ObjectCadNumber = item.Object?.Common_data?.Cad_number;
                cad.Floor = item.Location_in_build?.Floor;
                cad.Floor_typeCode = item.Location_in_build?.Floor_type?.Code;
                cad.Floor_typeName = item.Location_in_build?.Floor_type?.Value;
                cad.Plan_number = item.Location_in_build?.Plan_number;
                cad.Description = item.Location_in_build?.Description;
                cad.Extract = br.Extract;
                cad.ObjectRecord = br;
                cad.SetParkingPlans(uow, item.Location_in_build?.Plans);
            }
        }


        public static void CreateRights(
           this ObjectRecord br
          , List<SibRosReestr.EGRN.Unknown.RightRecordsBaseParamsRight_record> objs
          , ref ImportHolder holder
          )
        {
            if (objs == null) return;
            for (int i = 0; i < objs.Count; i++)
            {
                SibRosReestr.EGRN.Unknown.RightRecordsBaseParamsRight_record item = objs[i];
                RightRecord rr =
                    holder.UnitOfWork.GetRepository<RightRecord>()
                    .Create(new RightRecord());

                rr.Import(item, ref holder);
                rr.Extract = br.Extract;
                rr.ObjectRecord = br;
                if (rr.ObjectRecord != null && rr.PersonHolder)
                    rr.ObjectRecord.PersonHolder = true;
                rr.Extract.CountRights++;
            }
        }



        public static void CreateRestrictRecords(
              this ObjectRecord br
            , List<SibRosReestr.EGRN.Unknown.RestrictRecordType> objs
            , ref ImportHolder holder
            )
        {
            if (objs == null) return;
            for (int i = 0; i < objs.Count; i++)
            {
                SibRosReestr.EGRN.Unknown.RestrictRecordType item = objs[i];
                RestrictRecord rr =
                    holder.UnitOfWork.GetRepository<RestrictRecord>()
                    .Create(new RestrictRecord());
                rr.Extract = br.Extract;
                rr.ObjectRecord = br;                
                rr.Import(item, ref holder);
            }
        }



        public static void CreateRestrictRecords(
            this ObjectRecord br
          , List<SibRosReestr.EGRN.Unknown.RestrictRecordLandType> objs
          , ref ImportHolder holder
        )
        {
            if (objs == null) return;
            for (int i = 0; i < objs.Count; i++)
            {
                SibRosReestr.EGRN.Unknown.RestrictRecordLandType item = objs[i];
                RestrictRecord rr =
                    holder.UnitOfWork.GetRepository<RestrictRecord>()
                    .Create(new RestrictRecord());
                rr.Extract = br.Extract;
                rr.ObjectRecord = br;
                rr.Import(item, ref holder);



            }
        }

    }
}
