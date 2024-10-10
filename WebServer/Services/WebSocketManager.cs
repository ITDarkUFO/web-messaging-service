using SharedLibrary.DTOs;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace WebServer.Services
{
    public class WebSocketManager(ILogger<WebSocketManager> logger)
    {
        private readonly List<WebSocket> _clients = [];
        private readonly ILogger<WebSocketManager> _logger = logger;

        public async Task SendMessageToClientAsync(WebSocket client, ChatMessageDTO message)
        {
            if (client.State == WebSocketState.Open)
            {
                try
                {
                    var serializedMessage = JsonSerializer.Serialize(message);
                    var buffer = Encoding.UTF8.GetBytes(serializedMessage);
                    var segment = new ArraySegment<byte>(buffer, 0, buffer.Length);

                    await client.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
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
            else
            {
                RemoveClient(client);
            }
        }

        public async Task SendMessageToAllClientsAsync(ChatMessageDTO message)
        {
            foreach (var client in _clients.ToList())
            {
                await SendMessageToClientAsync(client, message);
            }
        }

        public void AddClient(WebSocket client) => _clients.Add(client);

        public void RemoveClient(WebSocket client) => _clients.Remove(client);
    }
}
