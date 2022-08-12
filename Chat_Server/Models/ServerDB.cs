using Chat_Server.Models;
using Chat_Server.Models.Requests;
using Microsoft.EntityFrameworkCore;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace Chat_Server.Models
{
    public class ServerDB
    {
        //////////////////////////////////////////// UsersController //////////////////////////////////////////////////
        public List<User> getAllUsers()
        {
            using (var db = new UsersContext())
            {
                return db.UsersDB.Include(x => x.Contacts).ThenInclude(x => x.messages).ToList();
            }
        }
        public void addUser(User user)
        {
            using (var db = new UsersContext())
            {
                db.UsersDB.Add(user);
                db.SaveChanges();
            }
        }

        public Boolean checkUserExistance(string username)
        {
            using (var db = new UsersContext())
            {
                var user = db.UsersDB.Find(username);
                if (user == null)
                    return false;
                else return true;
            }
        }

        public User getSpecificUser(string username)
        {
            using (var db = new UsersContext())
            {
                return db.UsersDB.Where(x => x.Username == username)
                    .Include(x => x.Contacts).ThenInclude(x => x.messages).ToList()[0];
            }
        }

        public Boolean checkPassword(string username, string password)
        {
            using (var db = new UsersContext())
            {
                User user = db.UsersDB.Find(username);
                if (user.Password == password)
                    return true;
                return false;
            }
        }

        //////////////////////////////////////////// ContactsController //////////////////////////////////////////////////

        public List<Contact> getContacts(string username)
        {
            using (var db = new UsersContext())
            {
                var usersDB = db.UsersDB;
                var contactsDB = db.ContactsDB;
                List<User> user = usersDB.Where(x => x.Username == username)
                    .Include(x => x.Contacts).ThenInclude(x => x.messages).ToList();
                if (user[0] == null) return null;
                return user[0].Contacts;

            }
        }
        public void addContact(string username, AddContactPost addContactPost)
        {
            // creating the contact
            Contact newContact = new Contact();
            newContact.Id = username + "_" + addContactPost.Username;
            newContact.ContactUsername = addContactPost.Username;
            newContact.DisplayName = addContactPost.DisplayName;
            newContact.LastMessage = "";
            newContact.LastDate = "";

            //inserting the created contact into the server's DB
            using (var db = new UsersContext())
            {
                var usersDB = db.UsersDB;
                var contactsDB = db.ContactsDB;
                List<User> user = usersDB.Where(x => x.Username == username)
                    .Include(x => x.Contacts).ThenInclude(x => x.messages).ToList();
                if (user[0] == null) return;
                user[0].Contacts.Add(newContact);
                db.SaveChanges();
            }
        }
        public Contact getSpecificContact(string username, string contactUsername)
        {
            using (var db = new UsersContext())
            {
                var contactsDB = db.ContactsDB;
                string key = username + "_" + contactUsername;
                List<Contact> contact = contactsDB.Where(x => x.Id == key).Include(x => x.messages).ToList();
                if (contact == null) return null;
                return contact[0];
            }
        }
        public void editContact(string username, string contactUsername, string newDisplayName, string server)
        {
            using (var db = new UsersContext())
            {
                var contactsDB = db.ContactsDB;
                string key = username + "_" + contactUsername;
                List<Contact> contact = contactsDB.Where(x => x.Id == key).Include(x => x.messages).ToList();
                if (contact == null || contact[0] == null) return;
                contact[0].DisplayName = newDisplayName;
                db.SaveChanges();
            }
        }
        public void deleteContact(string username, string contactId)
        {
            using (var db = new UsersContext())
            {
                var contactsDB = db.ContactsDB;
                string key = username + "_" + contactId;
                Contact contact = contactsDB.Find(key);
                if (contact == null) return;
                contactsDB.Remove(contact);
                db.SaveChanges();
            }
        }

        public Boolean checkContactExistence(string username, string contactUsername)
        {
            using (var db = new UsersContext())
            {
                var key = username + "_" + contactUsername;
                var contactExistence = db.ContactsDB.Find(key);
                if (contactExistence == null) return false;
                return true;
            }
        }

        ///////////////////////////////////////// invitationController /////////////////////////////////////////////
        public Boolean inviteContact(Invitation invitation)
        {
            using (var db = new UsersContext())
            {
                var usersDB = db.UsersDB;
                var contactsDB = db.ContactsDB;
                List<User> user = usersDB.Where(x => x.Username == invitation.To)
                    .Include(x => x.Contacts).ThenInclude(x => x.messages).ToList();
                if (user == null || user[0] == null) return false;
                string key = invitation.To + "_" + invitation.From;
                Contact contact = user[0].Contacts.Find(x => x.Id == key);
                if (contact != null) return false;

                // creating a new contact
                contact = new Contact();
                contact.Id = key;
                contact.ContactUsername = invitation.From;
                contact.DisplayName = invitation.From;
                contact.messages = new List<Message>();
                contact.LastMessage = "";
                contact.LastDate = "";
                user[0].Contacts.Add(contact);
                db.SaveChanges();
                return true;
            }
        }

        //////////////////////////////////////////// MessagesController //////////////////////////////////////////////////
        public IOrderedEnumerable<Message> getMessages(string username, string contactUsername)
        {
            using (var db = new UsersContext())
            {
                var usersDB = db.UsersDB;
                var messagesDB = db.MessagesDB;
                List<User> user = usersDB.Where(x => x.Username == username)
                    .Include(x => x.Contacts).ThenInclude(x => x.messages).ToList();
                if (user == null || user[0] == null) return null;
                Contact contact = user[0].Contacts.Find(x => x.ContactUsername == contactUsername);
                if (contact == null) return null;
                return contact.messages.OrderBy(x => x.SerialNumber);
            }
        }

        public void addMessage(string username, string contactUsername, AddMessagePost addMessagePost)
        {
            // creating the message
            Message newMessage = new Message();
            newMessage.SerialNumber = getMessages(username, contactUsername).Count();
            newMessage.Id = username + "_" + contactUsername + "_" + newMessage.SerialNumber;
            newMessage.Content = addMessagePost.Content;
            newMessage.Created = DateTime.Now.ToString();
            newMessage.Sent = true;

            // inserting the message into the DB
            using (var db = new UsersContext())
            {
                var usersDB = db.UsersDB;
                var contactsDB = db.ContactsDB;
                List<User> user = usersDB.Where(x => x.Username == username)
                    .Include(x => x.Contacts).ThenInclude(x => x.messages).ToList();
                string key = username + "_" + contactUsername;
                List<Contact> contact = contactsDB.Where(x => x.Id == key)
                    .Include(x => x.messages).ToList();
                if (contact == null || contact[0] == null) return;
                contact[0].messages.Add(newMessage);
                contact[0].LastDate = DateTime.Now.ToString();
                contact[0].LastMessage = newMessage.Content;
                db.SaveChanges();
            }
        }
        public Message getSpecificMessage(string username, string contactUsername, string messageSerialNumber)
        {
            using (var db = new UsersContext())
            {
                var messagesDB = db.MessagesDB;
                string key = username + "_" + contactUsername + "_" + messageSerialNumber;
                List<Message> message = messagesDB.Where(x => x.Id == key).ToList();
                if (message == null) return null;
                return message[0];
            }
        }

        public void editSpecificMessage(string username, string contactUsername,
                                        string messageSerialNumber, string content)
        {
            using (var db = new UsersContext())
            {
                var messagesDB = db.MessagesDB;
                string key = username + "_" + contactUsername + "_" + messageSerialNumber;
                List<Message> message = messagesDB.Where(x => x.Id == key).ToList();
                if (message == null || message[0] == null) return;
                message[0].Content = content;
                db.SaveChanges();
            }
        }
        public void deleteSpecificMessage(string username, string contactUsername, string messageSerialNumber)
        {
            using (var db = new UsersContext())
            {
                var messagesDB = db.MessagesDB;
                string key = username + "_" + contactUsername + "_" + messageSerialNumber;
                List<Message> message = messagesDB.Where(x => x.Id == key).ToList();
                if (message == null || message[0] == null) return;
                messagesDB.Remove(message[0]);
                db.SaveChanges();
            }
        }

        //////////////////////////////////////////// TransferController //////////////////////////////////////////////////
        public void addMessageTransfer(Transfer transfer)
        {
            using (var db = new UsersContext())
            {
                var usersDB = db.UsersDB;
                var contactsDB = db.ContactsDB;
                string key = transfer.To + "_" + transfer.From;
                List<Contact> contact = contactsDB.Where(x => x.Id == key)
                    .Include(x => x.messages).ToList();
                if (contact == null || contact[0] == null) return;
                string time = DateTime.UnixEpoch.ToString();
                int id = contact[0].messages.Count();
                Message message = new Message()
                {
                    Id = key + "_" + id.ToString(),
                    Content = transfer.Content,
                    Created = time,
                    SerialNumber = id,
                    Sent = false
                };
                contact[0].messages.Add(message);
                contact[0].LastDate = time;
                contact[0].LastMessage = transfer.Content;
                db.SaveChanges();
                ////////////////////////////////////////////// Firebase ///////////////////////////////////////////////////////
                List<User> user = usersDB.Where(x => x.Username == transfer.To).ToList();
                if (user.Count == 0) return;
                //string token = user[0].Token;
                //if (token == "") return;

                if (FirebaseApp.DefaultInstance == null )
                {
                    FirebaseApp.Create(new AppOptions()
                    {
                        Credential = GoogleCredential.FromFile("private_key.json")
                    });
                }
                

                // This registration token comes from the client FCM SDKs.
                //var registrationToken = token;

                // See documentation on defining a message payload.
                var messageFirebase = new FirebaseAdmin.Messaging.Message()
                {
                    Data = new Dictionary<string, string>()
    {
        { "myData", transfer.From },
    },
                    //Token = registrationToken,
                    Notification = new FirebaseAdmin.Messaging.Notification()
                    {
                        Title = transfer.From,
                        Body = transfer.From +": " + transfer.Content
                    }
                };

                // Send a message to the device corresponding to the provided
                // registration token.
                string response = FirebaseAdmin.Messaging.FirebaseMessaging.DefaultInstance.SendAsync(messageFirebase).Result;
                // Response is a message ID string.
                Console.WriteLine("Successfully sent message: " + response);
            }
        }
    }
}
