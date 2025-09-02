namespace DesafioTecnicoAvanade.VendasApi.Services.External.Contracts
{
    public interface ITokenService
    {
        Task<string> GetAccessToken();
    }
}
