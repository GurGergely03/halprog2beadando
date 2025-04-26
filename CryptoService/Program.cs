using System.Reflection;
using CryptoService.DTOs;
using CryptoService.Entities;
using CryptoService.Repositories;
using CryptoService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

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

        // controllers
        builder.Services.AddControllers();
        
        // unit of work
        builder.Services.AddScoped<UnitOfWork>();

        // services
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IWalletService, WalletService>();
        builder.Services.AddScoped<ICryptocurrencyService, CryptocurrencyService>();
        builder.Services.AddScoped<ITransactionHistoryService, TransactionHistoryService>();
        
        // automapper
        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        // dbcontext
        builder.Services.AddDbContext<CryptoContext>(options => options.UseSqlServer("Server=localhost;Database=CryptoService;User=sa;Password=Mikulas0;TrustServerCertificate=true;"));
        
        // background service
        builder.Services.AddHostedService<CryptocurrencyPriceChangeBackgroundService>();
        
        // logging
        builder.Logging.AddConsole();
        
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