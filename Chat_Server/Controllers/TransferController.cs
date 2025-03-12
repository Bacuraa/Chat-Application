using Microsoft.AspNetCore.Mvc;
using Chat_Server.Models;

namespace Chat_Server.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class TransferController : ControllerBase
    {
        private ServerDB serverDB = new ServerDB();
        [HttpPost]
        // transfers data to another server
        public void Post([Bind("From,To,Content")] Transfer transfer)
        {
            serverDB.addMessageTransfer(transfer);
        }
    }
}
