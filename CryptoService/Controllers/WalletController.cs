using System.Data.Common;
using System.Net.Mime;
using AutoMapper;
using CryptoService.DTOs;
using CryptoService.Entities;
using CryptoService.Repositories;
using CryptoService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CryptoService.Controllers;

[ApiController]
[Route("wallet")]
public class WalletController(ILogger<WalletController> logger, IMapper mapper, IWalletService walletService)
    : Controller
{
    private IWalletService _walletService = walletService;

    // Get By ID endpoint    
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(WalletGetByIdDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<WalletGetByIdDTO>> GetWallet([FromRoute]int id)
    {
        try
        {
            return Ok(mapper.Map<WalletGetByIdDTO>(await walletService.GetWalletByUserIdAsync(id)));
        }
        catch (DbException db)
        {
            logger.LogError(db, $"DatabaseException occured while getting wallet with UserID: {id}\nERROR MESSAGE: {db.Message}\nINNER EXCEPTION MESSAGE: {db.InnerException?.Message}");
            return Problem($"An error occured in the database while getting a wallet by UserID: {id}\nERROR MESSAGE: {db.Message}\nINNER EXCEPTION MESSAGE: {db.InnerException?.Message}.", statusCode: 500);
        }
        catch (ArgumentOutOfRangeException aoorEx)
        {
            logger.LogError(aoorEx, $"ArgumentOutOfRangeException occured while getting wallet with UserID: {id}.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}");
            return Problem($"An error occured while getting wallet with UserID: {id}.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}", statusCode: 400);

        }
        catch (KeyNotFoundException knfEx)
        {
            logger.LogError(knfEx, $"KeyNotFoundException occured while getting wallet with UserID: {id}.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}");
            return Problem($"An error occured while getting wallet with UserID: {id}.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}", statusCode: 404);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Unexpected error while getting wallet with UserID: {id}\nERROR MESSAGE: {e.Message}\nINNER EXCEPTION MESSAGE: {e.InnerException?.Message}");
            return Problem($"An error occured while getting a wallet by UserID: {id}.\nERROR MESSAGE: {e.Message}\nINNER EXCEPTION MESSAGE: {e.InnerException?.Message}", statusCode: 500);
        }
    }

    // Put Update endpoint
    [HttpPut("{id:int}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> PutWallet([FromRoute] int id, [FromBody] WalletUpdateDTO wallet)
    {
        try
        {
            await walletService.UpdateWalletBalanceAsync(id, wallet);
            return NoContent();
        }
        catch (DbUpdateException e)
        {
            logger.LogError(e,
                $"DatabaseUpdateException occured while updating wallet balance by UserID: {id}.\nERROR MESSAGE: {e.Message}\nINNER EXCEPTION MESSAGE: {e.InnerException?.Message}");
            return Problem(
                $"An error occured while updating the wallet balance with UserID: {id}.\nERROR MESSAGE: {e.Message}\nINNER EXCEPTION MESSAGE: {e.InnerException?.Message}",
                statusCode: 500);

        }
        catch (ArgumentOutOfRangeException aoorEx)
        {
            logger.LogError(aoorEx, $"ArgumentOutOfRangeException occured while updating wallet balance with UserID: {id}.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}");
            return Problem($"An error occured while updating wallet balance with UserID: {id}.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}", statusCode: 400);

        }
        catch (KeyNotFoundException knfEx)
        {
            logger.LogError(knfEx, $"KeyNotFoundException occured while updating wallet balance with UserID: {id}.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}");
            return Problem($"An error occured while updating user with ID: {id}.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}", statusCode: 404);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Unexpected exception updating wallet balance with UserID: {id}.\nERROR MESSAGE: {e.Message}\nINNER EXCEPTION MESSAGE: {e.InnerException?.Message}");
            return Problem($"An error occured while updating the wallet balance with UserID: {id}.\nERROR MESSAGE: {e.Message}\nINNER EXCEPTION MESSAGE: {e.InnerException?.Message}", statusCode: 500);
        }
    }
    
    // Delete By Id endpoint
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteWallet([FromRoute] int id)
    {
        try
        {
            await walletService.DeleteWalletAsync(id);
            return NoContent();
        }
        catch (DbUpdateException e)
        {
            logger.LogError(e,
                $"DatabaseUpdateException occured while deleting wallet with UserID: {id}.\nERROR MESSAGE: {e.Message}\nINNER EXCEPTION MESSAGE: {e.InnerException?.Message}");
            return Problem(
                $"Failed to delete wallet with UserID: {id}, due to database error.\nERROR MESSAGE: {e.Message}\nINNER EXCEPTION MESSAGE: {e.InnerException?.Message}",
                statusCode: 500);
        }
        catch (ArgumentOutOfRangeException aoorEx)
        {
            logger.LogError(aoorEx, $"ArgumentOutOfRangeException occured while deleting wallet with UserID: {id}.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}");
            return Problem($"An error occured while deleting user with ID: {id}.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}", statusCode: 400);
        }
        catch (KeyNotFoundException knfEx)
        {
            logger.LogError(knfEx, $"KeyNotFoundException occured while deleting wallet with UserID: {id}.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}");
            return Problem($"An error occured while deleting user with ID: {id}.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}", statusCode: 404);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Unexpected exception deleting wallet with UserID: {id}.\nERROR MESSAGE: {e.Message}\nINNER EXCEPTION MESSAGE: {e.InnerException?.Message}");
            return Problem($"Unexpected exception deleting wallet with UserID: {id}.\nERROR MESSAGE: {e.Message}\nINNER EXCEPTION MESSAGE: {e.InnerException?.Message}", statusCode:500);
        }
    }
}