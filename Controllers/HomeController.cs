using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Aiursoft.Pylon.Models;
using Aiursoft.Pylon.Services.ToStargateServer;
using Aiursoft.Pylon;
using MessageQueue.Services;
using Aiursoft.Pylon.Models.Stargate.ListenAddressModels;
using Aiursoft.Pylon.Attributes;
using MessageQueue.Data;
using Microsoft.EntityFrameworkCore;
using Aiursoft.Pylon.Models.Stargate;
using Aiursoft.Pylon.Services;

namespace MessageQueue.Controllers
{
    [AiurRequireHttps]
    [AiurExceptionHandler]
    public class HomeController : AiurController
    {
        private MessageQueueDbContext _dbContext;
        public HomeController(MessageQueueDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return Json(new AiurProtocal
            {
                code = ErrorType.Success,
                message = "Welcome to Aiursoft Message queue server!"
            });
        }

        public async Task<IActionResult> IntegratedTest()
        {
            string testAppId = "29bf5250a6d93d47b6164ac2821d5009";
            string testAppSecret = "784400c3d9066c5584489497273f867e";
            var token = AppsContainer.AccessToken(testAppId, testAppSecret);
            var result = await ChannelService.CreateChannelAsync(await token(), "Test Channel");
            await Task.Factory.StartNew(async () =>
            {
                await Debugger.SendDebuggingMessages(await token(), result.ChannelId);
            });
            var model = new ChannelAddressModel
            {
                Id = result.ChannelId,
                Key = result.ConnectKey
            };
            return View("Test", model);
        }

        public async Task<IActionResult> SelfTest()
        {
            string testAppId = "29bf5250a6d93d47b6164ac2821d5009";
            string testAppSecret = "784400c3d9066c5584489497273f867e";
            var token = AppsContainer.AccessToken(testAppId, testAppSecret);
            //Ensure app
            var appLocal = await _dbContext.Apps.Include(t => t.Channels).SingleOrDefaultAsync(t => t.Id == testAppId);
            if (appLocal == null)
            {
                appLocal = new StargateApp
                {
                    Id = testAppId,
                    Channels = new List<Channel>()
                };
                _dbContext.Apps.Add(appLocal);
            }
            //Create channel and save to database
            var newChannel = new Channel
            {
                Description = "Self Test Channel",
                ConnectKey = StringOperation.RandomString(20)
            };
            appLocal.Channels.Add(newChannel);
            await _dbContext.SaveChangesAsync();
            //Add messages to memory
            await Task.Factory.StartNew(async () =>
            {
                var random = new Random();
                for (int i = 0; i < 1000; i++)
                {
                    //Create Message
                    var message = new Message
                    {
                        Id = Startup.MessageIdCounter.GetUniqueNo,
                        ChannelId = newChannel.Id,
                        Content = DateTime.Now + StringOperation.RandomString(10)
                    };
                    MessageQueueMemory.Messages.Add(message);
                    await Task.Delay(10);
                }
            });
            //Prepare view
            var model = new ChannelAddressModel
            {
                Id = newChannel.Id,
                Key = newChannel.ConnectKey
            };
            return View("Test", model);
        }

        public IActionResult ListenTo(ChannelAddressModel model)
        {
            return View("Test", model);
        }
    }
}
