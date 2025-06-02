using JobServer.Application.Queries.GetAllJobs;
using JobServer.Domain.Entities;
using JobServer.Domain.Interfaces;
using Moq;

namespace JobServer.Tests.Features.GetAllJobs
{
    public class GetAllJobsQueryHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldReturnMappedJobDtos_WhenJobsExist()
        {
            // Arrange
            var jobs = new List<Job>
            {
                new Job("TypeA", "Job1") { IsRunning = true },
                new Job("TypeB", "Job2") { IsRunning = false }
            };

            var mockQueryService = new Mock<IJobQueryService>();
            mockQueryService
                .Setup(x => x.GetAllJobsAsync())
                .ReturnsAsync(jobs);

            var handler = new GetAllJobsQueryHandler(mockQueryService.Object);
            var query = new GetAllJobsQuery();

            // Act
            var result = (await handler.HandleAsync(query)).ToList();

            // Assert
            Assert.Equal(2, result.Count);

            Assert.Equal(jobs[0].Id, result[0].Id);
            Assert.Equal("Running", result[0].Status);

            Assert.Equal(jobs[1].Id, result[1].Id);
            Assert.Equal("Not Running", result[1].Status);

            mockQueryService.Verify(x => x.GetAllJobsAsync(), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnEmptyList_WhenNoJobsExist()
        {
            // Arrange
            var mockQueryService = new Mock<IJobQueryService>();
            mockQueryService
                .Setup(x => x.GetAllJobsAsync())
                .ReturnsAsync(new List<Job>());

            var handler = new GetAllJobsQueryHandler(mockQueryService.Object);
            var query = new GetAllJobsQuery();

            // Act
            var result = await handler.HandleAsync(query);

            // Assert
            Assert.Empty(result);
            mockQueryService.Verify(x => x.GetAllJobsAsync(), Times.Once);
        }
    }
}
