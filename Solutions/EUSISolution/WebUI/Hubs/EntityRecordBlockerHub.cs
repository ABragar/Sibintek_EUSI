using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Models;

namespace WebUI.Hubs
{
    [HubName("entityRecordBlockerHub")]
    public class EntityRecordBlockerHub : Hub
    {
        private static List<BlockerEntityModel> BlockedRecords = new List<BlockerEntityModel>();

        public void CheckBlocking(int entityId, string entityMnemonic)
        {
            bool isRecordBlocked =
                BlockedRecords.Any(b => b.BlockedEntityId == entityId && b.BlockedEntityMnemonic == entityMnemonic);

            if (isRecordBlocked)
            {
                Clients.Caller.blockRecord(true);
            }
            else
            {
                BlockedRecords.Add(new BlockerEntityModel
                {
                    BlockedEntityId = entityId,
                    BlockedEntityMnemonic = entityMnemonic,
                    ClientConnectionId = Context.ConnectionId
                });
                Clients.Caller.blockRecord(false);
            }
        }

        public void UnblockRecord()
        {
            var blockedItem =
                BlockedRecords.FirstOrDefault(x => x.ClientConnectionId == Context.ConnectionId);

            if (blockedItem != null)
            {
                BlockedRecords.Remove(blockedItem);
            }
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var blockedItem = 
                BlockedRecords.FirstOrDefault(x => x.ClientConnectionId == Context.ConnectionId);

            if (blockedItem != null)
            {
                BlockedRecords.Remove(blockedItem);
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}