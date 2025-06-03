using JobServer.API.Models.Requests;
using JobServer.Application.Commands.CancelJob;
using JobServer.Application.Commands.StartJob;
using JobServer.Application.Queries.GetAllJobs;
using JobServer.Application.Queries.GetJobStatus;
using Microsoft.AspNetCore.Mvc;

namespace JobServer.API.Controllers
{
    /// <summary>
    /// Handles job-related operations such as starting, cancelling, and querying jobs.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IStartJobCommandHandler _startJobHandler;
        private readonly ICancelJobCommandHandler _cancelJobHandler;
        private readonly IGetAllJobsQueryHandler _getAllJobsHandler;
        private readonly IGetJobStatusQueryHandler _getJobStatusHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobsController"/> class.
        /// </summary>
        public JobsController(
            IStartJobCommandHandler startJobHandler,
            ICancelJobCommandHandler cancelJobHandler,
            IGetAllJobsQueryHandler getAllJobsHandler,
            IGetJobStatusQueryHandler getJobStatusHandler)
        {
            _startJobHandler = startJobHandler;
            _cancelJobHandler = cancelJobHandler;
            _getAllJobsHandler = getAllJobsHandler;
            _getJobStatusHandler = getJobStatusHandler;
        }

        /// <summary>
        /// Retrieves all jobs currently tracked by the system.
        /// </summary>
        /// <returns>A list of all jobs.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllJobs()
        {
            var result = await _getAllJobsHandler.HandleAsync(new GetAllJobsQuery());
            return Ok(result);
        }

        /// <summary>
        /// Retrieves the status of a specific job by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the job.</param>
        /// <returns>The status of the job.</returns>
        [HttpGet("job-status/{id}")]
        public async Task<IActionResult> GetJobStatus(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Invalid JobId.");

            var result = await _getJobStatusHandler.HandleAsync(new GetJobStatusQuery(id));

            if (result == "Not Found")
                return NotFound();

            return Ok(new { JobId = id, Status = result });
        }

        /// <summary>
        /// Starts a new job with the specified type and name.
        /// </summary>
        /// <param name="request">The job type and name.</param>
        /// <returns>The ID of the newly created job.</returns>
        [HttpPost]
        public IActionResult StartJob([FromBody] StartJobRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            StartJobCommand command = new StartJobCommand(request.JobType,request.JobName);

            var jobId = _startJobHandler.HandleAsync(command);
            return Ok(new { JobId = jobId });
        }


        /// <summary>
        /// Cancels a running job by its ID.
        /// </summary>
        /// <param name="request">The ID of the job to cancel.</param>
        /// <returns>A message indicating the result of the cancellation.</returns>
        [HttpPost("cancel-job")]
        public async Task<IActionResult> CancelJob([FromBody] CancelJobRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            CancelJobCommand command = new CancelJobCommand(request.JobId);

            var result = await _cancelJobHandler.HandleAsync(command);

            if (!result)
                return NotFound(new { error = "Job not found or already completed." });

            return Ok(new { Message = "Job cancelled successfully." });
        }

        /// <summary>
        /// Test endpoint that throws an unhandled exception to verify global error handling.
        /// </summary>
        /// <returns>Always throws an exception.</returns>
        /// <response code="500">Returned when an unhandled exception occurs.</response>
        [HttpGet("test-error")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ThrowError()
        {
            throw new Exception("This is a test exception.");
        }
    }
}
