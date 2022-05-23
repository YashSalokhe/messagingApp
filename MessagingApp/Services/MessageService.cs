namespace MessagingApp.Services
{
    public class MessageService
    {
        private  readonly UserManager<IdentityUser> _userManager;
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

        public async Task<IEnumerable<Chat>> ReadChatsAsync(string receiver)
        {
            IEnumerable<Chat> particularChats = new List<Chat>();
            string currentUserName = http.HttpContext.Session.GetString("currentUser");
            http.HttpContext.Session.SetString("currentReceiver", receiver);
            var allChats = await chatContext.Chats.ToListAsync();

            var users = _userManager.Users.Where(x => x.UserName != currentUserName).Select(x => x.UserName);
            if (users.Contains(receiver))
            {
                particularChats = allChats.Where(d => d.SenderId == currentUserName || d.ReceiverId == currentUserName).Where(m => m.ReceiverId == receiver || m.SenderId == receiver);    
                      
            }
            else
            {
                particularChats = allChats.Where(m => m.ReceiverId == receiver);
            }
            foreach (var message in particularChats)
            {
                string decrypt = cipher.DecryptAsync(message.Message);
                message.Message = decrypt;
            }
            return particularChats;
        }


        public async Task<Chat> sendMessageAsync(string message)
        {
            Chat chat = new Chat();

            string currentUser = http.HttpContext.Session.GetString("currentUser");
            chat.SenderId = currentUser;

            string receiver = http.HttpContext.Session.GetString("currentReceiver");
            chat.ReceiverId = receiver;

            string encryptedMessage = cipher.EncryptAsync(message);
            chat.Message = encryptedMessage;

            chat.CurrentTime = DateTime.Now;
            await chatContext.Chats.AddAsync(chat);
            await chatContext.SaveChangesAsync();

            return chat;
        }
        
        public async Task<Group> CreateOrJoinGroupAsync(string groupName)
        {

            string currentUser = http.HttpContext.Session.GetString("currentUser");
            Group group = new Group()
            {
                GroupName = groupName,
                UserName = currentUser
            };
            await chatContext.Groups.AddAsync(group);
            await chatContext.SaveChangesAsync();

            return group;

        }

        public async Task<IEnumerable<string?>> NewGroupsToJoinAsync()
        {
            string currentUser = http.HttpContext.Session.GetString("currentUser");
            var allGroups = await chatContext.Groups.ToListAsync();
            var groups = allGroups.Where(x => x.UserName == currentUser).Select(x => x.GroupName);
            var newGroups = allGroups.Select(x => x.GroupName).Except(groups);

            return newGroups;
        }

        public async Task<IEnumerable<string?>> NewPeopleToChatAsync()
        {
            string currentUserName = http.HttpContext.Session.GetString("currentUser");
            var users = await _userManager.Users.Where(x => x.UserName != currentUserName).Select(x => x.UserName).ToListAsync();
            var chats = await chatContext.Chats.ToListAsync();

            var peopleHeSentChatsTo =chats.Where(c => c.SenderId == currentUserName).Select(x => x.ReceiverId).Distinct();
          
            var peopleHeRecievedChatsFrom =chats.Where(c => c.ReceiverId == currentUserName).Select(x => x.SenderId).Distinct();
            
            IEnumerable<string> newPeople = new List<string>();

            peopleHeSentChatsTo = peopleHeSentChatsTo.Union(peopleHeRecievedChatsFrom).ToList();
            newPeople = users.Except(peopleHeSentChatsTo);
            

            return newPeople;
        }
    }
}
