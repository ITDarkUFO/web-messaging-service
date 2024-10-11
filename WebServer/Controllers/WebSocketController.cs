using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace WebServer.Controllers
{
    public class WebSocketController(ILogger<WebSocketController> logger, Services.WebSocketManager webSocketManager) : ControllerBase
    {
        private readonly ILogger<WebSocketController> _logger = logger;
        private readonly Services.WebSocketManager _webSocketManager = webSocketManager;
        private WebSocket? _webSocket;

        [HttpGet("/ws")]
        public async Task Connect()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                _webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                _webSocketManager.AddClient(_webSocket);

                try
                {
                    while (_webSocket.State == WebSocketState.Open)
                    {
                        var buffer = new byte[1024 * 2];
                        var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                            break;
                        }
                    }
                }
                catch (WebSocketException ex)
                {
                    _logger.LogWarning($"WebSocket error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Unexpected error: {ex.Message}");
                }
                finally
                {
                    _webSocketManager.RemoveClient(_webSocket);
                }
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
    }
}
