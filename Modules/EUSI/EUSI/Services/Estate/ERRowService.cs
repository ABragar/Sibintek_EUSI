using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Base.Attributes;
using Base.DAL;
using Base.Events;
using Base.Service;
using Base.Utils.Common;
using CorpPropEstate = CorpProp.Entities.Estate.Estate;
using CorpProp.Entities.Base;
using CorpProp.Entities.Estate;
using CorpProp.Entities.Import;
using CorpProp.Entities.NSI;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.Services.Accounting;
using EUSI.Entities.Estate;
using EUSI.Entities.ManyToMany;
using CorpProp.Extentions;
using AppContext = Base.Ambient.AppContext;
using EUSI.Services.Accounting;
using Base.UI.Service;
using ExcelDataReader;
using EUSI.Import;
using Base.BusinessProcesses.Services.Abstract;
using Base.Settings;
using Base.Mail.Entities;
using CorpProp.Services.Base;
using EUSI.Services.Estate.FiasHandlers;
using Base.Service.Log;

namespace EUSI.Services.Estate
{
    public interface IERRowService : ITypeObjectService<EstateRegistrationRow>
    {

    }


    public class ERRowService : TypeObjectService<EstateRegistrationRow>, IERRowService
    {
        private readonly ILogService _logger;
        public ERRowService(IBaseObjectServiceFacade facade, ILogService logger) : base(facade, logger)
        {
            _logger = logger;
            
        }

        public override EstateRegistrationRow CreateDefault(IUnitOfWork unitOfWork)
        {
            var obj = base.CreateDefault(unitOfWork);
            obj.SibCountry = unitOfWork.GetRepository<CorpProp.Entities.FIAS.SibCountry>()
                .Filter(f => !f.Hidden && f.Code == "RU").FirstOrDefault();
            return obj;
        }

        protected override void OnSave(EstateRegistrationRow obj)
        {
            using (var uofw = UnitOfWorkFactory.Create())
            {
                var cityHandler = new CityHandler(uofw);
                var regionHandler = new RegionHandler(uofw);
                var federalDistrictHandler = new FederalDistrictHandler(uofw);

                cityHandler.SetNextChecker(regionHandler);
                regionHandler.SetNextChecker(federalDistrictHandler);

                cityHandler.Handle(obj);
                uofw.SaveChanges();
            }

            base.OnSave(obj);
        }

    }
}
