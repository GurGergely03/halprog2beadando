using System.Data.Common;
using System.Net.Mime;
using AutoMapper;
using CryptoService.DTOs;
using CryptoService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CryptoService.Controllers;

[Route("api")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class TransactionHistoryController : Controller
{
    private UnitOfWork _unitOfWork;
    private readonly ILogger<TransactionHistoryController> _logger;
    private readonly IMapper _mapper;

    public TransactionHistoryController(ILogger<TransactionHistoryController> logger, IMapper mapper,
        UnitOfWork unitOfWork)
    {
        _logger = logger;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    // Get all endpoint
    [HttpGet("transactionhistory")]
    [ProducesResponseType(typeof(List<TransactionHistoryGetByUserIdDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<TransactionHistoryGetByUserIdDTO>>> GetAll()
    {
        try
        {
            var transactions = await _unitOfWork.TransactionHistoryRepository.GetAllAsync();
            return _mapper.Map<List<TransactionHistoryGetByUserIdDTO>>(transactions);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error in the database while getting transactions.");
            return Problem("An error occured while getting transactions.", statusCode: 500);
        }
    }


    // Get By ID endpoint    
    [HttpGet("transactionhistory/{id:int}")]
    [ProducesResponseType(typeof(CryptocurrencyGetByIdDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TransactionHistoryGetByIdDTO>> GetTransactionHistory([FromRoute] int id)
    {
        if (id <= 0) return BadRequest("ID must be positive integer.");

        try
        {
            var transactionHistory = await _unitOfWork.TransactionHistoryRepository.GetByIdAsync(id);

            return Ok(_mapper.Map<TransactionHistoryGetByIdDTO>(transactionHistory));
        }
        catch (DbException db)
        {
            _logger.LogError(db, "Unexpected error in the database while getting transaction with ID: {id}", id);
            return Problem("An error occured in the database while getting a transaction by ID.", statusCode: 500);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error while getting transaction with ID: {id}", id);
            return Problem("An error occured while getting a transaction by ID.", statusCode: 500);
        }
    }
}