namespace DesafioTecnicoAvanade.EstoqueApi.RabbitMQ
{
    public class OrderCreatedMessage
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public long Quantity { get; set; }
    }
}
