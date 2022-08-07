using Microsoft.AspNetCore.Mvc;
using ChatWebAPI.Models;

namespace ChatWebAPI.Controllers
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
