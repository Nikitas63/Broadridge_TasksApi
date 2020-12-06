using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TasksApi.Errors;
using TasksApi.Handlers;
using TasksApi.Pagination;
using TasksApi.Resources;

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
        /// Gets task by its id.
        /// </summary>
        /// <param name="query">Query.</param>
        /// <returns>Task details.</returns>
        [HttpGet("{TaskId:guid}")]
        [ProducesResponseType(typeof(Page<TaskDetails>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTask(Get.Query query)
        {
            _logger.LogInformation("Start GetTasks query");

            var task = await _mediator.Send(query);

            _logger.LogInformation("End GetTasks query");

            return Ok(task);
        }

        /// <summary>
        /// Creates new task.
        /// </summary>
        /// <param name="query">Query.</param>
        /// <returns>Created task.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Page<TaskDetails>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateTask(Create.Command query)
        {
            _logger.LogInformation("Start CreateTask query");

            var task = await _mediator.Send(query);
            
            //notification
            await _mediator.Publish(new Notification.Command {Message = "Email send about task creation, or fucking job run"});

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
        public async Task<IActionResult> UpdateTask(Update.Command query)
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
