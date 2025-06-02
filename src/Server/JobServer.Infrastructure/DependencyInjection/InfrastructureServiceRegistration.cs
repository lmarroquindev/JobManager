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
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConcurrentDictionary<Guid, Job> jobStore)
        {
            services.AddSingleton<IJobExecutor>(sp =>
            {
                var notifier = sp.GetRequiredService<IJobNotifierService>();
                return new InMemoryJobExecutorAdapter(jobStore, notifier);
            });

            services.AddSingleton<IJobQueryService>(sp => new InMemoryJobQueryAdapter(jobStore));
            services.AddSingleton<IJobNotifierService, JobNotifierService>();
            services.AddSingleton<IWebSocketService, WebSocketService>();

            return services;
        }
    }
}
