using Discord_Core.Database;
using Discord_Core.Database.Entities;
using System.Data.SQLite;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WaitingList = Discord_Core.Database.Entities.WaitingList;
namespace DiscordApi.Controllers
{
    /// <summary>
    /// Controller for managing waitingLists.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class WaitingListController : ControllerBase
    {
        private readonly ILogger<WaitingListController> _logger;
        private readonly DatabaseContext _databaseContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="WaitingListController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="dbContext">The database context.</param>
        public WaitingListController(ILogger<WaitingListController> logger, DatabaseContext dbContext)
        {
            _logger = logger;
            _databaseContext = dbContext;
        }

        /// <summary>
        /// Get a list of waitingLists.
        /// </summary>
        /// <response code="200">Successful operation</response>
        /// <response code="404">No buff found</response>
        /// <response code="500">No connection to the database</response>
        /// <returns>The list of waitingLists.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<WaitingList>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public ActionResult Get()
        {
            List<WaitingList> waitingLists = _databaseContext.waitingLists
                .Include(wl => wl.User)
                .Include(wl => wl.Buff)
                .Include(wl => wl.Timer)
                .ToList();

            if (waitingLists.Count() == 0)
            {
                return NotFound("No buff found");
            }

            return Ok(waitingLists);
        }

        /// <summary>
        /// Add a new WaitingList.
        /// </summary>
        /// <param name="buff">WaitingList to add.</param>
        /// <response code="200">Successful operation</response>
        /// <response code="409">WaitingList couldn't be added</response>
        /// <response code="500">No connection to the database</response>
        [HttpPost]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public ActionResult Add([FromBody] WaitingList buff)
        {
            try
            {
                _databaseContext.waitingLists.Add(buff);
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
        [HttpPut("/WaitingList/ChangeById")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public ActionResult ChangeById([Required]long id, long? userId = null, long? buffId = null, long? timerId = null)
        {
            var oldData = _databaseContext.waitingLists.FirstOrDefault(u => u.Id == id);

            if (oldData == null)
            {
                return NotFound();
            }

            if(userId != null){
                oldData.UserId = (long)userId;
            }

            if(buffId != null){
                oldData.BuffId = (long)buffId;
            }

            if(timerId != null){
                oldData.TimerId = (long)timerId;
            }

            _databaseContext.SaveChanges();
            return Ok();
        }


        /// <summary>
        /// Deletes a WaitingList.
        /// </summary>
        /// <param name="waitingListId">TimerId of the waitingList to delete.</param>
        /// <response code="200">WaitingList removed</response>
        /// <response code="404">WaitingList not found</response>
        /// <response code="409">WaitingList couldn't be removed</response>
        /// <response code="500">No WaitingList in the database</response>
        [HttpDelete]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public IActionResult Remove([FromQuery][Required] long waitingListId)
        {
            var entity = _databaseContext.waitingLists.SingleOrDefault(u => u.Id == waitingListId);

            if (entity == null)
            {
                return NotFound();
            }

            try
            {
                _databaseContext.waitingLists.Remove(entity);

                var relatedRecords = _databaseContext.waitingLists.Where(wl => wl.Id == waitingListId);
                _databaseContext.waitingLists.RemoveRange(relatedRecords);

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
