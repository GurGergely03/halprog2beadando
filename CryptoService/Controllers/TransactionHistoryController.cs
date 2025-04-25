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

[Route("api")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[Tags("04_Transactions")]
public class TransactionHistoryController(
    ILogger<TransactionHistoryController> logger,
    IMapper mapper,
    ITransactionHistoryService transactionHistoryService)
    : Controller
{
    // purchase endpoint
    [HttpPost("trade/buy")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(TransactionHistory), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Buy([FromBody] TransactionHistoryCreateDTO transactionDetails)
    {
        try
        {
            await transactionHistoryService.BuyAsync(transactionDetails);
            return Created();
        }
        catch (DbUpdateException e)
        {
            logger.LogError(e, $"DatabaseUpdateException occured while creating transaction.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}");
            return Problem($"An error occured in the database while creating transaction.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}", statusCode: 500);
        }
        catch (ArgumentOutOfRangeException aoorEx)
        {
            logger.LogError(aoorEx,
                $"ArgumentOutOfRangeException occured while creating transaction.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}");
            return Problem(
                $"An error occured while creating transaction.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}",
                statusCode: 400);
        }
        catch (ArgumentException aEx)
        {
            logger.LogError(aEx,
                $"ArgumentException occured while creating transaction.\nERROR MESSAGE: {aEx.Message}\nINNER MESSAGE: {aEx.InnerException?.Message}");
            return Problem(
                $"An error occured while creating transaction.\nERROR MESSAGE: {aEx.Message}\nINNER MESSAGE: {aEx.InnerException?.Message}",
                statusCode: 400);
        }
        catch (KeyNotFoundException knfEx)
        {
            logger.LogError(knfEx, $"KeyNotFoundException occured while creating transaction.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}");
            return Problem($"An error occured while creating transaction.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}", statusCode: 404);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Unexpected exception creating transaction.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}");
            return Problem($"An error occured while creating transaction.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}", statusCode: 500);
        }
    }
    
    [HttpPost("trade/sell")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(TransactionHistory), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Sell([FromBody] TransactionHistoryCreateDTO transactionDetails)
    {
        try
        {
            await transactionHistoryService.SellAsync(transactionDetails);
            return Created();
        }
        catch (DbUpdateException e)
        {
            logger.LogError(e, $"DatabaseUpdateException occured while creating transaction.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}");
            return Problem($"An error occured in the database while creating transaction.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}", statusCode: 500);
        }
        catch (ArgumentOutOfRangeException aoorEx)
        {
            logger.LogError(aoorEx,
                $"ArgumentOutOfRangeException occured while creating transaction.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}");
            return Problem(
                $"An error occured while creating transaction.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}",
                statusCode: 400);
        }
        catch (ArgumentException aEx)
        {
            logger.LogError(aEx,
                $"ArgumentException occured while creating transaction.\nERROR MESSAGE: {aEx.Message}\nINNER MESSAGE: {aEx.InnerException?.Message}");
            return Problem(
                $"An error occured while creating transaction.\nERROR MESSAGE: {aEx.Message}\nINNER MESSAGE: {aEx.InnerException?.Message}",
                statusCode: 400);
        }
        catch (KeyNotFoundException knfEx)
        {
            logger.LogError(knfEx, $"KeyNotFoundException occured while creating transaction.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}");
            return Problem($"An error occured while creating transaction.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}", statusCode: 404);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Unexpected exception creating transaction.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}");
            return Problem($"An error occured while creating transaction.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}", statusCode: 500);
        }
    }
    
    // Get By ID endpoint    
    [HttpGet("transactions/{userId:int}")]
    [ProducesResponseType(typeof(List<TransactionHistoryGetByUserIdDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<TransactionHistoryGetByUserIdDTO>>> GetTransactionHistoryByUserId([FromRoute]int userId)
    {
        try
        {
            return Ok(await transactionHistoryService.GetTransactionsByUserIdAsync(userId));
        }
        catch (DbException db)
        {
            logger.LogError(db, $"DatabaseException occured while getting transactions with userID: {userId}\nERROR MESSAGE: {db.Message}\nINNER MESSAGE: {db.InnerException?.Message}");
            return Problem($"An error occured in the database while getting transactions with userID: {userId}.\nERROR MESSAGE: {db.Message}\nINNER MESSAGE: {db.InnerException?.Message}", statusCode: 500);
        }
        catch (ArgumentOutOfRangeException aoorEx)
        {
            logger.LogError(aoorEx, $"ArgumentOutOfRangeException occured while getting transactions with userID: {userId}.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}");
            return Problem($"An error occured while getting transactions with userID: {userId}.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}", statusCode: 400);
        }
        catch (KeyNotFoundException knfEx)
        {
            logger.LogError(knfEx, $"KeyNotFoundException occured while getting transactions with userID: {userId}.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}");
            return Problem($"An error occured while getting transactions with userID: {userId}.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}", statusCode: 404);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Unexpected exception while getting transactions with userID: {userId}.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}");
            return Problem($"An error occured while getting transactions with userID: {userId}.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}", statusCode: 500);
        }
    }
    
    // Get By ID endpoint    
    [HttpGet("transactions/details/{transactionId:int}")]
    [ProducesResponseType(typeof(TransactionHistoryGetByTransactionIdDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserGetByIdDTO>> GetTransactionHistoryByTransactionId([FromRoute]int transactionId)
    {
        try
        {
            return Ok(await transactionHistoryService.GetTransactionsByTransactionIdAsync(transactionId));
        }
        catch (DbException db)
        {
            logger.LogError(db, $"DatabaseException occured while getting transaction with transactionID {transactionId}\nERROR MESSAGE: {db.Message}\nINNER MESSAGE: {db.InnerException?.Message}");
            return Problem($"An error occured in the database while getting transaction with transactionID: {transactionId}.\nERROR MESSAGE: {db.Message}\nINNER MESSAGE: {db.InnerException?.Message}", statusCode: 500);
        }
        catch (ArgumentOutOfRangeException aoorEx)
        {
            logger.LogError(aoorEx, $"ArgumentOutOfRangeException occured while getting transaction with transactionID {transactionId}.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}");
            return Problem($"An error occured while getting transaction with transactionID {transactionId}.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}", statusCode: 400);
        }
        catch (KeyNotFoundException knfEx)
        {
            logger.LogError(knfEx, $"KeyNotFoundException occured while getting transaction with transactionID {transactionId}.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}");
            return Problem($"An error occured while getting transaction with transactionID {transactionId}.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}", statusCode: 404);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Unexpected exception while getting transaction with transactionID {transactionId}.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}");
            return Problem($"An error occured while getting transaction with transactionID: {transactionId}.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}", statusCode: 500);
        }
    }
}