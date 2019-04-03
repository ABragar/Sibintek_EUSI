namespace Base.DAL
{

    public interface IRepositoryFactory<TContext> 
        where TContext : IBaseContext
    {
        IRepository<T> CreateRepository<T>(TContext context)
            where T : BaseObject;
    }
}