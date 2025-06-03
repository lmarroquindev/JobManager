using JobServer.Domain.Entities;
using JobServer.Domain.Interfaces;
using System.Collections.Concurrent;

namespace JobServer.Infrastructure.Persistence
{
    /// <inheritdoc />
    public class InMemoryJobCommandRepository : IJobCommandRepository
    {
        private readonly ConcurrentDictionary<Guid, Job> _jobs;

        public InMemoryJobCommandRepository(ConcurrentDictionary<Guid, Job> jobs)
        {
            _jobs = jobs;
        }

        /// <inheritdoc />
        public Task<Guid> AddJobAsync(Job job)
        {
            _jobs.TryAdd(job.Id, job);
            return Task.FromResult(job.Id);
        }

        /// <inheritdoc />
        public Task MarkJobAsCompletedAsync(Guid jobId)
        {
            if (_jobs.TryGetValue(jobId, out var job))
            {
                job.MarkAsCompleted();
            }
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public async Task<bool> CancelJobAsync(Guid jobId)
        {
            if (_jobs.TryGetValue(jobId, out var job) && job.IsRunning)
            {
                job.CancellationTokenSource.Cancel();
                job.IsRunning = false;
                return true;
            }
            return false;
        }
    }
}
