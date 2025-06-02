using JobServer.Domain.Interfaces;

namespace JobServer.Application.Commands.StartJob
{
    /// <summary>
    /// Defines the contract for handling the <see cref="StartJobCommand"/>.
    /// </summary>
    public interface IStartJobCommandHandler
    {
        /// <summary>
        /// Handles the execution of the <see cref="StartJobCommand"/>.
        /// </summary>
        /// <param name="command">The command containing job details.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the unique identifier of the started job.</returns>
        Task<Guid> HandleAsync(StartJobCommand command);
    }

    /// <inheritdoc />
    public class StartJobCommandHandler : IStartJobCommandHandler
    {
        private readonly IJobExecutor _jobExecutor;

        public StartJobCommandHandler(IJobExecutor jobExecutor)
        {
            _jobExecutor = jobExecutor;
        }

        /// <inheritdoc />
        public async Task<Guid> HandleAsync(StartJobCommand command)
        {
            return await _jobExecutor.StartJobAsync(command.JobType, command.JobName);
        }
    }
}