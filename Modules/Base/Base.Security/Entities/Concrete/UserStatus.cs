using System;
using Newtonsoft.Json;

namespace Base.Security
{
    public class UserStatus : IUserStatus
    {
        public int UserId { get; set; }
        public string ConnectionId { get; set; }
        public bool Online { get; set; }
        public bool Updated { get; set; }

        public DateTime LastDate { get; set; }

        [JsonIgnore]
        public DateTime? LastPublicDate { get; set; }

        public CustomStatus CustomStatus { get; set; }

        [JsonIgnore]
        public CustomStatus? LastCustomStatus { get; set; }

        [JsonIgnore]
        public CustomStatus? LastPublicCustomStatus { get; set; }

        public IUserStatus GetPublicVersion()
        {
            var pub = new UserStatus()
            {
                UserId = UserId,
                ConnectionId = ConnectionId,
                Updated = Updated,
            };

            if (CustomStatus != CustomStatus.Disconnected)
            {
                pub.Online = Online;
                pub.LastDate = LastDate;
                pub.CustomStatus = CustomStatus;
            }
            else
            {
                pub.Online = false;
                pub.LastDate = LastPublicDate ?? LastDate;
                pub.CustomStatus = LastPublicCustomStatus ?? LastCustomStatus ?? CustomStatus.Ready;
            }

            return pub;
        }
    }
}
