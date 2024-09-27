using System.ComponentModel.DataAnnotations;

namespace WebServer.DTOs
{
    public class ChatMessageDTO
    {
        public string MessageText { get; set; } = default!;

        public DateTime MessageTimestamp { get; set; }

        public int MessageIndex { get; set; } = default!;
    }
}
