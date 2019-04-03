using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;
using Base.Extensions;
using Base.Service;
using Base.UI.Filter;
using Base.UI.Presets;
using Base.UI.Service.Abstract;
using System.Linq.Dynamic;
using Base.UI.QueryFilter;
using Newtonsoft.Json.Linq;

namespace Base.UI.Service.Concrete
{
    public class MnemonicFilterService<T> : BaseObjectService<T>, IMnemonicFilterService<T> where T : MnemonicFilter
    {
        private readonly IPresetService<GridPreset> _presetService;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IQueryTreeFilter _filter_service;

        public MnemonicFilterService(IBaseObjectServiceFacade facade, IPresetService<GridPreset> presetService, 
            IUnitOfWorkFactory unitOfWorkFactory, IQueryTreeFilter filter_service) : base(facade)
        {
            _presetService = presetService;
            _unitOfWorkFactory = unitOfWorkFactory;
            _filter_service = filter_service;
        }

        public async Task<IQueryable<TObject>> AddMnemonicFilter<TObject>(IUnitOfWork uofw, IQueryable<TObject> q, string mnemonic, int mnemonicFilterId) where TObject: IBaseObject
        {
            var filter = await GetAll(uofw).Where(x => x.ID == mnemonicFilterId).SingleOrDefaultAsync();
            QueryTreeService.OperatorInValues = uofw.GetRepository<OperatorInValues>().Filter(f => f.MnemonicFilterOid == filter.Oid)
                .Expression;
            q = await _filter_service.BuildQueryAsync(uofw, q, filter.Filter, async () =>
            {
                return JToken.Parse(filter.Filter);
            }, mnemonic);

            return q;
        }

        public async Task<IQueryable> AddMnemonicFilter(IUnitOfWork uofw, IQueryable q, string mnemonic, int mnemonicFilterId)
        {
            var filter = await GetAll(uofw).Where(x => x.ID == mnemonicFilterId).SingleOrDefaultAsync();
            QueryTreeService.OperatorInValues = uofw.GetRepository<OperatorInValues>().Filter(f => f.MnemonicFilterOid == filter.Oid)
                .Expression;
            q = await _filter_service.BuildQueryAsync(uofw, q, filter.Filter, async () =>
            {
                return JToken.Parse(filter.Filter);
            }, mnemonic);

            return q;
        }

        public override IQueryable<T> GetAll(IUnitOfWork unitOfWork, bool? hidden = false)
        {
            return
                base.GetAll(unitOfWork, hidden)
                    .Where(x => x.UserID == null || x.UserID == Base.Ambient.AppContext.SecurityUser.ID);
        }
    }
}
