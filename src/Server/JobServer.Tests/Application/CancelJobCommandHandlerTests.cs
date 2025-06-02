using JobServer.Application.Commands.CancelJob;
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
            var jobId = Guid.NewGuid();
            var command = new CancelJobCommand(jobId);

            var mockExecutor = new Mock<IJobExecutor>();
            mockExecutor
                .Setup(x => x.CancelJobAsync(jobId))
                .ReturnsAsync(true);

            var handler = new CancelJobCommandHandler(mockExecutor.Object);

            // Act
            var result = await handler.HandleAsync(command);

            // Assert
            Assert.True(result);
            mockExecutor.Verify(x => x.CancelJobAsync(jobId), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFalse_WhenJobDoesNotExistOrIsNotRunning()
        {
            // Arrange
            var jobId = Guid.NewGuid();
            var command = new CancelJobCommand(jobId);

            var mockExecutor = new Mock<IJobExecutor>();
            mockExecutor
                .Setup(x => x.CancelJobAsync(jobId))
                .ReturnsAsync(false);

            var handler = new CancelJobCommandHandler(mockExecutor.Object);

            // Act
            var result = await handler.HandleAsync(command);

            // Assert
            Assert.False(result);
            mockExecutor.Verify(x => x.CancelJobAsync(jobId), Times.Once);
        }
    }
}
