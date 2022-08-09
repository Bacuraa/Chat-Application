using Microsoft.AspNetCore.Mvc;
using ChatAppWebAPI.Models;
using ChatWebAPI.Models;

namespace ChatWebAPI.Controllers
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
        public void Add([Bind("Username,DisplayName,Password")] UserNoContacts userNoContacts)
        {
            User user = new User();
            user.Username = userNoContacts.Username;
            user.DisplayName = userNoContacts.DisplayName;
            user.Password = userNoContacts.Password;
            user.Contacts = new List<Contact>();
            serverDB.addUser(user);
        }
        [HttpHead]
        [Route("/api/[controller]")]
        // checks if username exists in the DB
        public Boolean CheckIfUserExists([Bind("Username")] string username)
        {
            return serverDB.checkUserExistance(username);
        }
        [HttpGet]
        [Route("/api/[controller]/{username}")]
        // returns a specific user
        public User GetUser(string username)
        {
            return serverDB.getSpecificUser(username);
        }
    }
}
