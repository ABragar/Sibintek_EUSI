using Base.BusinessProcesses.Services.Concrete;
using Base.BusinessProcesses.Strategies;
using Base.DAL;
using Base.Service;
using Base.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using Base.Exceptions;
using Base.Service.Crud;

namespace WebUI.Concrete
{
    public class BaseObjectServiceResolver : IWorkflowServiceResolver
    {
        private readonly IViewModelConfigService _configService;
        private readonly IServiceLocator _locator;

        public BaseObjectServiceResolver(IViewModelConfigService configService, IServiceLocator locator)
        {
            _configService = configService;
            _locator = locator;
        }

        public IBaseObjectCrudService GetObjectService(string objectTypeStr, IUnitOfWork unitOfWork = null)
        {
            //TODO : Преезжаем на GetTypeName()
            var config = _configService.GetAll().FirstOrDefault(x => x.TypeEntity.FullName == objectTypeStr || x.Entity == objectTypeStr);

            if (config == null)
                throw new Exception("Type not founded");

            return config.GetService<IBaseObjectCrudService>();
        }

        public ICollection<IStakeholdersSelectionStrategy> GetStakeholdersSelectionStrategies()
        {
            return _locator.GetServices(typeof(IStakeholdersSelectionStrategy)).Cast<IStakeholdersSelectionStrategy>().ToArray();
        }

        public IStakeholdersSelectionStrategy GetStakeholdersSelectionStrategy(Type type)
        {
            if (type == null)
                throw new ArgumentNullException($"Тип не найден {nameof(type)}");
            try
            {
                return (IStakeholdersSelectionStrategy)_locator.GetService(type);
            }
            catch (ActivationException ex)
            {
                throw new Exception($"Не удалось найти стратегию по типу {type}", ex);
            }



        }

    }
}