using HarryPovarBot.Repository;
using LiteDB;
using System;
using System.Collections.Generic;
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
        private readonly ITelegramBotClient botClient;
        private readonly IRepository<BsonValue, Sticker> repository;
        private StickerQuery? pendingSticker;
        private long? editId;
        public StickerBot(IRepository<BsonValue, Sticker> repository)
        {
            if (repository is null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            this.repository = repository;
            botClient = new TelegramBotClient("");
        }
        public async Task Run()
        {
            using var cts = new CancellationTokenSource();

            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new[] { UpdateType.Message } // receive all update types
            };

            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );

            var me = await botClient.GetMeAsync();

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();

            // Send cancellation request to stop bot
            cts.Cancel();
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message)
                return;

            var chatId = message.Chat.Id;
            if (pendingSticker is not null)
            {
                if (string.IsNullOrEmpty(pendingSticker.StickerId))
                {
                    if (message.Sticker is { } messageSticker)
                    {
                        pendingSticker.StickerId = messageSticker.FileId;
                        await botClient.SendTextMessageAsync(chatId, "Пришлите ключевые слова для отправки стикера. Для завершения введите /finishSticker", cancellationToken: cancellationToken);
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(chatId, "Пришлите стикер", cancellationToken: cancellationToken);
                    }
                }
                else if (message.Text is { } messageText)
                {
                    if (messageText.Split()[0] == "/finishSticker")
                    {
                        repository.Upsert(pendingSticker);
                        pendingSticker = null;
                    }
                    else
                    {
                        pendingSticker.ReferenceStrings.Add(messageText);
                    }
                }
            }
            else if(message.Text is { } messageText1)
            {
                if (messageText1.Split()[0] == "/addSticker")
                {
                    if (pendingSticker is null)
                    {
                        pendingSticker = new StickerQuery();
                        editId = chatId;
                        await botClient.SendTextMessageAsync(chatId, "Пришлите стикер", cancellationToken: cancellationToken);
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(chatId, "Укажите параметры для стикера\nЧтобы завершить редактирование, введите /finishSticker",
                            cancellationToken: cancellationToken);
                    }
                }
                else
                {
                    var meme = repository.FindSticker(messageText1);
                    if (meme?.StickerId is not null)
                    {
                        await botClient.SendStickerAsync(chatId, meme.StickerId, cancellationToken: cancellationToken);
                    }
                }
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

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}
