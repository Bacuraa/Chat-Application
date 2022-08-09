using Microsoft.AspNetCore.Mvc;
using ChatAppWebAPI.Models;
using ChatWebAPI.Models;


namespace ChatWebAPI.Controllers
{
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private ServerDB serverDB = new ServerDB();
        [HttpGet]
        [Route("/api/{username}/Contacts/{contactId}/[controller]")]
        // returns a list of messages from a specific contact (DB change done)
        public IEnumerable<Message> Get(string username, string contactId)
        {
            return serverDB.getMessages(username, contactId);
        }


        [HttpPost]
        [Route("/api/{username}/Contacts/{contactId}/[controller]")]
        // add a message (DB change done)
        public void Post(string username, string contactId, [Bind("Content")] string content)
        {
            Message message = new Message();
            message.SerialNumber = serverDB.getMessages(username, contactId).Count();
            message.Id = username + contactId + message.SerialNumber;
            message.Content = content;
            message.Created = DateTime.Now.ToString();
            message.Sent = true;
            serverDB.addMessage(username, contactId, message);
        }

        [HttpGet]
        [Route("/api/{username}/Contacts/{contactId}/[controller]/{id}")]
        // returns a specific message (DB change done)
        public Message Details(string username, string contactId, int id)
        {
            return serverDB.getSpecificMessage(username, contactId, id.ToString());
        }

        [HttpPut]
        [Route("/api/{username}/Contacts/{contactId}/[controller]/{id}")]
        // edits a specific message (DB change done)
        public void Put(string username, string contactId, int id, string content)
        {
            serverDB.editSpecificMessage(username, contactId, id.ToString(), content);
        }

        [HttpDelete]
        [Route("/api/{username}/Contacts/{contactId}/[controller]/{id}")]
        // deletes a specific message (DB change done)
        public void Delete(string username, string contactId, int id)
        {
            serverDB.deleteSpecificMessage(username, contactId, id.ToString());
        }

    }
}
