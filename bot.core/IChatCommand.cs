using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;

namespace bot.core
{
    public interface IChatCommand
    {
        IEnumerable<string> Command { get; }

        string Description { get; }

        TimeSpan? Cooldown { get; }

        bool CanBeListed() => true;

        void Execute(ITwitchClient client, ChatMessage message, ReadOnlyMemory<char> parsedText);
     
    }
}
