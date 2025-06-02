using JobServer.Application.DTOs;
using JobServer.Application.Interfaces.Services;
using JobServer.Domain.Entities;
using JobServer.Domain.Interfaces;
using System.Collections.Concurrent;

namespace JobServer.Infrastructure.Persistence
{
    /// <inheritdoc />
    public class InMemoryJobExecutorAdapter : IJobExecutor
    {
        private readonly ConcurrentDictionary<Guid, Job> _jobs;
        private readonly ConcurrentDictionary<string, SemaphoreSlim> _semaphores = new();
        private readonly IJobNotifierService _notifier;

        public InMemoryJobExecutorAdapter(
            ConcurrentDictionary<Guid, Job> jobs,
            IJobNotifierService notifier)
        {
            _jobs = jobs;
            _notifier = notifier;
        }

        /// <inheritdoc />
        public async Task<Guid> StartJobAsync(string jobType, string jobName)
        {
            var semaphore = _semaphores.GetOrAdd(jobType, jobTypeKey => new SemaphoreSlim(5, 5));

            if (!await semaphore.WaitAsync(0))
                throw new InvalidOperationException($"Maximum of 5 concurrent jobs allowed for job type '{jobType}'.");

            var job = new Job(jobType, jobName);
            _jobs.TryAdd(job.Id, job);

            _ = Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(15));
                }
                finally
                {
                    job.IsRunning = false;
                    semaphore.Release();

                    await _notifier.NotifyAsync(new JobNotificationDto(
                        "JobCompleted",
                        job.Id,
                        job.JobType
                    ));
                }
            });

            return job.Id;
        }

        /// <inheritdoc />
        public async Task<bool> CancelJobAsync(Guid jobId)
        {
            if (_jobs.TryGetValue(jobId, out var job) && job.IsRunning)
            {
                job.CancellationTokenSource.Cancel();

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
