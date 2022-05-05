using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace MessagingApp.Controllers
{
    public class ChatController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ChatHistorydbContext ctx;
        private readonly EncrytDecryptService cipher;
        public ChatController(UserManager<IdentityUser> _userManager, ChatHistorydbContext ctx, EncrytDecryptService cipher)
        {
            this._userManager = _userManager;
            this.ctx = ctx;
            this.cipher = cipher;
        }
        public async Task<IActionResult> Index(string name)
        {
            var currentUserName = HttpContext.Session.GetString("CurrentUser");
            var chats = await ctx.Chats.ToListAsync();
            var messages = chats.Where(d => d.SenderId == currentUserName || d.ReceiverId == currentUserName);
            foreach (var message in messages)
            {
                var decrypt = await cipher.DecryptAsync(message.Message);
                message.Message = decrypt;
            }
            if (name != null)
            {
                HttpContext.Session.SetString("currentReceiver" , name);
                var particularChats = messages.Where(m=>m.ReceiverId == name);
                return View(particularChats);
            }
           

            ViewBag.CurrentUserName = currentUserName;      
        
            return View(messages);
        }
        
        public async Task<IActionResult> Message()
        {
            return View(new Chat());
        }

        [HttpPost]
        public async Task<IActionResult> Message(string Message )
        {
            //, string? receiverEmail
              Chat chat = new Chat();
          //  var sender = await _userManager.FindByNameAsync(chat.SenderId);
            var currentUser = HttpContext.Session.GetString("CurrentUser");
            chat.SenderId = currentUser;
            // var receiver = HttpContext.Session.GetString("currentReceiver");
            var receiver = "user1";
            chat.ReceiverId = receiver;
            var encryptedMessage = await cipher.EncryptAsync(Message);
            chat.Message = encryptedMessage;
            chat.CurrentTime = DateTime.Now;
            await ctx.Chats.AddAsync(chat);
            await ctx.SaveChangesAsync();
            return View(chat);
        }
    }
}
