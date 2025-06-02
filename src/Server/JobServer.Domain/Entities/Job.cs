namespace JobServer.Domain.Entities
{
    /// <summary>
    /// Represents a unit of work that can be executed asynchronously.
    /// </summary>
    public class Job
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Job"/> class with the specified type and name.
        /// </summary>
        /// <param name="jobType">The type/category of the job.</param>
        /// <param name="jobName">The descriptive name of the job.</param>
        public Job(string jobType, string jobName)
        {
            JobType = jobType;
            JobName = jobName;
        }

        /// <summary>
        /// Gets the unique identifier of the job.
        /// </summary>
        public Guid Id { get; private set; } = Guid.NewGuid();

        /// <summary>
        /// Gets the type/category of the job.
        /// </summary>
        public string JobType { get; private set; }

        /// <summary>
        /// Gets the descriptive name of the job.
        /// </summary>
        public string JobName { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the job is currently running.
        /// </summary>
        public bool IsRunning { get; set; } = true;

        /// <summary>
        /// Gets or sets the cancellation token source used to cancel the job.
        /// </summary>
        public CancellationTokenSource CancellationTokenSource { get; set; } = new();
    }
}
