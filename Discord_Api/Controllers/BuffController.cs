using Discord_Core.Database;
using Discord_Core.Database.Entities;
using System.Data.SQLite;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DiscordApi.Controllers
{
    /// <summary>
    /// Controller for managing buffs.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class BuffController : ControllerBase
    {
        private readonly ILogger<BuffController> _logger;
        private readonly DatabaseContext _databaseContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuffController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="dbContext">The database context.</param>
        public BuffController(ILogger<BuffController> logger, DatabaseContext dbContext)
        {
            _logger = logger;
            _databaseContext = dbContext;
        }

        /// <summary>
        /// Get a list of buffs.
        /// </summary>
        /// <response code="200">Successful operation</response>
        /// <response code="404">No buff found</response>
        /// <response code="500">No connection to the database</response>
        /// <returns>The list of buffs.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Buff>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public ActionResult GetBuffs()
        {
            List<Buff> buffs = _databaseContext.buffs.ToList();

            if (buffs.Count() == 0)
            {
                return NotFound("No buff found");
            }

            return Ok(buffs);
        }

        /// <summary>
        /// Add a new Buff.
        /// </summary>
        /// <param name="buff">Buff to add.</param>
        /// <response code="200">Successful operation</response>
        /// <response code="409">User couldn't be added</response>
        /// <response code="500">No connection to the database</response>
        [HttpPost]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public ActionResult Add([FromBody] Buff buff)
        {
            if (_databaseContext.buffs.Any(u => u.Name == buff.Name))
            {
                return BadRequest("The Name already exists.");
            }

            try
            {
                _databaseContext.buffs.Add(buff);
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
        [HttpPut("/ChangeById")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public ActionResult ChangeById([Required]long id, string? newName = null, string? newDescription = null)
        {
            var oldData = _databaseContext.buffs.FirstOrDefault(u => u.Id == id);

            if (oldData == null)
            {
                return NotFound();
            }

            if(newName != null){
                oldData.Name = newName;
            }

            if(newDescription != null){
                oldData.Description = newDescription;
            }

            _databaseContext.SaveChanges();
            return Ok();
        }


        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="buffId">BuffId of the buff to delete.</param>
        /// <response code="200">User removed</response>
        /// <response code="404">User not found</response>
        /// <response code="409">User couldn't be removed</response>
        /// <response code="500">No user in the database</response>
        [HttpDelete]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public IActionResult Remove([FromQuery][Required] long buffId)
        {
            var entity = _databaseContext.buffs.SingleOrDefault(u => u.Id == buffId);

            if (entity == null)
            {
                return NotFound();
            }

            try
            {
                _databaseContext.buffs.Remove(entity);
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
