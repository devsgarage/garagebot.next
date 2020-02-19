using bot.core;
using System;
using System.Collections.Generic;
using System.Text;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;

namespace bot.commands
{
    public class RepoCommand : IChatCommand
    {
        public IEnumerable<string> Command => new[] { "repo" };
        public string Description => "Gets URL of repository for project";
        public TimeSpan? Cooldown => TimeSpan.FromSeconds(10);

        private string repo = "https://github.com/devsgarage";

        public void Execute(ITwitchClient client, ChatMessage message, ReadOnlyMemory<char> parsedText)
        {
            if (parsedText.IsEmpty)
            {  
                client.SendMessage(message.Channel, repo);
            }
            else if (message.IsBroadcaster)
            {
                repo = parsedText.ToString();
            }
        }
    }
}
