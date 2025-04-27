using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CryptoService;
using CryptoService.DTOs;
using CryptoService.Services;
using Microsoft.EntityFrameworkCore;

public class CryptocurrencyPriceChangeBackgroundService : BackgroundService
{
    private readonly ILogger<CryptocurrencyPriceChangeBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(60);
    private readonly TimeSpan _errorRetryDelay = TimeSpan.FromSeconds(30);

    public CryptocurrencyPriceChangeBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<CryptocurrencyPriceChangeBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Cryptocurrency Price Change Service is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var cryptoService = scope.ServiceProvider.GetRequiredService<ICryptocurrencyService>();
                    var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
                    var context = scope.ServiceProvider.GetRequiredService<CryptoContext>();

                    await UpdateCryptoPrices(cryptoService, mapper, context, stoppingToken);
                }

                await Task.Delay(_interval, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating cryptocurrency prices");
                await Task.Delay(_errorRetryDelay, stoppingToken);
            }
        }

        _logger.LogInformation("Cryptocurrency Price Change Service is stopping.");
    }

    private async Task UpdateCryptoPrices(
        ICryptocurrencyService cryptoService,
        IMapper mapper,
        CryptoContext context,
        CancellationToken stoppingToken)
    {
        _logger.LogInformation("Updating cryptocurrency prices...");

        var cryptos = await context.Cryptocurrencies.ToListAsync(cancellationToken: stoppingToken);
        var priceRandomizer = new PriceRandomizer(volatilityFactor: 0.15m);
        foreach (var crypto in cryptos)
        {
            var priceChange = mapper.Map<CryptocurrencyUpdateDTO>(crypto);

            priceChange.CurrentPrice = priceRandomizer.GetNextPrice(priceChange.CurrentPrice);
            
            await cryptoService.UpdateCryptoAsync(crypto.Id, priceChange);
        }

        await context.SaveChangesAsync(stoppingToken);
    }
}

public class PriceRandomizer
{
    private readonly Random _random = new();
    private readonly decimal _volatilityFactor; // 0.1 = ±10%, 0.5 = ±50%

    public PriceRandomizer(decimal volatilityFactor = 0.1m)
    {
        _volatilityFactor = Math.Clamp(volatilityFactor, 0.01m, 1m);
    }

    public decimal GetNextPrice(decimal currentPrice)
    {
        // Gaussian-like distribution (more realistic than pure Random)
        decimal rand = 1m + _volatilityFactor * (2m * (decimal)_random.NextDouble() - 1m);
        
        // Clamp to prevent extreme values
        decimal multiplier = Math.Clamp(rand, 1m - _volatilityFactor, 1m + _volatilityFactor);
        
        return currentPrice * multiplier;
    }
}