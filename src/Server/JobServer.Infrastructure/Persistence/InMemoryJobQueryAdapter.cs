using JobServer.Domain.Entities;
using JobServer.Domain.Interfaces;
using System.Collections.Concurrent;

namespace JobServer.Infrastructure.Persistence
{
    /// <inheritdoc />
    public class InMemoryJobQueryAdapter : IJobQueryService
    {
        private readonly ConcurrentDictionary<Guid, Job> _jobs;

        public InMemoryJobQueryAdapter(ConcurrentDictionary<Guid, Job> jobs)
        {
            _jobs = jobs;
        }

        /// <inheritdoc />
        public Task<IEnumerable<Job>> GetAllJobsAsync()
        {
            return Task.FromResult(_jobs.Values.AsEnumerable());
        }

        /// <inheritdoc />
        public Task<string> GetJobStatusAsync(Guid jobId)
        {
            if (_jobs.TryGetValue(jobId, out var job))
            {
                return Task.FromResult(job.IsRunning ? "Running" : "Not Running");
            }

            return Task.FromResult("Not Found");
        }
    }
}
