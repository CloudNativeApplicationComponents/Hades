using Hades.Broker.Abstraction;
using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Hades.Broker.Internal
{
    internal class SubscriptionEntry : IDisposable
    {
        private readonly Channel<IHadesMessage> _channel;
        private readonly SubscriptionEntryPool _pool;
        private readonly HadesSubscription _subscription;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public IHadesSubscription Subscription { get => _subscription; }
        public IHadesConsumer Consumer { get; }

        public SubscriptionEntry(
            HadesTopic topic,
            string groupName,
            IHadesConsumer consumer,
            SubscriptionEntryPool pool)
        {
            _pool = pool
                ?? throw new ArgumentNullException(nameof(pool));
            Consumer = consumer
                ?? throw new ArgumentNullException(nameof(consumer));
            _ = topic
                ?? throw new ArgumentNullException(nameof(topic));
            if (string.IsNullOrWhiteSpace(groupName))
                throw new ArgumentNullException(nameof(groupName));

            _cancellationTokenSource = new CancellationTokenSource();
            _subscription = new HadesSubscription(topic, groupName, this);
            _channel = Channel.CreateUnbounded<IHadesMessage>();
            Task.Run(() => ReadFromChannel(), _cancellationTokenSource.Token);
            pool.Add(this);
        }

        private async Task ReadFromChannel()
        {
            while (await _channel.Reader.WaitToReadAsync(_cancellationTokenSource.Token))
            {
                var envelope = await _channel.Reader.ReadAsync(_cancellationTokenSource.Token);
                await Consumer.ConsumeAsync(_subscription.Topic, envelope, _cancellationTokenSource.Token);
            }
        }

        public async Task ScheduleAsync(IHadesMessage message, CancellationToken cancellationToken)
        {
            await _channel.Writer.WriteAsync(message, cancellationToken);
        }

        public void Dispose()
        {
            _channel.Writer.Complete();
            _cancellationTokenSource.Cancel();
            _pool.Remove(this);
            GC.SuppressFinalize(this);
        }
    }
}
