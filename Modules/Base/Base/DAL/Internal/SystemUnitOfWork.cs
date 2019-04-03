
namespace Base.DAL.Internal
{
    internal class SystemUnitOfWork : UnitOfWork, ISystemUnitOfWork
    {
        public SystemUnitOfWork(IRepositoryManager repository_manager, IEntityConfiguration entityConfiguration)
            : base(repository_manager, entityConfiguration)
        {

        }
    }

    internal class WrapSystemUnitOfWork : WrapperUnitOfWork, ISystemUnitOfWork
    {
        public WrapSystemUnitOfWork(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }
    }
}
