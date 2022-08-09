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
        // gets contacts by username (DB change done)
        public List<Contact> Get(string username)
        {
            return serverDB.getContacts(username);
        }

        [HttpPost]
        [Route("/api/{username}/[controller]")]
        // adds new contact to a user (DB change done)
        public void Post(string username, [Bind("Username, DisplayName")] Contact contact)
        {
            contact.Id = contact.Username + serverDB.getContacts(username).Count();
            contact.Last = "";
            contact.LastDate = "";
            serverDB.addContact(username, contact);
        }

        [HttpGet]
        [Route("/api/{username}/[controller]/{contactId}")]
        // returns a specific contact (DB change done)
        public Contact Details(string username, string contactId)
        {
            return serverDB.getSpecificContact(username, contactId);
        }

        [HttpPut]
        [Route("/api/{username}/[controller]/{contactId}")]
        // edits a contact (DB change done)
        public void Put(string username, string contactId, string name, string server)
        {
            serverDB.editContact(username, contactId, name, server);
        }

        // GET: Contacts/Delete/5
        [HttpDelete]
        [Route("/api/{username}/[controller]/{contactId}")]
        // deletes a contact (DB change done)
        public void Delete(string username,string contactId)
        {
            serverDB.deleteContact(username, contactId);
        }

    }
}

