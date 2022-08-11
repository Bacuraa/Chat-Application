using ChatAppWebAPI.Models;
using ChatWebAPI.Models;
using Microsoft.AspNetCore.Mvc;


namespace ChatWebAPI.Controllers
{
    [ApiController]
    
    public class ContactsController : ControllerBase
    {
        private ServerDB serverDB = new ServerDB();
        [HttpGet]
        [Route("/api/{username}/[controller]")]
        // gets contacts by username
        public List<Contact> Get(string username)
        {
            return serverDB.getContacts(username);
        }

        [HttpPost]
        [Route("/api/{username}/[controller]")]
        // adds new contact to a user
        public void Post(string username, [Bind("Username, DisplayName")] Contact contact)
        {
            contact.Id = contact.ContactUsername + serverDB.getContacts(username).Count();
            contact.LastMessage = "";
            contact.LastDate = "";
            serverDB.addContact(username, contact);
        }

        [HttpGet]
        [Route("/api/{username}/[controller]/{contactId}")]
        // returns a specific contact
        public Contact Details(string username, string contactId)
        {
            return serverDB.getSpecificContact(username, contactId);
        }

        [HttpPut]
        [Route("/api/{username}/[controller]/{contactId}")]
        // edits a contact
        public void Put(string username, string contactId, string name, string server)
        {
            serverDB.editContact(username, contactId, name, server);
        }

        [HttpDelete]
        [Route("/api/{username}/[controller]/{contactId}")]
        // deletes a contact
        public void Delete(string username,string contactId)
        {
            serverDB.deleteContact(username, contactId);
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

