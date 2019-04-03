using Base.Enums;
using Base.Identity;
using Base.Identity.Core;
using Base.Service.Log;
using Base.Security;
using Microsoft.Owin;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.Linq;
using CorpProp.WindowsAuth.Helpers;
using Base.Identity.Entities;
using Base.DAL;
using CorpProp.WindowsAuth.Services;
using Owin;
using CorpProp.WindowsAuth.Middlewares;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Infrastructure;
using Base.Security.Service;
using Base.Ambient;

namespace CorpProp.WindowsAuth
{
    public class ADAuthInSystem : AuthenticationMiddleware<ADInternalAuthOptions>
    {
        private readonly IAppContextBootstrapper _appContextBootstrapper;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IAccountManager _accountManager;
        private readonly IADUserService _adUserService;
        private readonly IAccessUserService _accessUserService;

        public ADAuthInSystem(OwinMiddleware next, IAccountManager accountManager, IUnitOfWorkFactory unitOfWorkFactory,
            IADUserService adUserService, IAccessUserService accessUserService, IAppContextBootstrapper appContextBootstrapper,
            IAppBuilder app, ADInternalAuthOptions options) : base(next, options)
        {
            _appContextBootstrapper = appContextBootstrapper;
            _accountManager = accountManager;
            _unitOfWorkFactory = unitOfWorkFactory;
            _adUserService = adUserService;
            _accessUserService = accessUserService;

            #region Cookie storing options
            if (Options.TicketDataFormat == null)
            {
                var dataProtector = app.CreateDataProtector(typeof(ADAuthInSystem).FullName,
                    options.AuthenticationType);

                Options.TicketDataFormat = new TicketDataFormat(dataProtector);
            }

            if (Options.CookieManager == null)
            {
                Options.CookieManager = new ChunkingCookieManager();
            }
            #endregion
        }

        protected override AuthenticationHandler<ADInternalAuthOptions> CreateHandler()
        {
            return new ADInternalAuthHandler(_unitOfWorkFactory, _accountManager, _adUserService, _accessUserService, _appContextBootstrapper);
        }
    }
}
