using Domain.Messaging;
using Infra.Configuration;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infra.Messaging
{
    public class FraseProducer : IFraseProducer
    {
        private readonly IModel _channel;
        private readonly IConnection _connection;
        private readonly RabbitMQSettings _settings;
        public FraseProducer(RabbitMQSettings rabbitMQSettings)
        {
            _settings = rabbitMQSettings;
            var factory = new ConnectionFactory
            {
                HostName = _settings.HostName,
                UserName = _settings.UserName,
                Password = _settings.Password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: _settings.QueueName,
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);
            }
        public void Publicar(string mensagem)
        {
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(mensagem));

            _channel.BasicPublish(exchange: "",
                                  routingKey: _settings.QueueName,
                                  basicProperties: null,
                                  body: body);
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
