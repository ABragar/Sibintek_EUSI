using Base.DAL;
using CorpProp.Entities.Estate;
using CorpProp.Entities.Law;
using CorpProp.Entities.ManyToMany;
using CorpProp.Entities.Mapping;
using CorpProp.Entities.NSI;
using CorpProp.RosReestr.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Migration
{
    public static class MigrateEstate
    {

        public static void StartMigrateEstate(this Estate est, IUnitOfWork uow, ObjectRecord obj, MigrateHolder holder)
        {
            try
            {
                if (est is BuildingStructure)
                {
                    ((BuildingStructure)est).Migrate(uow, obj);
                    return;
                }
                if (est is Land)
                {
                    ((Land)est).Migrate(uow, obj);
                    return;
                }
                if (est is Cadastral)
                {
                    ((Cadastral)est).MigrateCadastral(uow, obj);
                    return;
                }
            }
            catch (Exception ex)
            {
                holder.AddError(est.GetType().Name, obj.CadastralNumber, ex.Message);
            }
        }


        public static void CreateCadastralAndExtract(this Cadastral est, IUnitOfWork uow, Extract obj)
        {
            try
            {
                if (obj == null) return;
                var cad = est.CadastralNumber;
                var numb = obj.ExtractNumber;
                var ext = uow.GetRepository<CadastralAndExtract>()
                    .Filter(f => !f.Hidden && f.ObjLeft != null && f.ObjLeft.CadastralNumber == cad
                    && f.ObjRigth != null && f.ObjRigth.ExtractNumber == numb).Any();

                if (ext) return;
                var link = uow.GetRepository<CadastralAndExtract>().Create(new CadastralAndExtract());
                link.ObjLeft = est;
                link.ObjRigth = obj;
            }
            catch 
            {

            }
        }

        public static void MigrateCadastral(this Cadastral est, IUnitOfWork uow, ObjectRecord obj)
        {
            est.RegDate = obj.Reg_date_by_doc;
            est.DeRegDate = obj.CancelDate;
            est.CadastralNumber = obj.CadastralNumber;
            est.CadastralNumbers = obj.CadastralNumber;
            est.BlocksNumber = obj.Quarter_cad_number;           
            est.OtherCadastralNumber = obj.Room_cad_numbersStr;
            est.OldRegNumbers = obj.Old_numbersStr;
            est.Area = (obj.Area == null) ? 0 : obj.Area.Value;
            est.AreaText = obj.AreaText;
            est.Confiscation = $"{obj.Date_removed_cad_account?.Name} {obj.Date_removed_cad_account?.Number}";            
            est.Appointments = obj.PurposeName;           
            
            est.NameByRight = obj.Name;

            if (String.IsNullOrEmpty(est.Name))
                est.Name = obj.Name;
            if (String.IsNullOrEmpty(obj.Name))
                est.Name = $"{est.CadastralNumber}";

            int fl = 0;
            int.TryParse(obj.Floors, out fl);
            est.FloorsCount = fl;
            est.UsesKind = obj.Permitted_usesStr;
            est.Address = obj.Address;
            est.RegionCode = obj.RegionCode;
            est.RegionName = obj.RegionName;
            est.RegionStr = obj.RegionStr;
            est.District = obj.AddressDistrictStr;
            est.City = obj.AddressCityStr;
            est.Locality = obj.AddressLocalityStr;
            est.Street = obj.AddressStreetStr;
            est.House = obj.AddressLevel1Str;
            est.House += (!string.IsNullOrEmpty(obj.AddressLevel2Str)) ? (" " + obj.AddressLevel2Str) : "";
            est.House += (!string.IsNullOrEmpty(obj.AddressLevel3Str)) ? " " + obj.AddressLevel3Str : "";
            est.House += (!string.IsNullOrEmpty(obj.AddressApartmentStr)) ? " " + obj.AddressLevel3Str : "";
            string kk = (!string.IsNullOrEmpty(obj.AddressOkato) && obj.AddressOkato.Length > 1) ? obj.AddressOkato.Substring(0, 2) : "";
            //est.OKATO = CorpProp.Helpers.ImportHelper.FindDictObject<OKATO>(uow, kk);
            string ks = (!string.IsNullOrEmpty(obj.AddressOktmo) && obj.AddressOktmo.Length > 1) ? obj.AddressOktmo.Substring(0, 2) : "";
            est.OKTMO = CorpProp.Helpers.ImportHelper.FindDictObject<OKTMO>(uow, ks);
            //est.KLADRRegionCode = (!string.IsNullOrEmpty(obj.AddressKladr) && obj.AddressKladr.Length > 1) ? obj.AddressKladr.Substring(0, 2) : "";
            est.CadastralValue = obj.Cost;

            est.CreateCadastralValue(uow, obj.Cost, obj.Extract?.ExtractDate);
            est.CreateCoordinates(uow, obj);
            est.CreateParts(uow, obj);
            est.CreateCadastralAndExtract(uow, obj.Extract);

            //формирование ИК
            est.CreateIK(uow, obj);
        }

        private static void CreateIK(this Cadastral est, IUnitOfWork uow, ObjectRecord obj)
        {
            if (!String.IsNullOrEmpty(obj.Land_cad_numbersStr) && est.ParentID == null)
            {
                var arr = obj.Land_cad_numbersStr.Split(';');

                //совпадение по кад номеру связанного ЗУ
                var ik = uow.GetRepository<PropertyComplexIO>()
                    .Filter(f => !f.Hidden
                        && !f.IsHistory
                        && f.Land != null
                        && f.Land.CadastralNumber == obj.Land_cad_numbersStr
                        )
                    .FirstOrDefault();

                //совпадение по номеру ЗУ
                if (ik == null)
                    ik = uow.GetRepository<PropertyComplexIO>()
                       .Filter(f => !f.Hidden
                           && !f.IsHistory
                           && f.Land != null
                           && !String.IsNullOrEmpty(f.LandCadNumber)
                           && f.LandCadNumber == obj.Land_cad_numbersStr
                           )
                       .FirstOrDefault();

                //совпадение по вхождению номера ЗУ
                if (ik == null)
                    ik = uow.GetRepository<PropertyComplexIO>()
                       .Filter(f => !f.Hidden
                           && !f.IsHistory
                           && f.Land != null
                           && !String.IsNullOrEmpty(f.LandCadNumber)
                           && arr.Contains(f.LandCadNumber)
                           )
                       .FirstOrDefault();

                if (ik == null)
                {
                    ik = uow.GetRepository<PropertyComplexIO>()
                        .Create(new PropertyComplexIO()
                        {
                            Name = $"Земельный участок {obj.Land_cad_numbersStr}",
                            NameEUSI = $"Земельный участок {obj.Land_cad_numbersStr}",
                            LandCadNumber = obj.Land_cad_numbersStr,
                            IsPropertyComplex = true,
                        });

                    ik.Land = uow.GetRepository<Land>()
                    .Filter(f => !f.Hidden
                        && !f.IsHistory
                        && !String.IsNullOrEmpty(f.CadastralNumber)
                        && (f.CadastralNumber == obj.Land_cad_numbersStr || arr.Contains(f.CadastralNumber))
                        )
                    .FirstOrDefault();
                }

                est.Parent = ik;
            }
        }

        public static void Migrate(this Land est, IUnitOfWork uow, ObjectRecord obj)
        {
            est.MigrateCadastral(uow, obj);
            est.LandType = CorpProp.RosReestr.Helpers.ImportHelper.FindOrCreateDictByName<LandType>(uow, obj.SubtypeName);
            est.GroundCategory = CorpProp.RosReestr.Helpers.ImportHelper.FindOrCreateDictByName<GroundCategory>(uow, obj.Category);
            est.PermittedByDoc = obj.PermittedBy_document;
            est.PermittedLandUse = obj.PermittedLand_use;
            est.PermittedLandUseMer = obj.PermittedLand_use_mer;
            est.PermittesGradRegNumbBorder = obj.Permittes_Grad_Reg_numb_border;
            est.PermittesGradLandUse = obj.Permittes_Grad_Land_use;
            est.RegDateByDoc = obj.Reg_date_by_doc;
            if (String.IsNullOrEmpty(obj.Name))
                est.Name = $"Земельный участок {est.CadastralNumber}";
        }

        public static void Migrate(this BuildingStructure est, IUnitOfWork uow, ObjectRecord obj)
        {
            est.MigrateCadastral(uow, obj);          
            est.BuildingKind = obj.TypeValue;            
            est.RoomsCadastralNumber = obj.Room_cad_numbersStr;
            est.CarPlaceCadastralNumber = obj.Car_parking_space_cad_numbersStr;   
            est.YearComplete = obj.Year_built;
            est.YearStart = obj.Year_commisioning;

            decimal dec = 0;
            Decimal.TryParse(obj.Underground_floors, out dec);
            est.UndergroundFloorCount = dec;

        }


        public static Estate FindOrCreateEstate(
         IUnitOfWork uofw
         , ObjectRecord obj
         , Right rr
         , MigrateHolder holder
         )
        {
            Estate est = null;

            //ищем ОИ по кадастровому номеру
            est = FindEstate(uofw, obj, holder);
            if (est != null)  
                return est;

            holder.AddLog(nameof(Cadastral), obj.CadastralNumber, "101");

            if (obj.Extract != null && obj.Extract is ExtractBuild)
                return uofw.GetRepository<BuildingStructure>().Create(new BuildingStructure());

            if (obj.Extract != null && obj.Extract is ExtractLand)
                return uofw.GetRepository<Land>().Create(new Land());
           
            //нету, создаем
            //по мэппингу Вид ОНИ - Тип ОИ
            if (!String.IsNullOrEmpty(obj.TypeCode))
            {
                est = CreateEstateByTypeCode(uofw, obj.TypeCode);
                if (est != null)
                    return est;
            }

            //Ничего не создали, создаем кадастровый номер
            est = uofw.GetRepository<Cadastral>().Create(new Cadastral());
            return est;
        }

        public static Cadastral CreateCadastral(
             IUnitOfWork uofw
         , ObjectRecord obj    
            , MigrateHolder holder
            )
        {
            holder.AddLog(nameof(Cadastral), obj.CadastralNumber, "101");
            if (obj.Extract != null && obj.Extract is ExtractBuild)
                return uofw.GetRepository<BuildingStructure>().Create(new BuildingStructure());

            if (obj.Extract != null && obj.Extract is ExtractLand)
                return uofw.GetRepository<Land>().Create(new Land());

            //по мэппингу Вид ОНИ - Тип ОИ
            if (!String.IsNullOrEmpty(obj.TypeCode))
            {
                var est = CreateEstateByTypeCode(uofw, obj.TypeCode);
                if (est != null)
                    return (Cadastral)est ;
            }

            //Ничего не создали, создаем кадастровый номер
            return uofw.GetRepository<Cadastral>().Create(new Cadastral());
           
        }

        public static Estate FindEstate(IUnitOfWork uow, ObjectRecord obj, MigrateHolder holder)
        {
            if (obj == null) return null;
            if (String.IsNullOrEmpty(obj.CadastralNumber)) return null;

            var cadNumber = obj.CadastralNumber;

            var items = uow.GetRepository<Cadastral>()
                    .Filter(f => !f.Hidden && f.CadastralNumber == cadNumber);
            var count = items.Count();
            if (count == 1)
            {
                holder.AddLog(nameof(Cadastral), obj.CadastralNumber, "102");
                return items.FirstOrDefault();
            }               
            if (count > 1)
            {
                return CreateFake(uow, obj, items, holder);
            }
            //TODO: поиск ОИ по номеру гос. регистрации.
            return null;
        }

       

        public static Cadastral CreateFake(IUnitOfWork uow
            , ObjectRecord obj
            , IExtendedQueryable<Cadastral> items
            , MigrateHolder holder)
        {
            if (items == null) return null;

            Cadastral cad = items.Where(f => f.IsFake).FirstOrDefault();
            if (cad == null)
                cad = CreateCadastral(uow, obj, holder);
            cad.StartMigrateEstate(uow, obj, holder);
            cad.IsFake = true;

            var objs = items.Where(f => !f.IsFake).ToList();            
            if (objs != null)
                foreach (var item in objs)
                {
                    item.Fake = cad;
                    item.StartMigrateEstate(uow, obj, holder);
                    uow.GetRepository<Cadastral>().Update(item);
                }

            return cad;
        }

        public static Estate CreateEstateByTypeCode(
           IUnitOfWork uow
           , string code
           )
        {
            if (String.IsNullOrEmpty(code)) return null;
            var tps = uow.GetRepository<RosReestrTypeEstate>()
                .Filter(x => x.Hidden == false
                && x.ObjectTypeCode == code).FirstOrDefault();
            if (tps != null)
            {
                var obr = CorpProp.Helpers.ImportHelper.CreateEstateByMnemonic(uow, tps.EstateMnemonic);
                if (obr != null)
                    return (Estate)obr;
            }
            return null;
        }

        private static void CreateCadastralValue(this Cadastral est, IUnitOfWork uow, decimal? cost, DateTime? date)
        {
            if (cost == null) return;
            CadastralValue val = null;
            val = uow.GetRepository<CadastralValue>()
                .Filter(f => !f.Hidden && f.Value == cost && f.StartDate == date)
                .FirstOrDefault();

            if (val != null)
                uow.GetRepository<CadastralValue>().Update(val);            
            else
                val = uow.GetRepository<CadastralValue>().Create(new CadastralValue());

            val.Value = cost;
            val.StartDate = date;
            val.InformationSource = uow.GetRepository<InformationSource>().Filter(x => x.Code == "Extract").FirstOrDefault();
        }

        private static void CreateCoordinates(this Cadastral est, IUnitOfWork uow, ObjectRecord obj)
        {
            if (obj == null) return;

            var objId = obj.ID;
            
            var list = uow.GetRepository<ContourOKSOut>()
                .Filter(f => !f.Hidden && f.ObjectRecordID == objId).ToList();
                

            foreach (var item in list)
            {
                string cad = obj.CadastralNumber;
                var cor = uow.GetRepository<Coordinate>()
                    .Filter(x => !x.Hidden && !x.IsHistory && x.Cadastral != null && x.Cadastral.CadastralNumber == cad
                && x.CadNumber == item.Cad_number && x.ContourNumber == item.Number_pp).FirstOrDefault();

                if (cor != null)
                    continue;
                Coordinate pp = uow.GetRepository<Coordinate>().Create(new Coordinate());
                pp.CadNumber = item.Cad_number;
                pp.ContourNumber = item.Number_pp;
                pp.Cadastral = est;
                pp.ContourType = CorpProp.RosReestr.Helpers.ImportHelper.FindOrCreateDictByName<ContourType>(uow, item.Level_contourName);
                pp.X = item.X;
                pp.Y = item.Y;
                pp.Z = item.Z;
                pp.Datum = item.Sk_id;
                pp.OrdNumber = item.Ord_nmb;
                pp.PointNumber = item.Num_geopoint;
                pp.Delta = item.Delta_geopoint;
                pp.R = item.R;
               
            }
        }


        public static void CreateParts(
         this Cadastral est,
         IUnitOfWork uow
        , ObjectRecord obj       
        )
        {
            if (obj == null) return;
            var objId = obj.ID;

            var list = uow.GetRepository<ObjectPartNumberRestrictions>()
                .Filter(f => !f.Hidden && f.ObjectRecordID == objId).ToList();

            foreach (var item in list)
            {
                string cad = obj.CadastralNumber;
                var cor = uow.GetRepository<CadastralPart>()
                    .Filter(x => !x.Hidden && !x.IsHistory && x.Cadastral != null && x.Cadastral.CadastralNumber == cad
                && x.Number == item.Number).FirstOrDefault();

                if (cor != null)
                    continue;
                CadastralPart pp = new CadastralPart();
                pp.Number = item.Number;
                pp.Cadastral = est;
                pp.NumberPart = item.Part_number;
                pp.NumberEncumbrance = item.Right_number;
                pp.RegDate = item.Registration_date;
                pp.NumberBorder = item.Reg_number;
                uow.GetRepository<CadastralPart>().Create(pp);
            }

        }
    }
}
