using JobServer.Application.Queries.GetJobStatus;
using JobServer.Domain.Interfaces;
using Moq;

namespace JobServer.Tests.Features.GetJobStatus
{
    public class GetJobStatusQueryHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldReturnRunning_WhenJobIsRunning()
        {
            // Arrange
            var jobId = Guid.NewGuid();
            var query = new GetJobStatusQuery(jobId);

            var mockQueryService = new Mock<IJobQueryRepository>();
            mockQueryService
                .Setup(x => x.GetJobStatusAsync(jobId))
                .ReturnsAsync("Running");

            var handler = new GetJobStatusQueryHandler(mockQueryService.Object);

            // Act
            var result = await handler.HandleAsync(query);

            // Assert
            Assert.Equal("Running", result);
            mockQueryService.Verify(x => x.GetJobStatusAsync(jobId), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnNotFound_WhenJobDoesNotExist()
        {
            // Arrange
            var jobId = Guid.NewGuid();
            var query = new GetJobStatusQuery(jobId);

            var mockQueryService = new Mock<IJobQueryRepository>();
            mockQueryService
                .Setup(x => x.GetJobStatusAsync(jobId))
                .ReturnsAsync("Not Found");

            var handler = new GetJobStatusQueryHandler(mockQueryService.Object);

            // Act
            var result = await handler.HandleAsync(query);

            // Assert
            Assert.Equal("Not Found", result);
            mockQueryService.Verify(x => x.GetJobStatusAsync(jobId), Times.Once);
        }
    }
}
