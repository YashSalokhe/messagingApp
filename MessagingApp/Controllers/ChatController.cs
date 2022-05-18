using MessagingApp.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace MessagingApp.Controllers
{
   [Authorize]
    public class ChatController : Controller
    {
        private readonly MessageService messageService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ChatHistorydbContext ctx;
        private readonly EncrytDecryptService cipher;
        private readonly IHubContext<ChatHub> chathub;

        public ChatController(UserManager<IdentityUser> _userManager, ChatHistorydbContext ctx, EncrytDecryptService cipher, MessageService messageService, IHubContext<ChatHub> chathub)
        {
            this._userManager = _userManager;
            this.ctx = ctx;
            this.cipher = cipher;
            this.messageService = messageService;
            this.chathub = chathub;
        }
        public IActionResult Index()
        {
            return View("Index");
        }

        public IActionResult Message()
        {
            return View(new Chat());
        }

        [HttpPost]
        public async Task<IActionResult> Message(string Message )
        {
           
            var chat = messageService.sendMessage(Message).Result;
            chat.Message =  cipher.DecryptAsync(chat.Message);
            //await chathub.Clients.Group(chat.ReceiverId).SendAsync("RecieveMessage", chat);
            await chathub.Clients.All.SendAsync("RecieveMessage", chat);
            return Ok(chat);

        }

        public async Task<IActionResult> StartNewChat()
        {
            var currentUserName = HttpContext.Session.GetString("CurrentUser");
            var chats = await ctx.Chats.ToListAsync();
            var users = _userManager.Users.Where(x=>x.UserName != currentUserName).Select(x=>x.UserName);
           
            return View(users);
        }
        

        public async Task<IActionResult> StartNewChatWith(string UserName)
        {        
            HttpContext.Session.SetString("currentReceiver", UserName);
            var sendMessage = await messageService.sendMessage("hii");
            return View("Index");
        }

       

        public PartialViewResult PartialChatHistory(string name)
        {

            ViewBag.CurrentReceiver =name;
            var particularChats =  messageService.Chats(name).Result;
            return PartialView("ChatHistoryPartial",particularChats);
        }

        public IActionResult CreateGroupChat()
        {
            Group group = new Group();
            return View(group);
        }

        [HttpPost]
        public  async Task<IActionResult> CreateGroupChat(string groupName)
        {
            var group = await messageService.CreateOrJoinGroupAsync(groupName);
            return View("Index");
        }

        public IActionResult JoinGroupChat()
        {
            var groups = ctx.Groups.ToListAsync().Result.Select(x => x.GroupName).Distinct();
            return View(groups);
        }

        public async Task<IActionResult> JoinGroupChatWith(string groupName)
        {
            var group = await messageService.CreateOrJoinGroupAsync(groupName);
            return View("Index");
        }
    }
}
