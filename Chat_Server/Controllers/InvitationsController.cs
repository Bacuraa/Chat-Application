using Microsoft.AspNetCore.Mvc;
using Chat_Server.Models;

namespace Chat_Server.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class InvitationsController : ControllerBase
    {
        private ServerDB serverDB = new ServerDB();
        [HttpPost]
        public IActionResult Post([Bind("From,To")] Invitation invitation)
        {
            Boolean b = serverDB.inviteContact(invitation);
            if (b)
                return Ok();
            else 
                return NotFound();
        }
    }
}
