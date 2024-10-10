using SharedLibrary.Models;

namespace WebServer.Services
{
    public class MessageEventAggregator
    {
        public event EventHandler<ChatMessage>? OnMessageReceived;

        public void RaiseMessageReceived(ChatMessage message)
        {
            OnMessageReceived?.Invoke(this, message);
        }
    }
}
