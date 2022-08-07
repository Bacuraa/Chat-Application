using System.ComponentModel.DataAnnotations;

namespace ChatAppWebAPI.Models
{
    public class MessageIdContent
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }

    }
}
