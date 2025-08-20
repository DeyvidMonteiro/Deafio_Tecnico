using DesafioTecnicoAvanade.IdentityServer.Configuration;
using DesafioTecnicoAvanade.IdentityServer.Data;
using DesafioTecnicoAvanade.IdentityServer.DTOs;
using Duende.IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace VShop.IdentityServer.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class RegisterController : ControllerBase
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;

            public RegisterController(UserManager<ApplicationUser> userManager,
                                     RoleManager<IdentityRole> roleManager)
            {
                _userManager = userManager;
                _roleManager = roleManager;
            }

            [HttpPost("register")]
            public async Task<IActionResult> Register([FromBody] RegisterUserDTO model)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // cria role Client se não existir
                if (!await _roleManager.RoleExistsAsync(IdentityConfiguration.Client))
                {
                    await _roleManager.CreateAsync(new IdentityRole(IdentityConfiguration.Client));
                }

                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                    return BadRequest(result.Errors);

                // adiciona ao perfil Client
                await _userManager.AddToRoleAsync(user, IdentityConfiguration.Client);

                // adiciona claims
                await _userManager.AddClaimsAsync(user, new Claim[]
                {
                new Claim(JwtClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(JwtClaimTypes.GivenName, user.FirstName),
                new Claim(JwtClaimTypes.FamilyName, user.LastName),
                new Claim(JwtClaimTypes.Role, IdentityConfiguration.Client)
                });

                return Ok(new { message = "Usuário registrado com sucesso!" });
            }
        }
    }
