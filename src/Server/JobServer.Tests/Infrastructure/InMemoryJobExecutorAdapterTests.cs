using JobServer.Application.DTOs;
using JobServer.Application.Interfaces.Services;
using JobServer.Domain.Entities;
using JobServer.Infrastructure.Persistence;
using Moq;
using System.Collections.Concurrent;

namespace JobServer.Tests.Infrastructure
{
    public class InMemoryJobExecutorAdapterTests
    {
        [Fact]
        public async Task StartJobAsync_ShouldReturnJobId_WhenJobIsStartedSuccessfully()
        {
            // Arrange
            var jobs = new ConcurrentDictionary<Guid, Job>();
            var mockNotifier = new Mock<IJobNotifierService>();
            var adapter = new InMemoryJobExecutorAdapter(jobs, mockNotifier.Object);

            var jobType = "TestType";
            var jobName = "TestJob";

            mockNotifier
                .Setup(x => x.NotifyAsync(It.IsAny<JobNotificationDto>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await adapter.StartJobAsync(jobType, jobName);

            // Active wait until the job is marked as not running
            var timeout = TimeSpan.FromSeconds(20);
            var start = DateTime.UtcNow;
            while (jobs[result].IsRunning && DateTime.UtcNow - start < timeout)
            {
                await Task.Delay(100);
            }

            // Assert
            Assert.NotEqual(Guid.Empty, result);
            mockNotifier.Verify(x => x.NotifyAsync(It.Is<JobNotificationDto>(dto =>
                dto.EventType == "JobCompleted" && dto.JobId == result)), Times.Once);
        }

        [Fact]
        public async Task StartJobAsync_ShouldThrow_WhenLimitExceeded()
        {
            // Arrange
            var jobs = new ConcurrentDictionary<Guid, Job>();
            var mockNotifier = new Mock<IJobNotifierService>();
            var adapter = new InMemoryJobExecutorAdapter(jobs, mockNotifier.Object);

            var jobType = "LimitedType";

            // Simulates 5 active jobs
            var tasks = new List<Task>();
            for (int i = 0; i < 5; i++)
            {
                tasks.Add(adapter.StartJobAsync(jobType, $"Job{i}"));
            }

            // Wait for the 5 jobs to be registered
            await Task.WhenAll(tasks);

            // Act & Assert: the sixth must throw an exception
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                adapter.StartJobAsync(jobType, "OverflowJob"));

            Assert.Contains("Maximum of 5 concurrent jobs", ex.Message);
        }

        [Fact]
        public async Task CancelJobAsync_ShouldReturnTrue_WhenJobIsCancelledSuccessfully()
        {
            // Arrange
            var jobs = new ConcurrentDictionary<Guid, Job>();
            var mockNotifier = new Mock<IJobNotifierService>();
            var adapter = new InMemoryJobExecutorAdapter(jobs, mockNotifier.Object);

            var jobType = "TestType";
            var jobName = "TestJob";
            var jobId = await adapter.StartJobAsync(jobType, jobName);

            // Act
            var result = await adapter.CancelJobAsync(jobId);

            // Assert
            Assert.True(result);
            mockNotifier.Verify(x => x.NotifyAsync(It.Is<JobNotificationDto>(dto => dto.EventType == "JobCancelled" && dto.JobId == jobId)), Times.Once);
        }

        [Fact]
        public async Task CancelJobAsync_ShouldReturnFalse_WhenJobDoesNotExistOrIsNotRunning()
        {
            // Arrange
            var jobs = new ConcurrentDictionary<Guid, Job>();
            var mockNotifier = new Mock<IJobNotifierService>();
            var adapter = new InMemoryJobExecutorAdapter(jobs, mockNotifier.Object);

            var jobId = Guid.NewGuid();

            // Act
            var result = await adapter.CancelJobAsync(jobId);

            // Assert
            Assert.False(result);
            mockNotifier.Verify(x => x.NotifyAsync(It.IsAny<JobNotificationDto>()), Times.Never);
        }
    }
}
