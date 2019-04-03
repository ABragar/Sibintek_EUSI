using Base.DAL.Internal;
using Base.Service;

namespace Base.DAL
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IServiceFactory<IRepositoryManager> _repository_manager_factory;
        private readonly IEntityConfiguration _configuration;

        public UnitOfWorkFactory(IServiceFactory<IRepositoryManager> repository_manager_factory, IEntityConfiguration configuration)
        {
            _repository_manager_factory = repository_manager_factory;
            _configuration = configuration;
        }


        public IUnitOfWork Create()
        {
            return new UnitOfWork(_repository_manager_factory.GetService(), _configuration);
        }

        public ISystemUnitOfWork CreateSystem()
        {
            return new SystemUnitOfWork(_repository_manager_factory.GetService(),_configuration);
        }

        public ISystemUnitOfWork CreateSystem(IUnitOfWork unitOfWork)
        {
            return new WrapSystemUnitOfWork(unitOfWork);
        }

        public ITransactionUnitOfWork CreateTransaction()
        {
            return new TransactionUnitOfWork(_repository_manager_factory.GetService(),_configuration);
        }

        public ISystemTransactionUnitOfWork CreateSystemTransaction()
        {
            return new SystemTransactionUnitOfWork(_repository_manager_factory.GetService(),_configuration);
        }
    }
}