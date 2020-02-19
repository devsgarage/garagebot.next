using bot.core;
using System;
using System.Collections.Generic;
using System.Text;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;

namespace bot.commands
{
    public class LiveCodersUrlCommand : IChatCommand
    {
        private const string liveCodersUrl = "https://twitch.tv/team/livecoders";
        public IEnumerable<string> Command => new[] { "livecoders" };
        public string Description => "Displays Twitch URL for Live Coders team";
        public TimeSpan? Cooldown => TimeSpan.FromSeconds(10);

        public void Execute(ITwitchClient client, ChatMessage message, ReadOnlyMemory<char> parsedText)
        {
            client.SendMessage(message.Channel, $"Check out all the Live Coders at {liveCodersUrl}");
        }
    }
}
