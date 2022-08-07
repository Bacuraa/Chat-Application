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
        public IEnumerable<MessageOld> Get(string username, string contactId)
        {
            IOrderedEnumerable<Message> messages = serverDB.getMessages(username, contactId);
            List<MessageOld> messageOlds = new List<MessageOld>();
            //if (messages == null) return messageOlds;
            foreach (Message message in messages)
            {
                MessageOld messageOld = new MessageOld();
                messageOld.Id = message.Id;
                messageOld.Content = message.Content;
                messageOld.Created = message.Created;
                messageOld.Sent = message.Sent;
                messageOlds.Add(messageOld);
            }

            return messageOlds;
        }


        [HttpPost]
        [Route("/api/{username}/Contacts/{contactId}/[controller]")]
        // add a message (DB change done)
        public void Post(string username, string contactId, [Bind("Id,Content")] MessageIdContent messageIdContent)
        {
            Message message = new Message();
            message.Id = username + contactId + messageIdContent.Id;
            message.SerialNumber = messageIdContent.Id;
            message.Content = messageIdContent.Content;
            message.Created = DateTime.Now.ToString();
            message.Sent = true;
            serverDB.addMessage(username, contactId, message);
        }

        [HttpGet]
        [Route("/api/{username}/Contacts/{contactId}/[controller]/{id}")]
        // returns a specific message (DB change done)
        public MessageOld Details(string username, string contactId, int id)
        {
            Message message = serverDB.getSpecificMessage(username, contactId, id.ToString());
            MessageOld messageOld = new MessageOld();
            messageOld.Id = message.Id;
            messageOld.Content = message.Content;
            messageOld.Created = message.Created;
            message.Sent = message.Sent;
            return messageOld;
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
