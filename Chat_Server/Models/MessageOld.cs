using System.ComponentModel.DataAnnotations;

namespace Chat_Server.Models
{
    public class MessageOld
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string Created { get; set; }
        [Required]
        public Boolean Sent { get; set; }
        
    }
}
