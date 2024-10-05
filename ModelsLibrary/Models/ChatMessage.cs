using System.ComponentModel.DataAnnotations;
using SharedLibrary.Resources;

namespace SharedLibrary.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "RequiredError",
            ErrorMessageResourceType = typeof(SharedResources))]
        [StringLength(128, ErrorMessageResourceName = "MessageText_LengthError",
            ErrorMessageResourceType = typeof(Resources.Models.ChatMessage))]
        public string MessageText { get; set; } = default!;

        public DateTime MessageTimestamp { get; set; }

        [Required(ErrorMessageResourceName = "RequiredError",
            ErrorMessageResourceType = typeof(SharedResources))]
        [Range(0, int.MaxValue, ErrorMessageResourceName = "RangeError",
            ErrorMessageResourceType = typeof(SharedResources))]
        public int MessageIndex { get; set; } = default!;
    }
}
