using Microsoft.AspNetCore.Mvc;
using ChatAppWebAPI.Models;
using ChatWebAPI.Models;


namespace ChatWebAPI.Controllers
{
    [ApiController]
    public class LoginController : ControllerBase
    {
        private ServerDB serverDB = new ServerDB();
        [HttpGet]
        [Route("/api/[controller]/{username}")]
        // gets a specific user (DB change done)
        public User Get(string username)
        {
            return serverDB.getUser(username);
        }
    }
}
