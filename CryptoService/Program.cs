using System.Reflection;
using CryptoService.Entities;
using CryptoService.Repositories;
using CryptoService.Services;
using Microsoft.EntityFrameworkCore;

namespace CryptoService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddControllers();
        
        builder.Services.AddScoped<UnitOfWork>();

        // services
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<ICryptocurrencyService, CryptocurrencyService>();
        
        
        // automapper
        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        // dbcontext
        builder.Services.AddDbContext<CryptoContext>(options => options.UseSqlServer("Server=localhost;Database=CryptoService;User=sa;Password=Mikulas0;TrustServerCertificate=true;"));
        
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        
        app.MapControllers();
        app.Run();
    }
}