using JobServer.Application.DTOs;
using JobServer.Application.Interfaces;
using JobServer.Application.Interfaces.Services;

namespace JobServer.Infrastructure.Services
{
    /// <inheritdoc />
    public class JobNotifierService : IJobNotifierService
    {
        private readonly IWebSocketService _webSocketService;

        public JobNotifierService(IWebSocketService webSocketService)
        {
            _webSocketService = webSocketService;
        }

        /// <inheritdoc />
        public async Task NotifyAsync(JobNotificationDto notification)
        {
            await _webSocketService.BroadcastMessageAsync(new WebSocketMessageDto(
                notification.EventType,
                notification.JobId,
                notification.JobType
            ));
        }
    }
}
