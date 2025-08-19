using DesafioTecnicoAvanade.VendasApi.Models;

namespace DesafioTecnicoAvanade.VendasApi.DTOs
{
    public class CartItemDTO
    {
        public int Id { get; set; } 
        public int Qauntity { get; set; } = 1; 
        public int ProductId { get; set; } 
        public int CartHeaderId { get; set; } 
        public ProductDTO Product { get; set; } = new ProductDTO(); 
        public CartHeaderDTO CartHeader { get; set; } = new CartHeaderDTO(); 
    }
}
