using Base.DAL.EF.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Base.Utils.Common;

namespace Base.DAL.EF
{
    internal class EFRepository<TObject> : IRepository<TObject> where TObject : BaseObject
    {
        private DbContext _context = null;

        public EFRepository(DbContext context)
        {
            _context = context;
        }

        public DbSet<TObject> DbSet => _context.Set<TObject>();

        public virtual IExtendedQueryable<TObject> All()
        {
            return DbSet.AsExtendedQueryable();
        }

        public virtual IExtendedQueryable<TObject> Filter(Expression<Func<TObject, bool>> predicate)
        {
            return DbSet.Where(predicate).AsExtendedQueryable();
        }

        public async Task<IEnumerable<TObject>> FilterAsync(Expression<Func<TObject, bool>> predicate)
        {
            return await DbSet.Where(predicate).ToListAsync();
        }

        public Task<IEnumerable<TObject>> FilterAsync(IDictionary<string, object> param)
        {
            throw new NotImplementedException();
        }

        public virtual TObject Find(params object[] keys)
        {
            return DbSet.Find(keys);
        }

        public virtual TObject Find(Expression<Func<TObject, bool>> predicate)
        {
            return DbSet.SingleOrDefault(predicate);
        }

        public virtual TObject Create(TObject TObject)
        {
            var newEntry = DbSet.Add(TObject);
            return newEntry;
        }

        public virtual void CreateCollection(IEnumerable<TObject> objs)
        {
            DbSet.AddRange(objs);            
        }

        public virtual int Count
        {
            get
            {
                return DbSet.Count();
            }
        }

        public virtual int Delete(TObject TObject)
        {
            DbSet.Remove(TObject);
            return 0;
        }

        public virtual int Update(TObject TObject)
        {
            DbSet.Attach(TObject);
            _context.Entry<TObject>(TObject).State = EntityState.Modified;
            return 0;
        }

        public virtual void Attach(TObject TObject)
        {
            DbSet.Attach(TObject);
        }

        public virtual void Detach(TObject TObject)
        {
            _context.Entry<TObject>(TObject).State = EntityState.Detached;
        }

        public virtual int Delete(Expression<Func<TObject, bool>> predicate)
        {
            var objects = Filter(predicate).ToArray();
            foreach (var obj in objects)
                DbSet.Remove(obj);

            return objects.Length;
        }

        public IObjectSaver<TObject> GetObjectSaver(IUnitOfWork unitOfWork, TObject objSrc, TObject objDest)
        {
            return new ObjectSaver<TObject>(unitOfWork, objSrc, objDest);
        }

        public TObject GetOriginal(int id)
        {
            return DbSet.AsNoTracking().FirstOrDefault(x => x.ID == id);
        }

        public bool AutoDetectChangesEnabled
        {
            get { return _context.Configuration.AutoDetectChangesEnabled; }
            set { _context.Configuration.AutoDetectChangesEnabled = value; }
        }

        public bool ValidateOnSaveEnabled
        {
            get { return _context.Configuration.ValidateOnSaveEnabled; }
            set { _context.Configuration.ValidateOnSaveEnabled = value; }
        }

        public void ChangeProperty<TProperty>(int id, Expression<Func<TObject, TProperty>> propFunc, TProperty value, byte[] rowVersion = null)
        {
            var obj = Activator.CreateInstance<TObject>();
            obj.ID = id;
            obj.RowVersion = rowVersion ?? All().Where(x => x.ID == id).Select(x => x.RowVersion).Single();

            Attach(obj);
            obj.SetPropertyValue(propFunc, value);

            _context.Entry<TObject>(obj).Property(propFunc).IsModified = true;
        }

        //sib       

        public virtual IExtendedQueryable<TObject> FilterAsNoTracking(Expression<Func<TObject, bool>> predicate)
        {
            return DbSet.AsNoTracking().Where(predicate).AsExtendedQueryable();
        }

        public virtual TObject FindAsNoTracking(int id)
        {
            TObject obj = DbSet.AsNoTracking().FirstOrDefault(w => w.ID == id);            
            return obj;
        }

        //end sib
    }
}
