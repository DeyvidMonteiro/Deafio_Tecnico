using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DesafioTecnicoAvanade.Identity.DataAccess
{
    public class AppDbContext(DbContextOptions options) : IdentityDbContext(options)
    {
    }
}
