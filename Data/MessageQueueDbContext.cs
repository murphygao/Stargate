using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MessageQueue.Models;
using AiursoftBase.Models.MessageQueue;

namespace MessageQueue.Data
{
    public class MessageQueueDbContext : DbContext
    {
        public MessageQueueDbContext(DbContextOptions<MessageQueueDbContext> options) : base(options)
        {
        }

        public DbSet<Channel> Channels { get; set; }
        public DbSet<MessageQueueApp> Apps { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
