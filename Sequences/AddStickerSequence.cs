using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HarryPovarBot.Sequences
{
    public class AddStickerSequence : ISequence<StickerQuery>
    {
        private readonly StickerRepository repository;
        private StickerQuery? pendingSticker;

        public IEnumerable<SequenceStep<StickerQuery>> Steps { get; }

        public SequenceStep<StickerQuery>? CurrentStep { get; set; }

        public AddStickerSequence(StickerRepository repository)
        {
            this.repository = repository;

            Steps = new List<SequenceStep<StickerQuery>>
            {
                new SequenceStep<StickerQuery>(
                    q => q.StickerId == null,
                    (a, b, c) => SelectSticker(a, b, c)),
                new SequenceStep<StickerQuery>(
                    q => q.ReferenceStrings == null,
                    (a, b, c) => SetReferenceStrings(a, b, c))
            };
        }

        private async Task SetReferenceStrings(ITelegramBotClient arg1, Update arg2, CancellationToken arg3)
        {
            throw new NotImplementedException();
        }

        private async Task SelectSticker(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task Start(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            CheckUpdate(update);

            var id = update.Message!.Chat.Id;

            if (pendingSticker is null)
            {
                pendingSticker = new StickerQuery();
                CurrentStep = Steps.First();
                return;
            }

            await botClient.SendTextMessageAsync(id,
                "В данный момент стикер уже обрабатывается. Введите /finishSticker для завершения.",
                cancellationToken: cancellationToken);

        }

        public async Task Stop(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            CheckUpdate(update);

            var id = update.Message!.Chat.Id;

            if (pendingSticker is not null)
            {
                repository.Upsert(pendingSticker);
                return;
            }

            await botClient.SendTextMessageAsync(id,
                "Не выбран стикер. Введите /addSticker для заполнения стикера.",
                cancellationToken: cancellationToken);
        }

        private void CheckUpdate(Update update, MessageType type = MessageType.Text)
        {
            if (update is null)
            {
                throw new ArgumentNullException(nameof(update));
            }

            if (update.Type != UpdateType.Message)
                throw new ArgumentException("Should be used only with type Message");

            if (update.Message!.Type != MessageType.Text)
                throw new ArgumentException("Should be used only with type Text");
        }
    }
}
