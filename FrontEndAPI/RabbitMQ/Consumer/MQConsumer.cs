using FrontEndAPI.RabbitMQ.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FrontEndAPI.RabbitMQ.Consumer
{
    public class MQConsumer : IMQConsumer
    {
        //private static readonly string _baseURL = "http://localhost:59116/api";
        private static readonly string _baseURL = "http://ec2-52-29-5-250.eu-central-1.compute.amazonaws.com:8080/api";

        private static readonly HttpClient _client = new HttpClient();

        private ConnectionFactory _factory;
        private IMQConfiguration _configuration;
        private IModel _channel;
        private BasicDeliverEventArgs _result;

        
        public MQConsumer(IMQConfiguration configuration)
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

        public async Task ReceiveMessagesAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                try
                {
                    using (var connection = _factory.CreateConnection())
                    using (_channel = connection.CreateModel())
                    {
                        _channel.ExchangeDeclare(_configuration.ExchangeName, ExchangeType.Fanout);
                        _channel.QueueDeclare(queue: _configuration.QueueName, durable: true, exclusive: false, autoDelete: false);
                        _channel.QueueBind(queue: _configuration.QueueName,
                                      exchange: _configuration.ExchangeName,
                                      routingKey: "");
                        var consumer = new EventingBasicConsumer(_channel);
                        consumer.Received += (model, result) =>
                        {
                            var body = result.Body;
                            _result = result;
                            var message = Encoding.UTF8.GetString(body);
                            TransitMessage(message);
                        };
                        _channel.BasicConsume(queue: _configuration.QueueName,
                                         autoAck: false,
                                         consumer: consumer);
                        while (!cancellationToken.IsCancellationRequested) ;
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }

            });
        }

        public void TransitMessage(string message)
        {
            object body = new { Message = message };
            var jsonBody = JsonConvert.SerializeObject(body);
            var postTask = _client.PostAsync(String.Format("{0}/message", _baseURL), new StringContent(jsonBody, Encoding.UTF8, "application/json"));
            postTask.ContinueWith(task => _channel.BasicAck(_result.DeliveryTag, false),TaskContinuationOptions.OnlyOnRanToCompletion);
        }
    }
}
