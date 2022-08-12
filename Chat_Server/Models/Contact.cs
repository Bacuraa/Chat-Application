using System.ComponentModel.DataAnnotations;

namespace Chat_Server.Models
{
    public class Contact
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public String ContactUsername { get; set; }
        [Required]
        public String DisplayName { get; set; }
        [Required]
        public String LastMessage { get; set; }
        public string LastDate { get; set; }
        public virtual List<Message> messages { get; set; }
    }
}
