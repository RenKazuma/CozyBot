using Discord_Core.Database;
using Discord_Core.Database.Entities;
using System.Data.SQLite;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DiscordApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    
    private readonly DatabaseContext _databaseContext;

    public UserController(ILogger<UserController> logger, DatabaseContext dbContext)
    {
        _logger = logger;
        _databaseContext = dbContext;
    }

    /// <summary>
    /// Find user by discordId
    /// </summary>
    /// <param name="discordId">DiscordId to get the user</param>>
    /// <response code="200">Successfull operation</response>
    /// <response code="404">No user found</response>
    /// <response code="500">No Connection to Database</response>
    /// <returns>The found user</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    public ActionResult GetUserById([FromQuery][Required] string discordId)
    {

         User? user = _databaseContext.users
         .SingleOrDefault(u => u.DiscordId == discordId);

        if (user == null)
        {
            return NotFound($"User with Discord ID '{discordId}' not found.");
        }

        return Ok(user);
    }

    /// <summary>
    /// Add a new user
    /// </summary>
    /// <param name="User">User to add</param>
    /// <response code="200">Successfull operation</response>
    /// <response code="409">User couldn't be added</response>
    /// <response code="500">No Connection to Database</response>
    [HttpPost]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    public ActionResult AddUser([FromQuery][Required] User user)
    {

        if (_databaseContext.users.Any(u => u.DiscordId == user.DiscordId))
        {
            return BadRequest("The DiscordId already exists.");
        }
        
        try{
            _databaseContext.users.Add(user);
            _databaseContext.SaveChanges();
        } catch(Exception ex)
        {
            return StatusCode(409);
        }

        return Ok();
        
    }

    /// <summary>
    /// Update an exisitng user
    /// </summary>
    /// <param name="User">Data of the user to be updated</param>
    /// <response code="200">Successfull operation</response>
    /// <response code="404">No user found</response>
    /// <response code="500">No Connection to Database</response>
    //[HttpPut]
    //[ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    //[ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    //public ActionResult ChangeUser([FromQuery] User user)
    //{
    //    var oldUser = _databaseContext.Users.FirstOrDefault(u => u.DiscordId == user.DiscordId && u.Id == user.Id);

    //    if (oldUser == null)
    //    {
    //        return NotFound();
    //    }

    //    oldUser.Current_Level = user.Current_Level;
    //    oldUser.Current_Exp = user.Current_Exp;
    //    oldUser.Coins = user.Coins;
        
    //    _databaseContext.SaveChanges();
    //    return Ok();
    //}

    /// <summary>
    /// Deletes a User
    /// </summary>
    /// <param name="connectionId">User to delete</param>
    /// <response code="200">User removed</response>
    /// <response code="404">User not found</response>
    /// <response code="409">User couldn't be removed</response>
    /// <response code="500">No User to Database</response>
    [HttpDelete]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    public IResult RemoveUser([FromQuery][Required] string discordId)
    {
        var entity = _databaseContext.users.SingleOrDefault(u => u.DiscordId == discordId);

        if (entity == null)
        {
            return Results.NotFound();
        }

        try
        {
            _databaseContext.users.Remove(entity);
            _databaseContext.SaveChanges();
        } catch
        {
            return Results.StatusCode(409);
        }

        return Results.Ok();
    }
}
