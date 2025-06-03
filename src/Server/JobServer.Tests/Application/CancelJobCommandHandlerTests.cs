using JobServer.Application.Commands.CancelJob;
using JobServer.Application.DTOs;
using JobServer.Application.Interfaces.Services;
using JobServer.Domain.Entities;
using JobServer.Domain.Interfaces;
using Moq;

namespace JobServer.Tests.Features.CancelJob
{
    public class CancelJobCommandHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldReturnTrue_WhenJobIsCancelledSuccessfully()
        {
            // Arrange
            var job = new Job("TestJobType", "TestJobName") { IsRunning = true };
            var command = new CancelJobCommand(job.Id);

            var mockExecutor = new Mock<IJobCommandRepository>();
            var mockNotifier = new Mock<IJobNotifierService>();
            var mockQueryService = new Mock<IJobQueryRepository>();

            mockQueryService
                .Setup(x => x.GetJobByIdAsync(job.Id))
                .ReturnsAsync(job);

            mockExecutor
                .Setup(x => x.CancelJobAsync(job.Id))
                .ReturnsAsync(true);

            mockNotifier
                .Setup(x => x.NotifyAsync(It.IsAny<JobNotificationDto>()))
                .Returns(Task.CompletedTask);

            var handler = new CancelJobCommandHandler(mockExecutor.Object, mockNotifier.Object, mockQueryService.Object);

            // Act
            var result = await handler.HandleAsync(command);

            // Assert
            Assert.True(result);
            mockQueryService.Verify(x => x.GetJobByIdAsync(job.Id), Times.Once);
            mockExecutor.Verify(x => x.CancelJobAsync(job.Id), Times.Once);
            mockNotifier.Verify(x => x.NotifyAsync(It.Is<JobNotificationDto>(dto =>
                dto.EventType == "JobCancelled" &&
                dto.JobId == job.Id &&
                dto.JobType == job.JobType)), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFalse_WhenJobDoesNotExist()
        {
            // Arrange
            var jobId = Guid.NewGuid();
            var command = new CancelJobCommand(jobId);

            var mockExecutor = new Mock<IJobCommandRepository>();
            var mockNotifier = new Mock<IJobNotifierService>();
            var mockQueryService = new Mock<IJobQueryRepository>();

            mockQueryService
                .Setup(x => x.GetJobByIdAsync(jobId))
                .ReturnsAsync((Job?)null);

            var handler = new CancelJobCommandHandler(mockExecutor.Object, mockNotifier.Object, mockQueryService.Object);

            // Act
            var result = await handler.HandleAsync(command);

            // Assert
            Assert.False(result);
            mockQueryService.Verify(x => x.GetJobByIdAsync(jobId), Times.Once);
            mockExecutor.Verify(x => x.CancelJobAsync(It.IsAny<Guid>()), Times.Never);
            mockNotifier.Verify(x => x.NotifyAsync(It.IsAny<JobNotificationDto>()), Times.Never);
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFalse_WhenJobIsNotRunning()
        {
            // Arrange
            var job = new Job("TestJobType", "TestJobName") { IsRunning = false };
            var command = new CancelJobCommand(job.Id);

            var mockExecutor = new Mock<IJobCommandRepository>();
            var mockNotifier = new Mock<IJobNotifierService>();
            var mockQueryService = new Mock<IJobQueryRepository>();

            mockQueryService
                .Setup(x => x.GetJobByIdAsync(job.Id))
                .ReturnsAsync(job);

            var handler = new CancelJobCommandHandler(mockExecutor.Object, mockNotifier.Object, mockQueryService.Object);

            // Act
            var result = await handler.HandleAsync(command);

            // Assert
            Assert.False(result);
            mockQueryService.Verify(x => x.GetJobByIdAsync(job.Id), Times.Once);
            mockExecutor.Verify(x => x.CancelJobAsync(It.IsAny<Guid>()), Times.Never);
            mockNotifier.Verify(x => x.NotifyAsync(It.IsAny<JobNotificationDto>()), Times.Never);
        }
    }
}
