using HarryPovarBot.Repository;
using LiteDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace HarryPovarBot.Extensions
{
    public static class IHostBuilderExtensions
    {
        public static IHostBuilder ConfigureHost(this IHostBuilder builder)
        {
            return builder
            .ConfigureAppConfiguration((context, builder) =>
            {
                builder.SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false);
            })
            .ConfigureLogging((context, logging) =>
            {
                logging
                    .AddConsole()
                    .AddDebug();
            })
            .ConfigureServices((context, services) =>
            {
                services
                    .Configure<AppSettings>(context.Configuration.GetSection("AppSettings"))

                    .AddSingleton<StickerBot>()
                    .AddSingleton<IRepository<BsonValue, Sticker>>(
                        new LiteDbRepository<Sticker>(context.Configuration
                        .GetSection("AppSettings")
                        !.Get<AppSettings>()
                        !.ConnectionString));

            });
        }
    }
}
