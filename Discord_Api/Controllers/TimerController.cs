using Discord_Core.Database;
using Discord_Core.Database.Entities;
using System.Data.SQLite;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Timer = Discord_Core.Database.Entities.Timer;
namespace DiscordApi.Controllers
{
    /// <summary>
    /// Controller for managing timers.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class TimerController : ControllerBase
    {
        private readonly ILogger<TimerController> _logger;
        private readonly DatabaseContext _databaseContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="dbContext">The database context.</param>
        public TimerController(ILogger<TimerController> logger, DatabaseContext dbContext)
        {
            _logger = logger;
            _databaseContext = dbContext;
        }

        /// <summary>
        /// Get a list of timers.
        /// </summary>
        /// <response code="200">Successful operation</response>
        /// <response code="404">No buff found</response>
        /// <response code="500">No connection to the database</response>
        /// <returns>The list of timers.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Timer>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public ActionResult GetBuffs()
        {
            List<Timer> timers = _databaseContext.timers.ToList();

            if (timers.Count() == 0)
            {
                return NotFound("No buff found");
            }

            return Ok(timers);
        }

        /// <summary>
        /// Add a new Timer.
        /// </summary>
        /// <param name="buff">Timer to add.</param>
        /// <response code="200">Successful operation</response>
        /// <response code="409">Timer couldn't be added</response>
        /// <response code="500">No connection to the database</response>
        [HttpPost]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public ActionResult Add([FromBody] Timer buff)
        {
            if (_databaseContext.timers.Any(u => u.Name == buff.Name))
            {
                return BadRequest("The Name already exists.");
            }

            try
            {
                _databaseContext.timers.Add(buff);
                _databaseContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode(409);
            }

            return Ok();
        }

        /// <summary>
        /// Update an existing buff.
        /// </summary>
        /// <param name="id">Id of the buff to be updated.</param>
        /// <param name="newName">Text to be set as a new Name</param>
        /// <param name="newDescription">Text to be set as a new Description</param>
        /// <returns>A status code indicating the result of the operation.</returns>
        [HttpPut("/Timer/ChangeById")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public ActionResult ChangeById([Required]long id, string? newName = null, int? newTimerAmount = null)
        {
            var oldData = _databaseContext.timers.FirstOrDefault(u => u.Id == id);

            if (oldData == null)
            {
                return NotFound();
            }

            if(newName != null){
                oldData.Name = newName;
            }

            if(newTimerAmount != null){
                oldData.TimerAmount = (int)newTimerAmount;
            }

            _databaseContext.SaveChanges();
            return Ok();
        }


        /// <summary>
        /// Deletes a Timer.
        /// </summary>
        /// <param name="timerId">TimerId of the timer to delete.</param>
        /// <response code="200">Timer removed</response>
        /// <response code="404">Timer not found</response>
        /// <response code="409">Timer couldn't be removed</response>
        /// <response code="500">No Timer in the database</response>
        [HttpDelete]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public IActionResult Remove([FromQuery][Required] long timerId)
        {
            var entity = _databaseContext.timers.SingleOrDefault(u => u.Id == timerId);

            if (entity == null)
            {
                return NotFound();
            }

            try
            {
                var relatedRecords = _databaseContext.waitingLists.Where(wl => wl.TimerId == timerId);
                foreach (var record in relatedRecords)
                {
                    record.TimerId = 0;
                }

                _databaseContext.SaveChanges();

                 _databaseContext.timers.Remove(entity);
                _databaseContext.SaveChanges();
            }
            catch
            {
                return StatusCode(409);
            }

            return Ok();
        }
    }
}
