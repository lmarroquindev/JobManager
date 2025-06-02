using JobServer.Application.Commands.StartJob;
using JobServer.Application.Interfaces.Services;
using JobServer.Domain.Entities;
using JobServer.Domain.Interfaces;
using JobServer.Infrastructure.Persistence;
using Moq;
using System.Collections.Concurrent;

namespace JobServer.Tests.Features.StartJob
{
    public class StartJobCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnJobId_WhenJobIsStartedSuccessfully()
        {
            // Arrange
            var mockExecutor = new Mock<IJobExecutor>();
            var command = new StartJobCommand("TestType", "TestName");
            var expectedJobId = Guid.NewGuid();

            mockExecutor
                .Setup(x => x.StartJobAsync(command.JobType, command.JobName))
                .ReturnsAsync(expectedJobId);

            var handler = new StartJobCommandHandler(mockExecutor.Object);

            // Act
            var result = await handler.HandleAsync(command);

            // Assert
            Assert.Equal(expectedJobId, result);
        }


        [Fact]
        public async Task StartJobAsync_ShouldThrow_WhenMoreThanFiveJobsAreRunningForSameType()
        {
            // Arrange
            var jobs = new ConcurrentDictionary<Guid, Job>();
            var mockNotifier = new Mock<IJobNotifierService>();
            var adapter = new InMemoryJobExecutorAdapter(jobs, mockNotifier.Object);
            
            var jobType = "TestType";
            
            // Simulates 5 running jobs (do not release the semaphore)
            var tasks = new List<Task>();
            for (int i = 0; i < 5; i++)
            {
                tasks.Add(adapter.StartJobAsync(jobType, $"Job{i}"));
            }
            
            // Wait for the 5 jobs to be registered
            await Task.WhenAll(tasks);
            
            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => adapter.StartJobAsync(jobType, "OverflowJob"));
            
            Assert.Contains("Maximum of 5 concurrent jobs", ex.Message);
        }

    }
}