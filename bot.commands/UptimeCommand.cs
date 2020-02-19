using bot.core;
using System;
using System.Collections.Generic;
using System.Text;
using TwitchLib.Api;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;

namespace bot.commands
{
    public class UptimeCommand : IChatCommand
    {
        public IEnumerable<string> Command => new[] { "uptime" };
        public string Description => "Displays the uptime of the stream";
        public TimeSpan? Cooldown => TimeSpan.FromSeconds(10);

        private TwitchAPI api;

        public UptimeCommand(TwitchAPI api)
        {
            this.api = api;
        }

        public async void Execute(ITwitchClient client, ChatMessage message, ReadOnlyMemory<char> parsedText)
        {
            var uptime = await api.V5.Streams.GetUptimeAsync(message.RoomId);
            if (uptime == null)
                client.SendMessage(message.Channel, "Currently Offline");
            else
                client.SendMessage(message.Channel, $"Streams been running for {((TimeSpan)uptime).ToString(@"hh\:mm\:ss")}");
        }
    }
}
