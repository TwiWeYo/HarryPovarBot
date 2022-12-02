using System;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HarryPovarBot.Sequences
{
    public class SequenceStep<T>
    {
        public SequenceStep(Predicate<T> condition, Action<ITelegramBotClient, Update, CancellationToken> step)
        {
            Condition = condition;
            Step = step;
        }

        public Predicate<T> Condition { get; }
        public Action<ITelegramBotClient, Update, CancellationToken> Step { get; }
    }
}