using System.ComponentModel.DataAnnotations;

namespace JobServer.API.Models.Requests
{
    /// <summary>
    /// Represents the input data required to cancel a job.
    /// </summary>
    public class CancelJobRequestDto
    {
        /// <summary>
        /// The unique identifier of the job to cancel.
        /// </summary>
        [Required(ErrorMessage = "JobId is required.")]
        public Guid JobId { get; set; }
    }
}
