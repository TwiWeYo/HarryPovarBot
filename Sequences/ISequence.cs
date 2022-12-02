using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HarryPovarBot.Sequences
{
    public interface ISequence<T>
    {
        IEnumerable<SequenceStep<T>> Steps { get; }

        SequenceStep<T>? CurrentStep { get; set; }

        Task Start(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);
        Task Stop(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);
    }
}
