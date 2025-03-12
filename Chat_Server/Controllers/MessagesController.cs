using Microsoft.AspNetCore.Mvc;
using Chat_Server.Models;
using Chat_Server.Models.Requests;


namespace Chat_Server.Controllers
{
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private ServerDB serverDB = new ServerDB();
        [HttpGet]
        [Route("/api/{username}/Contacts/{contactUsername}/[controller]")]
        // returns a list of messages from a specific contact
        public IEnumerable<Message> Get(string username, string contactUsername)
        {
            return serverDB.getMessages(username, contactUsername);
        }


        [HttpPost]
        [Route("/api/{username}/Contacts/{contactUsername}/[controller]")]
        // add a message
        public void Post(string username, string contactUsername, [FromBody] AddMessagePost addMessagePost)
        {
            serverDB.addMessage(username, contactUsername, addMessagePost);
        }

        [HttpGet]
        [Route("/api/{username}/Contacts/{contactUsername}/[controller]/{messageSerialNumber}")]
        // returns a specific message
        public Message Details(string username, string contactUsername, int messageSerialNumber)
        {
            return serverDB.getSpecificMessage(username, contactUsername, messageSerialNumber.ToString());
        }

        [HttpPut]
        [Route("/api/{username}/Contacts/{contactUsername}/[controller]/{messageSerialNumber}")]
        // edits a specific message
        public void Put(string username, string contactUsername, int messageSerialNumber, string content)
        {
            serverDB.editSpecificMessage(username, contactUsername, messageSerialNumber.ToString(), content);
        }

        [HttpDelete]
        [Route("/api/{username}/Contacts/{contactUsername}/[controller]/{messageSerialNumber}")]
        // deletes a specific message
        public void Delete(string username, string contactUsername, int messageSerialNumber)
        {
            serverDB.deleteSpecificMessage(username, contactUsername, messageSerialNumber.ToString());
        }

    }
}
