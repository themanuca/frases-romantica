using Microsoft.AspNetCore.Mvc;
using frases_romantica;
using Microsoft.AspNetCore.Connections;
using System.Text;
using RabbitMQ.Client;

namespace frases_romantica.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RabbitMQMessagesController : Controller
    {

        private readonly ILogger<RabbitMQMessagesController> _logger;

        public RabbitMQMessagesController(ILogger<RabbitMQMessagesController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "ReciveMenssage")]
        public List<MensagemRomantica> Get()
        {
            return Lista.FraseRomantica;
        }
        [HttpPost(Name="SendMenssage")]
        public async Task EnviarMensagemAsync(MensagemRomantica message)
        {
            Lista.FraseRomantica.Add(message);
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "MensagemRomance", durable: false, exclusive: false, autoDelete: false, arguments: null);
            const string mensagem = "Primeiro Teste com RABBITMQ";

            var body = Encoding.UTF8.GetBytes(mensagem);

            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "filaTESTE", body: body);
        }
    }

}
