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
        // gets all users from the DB (DB change done)
        public IEnumerable<User> Get(){
            return serverDB.getAllUsers();
            }


        [HttpPost]
        [Route("/api/[controller]")]
        // adds a user (DB change done)
        public void Add([Bind("Username,DisplayName,Password, Token")] UserNoContacts userNoContacts)
        {
            User user = new User();
            user.Username = userNoContacts.Username;
            user.DisplayName = userNoContacts.DisplayName;
            user.Password = userNoContacts.Password;
            user.Contacts = new List<Contact>();
            user.Token = userNoContacts.Token;
            serverDB.addUser(user);
        }
    }
}
