namespace JobServer.Application.DTOs
{
    /// <summary>
    /// Represents a message to be broadcasted to WebSocket clients.
    /// </summary>
    public class WebSocketMessageDto
    {
        public WebSocketMessageDto(string eventType, Guid jobId, string jobType)
        {
            EventType = eventType;
            JobId = jobId;
            JobType = jobType;
        }

        /// <summary>
        /// Gets or sets the type of the event (e.g., started, completed, canceled).
        /// </summary>
        public string EventType { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the job.
        /// </summary>
        public Guid JobId { get; set; }

        /// <summary>
        /// Gets or sets the type of the job.
        /// </summary>
        public string JobType { get; set; }
    }
}