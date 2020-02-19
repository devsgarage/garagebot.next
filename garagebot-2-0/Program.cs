using bot.core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace garagebot_2_0
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new HostBuilder()
                .ConfigureAppConfiguration((context, config)=>
                {
                    config.AddJsonFile("appsettings.json", optional: true);
                    config.AddUserSecrets<Program>();
                    config.AddEnvironmentVariables();
                })
                .ConfigureServices((context, services) =>
                {                    
                    var twitchSettings = context.Configuration.GetSection("twitch").Get<TwitchSettings>();
                    services.AddSingleton(twitchSettings);

                    services.AddSingleton<IGarageHubService>((provider) => new GarageHubService());

                    services.AddCommands();
                    services.AddLoggingServices();
                    services.AddTwitchAPI(twitchSettings);
                    services.AddSingleton<IHostedService, Bot>();
                })
                .RunConsoleAsync();
        }
    }
}
