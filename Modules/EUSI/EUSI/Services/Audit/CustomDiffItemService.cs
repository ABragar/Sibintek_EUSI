using Base.Audit.Entities;
using Base.DAL;
using Base.Extensions;
using Base.Service;
using CorpProp.Common;
using CorpProp.Entities.Import;
using CorpProp.Entities.Security;
using CorpProp.Extentions;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.Services.Base;
using EUSI.Entities.Audit;
using EUSI.Entities.BSC;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUSI.Services.Audit
{

    public interface ICustomDiffItemService : IBaseObjectService<CustomDiffItem>
    {

    }

    public class CustomDiffItemService : BaseObjectService<CustomDiffItem>, ICustomDiffItemService
    {
        public CustomDiffItemService(IBaseObjectServiceFacade facade) : base(facade)
        {

        }

        public override IQueryable<CustomDiffItem> GetAll(IUnitOfWork unitOfWork, bool? hidden = false)
        {
            var audit = unitOfWork.GetRepository<AuditItem>().Filter(f => f.Hidden == hidden).Include(inc => inc.Entity);
            var diff = unitOfWork.GetRepository<DiffItem>().Filter(f => f.Hidden == hidden);
            var user = unitOfWork.GetRepository<SibUser>().Filter(f => f.Hidden == hidden);


            var q = diff.Join(audit, d => d.ParentID, a => a.ID, (d, a) => new { d, a })
                .Join(user, da=>da.a.UserID, u => u.UserID, (da, u) => 
                
                new CustomDiffItem()
                {
                    ID = da.d.ID,
                    Hidden = da.d.Hidden,
                    RowVersion = da.d.RowVersion,
                    SortOrder = da.d.SortOrder,
                    Date = da.a.Date,
                    Type = da.a.Type,
                    Entity = da.a.Entity,
                    EntityID = da.a.Entity.ID,
                    EntityType = da.a.EntityType,
                    UserID = da.a.UserID,
                    User = da.a.User,
                    SibUserID = u.ID,
                    SibUser = u,
                    Description = da.a.Description,
                    ParentID = da.d.ParentID,
                    Parent = da.d.Parent,
                    Property = da.d.Property,
                    OldValue = da.d.OldValue,
                    NewValue = da.d.NewValue,
                    Member = da.d.Member
                });

            return q;
        }
    }
}
