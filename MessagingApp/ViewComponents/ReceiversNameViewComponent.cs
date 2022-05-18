using Microsoft.AspNetCore.Mvc;

namespace MessagingApp.ViewComponents
{
    public class ReceiversNameViewComponent : ViewComponent
    {
        private readonly ChatHistorydbContext chatContext;
        public ReceiversNameViewComponent(ChatHistorydbContext chatContext)
        {
            this.chatContext = chatContext;

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentUser = HttpContext.Session.GetString("CurrentUser");
            var chats = await chatContext.Chats.ToListAsync();
            var groups = await chatContext.Groups.ToListAsync();
            var particularGroups = groups.Where(x => x.UserName == currentUser).Select(x => x.GroupName).Distinct();
            
            var messages = chats.Where(d => d.SenderId == currentUser || d.ReceiverId == currentUser);
            List<string> receivers = new List<string>();
            if (particularGroups.Count() > 0)
            {
                receivers.AddRange(particularGroups);

            }
            foreach (var message in messages)
            {
                if(message.SenderId == currentUser && !receivers.Contains(message.ReceiverId))
                {
                    receivers.Add(message.ReceiverId);
                }
                else if(message.ReceiverId == currentUser && !receivers.Contains(message.SenderId))
                {
                    receivers.Add(message.SenderId);
                }
            }
            ViewBag.CurrentReceiver = HttpContext.Session.GetString("currentReceiver");
            ViewBag.CurrentUserName = currentUser;
            return View(receivers);
        }
    }
}
