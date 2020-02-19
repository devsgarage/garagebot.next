using bot.core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TwitchLib.Api;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

namespace garagebot_2_0
{
    public class Bot : IHostedService
    {
        ITwitchClient client;
        IServiceProvider serviceProvider;
        ILoggingService loggingService;
        public List<IChatCommand> commands = new List<IChatCommand>();
        private ConcurrentDictionary<string, DateTime> commandLastExecution = new ConcurrentDictionary<string, DateTime>();

        public Bot(TwitchSettings twitchSettings, IServiceProvider serviceProvider, ILoggingService loggingService)
        {
            this.serviceProvider = serviceProvider;
            this.loggingService = loggingService;
            LoadCommands();
            ConnectionCredentials credentials = new ConnectionCredentials("developersgarage_bot", twitchSettings.OauthToken);
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            WebSocketClient customClient = new WebSocketClient(clientOptions);
            client = new TwitchClient(customClient);
            client.Initialize(credentials, "developersgarage");

            client.OnLog += Client_OnLog;
            client.OnJoinedChannel += Client_OnJoinedChannel;
            client.OnMessageReceived += Client_OnMessageReceived;
            //client.OnWhisperReceived += Client_OnWhisperReceived;
            //client.OnNewSubscriber += Client_OnNewSubscriber;
            client.OnConnected += Client_OnConnected;
            client.OnUserJoined += Client_OnUserJoined;

            client.Connect();
        }

        private void Client_OnUserJoined(object sender, OnUserJoinedArgs e)
        {
            loggingService.UserJoined(e.Username, DateTime.Now);
            commands.Where(x => x.Command.Contains("userjoined")).ToList()
                .ForEach(y => y.Execute(client, new ChatMessage(null, null, e.Username, null, null, Color.Empty,
                    null, null, UserType.Broadcaster, e.Channel, null, true, 0, null, true, true, true, true, Noisy.NotSet,
                    null, null, null, null, 0, 0), e.Username.ToCharArray()));
        }

        private void LoadCommands()
        {
            var chatCommands = serviceProvider.GetServices<IChatCommand>().ToArray();
            foreach (var chatCommand in chatCommands)
            {
                commands.Add(chatCommand);
                //logger.LogInformation($"Loaded command: {chatCommand.Command}");
            }
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            ProcessMessage(e.ChatMessage);
        }

        private void ProcessMessage(ChatMessage chatMessage)
        {
            if (chatMessage.Message.FirstOrDefault() != '!')
                return;

            var command = ParseCommand(chatMessage.Message);

            var commandsToExecute =
                commands.Where(c => c.Command.Any(cmd => command.command.Span.Equals(cmd.AsSpan(), StringComparison.OrdinalIgnoreCase) &&
                                                         !CommandInCooldown(cmd, c.Cooldown)));

            foreach (var commandToExecute in commandsToExecute)
            {
                try
                {                    
                    commandToExecute.Execute(client, chatMessage, command.parameter);
                    commandLastExecution[commandToExecute.Command.First()] = DateTime.UtcNow;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"command {commandToExecute.GetType().ToString()} threw an exception: {ex.Message}");
                }
            }

            (ReadOnlyMemory<char> command, ReadOnlyMemory<char> parameter) ParseCommand(string message)
            {
                var command = message.AsMemory(1);
                var commandParameter = ReadOnlyMemory<char>.Empty;
                var splitLocation = command.Span.IndexOf(' ');
                if (splitLocation != -1)
                {
                    commandParameter = command.Slice(splitLocation + 1);
                    command = command.Slice(0, splitLocation);
                }
                return (command, commandParameter);
            };
        }

        private bool CommandInCooldown(string command, TimeSpan? cooldown = null)
        {
            DateTime lastExecuted;
            var gotLastExecution = commandLastExecution.TryGetValue(command, out lastExecuted);
            if (gotLastExecution && (lastExecuted + cooldown) > DateTime.UtcNow)
            {
                Console.WriteLine($"Ignoring {command} command because it's still in cool down");
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Client_OnLog(object sender, OnLogArgs e)
        {
            Console.WriteLine($"{e.DateTime.ToString()}: {e.BotUsername} - {e.Data}");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            
        }        

        private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            client.SendMessage(e.Channel, "GarageBot in the house!");
        }
    }
}
