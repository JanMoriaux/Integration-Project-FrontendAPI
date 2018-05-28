using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FrontEndAPI.RabbitMQ.Consumer
{
    public class ConsumerService: HostedService
    {
        private IMQConsumer _consumer;

        public ConsumerService(IMQConsumer consumer)
        {
            _consumer = consumer;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await _consumer.ReceiveMessagesAsync(cancellationToken);
        }
    }
}
