using JobServer.Application.Commands.CancelJob;
using JobServer.Application.Commands.StartJob;
using JobServer.Application.Queries.GetAllJobs;
using JobServer.Application.Queries.GetJobStatus;
using Microsoft.Extensions.DependencyInjection;

namespace JobServer.Application.DependencyInjection
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Command Handlers
            services.AddTransient<IStartJobCommandHandler, StartJobCommandHandler>();
            services.AddTransient<ICancelJobCommandHandler, CancelJobCommandHandler>();

            // Query Handlers
            services.AddTransient<IGetAllJobsQueryHandler, GetAllJobsQueryHandler>();
            services.AddTransient<IGetJobStatusQueryHandler, GetJobStatusQueryHandler>();

            return services;
        }
    }
}
