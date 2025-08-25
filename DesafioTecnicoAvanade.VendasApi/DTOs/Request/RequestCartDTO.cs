namespace DesafioTecnicoAvanade.VendasApi.DTOs.Request
{

    public class RequestCartDTO
    {
        public List<RequestCartItemDTO> CartItems { get; set; } = new List<RequestCartItemDTO>();
    }

}
