using Base;
using Base.DAL;
using Base.Security;
using Base.Security.Service.Abstract;
using Base.Service;
using Base.UI.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Base.UI.ViewModal;
using Base.Utils.Common.Caching;
using Base.Utils.Common.Wrappers;
using WebUI.Authorize;
using WebUI.Helpers;
using WebUI.Models;
using AppContext = Base.Ambient.AppContext;

namespace WebUI.Controllers
{
    public interface IBaseController
    {
        IUnitOfWork CreateUnitOfWork();
        ISystemUnitOfWork CreateSystemUnitOfWork();
        ISystemUnitOfWork CreateSystemUnitOfWork(IUnitOfWork unitOfWork);
        ITransactionUnitOfWork CreateTransactionUnitOfWork();
        ISystemTransactionUnitOfWork CreateSystemTransactionUnitOfWork();
        ISecurityService SecurityService { get; }
        ISecurityUser SecurityUser { get; }
        IFileSystemService FileSystemService { get; }
        IEnumerable<ViewModelConfig> GetViewModelConfigs();
        ViewModelConfig GetViewModelConfig(string mnemonic);
        ViewModelConfig GetViewModelConfig(Type type);
        ViewModelConfig GetViewModelConfig(Func<ViewModelConfig, bool> filter);
        ISimpleCacheWrapper CacheWrapper { get; }
        ISessionWrapper SessionWrapper { get; }
        IUiFasade UiFasade { get; }
        HttpContextBase HttpContext { get; }
        IPathHelper PathHelper { get; }
        //sib
        IUnitOfWorkFactory UnitOfWorkFactory { get; }
        //end sib
    }

    public interface IBaseControllerServiceFacade
    {
        IUnitOfWorkFactory UnitOfWorkFactory { get; }
        ISecurityService SecurityService { get; }
        IFileSystemService FileSystemService { get; }
        ISimpleCacheWrapper CacheWrapper { get; }
        ISessionWrapper SessionWrapper { get; }
        IUiFasade UiFasade { get; }
        IPathHelper PathHelper { get; }
        IAutoMapperCloner AutoMapperCloner { get; }
    }

    public class BaseControllerServiceFacade : IBaseControllerServiceFacade
    {
        public BaseControllerServiceFacade(IUnitOfWorkFactory unitOfWorkFactory, ISecurityService securityService, 
            IFileSystemService fileSystemService, ISimpleCacheWrapper cacheWrapper,
            ISessionWrapper sessionWrapper, IUiFasade uiFasade, IPathHelper pathHelper, IAutoMapperCloner autoMapperCloner)
        {
            UnitOfWorkFactory = unitOfWorkFactory;
            SecurityService = securityService;
            FileSystemService = fileSystemService;
            CacheWrapper = cacheWrapper;
            SessionWrapper = sessionWrapper;
            UiFasade = uiFasade;
            PathHelper = pathHelper;
            AutoMapperCloner = autoMapperCloner;
        }

        public IUnitOfWorkFactory UnitOfWorkFactory { get; }

        public ISecurityService SecurityService { get; }

        public IFileSystemService FileSystemService { get; }

        public ISimpleCacheWrapper CacheWrapper { get; }

        public ISessionWrapper SessionWrapper { get; }

        public IUiFasade UiFasade { get; }

        public IPathHelper PathHelper { get; }
        
        public IAutoMapperCloner AutoMapperCloner { get; }
    }

    [AuthorizeCustom]
    public abstract class BaseController : Controller, IBaseController
    {
        private readonly IBaseControllerServiceFacade _serviceFacade;
        public ISecurityService SecurityService => _serviceFacade.SecurityService;
        private ISecurityUser _securityUser;
        public ISecurityUser SecurityUser => _securityUser ?? (_securityUser = AppContext.SecurityUser);
        public IFileSystemService FileSystemService => _serviceFacade.FileSystemService;
        public ISimpleCacheWrapper CacheWrapper => _serviceFacade.CacheWrapper;
        public ISessionWrapper SessionWrapper => _serviceFacade.SessionWrapper;
        public IUiFasade UiFasade => _serviceFacade.UiFasade;
        //sib
        public IUnitOfWorkFactory UnitOfWorkFactory => _serviceFacade.UnitOfWorkFactory;
        //end sib
        protected BaseController(IBaseControllerServiceFacade serviceFacade)
        {
            _serviceFacade = serviceFacade;
        }



        protected IBaseControllerServiceFacade Facade => _serviceFacade;

        public IEnumerable<ViewModelConfig> GetViewModelConfigs()
        {
            return UiFasade.GetViewModelConfigs();
        }

        public ViewModelConfig GetViewModelConfig(string mnemonic)
        {
            return UiFasade.GetViewModelConfig(mnemonic);
        }

        public ViewModelConfig GetViewModelConfig(Type type)
        {
            return UiFasade.GetViewModelConfig(type);
        }

        public ViewModelConfig GetViewModelConfig(Func<ViewModelConfig, bool> filter)
        {
            return UiFasade.GetViewModelConfig(filter);
        }

        public T GetService<T>(string mnemonic) where T : IService
        {
            var config = GetViewModelConfig(mnemonic);

            if (config == null)
                throw new Exception($"config [{mnemonic}] not found");

            return config.GetService<T>();
        }

        public Type GetTypeEntity(string mnemonic)
        {
            return this.GetViewModelConfig(mnemonic).TypeEntity;
        }


        public JsonNetResult JsonNet(object jObject)
        {
            return new JsonNetResult(jObject);
        }

        HttpContextBase IBaseController.HttpContext => this.HttpContext;

        public IPathHelper PathHelper => Facade.PathHelper;

        public IUnitOfWork CreateUnitOfWork()
        {
            return _serviceFacade.UnitOfWorkFactory.Create();
        }

        public ISystemUnitOfWork CreateSystemUnitOfWork()
        {
            return _serviceFacade.UnitOfWorkFactory.CreateSystem();
        }

        public ISystemUnitOfWork CreateSystemUnitOfWork(IUnitOfWork unitOfWork)
        {
            return _serviceFacade.UnitOfWorkFactory.CreateSystem(unitOfWork);
        }

        public ITransactionUnitOfWork CreateTransactionUnitOfWork()
        {
            return _serviceFacade.UnitOfWorkFactory.CreateTransaction();
        }

        public ISystemTransactionUnitOfWork CreateSystemTransactionUnitOfWork()
        {
            return _serviceFacade.UnitOfWorkFactory.CreateSystemTransaction();
        }
    }
}
