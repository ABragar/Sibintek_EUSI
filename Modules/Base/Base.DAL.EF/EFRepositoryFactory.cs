using System.Data.Entity;

namespace Base.DAL.EF
{
    public class EFRepositoryFactory<TContext> : IRepositoryFactory<TContext>
        where TContext : DbContext, IBaseContext
    {


        public static readonly EFRepositoryFactory<TContext> Instance = new EFRepositoryFactory<TContext>();

        public IRepository<T> CreateRepository<T>(TContext context) where T : BaseObject
        {
            return new EFRepository<T>(context);
        }
    }
}