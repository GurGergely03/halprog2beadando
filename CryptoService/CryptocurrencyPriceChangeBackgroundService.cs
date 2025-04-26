using AutoMapper;
using CryptoService.DTOs;
using CryptoService.Repositories;

namespace CryptoService;

public class CryptocurrencyPriceChangeBackgroundService : BackgroundService
{
    private readonly ILogger<CryptocurrencyPriceChangeBackgroundService> _logger;
    private IMapper _mapper;
    private readonly UnitOfWork _unitOfWork;

    public CryptocurrencyPriceChangeBackgroundService(ILogger<CryptocurrencyPriceChangeBackgroundService> logger, UnitOfWork unitOfWork, IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Background Service is starting.");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await DoWork(stoppingToken);
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
            catch (OperationCanceledException) { break; }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in background service");
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
        _logger.LogInformation("Timed Background Service is stopping.");
    }

    private async Task DoWork(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Background work is being performed.");

        var cryptos = await _unitOfWork.CryptocurrencyRepository.GetAllAsync();
        foreach (var crypto in cryptos)
        {
            var priceChange = _mapper.Map<CryptocurrencyUpdateDTO>(crypto);
            Random random = new Random();
            priceChange.CurrentPrice *= (-3m + (decimal)random.NextDouble() * 6m) * (-3m + (decimal)random.NextDouble() * 6m) * (-3m + (decimal)random.NextDouble() * 6m);
            _mapper.Map(crypto, priceChange);
        }

        await _unitOfWork.SaveAsync();
    }
}