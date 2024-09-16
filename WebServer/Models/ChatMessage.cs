using System.ComponentModel.DataAnnotations;

namespace WebServer.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }

        [StringLength(128, ErrorMessage = "Сообщение должно быть не длинее 128 символов")]
        public char MessageText { get; set; }

        public DateTime Timestamp { get; set; }

        public int MessageIndex { get; set; }
    }
}
