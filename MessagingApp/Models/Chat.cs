using System;
using System.Collections.Generic;

namespace MessagingApp.Models
{
    public partial class Chat
    {
        public int ChatId { get; set; }
        public string SenderId { get; set; } = null!;
        public string Message { get; set; } = null!;
        public DateTime CurrentTime { get; set; }
        public string ReceiverId { get; set; } = null!;
    }
}
