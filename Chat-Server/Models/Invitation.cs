using System.ComponentModel.DataAnnotations;

namespace ChatWebAPI.Models
{
    public class Invitation
    {
        [Key]
        [Required]
        public string From { get; set; }

        [Required]
        public string To { get; set; }

        [Required]
        public string Server { get; set; }
    }
}
