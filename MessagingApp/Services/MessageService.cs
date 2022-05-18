namespace MessagingApp.Services
{
    public class MessageService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ChatHistorydbContext chatContext;
        private readonly EncrytDecryptService cipher;
        private readonly IHttpContextAccessor http;

        public MessageService(UserManager<IdentityUser> _userManager, ChatHistorydbContext chatContext, EncrytDecryptService cipher,IHttpContextAccessor http)
        {
            this._userManager = _userManager;
            this.chatContext = chatContext;
            this.cipher = cipher;
            this.http = http;
        }

        public async Task<List<Chat>> Chats(string? receiver)
        {
            IEnumerable<Chat> particularChats = new List<Chat>();
            var currentUserName = http.HttpContext.Session.GetString("CurrentUser");
            var allChat = await chatContext.Chats.ToListAsync();
            http.HttpContext.Session.SetString("currentReceiver", receiver);

            var users = _userManager.Users.Where(x => x.UserName != currentUserName).Select(x => x.UserName);
            if (users.Contains(receiver))
            {
                var messages = allChat.Where(d => d.SenderId == currentUserName || d.ReceiverId == currentUserName);
                particularChats = messages.Where(m => m.ReceiverId == receiver || m.SenderId == receiver);
                      
            }
            else
            {
                particularChats = allChat.Where(m => m.ReceiverId == receiver);
            }
            foreach (var message in particularChats)
            {
                var decrypt = cipher.DecryptAsync(message.Message);
                message.Message = decrypt;
            }
            return particularChats.ToList();
        }


        public async Task<Chat> sendMessage(string message)
        {
            Chat chat = new Chat();
            //  var sender = await _userManager.FindByNameAsync(chat.SenderId);
            var currentUser = http.HttpContext.Session.GetString("CurrentUser");
            chat.SenderId = currentUser;
            //var receiver = "yash";
            var receiver = http.HttpContext.Session.GetString("currentReceiver");
            chat.ReceiverId = receiver;
            var encryptedMessage = cipher.EncryptAsync(message);
            chat.Message = encryptedMessage;
            chat.CurrentTime = DateTime.Now;
            await chatContext.Chats.AddAsync(chat);
            await chatContext.SaveChangesAsync();

            return chat;
        }
        
        public async Task<Group> CreateOrJoinGroupAsync(string groupName)
        {
            
            var currentUser = http.HttpContext.Session.GetString("CurrentUser");
            Group group = new Group()
            {
                GroupName = groupName,
                UserName = currentUser
            };
            await chatContext.Groups.AddAsync(group);
            await chatContext.SaveChangesAsync();

            return group;
          


        }

        //public async Task<Group> JoinGroupAsync(string groupName)
        //{
        //    var group = await CreateGroupAsync(groupName);
        //    return group;
        //}

    }
}
