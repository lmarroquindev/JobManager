namespace JobServer.Application.Commands.CancelJob
{
    /// <summary>
    /// Represents the command to cancel a running job.
    /// </summary>
    public class CancelJobCommand
    {
        public CancelJobCommand(Guid jobId)
        {
            JobId = jobId;
        }

        /// <summary>
        /// Gets or sets the unique identifier of the job to cancel.
        /// </summary>
        public Guid JobId { get; set; }
    }
}
