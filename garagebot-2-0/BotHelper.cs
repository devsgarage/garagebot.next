using bot.core;
using bot.logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TwitchLib.Api;

namespace garagebot_2_0
{
    public static class BotHelper
    {
        public static void AddCommands(this IServiceCollection collection)
        {
            var type = typeof(IChatCommand);
            List<Type> types = new List<Type>();
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var di = new DirectoryInfo(path);
            var dlls = di.GetFiles("*.dll");
            foreach (var file in dlls)
            {
                try
                {
                    var nextAssembly = Assembly.LoadFrom(file.FullName);
                    types.AddRange(nextAssembly.GetTypes()
                        .Where(z => type.IsAssignableFrom(z) && !z.IsInterface && !z.IsAbstract)
                        .ToList());
                }
                catch (BadImageFormatException)
                {
                    // Not a .net assembly  - ignore
                }
            }

            foreach (var t in types)
            {
                collection.AddSingleton(type, t);
            }
        }

        public static void AddTwitchAPI(this IServiceCollection collection, TwitchSettings twitchSettings)
        {
            var api = new TwitchAPI();
            api.Settings.AccessToken = twitchSettings.OauthToken;
            api.Settings.ClientId = twitchSettings.ClientId;

            collection.AddSingleton(api);
        }

        public static void AddLoggingServices(this IServiceCollection collection)
        {
            collection.AddSingleton<ILoggingService, DummyLoggingService>();
        }
    }
}
