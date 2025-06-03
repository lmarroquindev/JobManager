using JobServer.Application.DTOs;
using JobServer.Application.Interfaces.Services;
using JobServer.Domain.Interfaces;

namespace JobServer.Application.Commands.CancelJob
{
    /// <summary>
    /// Defines the contract for handling the <see cref="CancelJobCommand"/>.
    /// </summary>
    public interface ICancelJobCommandHandler
    {
        /// <summary>
        /// Handles the cancellation of a job.
        /// </summary>
        /// <param name="command">The command containing the job ID to cancel.</param>
        /// <returns>A task that represents the asynchronous operation. The task result indicates whether the cancellation was successful.</returns>
        Task<bool> HandleAsync(CancelJobCommand command);
    }

    /// <inheritdoc />
    public class CancelJobCommandHandler : ICancelJobCommandHandler
    {
        private readonly IJobCommandRepository _executor;
        private readonly IJobNotifierService _notifier;
        private readonly IJobQueryRepository _queryService;

        public CancelJobCommandHandler(IJobCommandRepository executor, IJobNotifierService notifier, IJobQueryRepository queryService)
        {
            _executor = executor;
            _notifier = notifier;
            _queryService = queryService;
        }

        /// <inheritdoc />
        public async Task<bool> HandleAsync(CancelJobCommand command)
        {
            var job = await _queryService.GetJobByIdAsync(command.JobId);

            if (job == null || !job.IsRunning)
            {
                return false;
            }

            var canceled = await _executor.CancelJobAsync(command.JobId);

            if (canceled)
            {
                await _notifier.NotifyAsync(new JobNotificationDto(
                    "JobCancelled",
                    job.Id,
                    job.JobType
                ));

                return true;
            }

            return false;
        }
    }
}