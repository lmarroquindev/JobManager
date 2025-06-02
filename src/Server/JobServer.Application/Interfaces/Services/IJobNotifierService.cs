using JobServer.Application.DTOs;

namespace JobServer.Application.Interfaces.Services
{
    /// <summary>
    /// Defines a contract for notifying clients about job-related events.
    /// </summary>
    public interface IJobNotifierService
    {
        /// <summary>
        /// Sends a notification about a job event to connected clients.
        /// </summary>
        /// <param name="notification">The job notification data.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task NotifyAsync(JobNotificationDto notification);
    }
}
