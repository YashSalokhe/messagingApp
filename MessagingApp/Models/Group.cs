using System;
using System.Collections.Generic;

namespace MessagingApp.Models
{
    public partial class Group
    {
        public int Id { get; set; }
        public string? GroupName { get; set; }
        public string? UserName { get; set; }
    }
}
