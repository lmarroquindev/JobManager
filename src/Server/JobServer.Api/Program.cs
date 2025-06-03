using JobServer.API.Middleware;
using JobServer.Application.DependencyInjection;
using JobServer.Application.Interfaces;
using JobServer.Domain.Entities;
using JobServer.Infrastructure;
using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Shared in-memory job store
var jobStore = new ConcurrentDictionary<Guid, Job>();
builder.Services.AddSingleton(jobStore);

// Infrastructure
builder.Services.AddInfrastructure(jobStore);

// Application
builder.Services.AddApplicationServices();

// CORS
var clientUrl = builder.Configuration["CLIENT_URL"];

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(clientUrl ?? "http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
app.UseAuthorization();
app.UseWebSockets();

app.MapControllers();

// WebSocket endpoint
app.Map("/ws", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        var webSocketService = context.RequestServices.GetRequiredService<IWebSocketService>();
        await webSocketService.HandleClientAsync(webSocket);
    }
    else
    {
        context.Response.StatusCode = 400;
    }
});

app.Run();
