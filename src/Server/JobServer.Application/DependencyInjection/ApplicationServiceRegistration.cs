using JobServer.Application.Commands.CancelJob;
using JobServer.Application.Commands.StartJob;
using JobServer.Application.Queries.GetAllJobs;
using JobServer.Application.Queries.GetJobStatus;
using Microsoft.Extensions.DependencyInjection;

namespace JobServer.Application.DependencyInjection
{
    public static class ApplicationServiceRegistration
    {
        /// <summary>
        /// Registers application services such as command and query handlers.
        /// </summary>
        /// <param name="services">The service collection to add services to.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Command Handlers
            services.AddSingleton<IStartJobCommandHandler, StartJobCommandHandler>();
            services.AddSingleton<ICancelJobCommandHandler, CancelJobCommandHandler>();

            // Query Handlers
            services.AddSingleton<IGetAllJobsQueryHandler, GetAllJobsQueryHandler>();
            services.AddSingleton<IGetJobStatusQueryHandler, GetJobStatusQueryHandler>();

            return services;
        }
    }
}