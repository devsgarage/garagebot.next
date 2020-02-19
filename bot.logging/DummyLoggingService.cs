using bot.core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bot.logging
{
    public class DummyLoggingService : ILoggingService
    {
        public Task ChatMessages(string username, string chatMessage, DateTime messageDateTime)
        {
            Console.WriteLine($"Logging {nameof(ChatMessages)} received");
            return Task.CompletedTask;
        }

        public Task CommandUsed(string username, string commandName, DateTime executedDateTime, bool inCooldown)
        {
            Console.WriteLine($"Logging CommandUsed received");
            return Task.CompletedTask;
        }

        public Task IdeaSuggested(string idea, DateTime ideaRecievedDateTime)
        {
            Console.WriteLine($"Logging IdeaSuggested received");
            return Task.CompletedTask;
        }

        public Task LogMessage(string message, SeverityLevel severity)
        {
            Console.WriteLine($"Logging LogMessage received");
            return Task.CompletedTask;
        }

        public Task LogMessage(Exception exception, SeverityLevel severity)
        {
            Console.WriteLine($"Logging LogMessage received");
            return Task.CompletedTask;
        }

        public Task NewFollower(string username, DateTime followReceived)
        {
            Console.WriteLine($"Logging NewFollower received");
            return Task.CompletedTask;
        }

        public Task NewSubscriber(string username, DateTime subscriptionReceivedDateTime)
        {
            Console.WriteLine($"Logging NewSubscriber received");
            return Task.CompletedTask;
        }

        public Task UserJoined(string username, DateTime joinedDateTime)
        {
            Console.WriteLine($"Logging UserJoined received");
            return Task.CompletedTask;
        }

        public Task UserLeft(string username, DateTime leftDateTime)
        {
            Console.WriteLine($"Logging UserLeft received");
            return Task.CompletedTask;
        }
    }
}
