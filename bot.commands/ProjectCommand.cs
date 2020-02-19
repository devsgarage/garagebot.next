using bot.core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;

namespace bot.commands
{
    public class ProjectCommand : IChatCommand
    {
        public IEnumerable<string> Command => new[] { "project" };

        public string Description => "The current project being worked on";

        public TimeSpan? Cooldown => TimeSpan.FromSeconds(10);

        public bool CanBeListed() => true;

        private string currentProject = "";

        public void Execute(ITwitchClient client, ChatMessage message, ReadOnlyMemory<char> parsedText) 
        {
            if (message.IsBroadcaster && !parsedText.IsEmpty)
                currentProject = parsedText.ToString();
            if (string.IsNullOrWhiteSpace(currentProject))
                client.SendMessage("developersgarage", "Hey @developersgarage, what are we working on today?");
            else
                client.SendMessage("developersgarage", currentProject);
        }
    }
}
