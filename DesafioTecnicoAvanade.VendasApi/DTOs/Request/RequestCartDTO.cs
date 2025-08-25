namespace DesafioTecnicoAvanade.VendasApi.DTOs.Request
{

    public class RequestCartDTO
    {
        public string UserId { get; set; }
        public List<RequestCartItemDTO> CartItems { get; set; } = new List<RequestCartItemDTO>();
    }

}
