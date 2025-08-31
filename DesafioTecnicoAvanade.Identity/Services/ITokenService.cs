using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DesafioTecnicoAvanade.Identity.Services
{
    public interface ITokenService
    {
        JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration _config);

        string GenerateRefreshToken();

        ClaimsPrincipal GetPrincipalFromExpiredToken(string Token, IConfiguration _config);
    }
}
