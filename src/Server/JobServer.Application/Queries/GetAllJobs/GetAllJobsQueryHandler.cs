using JobServer.Application.DTOs;
using JobServer.Domain.Interfaces;

namespace JobServer.Application.Queries.GetAllJobs
{
    /// <summary>
    /// Defines the contract for handling the <see cref="GetAllJobsQuery"/>.
    /// </summary>
    public interface IGetAllJobsQueryHandler
    {
        /// <summary>
        /// Handles the query to retrieve all jobs.
        /// </summary>
        /// <param name="query">The query object.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of job DTOs.</returns>
        Task<IEnumerable<JobDto>> HandleAsync(GetAllJobsQuery query);
    }

    /// <inheritdoc />
    public class GetAllJobsQueryHandler : IGetAllJobsQueryHandler
    {
        private readonly IJobQueryRepository _jobQueryService;

        public GetAllJobsQueryHandler(IJobQueryRepository jobQueryService)
        {
            _jobQueryService = jobQueryService;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<JobDto>> HandleAsync(GetAllJobsQuery query)
        {
            var jobs = await _jobQueryService.GetAllJobsAsync();

            return jobs.Select(job => new JobDto
            {
                Id = job.Id,
                JobType = job.JobType,
                JobName = job.JobName,
                Status = job.IsRunning ? "Running" : "Not Running"
            });
        }
    }
}
