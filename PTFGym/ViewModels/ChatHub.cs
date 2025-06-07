using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace PTFGym.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string senderId, string receiverId, string message)
        {
            // Broadcast the message to the receiver
            await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, receiverId, message);

            // Optionally, broadcast to the sender as well (for UI updates)
            await Clients.User(senderId).SendAsync("ReceiveMessage", senderId, receiverId, message);
        }
    }
}