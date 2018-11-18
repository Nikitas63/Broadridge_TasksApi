using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TasksApi.Dto;
using TasksApi.Errors;
using TasksApi.Pagination;

namespace TasksApi.Controllers
{
    /// <summary>
    /// Tasks controller.
    /// </summary>
    [Produces("application/json")]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes new instance of TasksController.
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        public TasksController(IMediator mediator, ILogger<TasksController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Gets the available tasks list.
        /// </summary>
        /// <param name="query">Query.</param>
        /// <returns>Available tasks list.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(Page<TaskDetails>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTasks(List.Query query)
        {
            _logger.LogInformation("Start GetTasks query");

            var tasks = await _mediator.Send(query);

            _logger.LogInformation("End GetTasks query");

            return Ok(tasks);
        }

        /// <summary>
        /// Creates new task.
        /// </summary>
        /// <param name="query">Query.</param>
        /// <returns>Created task.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Page<TaskDetails>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateTask(Post.Command query)
        {
            _logger.LogInformation("Start CreateTask query");

            var task = await _mediator.Send(query);

            _logger.LogInformation("End CreateTask query");

            return CreatedAtAction(null, new { TaskId = task.Id }, task);
        }

        /// <summary>
        /// Updates existing task.
        /// </summary>
        /// <param name="query">Query.</param>
        /// <returns>Created task.</returns>
        [HttpPut("{TaskId:guid}")]
        [ProducesResponseType(typeof(Page<TaskDetails>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateTask(Put.Command query)
        {
            _logger.LogInformation("Start CreateTask query");

            var task = await _mediator.Send(query);

            _logger.LogInformation("End CreateTask query");

            return CreatedAtAction(null, new { TaskId = task.Id }, task);
        }

        /// <summary>
        /// Deletes existing task.
        /// </summary>
        /// <param name="query">Query.</param>
        /// <returns>Created task.</returns>
        [HttpDelete("{TaskId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteTask(Delete.Command query)
        {
            _logger.LogInformation("Start DeleteTask query");

            await _mediator.Send(query);

            _logger.LogInformation("End DeleteTask query");

            return NoContent();
        }

    }
}
