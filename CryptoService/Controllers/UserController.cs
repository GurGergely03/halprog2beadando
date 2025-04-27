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
[Tags("01_User")]
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
    [HttpGet("{userId:int}")]
    [ProducesResponseType(typeof(UserGetByIdDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserGetByIdDTO>> GetUser([FromRoute]int userId)
    {
        try
        {
            return Ok(await _userService.GetUserByIdAsync(userId));
        }
        catch (DbException db)
        {
            _logger.LogError(db, $"DatabaseException occured while getting user with ID: {userId}\nERROR MESSAGE: {db.Message}\nINNER MESSAGE: {db.InnerException?.Message}");
            return Problem($"An error occured in the database while getting user by ID: {userId}.\nERROR MESSAGE: {db.Message}\nINNER MESSAGE: {db.InnerException?.Message}", statusCode: 500);
        }
        catch (ArgumentOutOfRangeException aoorEx)
        {
            _logger.LogError(aoorEx, $"ArgumentOutOfRangeException occured while getting user with ID: {userId}.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}");
            return Problem($"An error occured while getting user with ID: {userId}.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}", statusCode: 400);
        }
        catch (KeyNotFoundException knfEx)
        {
            _logger.LogError(knfEx, $"KeyNotFoundException occured while getting user with ID: {userId}.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}");
            return Problem($"An error occured while getting user with ID: {userId}.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}", statusCode: 404);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unexpected exception while getting user with ID: {userId}.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}");
            return Problem($"An error occured while getting user by ID: {userId}.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}", statusCode: 500);
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
            return Problem($"An error occured in the database while creating user.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}", statusCode: 500);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unexpected exception creating user.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}");
            return Problem($"An error occured while creating user.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}", statusCode: 500);
        }
    }
    
    // Put Update endpoint
    [HttpPut("{userId:int}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> PutUser([FromRoute] int userId, [FromBody] UserUpdateDTO user)
    {
        // parameter checks
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            await _userService.UpdateUserAsync(userId, user);
            return NoContent();
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, $"DatabaseUpdateException occured while updating user with ID: {userId}.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}");
            return Problem($"DatabaseUpdateException occured while updating user with ID: {userId}.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}", statusCode: 500);
        }
        catch (ArgumentOutOfRangeException aoorEx)
        {
            _logger.LogError(aoorEx, $"ArgumentOutOfRangeException occured while updating user with ID: {userId}.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}");
            return Problem($"An error occured while updating user with ID: {userId}.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}", statusCode: 400);
        }
        catch (KeyNotFoundException knfEx)
        {
            _logger.LogError(knfEx, $"KeyNotFoundException occured while updating user with ID: {userId}.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}");
            return Problem($"An error occured while updating user with ID: {userId}.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}", statusCode: 404);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unexpected exception updating user with ID: {userId}.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}");
            return Problem($"An error occured while updating user with ID: {userId}.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}", statusCode: 500);
        }
    }

    // Delete By Id endpoint
    [HttpDelete("{userId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteUser([FromRoute] int userId)
    {
        try
        {
            await _userService.DeleteUserAsync(userId);
            return NoContent();
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, $"DatabaseUpdateException occured while deleting user with ID: {userId}.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}");
            return Problem($"Failed to delete user due to database error.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}", statusCode: 500);
        }
        catch (ArgumentOutOfRangeException aoorEx)
        {
            _logger.LogError(aoorEx, $"ArgumentOutOfRangeException occured while deleting user with ID: {userId}.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}");
            return Problem($"An error occured while deleting user with ID: {userId}.\nERROR MESSAGE: {aoorEx.Message}\nINNER MESSAGE: {aoorEx.InnerException?.Message}", statusCode: 400);
        }
        catch (KeyNotFoundException knfEx)
        {
            _logger.LogError(knfEx, $"KeyNotFoundException occured while deleting user with ID: {userId}.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}");
            return Problem($"An error occured while deleting user with ID: {userId}.\nERROR MESSAGE: {knfEx.Message}\nINNER MESSAGE: {knfEx.InnerException?.Message}", statusCode: 404);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unexpected exception occured while deleting user with ID: {userId}.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}");
            return Problem($"Unexpected exception deleting user with ID: {userId}.\nERROR MESSAGE: {e.Message}\nINNER MESSAGE: {e.InnerException?.Message}", statusCode:500);
        }
    }
}