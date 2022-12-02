using System;
using System.Threading.Tasks;

namespace HarryPovarBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var bot = new StickerBot();
            await bot.Run();
        }
    }
}
