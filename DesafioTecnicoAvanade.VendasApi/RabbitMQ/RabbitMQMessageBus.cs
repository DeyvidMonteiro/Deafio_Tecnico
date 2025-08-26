using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace DesafioTecnicoAvanade.VendasApi.RabbitMQ
{
    public class RabbitMQMessageBus : IMessageBus, IDisposable
    {
        private readonly string _hostname = "localhost";
        private readonly int _port = 5672;
        private IConnection _connection;
        private readonly string _queueName = "vendas_queue";

        public RabbitMQMessageBus()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostname,
                Port = _port
            };

            _connection = factory.CreateConnection();
        }

        public Task PublishAsync(BaseMessage message)
        {
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            using (var channel = _connection.CreateModel())
            {
                channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
            }
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}