
namespace Base.DAL.Internal
{
    internal class SystemTransactionUnitOfWork : TransactionUnitOfWork, ISystemTransactionUnitOfWork
    {
        public SystemTransactionUnitOfWork(IRepositoryManager repository_manager, IEntityConfiguration entityConfiguration)
            : base(repository_manager, entityConfiguration)
        {

        }
    }
}
