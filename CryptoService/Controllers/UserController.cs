using System.Data.Common;
using System.Net.Mime;
using AutoMapper;
using CryptoService.DTOs;
using CryptoService.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CryptoService.Controllers;

[Route("api/users")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class UserController : Controller
{
    private readonly CryptoContext _context;
    private readonly ILogger<UserController> _logger;
    private readonly IMapper _mapper;

    public UserController(CryptoContext context, ILogger<UserController> logger, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
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
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            return Ok();
            //return Ok(_mapper.Map<UserGetByIdDTO>);
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
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

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
    public async Task<ActionResult> PutUser([FromRoute] int id, [FromBody] User user)
    {
        // parameter checks
        if (id <= 0) return BadRequest("ID must be positive integer.");
        if (id != user.Id) return BadRequest("ID mismatch");
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null) return NotFound();

            _context.Entry(existingUser).CurrentValues.SetValues(user);

            await _context.SaveChangesAsync();
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
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            
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