using bot.core;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;
using System.Linq;

namespace bot.commands
{
    public class ListCommand : IChatCommand
    {
        public IEnumerable<string> Command => new[] { "list", "help" };
        public string Description => "Displays a list of available commands (ex. !list), or description of a specific command (ex. !list project)";
        public TimeSpan? Cooldown => TimeSpan.FromSeconds(10);

        private Dictionary<string,string> allCommands = new Dictionary<string,string>();

        IServiceProvider service;
        
        public ListCommand(IServiceProvider service)
        {
            this.service = service;            
        }

        private void PopulateAllCommandsDictionary(IServiceProvider service)
        {
            service.GetServices<IChatCommand>().Where(x=>x.CanBeListed()).ToList()
                .ForEach(x => x.Command.ToList()
                    .ForEach(y => allCommands.Add(y, x.Description))
                );
        }

        public void Execute(ITwitchClient client, ChatMessage message, ReadOnlyMemory<char> parsedText)
        {
            if (allCommands.Count == 0) PopulateAllCommandsDictionary(service);
            if (parsedText.IsEmpty)
            {
                var keys = allCommands.Keys;
                var output = string.Join(",", keys);
                client.SendMessage(message.Channel, $"Available commands: {output}");
            }
            else
            {
                var parsedTextAsString = parsedText.ToString();
                if (allCommands.ContainsKey(parsedTextAsString))
                    client.SendMessage(message.Channel, $"{parsedText}: {allCommands[parsedTextAsString]}");
                else
                    client.SendMessage(message.Channel, $"{parsedText} command does not exist in bot");
            }
        }
    }
}
