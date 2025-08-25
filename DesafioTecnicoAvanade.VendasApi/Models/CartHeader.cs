using System.Text.Json.Serialization;

namespace DesafioTecnicoAvanade.VendasApi.Models
{
    public class CartHeader
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public virtual ICollection<CartItem> CartItems { get; set; }

    }
}
