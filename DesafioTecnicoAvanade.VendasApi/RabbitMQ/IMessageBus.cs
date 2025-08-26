namespace DesafioTecnicoAvanade.VendasApi.RabbitMQ
{
    public interface IMessageBus
    {
        Task PublishAsync(BaseMessage message);
    }
}