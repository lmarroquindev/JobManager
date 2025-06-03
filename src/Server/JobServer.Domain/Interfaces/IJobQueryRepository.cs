using JobServer.Domain.Entities;

namespace JobServer.Domain.Interfaces
{
    /// <summary>
    /// Provides methods to query job information from the system.
    /// </summary>
    public interface IJobQueryRepository
    {
        /// <summary>
        /// Retrieves all jobs currently tracked by the system.
        /// </summary>
        /// <returns>A collection of all jobs.</returns>
        Task<IEnumerable<Job>> GetAllJobsAsync();

        /// <summary>
        /// Retrieves the current status of a specific job.
        /// </summary>
        /// <param name="jobId">The unique identifier of the job.</param>
        /// <returns>A string representing the job status.</returns>
        Task<string> GetJobStatusAsync(Guid jobId);

        /// <summary>
        /// Retrieves a job by its unique identifier.
        /// </summary>
        /// <param name="jobId">The unique identifier of the job.</param>
        /// <returns>The job if found; otherwise, null.</returns>
        Task<Job?> GetJobByIdAsync(Guid jobId);
    }
}
