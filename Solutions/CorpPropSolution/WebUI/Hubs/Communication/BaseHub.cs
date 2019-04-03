using System;
using System.Threading.Tasks;
using Base.DAL;
using Base.Security;
using Base.Service;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using WebUI.Concrete;
using WebUI.Helpers;

namespace WebUI.Hubs
{
    public class BaseHub : Hub
    {
        private readonly ConferenceHubFactory _conferenceHubFactory;

        public BaseHub(ConferenceHubFactory conferenceHubFactory)
        {
            _conferenceHubFactory = conferenceHubFactory;
        }

        internal ConferenceHubFactory HubFactory
        {
            get { return _conferenceHubFactory; }
        }

        internal T GetService<T>(string mnemonic) where T : IService
        {
            var config = _conferenceHubFactory.ViewModelConfigService.Get(mnemonic);

            return config.GetService<T>();
        }

        internal IUnitOfWork CreateSystemUnitOfWork()
        {
            return _conferenceHubFactory.UnitOfWorkFactory.CreateSystem();
        }

        public IUnitOfWork CreateSystemTransactionUnitOfWork()
        {
            return _conferenceHubFactory.UnitOfWorkFactory.CreateSystemTransaction();
        }

        internal IUserStatus MyStatus
        {
            get { return _conferenceHubFactory.UserStatusService.GetUserStatus(Context.ConnectionId); }
        }

        internal string ToStringResult(object obj)
        {
            return new JsonNetResult(obj).ToString();
        }

        internal T ToObjectResult<T>(string json)
        {
            var settings = new JsonSerializerSettings()
            {
                DateFormatString = "dd.MM.yyyy HH:mm:ss",
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                StringEscapeHandling = StringEscapeHandling.EscapeHtml,
            };

            settings.Converters.Add(new CustomDateTimeConvertor());


            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        public virtual async Task ConfirmConnection()
        {
            await Clients.Client(Context.ConnectionId).Confirmed(new { Status = "OK", ServerDate = DateTime.Now });
        }

    }
}