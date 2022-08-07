using System.ComponentModel.DataAnnotations;

namespace ChatAppWebAPI.Models
{
    public class UserNoContacts
    {
        [Key]  
        public String Username { get; set; }
        [Required]
        public String DisplayName { get; set; }
        [Required]
        public String Password { get; set; }

        public String Token { get; set; }
       
    }
}
