using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Base.DAL
{
    public interface IRepository<T> where T : BaseObject
    {
        /// <summary>
        /// Gets all objects from database
        /// </summary>
        IExtendedQueryable<T> All();

        DbSet<T> DbSet { get; }

        /// <summary>
        /// Gets objects from database by filter.
        /// </summary>
        /// <param name="predicate">Specified a filter</param>
        IExtendedQueryable<T> Filter(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FilterAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FilterAsync(IDictionary<string, object> param);

        /// <summary>
        /// Find object by keys.
        /// </summary>
        /// <param name="keys">Specified the search keys.</param>
        T Find(params object[] keys);

        /// <summary>
        /// Find object by specified expression.
        /// </summary>
        /// <param name="predicate"></param>
        T Find(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// TCreate a new object to database.
        /// </summary>
        /// <param name="t">Specified a new object to create.</param>
        T Create(T t);

        void CreateCollection(IEnumerable<T> objs);

        /// <summary>
        /// TDelete the object from database.
        /// </summary>
        /// <param name="t">Specified a existing object to delete.</param>        
        int Delete(T t);

        /// <summary>
        /// TDelete objects from database by specified filter expression.
        /// </summary>
        /// <param name="predicate"></param>
        int Delete(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// TUpdate object changes and save to database.
        /// </summary>
        int Update(T t);

        /// <summary>
        /// Attach
        /// </summary>
        /// <param name="t">object attach.</param>
        void Attach(T t);

        /// <summary>
        /// Detach
        /// </summary>
        /// <param name="t">object attach.</param>
        void Detach(T t);

        /// <summary>
        /// Get the total objects count.
        /// </summary>
        
        /// <summary>
        /// 
        /// </summary>
        IObjectSaver<T> GetObjectSaver(IUnitOfWork unitOfWork, T objSrc, T objDest);

        T GetOriginal(int id);

        bool AutoDetectChangesEnabled { get; set; }
        bool ValidateOnSaveEnabled { get; set; }

        void ChangeProperty<TProperty>(int id, Expression<Func<T, TProperty>> propFunc, TProperty value, byte[] rowVersion = null);

        //sib
        IExtendedQueryable<T> FilterAsNoTracking(Expression<Func<T, bool>> predicate);

        T FindAsNoTracking(int id);
        //end sib

    }

}