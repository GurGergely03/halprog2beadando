using System.Data.Common;
using System.Net.Mime;
using AutoMapper;
using CryptoService.DTOs;
using CryptoService.Entities;
using CryptoService.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CryptoService.Controllers;

[Route("api/crypto")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class CryptocurrencyHistoryController : Controller
{
    private UnitOfWork _unitOfWork;
    private readonly ILogger<CryptocurrencyHistoryController> _logger;
    private readonly IMapper _mapper;

    public CryptocurrencyHistoryController(ILogger<CryptocurrencyHistoryController> logger, IMapper mapper, UnitOfWork unitOfWork)
    {
        _logger = logger;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    
    // Get By crypto ID endpoint    
    [HttpGet("price/history/{id:int}")]
    [ProducesResponseType(typeof(CryptocurrencyHistoryGetByCryptoIdDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<CryptocurrencyHistoryGetByCryptoIdDTO>>> GetCryptocurrencyHistory([FromRoute]int id)
    {
        if (id <= 0) return BadRequest("ID must be positive integer.");

        try
        {
            var allHistory = await _unitOfWork.CryptocurrencyHistoryRepository.GetAllAsync();
            List<CryptocurrencyHistory> matchingHistory = new List<CryptocurrencyHistory>();
            foreach (var history in allHistory)
            {
                if (id == history.CryptocurrencyId)
                {
                    matchingHistory.Add(_mapper.Map<CryptocurrencyHistory>(history));
                }
            }
            return Ok(_mapper.Map<List<CryptocurrencyHistoryGetByCryptoIdDTO>>(matchingHistory));
        }
        catch (DbException db)
        {
            _logger.LogError(db, "Unexpected error in the database while getting crypto history with crypto ID: {id}", id);
            return Problem("An error occured in the database while getting a crypto history by crypto ID.", statusCode: 500);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error while getting crypto history with crypto ID: {id}", id);
            return Problem("An error occured while getting a crypto history by crypto ID.", statusCode: 500);
        }
    }

   /* // Post create crypto history endpoint
    [HttpPost("create")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(CryptocurrencyHistory), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CreateCryptocurrencyHistory([FromBody] CryptocurrencyHistoryCreateDTO cryptoHistory)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            await _unitOfWork.CryptocurrencyHistoryRepository.InsertAsync(_mapper.Map<CryptocurrencyHistory>(cryptoHistory));
            await _unitOfWork.SaveAsync();

            return Created();
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Unexpected error in the database while adding crypto history.");
            return Problem("An error occured in the database while creating a crypto history.", statusCode: 500);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected exception creating crypto history.");
            return Problem("An error occured while creating the crypto history.", statusCode: 500);
        }
    }*/
}