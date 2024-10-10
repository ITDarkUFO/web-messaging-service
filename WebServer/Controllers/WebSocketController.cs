using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace WebServer.Controllers
{
    public class WebSocketController(Services.WebSocketManager webSocketManager) : ControllerBase
    {
        private readonly Services.WebSocketManager _webSocketManager = webSocketManager;
        private WebSocket? _webSocket;

        [HttpGet("/ws")]
        public async Task Connect()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                _webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                _webSocketManager.AddClient(_webSocket);

                while (_webSocket.State == WebSocketState.Open)
                {
                    var buffer = new byte[1024 * 2];
                    var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                        _webSocketManager.RemoveClient(_webSocket);
                        break;
                    }
                }
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }

        //private static async Task Echo(WebSocket webSocket)
        //{
        //    var buffer = new byte[128 * 2];
        //    var receiveResult = await webSocket.ReceiveAsync(
        //        new ArraySegment<byte>(buffer), CancellationToken.None);

        //    while (!receiveResult.CloseStatus.HasValue)
        //    {
        //        await webSocket.SendAsync(
        //            new ArraySegment<byte>(buffer, 0, receiveResult.Count),
        //            receiveResult.MessageType,
        //            receiveResult.EndOfMessage,
        //            CancellationToken.None);

        //        receiveResult = await webSocket.ReceiveAsync(
        //            new ArraySegment<byte>(buffer), CancellationToken.None);
        //    }

        //    await webSocket.CloseAsync(
        //        receiveResult.CloseStatus.Value,
        //        receiveResult.CloseStatusDescription,
        //        CancellationToken.None);
        //}
    }
}
