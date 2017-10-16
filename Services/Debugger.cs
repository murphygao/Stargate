using AiursoftBase.Models.MessageQueue;
using AiursoftBase.Services;
using AiursoftBase.Services.ToMessageQueueServer;
using MessageQueue.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageQueue.Services
{
    public static class Debugger
    {
        public static async Task SendDebuggingMessages(string AccessToken, int ChannelId)
        {
            var random = new Random();
            for (int i = 0; i < 1000; i++)
            {
                await MessageService.PushMessageAsync(AccessToken, ChannelId, DateTime.Now + StringOperation.RandomString(10));
                await Task.Delay(10);
            }
        }
    }
}
