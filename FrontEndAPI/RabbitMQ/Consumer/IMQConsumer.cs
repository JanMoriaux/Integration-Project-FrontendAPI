using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FrontEndAPI.RabbitMQ.Consumer
{
    public interface IMQConsumer
    {
        Task ReceiveMessagesAsync(CancellationToken cancellationToken);
    }
}
