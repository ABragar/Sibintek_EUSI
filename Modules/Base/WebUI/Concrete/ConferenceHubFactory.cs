using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Base.Conference.Service;
using Base.DAL;
using Base.Security;
using Base.Security.Service;
using Base.Security.Service.Abstract;
using Base.Service;
using Base.UI;

namespace WebUI.Concrete
{
    public class ConferenceHubFactory
    {
        private readonly Lazy<IUserStatusService> _userStatusService;
        private readonly Lazy<IViewModelConfigService> _viewModelConfigService;
        private readonly Lazy<ISecurityUserService> _securityUserService;
        private readonly Lazy<IUserService<User>> _userService;
        private readonly Lazy<IUnitOfWorkFactory> _unitOfWorkFactory;

        private readonly Lazy<IConferenceService> _conferenceService;
        private readonly Lazy<IPrivateMessageService> _privateMessageService;
        private readonly Lazy<IPublicMessageService> _publicMessageService;

        private readonly Lazy<VideoChannelService> _videoChannelService;
        private readonly Lazy<ChatService> _chatService;

        public ConferenceHubFactory(IServiceLocator locator)
        {
            if (locator == null)
                throw new ArgumentNullException(nameof(locator));

            _locator = locator;

            _userService = CreateLazy<IUserService<User>>();
            _userStatusService = CreateLazy<IUserStatusService>();
            _viewModelConfigService = CreateLazy<IViewModelConfigService>();
            _securityUserService = CreateLazy<ISecurityUserService>();
            _unitOfWorkFactory = CreateLazy<IUnitOfWorkFactory>();
            _conferenceService = CreateLazy<IConferenceService>();
            _privateMessageService = CreateLazy<IPrivateMessageService>();
            _publicMessageService = CreateLazy<IPublicMessageService>();
            _videoChannelService = CreateLazy<VideoChannelService>();
            _chatService = CreateLazy<ChatService>();
        }

        private readonly IServiceLocator _locator;
        public Lazy<T> CreateLazy<T>() where T : class
        {
            return new Lazy<T>(_locator.GetService<T>);
        }

        public IUserStatusService UserStatusService => _userStatusService.Value;

        public IViewModelConfigService ViewModelConfigService => _viewModelConfigService.Value;

        public ISecurityUserService SecurityUserService => _securityUserService.Value;
        public IUserService<User> UserService => _userService.Value;

        public IUnitOfWorkFactory UnitOfWorkFactory => _unitOfWorkFactory.Value;

        public IConferenceService ConferenceService => _conferenceService.Value;

        public IPrivateMessageService PrivateMessageService => _privateMessageService.Value;

        public IPublicMessageService PublicMessageService => _publicMessageService.Value;

        public VideoChannelService VideoChannelService => _videoChannelService.Value;

        public ChatService ChatService => _chatService.Value;
    }

}