

namespace MessagingApp.Hubs
{
    public class ChatHub : Hub
    {

        public string getConnectionId()=>Context.ConnectionId;

        //public async Task SendMessage(Chat chat)
        //{
        //    await Clients.All.SendAsync("receiveMessages" , chat);
        //}
        public Task JoinRoom(string roomName)
        {//
            //Clients.All.SendAsync("receiveMessages", chat);
           
            return Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

         
    }
}
