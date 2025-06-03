using JobServer.Application.Commands.StartJob;
using JobServer.Application.DTOs;
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
        public async Task HandleAsync_ShouldReturnJobId_WhenJobIsStartedSuccessfully()
        {
            // Arrange
            var mockJobExecutor = new Mock<IJobCommandRepository>();
            var mockNotifier = new Mock<IJobNotifierService>();

            var command = new StartJobCommand("TestType", "TestName");

            Guid addedJobId = Guid.Empty;

            mockJobExecutor
                .Setup(x => x.AddJobAsync(It.IsAny<Job>()))
                .Returns<Job>(job =>
                {
                    addedJobId = job.Id;
                    return Task.FromResult(job.Id);
                });

            mockJobExecutor
                .Setup(x => x.MarkJobAsCompletedAsync(It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);

            mockNotifier
                .Setup(x => x.NotifyAsync(It.IsAny<JobNotificationDto>()))
                .Returns(Task.CompletedTask);

            var handler = new StartJobCommandHandler(mockJobExecutor.Object, mockNotifier.Object);

            // Act
            var result = await handler.HandleAsync(command);

            // Assert
            Assert.Equal(addedJobId, result);
            mockJobExecutor.Verify(x => x.AddJobAsync(It.IsAny<Job>()), Times.Once);
            mockNotifier.Verify(x => x.NotifyAsync(It.IsAny<JobNotificationDto>()), Times.Never);

            // Wait for background task to complete notification to verify it runs
            await Task.Delay(16000); // wait more than 15 sec delay inside handler

            mockJobExecutor.Verify(x => x.MarkJobAsCompletedAsync(addedJobId), Times.Once);
            mockNotifier.Verify(x => x.NotifyAsync(It.Is<JobNotificationDto>(dto => dto.JobId == addedJobId && dto.EventType == "JobCompleted")), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_ShouldThrow_WhenMoreThanFiveJobsAreRunningForSameType()
        {
            // Arrange
            var jobs = new ConcurrentDictionary<Guid, Job>();
            var mockNotifier = new Mock<IJobNotifierService>();
            var executor = new InMemoryJobCommandRepository(jobs);

            var handler = new StartJobCommandHandler(executor, mockNotifier.Object);

            var jobType = "TestType";

            var commands = new List<StartJobCommand>();
            for (int i = 0; i < 5; i++)
            {
                commands.Add(new StartJobCommand(jobType, $"Job{i}"));
            }

            // Act - Start 5 jobs in parallel
            var tasks = commands.Select(c => handler.HandleAsync(c)).ToList();

            await Task.WhenAll(tasks);

            // Assert that starting a 6th job throws due to semaphore limit
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(new StartJobCommand(jobType, "OverflowJob")));

            Assert.Contains("Maximum concurrent jobs reached", exception.Message);
        }
    }
}