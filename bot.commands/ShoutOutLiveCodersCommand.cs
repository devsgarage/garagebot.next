using bot.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Api;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;

namespace bot.commands
{
    public class ShoutOutLiveCodersCommand : IChatCommand
    {
        public IEnumerable<string> Command => new[] { "userjoined" };
        public string Description => "BROADCASTER ONLY -> Automatic shout out to Live Coders teammates";
        public TimeSpan? Cooldown => TimeSpan.FromMilliseconds(1);
        public bool CanBeListed() => false;

        private TwitchAPI api;
        private Task<List<string>> teammates;

        public ShoutOutLiveCodersCommand(TwitchAPI api)
        {
            this.api = api;
            this.teammates = LoadTeammates();
        }
        private async Task<List<string>> LoadTeammates()
        {
            var teammates = await api.V5.Teams.GetTeamAsync("livecoders");
            return teammates?.Users?.Where(x => !x.Name.Equals("developersgarage")).Select(x => x.Name).ToList();
        }

        public async void Execute(ITwitchClient client, ChatMessage message, ReadOnlyMemory<char> parsedText)
        {
            if (!message.IsBroadcaster)
                return;

            var username = parsedText.ToString();
            var mates = await teammates;
            var isPartOfTeam = mates?.Exists(x => x == username) ?? false;
            if (isPartOfTeam)
                client.SendMessage(message.Channel, $"Check out another member of the Live Coders, @{username}, over on https://twitch.tv/{username}");
        }
    }
}
