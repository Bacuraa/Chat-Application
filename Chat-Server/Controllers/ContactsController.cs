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
        public IEnumerable<Contact> Get(string username)
        {
            List<Contact> contacts = serverDB.getContacts(username);
            return contacts;
            //List<ContactNoMessages> contactNomessages = new List<ContactNoMessages>();
            //foreach (Contact contact in contacts)
            //{
            //    ContactNoMessages contactNomessages1 = new ContactNoMessages();
            //    contactNomessages1.Id = contact.ContactUsername;
            //    contactNomessages1.Name = contact.Name;
            //    contactNomessages1.Server = contact.Server;
            //    contactNomessages1.Last = contact.Last;
            //    contactNomessages1.LastDate = contact.LastDate;
            //    contactNomessages.Add(contactNomessages1);
            //}
            //return contactNomessages;
        }

        [HttpPost]
        [Route("/api/{username}/[controller]")]
        // adds new contact to a user (DB change done)
        public void Post(string username, [Bind("Id, ContactUsername, Name,Server,Last,LastDate")] ContactOld contactOld)
        {
            Contact contact = new Contact();
            contact.Id = contactOld.Id;
            String[]  contactId = contactOld.Id.Split(username);
            contact.ContactUsername = contactId[1];
            contact.Name = contactOld.Name;
            contact.Server = contactOld.Server;
            contact.Last = contactOld.Last;
            contact.LastDate = contactOld.LastDate;
            serverDB.addContact(username, contact);
        }

        [HttpGet]
        [Route("/api/{username}/[controller]/{contactId}")]
        // returns a specific contact (DB change done)
        public ContactNoMessages Details(string username, string contactId)
        {
            Contact contact = serverDB.getSpecificContact(username, contactId);
            ContactNoMessages contactNoMessages = new ContactNoMessages();
            contactNoMessages.Id = contact.ContactUsername;
            contactNoMessages.Name = contact.Name;
            contactNoMessages.Server = contact.Server;
            contactNoMessages.Last = contact.Last;
            contactNoMessages.LastDate = contact.LastDate;
            return contactNoMessages;
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

