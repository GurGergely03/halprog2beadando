using CryptoService.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CryptoService.Controllers;


[Route("api/[controller]")]
[ApiController]
public class CryptocurrencyController : Controller
{
    private readonly CryptoContext _context;

    public CryptocurrencyController(CryptoContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cryptocurrency>>> GetCryptocurrencies()
    {
        return await _context.Cryptocurrencies.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Cryptocurrency>> GetCryptocurrency([FromRoute] long id)
    {
        var cryptocurrency = await _context.Cryptocurrencies.FindAsync(id);

        if (cryptocurrency == null)
        {
            return NotFound();
        }
        return cryptocurrency;
    }
}