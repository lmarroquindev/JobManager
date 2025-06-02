using System.Net.WebSockets;
using JobServer.Application.DTOs;

namespace JobServer.Application.Interfaces
{
    /// <summary>
    /// Defines a contract for managing WebSocket connections and broadcasting messages.
    /// </summary>
    public interface IWebSocketService
    {
        /// <summary>
        /// Handles a new WebSocket client connection.
        /// </summary>
        /// <param name="socket">The WebSocket client.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task HandleClientAsync(WebSocket socket);

        /// <summary>
        /// Broadcasts a message to all connected WebSocket clients.
        /// </summary>
        /// <param name="message">The message to broadcast.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task BroadcastMessageAsync(WebSocketMessageDto message);
    }
}