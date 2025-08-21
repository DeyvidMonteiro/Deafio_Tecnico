namespace DesafioTecnicoAvanade.VendasApi.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; } // FK
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public Order Order { get; set; } = null!;
    }
}
