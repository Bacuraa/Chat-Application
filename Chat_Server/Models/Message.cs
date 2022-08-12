using System.ComponentModel.DataAnnotations;

namespace Chat_Server.Models
{
    public class Message
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public int SerialNumber { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string Created { get; set; }
        [Required]
        public Boolean Sent { get; set; }
        
    }
}
