using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace WebUI.Hubs
{
    [HubName("notificationHub")]
    public class NotificationHub : Hub
    {
        public bool Register(string userName)
        {


            return true;
        }
    }



    public interface IVotingClient
    {
        void SendVote(int countUp, int countDown);

    }

    public class VotingHub : Hub<IVotingClient>
    {
        public void Vote(int countUp, int countDown)
        {
            Clients.All.SendVote(countUp, countDown);
        }
    }
}