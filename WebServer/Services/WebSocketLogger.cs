using System.Text;

namespace WebServer.Services
{
    public class WebSocketLogger(ILogger<WebSocketLogger> logger)
    {
        private readonly ILogger<WebSocketLogger> _logger = logger;

        public void LogConnection(HttpContext context, Guid sessionId)
        {
            var clientIP = context.Connection.RemoteIpAddress?.ToString();
            var userAgent = context.Request.Headers.UserAgent.ToString();
            var requestUri = context.Request.Path;
            var referrer = context.Request.Headers.Referer.ToString();
            referrer = string.IsNullOrEmpty(referrer) ? "Empty" : referrer;

            var logMessage = new StringBuilder();
            logMessage.AppendLine($"Подключение по WebSocket:");
            logMessage.AppendLine($"\tClient IP: {clientIP}");
            logMessage.AppendLine($"\tUser-Agent: {userAgent}");
            logMessage.AppendLine($"\tRequest URI: {requestUri}");
            logMessage.AppendLine($"\tReferrer: {referrer}");
            logMessage.Append($"\tSession ID: {sessionId}");

            _logger.LogInformation(logMessage.ToString());
        }

        public void LogDisconnection(Guid sessionId)
        {
            _logger.LogInformation($"Клиент {sessionId} отключен");
        }
    }
}
