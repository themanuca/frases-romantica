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
        private  IChannel _channel;
        private  IConnection _connection;
        private readonly RabbitMQSettings _settings;
        public FraseProducer(RabbitMQSettings rabbitMQSettings)
        {
            _settings = rabbitMQSettings;
            InitializeRabbitMQ();
        }
        public async Task Publicar(string mensagem)
        {
            try
            {
                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(mensagem));

                await _channel.BasicPublishAsync(exchange: "",
                                      routingKey: _settings.QueueName,
                                      body: body);
            }catch(Exception ex)
            {
                throw new ArgumentException("Falha ao publicar mensagem no RabbitMQ", ex);
            }
          
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }

        public async Task InitializeRabbitMQ()
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.HostName,
                UserName = _settings.UserName,
                Password = _settings.Password
            };

            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync(); 

           await _channel.QueueDeclareAsync(queue: _settings.QueueName,
                                  durable: true,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);
        }
    }
}
