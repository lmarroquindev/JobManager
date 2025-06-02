namespace JobServer.Domain.Interfaces
{
    /// <summary>
    /// Defines the contract for executing and managing asynchronous jobs.
    /// </summary>
    public interface IJobExecutor
    {
        /// <summary>
        /// Starts a new asynchronous job of the specified type and name.
        /// </summary>
        /// <param name="jobType">The type of the job.</param>
        /// <param name="jobName">The name of the job.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the unique identifier of the started job.</returns>
        Task<Guid> StartJobAsync(string jobType, string jobName);

        /// <summary>
        /// Cancels a running job by its unique identifier.
        /// </summary>
        /// <param name="jobId">The unique identifier of the job to cancel.</param>
        /// <returns>A task that represents the asynchronous operation. The task result indicates whether the cancellation was successful.</returns>
        Task<bool> CancelJobAsync(Guid jobId);
    }
}