using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using JobServer.Application.DTOs;
using JobServer.Application.Interfaces;

namespace JobServer.Infrastructure.WebSockets
{
    /// <inheritdoc />
    public class WebSocketService : IWebSocketService
    {
        private static readonly List<WebSocket> _clients = new();

        /// <inheritdoc />
        public async Task HandleClientAsync(WebSocket socket)
        {
            _clients.Add(socket);

            var buffer = new byte[1024 * 4];
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    _clients.Remove(socket);
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                }
            }
        }

        /// <inheritdoc />
        public async Task BroadcastMessageAsync(WebSocketMessageDto message)
        {
            var json = JsonSerializer.Serialize(new
            {
                eventType = message.EventType,
                jobId = message.JobId,
                jobType = message.JobType,
                timestamp = DateTime.UtcNow
            });
            var buffer = Encoding.UTF8.GetBytes(json);
            var segment = new ArraySegment<byte>(buffer);

            foreach (var client in _clients.Where(c => c.State == WebSocketState.Open))
            {
                await client.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
