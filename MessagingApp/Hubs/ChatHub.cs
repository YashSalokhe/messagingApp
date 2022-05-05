

namespace MessagingApp.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(Chat chat)
        {
            await Clients.All.SendAsync("receiveMessages" , chat);
        }
    }
}
