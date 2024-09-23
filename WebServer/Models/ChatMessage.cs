using System.ComponentModel.DataAnnotations;

namespace WebServer.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "RequiredError",
            ErrorMessageResourceType = typeof(Resources.SharedResources))]
        [StringLength(128, ErrorMessageResourceName = "MessageText_LengthError",
            ErrorMessageResourceType = typeof(Resources.Models.ChatMessage))]
        public string MessageText { get; set; } = default!;

        public DateTime MessageTimestamp { get; set; }

        [Required(ErrorMessageResourceName = "RequiredError",
            ErrorMessageResourceType = typeof(Resources.SharedResources))]
        [Range(0, int.MaxValue, ErrorMessageResourceName = "RangeError",
            ErrorMessageResourceType = typeof(Resources.SharedResources))]
        public int? MessageIndex { get; set; }
    }
}
