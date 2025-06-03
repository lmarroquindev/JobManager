using JobServer.Domain.Entities;
using JobServer.Infrastructure.Persistence;
using System.Collections.Concurrent;

namespace JobServer.Tests.Infrastructure
{
    public class InMemoryJobCommandRepositoryTests
    {
        [Fact]
        public async Task AddJobAsync_ShouldReturnJobId_WhenJobIsAddedSuccessfully()
        {
            // Arrange
            var jobs = new ConcurrentDictionary<Guid, Job>();
            var executor = new InMemoryJobCommandRepository(jobs);

            var job = new Job("TestType", "TestJob");

            // Act
            var result = await executor.AddJobAsync(job);

            // Assert
            Assert.NotEqual(Guid.Empty, result);
            Assert.True(jobs.ContainsKey(result));
            Assert.Equal(job, jobs[result]);
        }

        [Fact]
        public async Task MarkJobAsCompletedAsync_ShouldMarkJobAsNotRunning()
        {
            // Arrange
            var jobs = new ConcurrentDictionary<Guid, Job>();
            var executor = new InMemoryJobCommandRepository(jobs);

            var job = new Job("TestType", "TestJob");
            jobs.TryAdd(job.Id, job);

            // Act
            await executor.MarkJobAsCompletedAsync(job.Id);

            // Assert
            Assert.False(jobs[job.Id].IsRunning);
        }

        [Fact]
        public async Task CancelJobAsync_ShouldReturnTrue_WhenJobIsCancelledSuccessfully()
        {
            // Arrange
            var jobs = new ConcurrentDictionary<Guid, Job>();
            var executor = new InMemoryJobCommandRepository(jobs);

            var job = new Job("TestType", "TestJob");
            jobs.TryAdd(job.Id, job);

            // Simulate job is running and has cancellation token source
            job.IsRunning = true;
            job.CancellationTokenSource = new System.Threading.CancellationTokenSource();

            // Act
            var result = await executor.CancelJobAsync(job.Id);

            // Assert
            Assert.True(result);
            Assert.False(jobs[job.Id].IsRunning);
            Assert.True(jobs[job.Id].CancellationTokenSource.IsCancellationRequested);
        }

        [Fact]
        public async Task CancelJobAsync_ShouldReturnFalse_WhenJobDoesNotExistOrIsNotRunning()
        {
            // Arrange
            var jobs = new ConcurrentDictionary<Guid, Job>();
            var executor = new InMemoryJobCommandRepository(jobs);

            var jobId = System.Guid.NewGuid();

            // Act
            var result = await executor.CancelJobAsync(jobId);

            // Assert
            Assert.False(result);
        }
    }
}
