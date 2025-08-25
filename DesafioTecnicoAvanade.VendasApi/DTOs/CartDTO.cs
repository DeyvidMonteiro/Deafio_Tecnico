using DesafioTecnicoAvanade.VendasApi.Models;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DesafioTecnicoAvanade.VendasApi.DTOs
{
    public class CartDTO
    {
        public CartHeaderDTO CartHeader { get; set; } = new CartHeaderDTO();

        public List<CartItemDTO> CartItems { get; set; } = new List<CartItemDTO>();
    }
}
