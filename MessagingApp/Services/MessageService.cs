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

        public async Task<IEnumerable<Chat>> Chats(string receiver)
        {
            IEnumerable<Chat> particularChats = new List<Chat>();
            string currentUserName = http.HttpContext.Session.GetString("currentUser");
            http.HttpContext.Session.SetString("currentReceiver", receiver);
            var allChat = await chatContext.Chats.ToListAsync();

            var users = _userManager.Users.Where(x => x.UserName != currentUserName).Select(x => x.UserName);
            if (users.Contains(receiver))
            {
                particularChats = allChat.Where(d => d.SenderId == currentUserName || d.ReceiverId == currentUserName).Where(m => m.ReceiverId == receiver || m.SenderId == receiver);    
                      
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

            string currentUser = http.HttpContext.Session.GetString("currentUser");
            chat.SenderId = currentUser;

            string receiver = http.HttpContext.Session.GetString("currentReceiver");
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

        public async Task<IEnumerable<string?>> NewGroupsToJoin()
        {
            string currentUser = http.HttpContext.Session.GetString("currentUser");
            var allGroups = await chatContext.Groups.ToListAsync();
            var groups = allGroups.Where(x => x.UserName == currentUser).Select(x => x.GroupName).Distinct();//.ToListAsync();//.ToListAsync();  //Select(x => x.GroupName).Distinct().ToListAsync();
            var newGroups = allGroups.Select(x => x.GroupName).Except(groups);

            return newGroups;
        }

        public async Task<IEnumerable<string>> NewPeopleToChat()
        {
            string currentUserName = http.HttpContext.Session.GetString("currentUser");
            var users = await _userManager.Users.Where(x => x.UserName != currentUserName).Select(x => x.UserName).ToListAsync();
            var peopleHeSentChatsTo = await chatContext.Chats.Where(c => c.SenderId == currentUserName).Select(x => x.ReceiverId).Distinct().ToListAsync();
           // var peopleHeSentChatsTo = users.Except(allChatHeSendsTo);
            var peopleHeRecievedChatsFrom = await chatContext.Chats.Where(c => c.ReceiverId == currentUserName).Select(x => x.SenderId).Distinct().ToListAsync();
            List<string> newPeople = new List<string>();

            newPeople = peopleHeSentChatsTo.Union(peopleHeRecievedChatsFrom).ToList();
            var allChatHeSendsTo = users.Except(newPeople);
            //foreach (var people in peopleHeChatsWith)
            //{

            //    if (!newPeople.Contains(people.SenderId) || !newPeople.Contains(people.ReceiverId))
            //    {   

            //        if (people.SenderId != currentUserName)
            //        {
            //            newPeople.Add(people.SenderId);
            //        }
            //        if(people.ReceiverId != currentUserName)
            //        {
            //            newPeople.Add(people.ReceiverId);
            //        }
            //    }

            //}


            return allChatHeSendsTo;
        }
    }
}
