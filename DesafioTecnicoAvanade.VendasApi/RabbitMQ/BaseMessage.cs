namespace DesafioTecnicoAvanade.VendasApi.RabbitMQ
{
    public abstract class BaseMessage
    {
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}