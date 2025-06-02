using JobServer.Domain.Entities;
using JobServer.Infrastructure.Persistence;
using System.Collections.Concurrent;

namespace JobServer.Tests.Infrastructure
{
    public class InMemoryJobQueryAdapterTests
    {
        [Fact]
        public async Task GetAllJobsAsync_ShouldReturnAllJobs_WhenJobsExist()
        {
            // Arrange
            var jobs = new ConcurrentDictionary<Guid, Job>();
            var job1 = new Job("TypeA", "Job1") { IsRunning = true };
            var job2 = new Job("TypeB", "Job2") { IsRunning = false };
            jobs.TryAdd(job1.Id, job1);
            jobs.TryAdd(job2.Id, job2);

            var adapter = new InMemoryJobQueryAdapter(jobs);

            // Act
            var result = (await adapter.GetAllJobsAsync()).ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, job => job.Id == job1.Id && job.IsRunning);
            Assert.Contains(result, job => job.Id == job2.Id && !job.IsRunning);
        }

        [Fact]
        public async Task GetAllJobsAsync_ShouldReturnEmptyList_WhenNoJobsExist()
        {
            // Arrange
            var jobs = new ConcurrentDictionary<Guid, Job>();
            var adapter = new InMemoryJobQueryAdapter(jobs);

            // Act
            var result = await adapter.GetAllJobsAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetJobStatusAsync_ShouldReturnRunning_WhenJobIsRunning()
        {
            // Arrange
            var jobs = new ConcurrentDictionary<Guid, Job>();
            var job = new Job("TypeA", "Job1") { IsRunning = true };
            jobs.TryAdd(job.Id, job);

            var adapter = new InMemoryJobQueryAdapter(jobs);

            // Act
            var result = await adapter.GetJobStatusAsync(job.Id);

            // Assert
            Assert.Equal("Running", result);
        }

        [Fact]
        public async Task GetJobStatusAsync_ShouldReturnNotRunning_WhenJobIsNotRunning()
        {
            // Arrange
            var jobs = new ConcurrentDictionary<Guid, Job>();
            var job = new Job("TypeA", "Job1") { IsRunning = false };
            jobs.TryAdd(job.Id, job);

            var adapter = new InMemoryJobQueryAdapter(jobs);

            // Act
            var result = await adapter.GetJobStatusAsync(job.Id);

            // Assert
            Assert.Equal("Not Running", result);
        }

        [Fact]
        public async Task GetJobStatusAsync_ShouldReturnNotFound_WhenJobDoesNotExist()
        {
            // Arrange
            var jobs = new ConcurrentDictionary<Guid, Job>();
            var adapter = new InMemoryJobQueryAdapter(jobs);

            var jobId = Guid.NewGuid();

            // Act
            var result = await adapter.GetJobStatusAsync(jobId);

            // Assert
            Assert.Equal("Not Found", result);
        }
    }
}
