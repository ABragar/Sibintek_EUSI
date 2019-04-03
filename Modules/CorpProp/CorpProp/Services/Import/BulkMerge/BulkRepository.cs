using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Base;
using Base.DAL;
using CorpProp.Helpers;

namespace CorpProp.Services.Import.BulkMerge
{
    public class BulkRepository : IBulkRepository<BaseObject>
    {
        private DbContext _context = null;
        

        public DbSet<BaseObject> DbSet => _context.Set<BaseObject>();

      




        public bool AutoDetectChangesEnabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool ValidateOnSaveEnabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IExtendedQueryable<BaseObject> All()
        {
            throw new NotImplementedException();
        }

        public void Attach(BaseObject t)
        {
            throw new NotImplementedException();
        }

        public void ChangeProperty<TProperty>(int id, Expression<Func<BaseObject, TProperty>> propFunc, TProperty value, byte[] rowVersion = null)
        {
            throw new NotImplementedException();
        }

        public IExtendedQueryable<BaseObject> FilterAsNoTracking(Expression<Func<BaseObject, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public BaseObject FindAsNoTracking(int id)
        {
            throw new NotImplementedException();
        }

        public BaseObject Create(BaseObject t)
        {
            throw new NotImplementedException();
        }

        public void CreateCollection(IEnumerable<BaseObject> objs)
        {
            throw new NotImplementedException();
        }

        public int Delete(BaseObject t)
        {
            throw new NotImplementedException();
        }

        public int Delete(Expression<Func<BaseObject, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void Detach(BaseObject t)
        {
            throw new NotImplementedException();
        }

        public IExtendedQueryable<BaseObject> Filter(Expression<Func<BaseObject, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BaseObject>> FilterAsync(Expression<Func<BaseObject, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BaseObject>> FilterAsync(IDictionary<string, object> param)
        {
            throw new NotImplementedException();
        }

        public BaseObject Find(params object[] keys)
        {
            throw new NotImplementedException();
        }

        public BaseObject Find(Expression<Func<BaseObject, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IObjectSaver<BaseObject> GetObjectSaver(IUnitOfWork unitOfWork, BaseObject objSrc, BaseObject objDest)
        {
            throw new NotImplementedException();
        }

        public BaseObject GetOriginal(int id)
        {
            throw new NotImplementedException();
        }

        public int Update(BaseObject t)
        {
            throw new NotImplementedException();
        }
    }
}
