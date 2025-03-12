using System.ComponentModel.DataAnnotations;

namespace Chat_Server.Models
{
    public class Transfer
    {
        [Required]
        public string From { get; set; }

        [Required]
        public string To { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
