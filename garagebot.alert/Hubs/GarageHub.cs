using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace garagebot.alert.Hubs
{
    public class GarageHub : Hub
    {
        public event EventHandler OnAlertReceived;
        public async Task AlertBroadcaster(string user)
        {
            OnAlertReceived?.Invoke(this, new EventArgs());
            await Clients.All.SendAsync("AlertReceived", user);
        }
    }
}
