using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebServer.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Сообщение не может быть пустым")]
        [StringLength(128, ErrorMessageResourceName = "LengthError", ErrorMessageResourceType = typeof(WebServer.Resources.SharedResource))]
        public string MessageText { get; set; } = default!;

        public DateTime MessageTimestamp { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessageResourceName = "RangeError")]
        public int? MessageIndex { get; set; }
    }
}
