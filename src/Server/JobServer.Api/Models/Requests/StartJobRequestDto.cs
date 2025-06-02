using System.ComponentModel.DataAnnotations;

namespace JobServer.API.Models.Requests
{
    /// <summary>
    /// Represents the input data required to start a new asynchronous job.
    /// </summary>
    public class StartJobRequestDto
    {
        /// <summary>
        /// The type of the job to be started. Must be at least 3 characters long.
        /// </summary>
        [Required(ErrorMessage = "JobType is required.")]
        [MinLength(3, ErrorMessage = "JobType must be at least 3 characters.")]
        public string JobType { get; set; }

        /// <summary>
        /// The name of the job to be started. Must be at least 3 characters long.
        /// </summary>
        [Required(ErrorMessage = "JobName is required.")]
        [MinLength(3, ErrorMessage = "JobName must be at least 3 characters.")]
        public string JobName { get; set; }
    }
}

