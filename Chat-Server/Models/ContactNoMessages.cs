using System.ComponentModel.DataAnnotations;

namespace ChatAppWebAPI.Models
{
    public class ContactNoMessages
    {
        [Required]
        public String Id { get; set; }
        [Required]
        public String Name { get; set; }
        [Required]
        public String Server { get; set; }
        public String Last { get; set; }
        public string LastDate { get; set; }
    }
}
