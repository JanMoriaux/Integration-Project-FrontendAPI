using FrontEndAPI.RabbitMQ.Configuration;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndAPI.RabbitMQ.Producer
{
    public class MQProducer : IMQProducer
    {
        private IMQConfiguration _configuration;
        private ConnectionFactory _factory;

        public MQProducer(IMQConfiguration configuration)
        {
            _configuration = configuration;
            _factory = new ConnectionFactory()
            {
                HostName = _configuration.Hostname,
                Port = _configuration.Port,
                UserName = _configuration.Username,
                Password = _configuration.Password,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };
        }


        public async Task SendMessageAsync(string message)
        {
            await Task.Run(() =>
            {
                try
                {
                    using (var connection = _factory.CreateConnection())
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(exchange: _configuration.ExchangeName, type: "fanout");
                        var body = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: _configuration.ExchangeName,
                                                routingKey: "",
                                                basicProperties: null,
                                                body: body);
                    }

                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }
            );
        }
    }
}

