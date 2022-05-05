using Microsoft.EntityFrameworkCore;

namespace MessagingApp.Models
{
    public class MessageAppSecurityContext : IdentityDbContext<IdentityUser>
    {
        public MessageAppSecurityContext(DbContextOptions<MessageAppSecurityContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);  
        }
    }
}
