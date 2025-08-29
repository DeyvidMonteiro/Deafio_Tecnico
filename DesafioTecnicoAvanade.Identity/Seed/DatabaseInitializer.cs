using DesafioTecnicoAvanade.Identity.Model;
using Microsoft.AspNetCore.Identity;

namespace DesafioTecnicoAvanade.Identity.Services
{
    public static class DatabaseInitializer
    {
        public static async Task SeedRolesAndUsersAsync(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            var adminUser = await userManager.FindByEmailAsync("admin@teste.com");
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "adminteste",
                    Email = "admin@teste.com",
                    Name = "Admin",
                    Surname = "Tester"
                };
                await userManager.CreateAsync(adminUser, "Senha@1010*");
            }

            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}