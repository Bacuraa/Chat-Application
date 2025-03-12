using Chat_Server.Models;
using Chat_Server.Models.Requests;

using Microsoft.AspNetCore.Mvc;


namespace Chat_Server.Controllers
{
    [ApiController]
    
    public class ContactsController : ControllerBase
    {
        private ServerDB serverDB = new ServerDB();
        [HttpGet]
        [Route("/api/{username}/[controller]")]
        // get contacts by username
        public List<Contact> Get(string username)
        {
            return serverDB.getContacts(username);
        }

        [HttpPost]
        [Route("/api/{username}/[controller]")]
        // adds new contact to a user
        public void Post(string username, [FromBody] AddContactPost addContactPost)
        {
            serverDB.addContact(username, addContactPost);
        }

        [HttpGet]
        [Route("/api/{username}/[controller]/{contactUsername}")]
        // returns a specific contact
        public Contact Details(string username, string contactUsername)
        {
            return serverDB.getSpecificContact(username, contactUsername);
        }

        [HttpPut]
        [Route("/api/{username}/[controller]/{contactUsername}")]
        // edits a contact
        public void Put(string username, string contactUsername, string newDisplayName, string server)
        {
            serverDB.editContact(username, contactUsername, newDisplayName, server);
        }

        [HttpDelete]
        [Route("/api/{username}/[controller]/{contactUsername}")]
        // deletes a contact
        public void Delete(string username,string contactUsername)
        {
            serverDB.deleteContact(username, contactUsername);
        }

        [HttpHead]
        [Route("/api/{username}/[controller]/{contactUsername}")]
        // checks if a contact exists in the DB
        public IActionResult CheckIfContactExists(string username, string contactUsername)
        {
            if (serverDB.checkContactExistence(username, contactUsername))
                return Ok();
            return NotFound();
        }
    }
}

