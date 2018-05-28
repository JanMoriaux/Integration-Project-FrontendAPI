using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEndAPI.RabbitMQ.Producer
{
    public interface IMQProducer
    {
        Task SendMessageAsync(string message);
    }
}
