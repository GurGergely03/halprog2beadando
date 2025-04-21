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

[Route("api/users")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class UserController : Controller
{
    private readonly ILogger<UserController> _logger;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public UserController(ILogger<UserController> logger, IMapper mapper, IUserService userService)
    {
        _logger = logger;
        _mapper = mapper;
        _userService = userService;
    }
    
    // not needed
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
            return Ok(await _userService.GetUsersAsync());
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unexpected exception occured in the database while getting users.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}");
            return Problem($"An error occured while getting users.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}", statusCode: 500);
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
        try
        {
            return Ok(await _userService.GetUserByIdAsync(id));
        }
        catch (DbException db)
        {
            _logger.LogError(db,
                $"DatabaseException occured in the database while getting user with ID: {id}\nERROR MESSAGE: {db.Message}\nINNER MESSAGE: {db.InnerException?.Message}");
            return Problem(
                $"An error occured in the database while getting a user by ID: {id}.\nERROR MESSAGE: {db.Message}\nINNER MESSAGE: {db.InnerException?.Message}",
                statusCode: 500);
        }
        catch (ArgumentOutOfRangeException aoorEx)
        {
            _logger.LogError(aoorEx,
                $"ArgumentOutOfRangeException occured while getting user with ID: {id}.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}");
            return Problem(
                $"An error occured while getting a user by ID: {id}.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}",
                statusCode: 400);
        }
        catch (KeyNotFoundException knfEx)
        {
            _logger.LogError(knfEx, $"KeyNotFoundException occured while getting user with ID: {id}.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}");
            return Problem($"An error occured while getting a user with ID: {id}.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}", statusCode: 400);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unexpected general exception while getting user with ID: {id}.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}");
            return Problem($"An error occured while getting a user by ID: {id}.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}", statusCode: 500);
        }
    }

    // Post Register endpoint
    [HttpPost("register")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CreateUser([FromBody] UserCreateDTO user)
    {
        try
        {
            await _userService.AddUserAsync(user);
            return Created();
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, $"DatabaseUpdateException occured while adding user to the database.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}");
            return Problem($"An error occured in the database while creating a user.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}", statusCode: 500);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unexpected general exception creating user.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}");
            return Problem($"An error occured while creating the user.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}", statusCode: 500);
        }
    }
    
    // Put Update endpoint
    [HttpPut("{id:int}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> PutUser([FromRoute] int id, [FromBody] UserUpdateDTO user)
    {
        // parameter checks
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            await _userService.UpdateUserAsync(id, user);
            return NoContent();
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, $"DatabaseUpdateException occured while updating user with ID: {id}.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}");
            return Problem($"DatabaseUpdateException occured while updating user with ID: {id}.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}", statusCode: 500);
        }
        catch (ArgumentOutOfRangeException aoorEx)
        {
            _logger.LogError(aoorEx, $"ArgumentOutOfRangeException occured while updating user with ID: {id}.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}");
            return Problem($"An error occured while updating a user with ID: {id}.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}", statusCode: 400);
        }
        catch (KeyNotFoundException knfEx)
        {
            _logger.LogError(knfEx, $"KeyNotFoundException occured while updating user with ID: {id}.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}");
            return Problem($"An error occured while updating a user with ID: {id}.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}", statusCode: 400);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unexpected general exception updating user with ID: {id}.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}");
            return Problem($"An error occured while updating the user with ID: {id}.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}", statusCode: 500);
        }
    }

    // Delete By Id endpoint
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteUser([FromRoute] int id)
    {
        try
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, $"Database error deleting user with ID: {id}.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}");
            return Problem($"Failed to delete user due to database error.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}", statusCode: 500);
        }
        catch (ArgumentOutOfRangeException aoorEx)
        {
            _logger.LogError(aoorEx, $"ArgumentOutOfRangeException occured while deleting user with ID: {id}.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}");
            return Problem($"An error occured while deleting a user with ID: {id}.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}", statusCode: 400);
        }
        catch (KeyNotFoundException knfEx)
        {
            _logger.LogError(knfEx, $"KeyNotFoundException occured while deleting user with ID: {id}.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}");
            return Problem($"An error occured while deleting a user with ID: {id}.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}", statusCode: 400);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unexpected exception deleting user with ID: {id}.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}");
            return Problem($"Unexpected exception deleting user with ID: {id}.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}", statusCode:500);
        }
    }
}