using System.ComponentModel.DataAnnotations;

namespace ChatAppWebAPI.Models
{
    public class Contact
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public String Username { get; set; }
        [Required]
        public String DisplayName { get; set; }
        [Required]
        public String Last { get; set; }
        public string LastDate { get; set; }
        public virtual List<Message> messages { get; set; }
    }
}
