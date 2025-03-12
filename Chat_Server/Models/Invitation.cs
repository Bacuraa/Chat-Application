using System.ComponentModel.DataAnnotations;

namespace Chat_Server.Models
{
    public class Invitation
    {
        [Key]
        [Required]
        public string From { get; set; }

        [Required]
        public string To { get; set; }

    }
}
