using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quze.Models.Logic
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            if (message == "registering" && user != null)
                await Groups.AddToGroupAsync(Context.ConnectionId, user);
            await Clients.Groups(user).SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendMessageToGroups(string user, string message)
        {
            List<string> groups = new List<string>() { "SignalR Users" };
            await Clients.Groups(user).SendAsync("ReceiveMessage", user, message);
        }

        public bool SendUpdateToClient(string user, string message)
        {
            if (Clients != null && Clients.All != null)
                Clients.All.SendAsync("ReceiveMessage", user, message);
            return true;
        }
    }
}
