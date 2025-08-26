using DesafioTecnicoAvanade.EstoqueApi.RabbitMQ;

public class RabbitMQHostedService : IHostedService
{
    private readonly IQueueConsumer _consumer;

    public RabbitMQHostedService(IQueueConsumer consumer)
    {
        _consumer = consumer;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _consumer.StartConsumer();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}