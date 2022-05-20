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
        private readonly ChatHistorydbContext chatContext;
        private readonly EncrytDecryptService cipher;
        private readonly IHubContext<ChatHub> chathub;

        public ChatController(UserManager<IdentityUser> _userManager, ChatHistorydbContext chatContext, EncrytDecryptService cipher, MessageService messageService, IHubContext<ChatHub> chathub)
        {
            this._userManager = _userManager;
            this.chatContext = chatContext;
            this.cipher = cipher;
            this.messageService = messageService;
            this.chathub = chathub;
        }
        public IActionResult Index()
        {
            return View("Index");
        }



        [HttpPost]
        public async Task<IActionResult> Message(string Message )
        {
           
            var chat = await messageService.sendMessage(Message);
            chat.Message = cipher.DecryptAsync(chat.Message);
            await chathub.Clients.All.SendAsync("RecieveMessage", chat);
            return Ok(chat);

        }

        public async Task<IActionResult> StartNewChat()
        {
            var newPeoples = await messageService.NewPeopleToChat();
            return View(newPeoples);
        }
        

        public async Task<IActionResult> StartNewChatWith(string UserName)
        {
            HttpContext.Session.SetString("currentReceiver", UserName);
            await messageService.sendMessage("hii");
            return RedirectToAction("Index");
        }

       

        public async Task<PartialViewResult> PartialChatHistory(string name)
        {
            ViewBag.CurrentReceiver =name;
            var particularChats = await messageService.Chats(name);
            return PartialView("ChatHistoryPartial",particularChats);
        }

     
        public IActionResult CreateGroupChat()
        {
            return View();
        }

        [HttpPost]
        public  async Task<IActionResult> CreateGroupChat(string groupName)
        {
            await messageService.CreateOrJoinGroupAsync(groupName);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> JoinGroupChat()
        {
            var newGroups = await messageService.NewGroupsToJoin();
            return View(newGroups);
        }

        public async Task<IActionResult> JoinGroupChatWith(string groupName)
        {
            await messageService.CreateOrJoinGroupAsync(groupName);
            return RedirectToAction("Index");
        }
    }
}
