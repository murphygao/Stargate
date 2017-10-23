using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using MessageQueue.Data;

namespace MessageQueue.Migrations
{
    [DbContext(typeof(MessageQueueDbContext))]
    [Migration("20170612140322_Message")]
    partial class Message
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Aiursoft.Pylon.Models.MessageQueue.Channel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AppId");

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("Description");

                    b.HasKey("Id");

                    b.HasIndex("AppId");

                    b.ToTable("Channels");
                });

            modelBuilder.Entity("Aiursoft.Pylon.Models.MessageQueue.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ChannelId");

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreateTime");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId");

                    b.ToTable("Message");
                });

            modelBuilder.Entity("Aiursoft.Pylon.Models.MessageQueue.MessageQueueApp", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.HasKey("Id");

                    b.ToTable("Apps");
                });

            modelBuilder.Entity("Aiursoft.Pylon.Models.MessageQueue.Channel", b =>
                {
                    b.HasOne("Aiursoft.Pylon.Models.MessageQueue.MessageQueueApp", "App")
                        .WithMany("Channels")
                        .HasForeignKey("AppId");
                });

            modelBuilder.Entity("Aiursoft.Pylon.Models.MessageQueue.Message", b =>
                {
                    b.HasOne("Aiursoft.Pylon.Models.MessageQueue.Channel", "Channel")
                        .WithMany("Messages")
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
