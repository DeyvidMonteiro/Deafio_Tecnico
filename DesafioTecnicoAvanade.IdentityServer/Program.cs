using DesafioTecnicoAvanade.IdentityServer.Configuration;
using DesafioTecnicoAvanade.IdentityServer.Data;
using DesafioTecnicoAvanade.IdentityServer.SeedDatabase;
using DesafioTecnicoAvanade.IdentityServer.Services;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Minha API", Version = "v1" });

    // Configuração de segurança (OAuth2 + Resource Owner Password)
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            Password = new OpenApiOAuthFlow
            {
                TokenUrl = new Uri("https://localhost:7270/connect/token"), // URL do seu IdentityServer
                Scopes = new Dictionary<string, string>
                {
                    { "read", "Leitura de dados" },
                    { "write", "Escrita de dados" },
                    { "profile", "Perfil do usuário" },
                    { "openid", "OpenID padrão" },
                    { "email", "E-mail do usuário" }
                }
            }
        }
    });

    // Aplica segurança globalmente
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                }
            },
            new[] { "read", "write", "profile", "openid", "email" }
        }
    });
});


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

var builderIdentityServer = builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;
    options.EmitStaticAudienceClaim = true;
}).AddInMemoryIdentityResources(
                       IdentityConfiguration.IdentityResources)
                       .AddInMemoryApiScopes(IdentityConfiguration.ApiScopes)
                       .AddInMemoryClients(IdentityConfiguration.Clients)
                       .AddAspNetIdentity<ApplicationUser>();

builderIdentityServer.AddDeveloperSigningCredential();

builder.Services.AddScoped<IProfileService, ProfileAppService>();
builder.Services.AddScoped<IDatabaseSeedInitializer, DatabaseIdentityServerInitializer>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // mostra erros detalhados
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha API V1");
        c.RoutePrefix = string.Empty; // abre no root, ex: https://localhost:5001
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("CorsPolicy");

app.UseRouting();

app.UseIdentityServer();

app.UseAuthorization();

SeedDatabaseIdentityServer(app);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

void SeedDatabaseIdentityServer(IApplicationBuilder app)
{
    using (var serviceScope = app.ApplicationServices.CreateScope())
    {
        var initRolesScopes = serviceScope.ServiceProvider.GetService<IDatabaseSeedInitializer>();

        initRolesScopes.InitializerSeedRoles();
        initRolesScopes.InitializerSeedUser();
    }

}