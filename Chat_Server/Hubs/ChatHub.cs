using Chat_Server.Models;
using Microsoft.AspNetCore.SignalR;

namespace Chat_Server.Hubs
{
    public class ChatHub : Hub
    {
        static Dictionary<string,string> connections = new Dictionary<string,string>();
         public void AddConnection(string username)
        {
            connections[username] = Context.ConnectionId;
        }

        public async Task SendMessage(string loggedUsername, string contactUsername)
        {
            await Clients.Client(connections[loggedUsername]).SendAsync("ReceiveMessage", loggedUsername, contactUsername);
        }
    }
}
