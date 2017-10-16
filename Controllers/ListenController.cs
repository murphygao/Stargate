using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AiursoftBase.Attributes;
using AiursoftBase;
using MessageQueue.Data;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using AiursoftBase.Services;
using AiursoftBase.Models.MessageQueue;
using MessageQueue.Services;
using AiursoftBase.Models;
using AiursoftBase.Models.MessageQueue.ListenAddressModels;

namespace MessageQueue.Controllers
{
    [AiurRequireHttps]
    [AiurExceptionHandler]
    public class ListenController : AiurController
    {
        private MessageQueueDbContext _dbContext;
        private IPusher<WebSocket> _pusher;

        public ListenController(MessageQueueDbContext dbContext)
        {
            _dbContext = dbContext;
            _pusher = new WebSocketPusher();
        }

        [AiurForceWebSocket]
        public async Task<IActionResult> Channel(ChannelAddressModel model)
        {
            var lastReadTime = DateTime.Now;
            var channel = await _dbContext.Channels.FindAsync(model.Id);
            if (channel.ConnectKey != model.Key)
            {
                return Json(new AiurProtocal
                {
                    code = ErrorType.Unauthorized,
                    message = "Wrong connection key!"
                });
            }
            await _pusher.Accept(HttpContext);
            int sleepTime = 0;
            while (_pusher.Connected)
            {
                try
                {
                    var nextMessages = MessageQueueMemory
                        .Messages
                        .Where(t => t.ChannelId == model.Id)
                        .Where(t => t.CreateTime > lastReadTime)
                        .ToList();
                    Console.WriteLine(DateTime.Now.Millisecond + "Checked!");
                    if (!nextMessages.Any())
                    {
                        if (sleepTime < 300)
                            sleepTime += 5;
                        await Task.Delay(sleepTime);
                    }
                    else
                    {
                        var nextMessage = nextMessages.OrderBy(t => t.CreateTime).First();
                        await _pusher.SendMessage(nextMessage.Content);
                        lastReadTime = nextMessage.CreateTime;
                        sleepTime = 0;
                    }
                }
                catch (InvalidOperationException)
                {

                }
            }
            return null;
        }
    }
}