using JobServer.Domain.Interfaces;

namespace JobServer.Application.Queries.GetJobStatus
{
    /// <summary>
    /// Defines the contract for handling the <see cref="GetJobStatusQuery"/>.
    /// </summary>
    public interface IGetJobStatusQueryHandler
    {
        /// <summary>
        /// Handles the query to retrieve the status of a specific job.
        /// </summary>
        /// <param name="query">The query containing the job ID.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the job status as a string.</returns>
        Task<string> HandleAsync(GetJobStatusQuery query);
    }

    /// <inheritdoc />
    public class GetJobStatusQueryHandler : IGetJobStatusQueryHandler
    {
        private readonly IJobQueryService _queryService;

        public GetJobStatusQueryHandler(IJobQueryService queryService)
        {
            _queryService = queryService;
        }

        /// <inheritdoc />
        public Task<string> HandleAsync(GetJobStatusQuery query)
        {
            return _queryService.GetJobStatusAsync(query.JobId);
        }
    }
}
