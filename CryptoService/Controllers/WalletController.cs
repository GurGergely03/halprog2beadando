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
[Route("api")]
[Tags("02_Wallet")]
public class WalletController(ILogger<WalletController> logger, IMapper mapper, IWalletService walletService)
    : Controller
{
    private IWalletService _walletService = walletService;

    // Get By ID endpoint    
    [HttpGet("wallet/{userId:int}")]
    [ProducesResponseType(typeof(WalletGetByIdDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<WalletGetByIdDTO>> GetWallet([FromRoute]int userId)
    {
        try
        {
            return Ok(mapper.Map<WalletGetByIdDTO>(await walletService.GetWalletByUserIdAsync(userId)));
        }
        catch (DbException db)
        {
            logger.LogError(db, $"DatabaseException occured while getting wallet with UserID: {userId}\nERROR MESSAGE: {db.Message}\nINNER EXCEPTION MESSAGE: {db.InnerException?.Message}");
            return Problem($"An error occured in the database while getting a wallet by UserID: {userId}\nERROR MESSAGE: {db.Message}\nINNER EXCEPTION MESSAGE: {db.InnerException?.Message}.", statusCode: 500);
        }
        catch (ArgumentOutOfRangeException aoorEx)
        {
            logger.LogError(aoorEx, $"ArgumentOutOfRangeException occured while getting wallet with UserID: {userId}.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}");
            return Problem($"An error occured while getting wallet with UserID: {userId}.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}", statusCode: 400);

        }
        catch (KeyNotFoundException knfEx)
        {
            logger.LogError(knfEx, $"KeyNotFoundException occured while getting wallet with UserID: {userId}.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}");
            return Problem($"An error occured while getting wallet with UserID: {userId}.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}", statusCode: 404);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Unexpected error while getting wallet with UserID: {userId}\nERROR MESSAGE: {e.Message}\nINNER EXCEPTION MESSAGE: {e.InnerException?.Message}");
            return Problem($"An error occured while getting a wallet by UserID: {userId}.\nERROR MESSAGE: {e.Message}\nINNER EXCEPTION MESSAGE: {e.InnerException?.Message}", statusCode: 500);
        }
    }

    // Put Update endpoint
    [HttpPut("wallet/{userId:int}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> PutWallet([FromRoute] int userId, [FromBody] WalletUpdateDTO wallet)
    {
        try
        {
            await walletService.UpdateWalletBalanceAsync(userId, wallet);
            return NoContent();
        }
        catch (DbUpdateException e)
        {
            logger.LogError(e,
                $"DatabaseUpdateException occured while updating wallet balance by UserID: {userId}.\nERROR MESSAGE: {e.Message}\nINNER EXCEPTION MESSAGE: {e.InnerException?.Message}");
            return Problem(
                $"An error occured while updating the wallet balance with UserID: {userId}.\nERROR MESSAGE: {e.Message}\nINNER EXCEPTION MESSAGE: {e.InnerException?.Message}",
                statusCode: 500);

        }
        catch (ArgumentOutOfRangeException aoorEx)
        {
            logger.LogError(aoorEx, $"ArgumentOutOfRangeException occured while updating wallet balance with UserID: {userId}.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}");
            return Problem($"An error occured while updating wallet balance with UserID: {userId}.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}", statusCode: 400);

        }
        catch (KeyNotFoundException knfEx)
        {
            logger.LogError(knfEx, $"KeyNotFoundException occured while updating wallet balance with UserID: {userId}.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}");
            return Problem($"An error occured while updating user with ID: {userId}.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}", statusCode: 404);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Unexpected exception updating wallet balance with UserID: {userId}.\nERROR MESSAGE: {e.Message}\nINNER EXCEPTION MESSAGE: {e.InnerException?.Message}");
            return Problem($"An error occured while updating the wallet balance with UserID: {userId}.\nERROR MESSAGE: {e.Message}\nINNER EXCEPTION MESSAGE: {e.InnerException?.Message}", statusCode: 500);
        }
    }
    
    // Delete By Id endpoint
    [HttpDelete("wallet/{userId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteWallet([FromRoute] int userId)
    {
        try
        {
            await walletService.DeleteWalletAsync(userId);
            return NoContent();
        }
        catch (DbUpdateException e)
        {
            logger.LogError(e,
                $"DatabaseUpdateException occured while deleting wallet with UserID: {userId}.\nERROR MESSAGE: {e.Message}\nINNER EXCEPTION MESSAGE: {e.InnerException?.Message}");
            return Problem(
                $"Failed to delete wallet with UserID: {userId}, due to database error.\nERROR MESSAGE: {e.Message}\nINNER EXCEPTION MESSAGE: {e.InnerException?.Message}",
                statusCode: 500);
        }
        catch (ArgumentOutOfRangeException aoorEx)
        {
            logger.LogError(aoorEx, $"ArgumentOutOfRangeException occured while deleting wallet with UserID: {userId}.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}");
            return Problem($"An error occured while deleting user with ID: {userId}.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}", statusCode: 400);
        }
        catch (KeyNotFoundException knfEx)
        {
            logger.LogError(knfEx, $"KeyNotFoundException occured while deleting wallet with UserID: {userId}.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}");
            return Problem($"An error occured while deleting user with ID: {userId}.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}", statusCode: 404);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Unexpected exception deleting wallet with UserID: {userId}.\nERROR MESSAGE: {e.Message}\nINNER EXCEPTION MESSAGE: {e.InnerException?.Message}");
            return Problem($"Unexpected exception deleting wallet with UserID: {userId}.\nERROR MESSAGE: {e.Message}\nINNER EXCEPTION MESSAGE: {e.InnerException?.Message}", statusCode:500);
        }
    }
    
    // Get Portfolio By Id
    [HttpGet("portfolio/{userId:int}")]
    [ProducesResponseType(typeof(PortfolioDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PortfolioDTO>> GetTransactionHistory([FromRoute] int userId)
    {
        try
        {
            return Ok(await walletService.GetPortfolioAsync(userId));
        }
        catch (DbException db)
        {
            logger.LogError(db,
                $"DatabaseException occured while getting portfolio with ID: {userId}.\nERROR MESSAGE: {db.Message}\nINNER MESSAGE: {db.InnerException?.Message}");
            return Problem(
                $"An error occured in the database while getting portfolio with ID: {userId}.\nERROR MESSAGE: {db.Message}\nINNER MESSAGE: {db.InnerException?.Message}",
                statusCode: 500);
        }
        catch (ArgumentOutOfRangeException aoorEx)
        {
            logger.LogError(aoorEx,
                $"ArgumentOutOfRangeException occured while getting portfolio with ID: {userId}.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}");
            return Problem(
                $"An error occured while getting portfolio with ID: {userId}.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}",
                statusCode: 400);
        }
        catch (ArgumentException aEx)
        {
            logger.LogError(aEx,
                $"ArgumentException occured while getting portfolio with ID: {userId}.\nERROR MESSAGE: {aEx.Message}\nINNER MESSAGE: {aEx.InnerException?.Message}");
            return Problem(
                $"An error occured while getting portfolio with ID: {userId}.\nERROR MESSAGE: {aEx.Message}\nINNER MESSAGE: {aEx.InnerException?.Message}",
                statusCode: 400);
        }
        catch (KeyNotFoundException knfEx)
        {
            logger.LogError(knfEx, $"KeyNotFoundException occured while getting portfolio with ID: {userId}.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}");
            return Problem($"An error occured while getting portfolio with ID: {userId}.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}", statusCode: 404);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Unexpected error while getting portfolio with ID: {userId}.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}");
            return Problem($"An error occured while getting portfolio with ID: {userId}.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}", statusCode: 500);
        }
    }
    
}