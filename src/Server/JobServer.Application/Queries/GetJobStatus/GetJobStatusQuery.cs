namespace JobServer.Application.Queries.GetJobStatus
{
    /// <summary>
    /// Represents the query to retrieve the status of a specific job.
    /// </summary>
    public class GetJobStatusQuery
    {
        public GetJobStatusQuery(Guid jobId)
        {
            JobId = jobId;
        }
        /// <summary>
        /// Gets or sets the unique identifier of the job.
        /// </summary>
        public Guid JobId { get; set; }
    }
}
