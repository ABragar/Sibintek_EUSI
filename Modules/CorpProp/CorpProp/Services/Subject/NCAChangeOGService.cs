using Base.DAL;
using Base.Service;
using CorpProp.Entities.Asset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppContext = Base.Ambient.AppContext;
using CorpProp.Extentions;
using CorpProp.Entities.Subject;
using CorpProp.Common;
using CorpProp.Entities.CorporateGovernance;
using Base.Extensions;

namespace CorpProp.Services.Subject
{

    public interface INCAChangeOGService : IBaseObjectService<Society>, ICustomDataSource<Society>
    {
    }

    /// <summary>
    /// Сервис для отображения окна выбора списка ОГ предполагаемых правопреемников заданного ОГ.
    /// </summary>
    public class NCAChangeOGService : BaseObjectService<Society>, INCAChangeOGService
    {


        public NCAChangeOGService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }


        public IQueryable<Society> GetAllCustom(IUnitOfWork uow, params object[] objs)
        {
            if (objs == null || objs.Length < 1) return base.GetAll(uow);

            var ogID = Convert.ToInt32(objs[0]);
            var og = uow.GetRepository<Society>().Find(ogID);

            if (og != null && !String.IsNullOrEmpty(og.INN))
            {
                var ds = uow.GetRepository<Predecessor>()
                .Filter(f => !f.Hidden && !f.IsHistory && f.INN == og.INN)
                .Include(inc => inc.SocietySuccessor)
                .Select(s => s.SocietySuccessor);

                if (ds.Any())
                    return ds;
            }

            return base.GetAll(uow);

        }
    }
}
