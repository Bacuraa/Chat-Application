using System.ComponentModel.DataAnnotations;

namespace Chat_Server.Models
{
    public class User
    {
        [Key]  
        public String Username { get; set; }
        [Required]
        public String DisplayName { get; set; }
        [Required]
        public String Password { get; set; }
        [Required]
        public virtual List<Contact> Contacts { get; set; }
       
    }
}
