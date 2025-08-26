using DesafioTecnicoAvanade.EstoqueApi.RabbitMQ;
using DesafioTecnicoAvanade.EstoqueApi.Services.Product;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

public class RabbitMQBusConsumer : IQueueConsumer
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly string _hostname = "localhost";
    private readonly int _port = 5672;
    private readonly string _queueName = "vendas_queue";

    public RabbitMQBusConsumer(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public void StartConsumer()
    {
        var factory = new ConnectionFactory()
        {
            HostName = _hostname,
            Port = _port
        };

        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();

        channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            // Crie um novo escopo para cada mensagem
            using (var scope = _scopeFactory.CreateScope())
            {
                var productService = scope.ServiceProvider.GetRequiredService<IProductService>();

                try
                {
                    var orderCreatedMessage = JsonConvert.DeserializeObject<OrderCreatedMessage>(message);

                    Console.WriteLine($" [x] Recebido: Pedido com ID {orderCreatedMessage.OrderId}");

                    await productService.DecrementStock(orderCreatedMessage.ProductId, orderCreatedMessage.Quantity);

                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" [!] Erro ao processar mensagem: {ex.Message}");
                    channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
                }
            }
        };

        channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);
    }
}