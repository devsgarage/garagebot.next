using bot.core;
using System;
using System.Collections.Generic;
using System.Text;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;

namespace bot.commands
{
    public class AlertCommand : IChatCommand
    {
        public IEnumerable<string> Command => new[] { "alert" };
        public string Description => "Plays a sound to alert the broadcaster to something going on in chat";
        public TimeSpan? Cooldown => TimeSpan.FromSeconds(10);
        private IGarageHubService garageHubService;
        public AlertCommand(IGarageHubService garageHubService)
        {
            this.garageHubService = garageHubService;
            garageHubService.Connect();
        }
        public void Execute(ITwitchClient client, ChatMessage message, ReadOnlyMemory<char> parsedText)
        {
            Console.WriteLine("Alert received");
            garageHubService.SendAlert(message.Username);
        }
    }
}
