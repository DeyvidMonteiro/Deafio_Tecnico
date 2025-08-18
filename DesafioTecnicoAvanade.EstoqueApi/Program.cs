using AutoMapper;
using DesafioTecnicoAvanade.EstoqueApi.DataAccess.Context;
using DesafioTecnicoAvanade.EstoqueApi.DataAccess.InterfacesRepositories;
using DesafioTecnicoAvanade.EstoqueApi.DataAccess.Repositories;
using DesafioTecnicoAvanade.EstoqueApi.DataAccess.UnitOfWork;
using DesafioTecnicoAvanade.EstoqueApi.Services.Category;
using DesafioTecnicoAvanade.EstoqueApi.Services.Product;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(opt => opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
    
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAutoMapper(cfg => { }, AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<ICategoryWriteOlyRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryRideOnlyRepository, CategoryRepository>();
builder.Services.AddScoped<IProductReadOnlyRepository, ProductRepository>();
builder.Services.AddScoped<IProductWriteOnlyRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryServices, CategoryServices>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
