## Projeto Frases Romântica
 O intuito do projeto é desenvolver uma pequena aplicação que gere frases romanticas programada e envie para minha noiva, via whatsapp.
Aplicação usára LLM para gerar as frases, para nunca se repitrem e para serem únicas.
Terá um painel web com dashboard para gerenciar tudo de forma visual.
- Modo Anônimo: Permitir que outras pessoas usem o sistema de para enviar mensagens secretas;
- Poderá gerar as frases por temas e humor;
- API terá um endpoint publico (*/api/frases/aleatoria*);
- Ranking das frases mais curtidas;
  
##STACKS
- RABBITMQ;
- DOCKER
- ASP.NET CORE
- NEXTJS (REACT)
- SQL Server
- ~~Entity Framework~~ Dapper (A escolha se da pelo fato de ter mais controle sobre a query usada)
- Tailwind
## EF ou Dapper
Vendo a documentação para uso do EF e controle das query usada, decide optar pelo Dapper, o uso dos .FromSql() e .FromSQLRaw() do LINQ é interessante mas dificulta o uso de argumentos e paramentros na query.
(https://learn.microsoft.com/pt-br/ef/core/querying/sql-queries?tabs=postgres).
O repostiório do Dapper mostra que o controle sobre a query é mais facil: https://github.com/DapperLib/Dapper
## Banco de Dados - SQL SERVER no Docker
Para criação do banco, vou manter uso de um container que já tenho em meu docker, criando nova base e sua tabela.

Até então, vejo a necessidade apenas da criação de uma tabela, as da frase.

script:

```
CREATE DATABASE FrasesDb;
GO
USE FrasesDb;

CREATE TABLE FrasesRomanticas (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Texto NVARCHAR(500) NOT NULL,
    Tema NVARCHAR(100) NOT NULL,
    Curtidas INT NOT NULL DEFAULT 0,
    CriadoEm DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

```
## Clean Arquiteture e DDD
Pensando nas boas praticas, o projeto acabou sendo um caso de estudo e pratica para uma arquitetura mais limpa e um Dominio forte e solitario.
--------------------
APP: Services e suas Interaces;
CONTRACT: DTOs;
DOMAIN: Entidades/Models, Interfaces do Repositorio, Interfaces do Publisher e Consumer (RABBITMQ);
INFRA: Configuração do Rabbitmq, DBContext (DAPPER OU EF), Implementação do Publisher e Consumer, Implementação do Repositorio;
API: Programs, appsettings e Controllers;
----------------------
O Projeto é pequeno para a implementação do Clean Arqu mas esta servindo como objeto de estudo. 

## RABBITMQ 
Rabbitmq se encontra rodando no docker para facilitar o desenvolvimento. A implementação do Rabbitmq e sua configuração se encontra na camada de INFRA. 
No Dominio foi criado apenas sua interface que implementar o método Publicar. 
Segue o padrão do Marcortatti, a  criação da fila (publisher).
```
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
```
## Tratamento de erro e os uso incorreots das Exception 
Sim, eu sei. Isso será ajustado mais para frente. Estou focando nas implementação das regras e funcionalidades. 
Mas sei que é essa parte é muito importante para as boas praticas.

## ainda 
