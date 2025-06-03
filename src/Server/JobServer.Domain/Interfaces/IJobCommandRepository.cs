using JobServer.Domain.Entities;

namespace JobServer.Domain.Interfaces
{
    /// <summary>
    /// Defines the contract for managing job persistence and state.
    /// Responsible for adding jobs, marking them as completed, and cancelling jobs.
    /// </summary>
    public interface IJobCommandRepository
    {
        /// <summary>
        /// Adds a new job to the persistence store.
        /// </summary>
        /// <param name="job">The job entity to add.</param>
        /// <returns>The unique identifier of the added job.</returns>
        Task<Guid> AddJobAsync(Job job);

        /// <summary>
        /// Marks a job as completed in the persistence store.
        /// </summary>
        /// <param name="jobId">The unique identifier of the job to mark as completed.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task MarkJobAsCompletedAsync(Guid jobId);

        /// <summary>
        /// Cancels a running job if it exists and is running.
        /// </summary>
        /// <param name="jobId">The unique identifier of the job to cancel.</param>
        /// <returns>True if the job was successfully cancelled; otherwise false.</returns>
        Task<bool> CancelJobAsync(Guid jobId);
    }
}
