using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using MessageQueue.Data;

namespace MessageQueue.Migrations
{
    [DbContext(typeof(MessageQueueDbContext))]
    [Migration("20170615075645_CreateKey")]
    partial class CreateKey
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AiursoftBase.Models.MessageQueue.Channel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AppId");

                    b.Property<string>("ConnectKey");

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("Description");

                    b.HasKey("Id");

                    b.HasIndex("AppId");

                    b.ToTable("Channels");
                });

            modelBuilder.Entity("AiursoftBase.Models.MessageQueue.MessageQueueApp", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.HasKey("Id");

                    b.ToTable("Apps");
                });

            modelBuilder.Entity("AiursoftBase.Models.MessageQueue.Channel", b =>
                {
                    b.HasOne("AiursoftBase.Models.MessageQueue.MessageQueueApp", "App")
                        .WithMany("Channels")
                        .HasForeignKey("AppId");
                });
        }
    }
}
