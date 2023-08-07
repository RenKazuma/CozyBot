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
public class PollController : ControllerBase
{
    private readonly ILogger<PollController> _logger;
    
    private readonly DatabaseContext _databaseContext;

    public PollController(ILogger<PollController> logger, DatabaseContext dbContext)
    {
        _logger = logger;
        _databaseContext = dbContext;
    }

    /// <summary>
    /// Find poll by discormessageId
    /// </summary>
    /// <param name="messageId">MessageId to get the poll</param>>
    /// <response code="200">Successfull operation</response>
    /// <response code="404">No poll found</response>
    /// <response code="500">No Connection to Database</response>
    /// <returns>The found poll</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<Poll>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    public ActionResult GetPollByMessageid([FromQuery][Required] string messageId)
    {

         Poll? poll = _databaseContext.Polls
         .SingleOrDefault(u => u.messageId == messageId);

        if (poll == null)
        {
            return NotFound($"Poll with Message ID '{messageId}' not found.");
        }

        return Ok(poll);
    }

    /// <summary>
    /// Add a new poll
    /// </summary>
    /// <param name="Poll">Poll to add</param>
    /// <response code="200">Successfull operation</response>
    /// <response code="409">Poll couldn't be added</response>
    /// <response code="500">No Connection to Database</response>
    [HttpPost]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    public ActionResult AddPoll([FromQuery][Required] Poll poll)
    {

        if (_databaseContext.Polls.Any(u => u.messageId == poll.messageId))
        {
            return BadRequest("The MessageId already exists.");
        }
        
        try{
            _databaseContext.Polls.Add(poll);
            _databaseContext.SaveChanges();
        } catch(Exception ex)
        {
            return StatusCode(409);
        }

        return Ok();
        
    }
}
