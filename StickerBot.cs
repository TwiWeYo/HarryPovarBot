using HarryPovarBot.Repository;
using LiteDB;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HarryPovarBot
{
    public class StickerBot
    {
        private readonly ILogger<StickerBot> logger;
        private readonly ITelegramBotClient botClient;
        private readonly IRepository<BsonValue, Sticker> repository;

        public StickerBot(IRepository<BsonValue, Sticker> repository, ILogger<StickerBot> logger)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            botClient = new TelegramBotClient("");
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task Run()
        {
            using var cts = new CancellationTokenSource();

            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new[] { UpdateType.Message }
            };

            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );

            var me = await botClient.GetMeAsync();

            logger.LogInformation($"Start listening for @{me.Username}");
            Console.ReadLine();

            // Send cancellation request to stop bot
            cts.Cancel();
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message)
                return;
            if (message.Text is not { } messageText)
                return;

            var chatId = message.Chat.Id;

            var result = repository.Get(
                x => messageText.Contains(x.ReferenceString!, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault();

            if (result?.Id is not null)
            {
                await botClient.SendStickerAsync(chatId, result.Id, cancellationToken: cancellationToken);
            }
        }

        Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            logger.LogError(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}
