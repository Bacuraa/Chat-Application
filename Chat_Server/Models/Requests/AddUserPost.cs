namespace Chat_Server.Models.Requests
{
    public class AddUserPost
    {
        public string Username { get; set; }

        public string DisplayName { get; set; }
        public string Password { get; set; }
    }
}
