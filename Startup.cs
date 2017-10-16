using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MessageQueue.Data;
using MessageQueue.Models;
using MessageQueue.Services;
using AiursoftBase;
using AiursoftBase.Services;

namespace MessageQueue
{
    public class Startup
    {
        public static Counter MessageIdCounter { get; set; } = new Counter();
        public static Counter ListenerIdCounter { get; set; } = new Counter();
        public static Random random { get; set; } = new Random();

        public IConfiguration Configuration { get; }
        public bool IsDevelopment { get; set; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            IsDevelopment = env.IsDevelopment();
            if (IsDevelopment)
            {
                Values.Schema = "http";
            }
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConnectToAiursoftDatabase<MessageQueueDbContext>("MessageQueue",IsDevelopment);
            services.AddMvc();
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddTransient<DataCleaner>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, MessageQueueDbContext dbContext, DataCleaner cleaner)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            app.UseWebSockets();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
            cleaner.StartCleanerService().Wait();
            //dbContext.Database.Migrate();
        }
    }
}
