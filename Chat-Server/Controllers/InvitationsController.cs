using Microsoft.AspNetCore.Mvc;
using ChatWebAPI.Models;

namespace ChatWebAPI.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class InvitationsController : ControllerBase
    {
        private ServerDB serverDB = new ServerDB();
        [HttpPost]
        public IActionResult Post([Bind("From,To,Server")] Invitation invitation)
        {
            Boolean b = serverDB.inviteContact(invitation);
            if (b)
                return Ok();
            else 
                return NotFound();
        }
    }
}
