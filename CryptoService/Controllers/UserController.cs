using System.Data.Common;
using System.Net.Mime;
using AutoMapper;
using CryptoService.DTOs;
using CryptoService.Entities;
using CryptoService.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CryptoService.Controllers;

[Route("api/users")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class UserController : Controller
{
    private readonly CryptoContext _context;
    private UnitOfWork _unitOfWork;
    private readonly ILogger<UserController> _logger;
    private readonly IMapper _mapper;

    public UserController(CryptoContext context, ILogger<UserController> logger, IMapper mapper, UnitOfWork unitOfWork)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    
    // Get all endpoint

    [HttpGet("getall")]
    [ProducesResponseType(typeof(List<UserGetDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<UserGetDTO>>> GetAll()
    {
        try
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync(includedProperties: ["Wallet"]);
            return _mapper.Map<List<UserGetDTO>>(users);        
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error in the database while getting users.");
            return Problem("An error occured while getting users.", statusCode: 500);
        }
    }
    
    
    // Get By ID endpoint    
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(UserGetByIdDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserGetByIdDTO>> GetUser([FromRoute]int id)
    {
        if (id <= 0) return BadRequest("ID must be positive integer.");

        try
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            
            return Ok(_mapper.Map<UserGetByIdDTO>(user));
        }
        catch (DbException db)
        {
            _logger.LogError(db, "Unexpected error in the database while getting user with ID: {id}", id);
            return Problem("An error occured in the database while getting a user by ID.", statusCode: 500);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error while getting user with ID: {id}", id);
            return Problem("An error occured while getting a user by ID.", statusCode: 500);
        }
    }

    [HttpPost("register")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CreateUser([FromBody] User user)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            await _unitOfWork.UserRepository.InsertAsync(user);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Unexpected error in the database while adding user.");
            return Problem("An error occured in the database while creating a user.", statusCode: 500);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected exception creating user.");
            return Problem("An error occured while creating the user.", statusCode: 500);
        }
    }
    
    [HttpPut("{id:int}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> PutUser([FromRoute] int id, [FromBody] UserUpdateDTO user)
    {
        // parameter checks
        if (id <= 0) return BadRequest("ID must be positive integer.");
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            User? existingUser = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (existingUser == null) { return NotFound(); }
            _mapper.Map(user, existingUser);
            try
            {
                await _unitOfWork.SaveAsync();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Unexpected error in the database while updating a user.");
                return Problem("An error occured while updating the user.", statusCode: 500);
            }
            return NoContent();
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Database error updating user {id}.", id);
            return Problem("Database error while updating user.", statusCode: 500);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected exception updating user {id}.", id);
            return Problem("An error occured while updating the user.", statusCode: 500);
        }
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteUser([FromRoute] int id)
    {
        if (id <= 0) return BadRequest("ID must be a positive integer.");
        try
        {
            await _unitOfWork.UserRepository.DeleteByIdAsync(id);
            await _unitOfWork.SaveAsync();
            
            return NoContent();
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Database error deleting user {id}.", id);
            return Problem("Failed to delete user due to database error.", statusCode: 500);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected exception deleting user {id}", id);
            return Problem("Unexpected exception deleting user.", statusCode:500);
        }
    }
}