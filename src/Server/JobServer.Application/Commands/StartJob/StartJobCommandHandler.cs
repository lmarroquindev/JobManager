using JobServer.Application.DTOs;
using JobServer.Application.Interfaces.Services;
using JobServer.Domain.Entities;
using JobServer.Domain.Interfaces;
using System.Collections.Concurrent;

namespace JobServer.Application.Commands.StartJob
{
    /// <summary>
    /// Defines the contract for handling the <see cref="StartJobCommand"/>.
    /// Responsible for orchestrating job start logic including concurrency control and notifications.
    /// </summary>
    public interface IStartJobCommandHandler
    {
        /// <summary>
        /// Handles the execution of the <see cref="StartJobCommand"/>.
        /// </summary>
        /// <param name="command">The command containing job details.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the unique identifier of the started job.</returns>
        Task<Guid> HandleAsync(StartJobCommand command);
    }

    /// <inheritdoc />
    public class StartJobCommandHandler : IStartJobCommandHandler
    {
        private readonly IJobCommandRepository _jobExecutor;
        private readonly IJobNotifierService _notifier;
        private readonly ConcurrentDictionary<string, SemaphoreSlim> _semaphores = new();

        public StartJobCommandHandler(IJobCommandRepository jobExecutor, IJobNotifierService notifier)
        {
            _jobExecutor = jobExecutor;
            _notifier = notifier;
        }

        /// <inheritdoc />
        public async Task<Guid> HandleAsync(StartJobCommand command)
        {
            var semaphore = _semaphores.GetOrAdd(command.JobType, _ => new SemaphoreSlim(5, 5));

            if (!await semaphore.WaitAsync(0))
                throw new InvalidOperationException("Maximum concurrent jobs reached for this job type.");

            var job = new Job(command.JobType, command.JobName);

            await _jobExecutor.AddJobAsync(job);

            _ = Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(15));
                }
                finally
                {
                    await _jobExecutor.MarkJobAsCompletedAsync(job.Id);

                    await _notifier.NotifyAsync(new JobNotificationDto(
                        "JobCompleted",
                        job.Id,
                        job.JobType
                    ));

                    semaphore.Release();
                }
            });

            return job.Id;
        }
    }
}
