using JobServer.Domain.Interfaces;

namespace JobServer.Application.Commands.CancelJob
{
    /// <summary>
    /// Defines the contract for handling the <see cref="CancelJobCommand"/>.
    /// </summary>
    public interface ICancelJobCommandHandler
    {
        /// <summary>
        /// Handles the cancellation of a job.
        /// </summary>
        /// <param name="command">The command containing the job ID to cancel.</param>
        /// <returns>A task that represents the asynchronous operation. The task result indicates whether the cancellation was successful.</returns>
        Task<bool> HandleAsync(CancelJobCommand command);
    }

    /// <inheritdoc />
    public class CancelJobCommandHandler : ICancelJobCommandHandler
    {
        private readonly IJobExecutor _executor;

        public CancelJobCommandHandler(IJobExecutor executor)
        {
            _executor = executor;
        }

        /// <inheritdoc />
        public Task<bool> HandleAsync(CancelJobCommand command)
        {
            return _executor.CancelJobAsync(command.JobId);
        }
    }
}
