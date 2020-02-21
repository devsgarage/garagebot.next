using bot.core;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace garagebot_2_0
{
    public class GarageHubService : IGarageHubService
    {
        HubConnection hubConnection;

        public GarageHubService()
        {
            hubConnection = new HubConnectionBuilder().WithUrl("https://localhost:44372/garageHub").Build(); 
            hubConnection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await hubConnection.StartAsync();
            };
        }

        public async Task Connect()
        {
            await hubConnection.StartAsync();
            Console.WriteLine("Connected to hub");
        }

        public async Task SendAlert(string user)
        {
            if (hubConnection.State == HubConnectionState.Disconnected)
            {
                throw new Exception("Connection must be started before sending an alert. Use the Connect method to connect to the Hub");
            }

            await hubConnection.SendAsync("AlertBroadcaster", user);
            //await hubConnection.SendCoreAsync("AlertReceived", new[] { user });
        }
    }
}
