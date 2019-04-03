using System;
using System.Linq.Expressions;

namespace Base.DAL
{
    public abstract class BaseObjectTypeRepository<TObject> : ITypesRepository<TObject> where TObject : BaseObjectType
    {

        protected BaseObjectTypeRepository()
        {

        }


        public virtual IExtendedQueryable<TObject> All()
        {
            return null; //DbSet.AsExtendedQueryable();
        }

        public virtual IExtendedQueryable<TObject> Filter(Expression<Func<TObject, bool>> predicate)
        {
            return null;//DbSet.Where(predicate).AsExtendedQueryable();
        }

        public virtual IExtendedQueryable<TObject> Filter(Expression<Func<TObject, bool>> filter, out int total,
            int index = 0, int size = 50)
        {
            //var skipCount = index * size;
            //var _resetSet = filter != null ? DbSet.Where(filter).AsQueryable() : DbSet.AsQueryable();
            //_resetSet = skipCount == 0 ? _resetSet.Take(size) : _resetSet.Skip(skipCount).Take(size);
            total = 0;
            //return _resetSet.AsExtendedQueryable();
            return null;
        }

        public virtual bool Contains(Expression<Func<TObject, bool>> predicate)
        {
            //return DbSet.Count(predicate) > 0;
            return false;
        }

        public virtual TObject Find(params object[] keys)
        {
            //return DbSet.Find(keys);
            return null;
        }

        public virtual TObject Find(Expression<Func<TObject, bool>> predicate)
        {
            //return DbSet.FirstOrDefault(predicate);
            return null;
        }

        public virtual TObject Create(TObject TObject)
        {
            //var newEntry = DbSet.Add(TObject);
            //return newEntry;
            return null;
        }

        public virtual int Count
        {
            get { return 0; }
        }

        public virtual int Delete(TObject TObject)
        {

            return 0;
        }

        public virtual int Update(TObject TObject)
        {
            return 0;
        }

        public virtual void Attach(TObject TObject)
        {
            //DbSet.Attach(TObject);
            return;
        }

        public virtual void Detach(TObject TObject)
        {
            // _context.Entry(TObject).State = EntityState.Detached;
        }

        public virtual int Delete(Expression<Func<TObject, bool>> predicate)
        {
            //var objects = Filter(predicate);
            //foreach (var obj in objects) DbSet.Remove(obj);
            return 0;
        }

        public virtual bool AutoDetectChangesEnabled { get; set; }


        public virtual bool ValidateOnSaveEnabled { get; set; }
    }
}
