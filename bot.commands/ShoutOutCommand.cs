using bot.core;
using System;
using System.Collections.Generic;
using System.Text;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;

namespace bot.commands
{
    public class ShoutOutCommand : IChatCommand
    {
        public IEnumerable<string> Command => new[] { "so" };
        public string Description => "BROADCASTER ONLY --- Gives a shout out to a fellow streamer!";
        public TimeSpan? Cooldown => TimeSpan.FromSeconds(10);
        public bool CanBeListed() => false;

        public void Execute(ITwitchClient client, ChatMessage message, ReadOnlyMemory<char> parsedText)
        {
            client.SendMessage(message.Channel, $"Check out another great streamer @{parsedText} over on https://twitch.tv/{parsedText}");
        }
    }
}
