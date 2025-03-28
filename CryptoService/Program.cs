using CryptoService.Entities;
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

        builder.Services.AddDbContext<CryptoContext>(options => options.UseSqlServer("Server=localhost;Database=CryptoService;User=sa;Password=Mikulas0;TrustServerCertificate=true;"));
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        app.MapGet("/weatherforecast", (HttpContext httpContext) =>
            {
                var forecast = Enumerable.Range(1, 5).Select(index =>
                        new WeatherForecast
                        {
                            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                            TemperatureC = Random.Shared.Next(-20, 55),
                            Summary = summaries[Random.Shared.Next(summaries.Length)]
                        })
                    .ToArray();
                return forecast;
            })
            .WithName("GetWeatherForecast");

        app.MapGet("/cryptocurrencies", (CryptoContext context) =>
        {
            return context.Cryptocurrencies.ToArray();
        });
        
        app.MapGet("/wallets", (CryptoContext context) =>
        {
            return context.Wallets.ToArray();
        });
        
        app.MapGet("/users", (CryptoContext context) =>
        {
            return context.Users.ToArray();
        });
        
        app.Run();
    }
}