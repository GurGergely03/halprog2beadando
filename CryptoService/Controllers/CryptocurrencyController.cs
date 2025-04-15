using System.Data.Common;
using System.Net.Mime;
using AutoMapper;
using CryptoService.DTOs;
using CryptoService.Entities;
using CryptoService.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CryptoService.Controllers;


[Route("api/cryptos")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class CryptocurrencyController : Controller
{
    private UnitOfWork _unitOfWork;
    private readonly ILogger<CryptocurrencyController> _logger;
    private readonly IMapper _mapper;

    public CryptocurrencyController(ILogger<CryptocurrencyController> logger, IMapper mapper, UnitOfWork unitOfWork)
    {
        _logger = logger;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    
    // Get all endpoint
    [HttpGet("getall")]
    [ProducesResponseType(typeof(List<CryptocurrencyGetDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<CryptocurrencyGetDTO>>> GetAll()
    {
        try
        {
            var cryptos = await _unitOfWork.CryptocurrencyRepository.GetAllAsync();
            return _mapper.Map<List<CryptocurrencyGetDTO>>(cryptos);        
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error in the database while getting cryptos.");
            return Problem("An error occured while getting cryptos.", statusCode: 500);
        }
    }
    
    
    // Get By ID endpoint    
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CryptocurrencyGetByIdDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CryptocurrencyGetByIdDTO>> GetCryptocurrency([FromRoute]int id)
    {
        if (id <= 0) return BadRequest("ID must be positive integer.");

        try
        {
            var crypto = await _unitOfWork.CryptocurrencyRepository.GetByIdAsync(id);
            
            return Ok(_mapper.Map<CryptocurrencyGetByIdDTO>(crypto));
        }
        catch (DbException db)
        {
            _logger.LogError(db, "Unexpected error in the database while getting crypto with ID: {id}", id);
            return Problem("An error occured in the database while getting a crypto by ID.", statusCode: 500);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error while getting crypto with ID: {id}", id);
            return Problem("An error occured while getting a crypto by ID.", statusCode: 500);
        }
    }

    // Post create crypto endpoint
    [HttpPost("create")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(Cryptocurrency), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CreateCryptocurrency([FromBody] CryptocurrencyCreateDTO crypto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            await _unitOfWork.CryptocurrencyRepository.InsertAsync(_mapper.Map<Cryptocurrency>(crypto));
            await _unitOfWork.SaveAsync();

            return Created();
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Unexpected error in the database while adding crypto.");
            return Problem("An error occured in the database while creating a crypto.", statusCode: 500);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected exception creating crypto.");
            return Problem("An error occured while creating the crypto.", statusCode: 500);
        }
    }
    
    // Put Update endpoint
    [HttpPut("{id:int}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> PutCryptocurrency([FromRoute] int id, [FromBody] CryptocurrencyUpdateDTO crypto)
    {
        // parameter checks
        if (id <= 0) return BadRequest("ID must be positive integer.");
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            Cryptocurrency? existingCryptocurrency = await _unitOfWork.CryptocurrencyRepository.GetByIdAsync(id);
            if (existingCryptocurrency == null) { return NotFound(); }
            _mapper.Map(crypto, existingCryptocurrency);
            
            if (crypto.CurrentPrice != null)
            {
                // after manual price change, create a history instance and add it to the list of the updated cryptocurrency
                // await _unitOfWork.CryptocurrencyHistoryRepository.InsertAsync(_mapper.Map<CryptocurrencyHistory>(crypto));
            }
            await _unitOfWork.SaveAsync();
        
            return NoContent();
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Database error updating crypto {id}.", id);
            return Problem("Database error while updating crypto.", statusCode: 500);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected exception updating crypto {id}.", id);
            return Problem("An error occured while updating the crypto.", statusCode: 500);
        }
    }

    // Delete By Id endpoint
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteCryptocurrency([FromRoute] int id)
    {
        if (id <= 0) return BadRequest("ID must be a positive integer.");
        try
        {
            await _unitOfWork.CryptocurrencyRepository.DeleteByIdAsync(id);
            await _unitOfWork.SaveAsync();
            
            return NoContent();
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Database error deleting crypto {id}.", id);
            return Problem("Failed to delete crypto due to database error.", statusCode: 500);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected exception deleting crypto {id}", id);
            return Problem("Unexpected exception deleting crypto.", statusCode:500);
        }
    }
}