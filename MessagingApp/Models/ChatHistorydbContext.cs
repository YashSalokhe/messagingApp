using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MessagingApp.Models
{
    public partial class ChatHistorydbContext : DbContext
    {
        public ChatHistorydbContext()
        {
        }

        public ChatHistorydbContext(DbContextOptions<ChatHistorydbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Chat> Chats { get; set; } = null!;
        public virtual DbSet<Group> Groups { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source= YSALOKHE-LAP-05\\MSSQLSERVER01 ;Initial Catalog=ChatHistorydb;Integrated Security=SSPI");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chat>(entity =>
            {
                entity.ToTable("chat");

                entity.Property(e => e.ChatId).HasColumnName("chatId");

                entity.Property(e => e.CurrentTime)
                    .HasColumnType("datetime")
                    .HasColumnName("currentTime");

                entity.Property(e => e.GroupName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("groupName");

                entity.Property(e => e.Message)
                    .IsUnicode(false)
                    .HasColumnName("message");

                entity.Property(e => e.ReceiverId)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("receiverId");

                entity.Property(e => e.SenderId)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("senderId");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.Property(e => e.GroupName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("groupName");

                entity.Property(e => e.UserName)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("userName");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
