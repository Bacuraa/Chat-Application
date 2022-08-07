using ChatAppWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace ChatWebAPI.Models
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

        //////////////////////////////////////////// LoginController //////////////////////////////////////////////////

        public User getUser(string username)
        {
            using (var db = new UsersContext())
            {
                List<User> user = db.UsersDB.Where(x => x.Username == username)
                    .Include(x => x.Contacts).ThenInclude(x => x.messages).ToList();
                if (user.Count == 0) return null;
                return user[0];
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
        public void addContact(string username, Contact contact)
        {
            using (var db = new UsersContext())
            {
                var usersDB = db.UsersDB;
                var contactsDB = db.ContactsDB;
                List<User> user = usersDB.Where(x => x.Username == username)
                    .Include(x => x.Contacts).ThenInclude(x => x.messages).ToList();
                if (user[0] == null) return;
                user[0].Contacts.Add(contact);
                db.SaveChanges();
            }
        }
        public Contact getSpecificContact(string username, string contactId)
        {
            using (var db = new UsersContext())
            {
                var contactsDB = db.ContactsDB;
                string key = username + contactId;
                List<Contact> contact = contactsDB.Where(x => x.Id == key).Include(x => x.messages).ToList();
                if (contact == null) return null;
                return contact[0];
            }
        }
        public void editContact(string username, string contactId, string name, string server)
        {
            using (var db = new UsersContext())
            {
                var contactsDB = db.ContactsDB;
                string key = username + contactId;
                List<Contact> contact = contactsDB.Where(x => x.Id == key).Include(x => x.messages).ToList();
                if (contact == null || contact[0] == null) return;
                contact[0].Name = name;
                contact[0].Server = server;
                db.SaveChanges();
            }
        }
        public void deleteContact(string username, string contactId)
        {
            using (var db = new UsersContext())
            {
                var contactsDB = db.ContactsDB;
                string key = username + contactId;
                Contact contact = contactsDB.Find(key);
                if (contact == null) return;
                contactsDB.Remove(contact);
                db.SaveChanges();
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
                string key = invitation.To + invitation.From;
                Contact contact = user[0].Contacts.Find(x => x.Id == key);
                if (contact != null) return false;
                // creating a new contact
                contact = new Contact();
                contact.Id = key;
                contact.ContactUsername = invitation.From;
                contact.Name = invitation.From;
                contact.Server = invitation.Server;
                contact.messages = new List<Message>();
                contact.Last = "";
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
                //List<Message> messages = messagesDB.Where(x => x.ContactId.StartsWith(username + contactUsername))
                //                                                        .OrderBy(x => x.SerialNumber).ToList();
                List<User> user = usersDB.Where(x => x.Username == username)
                    .Include(x => x.Contacts).ThenInclude(x => x.messages).ToList();
                if (user == null || user[0] == null) return null;
                Contact contact = user[0].Contacts.Find(x => x.ContactUsername == contactUsername);
                if (contact == null) return null;
                return contact.messages.OrderBy(x => x.SerialNumber);
            }
        }

        public void addMessage(string username, string contactUsername, Message message)
        {
            using (var db = new UsersContext())
            {
                var usersDB = db.UsersDB;
                var contactsDB = db.ContactsDB;
                List<User> user = usersDB.Where(x => x.Username == username)
                    .Include(x => x.Contacts).ThenInclude(x => x.messages).ToList();
                string key = username + contactUsername;
                List<Contact> contact = contactsDB.Where(x => x.Id == key)
                    .Include(x => x.messages).ToList();
                if (contact == null || contact[0] == null) return;
                contact[0].messages.Add(message);
                contact[0].LastDate = DateTime.Now.ToString();
                contact[0].Last = message.Content;
                db.SaveChanges();
            }
        }
        public Message getSpecificMessage(string username, string contactUsername, string Id)
        {
            using (var db = new UsersContext())
            {
                var messagesDB = db.MessagesDB;
                string key = username + contactUsername + Id;
                List<Message> message = messagesDB.Where(x => x.Id == key).ToList();
                if (message == null) return null;
                return message[0];
            }
        }

        public void editSpecificMessage(string username, string contactUsername, string Id, string content)
        {
            using (var db = new UsersContext())
            {
                var messagesDB = db.MessagesDB;
                string key = username + contactUsername + Id;
                List<Message> message = messagesDB.Where(x => x.Id == key).ToList();
                if (message == null || message[0] == null) return;
                message[0].Content = content;
                db.SaveChanges();
            }
        }
        public void deleteSpecificMessage(string username, string contactUsername, string Id)
        {
            using (var db = new UsersContext())
            {
                var messagesDB = db.MessagesDB;
                string key = username + contactUsername + Id;
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
                string key = transfer.To + transfer.From;
                List<Contact> contact = contactsDB.Where(x => x.Id == key)
                    .Include(x => x.messages).ToList();
                if (contact == null || contact[0] == null) return;
                string time = DateTime.Now.ToString();
                int id = contact[0].messages.Count();
                Message message = new Message()
                {
                    Id = key + id.ToString(),
                    Content = transfer.Content,
                    Created = time,
                    SerialNumber = id,
                    Sent = false
                };
                contact[0].messages.Add(message);
                contact[0].LastDate = time;
                contact[0].Last = transfer.Content;
                db.SaveChanges();
                ////////////////////////////////////////////// Firebase ///////////////////////////////////////////////////////
                List<User> user = usersDB.Where(x => x.Username == transfer.To).ToList();
                if (user.Count == 0) return;
                string token = user[0].Token;
                if (token == "") return;

                if (FirebaseApp.DefaultInstance == null )
                {
                    FirebaseApp.Create(new AppOptions()
                    {
                        Credential = GoogleCredential.FromFile("private_key.json")
                    });
                }
                

                // This registration token comes from the client FCM SDKs.
                var registrationToken = token;

                // See documentation on defining a message payload.
                var messageFirebase = new FirebaseAdmin.Messaging.Message()
                {
                    Data = new Dictionary<string, string>()
    {
        { "myData", transfer.From },
    },
                    Token = registrationToken,
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
