using System;
using System.Collections.Generic;
using System.Linq;
using Base.BusinessProcesses.Entities;
using Base.Contact.Service.Abstract;
using Base.CRM.Entities;
using Base.CRM.Services.Abstract;
using Base.CRM.UI.Presets;
using Base.DAL;
using Base.Document.Services.Concrete;
using Base.Enums;
using Base.Security;
using Base.Service;
using AppContext = Base.Ambient.AppContext;

namespace Base.CRM.Services.Concrete
{
    public class DealService<T> : BaseDocumentService<T>, IDealService, IWFObjectService where T : Deal, new()
    {
        private readonly IUnitOfWorkFactory _factory;
        private readonly IBaseObjectService<DealStatus> _dealStatusService;

        public DealService(IBaseObjectServiceFacade facade, IUnitOfWorkFactory factory, IBaseObjectService<DealStatus> dealStatusService, IUserService<User> userService, IEmployeeUserService employeeUserService) : base(facade, userService, employeeUserService)
        {
            _factory = factory;
            _dealStatusService = dealStatusService;
        }


        public List<SalesFunnel> GetSalesFunnel(SalesFunnelPreset salesFunnelPreset)
        {
            using (var uofw = _factory.CreateSystem())
            {
                


                var result = new List<SalesFunnel>();

                var deals = Base.Ambient.AppContext.SecurityUser.IsSysRole(SystemRole.Ceo)
                    ? this.GetAll(uofw)
                    : this.GetAll(uofw)
                        .Where(x => x.CreatorID == Base.Ambient.AppContext.SecurityUser.ID);

                var documentStatuses =
                    _dealStatusService.GetAll(uofw).Where(x => x.IsFunnel).OrderBy(x => x.SortOrder).ToList();

                var statusCount = documentStatuses.Count();

                var dealsCount = deals.Count();

                if (dealsCount == 0)
                    return null;

                for (int i = 0; i < statusCount; i++)
                {
                    var status = documentStatuses[i];

                    if (i == 0)
                    {
                        result.Add(new SalesFunnel()
                        {
                            Percent = 100,
                            Title = status.Title,
                            Color = status.Icon?.Color
                        });
                    }
                    else
                    {
                        var percent =
                            Math.Round(
                                (double)(dealsCount - deals.Count(x => x.Status.SortOrder < status.SortOrder)) * 100 /
                                dealsCount, 2);
                        result.Add(new SalesFunnel()
                        {
                            Percent = percent,
                            Title = status.Title,
                            Color = status.Icon?.Color
                        });
                    }
                }
                return result;
            }
        }

        protected override IObjectSaver<T> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<T> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneObject(x => x.Status)
                .SaveOneToMany(x => x.Nomenclature, x => x
                    .SaveOneObject(v => v.Nomenclature))
                .SaveOneToMany(x => x.NomenclatureVersion, x => x
                    .SaveOneObject(v => v.Nomenclature)
                    .SaveOneObject(v => v.DocumentStatus))
                .SaveOneToMany(x => x.DealDiscounts, x => x.SaveOneObject(o => o.Object));
        }

        public void BeforeInvoke(BaseObject obj)
        {

        }

        public void OnActionExecuting(ActionExecuteArgs args)
        {
            var dealOld = args.OldObject as Deal;
            if (dealOld == null)
                throw new Exception("Невозможно привести старый объект к типу \"Сделка\"!");

            var dealNew = args.NewObject as Deal;
            if (dealNew == null)
                throw new Exception("Невозможно привести новый объект к типу \"Сделка\"!");

            if (dealOld.Status != dealNew.Status && dealOld.Status != null)
            {
                if (dealOld.Nomenclature != null && dealOld.Nomenclature.Any())
                {
                    foreach (var dealNomenclature in dealOld.Nomenclature)
                    {
                        DealNomenclatureVersion tempObjDealNomenclature = new DealNomenclatureVersion
                        {
                            DocumentStatusID = dealOld.Status.ID,
                            NomenclatureID = dealNomenclature.Nomenclature.ID,
                            Amount = dealNomenclature.Amount
                        };
                        dealNew.NomenclatureVersion.Add(tempObjDealNomenclature);
                    }
                    args.NewObject = dealNew;
                }
            }
        }
    }
}