using HarryPovarBot.Repository;
using LiteDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarryPovarBot.Extensions
{
    public static class IHostBuilderExtensions
    {
        public static IHostBuilder ConfigureHost(this IHostBuilder builder)
        {
            return builder
            .ConfigureAppConfiguration(builder =>
            {
                builder.AddJsonFile("appsettings.json");
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
                    .Configure<AppSettings>(context.Configuration)

                    .AddSingleton<StickerBot>()
                    .AddSingleton<IRepository<BsonValue, Sticker>>(
                        new LiteDbRepository<Sticker>(context.Configuration.GetSection("ConnectionString").Value!));

            });
        }
    }
}
