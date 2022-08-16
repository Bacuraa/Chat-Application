using Microsoft.AspNetCore.Mvc;
using Chat_Server.Models;
using Chat_Server.Models.Requests;

namespace Chat_Server.Controllers
{
    [ApiController]
    public class UsersController : ControllerBase
    {
        private ServerDB serverDB = new ServerDB();
        [HttpGet]
        [Route("/api/[controller]")]
        // gets all users from the DB
        public IEnumerable<User> Get(){
            return serverDB.getAllUsers();
            }


        [HttpPost]
        [Route("/api/[controller]")]
        // adds a user
        public void Add([Bind("Username,DisplayName,Password")] AddUserPost addUserPost)
        {
            User user = new User();
            user.Username = addUserPost.Username;
            user.DisplayName = addUserPost.DisplayName;
            user.Password = addUserPost.Password;
            user.Contacts = new List<Contact>();
            serverDB.addUser(user);
        }
        [HttpHead]
        [Route("/api/[controller]/{username}")]
        // checks if username exists in the DB
        public IActionResult CheckIfUserExists(string username)
        {
            // "checkUserExistance" returns true if the user already exists
            if (serverDB.checkUserExistance(username))
                return Ok();
            return NotFound();
        }

        [HttpGet]
        [Route("/api/[controller]/{username}")]
        // returns a specific user
        public User GetUser(string username)
        {
            return serverDB.getSpecificUser(username);
        }

        [HttpHead]
        [Route("/api/[controller]/{username}/{password}")]
        // checks if the password inserted matches the usernames's password
        public IActionResult CheckPassword(string username ,string password)
        {
            if (serverDB.checkPassword(username, password))
                return Ok();
            return NotFound();
        }
    }
}
