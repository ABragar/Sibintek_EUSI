using Base.DAL;
using Base.Security;
using System.Linq;

namespace Base.Service
{
    public class BaseCategoryService<TH> : BaseObjectService<TH>, IBaseCategoryService<TH>
        where TH : HCategory
    {
        public BaseCategoryService(IBaseObjectServiceFacade facade) : base(facade) { }

        public virtual IQueryable<TH> GetRoots(IUnitOfWork unitOfWork, bool? hidden = false)
        {
            return this.GetAll(unitOfWork, hidden).Where(node => node.sys_all_parents == null).OrderBy(m => m.SortOrder);
        }

        public virtual IQueryable<TH> GetAllChildren(IUnitOfWork unitOfWork, int parentID, bool? hidden = false)
        {
            string strID = HCategory.IdToString(parentID);

            return this.GetAll(unitOfWork, hidden).Where(node => (node.sys_all_parents != null && node.sys_all_parents.Contains(strID))).OrderBy(m => m.SortOrder);
        }


        public virtual IQueryable<TH> GetChildren(IUnitOfWork unitOfWork, int parentID, bool? hidden = false)
        {
            return this.GetAll(unitOfWork, hidden).Where(node => node.ParentID == parentID).OrderBy(m => m.SortOrder);
        }

        public override TH Create(IUnitOfWork unitOfWork, TH obj)
        {
            if (obj.ParentID == 0) obj.ParentID = null;

            if (obj.ParentID != null || obj.Parent != null)
            {
                var id = obj.ParentID ?? obj.Parent.ID;
                var parent  = unitOfWork.GetRepository<TH>().Find(x => x.ID == id);

                obj.SetParent(parent);
            }

            return base.Create(unitOfWork, obj);
        }

        public override TH Update(IUnitOfWork unitOfWork, TH obj)
        {
            if (obj.ParentID == 0) obj.ParentID = null;

            if (obj.ParentID != null)
            {
                var parent = unitOfWork.GetRepository<TH>().Find(x => x.ID == obj.ParentID);

                obj.SetParent(parent);
            }

            return base.Update(unitOfWork, obj);
        }

        public override void Delete(IUnitOfWork unitOfWork, TH obj)
        {
            this.GetAllChildren(unitOfWork, obj.ID).ToList().ForEach(node =>
            { 
                base.Delete(unitOfWork, node);
            });

            base.Delete(unitOfWork, obj);
        }


        protected override void InitSortOrder(IUnitOfWork unitOfWork, TH obj)
        {
            if (obj.SortOrder == -1)
            {
                int parentID = obj.ParentID ?? 0;
                if (parentID == 0)
                {
                    obj.SortOrder =
                        this.GetAll(unitOfWork, false)
                            .Where(node => node.ParentID == null)
                            .Select(x => x.SortOrder)
                            .DefaultIfEmpty(0)
                            .Max() + 1;
                }
                else
                {
                    var max = this.GetChildren(unitOfWork, parentID).Select<TH, double?>(x => x.SortOrder).Max();
                    obj.SortOrder = (max ?? 0) + 1;
                }
            }
        }

        public virtual void ChangePosition(IUnitOfWork unitOfWork, TH obj, int? posChangeID, string typePosChange)
        {
            //TODO: access
            //SecurityService.ThrowIfAccessDenied(unitOfWork, typeof(TH), TypePermission.Write);

            var repository = unitOfWork.GetRepository<TH>();

            if (posChangeID != null && posChangeID != 0)
            {
                if (typePosChange == "over")
                {
                    var parent = this.Get(unitOfWork, (int)posChangeID);

                    if (parent != null)
                    {
                        var max = this.GetChildren(unitOfWork, (int)posChangeID).Select<TH, double?>(x => x.SortOrder).Max();
                        obj.SortOrder = max ?? 0 + 1;
                    }

                    this.ChangeParent(unitOfWork, obj, parent);
                }
                else
                {
                    var hc = this.Get(unitOfWork, (int)posChangeID);

                    if (hc != null)
                    {
                        if (hc.ParentID != obj.ParentID)
                        {
                            if (hc.ParentID != null)
                                this.ChangeParent(unitOfWork, obj, (hc.Parent ?? repository.Find((int)hc.ParentID)) as TH);
                            else
                                this.ChangeParent(unitOfWork, obj, null);
                        }

                        if (typePosChange == "after")
                        {
                            double order = repository.All().Where(x => x.SortOrder > hc.SortOrder)
                                .Select(x => x.SortOrder).DefaultIfEmpty(0).Min();

                            obj.SortOrder = hc.SortOrder + (order - hc.SortOrder) / 2;
                        }
                        else
                        {
                            double order = repository.All().Where(x => x.SortOrder < hc.SortOrder)
                                .Select(x => x.SortOrder).DefaultIfEmpty(0).Max();

                            obj.SortOrder = hc.SortOrder - (hc.SortOrder - order) / 2;
                        }

                        repository.Update(obj);
                    }
                }
            }

            unitOfWork.SaveChanges();
        }

        private void ChangeParent(IUnitOfWork unitOfWork, TH obj, TH parent)
        {
            obj.SetParent(parent);

            unitOfWork.GetRepository<TH>().Update(obj);

            this.GetChildren(unitOfWork, obj.ID).ToList().ForEach(node =>
            {
                this.ChangeParent(unitOfWork, node, obj);
            });
        }


    }

    public class BaseCategoryService<TH, THBase> : BaseObjectService<TH>, IBaseCategoryService<TH>
       where TH : THBase
       where THBase : HCategory, new()
    {
        public BaseCategoryService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        public virtual IQueryable<TH> GetRoots(IUnitOfWork unitOfWork, bool? hidden = false)
        {
            return this.GetAll(unitOfWork, hidden).Where(node => node.sys_all_parents == null).OrderBy(m => m.SortOrder);
        }

        public virtual IQueryable<TH> GetAllChildren(IUnitOfWork unitOfWork, int parentID, bool? hidden = false)
        {
            string strID = HCategory.IdToString(parentID);

            return this.GetAll(unitOfWork, hidden).GetAllChildren(parentID).OrderBy(m => m.SortOrder);
        }


        public virtual IQueryable<TH> GetChildren(IUnitOfWork unitOfWork, int parentID, bool? hidden = false)
        {
            return this.GetAll(unitOfWork, hidden).Where(node => node.ParentID == parentID);
        }

        public override TH Create(IUnitOfWork unitOfWork, TH obj)
        {
            if (obj.ParentID == 0) obj.ParentID = null;

            if (obj.ParentID != null)
            {
                var parent = unitOfWork.GetRepository<THBase>().Find(x => x.ID == obj.ParentID);

                obj.SetParent(parent);
            }

            return base.Create(unitOfWork, obj);
        }

        public override TH Update(IUnitOfWork unitOfWork, TH obj)
        {
            if (obj.ParentID == 0) obj.ParentID = null;

            if (obj.ParentID != null)
            {
                var parent = unitOfWork.GetRepository<THBase>().Find(x => x.ID == obj.ParentID);

                obj.SetParent(parent);
            }

            return base.Update(unitOfWork, obj);
        }

        public override void Delete(IUnitOfWork unitOfWork, TH obj)
        {
            var repository = unitOfWork.GetRepository<THBase>();
            var children = repository.All().GetAllChildren(obj.ID).Select(x => new { x.ID, x.RowVersion });

            foreach (var child in children)
            {
                repository.ChangeProperty(child.ID, x => x.Hidden, true, child.RowVersion);
            }

            base.Delete(unitOfWork, obj);
        }


        protected override void InitSortOrder(IUnitOfWork unitOfWork, TH obj)
        {
            var repository = unitOfWork.GetRepository<THBase>();
            if (obj.SortOrder == -1)
            {
                int parentID = obj.ParentID ?? 0;
                if (parentID == 0)
                {
                    obj.SortOrder =
                        this.GetAll(unitOfWork, false)
                            .Where(node => node.ParentID == null)
                            .Select(x => x.SortOrder)
                            .DefaultIfEmpty(0)
                            .Max() + 1;
                }
                else
                {
                    var max = repository.All().GetChildren(parentID).Select(x => x.SortOrder).DefaultIfEmpty().Max();
                    obj.SortOrder = (max) + 1;
                }
            }
        }

        public virtual void ChangePosition(IUnitOfWork unitOfWork, TH obj, int? posChangeID, string typePosChange)
        {
            //TODO: access
            //SecurityService.ThrowIfAccessDenied(unitOfWork, typeof(TH), TypePermission.Write);
            var repository = unitOfWork.GetRepository<THBase>();

            if (posChangeID != null && posChangeID != 0)
            {
                if (typePosChange == "over")
                {
                    //var parent = this.Get(unitOfWork, (int)posChangeID);
                    var parent = unitOfWork.GetRepository<THBase>().All().SingleOrDefault(x => x.ID == posChangeID);

                    if (parent != null)
                    {
                        var max = repository.All().GetChildren((int)posChangeID).Select<THBase, double?>(x => x.SortOrder).Max();
                        obj.SortOrder = max ?? 0 + 1;
                    }

                    this.ChangeParent(unitOfWork, obj, parent);
                }
                else
                {
                    //var hc = this.Get(unitOfWork, (int)posChangeID);
                    var hc = unitOfWork.GetRepository<THBase>().All().SingleOrDefault(x => x.ID == posChangeID);

                    if (hc != null)
                    {
                        if (hc.ParentID != obj.ParentID)
                        {
                            if (hc.ParentID != null)
                                this.ChangeParent(unitOfWork, obj, (hc.Parent ?? repository.Find((int)hc.ParentID)) as THBase);
                            else
                                this.ChangeParent(unitOfWork, obj, null);
                        }

                        if (typePosChange == "after")
                        {
                            double order = repository.All().Where(x => x.SortOrder > hc.SortOrder)
                                .Select(x => x.SortOrder).DefaultIfEmpty(0).Min();

                            obj.SortOrder = hc.SortOrder + (order - hc.SortOrder) / 2;
                        }
                        else
                        {
                            double order = repository.All().Where(x => x.SortOrder < hc.SortOrder)
                                .Select(x => x.SortOrder).DefaultIfEmpty(0).Max();

                            obj.SortOrder = hc.SortOrder - (hc.SortOrder - order) / 2;
                        }

                        repository.Update(obj);
                    }
                }
            }

            unitOfWork.SaveChanges();
        }

        private void ChangeParent(IUnitOfWork unitOfWork, THBase obj, THBase parent)
        {
            obj.SetParent(parent);

            var repository = unitOfWork.GetRepository<THBase>().All().Where(x => !x.Hidden);
            var children = repository.GetChildren(obj.ID).ToList();

            foreach (var child in children)
            {
                this.ChangeParent(unitOfWork, child, obj);
            }
        }
    }

    public static class HCategoryExt
    {
        public static IQueryable<TH> GetAllChildren<TH>(this IQueryable<TH> q, int parentID) where TH : HCategory
        {
            string strID = HCategory.IdToString(parentID);
            return q.Where(node => (node.sys_all_parents != null && node.sys_all_parents.Contains(strID)));
        }

        public static IQueryable<TH> GetChildren<TH>(this IQueryable<TH> q, int? parentID) where TH : HCategory
        {
            return q.Where(node => node.ParentID == parentID);
        }
    }
}
