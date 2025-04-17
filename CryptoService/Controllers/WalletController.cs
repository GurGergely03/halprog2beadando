using System.Data.Common;
using System.Net.Mime;
using AutoMapper;
using CryptoService.DTOs;
using CryptoService.Entities;
using CryptoService.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CryptoService.Controllers;

[ApiController]
[Route("wallet")]
public class WalletController : Controller
{
    private readonly ILogger<WalletController> _logger;
    private UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public WalletController(ILogger<WalletController> logger, UnitOfWork unitOfWork, IMapper mapper) 
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    // Get By ID endpoint    
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(WalletGetByIdDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<WalletGetByIdDTO>> GetWallet([FromRoute]int id)
    {
        if (id <= 0) return BadRequest("ID must be positive integer.");

        try
        {
            var wallet = await _unitOfWork.WalletRepository.GetByIdAsync(id);
            
            return Ok(_mapper.Map<WalletGetByIdDTO>(wallet));
        }
        catch (DbException db)
        {
            _logger.LogError(db, $"Unexpected error in the database while getting wallet with ID: {id}\nERROR MESSAGE: {db.Message}\nINNER EXCEPTION MESSAGE: {db.InnerException.Message}");
            return Problem($"An error occured in the database while getting a wallet by ID: {id}\nERROR MESSAGE: {db.Message}\nINNER EXCEPTION MESSAGE: {db.InnerException.Message}.", statusCode: 500);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unexpected error while getting wallet with ID: {id}\nERROR MESSAGE: {e.Message}\nINNER EXCEPTION MESSAGE: {e.InnerException.Message}");
            return Problem($"An error occured while getting a wallet by ID: {id}.\nERROR MESSAGE: {e.Message}\nINNER EXCEPTION MESSAGE: {e.InnerException.Message}", statusCode: 500);
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
        // parameter checks
        if (id <= 0) return BadRequest("ID must be positive integer.");
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            Wallet? existingWallet = await _unitOfWork.WalletRepository.GetByIdAsync(id);
            if (existingWallet == null) { return NotFound(); }
            _mapper.Map(wallet, existingWallet);
            
            return NoContent();
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, $"Unexpected error in the database while updating wallet by ID: {id}.\nERROR MESSAGE: {e.Message}\nINNER EXCEPTION MESSAGE: {e.InnerException.Message}");
            return Problem($"An error occured while updating the wallet with ID: {id}.\nERROR MESSAGE: {e.Message}\nINNER EXCEPTION MESSAGE: {e.InnerException.Message}", statusCode: 500);
            
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unexpected exception updating wallet with ID: {id}.\nERROR MESSAGE: {e.Message}\nINNER EXCEPTION MESSAGE: {e.InnerException.Message}");
            return Problem($"An error occured while updating the wallet with ID: {id}.\nERROR MESSAGE: {e.Message}\nINNER EXCEPTION MESSAGE: {e.InnerException.Message}", statusCode: 500);
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
        if (id <= 0) return BadRequest("ID must be a positive integer.");
        try
        {
            await _unitOfWork.WalletRepository.DeleteByIdAsync(id);
            await _unitOfWork.SaveAsync();
            
            return NoContent();
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, $"Database error deleting wallet with ID: {id}.\nERROR MESSAGE: {e.Message}\nINNER EXCEPTION MESSAGE: {e.InnerException.Message}");
            return Problem($"Failed to delete wallet with ID: {id}, due to database error.\nERROR MESSAGE: {e.Message}\nINNER EXCEPTION MESSAGE: {e.InnerException.Message}", statusCode: 500);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unexpected exception deleting wallet with ID: {id}.\nERROR MESSAGE: {e.Message}\nINNER EXCEPTION MESSAGE: {e.InnerException.Message}");
            return Problem($"Unexpected exception deleting wallet with ID: {id}.\nERROR MESSAGE: {e.Message}\nINNER EXCEPTION MESSAGE: {e.InnerException.Message}", statusCode:500);
        }
    }
}