using System.Data.Common;
using System.Net.Mime;
using AutoMapper;
using CryptoService.DTOs;
using CryptoService.Entities;
using CryptoService.Profiles;
using CryptoService.Repositories;
using CryptoService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CryptoService.Controllers;


[Route("api")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[Tags("03_Cryptocurrency")]
public class CryptocurrencyController(
    ILogger<CryptocurrencyController> logger,
    IMapper mapper,
    ICryptocurrencyService cryptocurrencyService)
    : Controller
{
    // Get all endpoint
    [HttpGet("cryptos")]
    [ProducesResponseType(typeof(List<CryptocurrencyGetDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<CryptocurrencyGetDTO>>> GetAll()
    {
        try
        {
             return Ok(await cryptocurrencyService.GetCryptocurrencies());
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Unexpected error in the database while getting cryptos.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}");
            return Problem($"An error occured while getting cryptos.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}", statusCode: 500);
        }
    }
    
    
    // Get By ID endpoint    
    [HttpGet("cryptos/{cryptoId:int}")]
    [ProducesResponseType(typeof(CryptocurrencyGetByIdDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CryptocurrencyGetByIdDTO>> GetCryptocurrency([FromRoute]int cryptoId)
    {
        if (cryptoId <= 0) return BadRequest("ID must be positive integer.");

        try
        {
            return Ok(await cryptocurrencyService.GetCryptocurrencyById(cryptoId));
        }
        catch (ArgumentOutOfRangeException aoorEx)
        {
            logger.LogError(aoorEx, $"ArgumentOutOfRangeException occured while getting crypto with ID: {cryptoId}.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}");
            return Problem($"An error occured while getting crypto with ID: {cryptoId}.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}", statusCode: 400);
        }
        catch (KeyNotFoundException knfEx)
        {
            logger.LogError(knfEx, $"KeyNotFoundException occured while getting crypto with ID: {cryptoId}.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}");
            return Problem($"An error occured while getting crypto with ID: {cryptoId}.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}", statusCode: 404);
        }
        catch (DbException db)
        {
            logger.LogError(db, $"DatabaseException occured while getting crypto with ID: {cryptoId}.\nERROR MESSAGE: {db.Message}\nINNER MESSAGE: {db.InnerException?.Message}");
            return Problem($"An error occured in the database while getting crypto by ID: {cryptoId}.\nERROR MESSAGE: {db.Message}\nINNER MESSAGE: {db.InnerException?.Message}", statusCode: 500);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Unexpected error while getting crypto with ID: {cryptoId}.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}");
            return Problem($"An error occured while getting crypto with ID: {cryptoId}.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}", statusCode: 500);
        }
    }

    // Post create crypto endpoint
    [HttpPost("cryptos")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(Cryptocurrency), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CreateCryptocurrency([FromBody] CryptocurrencyCreateDTO crypto)
    {
        try
        {
            await cryptocurrencyService.AddCryptoAsync(crypto);
            return Created();
        }
        catch (DbUpdateException e)
        {
            logger.LogError(e, $"DatabaseUpdateException occured while adding crypto.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}");
            return Problem($"An error occured in the database while creating a crypto.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}", statusCode: 500);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Unexpected exception creating crypto.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}");
            return Problem($"An error occured while creating the crypto.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}", statusCode: 500);
        }
    }
    
    // Put Update endpoint
    [HttpPut("crypto/price/{cryptoId:int}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> PutCryptocurrency([FromRoute] int cryptoId, [FromBody] CryptocurrencyUpdateDTO crypto)
    {
        try
        {
            await cryptocurrencyService.UpdateCryptoAsync(cryptoId, crypto);

            return NoContent();
        }
        catch (ArgumentOutOfRangeException aoorEx)
        {
            logger.LogError(aoorEx, $"ArgumentOutOfRangeException occured while updating crypto with ID: {cryptoId}.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}");
            return Problem($"An error occured while updating crypto with ID: {cryptoId}.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}", statusCode: 400);
        }
        catch (KeyNotFoundException knfEx)
        {
            logger.LogError(knfEx, $"KeyNotFoundException occured while updating crypto with ID: {cryptoId}.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}");
            return Problem($"An error occured while updating crypto with ID: {cryptoId}.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}", statusCode: 404);
        }
        catch (DbUpdateException e)
        {
            logger.LogError(e, $"DatabaseUpdateException occured while updating crypto with ID: {cryptoId}.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}");
            return Problem($"Database error while updating crypto with ID: {cryptoId}.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}", statusCode: 500);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Unexpected exception updating crypto with ID: {cryptoId}.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}");
            return Problem($"An error occured while updating crypto with ID: {cryptoId}.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}", statusCode: 500);
        }
    }

    // Delete By Id endpoint
    [HttpDelete("cryptos/{cryptoId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteCryptocurrency([FromRoute] int cryptoId)
    {
        try
        {
            await cryptocurrencyService.DeleteCryptoAsync(cryptoId);

            return NoContent();
        }
        catch (ArgumentOutOfRangeException aoorEx)
        {
            logger.LogError(aoorEx, $"ArgumentOutOfRangeException occured while deleting crypto with ID: {cryptoId}.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}");
            return Problem($"An error occured while deleting crypto with ID: {cryptoId}.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}", statusCode: 400);
        }
        catch (KeyNotFoundException knfEx)
        {
            logger.LogError(knfEx, $"KeyNotFoundException occured while deleting crypto with ID: {cryptoId}.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}");
            return Problem($"An error occured while deleting crypto with ID: {cryptoId}.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}", statusCode: 404);
        }
        catch (DbUpdateException e)
        {
            logger.LogError(e, $"DatabaseUpdateException occured while deleting crypto with ID: {cryptoId}.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}");
            return Problem($"An error occured while deleting crypto with ID: {cryptoId}.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}", statusCode: 500);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Unexpected exception deleting crypto with ID: {cryptoId}.\nERROR MESSAGE: {e.Message}\nINNER EXCEPTION: {e.InnerException?.Message}");
            return Problem($"Unexpected exception deleting crypto with ID: {cryptoId}.\nERROR MESSAGE: {e.Message}\nINNER EXCEPTION: {e.InnerException?.Message}", statusCode:500);
        }
    }
    
    // Get Crypto History by cryptoId endpoint    
    [HttpGet("crypto/price/history/{cryptoId:int}")]
    [ProducesResponseType(typeof(CryptocurrencyHistoryListDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<CryptocurrencyHistoryListDTO>>> GetCryptocurrencyHistory([FromRoute]int cryptoId)
    {
        try
        {
            return Ok(await cryptocurrencyService.GetCryptocurrencyHistories(cryptoId));
        }
        catch (DbException db)
        {
            logger.LogError(db,
                $"Unexpected error in the database while getting crypto history with crypto ID: {cryptoId}");
            return Problem("An error occured in the database while getting crypto history with crypto ID.",
                statusCode: 500);
        }
        catch (ArgumentOutOfRangeException aoorEx)
        {
            logger.LogError(aoorEx, $"ArgumentOutOfRangeException occured while getting crypto with ID: {cryptoId}.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}");
            return Problem($"An error occured while getting crypto with ID: {cryptoId}.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}", statusCode: 400);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Unexpected error while getting crypto history with crypto ID: {cryptoId}");
            return Problem("An error occured while getting a crypto history with crypto ID.", statusCode: 500);
        }
    }
}