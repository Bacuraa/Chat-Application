using System.ComponentModel.DataAnnotations;

namespace Chat_Server.Models
{
    public class Contact
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string ContactUsername { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        public string LastMessage { get; set; }
        public string LastDate { get; set; }

        public string TimeAgo
        {
            get
            {
                if (LastDate == "") return "";
                int seconds = Convert.ToInt32((DateTime.Now - DateTime.Parse(LastDate)).TotalSeconds);

                int interval = seconds / 31536000;
                if (interval >= 1) return interval + "y ago";

                interval = seconds / 2592000;
                if (interval >= 1) return interval + "m ago";

                interval = seconds / 86400;
                if (interval >= 1) return interval + "d ago";

                interval = seconds / 3600;
                if (interval >= 1) return interval + "h ago";

                interval = seconds / 60;
                if (interval >= 1) return interval + "m ago";

                return seconds + "s ago";
            }
        }
        public virtual List<Message> messages { get; set; }
    }
}
