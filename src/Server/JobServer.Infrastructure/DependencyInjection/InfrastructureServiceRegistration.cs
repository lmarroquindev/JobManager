using JobServer.Application.Interfaces;
using JobServer.Application.Interfaces.Services;
using JobServer.Domain.Entities;
using JobServer.Domain.Interfaces;
using JobServer.Infrastructure.Persistence;
using JobServer.Infrastructure.Services;
using JobServer.Infrastructure.WebSockets;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace JobServer.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        /// <summary>
        /// Registers infrastructure services including persistence and websocket notifier.
        /// </summary>
        /// <param name="services">The service collection to add services to.</param>
        /// <param name="jobStore">An optional concurrent dictionary to hold jobs. If null, a new one is created.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            ConcurrentDictionary<Guid, Job>? jobStore = null)
        {
            // Create the job store if not provided
            var jobs = jobStore ?? new ConcurrentDictionary<Guid, Job>();

            // Register job notifier and websocket services first
            services.AddSingleton<IJobNotifierService, JobNotifierService>();
            services.AddSingleton<IWebSocketService, WebSocketService>();

            // Register job query service using the job store
            services.AddSingleton<IJobQueryRepository>(sp => new InMemoryJobQueryRepository(jobs));

            // Register job executor passing job store
            services.AddSingleton<IJobCommandRepository>(sp =>
            {
                return new InMemoryJobCommandRepository(jobs);
            });

            return services;
        }
    }
}
