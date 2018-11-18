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
    [Produces("application/json")]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

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

    }
}
