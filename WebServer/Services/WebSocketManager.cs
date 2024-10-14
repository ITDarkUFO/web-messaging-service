using SharedLibrary.DTOs;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace WebServer.Services
{
    public class WebSocketManager(ILogger<WebSocketManager> logger, WebSocketLogger webSocketLogger)
    {
        private readonly ConcurrentDictionary<WebSocket, Guid> _clients = [];
        private readonly ILogger<WebSocketManager> _logger = logger;
        private readonly WebSocketLogger _webSocketLogger = webSocketLogger;

        public async Task SendMessageToClientAsync(WebSocket client, ChatMessageDTO message)
        {
            try
            {
                if (client.State == WebSocketState.Open)
                {
                    var serializedMessage = JsonSerializer.Serialize(message);
                    var buffer = Encoding.UTF8.GetBytes(serializedMessage);
                    var segment = new ArraySegment<byte>(buffer, 0, buffer.Length);

                    await client.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                }
                else
                {
                    RemoveClient(client);
                }
            }
            catch (WebSocketException ex)
            {
                _logger.LogWarning($"WebSocket error: {ex.Message}");
                RemoveClient(client);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error: {ex.Message}");
                RemoveClient(client);
            }
        }

        public async Task SendMessageToAllClientsAsync(ChatMessageDTO message)
        {
            foreach (var client in _clients.ToList())
            {
                await SendMessageToClientAsync(client.Key, message);
            }
        }

        public void AddClient(WebSocket client, HttpContext context)
        {
            var sessionId = Guid.NewGuid();

            if (_clients.TryAdd(client, sessionId))
            {
                _webSocketLogger.LogConnection(context, sessionId);
            }
        }

        public void RemoveClient(WebSocket client)
        {
            if (_clients.TryRemove(client, out var sessionId))
            {
                _webSocketLogger.LogDisconnection(sessionId);
            }
        }
    }
}
