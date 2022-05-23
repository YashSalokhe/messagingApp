using MessagingApp.Hubs;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace MessagingApp.Controllers
{

    public class ChatController : Controller
    {
        private readonly MessageService messageService;
        private readonly ChatHistorydbContext chatContext;
        private readonly EncrytDecryptService cipher;
        private readonly IHubContext<ChatHub> chathub;

        public ChatController( ChatHistorydbContext chatContext, EncrytDecryptService cipher, MessageService messageService, IHubContext<ChatHub> chathub)
        {    
            this.chatContext = chatContext;
            this.cipher = cipher;
            this.messageService = messageService;
            this.chathub = chathub;
        }
        public ViewResult Index()
        {
            return View("Index");
        }



        [HttpPost]
        public async Task<OkObjectResult> Message(string Message )
        {
            var chat = await messageService.sendMessageAsync(Message);
            chat.Message = cipher.DecryptAsync(chat.Message);
            await chathub.Clients.All.SendAsync("RecieveMessage", chat);
            return Ok(chat);
        }

        public async Task<ViewResult> StartNewChat()
        {
            var newPeoples = await messageService.NewPeopleToChatAsync();
            return View(newPeoples);
        }
        

        public async Task<RedirectToActionResult> StartNewChatWith(string UserName)
        {
            HttpContext.Session.SetString("currentReceiver", UserName);
            await messageService.sendMessageAsync("hii");
            return RedirectToAction("Index");
        }

       

        public async Task<PartialViewResult> PartialChatHistory(string name)
        {
            ViewBag.CurrentReceiver =name;
            var particularChats = await messageService.ReadChatsAsync(name);
            return PartialView("ChatHistoryPartial",particularChats);
        }

     
        public ViewResult CreateGroupChat()
        {
            return View();
        }

        [HttpPost]
        public  async Task<RedirectToActionResult> CreateGroupChat(string groupName)
        {
            await messageService.CreateOrJoinGroupAsync(groupName);
            return RedirectToAction("Index");
        }

        public async Task<ViewResult> JoinGroupChat()
        {
            var newGroups = await messageService.NewGroupsToJoinAsync();
            return View(newGroups);
        }

        public async Task<RedirectToActionResult> JoinGroupChatWith(string groupName)
        {
            await messageService.CreateOrJoinGroupAsync(groupName);
            return RedirectToAction("Index");
        }
    }
}
