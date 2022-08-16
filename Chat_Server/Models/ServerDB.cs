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
                return db.UsersDB.Include(x => x.Contacts)
                    .ThenInclude(x => x.messages.OrderBy(y => y.SerialNumber)).ToList();
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
                return db.UsersDB.Where(x => x.Username == username).Include(x => x.Contacts)
                    .ThenInclude(x => x.messages.OrderBy(y => y.SerialNumber)).ToList()[0];
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
                User user = db.UsersDB.Where(x => x.Username == username)
                    .Include(x => x.Contacts).ThenInclude(x => x.messages.OrderBy(y => y.SerialNumber)).ToList()[0];
                if (user == null) return null;
                return user.Contacts;

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
                User user = db.UsersDB.Where(x => x.Username == username)
                    .Include(x => x.Contacts).ToList()[0];
                if (user == null) return;
                user.Contacts.Add(newContact);
                db.SaveChanges();
            }
        }
        public Contact getSpecificContact(string username, string contactUsername)
        {
            using (var db = new UsersContext())
            {
                string key = username + "_" + contactUsername;
                Contact contact = db.ContactsDB.Where(x => x.Id == key)
                    .Include(x => x.messages.OrderBy(y => y.SerialNumber)).ToList()[0];
                if (contact == null) return null;
                return contact;
            }
        }
        public void editContact(string username, string contactUsername, string newDisplayName, string server)
        {
            using (var db = new UsersContext())
            {
                string key = username + "_" + contactUsername;
                Contact contact = db.ContactsDB.Find(key);
                contact.DisplayName = newDisplayName;
                db.SaveChanges();
            }
        }
        public void deleteContact(string username, string contactId)
        {
            using (var db = new UsersContext())
            {
                string key = username + "_" + contactId;
                Contact contact = db.ContactsDB.Find(key);
                if (contact == null) return;
                db.ContactsDB.Remove(contact);
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
                User user = db.UsersDB.Where(x => x.Username == invitation.To)
                    .Include(x => x.Contacts).ToList()[0];
                if (user == null) return false;

                // creating a new contact
                Contact contact = new Contact();
                contact.Id = invitation.To + "_" + invitation.From;
                contact.ContactUsername = invitation.From;
                contact.DisplayName = invitation.From;
                contact.LastMessage = "";
                contact.LastDate = "";
                user.Contacts.Add(contact);
                db.SaveChanges();
                return true;
            }
        }

        //////////////////////////////////////////// MessagesController //////////////////////////////////////////////////
        public List<Message> getMessages(string username, string contactUsername)
        {
            using (var db = new UsersContext())
            {
                var key = username + "_" + contactUsername;
                Contact contact = db.ContactsDB.Where(x => x.Id == key)
                    .Include(x => x.messages.OrderBy(y => y.SerialNumber)).ToList()[0];
                if (contact == null) return null;
                return contact.messages;

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
                string contactKey = username + "_" + contactUsername;
                Contact contact = db.ContactsDB.Where(x => x.Id == contactKey)
                    .Include(x => x.messages).ToList()[0];
                if (contact == null) return;
                contact.messages.Add(newMessage);
                contact.LastDate = DateTime.Now.ToString();
                contact.LastMessage = newMessage.Content;
                db.SaveChanges();
            }
        }
        public Message getSpecificMessage(string username, string contactUsername, string messageSerialNumber)
        {
            using (var db = new UsersContext())
            {
                var messagesDB = db.MessagesDB;
                string key = username + "_" + contactUsername + "_" + messageSerialNumber;
                Message message = messagesDB.Where(x => x.Id == key).ToList()[0];
                if (message == null) return null;
                return message;
            }
        }

        public void editSpecificMessage(string username, string contactUsername,
                                        string messageSerialNumber, string content)
        {
            using (var db = new UsersContext())
            {
                var messagesDB = db.MessagesDB;
                string key = username + "_" + contactUsername + "_" + messageSerialNumber;
                Message message = db.MessagesDB.Where(x => x.Id == key).ToList()[0];
                if (message == null) return;
                message.Content = content;
                db.SaveChanges();
            }
        }
        public void deleteSpecificMessage(string username, string contactUsername, string messageSerialNumber)
        {
            using (var db = new UsersContext())
            {
                string key = username + "_" + contactUsername + "_" + messageSerialNumber;
                Message message = db.MessagesDB.Where(x => x.Id == key).ToList()[0];
                if (message == null) return;
                db.MessagesDB.Remove(message);
                db.SaveChanges();
            }
        }

        //////////////////////////////////////////// TransferController //////////////////////////////////////////////////
        public void addMessageTransfer(Transfer transfer)
        {
            using (var db = new UsersContext())
            {
                string key = transfer.To + "_" + transfer.From;
                Contact contact = db.ContactsDB.Where(x => x.Id == key)
                    .Include(x => x.messages).ToList()[0];
                if (contact == null) return;
                string time = DateTime.Now.ToString();
                int id = contact.messages.Count();
                Message message = new Message()
                {
                    Id = key + "_" + id.ToString(),
                    Content = transfer.Content,
                    Created = time,
                    SerialNumber = id,
                    Sent = false
                };
                contact.messages.Add(message);
                contact.LastDate = time;
                contact.LastMessage = transfer.Content;
                db.SaveChanges();
                ////////////////////////////////////////////// Firebase ///////////////////////////////////////////////////////
                /*User user = db.UsersDB.Where(x => x.Username == transfer.To).ToList()[0];
                if (user == null) return;
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
                Console.WriteLine("Successfully sent message: " + response);*/
            }
        }
    }
}
