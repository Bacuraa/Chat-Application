using System.ComponentModel.DataAnnotations;

namespace ChatAppWebAPI.Models
{
    public class Chat
    {
        public int Id { get; set; }
        [Required]
        public string User1 { get; set; }
        [Required]
        public string User2 { get; set; }

        List<Message> Messages { get; set; }
    }
}
