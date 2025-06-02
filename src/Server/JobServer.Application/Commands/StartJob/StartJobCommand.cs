namespace JobServer.Application.Commands.StartJob
{
    /// <summary>
    /// Represents the command to start a new asynchronous job.
    /// </summary>
    public class StartJobCommand
    {
        public StartJobCommand(string jobType, string jobName)
        {
            JobType = jobType;
            JobName = jobName;
        }

        /// <summary>
        /// Gets or sets the type of the job to be executed.
        /// </summary>
        public string JobType { get; set; }

        /// <summary>
        /// Gets or sets the name of the job to be executed.
        /// </summary>
        public string JobName { get; set; }
    }
}
