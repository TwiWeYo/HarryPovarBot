using HarryPovarBot.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace HarryPovarBot
{
    class Program
    {
        private static IHost? _host;

        static async Task Main(string[] args)
        {
            _host = new HostBuilder().ConfigureHost().Build();
            var bot = _host.Services.GetRequiredService<StickerBot>();

            await bot.Run();
        }
    }
}
